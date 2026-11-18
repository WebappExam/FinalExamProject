using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.DTOs;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController(
    ILocationRepository locationRepository,
    ILogger<LocationsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LocationDto>>> GetLocations()
    {
        var locations = await locationRepository.GetAllAsync();
        return Ok(locations.Select(ToDto).ToList());
    }

    [HttpGet("{locationId:int}")]
    public async Task<ActionResult<LocationDto>> GetLocation(int locationId)
    {
        var location = await locationRepository.GetByIdAsync(locationId);
        return location is null ? NotFound() : Ok(ToDto(location));
    }

    [HttpGet("{locationId:int}/rooms")]
    public async Task<ActionResult<LocationWithRoomsDto>> GetLocationRooms(int locationId)
    {
        var location = await locationRepository.GetWithRoomsAsync(locationId);
        if (location is null)
        {
            return NotFound();
        }

        return Ok(new LocationWithRoomsDto
        {
            LocationId = location.LocationId,
            Name = location.Name,
            Rooms = location.Rooms.Select(RoomsController.ToDto).ToList()
        });
    }

    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateLocation(LocationWriteDto request)
    {
        var name = request.Name.Trim();
        if (await locationRepository.NameExistsAsync(name))
        {
            return Conflict(new { message = "A location with this name already exists." });
        }

        var location = new Location { Name = name };
        await locationRepository.CreateAsync(location);
        logger.LogInformation("Created location {LocationId}", location.LocationId);

        return CreatedAtAction(
            nameof(GetLocation),
            new { locationId = location.LocationId },
            ToDto(location));
    }

    [HttpPut("{locationId:int}")]
    public async Task<IActionResult> UpdateLocation(int locationId, LocationWriteDto request)
    {
        var location = await locationRepository.GetByIdAsync(locationId);
        if (location is null)
        {
            return NotFound();
        }

        var name = request.Name.Trim();
        if (await locationRepository.NameExistsAsync(name, locationId))
        {
            return Conflict(new { message = "A location with this name already exists." });
        }

        location.Name = name;
        await locationRepository.UpdateAsync(location);
        logger.LogInformation("Updated location {LocationId}", locationId);

        return NoContent();
    }

    [HttpDelete("{locationId:int}")]
    public async Task<IActionResult> DeleteLocation(int locationId)
    {
        if (await locationRepository.GetByIdAsync(locationId) is null)
        {
            return NotFound();
        }

        if (!await locationRepository.DeleteAsync(locationId))
        {
            return Conflict(new { message = "A location with rooms cannot be deleted." });
        }

        logger.LogInformation("Deleted location {LocationId}", locationId);
        return NoContent();
    }

    private static LocationDto ToDto(Location location) => new()
    {
        LocationId = location.LocationId,
        Name = location.Name
    };
}
