using Microsoft.EntityFrameworkCore;
using StudyRoomBooking.Mvc.DAL;
using Scalar.AspNetCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using StudyRoomBooking.Mvc.Models;

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

// Identity configuration
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT signing key is not configured.");

var jwtIssuer = "StudyRoomBookingApi";
var jwtAudience = "StudyRoomBookingReact";

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)),

                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role,

                ClockSkew = TimeSpan.FromMinutes(1)
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.SeedAsync(db);

    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var role in new[] { "User", "Admin" })
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    app.MapOpenApi(); // Serve modern OpenAPI document endpoint (/openapi/v1.json)
    app.MapScalarApiReference(); // Serve Scalar Interactive API Documentation UI (/scalar/v1)
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllers();

app.Run();
