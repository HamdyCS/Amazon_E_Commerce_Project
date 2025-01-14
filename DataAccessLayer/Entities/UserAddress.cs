using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class UserAddress
{
    public long Id { get; set; }

    public string Address { get; set; }

    public long CityId { get; set; }

    public string UserId { get; set; }

    public virtual User? user { get; set; }

    public virtual City? City { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeleted { get; set; }

    public virtual IEnumerable<Payment> Payments { get; set; } = new List<Payment>();


}
