namespace StudyRoomBooking.Mvc.viewModel;
public class RoomDeleteViewModel

{
    public int RoomId { get; set; }  //Identifiserer rommet som skal slettes
    public int LocationId { get; set; } //Gå tilbake til rom på valgt lokasjon

    //Opplysninger som vises for å bekrefte sletting av rommet
    public int? Capacity { get; set; }
    public string RoomName { get; set; } = string.Empty; 
    public string Building { get; set; } = string.Empty;

}