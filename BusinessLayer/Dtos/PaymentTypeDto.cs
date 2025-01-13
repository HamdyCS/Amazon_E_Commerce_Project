namespace BusinessLayer.Dtos
{
    public class PaymentTypeDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }
    }
}
