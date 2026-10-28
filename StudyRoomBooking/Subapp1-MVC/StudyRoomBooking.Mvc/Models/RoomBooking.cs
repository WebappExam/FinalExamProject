using System.ComponentModel.DataAnnotations;
namespace StudyRoomBooking.Web.Models;

/*
This class represents a concrete booking within a concrete timeframe (Start Time and End Time)
*/


public class RoomBooking
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }

    [Required]
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }
    [Required, StringLength(50)]
    public string Course { get; set; } = string.Empty;    // e.g. The course name for the booking.

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public int RoomId { get; set; }

    public Room Room { get; set; } = null!;

    // Who made the booking
    public string OrganizerId { get; set; } = string.Empty;
    public ApplicationUser Organizer { get; set; } = null!;

    //Navigation property
    public ICollection<BookingParticipant> Participants { get; set; } = new List<BookingParticipant>();

}