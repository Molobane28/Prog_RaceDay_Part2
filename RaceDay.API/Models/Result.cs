using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        [Required]
        [ForeignKey(nameof(EventEnrolment))]
        public int EnrolmentID { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        public bool Disqualified { get; set; } = false;

        // Navigation property
        public EventEnrolment EventEnrolment { get; set; } = null!;
    }
}