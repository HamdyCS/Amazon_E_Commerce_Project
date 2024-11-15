using BusinessLayer.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Cms;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using BusinessLayer.Options;

namespace BusinessLayer.Servicese
{
    public class MailService : IMailService
    {
        private readonly MailOptions _mailOptions;
        private readonly ILogger<MailService> _logger;

        public MailService(MailOptions mailOptions,ILogger<MailService> logger)
        {
            _mailOptions = mailOptions;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(subject, nameof(subject));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(body, nameof(body));

            try
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Amazon E-Commerce", _mailOptions.Email));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Text)
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(_mailOptions.Email, _mailOptions.AppPassword);

                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Send email, Error {error}", ex.Message);
                throw new Exception($"Error on Send email, Error: {ex.Message}");
            }

        }

    }
}
