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
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    public class AccountControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

        private readonly Mock<SignInManager<ApplicationUser>> signInManagerMock;

        private readonly AccountController controller;

        public static TheoryData<LoginViewModel, string> LoginWithModelData = 
            new TheoryData<LoginViewModel, string>()
        {
            { new LoginViewModel(), "url" }
        };

        public static TheoryData<LoginViewModel, string, SignInResult> LoginWithModelDataNonSuccessResult = 
            new TheoryData<LoginViewModel, string, SignInResult>()
        {
            { new LoginViewModel(), "url", SignInResult.Failed },
            { new LoginViewModel(), "url", SignInResult.LockedOut },
            { new LoginViewModel(), "url", SignInResult.NotAllowed },
            { new LoginViewModel(), "url", SignInResult.TwoFactorRequired },
        };

        public static TheoryData<LoginViewModel, string, SignInResult> LoginWithModelDataSuccessResult =
            new TheoryData<LoginViewModel, string, SignInResult>()
        {
            { new LoginViewModel(), "/localUrl", SignInResult.Success },
        };

        public AccountControllerTests()
        {
            this.userManagerMock = IdentityTestUtils.MockUserManager<ApplicationUser>();
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

        [Theory]
        [MemberData(nameof(LoginWithModelData))]
        public void LoginWithModelThrowsOnNullResult(LoginViewModel model, string returnUrl)
        {
            // Arrange
            Func<Task> task = () => controller.Login(model, returnUrl);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [MemberData(nameof(LoginWithModelDataNonSuccessResult))]
        public void LoginWithModelThrowsOnNonSuccessResult(
            LoginViewModel model, 
            string returnUrl, 
            SignInResult signInResult)
        {
            // Arrange
            this.signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, false, false))
                .Returns(Task.FromResult(signInResult));

            Func<Task> task = () => controller.Login(model, returnUrl);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [MemberData(nameof(LoginWithModelDataSuccessResult))]
        public void LoginWithModelDoesntThrowOnSuccessResult(
            LoginViewModel model,
            string returnUrl,
            SignInResult signInResult)
        {
            // Arrange
            this.signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Email, model.Password, false, false))
                .Returns(Task.FromResult(signInResult));

            var context = new Mock<HttpContext>();
            var requestContext = new ActionContext(
                context.Object, 
                new RouteData(), 
                new ActionDescriptor());
            UrlHelper urlHelper = new UrlHelper(requestContext);

            this.controller.Url = urlHelper;

            // Act
            var task = controller.Login(model, returnUrl);
            var result = task.Result;
        }
    }
}
