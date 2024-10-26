using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class City
{
    public long Id { get; set; }

    public string NameEn { get; set; }

    public string NameAr { get; set; }

    public virtual ICollection<CitiesWhereDeliveiesWork> CitiesWhereDeliveiesWorks { get; set; } = new List<CitiesWhereDeliveiesWork>();

    public virtual ICollection<UserAddress> PeopleAddresses { get; set; } = new List<UserAddress>();

    public virtual ICollection<ShippingCost> ShippingCosts { get; set; } = new List<ShippingCost>();
}
