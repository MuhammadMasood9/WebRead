using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webRead.Models
{
    public class Donor
    {
        [Key]
        [Column("Correspondence ID")]
        public int CorrespondenceId { get; set; }

        [Column("Name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Column("Mobile")]
        [StringLength(20)]
        public string? Mobile { get; set; }

        [Column("Email1")]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

      
    }
}