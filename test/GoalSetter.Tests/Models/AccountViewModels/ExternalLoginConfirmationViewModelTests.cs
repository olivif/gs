namespace GoalSetter.Models.AccountViewModels
{
    using Xunit;

    public class ExternalLoginConfirmationViewModelTests
    {
        [Fact]
        public void ConstructorDefault()
        {
            // Act
            new ExternalLoginConfirmationViewModel();
        }

        [Fact]
        public void ConstructorParams()
        {
            // Act
            var email = "email";
            var model = new ExternalLoginConfirmationViewModel()
            {
                Email = email
            };
        }

        [Fact]
        public void Email()
        {
            // Arrange
            var email = "email";
            var model = new ExternalLoginConfirmationViewModel()
            {
                Email = email
            };

            // Act
            var foundEmail = model.Email;

            // Assert
            Assert.Equal(email, foundEmail);
        }
    }
}
