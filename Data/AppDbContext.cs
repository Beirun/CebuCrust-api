using Microsoft.EntityFrameworkCore;
using CebuCrust_api.Models;

namespace CebuCrust_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Pizza> Pizzas => Set<Pizza>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderList> OrderLists => Set<OrderList>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Favorite>().HasKey(f => new { f.UserId, f.PizzaId });
            mb.Entity<Cart>().HasKey(c => new { c.PizzaId, c.UserId });
            mb.Entity<OrderList>().HasKey(ol => new { ol.OrderId, ol.PizzaId });


            // Role seed
            mb.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "User" }
            );
        }
    }
}
