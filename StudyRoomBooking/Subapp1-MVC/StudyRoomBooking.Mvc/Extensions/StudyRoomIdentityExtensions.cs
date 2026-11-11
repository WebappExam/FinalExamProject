using Microsoft.AspNetCore.Identity;
using StudyRoomBooking.Mvc.DAL;
using StudyRoomBooking.Mvc.Models;

namespace StudyRoomBooking.Mvc.Extensions;

/// <summary>
/// Authentication and role setup for the MVC application.
/// Keeps Identity registration out of Program.cs and uses the existing DbContext.
/// </summary>
public static class StudyRoomIdentityExtensions
{
    public static IServiceCollection AddStudyRoomIdentity(this IServiceCollection services)
    {
        // The default Identity Login/Register/Logout UI uses Razor Pages.
        services.AddRazorPages();

        services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                // MVP: no email confirmation provider is configured yet.
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static async Task EnsureStudyRoomRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in new[] { "User", "Admin" })
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                var errors = string.Join("; ",
                    result.Errors.Select(error => error.Description));
                throw new InvalidOperationException(
                    $"Could not create Identity role '{roleName}': {errors}");
            }
        }
    }
}
