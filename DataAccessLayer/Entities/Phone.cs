using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class Phone
{
    public long Id { get; set; }

    public string PhoneNumber { get; set; }

    public string userId { get; set; }

    public virtual User? user { get; set; }
}
