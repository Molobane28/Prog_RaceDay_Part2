using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [ForeignKey(nameof(Event))]
        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Distance { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal EntryFee { get; set; }

        [Required]
        [StringLength(50)]
        public string AgeGroup { get; set; } = string.Empty;

        [StringLength(20)]
        public string? GenderRestriction { get; set; }

        // Navigation properties
        public Event Event { get; set; } = null!;
        public ICollection<EventEnrolment> EventEnrolments { get; set; } = new List<EventEnrolment>();
    }
}