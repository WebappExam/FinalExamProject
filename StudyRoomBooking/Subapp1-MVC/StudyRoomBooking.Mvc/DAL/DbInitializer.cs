using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.DAL.SeedData;

// Creating test data to test if we get our predefined rooms with ID number
// Example: LocationId = 1 will give you Holmlia and all of its room



public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await AddLocationWithRoomsAsync (
            db, "Holmlia", HolmliaRooms.GetRooms());

        await AddLocationWithRoomsAsync (
            db, "Pilestredet", PilestredetRooms.GetRooms());
        
        await AddLocationWithRoomsAsync (
            db, "Kjeller", KjellerRooms.GetRooms());
        
    }

    private static async Task AddLocationWithRoomsAsync(
        ApplicationDbContext db,
        string locationName,
        params Room[] rooms)
    {
        var location = await db.Locations
            .FirstOrDefaultAsync(l => l.Name == locationName);

        if (location == null)
        {
            location = new Location
            {
                Name = locationName
            };

            db.Locations.Add(location);
            await db.SaveChangesAsync();
        }

        foreach (var room in rooms)
        {
            var roomExists = await db.Rooms.AnyAsync(r =>
                r.LocationId == location.LocationId &&
                r.RoomName == room.RoomName);

            if (!roomExists)
            {
                room.Location = location;
                db.Rooms.Add(room);
            }
        }

        await db.SaveChangesAsync();
    }
}