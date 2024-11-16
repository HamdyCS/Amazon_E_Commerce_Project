using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Otp
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        [NotMapped]
        public bool IsActive => ExpiresAt >= DateTime.UtcNow;


    }
}
