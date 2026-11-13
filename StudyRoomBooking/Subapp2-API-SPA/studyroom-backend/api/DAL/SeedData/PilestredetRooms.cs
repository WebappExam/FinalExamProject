using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL.SeedData;

public static class PilestredetRooms
{
    public static Room[] GetRooms()
    {
        return new Room[]
        {
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
            }
        };
    }
}