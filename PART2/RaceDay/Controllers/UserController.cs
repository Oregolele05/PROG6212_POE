using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Models;

namespace RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(RaceDayDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/users/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return Unauthorized(new { message = "You must be logged in to view your profile." });
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                string name = "Unknown";
                string phone = "Unknown";

                if (user.userRole == "Organiser")
                {
                    var organiser = await _context.Organisers.FirstOrDefaultAsync(o => o.userID == userId);
                    if (organiser != null)
                    {
                        name = organiser.organiserName;
                        phone = organiser.organiserPhone;
                    }
                }
                else if (user.userRole == "Participant")
                {
                    var participant = await _context.Participants.FirstOrDefaultAsync(p => p.userID == userId);
                    if (participant != null)
                    {
                        name = participant.participantName;
                        phone = participant.participantPhone;
                    }
                }

                var response = new UserProfile
                {
                    UserID = user.userID,
                    Username = user.username,
                    UserEmail = user.userEmail,
                    UserRole = user.userRole,
                    Name = name,
                    Phone = phone
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}