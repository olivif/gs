namespace GoalSetter.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Models;
    using Moq;
    using Xunit;

    public class HomeControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

        private readonly HomeController controller;

        public HomeControllerTests()
        {
            this.userManagerMock = IdentityTestUtils.MockUserManager<ApplicationUser>(); ;

            this.controller = new HomeController(this.userManagerMock.Object);
        }

        [Fact]
        public void CanCallIndex() 
        {
            // Act
            var actionResult = this.controller.Index();
        }

        [Fact]
        public void CanCallError()
        {
            // Act
            var actionResult = this.controller.Error();
        }
    }
}
