using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Mvc.DTOs;

public sealed class LocationDto
{
    public int LocationId { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class LocationWithRoomsDto
{
    public int LocationId { get; init; }
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<RoomDto> Rooms { get; init; } = [];
}

public sealed class LocationWriteDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;
}
