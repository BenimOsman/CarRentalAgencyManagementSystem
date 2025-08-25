using CarRentalAgencyMngSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApCarRental.Repositories
{
    public interface IPayment
    {
        Task<int> CountAsync();
        Task<Payment> AddPayment(Payment payment);
        Task<Payment?> GetPaymentById(int paymentId);
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment?> UpdatePayment(int paymentId, Payment payment);
        Task<Payment?> DeletePayment(int paymentId);
        Task<IEnumerable<Payment>> GetPaymentsByRentalId(int rentalId);
    }
}
