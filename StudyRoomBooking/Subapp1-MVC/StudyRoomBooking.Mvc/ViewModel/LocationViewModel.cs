using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.viewModel;

public class LocationViewModel
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = string.Empty;

    public List<Room> Rooms { get; set; } = new();
}