using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string MedicalConditions { get; set; } = string.Empty;

        // Navigation Properties
        public User? User { get; set; }
        public ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
    }
}