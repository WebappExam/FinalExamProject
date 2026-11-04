using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public class RoomRepository : IRoomRepository

{
    private readonly ApplicationDbContext _db;

    public RoomRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    //Metoden som skal lagre et nytt rom 
    public async Task Create(Room room)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
    }
}