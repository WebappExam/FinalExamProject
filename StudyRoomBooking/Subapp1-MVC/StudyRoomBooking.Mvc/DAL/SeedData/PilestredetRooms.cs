using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL.SeedData;

public static class PilestredetRooms
{
    public static Room[] GetRooms()
    {
       
        // Capacities and types are from OsloMet's room directory
        return new Room[]
        {
            // PP35
            new Room { RoomName = "U1013", Capacity = 12, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1014", Capacity = 12, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1015", Capacity = 12, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1030", Capacity = 12, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1031", Capacity = 18, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1032", Capacity = 9, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1033", Capacity = 12, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1034", Capacity = 17, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1036", Capacity = 12, Building = "PP35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "U1042", Capacity = 8, Building = "PP35", RoomType = "Group study room", Available = "" },


            // P32
            new Room { RoomName = "N020.024", Capacity = 12, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N020.108", Capacity = 1, Building = "P32", RoomType = "Special-purpose room", Available = "" },
            new Room { RoomName = "N020.116", Capacity = 1, Building = "P32", RoomType = "Special-purpose room", Available = "" },
            new Room { RoomName = "N020.117", Capacity = 1, Building = "P32", RoomType = "Special-purpose room", Available = "" },
            new Room { RoomName = "N020.118", Capacity = 1, Building = "P32", RoomType = "Special-purpose room", Available = "" },
            new Room { RoomName = "N030.111", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.112", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.113", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.114", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.115", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.116", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.117", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.118", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N030.119", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.008", Capacity = 6, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.005", Capacity = 24, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.006", Capacity = 24, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.007", Capacity = 24, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.010", Capacity = 14, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.014", Capacity = 22, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.106", Capacity = 20, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.107", Capacity = 14, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.108", Capacity = 20, Building = "P32", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "N040.114", Capacity = 10, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.115", Capacity = 10, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.116", Capacity = 10, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.117", Capacity = 10, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.118", Capacity = 14, Building = "P32", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "N040.119", Capacity = 14, Building = "P32", RoomType = "Group study room", Available = "" },



            // P35
            new Room { RoomName = "PE238", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PE240", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PE241", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PE242", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PE322", Capacity = 4, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PE328", Capacity = 4, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH351", Capacity = 7, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH355", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH356", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH357", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH358", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH359", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH360", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH372", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH373", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PH374", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI230", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI232", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI234", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI235", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI236", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI237", Capacity = 6, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI334", Capacity = 4, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI336", Capacity = 8, Building = "P35", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "PI747", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI749", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI752", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI852", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI761", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI855", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI857", Capacity = 20, Building = "P35", RoomType = "Seminar room", Available = "" },
            new Room { RoomName = "PI861", Capacity = 16, Building = "P35", RoomType = "Seminar room", Available = "" },


            // P44
            new Room { RoomName = "V232", Capacity = 6, Building = "P44", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "V233", Capacity = 6, Building = "P44", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "V234", Capacity = 8, Building = "P44", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "V235", Capacity = 6, Building = "P44", RoomType = "Group study room", Available = "" },

            // P48
            new Room { RoomName = "R220", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "R221", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "R215", Capacity = 8, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "R222", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "R223", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "R224", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "R225", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "S261", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "S262", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "S263", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "S264", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "S265", Capacity = 6, Building = "P48", RoomType = "Group study room", Available = "" },


            // P50
            new Room { RoomName = "G121", Capacity = 7, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G218", Capacity = 8, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G221", Capacity = 6, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G220", Capacity = 6, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G223", Capacity = 10, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G227", Capacity = 8, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G321", Capacity = 6, Building = "P50", RoomType = "Laboratory", Available = "" },
            new Room { RoomName = "G411", Capacity = 10, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G413", Capacity = 10, Building = "P50", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "G414", Capacity = 10, Building = "P50", RoomType = "Group study room", Available = "" },

            // P52
            new Room { RoomName = "B312", Capacity = 6, Building = "P52", RoomType = "Group study room", Available = "" },
            new Room { RoomName = "B314", Capacity = 6, Building = "P52", RoomType = "Group study room", Available = "" },

        };
    }
}