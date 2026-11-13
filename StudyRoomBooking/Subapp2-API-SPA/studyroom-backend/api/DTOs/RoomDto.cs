using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Mvc.DTOs;

public sealed class RoomDto
{
    public int RoomId { get; init; }
    public string RoomName { get; init; } = string.Empty;
    public int? Capacity { get; init; }
    public string Building { get; init; } = string.Empty;
    public int LocationId { get; init; }
    public string RoomType { get; init; } = string.Empty;
    public string Available { get; init; } = string.Empty;
}

public sealed class RoomWriteDto
{
    [Required]
    [RegularExpression(@"^[\p{L}\p{N}. \-]{2,50}$")]
    public string RoomName { get; init; } = string.Empty;

    [Required]
    [Range(1, 100)]
    public int? Capacity { get; init; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Building { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int LocationId { get; init; }

    public string RoomType { get; init; } = string.Empty;
    public string Available { get; init; } = string.Empty;
}