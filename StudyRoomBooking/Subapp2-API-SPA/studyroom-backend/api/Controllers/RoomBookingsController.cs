using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.DTOs;
using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/bookings")]
public class RoomBookingsController(
    IRoomBookingRepository bookingRepository,
    IRoomRepository roomRepository,
    ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RoomBookingDto>>> GetBookings()
    {
        var bookings = await bookingRepository.GetAllAsync();
        return Ok(bookings.Select(ToDto).ToList());
    }

    [HttpGet("{bookingId:int}")]
    public async Task<ActionResult<RoomBookingDto>> GetBooking(int bookingId)
    {
        var booking = await bookingRepository.GetByIdAsync(bookingId);
        if (booking is null)
        {
            return NotFound(new { message = "Booking not found." });
        }

        return Ok(ToDto(booking));
    }

    [HttpPost]
    public async Task<ActionResult<RoomBookingDto>> CreateBooking(
        CreateBookingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OrganizerId) ||
            !await db.Users.AnyAsync(user => user.Id == request.OrganizerId))
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]>
            {
                [nameof(request.OrganizerId)] =
                    ["A valid OrganizerId is required."]
            })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            });
        }

        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room is null)
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]>
            {
                [nameof(request.RoomId)] = ["The specified room does not exist."]
            })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            });
        }

        if (await bookingRepository.HasOverlappingBookingsAsync(
                request.RoomId,
                request.StartTime!.Value,
                request.EndTime!.Value))
        {
            return Conflict(new ProblemDetails
            {
                Title = "Booking conflict",
                Detail = "The room is already booked during this time.",
                Status = StatusCodes.Status409Conflict
            });
        }

        var booking = new RoomBooking
        {
            StartTime = request.StartTime.Value,
            EndTime = request.EndTime.Value,
            Course = request.Course,
            Description = request.Description ?? string.Empty,
            RoomId = request.RoomId,
            OrganizerId = request.OrganizerId
        };

        await bookingRepository.Create(booking);

        return CreatedAtAction(
            nameof(GetBooking),
            new { bookingId = booking.Id },
            ToDto(booking));
    }

    [HttpPut("{bookingId:int}")]
    public async Task<IActionResult> UpdateBooking(
        int bookingId,
        CreateBookingRequest request)
    {
        var booking = await bookingRepository.GetByIdAsync(bookingId);
        if (booking is null)
        {
            return NotFound(new { message = "Booking not found." });
        }

        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room is null)
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]>
            {
                [nameof(request.RoomId)] = ["The specified room does not exist."]
            })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            });
        }

        if (await bookingRepository.HasOverlappingBookingsAsync(
                request.RoomId,
                request.StartTime!.Value,
                request.EndTime!.Value,
                bookingId))
        {
            return Conflict(new ProblemDetails
            {
                Title = "Booking conflict",
                Detail = "The room is already booked during this time.",
                Status = StatusCodes.Status409Conflict
            });
        }

        booking.StartTime = request.StartTime.Value;
        booking.EndTime = request.EndTime.Value;
        booking.Course = request.Course;
        booking.Description = request.Description ?? string.Empty;
        booking.RoomId = request.RoomId;

        await bookingRepository.UpdateAsync(booking);

        return NoContent();
    }

    [HttpDelete("{bookingId:int}")]
    public async Task<IActionResult> DeleteBooking(int bookingId)
    {
        var deleted = await bookingRepository.DeleteAsync(bookingId);
        return deleted
            ? NoContent()
            : NotFound(new { message = "Booking not found." });
    }

    private static RoomBookingDto ToDto(RoomBooking booking) => new()
    {
        Id = booking.Id,
        StartTime = booking.StartTime,
        EndTime = booking.EndTime,
        Course = booking.Course,
        Description = booking.Description,
        RoomId = booking.RoomId
    };
}
