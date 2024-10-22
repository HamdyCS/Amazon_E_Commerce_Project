using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class CitiesWhereDeliveiesWork
{
    public long Id { get; set; }

    public long? CityId { get; set; }

    public long? DeliveryId { get; set; }

    public virtual City? City { get; set; }

    public virtual Delivery? Delivery { get; set; }
}
