
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.viewModel;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Controllers;

public class RoomLocationController : Controller
{
    private readonly ILocationRepository _locationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomBookingRepository _roomBookingRepository;
    private readonly ILogger<RoomLocationController> _logger;

    public RoomLocationController(
        ILocationRepository locationRepository,
        IRoomRepository roomRepository,
        IRoomBookingRepository roomBookingRepository,
        ILogger<RoomLocationController> logger)
    {
        _locationRepository = locationRepository;
        _roomRepository = roomRepository;
        _roomBookingRepository = roomBookingRepository;
        _logger = logger;
    }

    // GET: RoomLocation/Create
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Create(int locationId)
    {
        var location =
            await _locationRepository.GetWithRoomsAsync(locationId);

        if (location == null)
            return NotFound("Lokasjonen finnes ikke.");

        var viewModel = new RoomCreateViewModel
        {
            LocationId = locationId
        };

        return View(viewModel);
    }

    // POST: RoomLocation/Create
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        RoomCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var location =
            await _locationRepository.GetWithRoomsAsync(
                viewModel.LocationId);

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
            RoomType = viewModel.RoomType,
            LocationId = viewModel.LocationId
        };

        try
        {
            await _roomRepository.Create(room);

            _logger.LogInformation(
                "Room {RoomId} created at location {LocationId}.",
                room.RoomId, location.LocationId);

            return RedirectToAction(
                nameof(Rooms),
                new { locationId = viewModel.LocationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error creating room at location {LocationId}.",
                location.LocationId);

            ModelState.AddModelError(
                string.Empty,
                "En feil oppstod ved lagring av rommet. Vennligst prøv igjen.");

            return View(viewModel);
        }
    }

    // GET: RoomLocation/Update
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Update(int roomId)
    {
        var room =
            await _roomRepository.GetRoomById(roomId);

        if (room == null)
            return NotFound("Rommet finnes ikke.");

        var viewModel = new RoomUpdateViewModel
        {
            RoomId = room.RoomId,
            LocationId = room.LocationId,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            Building = room.Building,
            RoomType = room.RoomType
        };

        return View(viewModel);
    }

    // POST: RoomLocation/Update
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        RoomUpdateViewModel viewModel)
    {
        try
        {
            var room =
                await _roomRepository.GetRoomById(viewModel.RoomId);

            if (room == null)
                return NotFound("Rommet finnes ikke.");

            viewModel.LocationId = room.LocationId;

            ModelState.Remove(nameof(viewModel.LocationId));

            if (!ModelState.IsValid)
                return View(viewModel);

            room.RoomName = viewModel.RoomName;
            room.Capacity = viewModel.Capacity;
            room.Building = viewModel.Building;
            room.RoomType = viewModel.RoomType;

            await _roomRepository.Update(room);

            _logger.LogInformation(
                "Room {RoomId} updated at location {LocationId}.",
                room.RoomId, room.LocationId);

            return RedirectToAction(
                nameof(Rooms),
                new { locationId = room.LocationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating room {RoomId}.",
                viewModel.RoomId);

            ModelState.AddModelError(
                string.Empty,
                "En feil oppstod ved oppdatering av rommet. Vennligst prøv igjen.");

            return View(viewModel);
        }
    }

    // GET: RoomLocation/Delete
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int roomId)
    {
        var room =
            await _roomRepository.GetRoomById(roomId);

        if (room == null)
            return NotFound("Rommet finnes ikke.");

        var viewModel = new RoomDeleteViewModel
        {
            RoomId = room.RoomId,
            LocationId = room.LocationId,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            Building = room.Building
        };

        return View(viewModel);
    }

    // POST: RoomLocation/Delete
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int roomId)
    {
        var room =
            await _roomRepository.GetRoomById(roomId);

        if (room == null)
            return NotFound("Rommet finnes ikke.");

        var viewModel = new RoomDeleteViewModel
        {
            RoomId = room.RoomId,
            LocationId = room.LocationId,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            Building = room.Building
        };

        try
        {
            var deleted =
                await _roomRepository.Delete(roomId);

            if (!deleted)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Rommet kan ikke slettes fordi det finnes bookinger for dette rommet.");

                return View(viewModel);
            }

            _logger.LogInformation(
                "Room {RoomId} deleted from location {LocationId}.",
                room.RoomId, room.LocationId);

            return RedirectToAction(
                nameof(Rooms),
                new { locationId = room.LocationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error deleting room {RoomId}.",
                room.RoomId);

            ModelState.AddModelError(
                string.Empty,
                "En feil oppstod ved sletting av rommet. Vennligst prøv igjen.");

            return View(viewModel);
        }
    }

    // GET: RoomLocation/Index
    // Students and administrators can browse locations.
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var locations =
            await _locationRepository.GetAllAsync();

        var viewModels = locations.Select(location =>
            new LocationViewModel
            {
                LocationId = location.LocationId,
                LocationName = location.Name
            }).ToList();

        return View(viewModels);
    }

    // GET: RoomLocation/Rooms
    // Public browsing and availability search.
    [HttpGet]
    public async Task<IActionResult> Rooms(
        int locationId,
        string? searchQuery,
        string? building,
        int? minCapacity,
        DateTime? startTime,
        DateTime? endTime,
        bool onlyAvailable = false)
    {
        var location =
            await _locationRepository.GetWithRoomsAsync(locationId);

        if (location == null)
            return NotFound("Lokasjonen finnes ikke.");

        if (onlyAvailable)
        {
            if (!startTime.HasValue || !endTime.HasValue)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Velg både starttidspunkt og sluttidspunkt for å søke etter ledige rom.");
            }
            else if (startTime.Value >= endTime.Value)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Starttidspunkt må være før sluttidspunkt.");
            }
        }

        var rooms = location.Rooms.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            rooms = rooms.Where(room =>
                room.RoomName.Contains(
                    searchQuery.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(building))
        {
            rooms = rooms.Where(room =>
                string.Equals(
                    room.Building.Trim(),
                    building.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(room =>
                room.Capacity >= minCapacity.Value);
        }

        var occupiedRoomIds = new HashSet<int>();

        if (startTime.HasValue &&
            endTime.HasValue &&
            startTime.Value < endTime.Value)
        {
            var occupiedIds =
                await _roomBookingRepository.GetOccupiedRoomIdAsync(
                    locationId,
                    startTime.Value,
                    endTime.Value);

            occupiedRoomIds = occupiedIds.ToHashSet();

            if (onlyAvailable)
            {
                rooms = rooms.Where(room =>
                    !occupiedRoomIds.Contains(room.RoomId));
            }
        }

        var viewModel = new LocationViewModel
        {
            LocationId = location.LocationId,
            LocationName = location.Name,
            SearchQuery = searchQuery,
            Building = building,
            MinCapacity = minCapacity,
            StartTime = startTime,
            EndTime = endTime,
            OnlyAvailable = onlyAvailable,
            OccupiedRoomIds = occupiedRoomIds,
            Rooms = rooms
                .OrderBy(room => room.RoomName)
                .ToList()
        };

        return View(viewModel);
    }

    // GET: RoomLocation/CreateLocation
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CreateLocation()
    {
        return View(new LocationFormViewModel());
    }

    // POST: RoomLocation/CreateLocation
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLocation(
        LocationFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var name = viewModel.Name.Trim();

        if (await _locationRepository.NameExistsAsync(name))
        {
            ModelState.AddModelError(
                nameof(viewModel.Name),
                "Denne lokasjonen finnes allerede.");

            return View(viewModel);
        }

        try
        {
            var location = new Location
            {
                Name = name
            };

            await _locationRepository.CreateAsync(location);

            _logger.LogInformation(
                "Location {LocationId} created.",
                location.LocationId);

            TempData["SuccessMessage"] =
                "Lokasjonen ble opprettet.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error creating location.");

            ModelState.AddModelError(
                string.Empty,
                "Kunne ikke lagre lokasjonen.");

            return View(viewModel);
        }
    }

    // GET: RoomLocation/EditLocation/5
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> EditLocation(int id)
    {
        var location =
            await _locationRepository.GetByIdAsync(id);

        if (location == null)
            return NotFound();

        return View(new LocationFormViewModel
        {
            LocationId = location.LocationId,
            Name = location.Name
        });
    }

    // POST: RoomLocation/EditLocation/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLocation(
        int id,
        LocationFormViewModel viewModel)
    {
        if (id != viewModel.LocationId)
            return BadRequest();

        var location =
            await _locationRepository.GetByIdAsync(id);

        if (location == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View(viewModel);

        var name = viewModel.Name.Trim();

        if (await _locationRepository.NameExistsAsync(name, id))
        {
            ModelState.AddModelError(
                nameof(viewModel.Name),
                "En annen lokasjon har allerede dette navnet.");

            return View(viewModel);
        }

        try
        {
            location.Name = name;

            await _locationRepository.UpdateAsync(location);

            _logger.LogInformation(
                "Location {LocationId} updated.",
                id);

            TempData["SuccessMessage"] =
                "Lokasjonen ble oppdatert.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating location {LocationId}.",
                id);

            ModelState.AddModelError(
                string.Empty,
                "Kunne ikke oppdatere lokasjonen.");

            return View(viewModel);
        }
    }

    // GET: RoomLocation/DeleteLocation/5
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location =
            await _locationRepository.GetWithRoomsAsync(id);

        if (location == null)
            return NotFound();

        return View(location);
    }

    // POST: RoomLocation/DeleteLocation/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ActionName("DeleteLocation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLocationConfirmed(int id)
    {
        var location =
            await _locationRepository.GetWithRoomsAsync(id);

        if (location == null)
            return NotFound();

        if (location.Rooms.Any())
        {
            ModelState.AddModelError(
                string.Empty,
                "Lokasjonen kan ikke slettes fordi den har rom.");

            return View("DeleteLocation", location);
        }

        try
        {
            bool deleted =
                await _locationRepository.DeleteAsync(id);

            if (!deleted)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Lokasjonen kunne ikke slettes. Kontroller at den ikke har rom.");

                return View("DeleteLocation", location);
            }

            _logger.LogInformation(
                "Location {LocationId} deleted.",
                id);

            TempData["SuccessMessage"] =
                "Lokasjonen ble slettet.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error deleting location {LocationId}.",
                id);

            ModelState.AddModelError(
                string.Empty,
                "En feil oppstod ved sletting.");

            return View("DeleteLocation", location);
        }
    }

    // GET: RoomLocation/Buildings
    [HttpGet]
    public async Task<IActionResult> Buildings(int locationId)
    {
        var location =
            await _locationRepository.GetWithRoomsAsync(locationId);

        if (location == null)
            return NotFound();

        var buildings = location.Rooms
            .Select(room => room.Building.Trim())
            .Where(building => building.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(building => building)
            .ToList();

        if (buildings.Count <= 1)
        {
            return RedirectToAction(
                nameof(Rooms),
                new { locationId });
        }

        return View(new LocationBuildingsViewModel
        {
            LocationId = location.LocationId,
            LocationName = location.Name,
            Buildings = buildings
        });
    }
}
