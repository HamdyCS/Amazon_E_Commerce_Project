namespace BusinessLayer.Dtos
{
    public class SellerProductInShoppingCartDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long SellerProductId { get; set; }
        public long ProductId { get; set; }
        public long ShoppingCartId { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductImageUrl { get; set; }

    }
}
