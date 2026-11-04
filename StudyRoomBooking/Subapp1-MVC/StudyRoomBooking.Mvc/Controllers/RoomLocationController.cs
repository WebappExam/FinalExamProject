using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.viewModel;
using StudyRoomBooking.Mvc.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Linq.Expressions;

namespace StudyRoomBooking.Mvc.Controllers;

public class RoomLocationController : Controller
{
    private readonly ILocationRepository _locationRepository;
    private readonly IRoomRepository _roomRepository;

    private readonly ILogger<RoomLocationController> _logger;

//Tar inn repository som skal brukes til å hente lokasjoner og rom fra databasen
    public RoomLocationController(
        ILocationRepository locationRepository,
        IRoomRepository roomRepository,
        ILogger<RoomLocationController> logger)
    {
        _locationRepository = locationRepository;
        _roomRepository = roomRepository;
        _logger = logger;
    }

//Åpner skjema for å opprette et nytt rom på valgt 
 [HttpGet]
    public async Task<IActionResult> Create(int locationId)
    {
        var location = await _locationRepository.GetWithRoomsAsync(locationId);
        if (location == null)
        {
            return NotFound("Lokasjonen finnes ikke.");
        }

        var viewModel = new RoomCreateViewModel
        {
            LocationId = locationId
        };

        return View(viewModel);
    } 

    //Kontrollermetode som tar imot data fra skjemaet, og lagrer rom på valgt lokasjon 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoomCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var location = await _locationRepository.GetWithRoomsAsync(viewModel.LocationId);
        if (location == null)
        {
            ModelState.AddModelError(
                nameof(viewModel.LocationId), 
                "Lokasjonen finnes ikke.");
            return View(viewModel);
        }

        var room = new Room
        {
            RoomName = viewModel.RoomName,
            Capacity = viewModel.Capacity,
            Building = viewModel.Building,
            LocationId = viewModel.LocationId
        };

        try 
         {
        await _roomRepository.Create(room);

        //Logger at rom er lagret
        _logger.LogInformation(
            "Oprettet rom '{RoomId}' er lagret på lokasjon '{LocationId}'", 
            room.RoomId, location.LocationId);

        return RedirectToAction("Rooms", new { locationId = viewModel.LocationId });
    }
    catch (Exception ex)

    { //Logger feilmelding hvis lagring av rom feiler med melding i skjema
        _logger.LogError(ex, 
        "Feil ved lagring av rom '{RoomId}' på lokasjon '{LocationId}'", 
            room.RoomId, location.LocationId);

        ModelState.AddModelError(string.Empty, "En feil oppstod ved lagring av rommet. Vennligst prøv igjen.");
        return View(viewModel);
    }
 }

 [HttpGet]
    public async Task<IActionResult> Update(int roomId)
    {
        var room = await _roomRepository.GetRoomById(roomId);

        if (room == null)
        {
            return NotFound("Rommet finnes ikke.");
        }

    var viewModel = new RoomUpdateViewModel
        {
            RoomId= room.RoomId,
            LocationId = room.LocationId,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            Building = room.Building
        };
        return View(viewModel);
    }

[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(RoomUpdateViewModel viewModel)
    {
        try
        {
            var room = await _roomRepository.GetRoomById(viewModel.RoomId);
            if (room == null)
            {
                return NotFound("Rommet finnes ikke.");
            }
            viewModel.LocationId = room.LocationId; // Beholder lokasjonId for redirect
            ModelState.Remove(nameof(viewModel.LocationId)); //Fjerner validering for LocationId, da den ikke skal endres
            
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            room.RoomName = viewModel.RoomName;
            room.Capacity = viewModel.Capacity;
            room.Building = viewModel.Building;

            await _roomRepository.Update(room);
            _logger.LogInformation(
                "Rom '{RoomId}' er oppdatert på lokasjon '{LocationId}'", 
                room.RoomId, room.LocationId);

                return RedirectToAction("Rooms", new { locationId = room.LocationId });
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Feil ved oppdatering av rom '{RoomId}' på lokasjon '{LocationId}'", 
                viewModel.RoomId, viewModel.LocationId);

            ModelState.AddModelError(string.Empty, "En feil oppstod ved oppdatering av rommet. Vennligst prøv igjen.");
            return View(viewModel);
        }
    }

    // Viser lokasjonene på startsiden.
    public async Task<IActionResult> Index()
    {
        var locations = await _locationRepository.GetAllAsync();

        var viewModels = locations.Select(location => new LocationViewModel
        {
            LocationId = location.LocationId,
            LocationName = location.Name
        }).ToList();

        return View(viewModels);
    }

    // Viser rommene på lokasjonen brukeren valgte + søketekst som skal vises i søkefeltet
    public async Task<IActionResult> Rooms(int locationId, string ? searchQuery,string ? building, int? minCapacity )
    {
        var location = await _locationRepository
            .GetWithRoomsAsync(locationId);

        if (location == null)
        {
            return NotFound("Lokasjonen finnes ikke.");
        }

    //Filtrerer rommene basert på lokasjon,etter navn uavhening av store og små bokstaver, og fjerner eventuelle mellomrom i søketeksten
    var rooms = location.Rooms.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(searchQuery))
    {
        rooms = rooms.Where(room => 
        room.RoomName.ToLower().Contains(searchQuery.Trim().ToLower()));
    }

    //Filtrerer videre på bygg hvis bruker har skrevet inn i søkefeltet
if (!string.IsNullOrWhiteSpace(building))

    {
        rooms = rooms.Where(room => 
        room.Building.ToLower().Contains(building.Trim().ToLower()));
    }

    //Filtrerer videre på min kapasitet hvis bruker har skrevet inn i søkefeltet
if (minCapacity.HasValue)
    {
        rooms = rooms.Where(room => room.Capacity >= minCapacity.Value);
    }
    //Sender søketekst og filtrert romliste til viewmodel som skal vises i viewet
        var viewModel = new LocationViewModel
        {
            LocationId = location.LocationId,
            LocationName = location.Name,
            SearchQuery = searchQuery,
            Building = building,
            MinCapacity = minCapacity,
            Rooms = rooms
                .OrderBy(room => room.RoomName)
                .ToList()
        };

        return View(viewModel);
    }
}