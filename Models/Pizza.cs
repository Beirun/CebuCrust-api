using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CebuCrust_api.Models
{
    public class Pizza
    {
        [Key] public int PizzaId { get; set; }
        [Required] public string PizzaName { get; set; } = "";
        public string? PizzaDescription { get; set; }
        public string? PizzaCategory { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PizzaPrice { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();
    }
}
