using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.UnitTests.Shared.Security
{
    public sealed class CurrentUserServiceTests
    {
        [Fact]
        public void WhenGettingUser__WithNameIdentifierClaimInContext_NameIdentifierMustBeReturned()
        {
            // Arrange
            var httpContextAccesor = new Mock<IHttpContextAccessor>();
            var identity = new ClaimsIdentity(
                new List<Claim>()                 
                {
                    new Claim(ClaimTypes.NameIdentifier, "poschuler")
                },
                "Test",
                ClaimTypes.NameIdentifier,
                ClaimTypes.Role
                );
            var context = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = context
            };
            httpContextAccesor.Setup(x => x.HttpContext).
                Returns(httpContext);
            var currentUserService = new CurrentUserService(httpContextAccesor.Object);

            // Act
            var userId = currentUserService.UserId;

            // Assert
            Assert.Equal(identity.Name, userId);

        }
    }
}
