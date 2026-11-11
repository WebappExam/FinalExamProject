
using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Mvc.viewModel;

public class RoomBookingEditViewModel
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string RoomName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Starttidspunkt er påkrevd.")]
    public DateTime? StartTime { get; set; }

    [Required(ErrorMessage = "Sluttidspunkt er påkrevd.")]
    public DateTime? EndTime { get; set; }


    [StringLength(30, ErrorMessage = "Kursnavn kan ikke være lengre enn 30 tegn.")]
    public string? Course { get; set; } 

    [Required(ErrorMessage ="Skriv inn en beskrivelse")]
    [StringLength(200, ErrorMessage = "Beskrivelsen kan ikke være lengre enn 200 tegn.")]
    public string Description { get; set; } = string.Empty;
}
