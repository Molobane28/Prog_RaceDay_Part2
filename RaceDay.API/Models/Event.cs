using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [ForeignKey(nameof(Organiser))]
        public int OrganiserID { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? EventDescription { get; set; }

        [Required]
        public DateOnly EventDate { get; set; }

        [Required]
        public DateOnly RegistrationDeadline { get; set; }

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public int MaxParticipants { get; set; }

        // Navigation properties
        public Organiser Organiser { get; set; } = null!;
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<EventEnrolment> EventEnrolments { get; set; } = new List<EventEnrolment>();
    }
}