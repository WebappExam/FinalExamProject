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

    //Minste antall personer rom kan ha plass til
    public int? MinCapacity { get; set; }

    //Tidsrom bruker ønsker å finne ledige rom i 
    public DateTime? StartTime {get; set;}

    public DateTime? EndTime { get; set;}

    //Begrenser søkeresultat til ledige rom
    public bool OnlyAvailable {get; set;}

    //Rom som har overlappende bookinger i valgt tidsrom
    public HashSet<int> OccupiedRoomIds {get; set;} = new();
    public List<Room> Rooms { get; set; } = new();
}