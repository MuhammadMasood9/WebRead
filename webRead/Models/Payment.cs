using System;
using System.ComponentModel.DataAnnotations;

namespace webRead.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "Donation Head")]
        public string? DonationHead { get; set; }

        public string? Amount { get; set; }

        public string? Image { get; set; } // Likely a path or base64 string

        public string? Description { get; set; }

        [Display(Name = "Total Amount Paid")]
        public decimal? TotalAmountPaid { get; set; }

        [Display(Name = "User Correspondence ID")]
        public string? UserCorrespondenceId { get; set; }

        [Display(Name = "Payment Type")]
        public string? PaymentType { get; set; }

        [Display(Name = "Current CPay ID")]
        public string? CurrentCpayId { get; set; }

        [Display(Name = "Order Number")]
        public string? OrderNumber { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Status")]
        [Range(0, 2, ErrorMessage = "Status must be 0 (UnPaid), 1 (Paid), or 2 (Rejected).")]
        public int? Status { get; set; }
    }
}