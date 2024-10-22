using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ApplicationType
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
