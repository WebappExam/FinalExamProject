using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;
public interface IRoomRepository
{
    //Metoden som skal lagre et nytt rom 
    Task Create(Room room);
    
}