using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Product
{
    public long Id { get; set; }

    public string NameEn { get; set; }

    public string NameAr { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    public string Size { get; set; }

    public string Color { get; set; }

    public decimal Height { get; set; }

    public decimal Length { get; set; }

    public long ProductSubCategoryId { get; set; }

    public virtual ProductSubCategory? ProductSubCategory { get; set; }

    public long BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public string CreatedBy { get; set; }

    public virtual User user { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<SellerProduct> SellerProducts { get; set; } = new List<SellerProduct>();
}
