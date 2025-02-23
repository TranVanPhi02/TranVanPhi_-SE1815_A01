using System.Security.Claims;

namespace TranVanPhiMVC.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // Extension method to get UserId from ClaimsPrincipal
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
     
    }
}
