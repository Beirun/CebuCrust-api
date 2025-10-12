using CebuCrust_api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CebuCrust_api.Models
{
    public class Location
    {
        [Key] public int LocationId { get; set; }
        public int UserId { get; set; }
        [Required] public string LocationCity { get; set; } = "";
        public string? LocationBrgy { get; set; }
        public string? LocationStreet { get; set; }
        public string? LocationHouseNo { get; set; }
        public string? LocationPostal { get; set; }
        public string? LocationLandmark { get; set; }
        public bool IsDefault { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
