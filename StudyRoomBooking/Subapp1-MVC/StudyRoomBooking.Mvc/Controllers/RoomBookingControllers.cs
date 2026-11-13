using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Models;
using StudyRoomBooking.Mvc.viewModel;

namespace StudyRoomBooking.Mvc.Controllers;

public class RoomBookingController : Controller
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomBookingRepository _roomBookingRepository;
    // Brukes til å logge bookinghendelser og feil på serveren.
    private readonly ILogger<RoomBookingController> _logger;

    //Gir controller tilgang til rom og booking
    public RoomBookingController(
       IRoomRepository roomRepository,
        IRoomBookingRepository roomBookingRepository,
        ILogger<RoomBookingController> logger)
    {
        _roomRepository = roomRepository;
        _roomBookingRepository = roomBookingRepository;
        _logger =logger;
    }

    //Åpner bookingsiden for et gitt rom

[HttpGet]
    public async Task<IActionResult> Create(int roomId)
{
    // Validate room ID
    if (roomId <= 0)
    {
        return BadRequest("Please select a room before booking.");
    }

    // Retrieve room from database
    var room = await _roomRepository.GetRoomById(roomId);

    // Check if room exists
    if (room == null)
    {
        return NotFound("The selected room does not exist.");
    }

    // Get current date and time without seconds
    var now = DateTime.Now;

    var startTime = new DateTime(
        now.Year,
        now.Month,
        now.Day,
        now.Hour,
        now.Minute,
        0
    );

    // Prepare booking form
    var viewModel = new RoomBookingCreateViewModel
    {
        RoomId = roomId,
        RoomName = room.RoomName,
        StartTime = startTime,
        EndTime = startTime.AddHours(1)
    };

    return View(viewModel);
}

 //POST metode
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoomBookingCreateViewModel viewModel)
    {

    var room = await _roomRepository.GetRoomById(viewModel.RoomId);

    if (room == null)
        {
            return NotFound();
        }

        viewModel.RoomName = room.RoomName;

            //Kontrollerer at starttid ikke er i fortid
            if (viewModel.StartTime.HasValue && 
            viewModel.StartTime.Value < DateTime.Now)
            {
                 ModelState.AddModelError(
                nameof (viewModel.StartTime),
                "Starttidspunkt kan ikke være i fortiden");
        }
            if (viewModel.StartTime.HasValue &&
            viewModel.EndTime.HasValue && 
            viewModel.StartTime.Value >= viewModel.EndTime.Value)
        {
            
        ModelState.AddModelError(
        nameof(viewModel.StartTime),
        "Starttidspunkt må være før sluttidspunkt");
        } //Avslutter kontrollen av start og sluttid

        if (!ModelState.IsValid || 
        !viewModel.StartTime.HasValue || 
        !viewModel.EndTime.HasValue)
            {
                return View(viewModel);
            }

        // Sjekk for overlappende bookinger
        bool hasOverlap = await _roomBookingRepository.HasOverlappingBookingsAsync(
            viewModel.RoomId, 
            viewModel.StartTime.Value, 
            viewModel.EndTime.Value);

        if (hasOverlap)
        {
            ModelState.AddModelError(string.Empty, "Det finnes allerede en booking for dette rommet i den valgte tidsperioden.");
            return View(viewModel);
        }

        //Henter Id til inlogget bruker
        var organizerId = User.FindFirst (
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(organizerId))
        { 
            ModelState.AddModelError(
            string.Empty,
            "Du må være innlogget for å booke et rom");
            
            return View(viewModel);
        }

        var booking = new Models.RoomBooking
        {
            RoomId = viewModel.RoomId,
            StartTime = viewModel.StartTime.Value,
            EndTime = viewModel.EndTime.Value,
            Course = viewModel.Course,
            Description = viewModel.Description ?? string.Empty,
            OrganizerId = organizerId
        };

        // Lagring aktiveres når brukerkobling er klar
        try
    {
         await _roomBookingRepository.Create(booking);

        //Logger først når bookingen er lagret.
        _logger.LogInformation(
        "Booking {BookingId} ble opprettet for rom {RoomId}.",
        booking.Id, booking.RoomId);

    return RedirectToAction("Index", "RoomLocation");
}
catch (Exception ex)
{
    // Beholder feildetaljene i loggen og viser en forståelig melding.
    _logger.LogError(
        ex,
        "Feil ved oppretting av booking for rom {RoomId}.",
        booking.RoomId);

    ModelState.AddModelError(
        string.Empty,
        "Bookingen kunne ikke lagres. Vennligst prøv igjen.");

    return View(viewModel);
}
    }
        //Viser bookinger til innlogget bruker
        [HttpGet]
        public async Task<IActionResult> MyBookings()

        {
            var organizerId = User.FindFirst (System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            //Henter ingen bookinger hvis bruker ikke er innlogget
            if (User.Identity?.IsAuthenticated != true ||
            string.IsNullOrEmpty(organizerId))
        {
            return Unauthorized("Du må være innlogget for å se dine bookinger");
        }

        //Henter booking til innlogget bruker
        var bookings = await _roomBookingRepository
        .GetByOrganizerIdAsync(organizerId);

        //Velger opplysninger osm skal vises på siden

        var viewModels = bookings.Select(booking =>
        new RoomBookingListViewModel
    {
        BookingId= booking.Id,
        RoomName = booking.Room.RoomName,
        Building = booking.Room.Building, 
        Course = booking.Course, 
        StartTime = booking.StartTime,
        EndTime = booking.EndTime
    }).ToList();

        return View(viewModels);

        }
    
        // GET: RoomBooking/Index
        [HttpGet]
        public async Task<IActionResult> Index()
{
    var bookings = await _roomBookingRepository.GetAllAsync();

    var viewModels = new List<RoomBookingDetailsViewModel>();

    foreach (var booking in bookings)
    {
        var room = await _roomRepository.GetRoomById(booking.RoomId);

        viewModels.Add(new RoomBookingDetailsViewModel
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = room?.RoomName ?? "Unknown room",
            Course = booking.Course,
            Description = booking.Description,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime
        });
    }
    return View(viewModels);
}

    //Kansellere en booking
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int bookingId)
    {
        
    //Hent Id til innlogget bruker
    var organizerId = User.FindFirst(
        System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (User.Identity?.IsAuthenticated != true ||
        string.IsNullOrEmpty(organizerId))
        {
            return Unauthorized();
        }

        var booking = await _roomBookingRepository.GetRoomBookingAsync(bookingId);

    if (booking == null)
            {
                return NotFound();
            }
            //Bruker kan bare kansallere egne bookinger
            if (booking.OrganizerId != organizerId)
            {
                return Forbid();
            }
            try
        {
    await _roomBookingRepository.DeleteAsync(booking);

    // Logger først når bookingen er slettet.
    _logger.LogInformation(
        "Booking {BookingId} ble kansellert for rom {RoomId}.",
        booking.Id, booking.RoomId);
}
catch (Exception ex)
{
    // Logger feilen og sender en melding til Mine bookinger.
    _logger.LogError(
        ex,
        "Feil ved kansellering av booking {BookingId}.",
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
    var booking = await _roomBookingRepository.GetByIdAsync(id);

    if (booking == null)
    {
        return NotFound();
    }

    var room = await _roomRepository.GetRoomById(booking.RoomId);

    var viewModel = new RoomBookingDetailsViewModel
    {
        Id = booking.Id,
        RoomId = booking.RoomId,
        RoomName = room?.RoomName ?? "Unknown room",
        Course = booking.Course,
        Description = booking.Description,
        StartTime = booking.StartTime,
        EndTime = booking.EndTime
    };

    return View(viewModel);
}

    // GET: RoomBooking/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var booking = await _roomBookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        var room = await _roomRepository.GetRoomById(booking.RoomId);

        var viewModel = new RoomBookingEditViewModel
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = room?.RoomName ?? "Unknown room",
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Course = booking.Course,
            Description = booking.Description
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
        {
            return BadRequest();
        }

        var existingBooking =
            await _roomBookingRepository.GetByIdAsync(id);

        if (existingBooking == null)
        {
            return NotFound();
        }

        var room =
            await _roomRepository.GetRoomById(existingBooking.RoomId);

        viewModel.RoomId = existingBooking.RoomId;
        viewModel.RoomName = room?.RoomName ?? "Unknown room";

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

        // Update only the permitted booking fields.
        // Preserve OrganizerId, RoomId and Participants.
        existingBooking.StartTime = viewModel.StartTime.Value;
        existingBooking.EndTime = viewModel.EndTime.Value;
        existingBooking.Course = viewModel.Course;
        existingBooking.Description =
            viewModel.Description ?? string.Empty;

   try
{
    await _roomBookingRepository.UpdateAsync(existingBooking);

    // Logger først når endringene er lagret.
    _logger.LogInformation(
        "Booking {BookingId} ble oppdatert for rom {RoomId}.",
        existingBooking.Id, existingBooking.RoomId);

    TempData["SuccessMessage"] = "Bookingen ble oppdatert.";

    return RedirectToAction(nameof(Details), new { id });
}
catch (Exception ex)
{
    // Logger feildetaljene og lar brukeren beholde skjemaet.
    _logger.LogError(
        ex,
        "Feil ved oppdatering av booking {BookingId}.",
        existingBooking.Id);

    ModelState.AddModelError(
        string.Empty,
        "Bookingen kunne ikke oppdateres. Vennligst prøv igjen.");

    return View(viewModel);
}
    }

    // GET: RoomBooking/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var booking = await _roomBookingRepository.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        var room = await _roomRepository.GetRoomById(booking.RoomId);

        var viewModel = new RoomBookingDetailsViewModel
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            RoomName = room?.RoomName ?? "Unknown room",
            Course = booking.Course,
            Description = booking.Description,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime
        };

        return View(viewModel);
    }

    // POST: RoomBooking/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

       try
{
    var deleted = await _roomBookingRepository.DeleteAsync(id);

    if (!deleted)
    {
        return NotFound();
    }

    // Logger først når bookingen er slettet.
    _logger.LogInformation(
        "Booking {BookingId} ble slettet.",
        id);

    TempData["SuccessMessage"] = "Bookingen ble slettet.";
}
catch (Exception ex)
{
    // Logger feildetaljene og viser en melding i bookingoversikten.
    _logger.LogError(
        ex,
        "Feil ved sletting av booking {BookingId}.",
        id);

    TempData["ErrorMessage"] =
        "Bookingen kunne ikke slettes. Vennligst prøv igjen.";
}

return RedirectToAction(nameof(Index));
         } 
    }
