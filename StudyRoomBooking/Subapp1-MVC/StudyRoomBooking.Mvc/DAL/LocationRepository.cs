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

    public async Task<List<Location>> GetAllAsync()
    {
        return await _db.Locations
            .AsNoTracking()
            .OrderBy(location => location.Name)
            .ToListAsync();
    }

    public async Task<Location?> GetWithRoomsAsync(int locationId)
    {
        return await _db.Locations
            .AsNoTracking()
            .Include(location => location.Rooms)
            .FirstOrDefaultAsync(location =>
                location.LocationId == locationId);
    }
}