using BusinessLayer.Contracks;
using BusinessLayer.Options;
using CloudinaryDotNet;
using DataAccessLayer.Exceptions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class MailService : IMailService
    {
        private readonly MailOptions _mailOptions;
        private readonly ILogger<MailService> _logger;


        public MailService(MailOptions mailOptions, ILogger<MailService> logger)
        {
            _mailOptions = mailOptions;
            _logger = logger;
        }
        public async Task SendOtpEmailAsync(string email, string Otp)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(email, nameof(email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Otp, nameof(Otp));

            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "OtpEmailTemplate.html");
                string htmlTemplate = await File.ReadAllTextAsync(filePath);

                htmlTemplate = htmlTemplate.Replace("{{OTP_1}}", Otp[0].ToString())
                               .Replace("{{OTP_2}}", Otp[1].ToString())
                               .Replace("{{OTP_3}}", Otp[2].ToString())
                               .Replace("{{OTP_4}}", Otp[3].ToString())
                               .Replace("{{OTP_5}}", Otp[4].ToString())
                               .Replace("{{OTP_6}}", Otp[5].ToString());

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Amazon E-Commerce", _mailOptions.Email));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = Otp;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlTemplate,
                    TextBody = $"Your OTP code is: {Otp}"
                };

                message.Body = bodyBuilder.ToMessageBody();

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
