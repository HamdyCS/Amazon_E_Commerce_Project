using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Payment
{
    public long Id { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public long PaymentTypeId { get; set; }

    public long ShoppingCartId { get; set; }

    public long shippingCostId {  get; set; }

    public long UserAddressId { get; set; }

    public virtual UserAddress? UserAddress { get; set; }

    public virtual ShoppingCart? shoppingCart { get; set; }

    public virtual ShippingCost? shippingCost { get; set; }

    public virtual IEnumerable<ApplicationOrder> ApplicationOrders { get; set; } = new List<ApplicationOrder>();

    public virtual PaymentsType? PaymentType { get; set; }
}
