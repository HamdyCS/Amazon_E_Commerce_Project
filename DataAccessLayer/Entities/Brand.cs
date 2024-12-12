using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Brand
{
    public long Id { get; set; }

    public string NameEn { get; set; }

    public string NameAr { get; set; }

    public string CreatedBy { get; set; }

    public byte[] Image { get; set; }

    public virtual User? user { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
