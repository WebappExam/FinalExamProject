
using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Models;

namespace StudyRoomBooking.Data;

public class StudyRoomDbContext : DbContext
{
    public StudyRoomDbContext(
        DbContextOptions<StudyRoomDbContext> options)
        : base(options)
    {
    }

    public DbSet<Facility> Facilities { get; set; }
        = null!;

    public DbSet<Location> Locations { get; set; }
        = null!;

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Facility>()
            .HasOne(f => f.Location)
            .WithMany(l => l.Facilities)
            .HasForeignKey(f => f.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Facility>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Facility_Capacity",
                "Capacity >= 1 AND Capacity <= 1000"));
    }
}
