using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApCarRental.Repositories;

namespace CarRentalAgencyMngSystem.Repositories
{
    public class PaymentRepo : IPayment
    {
        private readonly CarRentalContext _context;

        public PaymentRepo(CarRentalContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync() => await _context.Payments.CountAsync();

        public async Task<Payment> AddPayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetPaymentById(int paymentId)
        {
            return await _context.Payments
                                 .Include(p => p.Rental)
                                    .ThenInclude(r => r.Car)
                                 .Include(p => p.Rental)
                                    .ThenInclude(r => r.Customer)
                                 .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _context.Payments
                                 .Include(p => p.Rental)
                                    .ThenInclude(r => r.Car)
                                 .Include(p => p.Rental)
                                    .ThenInclude(r => r.Customer)
                                 .OrderBy(p => p.PaymentDate)
                                 .ToListAsync();
        }

        public async Task<Payment?> UpdatePayment(int paymentId, Payment newPayment)
        {
            var existing = await _context.Payments.FindAsync(paymentId);
            if (existing == null) return null;

            existing.RentalId = newPayment.RentalId;
            existing.Amount = newPayment.Amount;
            existing.PaymentDate = newPayment.PaymentDate;
            existing.Method = newPayment.Method;

            _context.Payments.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Payment?> DeletePayment(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return null;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByRentalId(int rentalId)
        {
            return await _context.Payments
                                 .Where(p => p.RentalId == rentalId)
                                 .Include(p => p.Rental)
                                    .ThenInclude(r => r.Car)
                                 .Include(p => p.Rental)
                                    .ThenInclude(r => r.Customer)
                                 .OrderBy(p => p.PaymentDate)
                                 .ToListAsync();
        }
    }
}
