
using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Data;
using StudyRoomBooking.Models;

// Connect to the existing SQLite database
var databasePath = Path.GetFullPath(
    Path.Combine("StudyRoomBooking.Database", "studyrooms.db")
);

var options = new DbContextOptionsBuilder<StudyRoomDbContext>()
    .UseSqlite($"Data Source={databasePath}")
    .Options;

using var db = new StudyRoomDbContext(options);

// Verify the database schema is ready
if (!(await db.Database.GetAppliedMigrationsAsync()).Any())
{
    Console.WriteLine("No applied migrations found.");
    return;
}

// Create a sample location
var location = new Location
{
    Building = "Pilestredet",
    Floor = 3,
    RoomNumber = "P301"
};

// Create a sample facility
var facility = new Facility
{
    Name = "Study Room 301",
    Capacity = 8,
    Location = location
};

// Save to SQLite
db.Facilities.Add(facility);
await db.SaveChangesAsync();

Console.WriteLine("Data saved successfully!");

// Retrieve the saved facility
var savedFacility = await db.Facilities
    .Include(f => f.Location)
    .FirstAsync(f => f.Id == facility.Id);

Console.WriteLine($"Facility: {savedFacility.Name}");
Console.WriteLine($"Capacity: {savedFacility.Capacity}");
Console.WriteLine($"Building: {savedFacility.Location.Building}");
Console.WriteLine($"Floor: {savedFacility.Location.Floor}");
Console.WriteLine($"Room: {savedFacility.Location.RoomNumber}");

//This test deliberately attempts to save a room with capacity 0.
//  It expects the database constraint to reject it.
Console.WriteLine("\nTesting invalid capacity...");

var invalidFacility = new Facility
{
    Name = "Invalid Test Room",
    Capacity = 0,
    LocationId = location.Id
};

db.Facilities.Add(invalidFacility);

try
{
    await db.SaveChangesAsync();
    Console.WriteLine("TEST FAILED: Invalid capacity was accepted.");
}
catch (DbUpdateException)
{
    Console.WriteLine("TEST PASSED: Capacity 0 was rejected.");
    db.Entry(invalidFacility).State = EntityState.Detached;
}
