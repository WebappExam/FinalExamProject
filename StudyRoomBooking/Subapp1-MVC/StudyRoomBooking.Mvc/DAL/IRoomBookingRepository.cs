using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public interface IRoomBookingRepository
{

    //Sjekker om det finnes overlappende bookinger for et gitt rom og tidsperiode
    Task<bool> HasOverlappingBookingsAsync(int roomId, DateTime startTime, DateTime endTime);

    //Lagrer ny booking
    Task Create(RoomBooking booking);

    //Henter en booking basert på ID
    Task<RoomBooking?> GetRoomBookingAsync (int bookingId);

    //Sletter en booking fra databasen
    Task DeleteAsync(RoomBooking booking);

    //Tar imot brukerId og returnerer en samling bookinger
    Task<IEnumerable<RoomBooking>> GetByOrganizerIdAsync(string organizerId);
}