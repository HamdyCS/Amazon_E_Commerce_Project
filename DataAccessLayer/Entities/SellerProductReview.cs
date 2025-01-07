using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class SellerProductReview
{
    public long Id { get; set; }

    public int NumberOfStars { get; set; }

    public string Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; }

    public virtual User? user { get; set; }

    public long SellerProductId { get; set; }

    public virtual SellerProduct? SellerProduct { get; set; }
}
