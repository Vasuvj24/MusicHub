using System.Security.Claims;

namespace MusicHub.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //at time of request getting we get claimsprincipal when we extract claims from payload so asp.net gives us claims principal
        //making it extension meathod of httpcontext.user(claimsprincipal)
        public static Guid GetUserId(this ClaimsPrincipal User)
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.Parse(raw!);
        }
    }
}
