using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class EventEnrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        [Required]
        [ForeignKey(nameof(Participant))]
        public int ParticipantID { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }

        [Required]
        [ForeignKey(nameof(Event))]
        public int EventID { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PaymentAmount { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;

        // Navigation properties
        public Participant Participant { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public Event Event { get; set; } = null!;
        public Result? Result { get; set; }
    }
}