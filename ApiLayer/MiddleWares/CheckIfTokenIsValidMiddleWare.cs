using BusinessLayer.Contracks;
using BusinessLayer.Servicese;
using System.Security.Claims;

namespace ApiLayer.MiddleWares
{
    public class CheckIfTokenIsValidMiddleWare
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RequestDelegate _next;
        private readonly ILogger<CheckIfTokenIsValidMiddleWare> _logger;

        public CheckIfTokenIsValidMiddleWare(IServiceScopeFactory serviceScopeFactory,RequestDelegate next,ILogger<CheckIfTokenIsValidMiddleWare> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
           
            if(context.User.Identity.IsAuthenticated)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                        if (userId == null)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.WriteAsync("Token is not vaild");
                        }

                        var user = await userService.FindByIdAsync(userId);

                        if (user is null)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        }

                        var IsUserDeleted = await userService.IsUserDeletedByIdAsync(userId);

                        if (IsUserDeleted)
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                   
                }

                
            }
            else
            {
                await _next.Invoke(context);
            }

           
        }
    }
}
