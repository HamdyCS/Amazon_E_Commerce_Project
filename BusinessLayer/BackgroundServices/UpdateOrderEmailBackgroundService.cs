using BusinessLayer.Contracks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BackgroundServices
{
    public class UpdateOrderEmailBackgroundService : BackgroundService
    {
        private readonly ILogger<UpdateOrderEmailBackgroundService> _logger;
        private readonly IUpdateOrderEmailQueue _updateOrderEmailQueue;
        private readonly IServiceProvider _serviceProvider;

        public UpdateOrderEmailBackgroundService(ILogger<UpdateOrderEmailBackgroundService> logger,IUpdateOrderEmailQueue updateOrderEmailQueue,IServiceProvider serviceProvider)
        {
            _logger = logger;
            _updateOrderEmailQueue = updateOrderEmailQueue;
            _serviceProvider = serviceProvider;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                try
                {
                    //wait for new email
                    var updateOrderEmailDto = await _updateOrderEmailQueue.DeQueueAsync(stoppingToken);

                    var scope = _serviceProvider.CreateScope();
                    var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                   await mailService.SendUpdateOrderEmailAsync(updateOrderEmailDto);
                }
                catch (Exception ex) {
                    _logger.LogError($"Error on sending email. Message: {ex.Message}");
                }
            }
        }
    }
}
