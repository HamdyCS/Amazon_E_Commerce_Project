using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ProductCategoryImageDto
    {
        public long Id { get; set; }

        public byte[] Image { get; set; }

        public long ProductCategoryId { get; set; }
    }
}
