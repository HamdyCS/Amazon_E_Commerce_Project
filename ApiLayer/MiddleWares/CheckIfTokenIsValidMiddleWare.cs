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
                    var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    if (userId == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.WriteAsync("Token is not vaild");
                    }

                    var user = await _userService.FindByIdAsync(userId);

                    if (user is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    }
                    await _next(context);
                }

                
            }
            else
            {
                await _next.Invoke(context);
            }

           
        }
    }
}
