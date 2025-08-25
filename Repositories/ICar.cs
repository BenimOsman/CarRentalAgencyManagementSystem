using CarRentalAgencyMngSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalAgencyMngSystem.Repositories
{
    public interface ICar
    {
        // Get the total number of cars asynchronously
        Task<int> CountAsync();

        // Add a new car to the database
        Task AddCar(Car car);

        // Get a car by its ID
        Task<Car?> GetCarById(int carId);

        // Get all cars from the database
        Task<IEnumerable<Car>> GetAllCars();

        // Update an existing car by ID
        Task UpdateCar(int carId, Car car);

        // Delete a car by ID
        Task DeleteCar(int carId);

        // Get all available cars for rental
        Task<IEnumerable<Car>> GetAvailableCars();
    }
}