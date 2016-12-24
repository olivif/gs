namespace GoalSetter.Service
{
    using GoalSetter.Data;
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
        public void Constructor()
        {
            // Arrange
            var goal = new Goal();

            // Act
            this.dataStorage.Create(goal);
        }
    }
}
