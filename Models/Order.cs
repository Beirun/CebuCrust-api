using CebuCrust_api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CebuCrust_api.Models
{
    public class Order
    {
        [Key] public int OrderId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string? OrderInstruction { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderEstimate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Location Location { get; set; } = null!;
        public ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();
    }
}
