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
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<ResultsController> _logger;

        public ResultsController(RaceDayDbContext context, ILogger<ResultsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/events/{eventId}/results (Organiser Only)
        [HttpPost("events/{eventId}/results")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CaptureResult(int eventId, [FromBody] ResultDTO request)
        {
            try
            {
                // 1. Verify the event exists
                var eventGig = await _context.EventGigs.FindAsync(eventId);
                if (eventGig == null) return NotFound(new { message = "Event not found." });

                // 2. Verify the participant exists
                var participant = await _context.Participants.FindAsync(request.ParticipantID);
                if (participant == null) return NotFound(new { message = "Participant not found." });

                // 3. Verify the participant is actually enrolled in this event
                var enrollment = await _context.EventEnrollments
                    .FirstOrDefaultAsync(e => e.eventID == eventId && e.participantID == request.ParticipantID);
                if (enrollment == null) return NotFound(new { message = "Participant enrollment not found for this event." });

                // 4. Create the result
                var newResult = new Result
                {
                    eventID = eventId,
                    participantID = request.ParticipantID,
                    participantTime = request.ParticipantTime,
                    participantPosition  = request.ParticipantPosition
                };

                _context.Result.Add(newResult);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Result captured successfully.", resultId = newResult.resultID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error capturing result.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // GET: api/events/{eventId}/results (Public)
        [HttpGet("events/{eventId}/results")]
        public async Task<IActionResult> GetEventLeaderboard(int eventId)
        {
            try
            {
                var eventExists = await _context.EventGigs.AnyAsync(e => e.eventID == eventId);
                if (!eventExists) return NotFound(new { message = "Event not found." });

                var leaderboard = await _context.Result
                    .Where(r => r.eventID == eventId)
                    .Include(r => r.Participant)
                    .OrderBy(r => r.participantPosition)
                    .Select(r => new
                    {
                        r.resultID,
                        ParticipantName = r.Participant.participantName,
                        r.participantPosition,
                        r.participantTime
                    })
                    .ToListAsync();

                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving leaderboard.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // GET: api/results/my-results (Participant Only)
        [HttpGet("results/my-results")]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var participant = await _context.Participants.FirstOrDefaultAsync(p => p.userID == userId);
                if (participant == null) return BadRequest(new { message = "Participant profile not found." });

                var myResults = await _context.Result
                    .Where(r => r.participantID == participant.participantID)
                    .Include(r => r.EventGig)
                    .Select(r => new
                    {
                        r.resultID,
                        EventName = r.EventGig.eventName,
                        EventDate = r.EventGig.eventDate,
                        EventLocation = r.EventGig.eventLocation,
                        r.participantPosition,
                        r.participantTime
                    })
                    .ToListAsync();

                return Ok(myResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving participant results.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

    }
}