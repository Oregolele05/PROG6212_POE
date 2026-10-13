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
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(RaceDayDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/events/{eventID}/categories
        /// <summary>
        /// Retrieves all categories belonging to a specific event. Public access.
        /// </summary>
        /// <param name="eventID">The ID of the event to fetch categories for.</param>
        /// <returns>200 OK with list of categories, 404 Not Found if the event doesn't exist.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("events/{eventID}/categories")]
        public async Task<IActionResult> GetCategoriesForEvent(int eventID)
        {
            try
            {
                var eventExists = await _context.EventGigs.AnyAsync(e => e.eventID == eventID);
                if (!eventExists) return NotFound(new { message = "Event not found." });

                var categories = await _context.Categories
                    .Where(c => c.eventID == eventID)
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // POST: api/events/{eventID}/categories (Organiser Only)
        /// <summary>
        /// Adds a new category (age bracket and distance) to a specific event. Requires Organiser role.
        /// </summary>
        /// <param name="eventID">The event to attach the category to.</param>
        /// <param name="request">Category details (age bracket and distance).</param>
        /// <returns>201 Created, 401 Unauthorized, 403 Forbidden, 404 Not Found.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("events/{eventID}/categories")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CreateCategory(int eventID, [FromBody] Category request)
        {
            try
            {
                var eventGig = await _context.EventGigs.FindAsync(eventID);
                if (eventGig == null) return NotFound(new { message = "Event not found." });

                var newCategory = new Categories
                {
                    categoryAge = request.CategoryAge,
                    categoryDistance = request.CategoryDistance,
                    eventID = eventID
                };

                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Category created successfully.", categoryId = newCategory.categoryID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        // DELETE: api/categories/{id} (Organiser Only)
        /// <summary>
        /// Deletes a specific category. Requires Organiser role.
        /// </summary>
        /// <param name="id">The unique CategoryID to delete.</param>
        /// <returns>200 OK, 401 Unauthorized, 403 Forbidden, 404 Not Found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("categories/{id}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) return NotFound(new { message = "Category not found." });

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
    }
}