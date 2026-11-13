using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.DTOs;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Controllers;

[ApiController]
[Route("api/bookings")]
public class RoomBookingsController(
    IRoomBookingRepository bookingRepository) : ControllerBase
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
        return booking is null ? NotFound() : Ok(ToDto(booking));
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
