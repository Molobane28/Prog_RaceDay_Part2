using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.DTOs; // Make sure your DTOs are in this namespace

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
            // 1. Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email already exists." });
            }

            // 2. Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Create the User
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
            await _context.SaveChangesAsync(); // This generates the UserID

            // 4. Create the specific profile based on the role
            if (request.Role == "Organiser")
            {
                var organiser = new Organiser
                {
                    UserID = user.UserID,
                    OrganisationName = request.FirstName + " " + request.LastName, // Default value
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
                    DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)), // Default value
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
            // 1. Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // 2. Verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // 3. Set session (Simple version for Part 2)
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Name", user.FirstName);

            return Ok(new
            {
                message = "Login successful",
                userID = user.UserID,
                firstName = user.FirstName,
                role = user.Role
            });
        }
    }
}