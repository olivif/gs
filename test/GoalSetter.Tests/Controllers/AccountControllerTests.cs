namespace GoalSetter.Controllers
{
    using Xunit;
    using Models;
    using Moq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Builder;
    using Models.AccountViewModels;

    public class AccountControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

        private readonly Mock<SignInManager<ApplicationUser>> signInManagerMock;

        private readonly AccountController controller;

        public AccountControllerTests()
        {
            this.userManagerMock = this.MockUserManager<ApplicationUser>(); ;
            this.signInManagerMock = this.MockSignInManager<ApplicationUser>(userManagerMock);

            this.controller = new AccountController(
                this.userManagerMock.Object,
                this.signInManagerMock.Object);
        }

        [Fact]
        public void Constructor()
        {
            // Act
            new AccountController(
                this.userManagerMock.Object,
                this.signInManagerMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("someUrl")]
        public void Login(string returnUrl)
        {
            // Act
            var actionResult = controller.Login(returnUrl);
        }

        public void LoginWithModel(LoginViewModel model, string returnUrl)
        {
            // Act
            var actionResult = controller.Login(model, returnUrl);
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
