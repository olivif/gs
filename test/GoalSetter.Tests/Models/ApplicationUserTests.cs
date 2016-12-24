namespace GoalSetter.Models
{
    using Xunit;

    public class ApplicationUserTests
    {
        [Fact]
        public void ConstructorDefault()
        {
            // Act
            new ApplicationUser();
        }

        [Fact]
        public void HasIdDefaultConstructor()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act
            var id = user.Id;

            // Assert
            Assert.NotNull(id);
            Assert.NotEmpty(id);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("email", "password")]
        public void HasId(string email, string password)
        {
            // Arrange
            var user = new ApplicationUser
            {
                UserName = email,
                Email = password
            };

            // Act
            var id = user.Id;

            // Assert
            Assert.NotNull(id);
            Assert.NotEmpty(id);
        }

        [Theory]
        [InlineData("email1", "password1", "email1", "password1")]
        [InlineData("email1", "password1", "email2", "password2")]
        public void HasDifferentId(
            string email1, 
            string password1,
            string email2,
            string password2)
        {
            // Arrange
            var user1 = new ApplicationUser
            {
                UserName = email1,
                Email = password1
            };
            var user2 = new ApplicationUser
            {
                UserName = email2,
                Email = password2
            };

            // Act
            var idsEqual = user1.Id == user2.Id;

            // Assert
            Assert.False(idsEqual);
        }
    }
}
