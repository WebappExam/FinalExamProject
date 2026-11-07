using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.viewModel;

namespace StudyRoomBooking.Mvc.Controllers;

public class RoomBookingController : Controller
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomBookingRepository _roomBookingRepository;

    //Gir controller tilgang til rom og booking
    public RoomBookingController(
        IRoomRepository roomRepository, 
        IRoomBookingRepository roomBookingRepository)
    {
        _roomRepository = roomRepository;
        _roomBookingRepository = roomBookingRepository;
    }

    //Åpner bookingsiden for et gitt rom
    [HttpGet]
    public async Task<IActionResult> Create(int roomId)
    {
        var room = await _roomRepository.GetRoomById(roomId);
        if (room == null)
        {
            return NotFound();
        }

    //Henter dagens dato og klokkeslett uten sekunder/millisekunder
        var now = DateTime.Now;
        var startTime = new DateTime (
        now.Year,now.Month,now.Day,
        now.Hour,now.Minute,0
        );

    //Sender rominfo til skjema
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
        await _roomBookingRepository.Create(booking);
         return RedirectToAction ("Index","RoomLocation");
        }
    }
