namespace GoalSetter.Service.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GoalSetter.Service.Storage;
    using ModelsLogic;
    using Moq;
    using Xunit;

    public class GoalManagerTests
    {
        private readonly Mock<IGoalStorage> goalStorageMock;
        private readonly GoalManager goalManager;

        public GoalManagerTests()
        {
            this.goalStorageMock = new Mock<IGoalStorage>();
            this.goalManager = new GoalManager(
                this.goalStorageMock.Object);
        }

        [Fact]
        public void CallsStorageCreate()
        {
            // Arrange
            var goal = new Goal();

            // Act
            this.goalManager.Create(goal);

            // Assert
            this.goalStorageMock.Verify(x => x.Create(goal), Times.Once);
        }

        [Fact]
        public void CreateReturnsTrue()
        {
            // Arrange
            var goal = new Goal();

            // Act
            var createSucceeded = this.goalManager.Create(goal);

            // Assert
            Assert.True(createSucceeded);
        }

        [Fact]
        public void CallsStorageRead()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            this.goalManager.Read(userId);

            // Assert
            this.goalStorageMock.Verify(x => x.Read(userId), Times.Once);
        }

        [Fact]
        public void ReturnsWhatStorageReturned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var goals = new List<Goal>
            {
                new Goal() { UserId = userId },
                new Goal() { UserId = userId },
            };
            this.goalStorageMock.Setup(x => x.Read(userId)).Returns(goals);

            // Act
            var returnedGoals = this.goalManager.Read(userId);

            // Assert
            Assert.Equal(goals.Count, returnedGoals.Count);
            Assert.True(Enumerable.SequenceEqual(goals, returnedGoals));
        }
    }
}
