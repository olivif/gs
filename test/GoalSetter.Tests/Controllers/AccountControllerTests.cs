namespace GoalSetter.Controllers
{
    using Xunit;
    using Models;
    using Moq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Http;
    using Models.AccountViewModels;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    using System.Security.Claims;

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
            { new LoginViewModel(), "https://google.com/notlocalUrl", SignInResult.Success },
        };

        public static TheoryData<string, SignInResult> ExternalLoginCallbackData =
            new TheoryData<string, SignInResult>()
        {
            { "url", null },
            { "url", SignInResult.Failed },
            { "url", SignInResult.LockedOut },
            { "url", SignInResult.NotAllowed },
            { "url", SignInResult.TwoFactorRequired },
        };

        public static TheoryData<string, SignInResult> ExternalLoginCallbackDataSuccess =
            new TheoryData<string, SignInResult>()
        {
            { "url", SignInResult.Success },
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

            this.controller.Url = this.CreateMockUrlHelper();

            // Act
            var task = controller.Login(model, returnUrl);
            var result = task.Result;
        }

        [Fact]
        public void LogOffCallsSignOut()
        {
            // Act
            var actionResult = controller.LogOff();

            // Assert
            this.signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
        }

        //[Theory]
        //[InlineData("provider", "someUrl")]
        public void ExternalLoginCallsConfigureExternalAuthenticationProperties(string provider, string returnUrl)
        {
            // Arrange
            this.controller.Url = this.CreateMockUrlHelper();

            // Act
            var actionResult = controller.ExternalLogin(provider, returnUrl);

            // Assert
            this.signInManagerMock.Verify(x => x.ConfigureExternalAuthenticationProperties(provider, returnUrl, null), Times.Once);
        }

        [Theory]
        [InlineData(null, "error")]
        [InlineData("someUrl", null)]
        public void ExternalLoginCallbackThrowsForNullArgs(string returnUrl, string remoteError)
        {
            // Arrange
            Func<Task> task = () => controller.ExternalLoginCallback(returnUrl, remoteError);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData("someUrl")]
        public void ExternalLoginCallbackCallsGetExternalLoginInfoAsync(string returnUrl)
        {
            // Arrange
            string remoteError = null;

            var task = this.controller.ExternalLoginCallback(returnUrl, remoteError);

            // Act
            var result = task.Result;

            // Assert
            this.signInManagerMock.Verify(x => x.GetExternalLoginInfoAsync(null), Times.Once);
        }

        [Theory]
        [InlineData("someUrl")]
        public void ExternalLoginCallbackReturnsOnNullInfo(string returnUrl)
        {
            // Arrange
            string remoteError = null;

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult((ExternalLoginInfo)null));
            var task = this.controller.ExternalLoginCallback(returnUrl, remoteError);

            // Act
            var result = task.Result;
        }

        [Theory]
        [InlineData("someUrl")]
        public void ExternalLoginCallbackCallsExternalLoginSignInAsyncThrowsOnNull(string returnUrl)
        {
            // Arrange
            string remoteError = null;

            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            Func<Task> task = () => controller.ExternalLoginCallback(returnUrl, remoteError);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);

            this.signInManagerMock.Verify(x => x.ExternalLoginSignInAsync(
                externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey,
                false), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExternalLoginCallbackData))]
        public void ExternalLoginCallbackCallsExternalLoginSignInAsyncThrowsFailResult(string returnUrl, SignInResult signInResult)
        {
            // Arrange
            string remoteError = null;

            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            this.signInManagerMock
                .Setup(x => x.ExternalLoginSignInAsync(
                    externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey,
                    false))
                .Returns(Task.FromResult(signInResult));

            Func<Task> task = () => controller.ExternalLoginCallback(returnUrl, remoteError);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);

            this.signInManagerMock.Verify(x => x.ExternalLoginSignInAsync(
                externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey,
                false), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExternalLoginCallbackDataSuccess))]
        public void ExternalLoginCallbackSuccess(string returnUrl, SignInResult signInResult)
        {
            // Arrange
            string remoteError = null;

            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            this.signInManagerMock
                .Setup(x => x.ExternalLoginSignInAsync(
                    externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey,
                    false))
                .Returns(Task.FromResult(signInResult));

            this.controller.Url = this.CreateMockUrlHelper();

            // Act
            var task = controller.ExternalLoginCallback(returnUrl, remoteError).Result;
        }

        private IUrlHelper CreateMockUrlHelper()
        {
            var context = new Mock<HttpContext>();
            var requestContext = new ActionContext(
                context.Object,
                new RouteData(),
                new ActionDescriptor());
            UrlHelper urlHelper = new UrlHelper(requestContext);

            return urlHelper;
        }

        private ExternalLoginInfo CreateFakeLoginData()
        {
            return new ExternalLoginInfo(
                principal: new ClaimsPrincipal(),
                loginProvider: "provider",
                providerKey: "providerKey",
                displayName: "displayName");
        }
    }
}
