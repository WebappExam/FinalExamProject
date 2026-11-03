using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL;

public interface ILocationRepository
{
    Task<List<Location>> GetAllAsync();

    Task<Location?> GetWithRoomsAsync(int locationId);
}