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

    [Required]
    public string UserId { get; set; }

    public virtual User? user { get; set; }

    public virtual City? City { get; set; }

  
}
