using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class CityDto
    {
        
        public int Id { get; set; }
        [Required]
        public string NameEn { get; set; }

        [Required]
        public string NameAr { get; set; }

       

    }
}
