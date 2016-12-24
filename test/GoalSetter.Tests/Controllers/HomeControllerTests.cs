namespace GoalSetter.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Moq;
    using Service.GoalManager;
    using Xunit;

    public class HomeControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

        private readonly GoalsDbContext goalsDbContext;

        private readonly Mock<GoalManager> goalManagerMock;

        private readonly HomeController controller;

        public HomeControllerTests()
        {
            this.userManagerMock = IdentityTestUtils.MockUserManager<ApplicationUser>(); ;

            var options = new DbContextOptions<GoalsDbContext>();
            this.goalsDbContext = new GoalsDbContext(options);

            this.goalManagerMock = new Mock<GoalManager>();

            this.controller = new HomeController(
                this.userManagerMock.Object,
                this.goalsDbContext,
                this.goalManagerMock.Object);
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

        [Fact]
        public void CanCallData()
        {
            // Act
            var actionResult = this.controller.Data();
        }
    }
}
