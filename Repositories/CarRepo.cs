using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;

namespace CarRentalAgencyMngSystem.Repositories
{
    public class CarRepo : ICar
    {
        private readonly CarRentalContext _context; // DB context for accessing tables

        // Constructor with dependency injection
        public CarRepo(CarRentalContext context)
        {
            _context = context;
        }

        // Get the total number of cars
        public async Task<int> CountAsync()
        {
            return await _context.Cars.CountAsync();
        }

        // Add a new car to the database
        public async Task AddCar(Car car)
        {
            await _context.Cars.AddAsync(car); // Add car to DbSet
            await _context.SaveChangesAsync(); // Save changes to DB
        }

        // Get a car by ID
        public async Task<Car?> GetCarById(int carId)
        {
            return await _context.Cars.FindAsync(carId); // Find car by primary key
        }

        // Get all cars
        public async Task<IEnumerable<Car>> GetAllCars()
        {
            return await _context.Cars.ToListAsync(); // Convert DbSet to list
        }

        // Update a car by ID
        public async Task UpdateCar(int carId, Car newCar)
        {
            var existingCar = await _context.Cars.FindAsync(carId); // Find existing car
            if (existingCar == null) return;                        // Exit if not found

            // Update properties
            existingCar.Make = newCar.Make;
            existingCar.Model = newCar.Model;
            existingCar.Year = newCar.Year;
            existingCar.LicensePlate = newCar.LicensePlate;
            existingCar.DailyRate = newCar.DailyRate;
            existingCar.IsAvailable = newCar.IsAvailable;

            await _context.SaveChangesAsync(); // Save updates
        }

        // Delete a car by ID
        public async Task DeleteCar(int carId)
        {
            var car = await _context.Cars.FindAsync(carId); // Find car
            if (car == null) return;                        // Exit if not found

            _context.Cars.Remove(car);                       // Remove from DbSet
            await _context.SaveChangesAsync();              // Save deletion
        }

        // Get all available cars
        public async Task<IEnumerable<Car>> GetAvailableCars()
        {
            return await _context.Cars
                                 .Where(c => c.IsAvailable) // Filter available cars
                                 .ToListAsync();             // Convert to list
        }
    }
}