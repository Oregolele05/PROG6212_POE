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
        /// <summary>
        /// Retrieves the profile of the currently logged-in user. Merges data from the Users table
        /// and the corresponding Organiser or Participant table.
        /// </summary>
        /// <returns>200 OK with profile data, 401 Unauthorized if not logged in, 404 Not Found if user missing.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    userID = user.userID,
                    username = user.username,
                    userEmail = user.userEmail,
                    userRole = user.userRole,
                    name = name,
                    phone = phone
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // PUT: api/users/profile
        /// <summary>
        /// Updates the logged-in user's username, email, name, and phone number.
        /// </summary>
        /// <param name="request">Updated profile details.</param>
        /// <returns>200 OK on success, 401 Unauthorized, 404 Not Found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return Unauthorized(new { message = "You must be logged in to update your profile." });
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                user.username = request.Username;
                user.userEmail = request.UserEmail;

                if (user.userRole == "Organiser")
                {
                    var organiser = await _context.Organisers.FirstOrDefaultAsync(o => o.userID == userId);
                    if (organiser != null)
                    {
                        organiser.organiserName = request.Name;
                        organiser.organiserPhone = request.Phone;
                        organiser.organiserEmail = request.UserEmail;
                    }
                }
                else if (user.userRole == "Participant")
                {
                    var participant = await _context.Participants.FirstOrDefaultAsync(p => p.userID == userId);
                    if (participant != null)
                    {
                        participant.participantName = request.Name;
                        participant.participantPhone = request.Phone;
                        participant.participantEmail = request.UserEmail;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

    }
}