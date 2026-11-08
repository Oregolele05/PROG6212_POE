using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Filters;
using RaceDay.Models;
using RaceDay.Services;

namespace RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<EventsController> _logger;
        private readonly BlobStorageService _blobService; 

        public EventsController(RaceDayDbContext context, ILogger<EventsController> logger, BlobStorageService blobService)
        {
            _context = context;
            _logger = logger;
            _blobService = blobService;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _context.EventGigs.ToListAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving events.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                var eventGig = await _context.EventGigs.FindAsync(id);
                if (eventGig == null) return NotFound(new { message = "Event not found." });
                return Ok(eventGig);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving event.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // POST: api/events (Organiser Only)
        [HttpPost]
        [Consumes("multipart/form-data")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CreateEvent([FromForm] Event request, IFormFile? imageFile)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                    return Unauthorized(new { message = "Session lost." });

                var organiser = await _context.Organisers.FirstOrDefaultAsync(o => o.userID == userId);
                if (organiser == null)
                    return BadRequest(new { message = "Organiser profile not found.", userId = userId });

                string? imageUrl = null;

                // --- AZURE BLOB UPLOAD LOGIC ---
                if (imageFile != null && imageFile.Length > 0)
                {
                    _logger.LogInformation("API RECEIVED THE IMAGE FILE. Uploading to Azure...");
                    try
                    {
                        imageUrl = await _blobService.UploadFileAsync(imageFile);
                        _logger.LogInformation($"AZURE UPLOAD SUCCESSFUL. URL: {imageUrl}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"AZURE UPLOAD FAILED: {ex.Message}");
                        return StatusCode(500, new { message = "Azure upload failed", error = ex.Message });
                    }
                }
                else
                {
                    _logger.LogWarning("API DID NOT RECEIVE AN IMAGE FILE (imageFile is null or empty).");
                }
                // -------------------------------

                var newEvent = new EventGig
                {
                    eventName = request.EventName,
                    eventDescription = request.EventDescription,
                    eventDate = request.EventDate,
                    eventLocation = request.EventLocation,
                    eventDistance = request.EventDistance,
                    eventType = request.EventType,
                    eventRoute = request.EventRoute,
                    eventMap = request.EventMap,
                    EventImageUrl = imageUrl,
                    organiserID = organiser.organiserID
                };

                _context.EventGigs.Add(newEvent);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Event created successfully.", eventId = newEvent.eventID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event.");
                return StatusCode(500, new
                {
                    message = "An internal server error occurred.",
                    errorType = ex.GetType().FullName,
                    errorMessage = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // PUT: api/events/{id} (Organiser Only)
        [HttpPut("{id}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event request)
        {
            try
            {
                var eventGig = await _context.EventGigs.FindAsync(id);
                if (eventGig == null) return NotFound(new { message = "Event not found." });

                eventGig.eventName = request.EventName;
                eventGig.eventDescription = request.EventDescription;
                eventGig.eventDate = request.EventDate;
                eventGig.eventLocation = request.EventLocation;
                eventGig.eventDistance = request.EventDistance;
                eventGig.eventType = request.EventType;
                eventGig.eventRoute = request.EventRoute;
                eventGig.eventMap = request.EventMap;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Event updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // DELETE: api/events/{id} (Organiser Only)
        [HttpDelete("{id}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var eventGig = await _context.EventGigs.FindAsync(id);
                if (eventGig == null) return NotFound(new { message = "Event not found." });

                _context.EventGigs.Remove(eventGig);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Event deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}