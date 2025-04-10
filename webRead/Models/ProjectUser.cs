using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webRead.Models
{
    public class ProjectUser
    {
        [Key]
        [Column("ProjectUserID")]
        public int ProjectUserId { get; set; }

        [Column("ProjectID")]
        [ForeignKey("Project")]
        [Required(ErrorMessage = "Project is required.")]
        public int ProjectID { get; set; }

        [Column("UserID")]
        [Required(ErrorMessage = "User is required.")]
        public int? UserId { get; set; } // Keep as a plain int, no navigation

        [Column("TaskID")]
        [ForeignKey("Task")]
        [Required(ErrorMessage = "Task is required.")]
        public int? TaskId { get; set; }

        public virtual Project Project { get; set; }
        public virtual TaskProject Task { get; set; }
    }
}