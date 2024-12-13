using DataAccessLayer.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ProductSubCategory
    {
        public long Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public byte[] Image { get; set; }

        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DateOfDeletion { get; set; }

        public virtual User? user { get; set; }

        public long ProductCategoryId { get; set; }

        public virtual ProductCategory productCategory { get; set; }
    }
}
