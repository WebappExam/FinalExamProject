using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudyRoomBooking.Mvc.DTOs.Auth;
using StudyRoomBooking.Mvc.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyRoomBooking.Mvc.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var existingUser =
            await userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return Conflict(new
            {
                message = "An account with this email already exists."
            });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            StudyProgram = request.StudyProgram
        };

        var result = await userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });
        }

        // New registrations receive the ordinary User role.
        var roleResult = await userManager.AddToRoleAsync(
            user, "User");

        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);

            logger.LogError(
                "Could not assign User role to {Email}",
                request.Email);

            return StatusCode(500, new
            {
                message = "Could not complete registration."
            });
        }

        logger.LogInformation(
            "Registered new user {UserId}",
            user.Id);

        return Ok(new
        {
            message = "Registration successful."
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(
            request.Email);

        if (user == null ||
            !await userManager.CheckPasswordAsync(
                user, request.Password))
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id),

            new(
                ClaimTypes.Name,
                user.Email ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(
                ClaimTypes.Role,
                role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: "StudyRoomBookingApi",
            audience: "StudyRoomBookingReact",
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenString =
            new JwtSecurityTokenHandler().WriteToken(token);

        logger.LogInformation(
            "User {UserId} logged in",
            user.Id);

        return Ok(new
        {
            token = tokenString,
            expiresAt = expires,
            email = user.Email,
            roles
        });
    }
}
