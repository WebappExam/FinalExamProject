using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public interface IRoomBookingRepository
{

    //Sjekker om det finnes overlappende bookinger for et gitt rom og tidsperiode
    Task<bool> HasOverlappingBookingsAsync(int roomId, DateTime startTime, DateTime endTime);

    //create new booking
    Task Create(RoomBooking booking);

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