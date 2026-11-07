using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Location> Locations { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public DbSet<RoomBooking> RoomBookings { get; set; }
}
