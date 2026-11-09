
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL;

public interface ILocationRepository
{
    // Retrieve all locations
    Task<List<Location>> GetAllAsync();

    // Retrieve a location by ID
    Task<Location?> GetByIdAsync(int locationId);

    // Retrieve a location with its rooms
    Task<Location?> GetWithRoomsAsync(int locationId);

    // Create a location
    Task CreateAsync(Location location);

    // Update a location
    Task UpdateAsync(Location location);

    // Delete only when the location has no rooms
    Task<bool> DeleteAsync(int locationId);

    // Check if another location already has the same name
    Task<bool> NameExistsAsync(string name, int? excludeLocationId = null);
}
