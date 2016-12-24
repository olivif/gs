namespace GoalSetter.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GoalSetter.Data;
    using Microsoft.EntityFrameworkCore;
    using ModelsLogic;
    using Moq;
    using Storage;
    using Xunit;

    public class DatabaseGoalStorageTests
    {
        private readonly Mock<GoalsDbContext> dbContextMock;

        private readonly IGoalStorage dataStorage;

        public DatabaseGoalStorageTests()
        {
            this.dbContextMock = new Mock<GoalsDbContext>();

            this.dataStorage = new DatabaseGoalStorage(this.dbContextMock.Object);
        }

        [Fact]
        public void Read()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var notUserId = Guid.NewGuid();
            var goal = new Goal();
            var goals = new List<Goal>
            {
                new Goal() { UserId = userId },
                new Goal() { UserId = userId },
                new Goal() { UserId = notUserId },
            };

            var dbSetMock = AsMockDbSet(goals);
            this.dbContextMock.SetupGet(x => x.Goals).Returns(dbSetMock.Object);

            // Act
            var returnedGoals = this.dataStorage.Read(userId);

            // Assert
            Assert.Equal(2, returnedGoals.Count);
        }

        private static Mock<DbSet<T>> AsMockDbSet<T>(IEnumerable<T> data, bool callBase = true) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            var queryable = data.AsQueryable();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockSet.CallBase = callBase;

            return mockSet;
        }
    }
}
