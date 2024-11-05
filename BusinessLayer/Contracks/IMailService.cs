using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IMailService
    {
        public Task SendEmailAsync(string email, string subject, string body);
    }
}
