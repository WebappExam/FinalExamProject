using System.ComponentModel.DataAnnotations;
namespace StudyRoomBooking.Mvc.viewModel;
public class RoomBookingCreateViewModel
{

    //identifiserer rom som skal bookes
    [Range(1, int.MaxValue, ErrorMessage = "Velg et rom.")]
    public int RoomId { get; set; }

    //Vis i skjema og hentes fra database
    public string RoomName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Starttidspunkt er påkrevd.")]
    public DateTime? StartTime { get; set; }

    [Required(ErrorMessage = "Sluttidspunkt er påkrevd.")]
     public DateTime? EndTime { get; set; }

     //Skal vi beholde kursnavn og beskrivelse i booking? 

    [Required(ErrorMessage = "Skriv inn kursnavn")]
    [StringLength(30, ErrorMessage = "Kursnavn kan ikke være lengre enn 30 tegn.")]

    public string Course { get; set; } = string.Empty;
    [StringLength(200, ErrorMessage = "Kommentar kan ikke være lengre enn 200 tegn.")]
    public string? Description { get; set; } 
    }
