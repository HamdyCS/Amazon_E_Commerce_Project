using System.Security.Claims;

namespace ApiLayer.Help
{
    public static class Helper
    {
        public static string? GetIdFromClaimsPrincipal(ClaimsPrincipal user)
        {
            if (user is null) return null;
            try
            {
                var Id = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                return Id;
            }
            catch(Exception ex)
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
    }

}
