using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class ApplicationDto
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public long ApplicationTypeId { get; set; }

    }
}
