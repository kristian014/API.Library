using Application.Common.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.User
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            ClaimsPrincipal? user = httpContextAccessor?.HttpContext?.User;

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if(user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            string userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            string email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            IEnumerable<string> roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);

            return new CurrentUser(userId, email, roles);
        }
    }
}
