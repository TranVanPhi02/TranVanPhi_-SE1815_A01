using System.Security.Claims;

namespace TranVanPhiMVC.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // Extension method to get UserId from ClaimsPrincipal
        public static short GetUserId(this ClaimsPrincipal user)
        {
            var accountId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return short.Parse(accountId);
        }
     
    }
}
