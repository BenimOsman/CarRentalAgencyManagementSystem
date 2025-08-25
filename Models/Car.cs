using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAgencyMngSystem.Models
{
    public class Car
    {
        [Key]                                                               // Primary Key
        public int CarId { get; set; }

        [Required]                                                          // Car Make (e.g., Toyota, Honda)
        public string Make { get; set; } = string.Empty;

        [Required]                                                          // Car Model (e.g., Civic, Corolla)
        public string Model { get; set; } = string.Empty;

        [Required]                                                          // Year of manufacture
        public int Year { get; set; }

        [Required]                                                          // Unique License Plate
        public string LicensePlate { get; set; } = string.Empty;

        [Required]                                                          // Daily rental rate
        [Range(0, double.MaxValue)]
        public decimal DailyRate { get; set; }

        public bool IsAvailable { get; set; } = true;                        // Availability status

        // Navigation property → One car can have multiple rentals
        public virtual ICollection<Rental>? Rentals { get; set; } = new List<Rental>();

        public Car() { }                                                     // Parameterless constructor

        public Car(int carId, string make, string model, int year, string licensePlate, decimal dailyRate, bool isAvailable)
        {
            CarId = carId;
            Make = make;
            Model = model;
            Year = year;
            LicensePlate = licensePlate;
            DailyRate = dailyRate;
            IsAvailable = isAvailable;
        }
    }
}