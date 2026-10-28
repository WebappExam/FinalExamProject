using Microsoft.AspNetCore.Identity;

namespace StudyRoomBooking.Web.Models;

/*
This class represents a student/user in the system
*/


public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string StudyProgram { get; set; } = string.Empty;

    // Navigation property
    
    public ICollection<RoomBooking> OrganizedBookings { get; set; } = new List<RoomBooking>();
    public ICollection<BookingParticipant> JoinedBookings { get; set; } = new List<BookingParticipant>();
}