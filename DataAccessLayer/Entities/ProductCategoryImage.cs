namespace DataAccessLayer.Entities;

public partial class ProductCategoryImage
{
    public long Id { get; set; }

    public string ImageUrl { get; set; }

    public string PublicId { get; set; }

    public long ProductCategoryId { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }
}
