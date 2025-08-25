using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalAgencyMngSystem.Repositories
{
    public class CustomerRepo : ICustomer
    {
        private readonly CarRentalContext _context; // DB context to access tables

        // Constructor with dependency injection
        public CustomerRepo(CarRentalContext context)
        {
            _context = context;
        }

        // Get the total number of customers
        public async Task<int> CountAsync()
        {
            return await _context.Customers.CountAsync(); // Count rows in Customers table
        }

        // Add a new customer
        public async Task AddCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer); // Add customer to DbSet
            await _context.SaveChangesAsync();           // Save changes to DB
        }

        // Get a customer by ID
        public async Task<Customer?> GetCustomerById(int customerId)
        {
            return await _context.Customers.FindAsync(customerId); // Find by primary key
        }

        // Get all customers
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync(); // Convert DbSet to list
        }

        // Update a customer by ID
        public async Task UpdateCustomer(int customerId, Customer newCustomer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customerId); // Find existing customer
            if (existingCustomer == null) return;                                   // Exit if not found

            // Update properties
            existingCustomer.Name = newCustomer.Name;
            existingCustomer.Email = newCustomer.Email;
            existingCustomer.Phone = newCustomer.Phone;
            existingCustomer.LicenseNumber = newCustomer.LicenseNumber;

            await _context.SaveChangesAsync(); // Save changes to DB
        }

        // Delete a customer by ID
        public async Task DeleteCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId); // Find customer
            if (customer == null) return;                                   // Exit if not found

            _context.Customers.Remove(customer);                             // Remove from DbSet
            await _context.SaveChangesAsync();                               // Commit deletion
        }
    }
}