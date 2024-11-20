using Microsoft.AspNetCore.Authorization;

namespace TravelInspiration.API.Shared.Security
{
    public static class AuthorizationPolices
    {
        public static AuthorizationPolicy HasWriteActionPolicy { get;  }
            = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("scope", "write")
            .Build();
    }
}
