using Microsoft.EntityFrameworkCore;
using CarRentalAgencyMngSystem.Models;

namespace CarRentalAgencyMngSystem.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Rental> Rentals { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Car → Rental: One-to-Many
            modelBuilder.Entity<Car>()
                .HasMany(c => c.Rentals)
                .WithOne(r => r.Car)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete if car is deleted

            // Customer → Rental: One-to-Many
            modelBuilder.Entity<Customer>()
                .HasMany(cu => cu.Rentals)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete if customer is deleted

            // Rental → Payment: One-to-Many
            modelBuilder.Entity<Rental>()
                .HasMany(r => r.Payments)
                .WithOne(p => p.Rental)
                .HasForeignKey(p => p.RentalId)
                .OnDelete(DeleteBehavior.Cascade); // deleting rental deletes associated payments

            // Store enums as strings
            modelBuilder.Entity<Rental>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Method)
                .HasConversion<string>();
        }
    }
}