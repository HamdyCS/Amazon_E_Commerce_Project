using BusinessLayer.Contracks;
using BusinessLayer.Servicese;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace ApiLayer.MiddleWares
{
    public class CheckIfUserIsNotDeletedMiddleWare
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RequestDelegate _next;
        private readonly ILogger<CheckIfUserIsNotDeletedMiddleWare> _logger;

        public CheckIfUserIsNotDeletedMiddleWare(IServiceScopeFactory serviceScopeFactory,RequestDelegate next,ILogger<CheckIfUserIsNotDeletedMiddleWare> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _next = next;
            _logger = logger;
        }

        
        public async Task Invoke(HttpContext context)
        {
            if(context is null || context.GetEndpoint() is null)
            {
                context.Response.StatusCode = 404;
                return;
            }
            if(context.GetEndpoint().Metadata.OfType<AllowAnonymousAttribute>().Any())
            {
                await _next(context);
                return;
            }
                      
            if(context.GetEndpoint().Metadata.OfType<AuthorizeAttribute>().Any())
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                   

                    try
                    {
                        
                        var Id = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                        var UserRole = context.User.FindFirst(ClaimTypes.Role).Value;

                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CheckIfUserIsNotDeletedMiddleWare>>();

                        if(context.User is null || !context.User.Identity.IsAuthenticated)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }

                        

                        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                        if (userId == null)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }

                        var user = await userService.FindByIdAsync(userId);

                        if (user is null)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }

                        var IsUserDeleted = await userService.IsUserDeletedByIdAsync(userId);

                        if (IsUserDeleted)
                        {

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("User is deleted");
                            return;
                        }

                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        _logger.LogError(ex, "Error on CheckIfTokenIsValidMiddleWare. Error {error}", ex.Message);
                    }
                   
                }

                
            }
            else
            {
                await _next(context);
            }

           
        }
    }
}
