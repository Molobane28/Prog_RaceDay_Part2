using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // POST: /api/auth/register
        // Registers a new user and creates the matching Organiser
        // or Participant profile based on the chosen Role.
        // =========================================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // 1. Validate the Role value manually (matches CK_USERS_Role)
            if (request.Role != "Organiser" && request.Role != "Participant")
            {
                return BadRequest(new
                {
                    message = "Role must be either 'Organiser' or 'Participant'."
                });
            }

            // 2. Check if email is already taken (409 Conflict)
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "A user with this email already exists."
                });
            }

            // 3. Create the User with a hashed password
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                DateRegistered = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // get the auto-generated UserID

            // 4. Create the matching profile based on Role
            if (request.Role == "Organiser")
            {
                var organiser = new Organiser
                {
                    UserID = user.UserID,
                    OrganisationName = "Not set yet",
                    ContactPhone = "Not set yet",
                    ContactEmail = user.Email
                };
                _context.Organisers.Add(organiser);
            }
            else // Participant
            {
                var participant = new Participant
                {
                    UserID = user.UserID,
                    DateOfBirth = new DateOnly(1900, 1, 1),
                    Gender = "Not set yet",
                    EmergencyContact = "Not set yet",
                    MedicalConditions = null
                };
                _context.Participants.Add(participant);
            }

            await _context.SaveChangesAsync();

            // 5. Respond with 201 Created
            var response = new AuthResponseDto
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Message = "User registered successfully."
            };

            return StatusCode(StatusCodes.Status201Created, response);
        }

        // =========================================================
        // POST: /api/auth/login
        // Authenticates a user with email + password.
        // =========================================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // 1. Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

            // 2. Generic 401 for both "no user" and "wrong password" (prevents email enumeration)
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // 3. Verify password against the BCrypt hash
            var passwordMatches = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!passwordMatches)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // 4. Return 200 OK
            var response = new AuthResponseDto
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Message = "Login successful."
            };

            return Ok(response);
        }
    }
}