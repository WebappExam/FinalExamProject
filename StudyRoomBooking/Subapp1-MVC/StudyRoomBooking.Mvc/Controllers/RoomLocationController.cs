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

    // Viser rommene på lokasjonen brukeren valgte.
    public async Task<IActionResult> Rooms(int locationId)
    {
        var location = await _locationRepository
            .GetWithRoomsAsync(locationId);

        if (location == null)
        {
            return NotFound("Lokasjonen finnes ikke.");
        }

        var viewModel = new LocationViewModel
        {
            LocationId = location.LocationId,
            LocationName = location.Name,
            Rooms = location.Rooms
                .OrderBy(room => room.RoomName)
                .ToList()
        };

        return View(viewModel);
    }
}