using Microsoft.AspNetCore.Mvc;
using CarRentalAgencyMngSystem.Models;
using CarRentalAgencyMngSystem.Repositories;

namespace CarRentalAgencyMngSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly ICar _carRepo;                                // Repository interface for managing car data

        public CarController(ICar carRepo)                             // Constructor with dependency injection for repository
        {
            _carRepo = carRepo;
        }

        // GET: Cars
        // Displays a list of all cars
        public async Task<IActionResult> Index()
        {
            var cars = await _carRepo.GetAllCars();                    // Fetch all cars from repository
            return View(cars);                                         // Return view with car list
        }

        // GET: Cars/Details/{id}
        // Displays details of a specific car by ID
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carRepo.GetCarById(id);                   // Fetch car by ID
            if (car == null)
            {
                return NotFound();                                     // Return 404 if not found
            }
            return View(car);                                          // Show car details
        }

        // GET: Cars/Create
        // Shows form to create a new car
        public IActionResult Create()
        {
            return View();                                             // Return empty create form
        }

        // POST: Cars/Create
        // Handles submission of new car data
        [HttpPost]
        [ValidateAntiForgeryToken]                                     // Protects against CSRF
        public async Task<IActionResult> Create([Bind("Make,Model,Year,LicensePlate,DailyRate,IsAvailable")] Car car)
        {
            if (ModelState.IsValid)                                    // Validate model state
            {
                await _carRepo.AddCar(car);                            // Add car to repository
                return RedirectToAction(nameof(Index));                // Redirect back to list
            }
            return View(car);                                          // Return form with validation errors
        }

        // GET: Cars/Edit/{id}
        // Displays form to edit an existing car
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carRepo.GetCarById(id);                   // Fetch car by ID
            if (car == null)
            {
                return NotFound();
            }
            return View(car);                                          // Return car data to edit form
        }

        // POST: Cars/Edit/{id}
        // Handles submission of edited car data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Make,Model,Year,LicensePlate,DailyRate,IsAvailable")] Car car)
        {
            if (id != car.CarId)                                       // Ensure ID matches
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _carRepo.UpdateCar(id, car);                     // Update car in repository
                return RedirectToAction(nameof(Index));                // Redirect back to list
            }

            return View(car);                                          // Return form with errors
        }

        // GET: Cars/Delete/{id}
        // Displays confirmation page before deleting a car
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carRepo.GetCarById(id);                   // Fetch car by ID
            if (car == null)
            {
                return NotFound();
            }
            return View(car);                                          // Show delete confirmation page
        }

        // POST: Cars/Delete/{id}
        // Handles confirmation and deletes the car
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _carRepo.GetCarById(id);                   // Confirm car exists
            if (car != null)
            {
                await _carRepo.DeleteCar(id);                          // Delete car from repository
            }
            return RedirectToAction(nameof(Index));                    // Redirect to list after delete
        }

        // GET: Cars/Available
        // Optional: Fetch only available cars for rentals
        public async Task<IActionResult> Available()
        {
            var cars = await _carRepo.GetAvailableCars();              // Fetch cars where IsAvailable = true
            return View("Index", cars);                                // Reuse Index view to display available cars
        }
    }
}