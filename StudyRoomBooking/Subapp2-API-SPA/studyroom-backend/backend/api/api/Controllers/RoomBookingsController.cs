using Microsoft.AspNetCore.Mvc;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Controllers;

[ApiController]
[Route("api/bookings")]
public class RoomBookingsController(
    IRoomBookingRepository bookingRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RoomBooking>>> GetBookings()
    {
        return Ok(await bookingRepository.GetAllAsync());
    }

    [HttpGet("{bookingId:int}")]
    public async Task<ActionResult<RoomBooking>> GetBooking(int bookingId)
    {
        var booking = await bookingRepository.GetByIdAsync(bookingId);
        return booking is null ? NotFound() : Ok(booking);
    }
}
