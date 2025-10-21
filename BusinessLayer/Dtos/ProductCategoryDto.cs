namespace BusinessLayer.Dtos
{
    public class ProductCategoryDto
    {
        public long Id { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public List<ImageDto> Images { get; set; }

    }
}
