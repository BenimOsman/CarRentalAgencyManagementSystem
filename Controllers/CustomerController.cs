using CarRentalAgencyMngSystem.Models;
using CarRentalAgencyMngSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAgencyMngSystem.Controllers
{
    public class CustomerController : Controller                                // Controller to handle HTTP requests for Customer operations
    {
        private readonly ICustomer _customerRepo;                               // Repository interface for customer data access

        public CustomerController(ICustomer customerRepo)                       // Constructor injection for repository
        {
            _customerRepo = customerRepo;
        }

        // GET: Customers
        // Displays a list of all customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerRepo.GetAllCustomers();              // Fetch all customers
            return View(customers);                                             // Return view with customer list
        }

        // GET: Customers/Details/{id}
        // Displays the details of a specific customer by ID
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerRepo.GetCustomerById(id);             // Get customer by ID
            if (customer == null)
            {
                return NotFound();                                              // Return 404 if not found
            }
            return View(customer);                                              // Show customer details
        }

        // GET: Customers/Create
        // Shows form to create a new customer
        public IActionResult Create()
        {
            return View();                                                      // Return empty form for customer creation
        }

        // POST: Customers/Create
        // Handles submission of a new customer
        [HttpPost]
        [ValidateAntiForgeryToken]                                              // Protects against CSRF
        public async Task<IActionResult> Create([Bind("Name,Email,Phone,LicenseNumber")] Customer customer)
        {
            if (ModelState.IsValid)                                             // Validate model input
            {
                await _customerRepo.AddCustomer(customer);                      // Add customer to repository
                return RedirectToAction(nameof(Index));                         // Redirect to list
            }
            return View(customer);                                              // Return form with validation errors
        }

        // GET: Customers/Edit/{id}
        // Displays form to edit an existing customer
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepo.GetCustomerById(id);             // Fetch customer by ID
            if (customer == null)
            {
                return NotFound();                                              // Return 404 if not found
            }
            return View(customer);                                              // Show edit form
        }

        // POST: Customers/Edit/{id}
        // Handles submission of edited customer data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Email,Phone,LicenseNumber")] Customer customer)
        {
            if (id != customer.CustomerId)                                      // Ensure IDs match
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _customerRepo.UpdateCustomer(id, customer);           // Update in repository
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _customerRepo.GetCustomerById(id) == null)        // Check if still exists
                    {
                        return NotFound();
                    }
                    throw;                                                      // Re-throw if other issue
                }
                return RedirectToAction(nameof(Index));                         // Redirect after update
            }
            return View(customer);                                              // Return form with validation errors
        }

        // GET: Customers/Delete/{id}
        // Shows confirmation page before deleting a customer
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerRepo.GetCustomerById(id);             // Fetch by ID
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);                                              // Show confirmation page
        }

        // POST: Customers/Delete/{id}
        // Handles confirmation and deletes the customer
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _customerRepo.GetCustomerById(id);             // Confirm existence
            if (customer != null)
            {
                await _customerRepo.DeleteCustomer(id);                         // Delete from repository
            }
            return RedirectToAction(nameof(Index));                             // Redirect to list
        }
    }
}