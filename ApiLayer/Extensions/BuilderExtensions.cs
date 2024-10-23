using Serilog;

namespace ApiLayer.Extensions
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }
    }


    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServicese(this IServiceCollection services)
        {

            return services;
           
        }

    }
}
