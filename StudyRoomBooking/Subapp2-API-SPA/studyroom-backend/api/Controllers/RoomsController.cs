using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.DTOs;
using StudyRoomBooking.Mvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace StudyRoomBooking.Mvc.Controllers;

[Authorize]
[ApiController]
[Route("api/rooms")]
public class RoomsController(
    ILocationRepository locationRepository,
    IRoomRepository roomRepository,
    ILogger<RoomsController> logger) : ControllerBase
{
   [AllowAnonymous]
[HttpGet("{roomId:int}")]
public async Task<ActionResult<RoomDto>> GetRoom(int roomId)
 {
        var room = await roomRepository.GetRoomById(roomId);
       if (room is null)
       {
           return Problem(
               title: "Room not found",
               detail: $"Room with ID {roomId} does not exist.",
               statusCode: StatusCodes.Status404NotFound);
       }

       return Ok(ToDto(room));
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom(RoomWriteDto request)
    {
        if (await locationRepository.GetByIdAsync(request.LocationId) is null)
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]>
                {
                    [nameof(request.LocationId)] =
                        ["The specified location does not exist."]
                })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            });
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

        return CreatedAtAction(
            nameof(GetRoom),
            new { roomId = room.RoomId },
            ToDto(room));
    }

    [HttpPut("{roomId:int}")]
    public async Task<IActionResult> UpdateRoom(int roomId, RoomWriteDto request)
    {
        var room = await roomRepository.GetRoomById(roomId);
        if (room is null)
        {
            return Problem(
                title: "Room not found",
                detail: $"Room with ID {roomId} does not exist.",
                statusCode: StatusCodes.Status404NotFound);
        }

        if (await locationRepository.GetByIdAsync(request.LocationId) is null)
        {
            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]>
                {
                    [nameof(request.LocationId)] =
                        ["The specified location does not exist."]
                })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            });
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
            return Problem(
                title: "Room not found",
                detail: $"Room with ID {roomId} does not exist.",
                statusCode: StatusCodes.Status404NotFound);
        }

        if (!await roomRepository.Delete(roomId))
        {
            return Problem(
                title: "Room conflict",
                detail: "A room with bookings cannot be deleted.",
                statusCode: StatusCodes.Status409Conflict);
        }

        logger.LogInformation("Deleted room {RoomId}", roomId);
        return NoContent();
    }

    internal static RoomDto ToDto(Room room) => new()
    {
        RoomId = room.RoomId,
        RoomName = room.RoomName,
        Capacity = room.Capacity,
        Building = room.Building,
        LocationId = room.LocationId,
        RoomType = room.RoomType,
        Available = room.Available
    };
}
