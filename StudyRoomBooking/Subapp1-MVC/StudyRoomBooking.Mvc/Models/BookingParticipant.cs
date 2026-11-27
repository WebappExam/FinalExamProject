using System.ComponentModel.DataAnnotations;
namespace StudyRoomBooking.Mvc.Models;


/*
This class is the link that creates the Many-to-Many (N:M) relationship.
Without this class, a user could not sign up for multiple bookings,
and a booking could not have multiple participants 
*/

public class BookingParticipant
{
    public int Id { get; set; }

    //Connecting it to RoomBooking (meaning the booking made)
    public int RoomBookingId { get; set; }
    public RoomBooking RoomBooking { get; set; } = null!;


    //Connecting the users who are going to join the room

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    //Tells us how many participants will use the room
    //Does not go above the room capasity (e.g Can't be 6 students in a 5 capacity room)
    public int BookingParticipants { get; set; }
}