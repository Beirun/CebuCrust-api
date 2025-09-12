using CebuCrust_api.Models;
using System;
namespace CebuCrust_api.Models
{
    public class Favorite
    {
        public int UserId { get; set; }
        public int PizzaId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Pizza Pizza { get; set; } = null!;
    }
}
