using Xunit;
using FluentAssertions;
using InventorySystem.BLL;
using InventorySystem.Tests.TestHelpers;
using InventorySystem.Models.Enums;

namespace InventorySystem.Tests.Services
{
    public class ShiftServiceTests : IDisposable
    {
        private readonly DatabaseTestHelper _dbHelper;
        private readonly TestDataBuilder _dataBuilder;
        private readonly AuthService _authService;
        private readonly ShiftService _shiftService;

        public ShiftServiceTests()
        {
            _dbHelper = new DatabaseTestHelper();
            _dataBuilder = new TestDataBuilder();
            _dbHelper.SeedTestData();
            
            _authService = new AuthService();
            _authService.Login("vendedor1", "pass123");
            _shiftService = new ShiftService(_authService);
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void StartShift_WhenNoActiveShift_ShouldReturnTrue()
        {
            // Act
            var result = _shiftService.StartShift();

            // Assert
            result.Should().BeTrue("shift should start successfully when no active shift exists");
            
            var activeShift = _shiftService.GetActiveShift();
            activeShift.Should().NotBeNull("there should be an active shift after starting");
            activeShift!.UserId.Should().Be(_authService.CurrentUser!.Id, "shift should belong to current user");
            activeShift.IsClosed.Should().BeFalse("new shift should not be closed");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void StartShift_WhenActiveShiftExists_ShouldReturnFalse()
        {
            // Arrange
            _shiftService.StartShift(); // Start first shift

            // Act
            var result = _shiftService.StartShift(); // Try to start another

            // Assert
            result.Should().BeFalse("should not be able to start shift when one is already active");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void StartShift_AsUnauthorizedRole_ShouldReturnFalse()
        {
            // Arrange
            var counterAuthService = new AuthService();
            counterAuthService.Login("contador1", "pass123"); // Counter role
            var counterShiftService = new ShiftService(counterAuthService);

            // Act
            var result = counterShiftService.StartShift();

            // Assert
            result.Should().BeFalse("counter role should not be able to start shifts");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void CloseShift_WithActiveShift_ShouldReturnTrue()
        {
            // Arrange
            _shiftService.StartShift();
            var activeShift = _shiftService.GetActiveShift();
            activeShift.Should().NotBeNull("shift should be active before closing");

            // Act
            var result = _shiftService.CloseShift();

            // Assert
            result.Should().BeTrue("shift should close successfully");
            
            var closedShift = _shiftService.GetActiveShift();
            closedShift.Should().BeNull("there should be no active shift after closing");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void CloseShift_WithoutActiveShift_ShouldReturnFalse()
        {
            // Act
            var result = _shiftService.CloseShift();

            // Assert
            result.Should().BeFalse("should not be able to close shift when none is active");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void GetActiveShift_WhenShiftExists_ShouldReturnShift()
        {
            // Arrange
            _shiftService.StartShift();

            // Act
            var activeShift = _shiftService.GetActiveShift();

            // Assert
            activeShift.Should().NotBeNull("should return the active shift");
            activeShift!.UserId.Should().Be(_authService.CurrentUser!.Id, "shift should belong to current user");
            activeShift.StartTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1), 
                "shift start time should be recent");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void GetActiveShift_WhenNoShiftExists_ShouldReturnNull()
        {
            // Act
            var activeShift = _shiftService.GetActiveShift();

            // Assert
            activeShift.Should().BeNull("should return null when no active shift exists");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void GetShiftsByDate_AsAdmin_ShouldReturnAllShifts()
        {
            // Arrange
            var adminAuthService = new AuthService();
            adminAuthService.Login("admin", "admin123");
            var adminShiftService = new ShiftService(adminAuthService);
            
            // Start and close a shift
            _shiftService.StartShift();
            _shiftService.CloseShift();

            // Act
            var shifts = adminShiftService.GetShiftsByDate(DateTime.Today);

            // Assert
            shifts.Should().NotBeEmpty("admin should be able to see all shifts");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void GetShiftsByDate_AsNonAdmin_ShouldReturnEmptyList()
        {
            // Arrange
            _shiftService.StartShift();
            _shiftService.CloseShift();

            // Act
            var shifts = _shiftService.GetShiftsByDate(DateTime.Today);

            // Assert
            shifts.Should().BeEmpty("non-admin users should not see all shifts");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void ShiftWorkflow_CompleteFlow_ShouldWorkCorrectly()
        {
            // Arrange & Act - Start shift
            var startResult = _shiftService.StartShift();
            startResult.Should().BeTrue("shift should start successfully");

            // Verify active shift
            var activeShift = _shiftService.GetActiveShift();
            activeShift.Should().NotBeNull("shift should be active");
            var shiftId = activeShift!.Id;

            // Close shift
            var closeResult = _shiftService.CloseShift();
            closeResult.Should().BeTrue("shift should close successfully");

            // Verify no active shift
            var noActiveShift = _shiftService.GetActiveShift();
            noActiveShift.Should().BeNull("no shift should be active after closing");

            // Assert
            // Complete workflow should execute without issues
            startResult.Should().BeTrue();
            closeResult.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void StartShift_WhenNotLoggedIn_ShouldReturnFalse()
        {
            // Arrange
            var noUserAuthService = new AuthService(); // Not logged in
            var noUserShiftService = new ShiftService(noUserAuthService);

            // Act
            var result = noUserShiftService.StartShift();

            // Assert
            result.Should().BeFalse("should not be able to start shift when not logged in");
        }

        [Fact]
        [Trait("Category", "ShiftManagement")]
        public void CloseShift_WhenNotLoggedIn_ShouldReturnFalse()
        {
            // Arrange
            var noUserAuthService = new AuthService(); // Not logged in
            var noUserShiftService = new ShiftService(noUserAuthService);

            // Act
            var result = noUserShiftService.CloseShift();

            // Assert
            result.Should().BeFalse("should not be able to close shift when not logged in");
        }

        public void Dispose()
        {
            _dbHelper?.Dispose();
        }
    }
}