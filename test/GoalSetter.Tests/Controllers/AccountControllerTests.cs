namespace GoalSetter.Controllers
{
    using Controllers;
    using Xunit;
    using Models;
    using Moq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Builder;

    public class AccountControllerTests
    {
        [Fact]
        public void Constructor()
        {
            // Arrange
            var userManagerMock = this.MockUserManager<ApplicationUser>(); ;
            var signInManagerMock = this.MockSignInManager<ApplicationUser>(userManagerMock);

            // Act
            new AccountController(userManagerMock.Object,signInManagerMock.Object);
        }

        /// Took this from https://github.com/aspnet/Identity/blob/master/test/Shared/MockHelpers.cs
        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            return mgr;
        }

        private Mock<SignInManager<TUser>> MockSignInManager<TUser>(Mock<UserManager<TUser>> userManager) where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsManager = new Mock<IUserClaimsPrincipalFactory<TUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();

            var signInManager = new Mock<SignInManager<TUser>>(
                userManager.Object,
                contextAccessor.Object, 
                claimsManager.Object, 
                options.Object, 
                null);

            return signInManager;
        }
    }
}
