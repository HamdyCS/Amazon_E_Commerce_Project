using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities;

public partial class PaymentsType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    [NotMapped]
    public EnPaymentType enPaymentType => (EnPaymentType)Id;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
