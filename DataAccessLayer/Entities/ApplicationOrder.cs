using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class ApplicationOrder
{
    public long Id { get; set; }

    public long ApplicationId { get; set; }

    public long ApplicationOrderTypeId { get; set; }

    public long ShoppingCartId { get; set; }

    public long UserAddressId { get; set; }

    public long PaymentId { get; set; }

    public string DeliveryId { get; set; }

    public string CreatedBy { get; set; }

    public virtual UserAddress? UserAddress { get; set; }
   
    public virtual User? Delivery { get; set; }

    public virtual User? user { get; set; }

    public virtual Application? Application { get; set; }

    public virtual ApplicationOrdersType? ApplicationOrderType { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ShoppingCart? ShoppingCart { get; set; }
}
