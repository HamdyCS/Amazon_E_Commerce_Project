using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ProductCategoryDto
    {
        public long Id { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public string CreatedBy { get; set; }

        public List<byte[]> Images { get; set; } 

    }
}
