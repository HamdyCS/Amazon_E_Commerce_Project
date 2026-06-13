using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IMailService
    {
        Task SendOtpEmailAsync(string email, string otp);

        Task SendUpdateOrderEmailAsync(UpdateOrderEmailQueueDto updateOrderEmailQueueDto);

        Task SendEmailAsync(string email, string subject, string body);
    }
}
