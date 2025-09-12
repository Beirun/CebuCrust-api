using CebuCrust_api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CebuCrust_api.Models
{
    public class Notification
    {
        [Key] public int NotificationId { get; set; }
        public int UserId { get; set; }
        [Required] public string NotificationMessage { get; set; } = "";
        public string? NotificationStatus { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
