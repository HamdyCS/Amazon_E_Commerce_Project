using BusinessLayer.Validitions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dtos
{
    public class CreateBannerDto
    {
        public string Title { get; set; }

        public string Link { get; set; }


        public IFormFile Image { get; set; }

        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }
    }
}
