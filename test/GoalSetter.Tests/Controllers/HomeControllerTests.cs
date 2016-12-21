namespace GoalSetter
{
    using Controllers;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void Index() 
        {
            // Arrange
            var controller = new HomeController();

            // Act
            controller.Index();
        }
    }
}
