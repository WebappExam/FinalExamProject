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

    //Lagrer ny booking
    public async Task Create(RoomBooking booking)
    {
        _db.RoomBooking.Add(booking);
        await _db.SaveChangesAsync();
    }

    //Henter en booking basert på booking-Id
    public async Task<RoomBooking?> GetRoomBookingAsync (int bookingId)
    {
        return await _db.RoomBooking.FindAsync(bookingId);
    }

    //Slett en booking fra database
    public async Task DeleteAsync(RoomBooking booking)
    {
        _db.RoomBooking.Remove(booking);
        await _db.SaveChangesAsync();
    
    }

    //Henter brukers bookinger med informasjon om rommet
    public async Task<IEnumerable<RoomBooking>> GetByOrganizerIdAsync(
        string organizerId)
    { 
        return await _db.RoomBooking
    .AsNoTracking()
    .Where(booking => booking.OrganizerId == organizerId)
    .Include(booking => booking.Room)
    .OrderBy(booking => booking.StartTime)
    .ToListAsync ();

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

    public async Task<List<int>> GetOccupiedRoomIdAsync(
        int locationId, 
        DateTime startTime, 
        DateTime endTime)
    {

        return await _db.RoomBooking

        .Where(booking =>
        booking.Room.LocationId == locationId &&
        booking.StartTime <endTime &&
        booking.EndTime > startTime)
        .Select(booking => booking.RoomId)
        .Distinct()
        .ToListAsync();
    }

}