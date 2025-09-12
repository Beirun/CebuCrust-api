using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CebuCrust_api.Models
{
    public class User
    {
        [Key] public int UserId { get; set; }
        [Required] public string UserFName { get; set; } = "";
        [Required] public string UserLName { get; set; } = "";
        [Required] public string UserEmail { get; set; } = "";
        public string? UserPhoneNo { get; set; }
        [Required] public string PasswordHash { get; set; } = "";
        public int RoleId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }

        public Role Role { get; set; } = null!;
        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
