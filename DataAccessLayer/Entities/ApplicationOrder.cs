using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class ApplicationOrder
{
    public long Id { get; set; }

    public long ApplicationId { get; set; }

    public long ApplicationOrderTypeId { get; set; }

    public decimal ShippingCost { get; set; }

    public long ShoppingCartId { get; set; }

    [Required]
    public long UserAddressId { get; set; }

    public virtual UserAddress? UserAddress { get; set; }
    public long PaymentId { get; set; }

    public long DeliveryId { get; set; }

    public virtual Application? Application { get; set; }

    public virtual ApplicationOrdersType? ApplicationOrderType { get; set; }

    public virtual Delivery? Delivery { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ShoppingCart? ShoppingCart { get; set; }
}
