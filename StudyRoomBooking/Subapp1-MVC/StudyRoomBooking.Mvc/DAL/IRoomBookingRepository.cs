using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public interface IRoomBookingRepository
{

    //Sjekker om det finnes overlappende bookinger for et gitt rom og tidsperiode
    Task<bool> HasOverlappingBookingsAsync(int roomId, DateTime startTime, DateTime endTime);

    //Lagrer ny booking
    Task Create(RoomBooking booking);
    
}