using CarDealerSystem.Models;
using CarDealerSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<CarImage> CarImages { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //=====================================
            // Employee <-> Identity (One To One)
            //=====================================

            builder.Entity<Employee>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Employee -> Branch
            //=====================================

            builder.Entity<Employee>()
                .HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Car -> Branch
            //=====================================

            builder.Entity<Car>()
                .HasOne(c => c.Branch)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Car -> Supplier
            //=====================================

            builder.Entity<Car>()
                .HasOne(c => c.Supplier)
                .WithMany(s => s.Cars)
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Car -> Images
            //=====================================

            builder.Entity<CarImage>()
                .HasOne(i => i.Car)
                .WithMany(c => c.Images)
                .HasForeignKey(i => i.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            //=====================================
            // Service
            //=====================================

            builder.Entity<Service>()
                .HasOne(s => s.Car)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Service>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Services)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Sales
            //=====================================

            builder.Entity<Sale>()
                .HasOne(s => s.Car)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Sales)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Payment
            //=====================================

            builder.Entity<Sale>()
                .HasOne(s => s.Payment)
                .WithOne(p => p.Sale)
                .HasForeignKey<Payment>(p => p.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            //=====================================
            // Purchases
            //=====================================

            builder.Entity<Purchase>()
                .HasOne(p => p.Car)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Purchase>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Purchases)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            //=====================================
            // Decimal Precision
            //=====================================

            builder.Entity<Car>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            builder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            builder.Entity<Sale>()
                .Property(s => s.SalePrice)
                .HasPrecision(18, 2);

            builder.Entity<Purchase>()
                .Property(p => p.PurchasePrice)
                .HasPrecision(18, 2);

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Service>()
                .Property(s => s.Cost)
                .HasPrecision(18, 2);

            //=====================================
            // Indexes
            //=====================================

            builder.Entity<Car>()
                .HasIndex(c => c.VIN)
                .IsUnique();

            builder.Entity<Customer>()
                .HasIndex(c => c.NationalId)
                .IsUnique();
        }
    }
}