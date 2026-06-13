using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Dtos
{
    public class EmailContentDto
    {
        public string Subject { get; set; }

        public string Messsage { get; set; }

        public string TextBody { get; set; }
    }
}
