using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Filters;
using RaceDay.Models;

namespace RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<EventsController> _logger;

        public EventsController(RaceDayDbContext context, ILogger<EventsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/events
        /// <summary>
        /// Retrieves all upcoming race events. Public access.
        /// </summary>
        /// <returns>200 OK with a list of all events.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves a single event by its ID. Public access.
        /// </summary>
        /// <param name="id">The unique EventID of the race.</param>
        /// <returns>200 OK with event details, 404 Not Found if the event doesn't exist.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Creates a new race event. Requires Organiser role.
        /// </summary>
        /// <param name="request">Event details: name, description, date, location, distance, type, route, and map.</param>
        /// <returns>201 Created, 401 Unauthorized, 403 Forbidden.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CreateEvent([FromBody] Event request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserID");
                var organiser = await _context.Organisers.FirstOrDefaultAsync(o => o.userID == userId);

                if (organiser == null) return BadRequest(new { message = "Organiser profile not found." });

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
                    organiserID = organiser.organiserID // Link to the logged-in organiser
                };

                _context.EventGigs.Add(newEvent);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Event created successfully.", eventId = newEvent.eventID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // PUT: api/events/{id} (Organiser Only)
        /// <summary>
        /// Updates an existing race event. Requires Organiser role.
        /// </summary>
        /// <param name="id">The unique EventID of the event to update.</param>
        /// <param name="request">Updated event details.</param>
        /// <returns>200 OK, 401 Unauthorized, 403 Forbidden, 404 Not Found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Deletes a race event. Requires Organiser role.
        /// </summary>
        /// <param name="id">The unique EventID of the event to delete.</param>
        /// <returns>200 OK, 401 Unauthorized, 403 Forbidden, 404 Not Found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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