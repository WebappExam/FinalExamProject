using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.DAL;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = // Prevents circular reference exceptions
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi(); // 2. Native .NET OpenAPI Support

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' is not configured."));
});

builder.Services.AddCors(options => // CORS Policy for React Frontend Integration
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") // Allows any port on localhost
              .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>(); //lagt til room repository
builder.Services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File($"Logs/api_{DateTime.Now:yyyyMMdd_HHmmss}.log");

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.SeedAsync(db);

    app.MapOpenApi(); // Serve modern OpenAPI document endpoint (/openapi/v1.json)
    app.MapScalarApiReference(); // Serve Scalar Interactive API Documentation UI (/scalar/v1)
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapStaticAssets();
app.MapControllers();

app.Run();
