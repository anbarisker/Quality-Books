using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualityBooks.Models;

namespace QualityBooks.Data
{
    public class QualityBooksContext : IdentityDbContext<ApplicationUser>
    {
        public QualityBooksContext(DbContextOptions<QualityBooksContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Supplier>().ToTable("Suppliers");
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<CartItem>().ToTable("CartItems");
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<OrderDetail>().ToTable("OrderDetails");

            builder.Entity<OrderDetail>().HasOne(p => p.Order).WithMany(o => o.OrderDetails).OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<QualityBooks.Models.ShoppingCart> ShoppingCart { get; set; }
        public DbSet<QualityBooks.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
