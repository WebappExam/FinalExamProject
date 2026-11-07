using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Location> Locations { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public DbSet<RoomBooking> RoomBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Beholder opsett for identity og brukertabeller
        base.OnModelCreating(modelBuilder);

        //Bruker smame bookingtabell som de eksisterende migrasjonene 
        modelBuilder.Entity <RoomBooking>().ToTable("RoomBooking");
    }
}
