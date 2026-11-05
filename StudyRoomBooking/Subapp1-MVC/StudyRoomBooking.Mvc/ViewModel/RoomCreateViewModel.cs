using System.ComponentModel.DataAnnotations;
namespace StudyRoomBooking.Mvc.viewModel;

public class RoomCreateViewModel
{

    //Knytter rom til valgt lokasjon
    [Range(1, int.MaxValue, ErrorMessage = "Velg en lokasjon")]
    public int LocationId { get; set; }

    [Required (ErrorMessage = "Skriv romnavn")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Romnavn må ha mellom 2 og 50 tegn")]
    public string RoomName { get; set; } = string.Empty;

    [Required (ErrorMessage = "Skriv antall personer rommet skal ha plass til")]
    [Range(1, 100, ErrorMessage = "Antall personer må være mellom 1 og 100")] // Eksempel på validering av kapasitet, må være mellom 1 og 100
    public int? Capacity { get; set; }

    [Required (ErrorMessage = "Skriv byggnavn")]
    [StringLength(50, MinimumLength = 2,
    ErrorMessage = "Byggnavn må ha mellom 2 og 50 tegn")]
    public string Building { get; set; } = string.Empty;

}