using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Identity.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public long PersonId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual Person? Person { get; set; }

        [Required]
        public long PhoneId { get; set; }

        public virtual Phone? phone { get; set; }

        public virtual IEnumerable<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    }
}
