using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities;

public partial class RefreshToken
{
    public long Id { get; set; }

    public string Token { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public string UserId { get; set; }

    public virtual User? user { get; set; }

    [NotMapped]
    public bool IsActive => ExpiresAt > DateTime.UtcNow;
}
