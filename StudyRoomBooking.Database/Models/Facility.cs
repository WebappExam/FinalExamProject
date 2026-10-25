using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Models;

public class Facility
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Capacity { get; set; }

    public int LocationId { get; set; }

    public Location Location { get; set; } = null!;
}
