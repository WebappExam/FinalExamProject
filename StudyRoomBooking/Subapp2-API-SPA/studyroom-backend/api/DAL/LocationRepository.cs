
using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _db;

    public LocationRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    // Get all locations
    public async Task<List<Location>> GetAllAsync()
    {
        return await _db.Locations
            .AsNoTracking()
            .OrderBy(location => location.Name)
            .ToListAsync();
    }

    // Get one location
    public async Task<Location?> GetByIdAsync(int locationId)
    {
        return await _db.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(location =>
                location.LocationId == locationId);
    }

    // Get location with rooms
    public async Task<Location?> GetWithRoomsAsync(int locationId)
    {
        return await _db.Locations
            .AsNoTracking()
            .Include(location => location.Rooms)
            .FirstOrDefaultAsync(location =>
                location.LocationId == locationId);
    }

    // Create a new location
    public async Task CreateAsync(Location location)
    {
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();
    }

    // Update an existing location
    public async Task UpdateAsync(Location location)
    {
        _db.Locations.Update(location);
        await _db.SaveChangesAsync();
    }

    // Delete a location only if it contains no rooms
    public async Task<bool> DeleteAsync(int locationId)
    {
        var location = await _db.Locations
            .FirstOrDefaultAsync(l => l.LocationId == locationId);

        if (location == null)
        {
            return false;
        }

        // Prevent deletion when rooms are assigned to this location
        bool hasRooms = await _db.Rooms
            .AnyAsync(room => room.LocationId == locationId);

        if (hasRooms)
        {
            return false;
        }

        _db.Locations.Remove(location);
        await _db.SaveChangesAsync();

        return true;
    }

    // Check for duplicate location names
    public async Task<bool> NameExistsAsync(
        string name,
        int? excludeLocationId = null)
    {
        var normalizedName = name.Trim().ToLower();

        return await _db.Locations.AnyAsync(location =>
            location.Name.ToLower() == normalizedName &&
            (!excludeLocationId.HasValue ||
             location.LocationId != excludeLocationId.Value));
    }
}
