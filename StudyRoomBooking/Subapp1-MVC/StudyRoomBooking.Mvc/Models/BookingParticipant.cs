using System.ComponentModel.DataAnnotations;
namespace StudyRoomBooking.Web.Models;


/*
This class is the link that creates the Many-to-Many (N:M) relationship.
Without this class, a user could not  sign up for multiple bookings,
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

    //ApplicationUser class is not finished ATM
    public ApplicationUser User { get; set; } = null!;
}