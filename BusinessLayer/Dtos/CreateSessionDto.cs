using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class CreateSessionDto
    {

        [Required]
        public PaymentDto paymentDto { get; set; }

        [Required]
        public long PaymentId { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public string SuccessUrl { get; set; }

        [Required]
        public string CancelUrl { get; set; }

        [Required]
        public decimal ShippingCost { get; set; }

    }
}
