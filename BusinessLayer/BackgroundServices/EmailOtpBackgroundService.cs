using BusinessLayer.Contracks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BackgroundServices
{
    public class EmailOtpBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailOtpBackgroundService> _logger;

        public EmailOtpBackgroundService(IEmailQueue emailQueue,IServiceProvider serviceProvider,ILogger<EmailOtpBackgroundService> logger)
        {
            _emailQueue = emailQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //waiting for new email
                var emailDto = await _emailQueue.DeQueueAsync(stoppingToken);

                //create scope to get mailSerivce
                using var scope = _serviceProvider.CreateScope();
                var mailSerivce = scope.ServiceProvider.GetRequiredService<IMailService>();

                await mailSerivce.SendOtpEmailAsync(emailDto.Email, emailDto.OTP);

            }
        }
    }
}
