namespace webRead.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Program { get; set; }
        public string? Project { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; } // Can store base64 strings or be null
        public DateTime CreatedAt { get; set; }
        public string? Others { get; set; }
        public int? Crossponding_id { get; set; }
        public int? TotalAmount { get; set; } // Changed to int based on sample data
        public string? PaymentType { get; set; }
        public string? DontaionType { get; set; } // Keeping the typo to match DB
        public string? OrderNumber { get; set; }
        public string? CurrentCpayId { get; set; }
        public int? Status { get; set; } // Assuming int for status editing
    }
}