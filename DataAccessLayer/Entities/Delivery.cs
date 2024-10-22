using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Delivery
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public virtual User? user { get; set; }

    public virtual ICollection<ApplicationOrder> ApplicationOrders { get; set; } = new List<ApplicationOrder>();

    public virtual ICollection<CitiesWhereDeliveiesWork> CitiesWhereDeliveiesWorks { get; set; } = new List<CitiesWhereDeliveiesWork>();
}
