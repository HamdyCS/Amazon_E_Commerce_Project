namespace DataAccessLayer.Entities;

public partial class ProductImage
{
    public long Id { get; set; }

    public string ImageUrl { get; set; }

    public string PublicId { get; set; }

    public long ProductId { get; set; }

    public virtual Product? Product { get; set; }

}
