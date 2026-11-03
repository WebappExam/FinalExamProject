using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.Models;
namespace StudyRoomBooking.Mvc.DAL;

// Creating test data to test if we get our predefined rooms with ID number
// Example: LocationId = 1 will give you Holmlia and all of its room



public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await AddLocationWithRoomsAsync(db, "Holmlia",
            new Room
            {
                RoomName = "Bjorndal",
                Capacity = 6,
                Building = "Senteret"
            },
            new Room
            {
                RoomName = "H102",
                Capacity = 4,
                Building = "Senteret"
            });

        await AddLocationWithRoomsAsync(db, "Pilestredet",
            new Room
            {
                RoomName = "P301",
                Capacity = 8,
                Building = "P35"
            },
            new Room
            {
                RoomName = "P302",
                Capacity = 4,
                Building = "P35"
            });

        await AddLocationWithRoomsAsync(db, "Kjeller",
            new Room
            {
                RoomName = "K201",
                Capacity = 6,
                Building = "Hovedbygget"
            },
            new Room
            {
                RoomName = "K202",
                Capacity = 10,
                Building = "Hovedbygget"
            });
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