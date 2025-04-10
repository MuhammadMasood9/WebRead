namespace webRead.Models
{
    public class TaxCertificate
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Status { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
    }
}