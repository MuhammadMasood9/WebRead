namespace webRead.Models
{
    public class DonationDropdown
    {
        public int ID { get; set; }
        public string? DonationType { get; set; }
        public string? DonationStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}