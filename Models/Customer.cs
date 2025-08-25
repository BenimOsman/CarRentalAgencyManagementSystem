using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAgencyMngSystem.Models
{
    public class Customer
    {
        [Key]                                                               // Primary Key
        public int CustomerId { get; set; }

        [Required]                                                          // Customer Name
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]                                                      // Ensures valid email format
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]                                                             // Ensures valid phone format
        public string Phone { get; set; } = string.Empty;

        [Required]                                                          // Driver's License Number
        public string LicenseNumber { get; set; } = string.Empty;

        // Navigation property → One customer can have multiple rentals
        public virtual ICollection<Rental>? Rentals { get; set; } = new List<Rental>();

        public Customer() { }                                                 // Parameterless constructor

        public Customer(int customerId, string name, string email, string phone, string licenseNumber)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Phone = phone;
            LicenseNumber = licenseNumber;
        }
    }
}