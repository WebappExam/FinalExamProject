using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Gjør booking repository tilgjengelig for controlleren
builder.Services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();

// Registrer databasekonteksten med SQLite.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' mangler.")
    ));

// Registrer repositoryet som controlleren bruker.
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>(); //lagt til room repository
var app = builder.Build();

// Legg inn eksempeldata med: dotnet run -- --seed

/*
DON'T DELETE THIS PART YET
if (args.Contains("--seed"))
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await DbInitializer.SeedAsync(db);

    Console.WriteLine("Eksempeldata er lagt inn.");
    return;
}
*/

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await DbInitializer.SeedAsync(db);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Vis lokasjonene på startsiden.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RoomLocation}/{action=Index}/{id?}");

app.Run();