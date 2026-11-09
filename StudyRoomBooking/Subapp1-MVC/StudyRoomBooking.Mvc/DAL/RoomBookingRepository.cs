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
        return await _db.RoomBooking.AnyAsync(booking =>
            booking.RoomId == roomId &&
            booking.StartTime < endTime && 
            booking.EndTime > startTime);
    }

    //Lagrer ny booking
    public async Task Create(RoomBooking booking)
    {
        _db.RoomBooking.Add(booking);
        await _db.SaveChangesAsync();
    }
    // Retrieve all bookings
    public async Task<List<RoomBooking>> GetAllAsync()
    {
        return await _db.RoomBooking
            .AsNoTracking()
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    // Retrieve one booking
    public async Task<RoomBooking?> GetByIdAsync(int id)
    {
        return await _db.RoomBooking
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }


    // Update an existing booking
    public async Task UpdateAsync(RoomBooking booking)
    {
        _db.RoomBooking.Update(booking);
        await _db.SaveChangesAsync();
    }

    // Delete an existing booking
    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await _db.RoomBooking
            .FindAsync(id);

        if (booking == null)
        {
            return false;
        }

        _db.RoomBooking.Remove(booking);
        await _db.SaveChangesAsync();

        return true;
    }

    // Check whether a room is booked during the requested period
    public async Task<bool> HasOverlappingBookingsAsync(
        int roomId,
        DateTime startTime,
        DateTime endTime,
        int? excludeBookingId = null)
    {
        return await _db.RoomBooking.AnyAsync(b =>
            b.RoomId == roomId &&
            b.StartTime < endTime &&
            b.EndTime > startTime &&
            (!excludeBookingId.HasValue ||
             b.Id != excludeBookingId.Value));
    }
}