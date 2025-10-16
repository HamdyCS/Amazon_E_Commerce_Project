using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class PaymentStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? DescriptionAr { get; set; }

        public string? DescriptionEn { get; set; }

        [NotMapped]
        public EnPaymentStatus status => (EnPaymentStatus)Id;

        public virtual IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
    }
}
