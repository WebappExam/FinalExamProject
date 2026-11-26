using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Models;

public class Location
{
    public int LocationId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}