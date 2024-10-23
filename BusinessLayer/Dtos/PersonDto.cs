using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class PersonDto
    {
        public long Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
}
