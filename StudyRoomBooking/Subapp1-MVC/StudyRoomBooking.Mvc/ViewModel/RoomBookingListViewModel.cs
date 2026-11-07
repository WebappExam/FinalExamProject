namespace StudyRoomBooking.Mvc.viewModel;

public class RoomBookingListViewModel

{
    //Identifiser booking
     public int BookingId {get; set;}

    //Opplysninger som vises i bookingslisten
    public string RoomName {get; set;} = string.Empty;
    public string Building {get; set;} = string.Empty;
    public string Course {get; set;} = string.Empty;

    public DateTime StartTime { get; set;}
    public DateTime EndTime {get; set;}
    
}

