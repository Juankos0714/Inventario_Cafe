using Xunit;
using FluentAssertions;
using InventorySystem.BLL;
using InventorySystem.Tests.TestHelpers;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.Tests.Services
{
    public class ProductServiceTests : IDisposable
    {
        private readonly DatabaseTestHelper _dbHelper;
        private readonly TestDataBuilder _dataBuilder;
        private readonly AuthService _authService;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _dbHelper = new DatabaseTestHelper();
            _dataBuilder = new TestDataBuilder();
            _dbHelper.SeedTestData();
            
            _authService = new AuthService();
            _authService.Login("admin", "admin123"); // Login as admin for tests
            _productService = new ProductService(_authService);
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void GetAllProducts_ShouldReturnAllActiveProducts()
        {
            // Act
            var products = _productService.GetAllProducts();

            // Assert
            products.Should().NotBeEmpty("there should be products in the database");
            products.Should().OnlyContain(p => p.IsActive, "only active products should be returned");
            products.Should().Contain(p => p.Name == "Café Expreso", "seeded product should be present");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void CreateProduct_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var newProduct = _dataBuilder.CreateProduct(
                name: "Nuevo Producto",
                category: "Test",
                price: 5000,
                stock: 10,
                minStock: 2
            );

            // Act
            var result = _productService.CreateProduct(newProduct);

            // Assert
            result.Should().BeTrue("product creation should succeed with valid data");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void CreateProduct_AsNonAdmin_ShouldReturnFalse()
        {
            // Arrange
            var vendorAuthService = new AuthService();
            vendorAuthService.Login("vendedor1", "pass123");
            var vendorProductService = new ProductService(vendorAuthService);
            
            var newProduct = _dataBuilder.CreateProduct();

            // Act
            var result = vendorProductService.CreateProduct(newProduct);

            // Assert
            result.Should().BeFalse("only admins should be able to create products");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void UpdateStock_WithValidQuantity_ShouldReturnTrue()
        {
            // Arrange
            var products = _productService.GetAllProducts();
            var product = products.First();
            var newStock = 100;

            // Act
            var result = _productService.UpdateStock(product.Id, newStock, MovementType.Ajuste);

            // Assert
            result.Should().BeTrue("stock update should succeed with valid quantity");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void UpdateStock_WithNegativeResult_ShouldReturnFalse()
        {
            // Arrange
            var products = _productService.GetAllProducts();
            var product = products.First();
            var excessiveQuantity = product.StockActual + 1000; // More than available

            // Act
            var result = _productService.UpdateStock(product.Id, excessiveQuantity, MovementType.Venta);

            // Assert
            result.Should().BeFalse("stock should not go negative");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void GetLowStockProducts_ShouldReturnProductsBelowMinimum()
        {
            // Act
            var lowStockProducts = _productService.GetLowStockProducts();

            // Assert
            lowStockProducts.Should().NotBeNull("low stock products list should not be null");
            lowStockProducts.Should().OnlyContain(p => p.StockActual <= p.StockMinimo, 
                "only products with stock at or below minimum should be returned");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void ValidateStockForSale_WithSufficientStock_ShouldReturnTrue()
        {
            // Arrange
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            var requestedQuantity = 1;

            // Act
            var result = _productService.ValidateStockForSale(product.Id, requestedQuantity);

            // Assert
            result.Should().BeTrue("validation should pass when sufficient stock is available");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void ValidateStockForSale_WithInsufficientStock_ShouldReturnFalse()
        {
            // Arrange
            var products = _productService.GetAllProducts();
            var product = products.First();
            var excessiveQuantity = product.StockActual + 1;

            // Act
            var result = _productService.ValidateStockForSale(product.Id, excessiveQuantity);

            // Assert
            result.Should().BeFalse("validation should fail when insufficient stock is available");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void GetProductById_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var products = _productService.GetAllProducts();
            var expectedProduct = products.First();

            // Act
            var result = _productService.GetProductById(expectedProduct.Id);

            // Assert
            result.Should().NotBeNull("product should be found with valid ID");
            result!.Id.Should().Be(expectedProduct.Id, "returned product should have correct ID");
            result.Name.Should().Be(expectedProduct.Name, "returned product should have correct name");
        }

        [Fact]
        [Trait("Category", "ProductManagement")]
        public void GetProductById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = 99999;

            // Act
            var result = _productService.GetProductById(invalidId);

            // Assert
            result.Should().BeNull("product should not be found with invalid ID");
        }

        [Theory]
        [Trait("Category", "ProductManagement")]
        [InlineData(MovementType.Entrada, 10, 60)] // Adding stock
        [InlineData(MovementType.Venta, 5, 45)]    // Selling stock
        [InlineData(MovementType.Ajuste, 25, 25)]  // Adjusting to specific amount
        public void UpdateStock_WithDifferentMovementTypes_ShouldCalculateCorrectly(
            MovementType movementType, int quantity, int expectedFinalStock)
        {
            // Arrange
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual == 50); // Assuming we have a product with 50 stock
            
            if (product == null)
            {
                // Create a product with known stock for this test
                var testProduct = _dataBuilder.CreateProduct(stock: 50);
                _productService.CreateProduct(testProduct);
                products = _productService.GetAllProducts();
                product = products.Last(); // Get the newly created product
            }

            // Act
            var result = _productService.UpdateStock(product.Id, quantity, movementType);

            // Assert
            result.Should().BeTrue($"stock update should succeed for {movementType}");
            
            // Verify the stock was updated correctly
            var updatedProduct = _productService.GetProductById(product.Id);
            updatedProduct!.StockActual.Should().Be(expectedFinalStock, 
                $"stock should be correctly calculated for {movementType}");
        }

        public void Dispose()
        {
            _dbHelper?.Dispose();
        }
    }
}