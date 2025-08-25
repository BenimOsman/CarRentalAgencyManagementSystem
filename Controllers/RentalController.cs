using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;
using CarRentalAgencyMngSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAgencyMngSystem.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRental _rentalRepo;
        private readonly CarRentalContext _context;

        public RentalController(IRental rentalRepo, CarRentalContext context)
        {
            _rentalRepo = rentalRepo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rentals = await _rentalRepo.GetAllRentals();
            return View(rentals);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rental = await _rentalRepo.GetRentalById(id);
            if (rental == null) return NotFound();
            return View(rental);
        }

        public IActionResult Create()
        {
            PopulateDropdowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,CustomerId,StartDate,EndDate,Status")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                var car = await _context.Cars.FindAsync(rental.CarId);
                if (car == null)
                {
                    ModelState.AddModelError("CarId", "Invalid Car selected.");
                    PopulateDropdowns(rental);
                    return View(rental);
                }

                // Calculate TotalCost
                var days = (rental.EndDate - rental.StartDate).Days;
                if (days < 1) days = 1;
                rental.TotalCost = car.DailyRate * days;

                await _rentalRepo.AddRental(rental);
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(rental);
            return View(rental);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rental = await _rentalRepo.GetRentalById(id);
            if (rental == null) return NotFound();

            PopulateDropdowns(rental);
            return View(rental);
        }

        // POST for Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalId,CarId,CustomerId,StartDate,EndDate,Status")] Rental rental)
        {
            if (id != rental.RentalId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var car = await _context.Cars.FindAsync(rental.CarId);
                    if (car == null)
                    {
                        ModelState.AddModelError("CarId", "Invalid Car selected.");
                        PopulateDropdowns(rental);
                        return View(rental);
                    }

                    // Recalculate TotalCost
                    var days = (rental.EndDate - rental.StartDate).Days;
                    if (days < 1) days = 1;
                    rental.TotalCost = car.DailyRate * days;

                    await _rentalRepo.UpdateRental(id, rental);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _rentalRepo.GetRentalById(id) == null) return NotFound();
                    throw;
                }
            }

            PopulateDropdowns(rental);
            return View(rental);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rental = await _rentalRepo.GetRentalById(id);
            if (rental == null) return NotFound();
            return View(rental);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _rentalRepo.DeleteRental(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns(Rental rental)
        {
            ViewBag.Cars = new SelectList(_context.Cars.ToList(), "CarId", "LicensePlate", rental?.CarId);
            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "CustomerId", "Email", rental?.CustomerId);
            ViewBag.StatusList = Enum.GetValues(typeof(RentalStatus))
                                     .Cast<RentalStatus>()
                                     .Select(s => new SelectListItem { Value = s.ToString(), Text = s.ToString() });
        }
    }
}