using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [ForeignKey("Event")]
        public int EventID { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public decimal EntryFee { get; set; }
        public string AgeGroup { get; set; } = string.Empty;
        public string GenderRestriction { get; set; } = string.Empty;

        // Navigation Properties
        public Event? Event { get; set; }
        public ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
    }
}