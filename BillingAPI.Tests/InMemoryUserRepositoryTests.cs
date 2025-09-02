using BillingAPI.Infrastructure.Repositories;
using FluentAssertions;

namespace BillingAPI.Tests
{
    public class InMemoryUserRepositoryTests
    {
        [Fact]
        public async Task ValidateCredentials_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var repo = new InMemoryUserRepository();

            // Act
            var result = await repo.ValidateCredentialsAsync("test", "1234");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateCredentials_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var repo = new InMemoryUserRepository();

            // Act
            var result = await repo.ValidateCredentialsAsync("test", "wrong");

            // Assert
            result.Should().BeFalse();
        }
    }
}
