using BusinessLayer.Contracks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.BackgroundServices
{
    public class ProductsCacheUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsCacheUpdateBackgroundService> _logger;

        public ProductsCacheUpdateBackgroundService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<ProductsCacheUpdateBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //number of houers
            int hours = _configuration.GetValue<int>("Redis:ProductsDurationInHoues");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Create a new scope to get scoped services in sigeleton background service
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                        await productService.UpdateProductsInRedisCacheAsync();

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while updating products cache. {Message}", ex.Message);
                }

                // Wait for the specified interval before the next update
                await Task.Delay(TimeSpan.FromHours(hours), stoppingToken);
            }
        }
    }
}
