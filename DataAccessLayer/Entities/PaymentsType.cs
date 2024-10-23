using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class PaymentsType
{
    public long Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
