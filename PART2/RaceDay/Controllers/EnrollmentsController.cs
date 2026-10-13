using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Filters;
using RaceDay.Models;

namespace RaceDay.Controllers
{
    [Route("api")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(RaceDayDbContext context, ILogger<EnrollmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/enrolments (Participant Only)
        [HttpPost("enrolments")]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> EnrollInEvent([FromBody] Enrollment request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var participant = await _context.Participants.FirstOrDefaultAsync(p => p.userID == userId);
                if (participant == null) return BadRequest(new { message = "Participant profile not found." });

                // Check if event exists
                var eventGig = await _context.EventGigs.FindAsync(request.EventID);
                if (eventGig == null) return NotFound(new { message = "Event not found." });

                // Check if category exists and belongs to the event
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.categoryID == request.CategoryID && c.eventID == request.EventID);
                if (category == null) return BadRequest(new { message = "Invalid category for this event." });

                // Check if already enrolled
                var existingEnrollment = await _context.EventEnrollments
                    .FirstOrDefaultAsync(e => e.eventID == request.EventID && e.participantID == participant.participantID);
                if (existingEnrollment != null) return Conflict(new { message = "You are already enrolled in this event." });

                var newEnrollment = new EventEnrollment
                {
                    eventID = request.EventID,
                    participantID = participant.participantID,
                    categoryID = request.CategoryID,
                    enrollmentDate = DateTime.Now
                };

                _context.EventEnrollments.Add(newEnrollment);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Enrolment confirmed.", enrollmentId = newEnrollment.enrollmentID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling participant.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // GET: api/enrolments/my-enrolments (Participant Only)
        [HttpGet("enrolments/my-enrolments")]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var participant = await _context.Participants.FirstOrDefaultAsync(p => p.userID == userId);
                if (participant == null) return BadRequest(new { message = "Participant profile not found." });

                var enrollments = await _context.EventEnrollments
                    .Where(e => e.participantID == participant.participantID)
                    .Include(e => e.EventGig)
                    .Include(e => e.Categories)
                    .Select(e => new
                    {
                        e.enrollmentID,
                        e.enrollmentDate,
                        EventName = e.EventGig.eventName,
                        EventDate = e.EventGig.eventDate,
                        EventLocation = e.EventGig.eventLocation,
                        CategoryAge = e.Categories.categoryAge  ,
                        CategoryDistance = e.Categories.categoryDistance
                    })
                    .ToListAsync();

                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving enrolments.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // DELETE: api/enrolments/{id} (Participant Only)
        [HttpDelete("enrolments/{id}")]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> CancelEnrollment(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var participant = await _context.Participants.FirstOrDefaultAsync(p => p.userID == userId);
                if (participant == null) return BadRequest(new { message = "Participant profile not found." });

                var enrollment = await _context.EventEnrollments.FindAsync(id);
                if (enrollment == null) return NotFound(new { message = "Enrolment not found." });

                // Security check: ensure this enrollment belongs to the logged-in participant
                if (enrollment.participantID != participant.participantID)
                    return Forbid();

                _context.EventEnrollments.Remove(enrollment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Enrolment cancelled." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling enrolment.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}