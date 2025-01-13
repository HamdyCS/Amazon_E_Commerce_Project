using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ApplicationOrdersType
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    public virtual ICollection<ApplicationOrder> ApplicationOrders { get; set; } = new List<ApplicationOrder>();
}
