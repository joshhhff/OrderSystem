using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Models;

namespace OrderSystemApp.Data
{
    public class SystemContext : DbContext
    {
        public SystemContext (DbContextOptions<SystemContext> options)
            : base(options)
        {
        }

        public DbSet<OrderSystemApp.Models.Customer> Customer { get; set; }
        public DbSet<OrderSystemApp.Models.Order> Order { get; set; }
        public DbSet<OrderSystemApp.Models.OrderLine> OrderLine { get; set; }
        public DbSet<OrderSystemApp.Models.Role> Role { get; set; }
        public DbSet<OrderSystemApp.Models.OrderLine> User { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderLine>().ToTable("OrderLine");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");
        }
        public DbSet<OrderSystemApp.Models.Product> Product { get; set; }
    }
}
