using CebuCrust_api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CebuCrust_api.Models
{
    public class Rating
    {
        [Key] public int RatingId { get; set; }
        public int UserId { get; set; }
        public int PizzaId { get; set; }
        public int RatingValue { get; set; }
        public string? RatingMessage { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }

        public User User { get; set; } = null!;
        public Pizza Pizza { get; set; } = null!;
    }
}
