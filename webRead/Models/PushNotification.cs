using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webRead.Models
{
    public class PushNotification
    {
        [Key]  // Marking the NotificationId as the primary key
        [Column("notification_id")] // Mapping the NotificationId to the correct column name in the database
        public int NotificationId { get; set; }

        [Required]
        [Column("title")] // Mapping Title to the correct column
        public string Title { get; set; }

        [Required]
        [Column("message")] // Mapping Message to the correct column
        public string Message { get; set; }

        [Required]
        [Column("target_audience")] // Mapping TargetAudience to the correct column
        public string TargetAudience { get; set; }

        [Column("sent_at")] // Mapping SentAt to the correct column
        public DateTime? SentAt { get; set; }

        [Column("is_sent")] // Mapping IsSent to the correct column
        public bool IsSent { get; set; }

        [Required]
        [Column("notification_type")] // Mapping NotificationType to the correct column
        public string NotificationType { get; set; }

        [Column("created_at")] // Mapping CreatedAt to the correct column
        public DateTime CreatedAt { get; set; }

        [Column("is_read")] // Mapping IsRead to the correct column
        public bool IsRead { get; set; }
    }
}
