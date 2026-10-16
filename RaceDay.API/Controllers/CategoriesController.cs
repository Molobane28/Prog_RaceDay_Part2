using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/categories/event/5 (View categories for a specific event)
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesForEvent(int eventId)
        {
            return await _context.Categories
                .Where(c => c.EventID == eventId)
                .ToListAsync();
        }

        // POST: api/categories
        [HttpPost]
        // [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category created successfully!", categoryId = category.CategoryID });
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}