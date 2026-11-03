using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.viewModel;

public class LocationViewModel
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = string.Empty;

    //Søketekst som skal vises i søkefelt
    public string? SearchQuery { get; set; } 

    //Bygget bruker ønsker å filtrere rom på
    public string? Building { get; set; }
    public List<Room> Rooms { get; set; } = new();
}