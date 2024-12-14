using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ProductImageDto
    {
        public long Id { get; set; }

        public byte[] Image { get; set; }

        public long ProductId { get; set; }

    }
}
