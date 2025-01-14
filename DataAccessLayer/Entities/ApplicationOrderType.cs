using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ApplicationOrderType
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    public EnApplicationOrderType enApplicationOrderType => (EnApplicationOrderType)Id;

    public virtual ICollection<ApplicationOrder> ApplicationOrders { get; set; } = new List<ApplicationOrder>();
}
