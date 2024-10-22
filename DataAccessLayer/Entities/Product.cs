using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Product
{
    public long Id { get; set; }

    public string? NameEn { get; set; }

    public string? NameAr { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public decimal? Height { get; set; }

    public decimal? Length { get; set; }

    public string? UserId { get; set; }

    public virtual User? user { get; set; }

    public long? ProductCategoryId { get; set; }

    public long? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<SellerProduct> SellerProducts { get; set; } = new List<SellerProduct>();
}
