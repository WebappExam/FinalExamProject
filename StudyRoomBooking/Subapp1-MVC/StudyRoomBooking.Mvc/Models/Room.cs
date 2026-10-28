using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Web.Models;

// This class represents a physical group room on campus
public class Room
{
    // Unique identifier for the room
    public int RoomId { get; set; }

    // Room name must be between 2 and 50 characters
    [Required(ErrorMessage = "Room name is required")]
    [RegularExpression(
         @"^[\p{L}\p{N}. \-]{2,50}",
        ErrorMessage = "Room name must be between 2 and 50 characters and contain only valid characters."
    )]
    [Display(Name = "Room name")]
    public string RoomName { get; set; } = string.Empty;

    // Capacity must be between 1 and 100 people
    [Range(
        1,
        100,
        ErrorMessage = "Capacity must be between 1 and 100."
    )]

    [Required]
    public int? Capacity { get; set; } 

    // Building name must be between 2 and 50 characters
    [StringLength(
        50,
        MinimumLength = 2,
        ErrorMessage = "Building must be between 2 and 50 characters."
    )]

    [Required]
    public string Building { get; set; } = string.Empty;

    // Navigation property for bookings associated with the room
    public ICollection<RoomBooking> Bookings { get; set; } = new List<RoomBooking>();
}