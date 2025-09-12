using CebuCrust_api.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CebuCrust_api.Models
{
    public class Role
    {
        [Key] public int RoleId { get; set; }
        [Required] public string RoleName { get; set; } = "";

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
