using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalAgencyMngSystem.Models
{
    public enum RentalStatus
    {
        Booked,
        Active,
        Completed,
        Cancelled
    }

    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalCost { get; set; }

        [Required]
        public RentalStatus Status { get; set; } = RentalStatus.Booked;

        public virtual Car? Car { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; } = new List<Payment>();

        public Rental() { }

        public Rental(int rentalId, int carId, int customerId, DateTime startDate, DateTime endDate, decimal totalCost, RentalStatus status)
        {
            RentalId = rentalId;
            CarId = carId;
            CustomerId = customerId;
            StartDate = startDate;
            EndDate = endDate;
            TotalCost = totalCost;
            Status = status;
        }

        // Method to calculate total cost based on car's DailyRate and rental duration
        //public void CalculateTotalCost()
        //{
        //    if (Car != null)
        //    {
        //        var days = (EndDate - StartDate).Days;
        //        if (days < 1) days = 1;
        //        TotalCost = Car.DailyRate * days;
        //    }
        //}
    }
}