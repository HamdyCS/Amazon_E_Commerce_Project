using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ProductImage
{
    public long Id { get; set; }

    public byte[] Image { get; set; }

    public long ProductId { get; set; }

    public virtual Product? Product { get; set; }
    
}
