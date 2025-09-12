using CebuCrust_api.Models;
using System;
namespace CebuCrust_api.Models
{
    public class Cart
    {
        public int PizzaId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }

        public User User { get; set; } = null!;
        public Pizza Pizza { get; set; } = null!;
    }
}
