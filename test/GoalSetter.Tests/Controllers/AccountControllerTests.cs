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

        public static TheoryData<ExternalLoginConfirmationViewModel, string> ExternalLoginConfirmationData =
            new TheoryData<ExternalLoginConfirmationViewModel, string>()
        {
            { new ExternalLoginConfirmationViewModel(), "url" },
            { new ExternalLoginConfirmationViewModel(), "url" },
            { new ExternalLoginConfirmationViewModel(), "url" },
            { new ExternalLoginConfirmationViewModel(), "url" },
        };

        public static TheoryData<ExternalLoginConfirmationViewModel, string, IdentityResult> ExternalLoginConfirmationDataFailed =
            new TheoryData<ExternalLoginConfirmationViewModel, string, IdentityResult>()
        {
            { new ExternalLoginConfirmationViewModel(), "url", null },
        };

        public static TheoryData<ExternalLoginConfirmationViewModel, string, IdentityResult> ExternalLoginConfirmationDataSuccess =
            new TheoryData<ExternalLoginConfirmationViewModel, string, IdentityResult>()
        {
            { new ExternalLoginConfirmationViewModel(), "url", IdentityResult.Success },
        };

        public AccountControllerTests()
        {
            this.userManagerMock = IdentityTestUtils.MockUserManager<ApplicationUser>();
            this.signInManagerMock = IdentityTestUtils.MockSignInManager<ApplicationUser>(userManagerMock);

            this.controller = new AccountController(
                this.userManagerMock.Object,
                this.signInManagerMock.Object);

            this.controller.Url = this.CreateMockUrlHelper();
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

            // Act
            var task = controller.ExternalLoginCallback(returnUrl, remoteError).Result;
        }

        [Theory]
        [MemberData(nameof(ExternalLoginConfirmationData))]
        public void ExternalLoginConfirmationCallsGetExternalLoginInfoAsync(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            // Arrange
            var task = this.controller.ExternalLoginConfirmation(model, returnUrl);

            // Act
            var result = task.Result;

            // Assert
            this.signInManagerMock.Verify(x => x.GetExternalLoginInfoAsync(null), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExternalLoginConfirmationData))]
        public void ExternalLoginConfirmationReturnsOnNullInfo(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            // Arrange
            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult((ExternalLoginInfo)null));

            var task = this.controller.ExternalLoginConfirmation(model, returnUrl);

            // Act
            var result = task.Result;
        }

        [Theory]
        [MemberData(nameof(ExternalLoginConfirmationDataFailed))]
        public void ExternalLoginConfirmationCallsCreateAsync(
            ExternalLoginConfirmationViewModel model, 
            string returnUrl, 
            IdentityResult identityResult)
        {
            // Arrange
            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            this.userManagerMock
                .Setup(x => x.CreateAsync(user))
                .Returns(Task.FromResult(identityResult));

            Func<Task> task = () => controller.ExternalLoginConfirmation(model, returnUrl);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);

            this.userManagerMock
                .Verify(x => x.CreateAsync(It.Is<ApplicationUser>(u => 
                    u.Email == model.Email && 
                    u.UserName == model.Email)), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExternalLoginConfirmationDataSuccess))]
        public void ExternalLoginConfirmationCallsCreateAsyncSuccess(
            ExternalLoginConfirmationViewModel model,
            string returnUrl,
            IdentityResult identityResult)
        {
            // Arrange
            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            this.userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(identityResult));

            Func<Task> task = () => controller.ExternalLoginConfirmation(model, returnUrl);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);

            this.userManagerMock
                .Verify(x => x.AddLoginAsync(
                    It.Is<ApplicationUser>(u =>
                        u.Email == model.Email &&
                        u.UserName == model.Email),
                    It.Is<ExternalLoginInfo>(u =>
                        u.LoginProvider == externalLoginInfo.LoginProvider &&
                        u.ProviderKey == externalLoginInfo.ProviderKey &&
                        u.ProviderDisplayName == externalLoginInfo.ProviderDisplayName)),
                    Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExternalLoginConfirmationDataSuccess))]
        public void ExternalLoginConfirmationSuccessCallsSignInAsync(
            ExternalLoginConfirmationViewModel model,
            string returnUrl,
            IdentityResult identityResult)
        {
            // Arrange
            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            this.userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(identityResult));

            this.userManagerMock
                .Setup(x => x.AddLoginAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<ExternalLoginInfo>()))
                .Returns(Task.FromResult(identityResult));

            Func<Task> task = () => controller.ExternalLoginConfirmation(model, returnUrl);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(task);

            // Assert
            Assert.NotNull(ex);

            this.signInManagerMock
                .Verify(x => x.SignInAsync(
                        It.IsAny<ApplicationUser>(), 
                        false,
                        null),
                    Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExternalLoginConfirmationDataSuccess))]
        public void ExternalLoginConfirmationSuccess(
            ExternalLoginConfirmationViewModel model,
            string returnUrl,
            IdentityResult identityResult)
        {
            // Arrange
            var externalLoginInfo = this.CreateFakeLoginData();

            this.signInManagerMock
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .Returns(Task.FromResult(externalLoginInfo));

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            this.userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(identityResult));

            this.userManagerMock
                .Setup(x => x.AddLoginAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<ExternalLoginInfo>()))
                .Returns(Task.FromResult(identityResult));

            this.signInManagerMock
                .Setup(x => x.SignInAsync(
                        It.IsAny<ApplicationUser>(),
                        false,
                        null))
                .Returns(Task.FromResult(identityResult));

            var task = this.controller.ExternalLoginConfirmation(model, returnUrl);

            // Act
            var result = task.Result;
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
