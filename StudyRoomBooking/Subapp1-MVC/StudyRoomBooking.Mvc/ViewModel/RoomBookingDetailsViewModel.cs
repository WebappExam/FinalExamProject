namespace StudyRoomBooking.Mvc.viewModel;


//This gives our views only the data they need,
//  without exposing organizer IDs or other sensitive information.
public class RoomBookingDetailsViewModel
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = "";
    public string? Course { get; set; }
    public string Description { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}