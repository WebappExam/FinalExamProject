using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Extensions; 

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

builder.Services.AddStudyRoomIdentity();
var app = builder.Build();
// Initialize User and Admin roles
await app.EnsureStudyRoomRolesAsync();


// Viser en generell feilside utenfor utviklingsmiljøet.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); 

// Vis lokasjonene på startsiden.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RoomLocation}/{action=Index}/{id?}");

app.Run();