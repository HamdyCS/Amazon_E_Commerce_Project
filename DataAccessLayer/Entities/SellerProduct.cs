using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class SellerProduct
{
    public long Id { get; set; }

    public long? SellerId { get; set; }

    public decimal? Price { get; set; }

    public int? NumberInStock { get; set; }

    public long? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<ProductsInShoppingCart> ProductsInShoppingCarts { get; set; } = new List<ProductsInShoppingCart>();
}
