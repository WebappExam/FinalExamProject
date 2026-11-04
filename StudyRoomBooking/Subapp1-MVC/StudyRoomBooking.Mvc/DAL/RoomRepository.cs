using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

public class RoomRepository : IRoomRepository

{
    private readonly ApplicationDbContext _db;

    public RoomRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    //Hent rom med Id, eller null hvis rommet ikke finnes
    public async Task<Room?> GetRoomById(int roomId)

    {
        return await _db.Rooms.FindAsync(roomId);
    }

    //Metoden som skal lagre et nytt rom 
    public async Task Create(Room room)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
    }
    //Lagrer endringer i et eksisterende rom
    public async Task Update(Room room)
    {
        _db.Rooms.Update(room);
        await _db.SaveChangesAsync();
    }
}