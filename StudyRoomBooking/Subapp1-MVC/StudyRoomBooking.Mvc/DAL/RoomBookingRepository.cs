using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public class RoomBookingRepository : IRoomBookingRepository
{
    private readonly ApplicationDbContext _db;

    public RoomBookingRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    //Sjekker om det finnes overlappende bookinger for et gitt rom og tidsperiode
    public async Task<bool> HasOverlappingBookingsAsync(int roomId, DateTime startTime, DateTime endTime)
    {
        return await _db.RoomBookings.AnyAsync(booking =>
            booking.RoomId == roomId &&
            (booking.StartTime < endTime && 
            booking.EndTime > startTime));
    }

    //Lagrer ny booking
    public async Task Create(RoomBooking booking)
    {
        _db.RoomBookings.Add(booking);
        await _db.SaveChangesAsync();
    }
}