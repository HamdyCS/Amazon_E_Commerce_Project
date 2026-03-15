using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IMailService
    {
        public Task SendOtpEmailAsync(string email, string otp);

        public Task SendEmailAsync(string email, string subject, string body);
    }
}
