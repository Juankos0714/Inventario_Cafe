using Xunit;
using FluentAssertions;
using InventorySystem.BLL;
using InventorySystem.Tests.TestHelpers;
using InventorySystem.Models.Enums;
using BCrypt.Net;

namespace InventorySystem.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly DatabaseTestHelper _dbHelper;
        private readonly TestDataBuilder _dataBuilder;

        public AuthServiceTests()
        {
            _dbHelper = new DatabaseTestHelper();
            _dataBuilder = new TestDataBuilder();
            _dbHelper.SeedTestData();
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void Login_WithValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            var authService = new AuthService();
            var username = "admin";
            var password = "admin123";

            // Act
            var result = authService.Login(username, password);

            // Assert
            result.Should().BeTrue("valid credentials should allow login");
            authService.CurrentUser.Should().NotBeNull("current user should be set after successful login");
            authService.CurrentUser!.Name.Should().Be(username, "current user name should match login username");
            authService.CurrentUser.Role.Should().Be(UserRole.Admin, "admin user should have Admin role");
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void Login_WithInvalidCredentials_ShouldReturnFalse()
        {
            // Arrange
            var authService = new AuthService();
            var username = "admin";
            var wrongPassword = "wrongpassword";

            // Act
            var result = authService.Login(username, wrongPassword);

            // Assert
            result.Should().BeFalse("invalid credentials should not allow login");
            authService.CurrentUser.Should().BeNull("current user should remain null after failed login");
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void Login_WithNonExistentUser_ShouldReturnFalse()
        {
            // Arrange
            var authService = new AuthService();
            var nonExistentUser = "nonexistent";
            var password = "anypassword";

            // Act
            var result = authService.Login(nonExistentUser, password);

            // Assert
            result.Should().BeFalse("non-existent user should not be able to login");
            authService.CurrentUser.Should().BeNull("current user should remain null");
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void Logout_WhenUserIsLoggedIn_ShouldClearCurrentUser()
        {
            // Arrange
            var authService = new AuthService();
            authService.Login("admin", "admin123");
            authService.CurrentUser.Should().NotBeNull("user should be logged in initially");

            // Act
            authService.Logout();

            // Assert
            authService.CurrentUser.Should().BeNull("current user should be null after logout");
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void ChangePassword_WithValidCurrentPassword_ShouldReturnTrue()
        {
            // Arrange
            var authService = new AuthService();
            authService.Login("admin", "admin123");
            var currentPassword = "admin123";
            var newPassword = "newpassword123";

            // Act
            var result = authService.ChangePassword(currentPassword, newPassword);

            // Assert
            result.Should().BeTrue("password change should succeed with valid current password");
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void ChangePassword_WithInvalidCurrentPassword_ShouldReturnFalse()
        {
            // Arrange
            var authService = new AuthService();
            authService.Login("admin", "admin123");
            var wrongCurrentPassword = "wrongpassword";
            var newPassword = "newpassword123";

            // Act
            var result = authService.ChangePassword(wrongCurrentPassword, newPassword);

            // Assert
            result.Should().BeFalse("password change should fail with invalid current password");
        }

        [Fact]
        [Trait("Category", "Authentication")]
        public void ChangePassword_WhenNotLoggedIn_ShouldReturnFalse()
        {
            // Arrange
            var authService = new AuthService();
            var currentPassword = "anypassword";
            var newPassword = "newpassword123";

            // Act
            var result = authService.ChangePassword(currentPassword, newPassword);

            // Assert
            result.Should().BeFalse("password change should fail when not logged in");
        }

        [Theory]
        [Trait("Category", "Authentication")]
        [InlineData("", "password")]
        [InlineData("username", "")]
        [InlineData("", "")]
        [InlineData(null, "password")]
        [InlineData("username", null)]
        public void Login_WithEmptyOrNullCredentials_ShouldReturnFalse(string? username, string? password)
        {
            // Arrange
            var authService = new AuthService();

            // Act
            var result = authService.Login(username!, password!);

            // Assert
            result.Should().BeFalse("login should fail with empty or null credentials");
            authService.CurrentUser.Should().BeNull("current user should remain null");
        }

        public void Dispose()
        {
            _dbHelper?.Dispose();
        }
    }
}