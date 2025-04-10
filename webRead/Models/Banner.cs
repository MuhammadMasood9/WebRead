
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webRead.Models
{
  

        public class Banner
        {
            [Key]
            [Column("banner_id")]
            public int BannerId { get; set; }

            [Column("admin_id")]
            public int AdminId { get; set; }

            [Required]
            [Column("title")]
            public string Title { get; set; }

            [Column("description")]
            public string Description { get; set; }

            [Column("img_url")]
            public string ImageUrl { get; set; }  // This will store the base64 string

            [Column("banner_link")]
            public string BannerLink { get; set; }

            [Column("is_active")]
            public bool IsActive { get; set; }

            [Column("created_at")]
            public DateTime CreatedAt { get; set; }

            [Column("updated_at")]
            public DateTime? UpdatedAt { get; set; }
        }
    
}
