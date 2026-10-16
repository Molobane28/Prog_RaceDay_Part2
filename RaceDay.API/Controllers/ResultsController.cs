using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/results (Organiser captures a result for a participant's enrolment)
        [HttpPost]
        // [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Result>> CreateResult([FromBody] Result result)
        {
            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Result captured successfully!", resultId = result.ResultID });
        }

        // GET: api/results/participant/5 (Participant views their own results)
        [HttpGet("participant/{participantId}")]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<Result>>> GetResultsForParticipant(int participantId)
        {
            return await _context.Results
                .Include(r => r.EventEnrolment)
                    .ThenInclude(e => e.Event)
                .Include(r => r.EventEnrolment)
                    .ThenInclude(e => e.Category)
                .Where(r => r.EventEnrolment != null && r.EventEnrolment.ParticipantID == participantId)
                .ToListAsync();
        }
    }
}