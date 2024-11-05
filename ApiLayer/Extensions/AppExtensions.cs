using ApiLayer.MiddleWares;

namespace ApiLayer.Extensions
{
    public static class AppExtensions
    {
        public static WebApplication AddCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<CheckIfTokenIsValidMiddleWare>();
            return app;
        }
    }
}
