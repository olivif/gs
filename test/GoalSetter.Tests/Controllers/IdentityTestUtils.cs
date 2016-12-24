namespace GoalSetter.Controllers
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Moq;

    public class IdentityTestUtils
    {
        /// Took this from https://github.com/aspnet/Identity/blob/master/test/Shared/MockHelpers.cs
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            return mgr;
        }

        public static Mock<SignInManager<TUser>> MockSignInManager<TUser>(Mock<UserManager<TUser>> userManager) where TUser : class
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
