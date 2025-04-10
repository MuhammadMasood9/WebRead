// Project.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace webRead.Models
{
    public class Project
    {
        [Column("ProjectID")]
        public int ProjectId { get; set; }

        [Column("ProjectName")]
        public string ProjectName { get; set; }

        [Column("ProjectDescription")]
        public string ProjectDescription { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("Image")]
        public string Image { get; set; }

        [Column("Status")]
        public int Status { get; set; }

        [Column("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [Column("UpdatedAt")]

        public DateTime? UpdatedAt { get; set; }

        // Make DateTime nullable to handle NULL values in database



        public virtual ICollection<TaskProject> Tasks { get; set; } = new List<TaskProject>();
    }
}