namespace StudyRoomBooking.Mvc.DTOs;

public sealed class RoomBookingDto
{
    public int Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Course { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int RoomId { get; init; }
}
