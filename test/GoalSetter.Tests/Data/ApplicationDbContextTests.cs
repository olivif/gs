namespace GoalSetter.Data
{
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ApplicationDbContextTests
    {
        [Fact]
        public void Constructor()
        {
            // Arrange
            var options = new DbContextOptions<ApplicationDbContext>();

            // Act
            var dbContext = new ApplicationDbContext(options);
        }
    }
}
