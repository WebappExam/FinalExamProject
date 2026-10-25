
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Models;

public class Location
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Building { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    public ICollection<Facility> Facilities { get; set; }
        = new List<Facility>();
}
