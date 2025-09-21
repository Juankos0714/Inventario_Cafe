using Xunit;
using FluentAssertions;
using InventorySystem.BLL;
using InventorySystem.Tests.TestHelpers;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.Tests.Services
{
    public class SalesServiceTests : IDisposable
    {
        private readonly DatabaseTestHelper _dbHelper;
        private readonly TestDataBuilder _dataBuilder;
        private readonly AuthService _authService;
        private readonly SalesService _salesService;
        private readonly ShiftService _shiftService;
        private readonly ProductService _productService;

        public SalesServiceTests()
        {
            _dbHelper = new DatabaseTestHelper();
            _dataBuilder = new TestDataBuilder();
            _dbHelper.SeedTestData();
            
            _authService = new AuthService();
            _authService.Login("vendedor1", "pass123"); // Login as vendor
            _salesService = new SalesService(_authService);
            _shiftService = new ShiftService(_authService);
            _productService = new ProductService(_authService);
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void ProcessSale_WithValidSingleItem_ShouldReturnTrue()
        {
            // Arrange
            _shiftService.StartShift(); // Start shift for sales
            
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            
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

            // Act
            var result = _salesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeTrue("sale should be processed successfully with valid items");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void ProcessSale_WithMultipleItems_ShouldReturnTrue()
        {
            // Arrange
            _shiftService.StartShift();
            
            var products = _productService.GetAllProducts().Where(p => p.StockActual > 0).Take(2).ToList();
            
            var saleItems = products.Select(p => new SaleItem
            {
                ProductId = p.Id,
                Quantity = 1,
                UnitPrice = p.Price,
                Product = p
            }).ToList();

            // Act
            var result = _salesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeTrue("sale should be processed successfully with multiple items");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void ProcessSale_WithInsufficientStock_ShouldReturnFalse()
        {
            // Arrange
            _shiftService.StartShift();
            
            var products = _productService.GetAllProducts();
            var product = products.First();
            
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
            result.Should().BeFalse("sale should fail when insufficient stock is available");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void ProcessSale_WithoutActiveShift_ShouldReturnFalse()
        {
            // Arrange - No shift started
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            
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

            // Act
            var result = _salesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeFalse("sale should fail without an active shift");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void ProcessSale_AsUnauthorizedRole_ShouldReturnFalse()
        {
            // Arrange
            var counterAuthService = new AuthService();
            counterAuthService.Login("contador1", "pass123"); // Login as counter (not authorized for sales)
            var counterSalesService = new SalesService(counterAuthService);
            
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            
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

            // Act
            var result = counterSalesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeFalse("counter role should not be able to process sales");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void ProcessSale_ShouldUpdateStockCorrectly()
        {
            // Arrange
            _shiftService.StartShift();
            
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 5);
            var initialStock = product.StockActual;
            var quantityToSell = 2;
            
            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = quantityToSell,
                    UnitPrice = product.Price,
                    Product = product
                }
            };

            // Act
            var result = _salesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeTrue("sale should be processed successfully");
            
            // Verify stock was updated
            var updatedProduct = _productService.GetProductById(product.Id);
            updatedProduct!.StockActual.Should().Be(initialStock - quantityToSell, 
                "stock should be reduced by the sold quantity");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void GetSalesByUser_ShouldReturnUserSales()
        {
            // Arrange
            _shiftService.StartShift();
            
            // Process a sale first
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            
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
            
            _salesService.ProcessSale(saleItems);

            // Act
            var sales = _salesService.GetSalesByUser(_authService.CurrentUser!.Id, DateTime.Today);

            // Assert
            sales.Should().NotBeEmpty("there should be sales for the current user");
            sales.Should().OnlyContain(s => s.Type == MovementType.Venta, "only sale movements should be returned");
            sales.Should().OnlyContain(s => s.UserId == _authService.CurrentUser.Id, 
                "only sales from the specified user should be returned");
        }

        [Fact]
        [Trait("Category", "Sales")]
        public void GetSalesByDateRange_ShouldReturnSalesInRange()
        {
            // Arrange
            _shiftService.StartShift();
            
            // Process a sale
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual > 0);
            
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
            
            _salesService.ProcessSale(saleItems);

            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(1);

            // Act
            var sales = _salesService.GetSalesByDateRange(startDate, endDate);

            // Assert
            sales.Should().NotBeEmpty("there should be sales in the date range");
            sales.Should().OnlyContain(s => s.Type == MovementType.Venta, "only sale movements should be returned");
            sales.Should().OnlyContain(s => s.Date.Date >= startDate.Date && s.Date.Date <= endDate.Date,
                "only sales within the date range should be returned");
        }

        [Theory]
        [Trait("Category", "Sales")]
        [InlineData(1, 5000, 5000)]
        [InlineData(2, 3000, 6000)]
        [InlineData(3, 2500, 7500)]
        public void ProcessSale_ShouldCalculateTotalAmountCorrectly(int quantity, decimal unitPrice, decimal expectedTotal)
        {
            // Arrange
            _shiftService.StartShift();
            
            var products = _productService.GetAllProducts();
            var product = products.First(p => p.StockActual >= quantity);
            
            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Product = product
                }
            };

            // Act
            var result = _salesService.ProcessSale(saleItems);

            // Assert
            result.Should().BeTrue("sale should be processed successfully");
            
            // Verify the movement was created with correct total
            var sales = _salesService.GetSalesByUser(_authService.CurrentUser!.Id, DateTime.Today);
            var latestSale = sales.OrderByDescending(s => s.Date).First();
            latestSale.TotalAmount.Should().Be(expectedTotal, "total amount should be calculated correctly");
        }

        public void Dispose()
        {
            _dbHelper?.Dispose();
        }
    }
}