
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudyRoomBooking.Data;

public class StudyRoomDbContextFactory
    : IDesignTimeDbContextFactory<StudyRoomDbContext>
{
    public StudyRoomDbContext CreateDbContext(string[] args)
    {
        var databasePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "studyrooms.db"
        );

        var optionsBuilder =
            new DbContextOptionsBuilder<StudyRoomDbContext>();

        optionsBuilder.UseSqlite(
            $"Data Source={databasePath}"
        );

        return new StudyRoomDbContext(optionsBuilder.Options);
    }
}

