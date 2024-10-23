using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class PersonAddress
{
    public long Id { get; set; }

    public string Address { get; set; }

    public long CityId { get; set; }

    public long PersonId { get; set; }

    public virtual City? City { get; set; }

    public virtual Person? Person { get; set; }
}
