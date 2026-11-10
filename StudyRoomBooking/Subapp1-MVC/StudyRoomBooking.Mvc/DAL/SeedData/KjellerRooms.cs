using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL.SeedData;

public static class KjellerRooms
{
    public static Room[] GetRooms()
    {
        return new Room[]
        {
            CreateRoom("C146", 5, "Group study room"),
            CreateRoom("B236", 8, "Group study room"),
            CreateRoom("B235", 8, "Group study room"),
            CreateRoom("B237", 8, "Group study room"),
            CreateRoom("B244", 8, "Group study room"),
            CreateRoom("B242", 8, "Group study room"),
            CreateRoom("B239", 8, "Group study room"),
            CreateRoom("AB202A", 8, "Group study room"),
            CreateRoom("C150", 8, "Group study room"),
            CreateRoom("B238", 8, "Group study room"),
            CreateRoom("B246", 10, "Group study room"),
            CreateRoom("F243", 11, "Group study room"),
            CreateRoom("B136", 12, "Group study room"),
            CreateRoom("AB202B", 8, "Group study room"),
            CreateRoom("B240", 8, "Group study room"),
            CreateRoom("F244", 9, "Group study room"),
            CreateRoom("B119", 1, "Laboratory"),
            CreateRoom("C118", 1, "Special-purpose room"),
            CreateRoom("C149", 1, "Special-purpose room"),
            CreateRoom("K-D130", 1, "Ward"),
            CreateRoom("D132-Podcaststudio", 1, "Special-purpose room"),
            CreateRoom("G105", 1, "Special-purpose room"),
            CreateRoom("G110-G111-Spisestua", 1, "Special-purpose room"),
            CreateRoom("G114", 1, "Special-purpose room"),
            CreateRoom("G117", 1, "Special-purpose room"),
            CreateRoom("G118", 1, "Special-purpose room"),
            CreateRoom("G119", 1, "Special-purpose room"),
            CreateRoom("Verksted PD", 1, "Special-purpose room"),
            CreateRoom("G120-Øvelseskjøkken", 8, "Special-purpose room"),
            CreateRoom("Bibliotekets stillerom", 8, "Special-purpose room"),
            CreateRoom("F104", 20, "Classroom"),
            CreateRoom("B123", 23, "Classroom"),
            CreateRoom("B249", 30, "Classroom"),
            CreateRoom("B250", 30, "Classroom"),
            CreateRoom("F222", 30, "Classroom"),
            CreateRoom("F123", 32, "Classroom"),
            CreateRoom("A324", 32, "Special-purpose room"),
            CreateRoom("B349", 32, "Classroom"),
            CreateRoom("B350", 32, "Classroom"),
            CreateRoom("F121", 36, "Classroom"),
            CreateRoom("B134", 36, "Classroom"),
            CreateRoom("F120", 38, "Classroom"),
            CreateRoom("F122", 38, "Classroom"),
            CreateRoom("E210-211", 40, "Special-purpose room"),
            CreateRoom("F203", 44, "Classroom"),
            CreateRoom("Baserom 1MA", 45, "Special-purpose room"),
            CreateRoom("Baserom 2BA", 45, "Special-purpose room"),
            CreateRoom("Baserom 3BA", 45, "Special-purpose room"),
            CreateRoom("H105", 45, "Special-purpose room"),
            CreateRoom("Baserom 1BA", 45, "Special-purpose room"),
            CreateRoom("Baserom 2MA", 45, "Special-purpose room"),
            CreateRoom("F204", 50, "Classroom"),
            CreateRoom("E200/E201", 50, "Special-purpose room"),
            CreateRoom("F106", 60, "Classroom"),
            CreateRoom("F110", 60, "Classroom"),
            CreateRoom("F112", 60, "Classroom"),
            CreateRoom("E120, Ingrid Espelid Hovigs auditorium", 69, "Auditorium"),
            CreateRoom("F211", 70, "Classroom"),
            CreateRoom( "E111, Gudrun Wolans auditorium", 115, "Auditorium"),
            CreateRoom("E101, Grete Prytz Kittelsens auditorium", 200,"Auditorium"),
            CreateRoom("Lesesal med takterrasse", 300, "KK room"),
            CreateRoom("Vestre Gate", 1000, "Special-purpose room"),
            CreateRoom("Søndre Gate", 1000, "Special-purpose room"),
            CreateRoom("Østre Gate", 1000, "Special-purpose room")
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