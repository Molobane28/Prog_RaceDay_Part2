using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.DTOs;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/enrolments (Participant enters an event)
        [HttpPost]
        // [Authorize(Roles = "Participant")]
        public async Task<ActionResult<EventEnrolment>> CreateEnrolment([FromBody] CreateEnrolmentDto request)
        {
            // Map the DTO to the actual database model
            var enrolment = new EventEnrolment
            {
                ParticipantID = request.ParticipantID,
                CategoryID = request.CategoryID,
                EventID = request.EventID,
                PaymentStatus = request.PaymentStatus,
                PaymentAmount = request.PaymentAmount,
                Status = request.Status,
                EnrolmentDate = DateTime.UtcNow
            };

            _context.EventEnrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Successfully enrolled in the event!", enrolmentId = enrolment.EnrolmentID });
        }

        // GET: api/enrolments/event/5 (Organiser views all enrolments for their event)
        [HttpGet("event/{eventId}")]
        // [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<IEnumerable<EventEnrolment>>> GetEnrolmentsForEvent(int eventId)
        {
            return await _context.EventEnrolments
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .Where(e => e.EventID == eventId)
                .ToListAsync();
        }

        // GET: api/enrolments/participant/5 (Participant views their own enrolments)
        [HttpGet("participant/{participantId}")]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<EventEnrolment>>> GetEnrolmentsForParticipant(int participantId)
        {
            return await _context.EventEnrolments
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Where(e => e.ParticipantID == participantId)
                .ToListAsync();
        }
    }
}