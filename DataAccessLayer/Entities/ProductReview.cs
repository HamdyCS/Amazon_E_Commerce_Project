using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccessLayer.Entities;

public partial class ProductReview
{
    public long Id { get; set; }

    public int NumberOfStars { get; set; }

    public string Message { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; }

    public virtual User? user { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public long ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
