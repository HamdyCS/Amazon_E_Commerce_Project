using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ProductsInShoppingCart
{
    public long Id { get; set; }

    public int? Number { get; set; }

    public decimal? TotalPrice { get; set; }

    public long? SellerProductId { get; set; }

    public long? ShoppingCartId { get; set; }

    public virtual SellerProduct? SellerProduct { get; set; }

    public virtual ShoppingCart? ShoppingCart { get; set; }
}
