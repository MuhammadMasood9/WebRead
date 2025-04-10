using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webRead.Models
{
    public class Feedback
    {
        [Key]
        [Column("feedback_id")]
        public int FeedbackId { get; set; }

        [Column("crosponding_id")]
        public int CrospondingId { get; set; } // Assuming this is a foreign key or identifier

        [Required]
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Column("email")]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("rating")]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Column("submited_at")]
        public DateTime SubmitedAt { get; set; }
    }
}