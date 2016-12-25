namespace GoalSetter.Data
{
    using Microsoft.EntityFrameworkCore;
    using ModelsLogic;
    using Xunit;

    public class GoalsDbContextTests
    {
        private readonly GoalsDbContext dbContext;

        public GoalsDbContextTests()
        {
            var options = new DbContextOptions<GoalsDbContext>();
            this.dbContext = new GoalsDbContext(options);
        }

        [Fact]
        public void Constructor()
        {
            // Arrange
            var options = new DbContextOptions<GoalsDbContext>();

            // Act
            var dbContext = new GoalsDbContext(options);
        }

        [Fact]
        public void HasGoals()
        {
            // Act
            var goalsDbSet = this.dbContext.Goals;

            // Assert
            Assert.True(goalsDbSet is DbSet<Goal>);
        }
    }
}
