using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalAgencyMngSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly CarRentalContext _context;

        public PaymentController(CarRentalContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = _context.Payments
                                   .Include(p => p.Rental)
                                   .ThenInclude(r => r.Car)
                                   .Include(p => p.Rental)
                                   .ThenInclude(r => r.Customer)
                                   .OrderBy(p => p.PaymentDate);

            return View(await payments.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _context.Payments
                                        .Include(p => p.Rental)
                                        .ThenInclude(r => r.Car)
                                        .Include(p => p.Rental)
                                        .ThenInclude(r => r.Customer)
                                        .FirstOrDefaultAsync(m => m.PaymentId == id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            PopulateRentalsDropdown();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalId,Amount,PaymentDate,Method")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                // Auto-set Amount from Rental
                var rental = await _context.Rentals.FindAsync(payment.RentalId);
                if (rental == null)
                {
                    ModelState.AddModelError("RentalId", "Selected rental does not exist.");
                    PopulateRentalsDropdown();
                    return View(payment);
                }

                payment.Amount = rental.TotalCost;      // Set amount automatically
                payment.PaymentDate = DateTime.Now;

                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateRentalsDropdown();
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            PopulateRentalsDropdown(payment.RentalId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,RentalId,Amount,PaymentDate,Method")] Payment payment)
        {
            if (id != payment.PaymentId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var rental = await _context.Rentals.FindAsync(payment.RentalId);
                    if (rental != null)
                        payment.Amount = rental.TotalCost;

                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Payments.Any(e => e.PaymentId == payment.PaymentId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateRentalsDropdown(payment.RentalId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.Payments
                                        .Include(p => p.Rental)
                                        .ThenInclude(r => r.Car)
                                        .Include(p => p.Rental)
                                        .ThenInclude(r => r.Customer)
                                        .FirstOrDefaultAsync(m => m.PaymentId == id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper: populate Rentals dropdown
        private void PopulateRentalsDropdown(int? selectedId = null)
        {
            var rentals = _context.Rentals
                                  .Include(r => r.Car)
                                  .Include(r => r.Customer)
                                  .ToList();

            ViewBag.Rentals = new SelectList(rentals.Select(r => new
            {
                r.RentalId,
                DisplayText = $"{r.RentalId} - {r.Customer.Name} ({r.Car.Model}) - {r.TotalCost:C}"
            }), "RentalId", "DisplayText", selectedId);
        }
    }
}