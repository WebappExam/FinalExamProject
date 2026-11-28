
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Models;
using StudyRoomBooking.Mvc.viewModel;
using System.Security.Claims;

namespace StudyRoomBooking.Mvc.Controllers;

[Authorize]
public class RoomBookingController : Controller
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomBookingRepository _roomBookingRepository;
    private readonly ILogger<RoomBookingController> _logger;

    public RoomBookingController(
        IRoomRepository roomRepository,
        IRoomBookingRepository roomBookingRepository,
        ILogger<RoomBookingController> logger)
    {
        _roomRepository = roomRepository;
        _roomBookingRepository = roomBookingRepository;
        _logger = logger;
    }

    // GET: RoomBooking/Create
    [HttpGet]
    public async Task<IActionResult> Create(int roomId)
    {
        if (roomId <= 0)
            return BadRequest("Please select a room before booking.");

        var room = await _roomRepository.GetRoomById(roomId);

        if (room == null)
            return NotFound("The selected room does not exist.");

        var now = DateTime.Now.AddMinutes(15);

        var startTime = new DateTime(
            now.Year, now.Month, now.Day,
            now.Hour, now.Minute, 0);

        var viewModel = new RoomBookingCreateViewModel
        {
            RoomId = roomId,
            RoomName = room.RoomName,
            StartTime = startTime,
            EndTime = startTime.AddHours(1)
        };

        return View(viewModel);
    }

    // POST: RoomBooking/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        RoomBookingCreateViewModel viewModel)
    {
        var room = await _roomRepository.GetRoomById(viewModel.RoomId);

        if (room == null)
            return NotFound();

        viewModel.RoomName = room.RoomName;

        if (viewModel.StartTime.HasValue &&
            viewModel.StartTime.Value < DateTime.Now)
        {
            ModelState.AddModelError(
                nameof(viewModel.StartTime),
                "Starttidspunkt kan ikke være i fortiden.");
        }

        if (viewModel.StartTime.HasValue &&
            viewModel.EndTime.HasValue &&
            viewModel.StartTime.Value >= viewModel.EndTime.Value)
        {
            ModelState.AddModelError(
                nameof(viewModel.StartTime),
                "Starttidspunkt må være før sluttidspunkt.");
        }

        if (!room.Capacity.HasValue ||
            viewModel.NumberOfParticipants > room.Capacity.Value)
        {
            ModelState.AddModelError(
                nameof(viewModel.NumberOfParticipants),
                "Invalid room capacity or too many participants.");
        }

        if (!ModelState.IsValid ||
            !viewModel.StartTime.HasValue ||
            !viewModel.EndTime.HasValue)
        {
            return View(viewModel);
        }

        bool hasOverlap =
            await _roomBookingRepository.HasOverlappingBookingsAsync(
                viewModel.RoomId,
                viewModel.StartTime.Value,
                viewModel.EndTime.Value);

        if (hasOverlap)
        {
            _logger.LogWarning(
                "Booking conflict detected for room {RoomId}.",
                viewModel.RoomId);

            ModelState.AddModelError(
                string.Empty,
                "There is already a booking for this room during the selected time period.");

            return View(viewModel);
        }

        var organizerId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(organizerId))
            return Unauthorized();

        var booking = new RoomBooking
        {
            RoomId = viewModel.RoomId,
            StartTime = viewModel.StartTime.Value,
            EndTime = viewModel.EndTime.Value,
            Course = viewModel.Course,
            Description = viewModel.Description ?? string.Empty,
            OrganizerId = organizerId,
            NumberOfParticipants = viewModel.NumberOfParticipants
        };

        try
        {
            await _roomBookingRepository.Create(booking);

            _logger.LogInformation(
                "Booking {BookingId} created for room {RoomId}.",
                booking.Id, booking.RoomId);

            return RedirectToAction("Index", "RoomLocation");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error creating booking for room {RoomId}.",
                booking.RoomId);

            ModelState.AddModelError(
                string.Empty,
                "Bookingen kunne ikke lagres. Vennligst prøv igjen.");

            return View(viewModel);
        }
    }

    // GET: RoomBooking/MyBookings
    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var organizerId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(organizerId))
            return Unauthorized();

        var bookings =
            await _roomBookingRepository.GetByOrganizerIdAsync(organizerId);

        var viewModels = bookings.Select(booking =>
            new RoomBookingListViewModel
            {
                BookingId = booking.Id,
                RoomName = booking.Room.RoomName,
                Building = booking.Room.Building,
                Course = booking.Course,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                NumberOfParticipants = booking.NumberOfParticipants
            }).ToList();

        return View(viewModels);
    }

    // GET: RoomBooking/Index
    // Only administrators can see all bookings.
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var bookings = await _roomBookingRepository.GetAllAsync();

            var viewModels = new List<RoomBookingDetailsViewModel>();

            foreach (var booking in bookings)
            {
                var room =
                    await _roomRepository.GetRoomById(booking.RoomId);

                viewModels.Add(new RoomBookingDetailsViewModel
                {
                    Id = booking.Id,
                    RoomId = booking.RoomId,
                    RoomName = room?.RoomName ?? "Unknown room",
                    Course = booking.Course,
                    Description = booking.Description,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    NumberOfParticipants = booking.NumberOfParticipants
                });
            }

            return View(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve bookings.");

            Response.StatusCode = 500;

            return View("~/Views/Shared/Error.cshtml");
        }
    }

    // POST: RoomBooking/Cancel
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int bookingId)
    {
        var organizerId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(organizerId))
            return Unauthorized();

        var booking =
            await _roomBookingRepository.GetRoomBookingAsync(bookingId);

        if (booking == null)
            return NotFound();

        // Owner or administrator can cancel.
        if (booking.OrganizerId != organizerId &&
            !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        try
        {
            await _roomBookingRepository.DeleteAsync(booking);

            _logger.LogInformation(
                "Booking {BookingId} cancelled for room {RoomId}.",
                booking.Id, booking.RoomId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error cancelling booking {BookingId}.",
                booking.Id);

            TempData["ErrorMessage"] =
                "Bookingen kunne ikke kanselleres. Vennligst prøv igjen.";
        }

        return RedirectToAction(nameof(MyBookings));
    }

    // GET: RoomBooking/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var booking =
            await _roomBookingRepository.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (booking.OrganizerId != userId &&
            !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var room =
            await _roomRepository.GetRoomById(booking.RoomId);

        var viewModel = new RoomBookingDetailsViewModel
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = room?.RoomName ?? "Unknown room",
            Course = booking.Course,
            Description = booking.Description,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            NumberOfParticipants = booking.NumberOfParticipants
        };

        return View(viewModel);
    }

    // GET: RoomBooking/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var booking =
            await _roomBookingRepository.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (booking.OrganizerId != userId &&
            !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var room =
            await _roomRepository.GetRoomById(booking.RoomId);

        var viewModel = new RoomBookingEditViewModel
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = room?.RoomName ?? "Unknown room",
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Course = booking.Course,
            Description = booking.Description,
            NumberOfParticipants = booking.NumberOfParticipants
        };

        return View(viewModel);
    }

    // POST: RoomBooking/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        RoomBookingEditViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        var existingBooking =
            await _roomBookingRepository.GetByIdAsync(id);

        if (existingBooking == null)
            return NotFound();

        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Important: Check authorization again on POST.
        if (existingBooking.OrganizerId != userId &&
            !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var room =
            await _roomRepository.GetRoomById(existingBooking.RoomId);

        if (room == null)
            return NotFound();

        viewModel.RoomId = existingBooking.RoomId;
        viewModel.RoomName = room.RoomName;

        if (!room.Capacity.HasValue ||
            viewModel.NumberOfParticipants > room.Capacity.Value)
        {
            ModelState.AddModelError(
                nameof(viewModel.NumberOfParticipants),
                "Invalid room capacity or too many participants.");
        }

        if (viewModel.StartTime.HasValue &&
            viewModel.StartTime.Value < DateTime.Now)
        {
            ModelState.AddModelError(
                nameof(viewModel.StartTime),
                "Starttidspunkt kan ikke være i fortiden.");
        }

        if (viewModel.StartTime.HasValue &&
            viewModel.EndTime.HasValue &&
            viewModel.StartTime.Value >= viewModel.EndTime.Value)
        {
            ModelState.AddModelError(
                nameof(viewModel.EndTime),
                "Sluttidspunkt må være etter starttidspunkt.");
        }

        if (!ModelState.IsValid ||
            !viewModel.StartTime.HasValue ||
            !viewModel.EndTime.HasValue)
        {
            return View(viewModel);
        }

        bool hasOverlap =
            await _roomBookingRepository.HasOverlappingBookingsAsync(
                existingBooking.RoomId,
                viewModel.StartTime.Value,
                viewModel.EndTime.Value,
                existingBooking.Id);

        if (hasOverlap)
        {
            ModelState.AddModelError(
                string.Empty,
                "Rommet er allerede booket i denne tidsperioden.");

            return View(viewModel);
        }

        existingBooking.StartTime = viewModel.StartTime.Value;
        existingBooking.EndTime = viewModel.EndTime.Value;
        existingBooking.Course = viewModel.Course;
        existingBooking.Description =
            viewModel.Description ?? string.Empty;
        existingBooking.NumberOfParticipants =
            viewModel.NumberOfParticipants;

        try
        {
            await _roomBookingRepository.UpdateAsync(existingBooking);

            _logger.LogInformation(
                "Booking {BookingId} updated for room {RoomId}.",
                existingBooking.Id, existingBooking.RoomId);

            TempData["SuccessMessage"] =
                "Bookingen ble oppdatert.";

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating booking {BookingId}.",
                existingBooking.Id);

            ModelState.AddModelError(
                string.Empty,
                "Bookingen kunne ikke oppdateres. Vennligst prøv igjen.");

            return View(viewModel);
        }
    }

    // GET: RoomBooking/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest();

        var booking =
            await _roomBookingRepository.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        var room =
            await _roomRepository.GetRoomById(booking.RoomId);

        var viewModel = new RoomBookingDetailsViewModel
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = room?.RoomName ?? "Unknown room",
            Course = booking.Course,
            Description = booking.Description,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            NumberOfParticipants = booking.NumberOfParticipants
        };

        return View(viewModel);
    }

    // POST: RoomBooking/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            var deleted =
                await _roomBookingRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            _logger.LogInformation(
                "Booking {BookingId} deleted.",
                id);

            TempData["SuccessMessage"] =
                "Bookingen ble slettet.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error deleting booking {BookingId}.",
                id);

            TempData["ErrorMessage"] =
                "Bookingen kunne ikke slettes. Vennligst prøv igjen.";
        }

        return RedirectToAction(nameof(Index));
    }
}
