using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Payment
{
    public long Id { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? PaymentTypeId { get; set; }

    public virtual ICollection<ApplicationOrder> ApplicationOrders { get; set; } = new List<ApplicationOrder>();

    public virtual PaymentsType? PaymentType { get; set; }
}
