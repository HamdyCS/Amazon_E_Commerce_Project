using System.Net;
using System.Security.Claims;

namespace ApiLayer.Help
{
    public static class Helper
    {
        public static string[] AllowedOrigin = [
            "http://localhost:5173/"
            ];

        public static string? GetIdFromClaimsPrincipal(ClaimsPrincipal user)
        {
            if (user is null) return null;
            try
            {
                var Id = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                return Id;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string? GetEmailFromClaimsPrincipal(ClaimsPrincipal user)
        {
            if (user is null) return null;
            try
            {
                var Email = user.FindFirst(ClaimTypes.Email).Value;
                return Email;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

      

        public static bool IsValidReturnUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) return false;

            //check if the returnUrl is valid and is in the allowed origin list
            return AllowedOrigin.Any(origin => returnUrl.StartsWith(origin));
        }
    }

}
