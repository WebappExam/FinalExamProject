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

    //Henter en booking basert på booking-Id
    public async Task<RoomBooking?> GetRoomBookingAsync (int bookingId)
    {
        return await _db.RoomBookings.FindAsync(bookingId);
    }

    //Slett en booking fra database
    public async Task DeleteAsync(RoomBooking booking)
    {
        _db.RoomBookings.Remove(booking);
        await _db.SaveChangesAsync();
    
    }

    //Henter brukers bookinger med informasjon om rommet
    public async Task<IEnumerable<RoomBooking>> GetByOrganizerIdAsync(
        string organizerId)
    { 
        return await _db.RoomBookings
    .AsNoTracking()
    .Where(booking => booking.OrganizerId == organizerId)
    .Include(booking => booking.Room)
    .OrderBy(booking => booking.StartTime)
    .ToListAsync ();

}
}