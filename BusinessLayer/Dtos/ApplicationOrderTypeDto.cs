using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ApplicationOrderTypeDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }
    }
}
