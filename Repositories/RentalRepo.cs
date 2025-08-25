using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalAgencyMngSystem.Repositories
{
    public class RentalRepo : IRental
    {
        private readonly CarRentalContext _context; // DB context for accessing Rentals

        public RentalRepo(CarRentalContext context) // Constructor with DI
        {
            _context = context;
        }

        // Get total number of rentals
        public async Task<int> CountAsync()
        {
            return await _context.Rentals.CountAsync(); // Count all rentals in DB
        }

        // Add a new rental
        public async Task<Rental> AddRental(Rental rental)
        {
            await _context.Rentals.AddAsync(rental); // Add rental to DbSet
            await _context.SaveChangesAsync();       // Commit changes
            return rental;                           // Return added rental
        }

        // Get a rental by ID including Car and Customer info
        public async Task<Rental?> GetRentalById(int rentalId)
        {
            return await _context.Rentals
                                 .Include(r => r.Car)       // Include Car info
                                 .Include(r => r.Customer)  // Include Customer info
                                 .FirstOrDefaultAsync(r => r.RentalId == rentalId); // Filter by ID
        }

        // Get all rentals with Car and Customer info
        public async Task<IEnumerable<Rental>> GetAllRentals()
        {
            return await _context.Rentals
                                 .Include(r => r.Car)
                                 .Include(r => r.Customer)
                                 .OrderBy(r => r.StartDate) // Optional: order by start date
                                 .ToListAsync();            // Convert to List
        }

        // Update rental by ID
        public async Task<Rental?> UpdateRental(int rentalId, Rental newRental)
        {
            var rental = await _context.Rentals.FindAsync(rentalId); // Find rental
            if (rental == null) return null;                         // Exit if not found

            rental.CarId = newRental.CarId;                           // Update Car
            rental.CustomerId = newRental.CustomerId;                 // Update Customer
            rental.StartDate = newRental.StartDate;                   // Update StartDate
            rental.EndDate = newRental.EndDate;                       // Update EndDate
            rental.TotalCost = newRental.TotalCost;                   // Update TotalCost
            rental.Status = newRental.Status;                         // Update Status

            _context.Rentals.Update(rental);                          // Mark entity as updated
            await _context.SaveChangesAsync();                        // Save changes
            return rental;                                           // Return updated rental
        }

        // Delete rental by ID
        public async Task<Rental?> DeleteRental(int rentalId)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);  // Find rental
            if (rental == null) return null;                         // Exit if not found

            _context.Rentals.Remove(rental);                         // Remove rental
            await _context.SaveChangesAsync();                       // Commit deletion
            return rental;                                           // Return deleted rental
        }

        // Get all active rentals (Status = "Active") using queryable filter
        public async Task<IEnumerable<Rental>> GetActiveRentals()
        {
            var query = _context.Rentals
                                .Include(r => r.Car)       // Include Car info
                                .Include(r => r.Customer)  // Include Customer info
                                .Where(r => r.Status.Equals("Active")); // Filter Active rentals safely

            return await query.ToListAsync();                        // Execute query and convert to List
        }
    }
}