
using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Mvc.viewModel;

public class LocationFormViewModel
{
    public int LocationId { get; set; }

    [Required(ErrorMessage = "Lokasjonsnavn er påkrevd.")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Navnet må være mellom 2 og 50 tegn.")]
    public string Name { get; set; } = string.Empty;
}

