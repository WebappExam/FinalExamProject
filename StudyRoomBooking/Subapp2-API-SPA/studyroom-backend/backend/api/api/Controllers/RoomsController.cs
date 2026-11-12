using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController(
    ILocationRepository locationRepository,
    IRoomRepository roomRepository,
    ILogger<RoomsController> logger) : ControllerBase
{
    [HttpGet("{roomId:int}")]
    public async Task<ActionResult<Room>> GetRoom(int roomId)
    {
        var room = await roomRepository.GetRoomById(roomId);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> CreateRoom(Room request)
    {
        if (await locationRepository.GetByIdAsync(request.LocationId) is null)
        {
            return BadRequest(new { message = "The specified location does not exist." });
        }

        var room = new Room
        {
            RoomName = request.RoomName,
            Capacity = request.Capacity,
            Building = request.Building,
            LocationId = request.LocationId,
            RoomType = request.RoomType,
            Available = request.Available
        };

        await roomRepository.Create(room);
        logger.LogInformation("Created room {RoomId}", room.RoomId);

        return CreatedAtAction(nameof(GetRoom), new { roomId = room.RoomId }, room);
    }

    [HttpPut("{roomId:int}")]
    public async Task<IActionResult> UpdateRoom(int roomId, Room request)
    {
        if (roomId != request.RoomId)
        {
            return BadRequest(new { message = "The route room ID does not match the request room ID." });
        }

        var room = await roomRepository.GetRoomById(roomId);
        if (room is null)
        {
            return NotFound();
        }

        if (await locationRepository.GetByIdAsync(request.LocationId) is null)
        {
            return BadRequest(new { message = "The specified location does not exist." });
        }

        room.RoomName = request.RoomName;
        room.Capacity = request.Capacity;
        room.Building = request.Building;
        room.LocationId = request.LocationId;
        room.RoomType = request.RoomType;
        room.Available = request.Available;

        await roomRepository.Update(room);
        logger.LogInformation("Updated room {RoomId}", roomId);

        return NoContent();
    }

    [HttpDelete("{roomId:int}")]
    public async Task<IActionResult> DeleteRoom(int roomId)
    {
        var room = await roomRepository.GetRoomById(roomId);
        if (room is null)
        {
            return NotFound();
        }

        if (!await roomRepository.Delete(roomId))
        {
            return Conflict(new { message = "A room with bookings cannot be deleted." });
        }

        logger.LogInformation("Deleted room {RoomId}", roomId);
        return NoContent();
    }
}
