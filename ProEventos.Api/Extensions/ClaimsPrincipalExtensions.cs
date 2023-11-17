using System.Net.NetworkInformation;
using System.Security.Claims;

namespace ProEventos.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal userClaim)
        {
            var userName = userClaim.FindFirst(ClaimTypes.Name)?.Value;
            if (userName != null)
                return userName;
            return "";
        }

        public static int GetUserId(this ClaimsPrincipal userClaim)
        {
            var userId = userClaim.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
                return int.Parse(userId);
            return 0;
        }
    }
}
