using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ProductCategory
{
    public long Id { get; set; }

    public string NameEn { get; set; }

    public string NameAr { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    public string CreatedBy { get; set; }

    public bool IsDeleted {  get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public virtual User? user { get; set; }

    public virtual ICollection<ProductCategoryImage> ProductCategoryImages { get; set; } = new List<ProductCategoryImage>();

    public virtual ICollection<ProductSubCategory> ProductSubCategories { get; set; } = new List<ProductSubCategory>();
}
