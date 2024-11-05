using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Options
{
    public class MailOptions
    {
        public string Email { get; set; }
        public string AppPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
 
}
