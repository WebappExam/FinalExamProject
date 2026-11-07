using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL.SeedData;

public static class KjellerRooms
{
    public static Room[] GetRooms()
    {
        return new Room[]
        {
            CreateRoom("C146", 5, "Grupperom"),
            CreateRoom("B236", 8, "Grupperom"),
            CreateRoom("B235", 8, "Grupperom"),
            CreateRoom("B237", 8, "Grupperom"),
            CreateRoom("B244", 8, "Grupperom"),
            CreateRoom("B242", 8, "Grupperom"),
            CreateRoom("B239", 8, "Grupperom"),
            CreateRoom("AB202A", 8, "Grupperom"),
            CreateRoom("C150", 8, "Grupperom"),
            CreateRoom("B238", 8, "Grupperom"),
            CreateRoom("B246", 10, "Grupperom"),
            CreateRoom("F243", 11, "Grupperom"),
            CreateRoom("B136", 12, "Grupperom"),
            CreateRoom("AB202B", 8, "Grupperom"),
            CreateRoom("B240", 8, "Grupperom"),
            CreateRoom("F244", 9, "Grupperom"),

            CreateRoom("B119", 1, "Laboratorium"),
            CreateRoom("C118", 1, "Spesialrom"),
            CreateRoom("C149", 1, "Spesialrom"),
            CreateRoom("K-D130", 1, "Sengepost"),
            CreateRoom("D132-Podcaststudio", 1, "Spesialrom"),
            CreateRoom("G105", 1, "Spesialrom"),
            CreateRoom("G110-G111-Spisestua", 1, "Spesialrom"),
            CreateRoom("G114", 1, "Spesialrom"),
            CreateRoom("G117", 1, "Spesialrom"),
            CreateRoom("G118", 1, "Spesialrom"),
            CreateRoom("G119", 1, "Spesialrom"),
            CreateRoom("Verksted PD", 1, "Spesialrom"),
            CreateRoom("G120-Øvelseskjøkken", 8, "Spesialrom"),
            CreateRoom("Bibliotekets stillerom", 8, "Spesialrom"),

            CreateRoom("F104", 20, "Klasserom"),
            CreateRoom("B123", 23, "Klasserom"),
            CreateRoom("B249", 30, "Klasserom"),
            CreateRoom("B250", 30, "Klasserom"),

            // Romtypen er skjult på skjermbildet.
            CreateRoom("F222", 30, ""),

            CreateRoom("F123", 32, "Klasserom"),
            CreateRoom("A324", 32, "Spesialrom"),
            CreateRoom("B349", 32, "Klasserom"),
            CreateRoom("B350", 32, "Klasserom"),
            CreateRoom("F121", 36, "Klasserom"),
            CreateRoom("B134", 36, "Klasserom"),
            CreateRoom("F120", 38, "Klasserom"),
            CreateRoom("F122", 38, "Klasserom"),
            CreateRoom("E210-211", 40, "Spesialrom"),
            CreateRoom("F203", 44, "Klasserom"),
            CreateRoom("Baserom 1MA", 45, "Spesialrom"),
            CreateRoom("Baserom 2BA", 45, "Spesialrom"),

            // Romtypen er skjult på skjermbildet.
            CreateRoom("Baserom 3BA", 45, ""),

            CreateRoom("H105", 45, "Spesialrom"),
            CreateRoom("Baserom 1BA", 45, "Spesialrom"),
            CreateRoom("Baserom 2MA", 45, "Spesialrom"),
            CreateRoom("F204", 50, "Klasserom"),
            CreateRoom("E200/E201", 50, "Spesialrom"),
            CreateRoom("F106", 60, "Klasserom"),
            CreateRoom("F110", 60, "Klasserom"),
            CreateRoom("F112", 60, "Klasserom"),

            CreateRoom(
                "E120, Ingrid Espelid Hovigs auditorium",
                69,
                "Auditorium"),

            CreateRoom("F211", 70, "Klasserom"),

            CreateRoom(
                "E111, Gudrun Wolans auditorium",
                115,
                "Auditorium"),

            CreateRoom(
                "E101, Grete Prytz Kittelsens auditorium",
                200,
                "Auditorium"),

            CreateRoom("Lesesal med takterrasse", 300, "KK rom"),
            CreateRoom("Vestre Gate", 1000, "Spesialrom"),
            CreateRoom("Søndre Gate", 1000, "Spesialrom"),
            CreateRoom("Østre Gate", 1000, "Spesialrom")
        };
    }

    private static Room CreateRoom(
        string roomName,
        int capacity,
        string roomType)
    {
        return new Room
        {
            RoomName = roomName,
            Capacity = capacity,
            Building = "Karethe Johnsens hus",
            RoomType = roomType,
            Available = ""
        };
    }
}