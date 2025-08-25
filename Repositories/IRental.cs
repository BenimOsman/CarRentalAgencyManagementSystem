using CarRentalAgencyMngSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalAgencyMngSystem.Repositories
{
    public interface IRental
    {
        // Get the total number of rentals asynchronously
        Task<int> CountAsync();

        // Add a new rental transaction
        Task<Rental> AddRental(Rental rental);

        // Get a rental by ID
        Task<Rental?> GetRentalById(int rentalId);

        // Get all rentals
        Task<IEnumerable<Rental>> GetAllRentals();

        // Update a rental by ID
        Task<Rental?> UpdateRental(int rentalId, Rental rental);

        // Delete a rental by ID
        Task<Rental?> DeleteRental(int rentalId);

        // Get all active rentals
        Task<IEnumerable<Rental>> GetActiveRentals();
    }
}