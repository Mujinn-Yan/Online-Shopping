using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.DataAccess
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
             
        }
        
        public DbSet<Category> Category { get; set; } 
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<RecentlyView> RecentlyView { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<ShippingDetail> ShippingDetail { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public ClaimsIdentity? UserName { get; set; }
        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any())
            {
                return;   // DB has been seeded
            }

            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@onlinewomanstore.com",
                EmailConfirmed = true
            };

            var result = userManager.CreateAsync(adminUser, "123456").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }
        }
    }
}
