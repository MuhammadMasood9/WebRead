﻿using System.ComponentModel.DataAnnotations;

namespace webRead.Models
{
    public class DonationMainHead
    {
        [Key]
        public int MID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Image path cannot exceed 500 characters")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0, 1, ErrorMessage = "Status must be 0 or 1")]
        public int Status { get; set; }
    }
}
