using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ShippingCost
{
    public long Id { get; set; }

    public decimal Price { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? user { get; set; }

    public long CityId { get; set; }

    public virtual City? City { get; set; }
}
