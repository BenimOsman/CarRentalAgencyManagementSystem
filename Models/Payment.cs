using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAgencyMngSystem.Models
{
    public enum PaymentMethod
    {
        Cash,
        Card,
        Online
    }

    public class Payment
    {
        [Key]  // Primary Key
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Rental")]
        public int RentalId { get; set; }  // FK → Rental

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }  // Payment amount

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;  // Payment date

        [Required]
        public PaymentMethod Method { get; set; }  // Payment method

        // Navigation property
        public virtual Rental? Rental { get; set; }

        public Payment() { }

        public Payment(int paymentId, int rentalId, decimal amount, DateTime paymentDate, PaymentMethod method)
        {
            PaymentId = paymentId;
            RentalId = rentalId;
            Amount = amount;
            PaymentDate = paymentDate;
            Method = method;
        }
    }
}
