using System.Security.Claims;

namespace TravelInspiration.API.Shared.Security
{
    public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) 
        : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        public string? UserId
        {
            get
            {
                return httpContextAccessor.HttpContext?.
                    User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}
