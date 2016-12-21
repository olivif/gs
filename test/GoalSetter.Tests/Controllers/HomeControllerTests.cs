namespace GoalSetter
{
    using Controllers;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void CanCallIndex() 
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var actionResult = controller.Index();
        }

        [Fact]
        public void CanCallError()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var actionResult = controller.Error();
        }

        [Fact]
        public void Fail()
        {
            Assert.True(false);
        }
    }
}
