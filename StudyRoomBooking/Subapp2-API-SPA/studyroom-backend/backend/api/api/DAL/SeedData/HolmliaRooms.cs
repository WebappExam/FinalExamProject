using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL.SeedData;

public static class HolmliaRooms
{
    public static Room[] GetRooms()
    {
        return new Room[]
        {
            new Room
            {
                RoomName = "Lofsrud",
                Capacity = 4,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
            new Room
            {
                RoomName = "Aasbraaten",
                Capacity = 4,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
            new Room
            {
                RoomName = "Mortensrud",
                Capacity = 6,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
            new Room
            {
                RoomName = "Bjorndal",
                Capacity = 6,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
            new Room
            {
                RoomName = "Bjornholt",
                Capacity = 4,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
            new Room
            {
                RoomName = "Prinsdal",
                Capacity = 8,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
             new Room
            {
                RoomName = "Deichman",
                Capacity = 10,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            },
             new Room
            {
                RoomName = "Hvervenbukta",
                Capacity = 10,
                Building = "Holmlia",
                RoomType = "Conference room",
                Available = ""
            },
             new Room
            {
                RoomName = "Torget",
                Capacity = 25,
                Building = "Holmlia",
                RoomType = "Conference room",
                Available = ""
            },
             new Room
            {
                RoomName = "Kinosalen",
                Capacity = 80,
                Building = "Holmlia",
                RoomType = "Group rom",
                Available = ""
            }
        };
    }
}