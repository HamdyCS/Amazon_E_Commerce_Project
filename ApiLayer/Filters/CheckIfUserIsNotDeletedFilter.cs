using BusinessLayer.Contracks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiLayer.Filters
{
    public class CheckIfUserIsNotDeletedFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;
        private readonly ILogger<CheckIfUserIsNotDeletedFilter> _logger;

        public CheckIfUserIsNotDeletedFilter(IUserService userService, ILogger<CheckIfUserIsNotDeletedFilter> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                HttpContext httpContext = context.HttpContext;

                //check if endpoint is null
                if (httpContext is null || httpContext.GetEndpoint() is null)
                {
                    context.HttpContext.Response.StatusCode = 404;
                    return;
                }

                //check if endpoint allows anonymous access
                if (httpContext.GetEndpoint().Metadata.OfType<AllowAnonymousAttribute>().Any())
                {
                    await next();
                    return;
                }

                //check if endpoint did`t have authorization attribute
                if (!httpContext.GetEndpoint().Metadata.OfType<AuthorizeAttribute>().Any())
                {
                    await next();
                    return;
                }

                //check if user is authenticated
                if (httpContext.User is null || !httpContext.User.Identity.IsAuthenticated)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                //get user id from claims
                var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                //check if user is deleted
                var isUserDeleted = await _userService.IsUserDeletedByIdAsync(userId);
                if (isUserDeleted)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("User is deleted");
                    return;
                }

                await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on CheckIfTokenIsValidMiddleWare. Error {error}", ex.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
               await context.HttpContext.Response.WriteAsync($"Internal server error. {ex.Message}");
            }


        }
    }
}
