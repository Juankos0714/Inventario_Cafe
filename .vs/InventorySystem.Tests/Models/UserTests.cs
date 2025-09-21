using Xunit;
using FluentAssertions;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;
using InventorySystem.Tests.TestHelpers;
using BCrypt.Net;

namespace InventorySystem.Tests.Models
{
    public class UserTests
    {
        private readonly TestDataBuilder _dataBuilder;

        public UserTests()
        {
            _dataBuilder = new TestDataBuilder();
        }

        [Fact]
        [Trait("Category", "Models")]
        public void User_DefaultValues_ShouldBeSetCorrectly()
        {
            // Act
            var user = new User();

            // Assert
            user.Id.Should().Be(0, "default ID should be 0");
            user.Name.Should().BeEmpty("default name should be empty");
            user.PasswordHash.Should().BeEmpty("default password hash should be empty");
            user.Role.Should().Be(UserRole.Admin, "default role should be Admin (first enum value)");
            user.IsActive.Should().BeTrue("default active status should be true");
            user.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1), 
                "creation date should be set to now");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData("admin", UserRole.Admin, true)]
        [InlineData("vendedor1", UserRole.Vendedor, true)]
        [InlineData("contador1", UserRole.Contador, true)]
        [InlineData("inactive_user", UserRole.Vendedor, false)]
        public void User_WithValidData_ShouldSetPropertiesCorrectly(
            string name, UserRole role, bool isActive)
        {
            // Arrange
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("testpassword");

            // Act
            var user = new User
            {
                Name = name,
                PasswordHash = passwordHash,
                Role = role,
                IsActive = isActive
            };

            // Assert
            user.Name.Should().Be(name, "name should be set correctly");
            user.PasswordHash.Should().Be(passwordHash, "password hash should be set correctly");
            user.Role.Should().Be(role, "role should be set correctly");
            user.IsActive.Should().Be(isActive, "active status should be set correctly");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void User_CreatedWithBuilder_ShouldHaveValidData()
        {
            // Act
            var user = _dataBuilder.CreateUser(
                name: "TestUser",
                passwordHash: "hashedpassword",
                role: UserRole.Vendedor,
                isActive: true
            );

            // Assert
            user.Should().NotBeNull("user should be created");
            user.Name.Should().Be("TestUser", "name should match builder input");
            user.PasswordHash.Should().Be("hashedpassword", "password hash should match builder input");
            user.Role.Should().Be(UserRole.Vendedor, "role should match builder input");
            user.IsActive.Should().BeTrue("active status should match builder input");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData(UserRole.Admin)]
        [InlineData(UserRole.Vendedor)]
        [InlineData(UserRole.Contador)]
        public void User_WithDifferentRoles_ShouldSetRoleCorrectly(UserRole role)
        {
            // Act
            var user = new User { Role = role };

            // Assert
            user.Role.Should().Be(role, $"role should be set to {role}");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void User_PasswordHashing_ShouldWorkCorrectly()
        {
            // Arrange
            var plainPassword = "mySecretPassword123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            // Act
            var user = new User
            {
                Name = "testuser",
                PasswordHash = hashedPassword
            };

            // Assert
            user.PasswordHash.Should().NotBe(plainPassword, "password should be hashed, not stored in plain text");
            user.PasswordHash.Should().StartWith("$2", "BCrypt hash should start with $2");
            BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash).Should().BeTrue(
                "original password should verify against the hash");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void User_WithEmptyOrNullName_ShouldAcceptValues(string? name)
        {
            // Act
            var user = new User
            {
                Name = name ?? string.Empty
            };

            // Assert
            user.Name.Should().Be(name ?? string.Empty, "model should accept empty/null names");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void User_WithLongName_ShouldAcceptValue()
        {
            // Arrange
            var longName = new string('A', 1000);

            // Act
            var user = new User
            {
                Name = longName
            };

            // Assert
            user.Name.Should().Be(longName, "model should accept long names");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void User_CreatedAt_ShouldBeSetAutomatically()
        {
            // Arrange
            var beforeCreation = DateTime.Now;

            // Act
            var user = new User();
            var afterCreation = DateTime.Now;

            // Assert
            user.CreatedAt.Should().BeOnOrAfter(beforeCreation, "creation time should be after test start");
            user.CreatedAt.Should().BeOnOrBefore(afterCreation, "creation time should be before test end");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData(true)]
        [InlineData(false)]
        public void User_IsActive_ShouldAcceptBothValues(bool isActive)
        {
            // Act
            var user = new User { IsActive = isActive };

            // Assert
            user.IsActive.Should().Be(isActive, $"IsActive should be set to {isActive}");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void User_MultipleInstances_ShouldHaveUniqueCreationTimes()
        {
            // Act
            var user1 = new User();
            Thread.Sleep(1); // Ensure different timestamps
            var user2 = new User();

            // Assert
            user1.CreatedAt.Should().NotBe(user2.CreatedAt, "different users should have different creation times");
            user1.CreatedAt.Should().BeBefore(user2.CreatedAt, "first user should be created before second user");
        }
    }
}