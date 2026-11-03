using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.viewModel;

namespace StudyRoomBooking.Mvc.Controllers;

public class RoomLocationController : Controller
{
    private readonly ILocationRepository _locationRepository;

    public RoomLocationController(
        ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
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