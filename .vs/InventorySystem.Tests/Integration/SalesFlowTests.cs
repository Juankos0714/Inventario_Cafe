using Xunit;
using FluentAssertions;
using InventorySystem.BLL;
using InventorySystem.Tests.TestHelpers;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.Tests.Integration
{
    public class SalesFlowTests : IDisposable
    {
        private readonly DatabaseTestHelper _dbHelper;
        private readonly TestDataBuilder _dataBuilder;
        private readonly AuthService _authService;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
        private readonly ShiftService _shiftService;

        public SalesFlowTests()
        {
            _dbHelper = new DatabaseTestHelper();
            _dataBuilder = new TestDataBuilder();
            _dbHelper.SeedTestData();
            
            _authService = new AuthService();
            _productService = new ProductService(_authService);
            _salesService = new SalesService(_authService);
            _shiftService = new ShiftService(_authService);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void CompleteSalesFlow_FromLoginToSale_ShouldWorkCorrectly()
        {
            // Arrange - Login as vendor
            var loginResult = _authService.Login("vendedor1", "pass123");
            loginResult.Should().BeTrue("vendor should be able to login");

            // Act & Assert - Start shift
            var shiftStartResult = _shiftService.StartShift();
            shiftStartResult.Should().BeTrue("vendor should be able to start shift");

            var activeShift = _shiftService.GetActiveShift();
            activeShift.Should().NotBeNull("there should be an active shift");

            // Get products for sale
            var products = _productService.GetAllProducts();
            products.Should().NotBeEmpty("there should be products available");

            var productToSell = products.First(p => p.StockActual > 0);
            var initialStock = productToSell.StockActual;

            // Create sale items
            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = productToSell.Id,
                    Quantity = 2,
                    UnitPrice = productToSell.Price,
                    Product = productToSell
                }
            };

            // Process sale
            var saleResult = _salesService.ProcessSale(saleItems);
            saleResult.Should().BeTrue("sale should be processed successfully");

            // Verify stock was updated
            var updatedProduct = _productService.GetProductById(productToSell.Id);
            updatedProduct!.StockActual.Should().Be(initialStock - 2, "stock should be reduced by sold quantity");

            // Verify sale was recorded
            var sales = _salesService.GetSalesByUser(_authService.CurrentUser!.Id, DateTime.Today);
            sales.Should().NotBeEmpty("sale should be recorded");
            sales.Should().Contain(s => s.ProductId == productToSell.Id, "sale should contain the sold product");

            // Close shift
            var shiftCloseResult = _shiftService.CloseShift();
            shiftCloseResult.Should().BeTrue("shift should close successfully");

            var closedShift = _shiftService.GetActiveShift();
            closedShift.Should().BeNull("there should be no active shift after closing");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void MultipleSalesInSameShift_ShouldAccumulateCorrectly()
        {
            // Arrange
            _authService.Login("vendedor1", "pass123");
            _shiftService.StartShift();

            var products = _productService.GetAllProducts().Where(p => p.StockActual > 0).Take(2).ToList();
            var expectedTotalAmount = 0m;

            // Act - Process multiple sales
            foreach (var product in products)
            {
                var saleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = product.Id,
                        Quantity = 1,
                        UnitPrice = product.Price,
                        Product = product
                    }
                };

                var result = _salesService.ProcessSale(saleItems);
                result.Should().BeTrue($"sale of {product.Name} should be processed successfully");
                
                expectedTotalAmount += product.Price;
            }

            // Assert - Verify all sales were recorded
            var allSales = _salesService.GetSalesByUser(_authService.CurrentUser!.Id, DateTime.Today);
            allSales.Should().HaveCount(products.Count, "all sales should be recorded");
            
            var totalSalesAmount = allSales.Sum(s => s.TotalAmount);
            totalSalesAmount.Should().Be(expectedTotalAmount, "total sales amount should match expected");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void SaleWithInsufficientStock_ShouldFailGracefully()
        {
            // Arrange
            _authService.Login("vendedor1", "pass123");
            _shiftService.StartShift();

            var products = _productService.GetAllProducts();
            var product = products.First();
            var initialStock = product.StockActual;

            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = product.StockActual + 10, // More than available
                    UnitPrice = product.Price,
                    Product = product
                }
            };

            // Act
            var result = _salesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeFalse("sale should fail with insufficient stock");
            
            // Verify stock was not changed
            var unchangedProduct = _productService.GetProductById(product.Id);
            unchangedProduct!.StockActual.Should().Be(initialStock, "stock should remain unchanged after failed sale");

            // Verify no sale was recorded
            var sales = _salesService.GetSalesByUser(_authService.CurrentUser!.Id, DateTime.Today);
            sales.Should().BeEmpty("no sale should be recorded for failed transaction");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void ConcurrentStockOperations_ShouldMaintainConsistency()
        {
            // Arrange
            _authService.Login("admin", "admin123");
            
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual >= 10);
            var initialStock = product.StockActual;

            // Act - Simulate concurrent operations
            var tasks = new List<Task<bool>>();

            // Add stock (admin operation)
            tasks.Add(Task.Run(() => _productService.UpdateStock(product.Id, 5, MovementType.Entrada)));
            
            // Adjust stock (admin operation)
            tasks.Add(Task.Run(() => _productService.UpdateStock(product.Id, initialStock + 3, MovementType.Ajuste)));

            Task.WaitAll(tasks.ToArray());

            // Assert
            var results = tasks.Select(t => t.Result).ToList();
            results.Should().Contain(true, "at least one operation should succeed");

            // Verify final stock is consistent
            var finalProduct = _productService.GetProductById(product.Id);
            finalProduct.Should().NotBeNull("product should still exist");
            finalProduct!.StockActual.Should().BeGreaterOrEqualTo(0, "stock should never be negative");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void AdminCanPerformAllOperations_VendorHasLimitedAccess()
        {
            // Test Admin capabilities
            _authService.Login("admin", "admin123");

            // Admin can create products
            var newProduct = _dataBuilder.CreateProduct(name: "Admin Product", stock: 10);
            var createResult = _productService.CreateProduct(newProduct);
            createResult.Should().BeTrue("admin should be able to create products");

            // Admin can start shifts
            var shiftResult = _shiftService.StartShift();
            shiftResult.Should().BeTrue("admin should be able to start shifts");

            // Admin can process sales
            var products = _productService.GetAllProducts();
            var productToSell = products.First(p => p.StockActual > 0);
            
            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = productToSell.Id,
                    Quantity = 1,
                    UnitPrice = productToSell.Price,
                    Product = productToSell
                }
            };

            var saleResult = _salesService.ProcessSale(saleItems);
            saleResult.Should().BeTrue("admin should be able to process sales");

            _shiftService.CloseShift();
            _authService.Logout();

            // Test Vendor limitations
            _authService.Login("vendedor1", "pass123");
            var vendorProductService = new ProductService(_authService);

            // Vendor cannot create products
            var vendorCreateResult = vendorProductService.CreateProduct(newProduct);
            vendorCreateResult.Should().BeFalse("vendor should not be able to create products");

            // But vendor can start shifts and process sales
            var vendorShiftService = new ShiftService(_authService);
            var vendorSalesService = new SalesService(_authService);

            var vendorShiftResult = vendorShiftService.StartShift();
            vendorShiftResult.Should().BeTrue("vendor should be able to start shifts");

            var vendorSaleResult = vendorSalesService.ProcessSale(saleItems);
            vendorSaleResult.Should().BeTrue("vendor should be able to process sales");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void DatabaseTransactionIntegrity_ShouldMaintainConsistency()
        {
            // Arrange
            _authService.Login("vendedor1", "pass123");
            _shiftService.StartShift();

            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            var initialStock = product.StockActual;

            // Act - Process a sale
            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    UnitPrice = product.Price,
                    Product = product
                }
            };

            var result = _salesService.ProcessSale(saleItems);

            // Assert - Verify all related data is consistent
            result.Should().BeTrue("sale should be processed successfully");

            // Check product stock was updated
            var updatedProduct = _productService.GetProductById(product.Id);
            updatedProduct!.StockActual.Should().Be(initialStock - 1, "product stock should be decremented");

            // Check movement was recorded
            var sales = _salesService.GetSalesByUser(_authService.CurrentUser!.Id, DateTime.Today);
            sales.Should().NotBeEmpty("sale movement should be recorded");
            
            var saleMovement = sales.First();
            saleMovement.ProductId.Should().Be(product.Id, "movement should reference correct product");
            saleMovement.Quantity.Should().Be(1, "movement should have correct quantity");
            saleMovement.TotalAmount.Should().Be(product.Price, "movement should have correct total amount");
            saleMovement.Type.Should().Be(MovementType.Venta, "movement should be of type Venta");
        }

        public void Dispose()
        {
            _dbHelper?.Dispose();
        }
    }
}