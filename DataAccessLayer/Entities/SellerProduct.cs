using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccessLayer.Entities;

public partial class SellerProduct
{
    public long Id { get; set; }

    public required string SellerId { get; set; }

    public decimal Price { get; set; }

    public int NumberInStock { get; set; }

    public long ProductId { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    public DateTime? DateOfDeletion { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? Seller {  get; set; }

    public virtual ICollection<SellerProductReview> SellerProductReviews { get; set; } = new List<SellerProductReview>();

    public virtual ICollection<SellerProductInShoppingCart> ProductsInShoppingCarts { get; set; } = new List<SellerProductInShoppingCart>();
}
