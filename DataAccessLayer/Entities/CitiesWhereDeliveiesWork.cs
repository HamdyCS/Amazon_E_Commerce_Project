using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class CityWhereDeliveryWork
{
    public long Id { get; set; }

    public long CityId { get; set; }

    public string DeliveryId { get; set; }

    public virtual City? City { get; set; }

    public virtual User? user { get; set; }
}
