using CarRentalAgencyMngSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalAgencyMngSystem.Repositories
{
    public interface ICustomer
    {
        // Get the total number of customers asynchronously
        Task<int> CountAsync();

        // Add a new customer
        Task AddCustomer(Customer customer);

        // Get a customer by ID
        Task<Customer?> GetCustomerById(int customerId);

        // Get all customers
        Task<IEnumerable<Customer>> GetAllCustomers();

        // Update an existing customer by ID
        Task UpdateCustomer(int customerId, Customer customer);

        // Delete a customer by ID
        Task DeleteCustomer(int customerId);
    }
}