using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;
using Microsoft.EntityFrameworkCore;

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

//Sletter rom bare hvis det ikke finnes noen bookinger for rommet
public async Task<bool> Delete(int roomId)
    {
        var room = await _db.Rooms
            .Include(r => r.Bookings)
            .FirstOrDefaultAsync(r => r.RoomId == roomId);

        if (room == null || room.Bookings.Any())
        {
            // Rommet finnes, men har bookinger, så vi kan ikke slette det
            return false;
        }
        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync();


        return true;

    }





}