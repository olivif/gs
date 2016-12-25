namespace GoalSetter.Controllers
{
    using System;
    using System.Security.Claims;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Goals;
    using ModelsLogic;
    using Moq;
    using Service.Manager;
    using Xunit;

    public class HomeControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

        private readonly GoalsDbContext goalsDbContext;

        private readonly Mock<IGoalManager> goalManagerMock;

        private readonly HomeController controller;

        private static Guid FakeUserId = new Guid("8f2b6d4c-ccdf-44a8-9691-c8d8f33f068e");

        public HomeControllerTests()
        {
            this.userManagerMock = IdentityTestUtils.MockUserManager<ApplicationUser>(); ;

            var options = new DbContextOptions<GoalsDbContext>();
            this.goalsDbContext = new GoalsDbContext(options);

            this.goalManagerMock = new Mock<IGoalManager>();

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

        [Fact]
        public void Data()
        {
            // Arrange
            var goalModelData = "data";
            var goalModel = new GoalViewModel()
            {
                Data = goalModelData
            };

            var userId = FakeUserId;

            this.userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId.ToString());

            // Act
            var actionResult = this.controller.Data(goalModel);

            // Assert
            this.goalManagerMock.Verify(
                x => x.Create(It.Is<Goal>(
                    g => g.Data == goalModelData && 
                         g.UserId == userId && 
                         g.GoalId != Guid.Empty)), 
                Times.Once);
        }
    }
}
