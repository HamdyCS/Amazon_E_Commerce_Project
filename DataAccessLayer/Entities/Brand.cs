using DataAccessLayer.Identity.Entities;

namespace DataAccessLayer.Entities;

public partial class Brand
{
    public long Id { get; set; }

    public string NameEn { get; set; }

    public string NameAr { get; set; }

    public string CreatedBy { get; set; }

    public string ImageUrl { get; set; }

    public string PublicId { get; set; }

    public virtual User? user { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
