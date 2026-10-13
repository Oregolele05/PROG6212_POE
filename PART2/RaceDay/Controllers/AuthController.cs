using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Models;

namespace RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(RaceDayDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user as either an Organiser or a Participant. Creates the base User
        /// record and links it to the corresponding profile table. Password is hashed using BCrypt.
        /// </summary>
        /// <param name="request">Registration details: username, email, password, role, name, and phone.</param>
        /// <returns>201 Created on success, 400 Bad Request if the email already exists or the role is invalid.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Registration request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.userEmail == request.UserEmail))
                    return BadRequest(new { message = "Email is already registered." });

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.UserPassword);

                var newUser = new User
                {
                    username = request.Username,
                    userEmail = request.UserEmail,
                    userRole = request.UserRole,
                    password = hashedPassword
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                if (request.UserRole == "Organiser")
                {
                    var newOrganiser = new Organiser
                    {
                        organiserName = request.Name,
                        organiserEmail = request.UserEmail,
                        organiserPhone = request.Phone,
                        userID = newUser.userID
                    };
                    _context.Organisers.Add(newOrganiser);
                }
                else if (request.UserRole == "Participant")
                {
                    var newParticipant = new Participant
                    {
                        participantName = request.Name,
                        participantEmail = request.UserEmail,
                        participantPhone = request.Phone,
                        userID = newUser.userID
                    };
                    _context.Participants.Add(newParticipant);
                }
                else
                {
                    return BadRequest(new { message = "Invalid role. Must be 'Organiser' or 'Participant'." });
                }

                await _context.SaveChangesAsync();
                return StatusCode(201, new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        /// <summary>
        /// Authenticates a user with email and password. Creates a server-side session storing
        /// the UserID and UserRole for all subsequent authorized requests.
        /// </summary>
        /// <param name="request">Login credentials (email and password).</param>
        /// <returns>200 OK with userID and role on success, 401 Unauthorized on invalid credentials.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.userEmail == request.UserEmail);
                if (user == null) return Unauthorized(new { message = "Invalid email or password." });

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.UserPassword, user.password);
                if (!isPasswordValid) return Unauthorized(new { message = "Invalid email or password." });

                HttpContext.Session.SetInt32("UserID", user.userID);
                HttpContext.Session.SetString("UserRole", user.userRole);
                HttpContext.Session.SetString("UserEmail", user.userEmail);

                return Ok(new { message = "Login successful", userID = user.userID, role = user.userRole });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}