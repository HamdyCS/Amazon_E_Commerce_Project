using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ProductCategoryImage
{
    public long Id { get; set; }

    public byte[] Image { get; set; }

    public long ProductCategoryId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }
}
