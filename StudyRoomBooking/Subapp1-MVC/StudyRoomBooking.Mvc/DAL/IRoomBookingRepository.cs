using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public interface IRoomBookingRepository
{

    //create new booking
    Task Create(RoomBooking booking);

    //Henter en booking basert på ID
    Task<RoomBooking?> GetRoomBookingAsync (int bookingId);

    //Sletter en booking fra databasen
    Task DeleteAsync(RoomBooking booking);

    //Tar imot brukerId og returnerer en samling bookinger
    Task<IEnumerable<RoomBooking>> GetByOrganizerIdAsync(string organizerId);
     // Get all bookings
    Task<List<RoomBooking>> GetAllAsync();

    // Get a booking by ID
    Task<RoomBooking?> GetByIdAsync(int id);

    // Update a booking
    Task UpdateAsync(RoomBooking booking);

    // Delete a booking
    Task<bool> DeleteAsync(int id);

    // Check for overlapping bookings
    // excludeBookingId prevents a booking from conflicting with itself
    Task<bool> HasOverlappingBookingsAsync(
        int roomId,
        DateTime startTime,
        DateTime endTime,
        int? excludeBookingId = null
    );
}