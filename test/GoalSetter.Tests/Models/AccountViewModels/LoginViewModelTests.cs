namespace GoalSetter.Models.AccountViewModels
{
    using Xunit;

    public class LoginViewModelTests
    {
        [Fact]
        public void ConstructorDefault()
        {
            // Act
            new LoginViewModel();
        }

        [Fact]
        public void ConstructorParams()
        {
            // Act
            var email = "email";
            var password = "password";
            var model = new LoginViewModel()
            {
                Email = email,
                Password = password
            };
        }

        [Fact]
        public void Email()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var model = new LoginViewModel()
            {
                Email = email,
                Password = password
            };

            // Act
            var foundEmail = model.Email;

            // Assert
            Assert.Equal(email, foundEmail);
        }

        [Fact]
        public void Password()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var model = new LoginViewModel()
            {
                Email = email,
                Password = password
            };

            // Act
            var foundPassword = model.Password;

            // Assert
            Assert.Equal(password, foundPassword);
        }
    }
}
