using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class Application
{
    
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; }

    public virtual User? user { get; set; }

    public long ApplicationTypeId { get; set; }

    public virtual ApplicationType? ApplicationType { get; set; }

    public virtual ICollection<ApplicationOrder> ApplicationOrders { get; set; } = new List<ApplicationOrder>();

}
