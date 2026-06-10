using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class RefundStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? DescriptionAr { get; set; }

        public string? DescriptionEn { get; set; }

        [NotMapped]
        public EnRefundStatus Status => (EnRefundStatus)Id;

        public virtual IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
    }
}
