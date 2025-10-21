namespace BusinessLayer.Dtos
{
    public class ProductCategoryImageDto
    {
        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public string PublicId { get; set; }

        public long ProductCategoryId { get; set; }
    }
}
