using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using TestShop.Application.DTOs.Auth;
using TestShop.Application.Interfaces;
using TestShop.Domain.Entities;
using TestShop.Infrastructure.Services;

namespace TestShop.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task Login_WithInvalidPassword_Throws_UnauthorizedAccessException()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "demo", Email = "demo@test.com" };
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "Pass123!");

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(r => r.GetByUsernameAsync("demo", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

            var jwt = Options.Create(new JwtOptions
            {
                Issuer = "Test",
                Audience = "Test",
                Key = "0123456789abcdef0123456789abcdef",
                ExpMinutes = 60
            });

            var sut = new AuthService(userRepo.Object, hasher, jwt);

            // Act
            Func<Task> act = async () => await sut.LoginAsync(new LoginRequest("demo", "wrong"), default);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
