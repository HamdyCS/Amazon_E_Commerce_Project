using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class City
{
    public long Id { get; set; }

    public string NameEn { get; set; }

    public string NameAr { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? DateOfDelete { get; set; }

    public virtual User? user { get; set; }

    public virtual ICollection<CityWhereDeliveryWork> CitiesWhereDeliveiesWorks { get; set; } = new List<CityWhereDeliveryWork>();

    public virtual ICollection<UserAddress> UsersAddresses { get; set; } = new List<UserAddress>();

    public virtual ICollection<ShippingCost> ShippingCosts { get; set; } = new List<ShippingCost>();
}
