using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Phone
{
    public long Id { get; set; }

    public string? PhoneNumber { get; set; }

    public long? PersonId { get; set; }

    public virtual Person? Person { get; set; }
}
