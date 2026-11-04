using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;
public interface IRoomRepository
{
    //Metoden som skal lagre et nytt rom 
    Task Create(Room room);

    //Hent rom med Id
    Task<Room?> GetRoomById(int roomId);
    

    //Lagrer endringer i et eksisterende rom
    Task Update(Room room);
    
}