namespace BusinessLayer.Dtos
{
    public class BrandDto
    {
        public long Id { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public string CreatedBy { get; set; }

        public ImageDto Image { get; set; }
    }
}
