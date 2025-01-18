using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class PaymentPrePaidDto
    {
        public long Id { get; set; }


        [Required, Range(1, double.MaxValue, ErrorMessage = "UserAddressId must be bigger than zero")]
        public long UserAddressId { get; set; }


        [Required, Range(1, double.MaxValue, ErrorMessage = "ShoppingCartId must be bigger than zero")]
        public long ShoppingCartId { get; set; }

        [Required]
        public CardInfoDto CardInfo { get; set; }
    }

}
