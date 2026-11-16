namespace StudyRoomBooking.Mvc.viewModel;

public class LocationBuildingsViewModel
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;

    public List<string> Buildings { get; set; } = new();
}