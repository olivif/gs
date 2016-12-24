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
            this.userManagerMock = IdentityTestUtils.MockUserManager<ApplicationUser>(); ;
            this.signInManagerMock = IdentityTestUtils.MockSignInManager<ApplicationUser>(userManagerMock);

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
    }
}
