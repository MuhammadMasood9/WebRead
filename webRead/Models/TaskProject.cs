// TaskProject.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webRead.Models
{
    public class TaskProject
    {
        [Key]
        [Column("TaskID")]
        public int TaskId { get; set; }

        [Column("ProjectID")]
        [ForeignKey("Project")]
        [Required(ErrorMessage = "The Project selection is required.")]
        public int ProjectId { get; set; } // Property name matches EF convention

        [Required]
        [Column("Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Column("Progress")]
        [Range(0, 100)]
        public decimal Progress { get; set; }

        [Column("Report")]
        public string Report { get; set; }

        [Column("Status")]
        public int Status { get; set; }

        [Column("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        public virtual Project Project { get; set; }
    }
}