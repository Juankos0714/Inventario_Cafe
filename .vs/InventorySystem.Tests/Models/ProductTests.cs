using Xunit;
using FluentAssertions;
using InventorySystem.Models.Entities;
using InventorySystem.Tests.TestHelpers;

namespace InventorySystem.Tests.Models
{
    public class ProductTests
    {
        private readonly TestDataBuilder _dataBuilder;

        public ProductTests()
        {
            _dataBuilder = new TestDataBuilder();
        }

        [Fact]
        [Trait("Category", "Models")]
        public void Product_DefaultValues_ShouldBeSetCorrectly()
        {
            // Act
            var product = new Product();

            // Assert
            product.Id.Should().Be(0, "default ID should be 0");
            product.Name.Should().BeEmpty("default name should be empty");
            product.Category.Should().BeEmpty("default category should be empty");
            product.Price.Should().Be(0, "default price should be 0");
            product.StockActual.Should().Be(0, "default stock should be 0");
            product.StockMinimo.Should().Be(0, "default minimum stock should be 0");
            product.IsActive.Should().BeTrue("default active status should be true");
            product.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1), 
                "creation date should be set to now");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData("Café Expreso", "Bebidas", 3000, 50, 10, true)]
        [InlineData("Sandwich", "Comida", 8500, 20, 5, true)]
        [InlineData("Producto Inactivo", "Test", 1000, 0, 0, false)]
        public void Product_WithValidData_ShouldSetPropertiesCorrectly(
            string name, string category, decimal price, int stock, int minStock, bool isActive)
        {
            // Act
            var product = new Product
            {
                Name = name,
                Category = category,
                Price = price,
                StockActual = stock,
                StockMinimo = minStock,
                IsActive = isActive
            };

            // Assert
            product.Name.Should().Be(name, "name should be set correctly");
            product.Category.Should().Be(category, "category should be set correctly");
            product.Price.Should().Be(price, "price should be set correctly");
            product.StockActual.Should().Be(stock, "stock should be set correctly");
            product.StockMinimo.Should().Be(minStock, "minimum stock should be set correctly");
            product.IsActive.Should().Be(isActive, "active status should be set correctly");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void Product_CreatedWithBuilder_ShouldHaveValidData()
        {
            // Act
            var product = _dataBuilder.CreateProduct(
                name: "Test Product",
                category: "Test Category",
                price: 1500,
                stock: 25,
                minStock: 5
            );

            // Assert
            product.Should().NotBeNull("product should be created");
            product.Name.Should().Be("Test Product", "name should match builder input");
            product.Category.Should().Be("Test Category", "category should match builder input");
            product.Price.Should().Be(1500, "price should match builder input");
            product.StockActual.Should().Be(25, "stock should match builder input");
            product.StockMinimo.Should().Be(5, "minimum stock should match builder input");
            product.IsActive.Should().BeTrue("product should be active by default");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData(10, 5, false)] // Stock above minimum
        [InlineData(5, 5, true)]   // Stock equals minimum
        [InlineData(3, 5, true)]   // Stock below minimum
        [InlineData(0, 1, true)]   // No stock
        public void Product_StockComparison_ShouldIndicateLowStock(int actualStock, int minStock, bool expectedLowStock)
        {
            // Arrange
            var product = new Product
            {
                StockActual = actualStock,
                StockMinimo = minStock
            };

            // Act
            var isLowStock = product.StockActual <= product.StockMinimo;

            // Assert
            isLowStock.Should().Be(expectedLowStock, 
                $"stock comparison should correctly identify low stock status for {actualStock}/{minStock}");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void Product_WithNegativeValues_ShouldStillAcceptValues()
        {
            // Note: Business logic should prevent negative values, but the model itself should accept them
            // Act
            var product = new Product
            {
                Price = -100,
                StockActual = -5,
                StockMinimo = -1
            };

            // Assert
            product.Price.Should().Be(-100, "model should accept negative price (validation should be in business layer)");
            product.StockActual.Should().Be(-5, "model should accept negative stock (validation should be in business layer)");
            product.StockMinimo.Should().Be(-1, "model should accept negative minimum stock (validation should be in business layer)");
        }

        [Fact]
        [Trait("Category", "Models")]
        public void Product_WithLongStrings_ShouldAcceptValues()
        {
            // Arrange
            var longName = new string('A', 1000);
            var longCategory = new string('B', 500);

            // Act
            var product = new Product
            {
                Name = longName,
                Category = longCategory
            };

            // Assert
            product.Name.Should().Be(longName, "model should accept long names");
            product.Category.Should().Be(longCategory, "model should accept long categories");
        }

        [Theory]
        [Trait("Category", "Models")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Product_WithEmptyOrNullStrings_ShouldAcceptValues(string? value)
        {
            // Act
            var product = new Product
            {
                Name = value ?? string.Empty,
                Category = value ?? string.Empty
            };

            // Assert
            product.Name.Should().Be(value ?? string.Empty, "model should accept empty/null names");
            product.Category.Should().Be(value ?? string.Empty, "model should accept empty/null categories");
        }
    }
}