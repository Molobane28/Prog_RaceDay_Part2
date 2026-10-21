namespace RaceDay.API.DTOs
{
    public class CreateEnrolmentDto
    {
        public int ParticipantID { get; set; }
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public decimal PaymentAmount { get; set; }
        public string Status { get; set; } = "Confirmed";
    }
}