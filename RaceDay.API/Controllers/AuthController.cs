using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.DTOs;
using System.Security.Claims;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email already exists." });
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = passwordHash,
                Role = request.Role,
                DateRegistered = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (request.Role == "Organiser")
            {
                var organiser = new Organiser
                {
                    UserID = user.UserID,
                    OrganisationName = request.FirstName + " " + request.LastName,
                    ContactPhone = "N/A",
                    ContactEmail = request.Email
                };
                _context.Organisers.Add(organiser);
            }
            else if (request.Role == "Participant")
            {
                var participant = new Participant
                {
                    UserID = user.UserID,
                    DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
                    Gender = "N/A",
                    EmergencyContact = "N/A",
                    MedicalConditions = "None"
                };
                _context.Participants.Add(participant);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Create the claims for the cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            // Sign the user in (creates the encrypted cookie)
            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            return Ok(new
            {
                message = "Login successful",
                userID = user.UserID,
                firstName = user.FirstName,
                role = user.Role
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok(new { message = "Logged out successfully." });
        }
    }
}