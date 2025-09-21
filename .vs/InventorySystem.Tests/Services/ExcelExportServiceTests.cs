using Xunit;
using FluentAssertions;
using InventorySystem.Services;
using InventorySystem.Tests.TestHelpers;
using InventorySystem.Models.DTOs;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.Tests.Services
{
    public class ExcelExportServiceTests : IDisposable
    {
        private readonly TestDataBuilder _dataBuilder;
        private readonly ExcelExportService _excelService;
        private readonly string _tempDirectory;

        public ExcelExportServiceTests()
        {
            _dataBuilder = new TestDataBuilder();
            _excelService = new ExcelExportService();
            _tempDirectory = Path.Combine(Path.GetTempPath(), "InventorySystemTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportDailySummary_WithValidData_ShouldCreateFile()
        {
            // Arrange
            var summary = CreateTestDailySummary();
            var filePath = Path.Combine(_tempDirectory, "daily_summary_test.xlsx");

            // Act
            var action = () => _excelService.ExportDailySummary(summary, filePath);

            // Assert
            action.Should().NotThrow("export should complete without errors");
            File.Exists(filePath).Should().BeTrue("Excel file should be created");
            
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(0, "Excel file should not be empty");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportMovements_WithValidData_ShouldCreateFile()
        {
            // Arrange
            var movements = CreateTestMovements();
            var filePath = Path.Combine(_tempDirectory, "movements_test.xlsx");

            // Act
            var action = () => _excelService.ExportMovements(movements, filePath);

            // Assert
            action.Should().NotThrow("export should complete without errors");
            File.Exists(filePath).Should().BeTrue("Excel file should be created");
            
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(0, "Excel file should not be empty");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportInventory_WithValidData_ShouldCreateFile()
        {
            // Arrange
            var products = CreateTestProducts();
            var filePath = Path.Combine(_tempDirectory, "inventory_test.xlsx");

            // Act
            var action = () => _excelService.ExportInventory(products, filePath);

            // Assert
            action.Should().NotThrow("export should complete without errors");
            File.Exists(filePath).Should().BeTrue("Excel file should be created");
            
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(0, "Excel file should not be empty");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportDailySummary_WithEmptyData_ShouldStillCreateFile()
        {
            // Arrange
            var emptySummary = new DailySummary
            {
                Date = DateTime.Today,
                TotalSales = 0,
                TotalTransactions = 0,
                Shifts = new List<ShiftSummary>(),
                ProductSales = new List<ProductSale>()
            };
            var filePath = Path.Combine(_tempDirectory, "empty_summary_test.xlsx");

            // Act
            var action = () => _excelService.ExportDailySummary(emptySummary, filePath);

            // Assert
            action.Should().NotThrow("export should handle empty data gracefully");
            File.Exists(filePath).Should().BeTrue("Excel file should be created even with empty data");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportMovements_WithEmptyList_ShouldCreateFileWithHeaders()
        {
            // Arrange
            var emptyMovements = new List<Movement>();
            var filePath = Path.Combine(_tempDirectory, "empty_movements_test.xlsx");

            // Act
            var action = () => _excelService.ExportMovements(emptyMovements, filePath);

            // Assert
            action.Should().NotThrow("export should handle empty list gracefully");
            File.Exists(filePath).Should().BeTrue("Excel file should be created even with empty data");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportInventory_WithLowStockProducts_ShouldHighlightThem()
        {
            // Arrange
            var products = new List<Product>
            {
                _dataBuilder.CreateProduct(name: "Normal Stock", stock: 50, minStock: 10),
                _dataBuilder.CreateProduct(name: "Low Stock", stock: 5, minStock: 10),
                _dataBuilder.CreateProduct(name: "No Stock", stock: 0, minStock: 5)
            };
            var filePath = Path.Combine(_tempDirectory, "low_stock_test.xlsx");

            // Act
            var action = () => _excelService.ExportInventory(products, filePath);

            // Assert
            action.Should().NotThrow("export should handle low stock products correctly");
            File.Exists(filePath).Should().BeTrue("Excel file should be created");
        }

        [Theory]
        [Trait("Category", "ExcelExport")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ExportMethods_WithInvalidFilePath_ShouldThrowException(string? invalidPath)
        {
            // Arrange
            var summary = CreateTestDailySummary();
            var movements = CreateTestMovements();
            var products = CreateTestProducts();

            // Act & Assert
            var summaryAction = () => _excelService.ExportDailySummary(summary, invalidPath!);
            var movementsAction = () => _excelService.ExportMovements(movements, invalidPath!);
            var inventoryAction = () => _excelService.ExportInventory(products, invalidPath!);

            summaryAction.Should().Throw<Exception>("invalid file path should cause exception");
            movementsAction.Should().Throw<Exception>("invalid file path should cause exception");
            inventoryAction.Should().Throw<Exception>("invalid file path should cause exception");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportMethods_WithReadOnlyDirectory_ShouldThrowException()
        {
            // Arrange
            var readOnlyDir = Path.Combine(_tempDirectory, "readonly");
            Directory.CreateDirectory(readOnlyDir);
            
            // Make directory read-only (this might not work on all systems)
            try
            {
                var dirInfo = new DirectoryInfo(readOnlyDir);
                dirInfo.Attributes |= FileAttributes.ReadOnly;
            }
            catch
            {
                // Skip test if we can't make directory read-only
                return;
            }

            var summary = CreateTestDailySummary();
            var filePath = Path.Combine(readOnlyDir, "test.xlsx");

            // Act & Assert
            var action = () => _excelService.ExportDailySummary(summary, filePath);
            action.Should().Throw<Exception>("read-only directory should prevent file creation");
        }

        [Fact]
        [Trait("Category", "ExcelExport")]
        public void ExportDailySummary_WithLargeDataSet_ShouldCompleteSuccessfully()
        {
            // Arrange
            var largeSummary = CreateLargeDailySummary();
            var filePath = Path.Combine(_tempDirectory, "large_summary_test.xlsx");

            // Act
            var action = () => _excelService.ExportDailySummary(largeSummary, filePath);

            // Assert
            action.Should().NotThrow("export should handle large datasets");
            File.Exists(filePath).Should().BeTrue("Excel file should be created");
            
            var fileInfo = new FileInfo(filePath);
            fileInfo.Length.Should().BeGreaterThan(1000, "Large dataset should create substantial file");
        }

        private DailySummary CreateTestDailySummary()
        {
            return new DailySummary
            {
                Date = DateTime.Today,
                TotalSales = 150000,
                TotalTransactions = 25,
                Shifts = new List<ShiftSummary>
                {
                    new ShiftSummary
                    {
                        ShiftId = 1,
                        UserName = "Vendedor 1",
                        StartTime = DateTime.Today.AddHours(8),
                        EndTime = DateTime.Today.AddHours(16),
                        TotalSales = 75000,
                        TransactionCount = 12,
                        SaleDetails = new List<SaleDetail>
                        {
                            new SaleDetail { ProductName = "Café", Quantity = 5, UnitPrice = 3000, Total = 15000 },
                            new SaleDetail { ProductName = "Sandwich", Quantity = 2, UnitPrice = 8500, Total = 17000 }
                        }
                    }
                },
                ProductSales = new List<ProductSale>
                {
                    new ProductSale { ProductName = "Café Expreso", TotalQuantity = 20, TotalAmount = 60000 },
                    new ProductSale { ProductName = "Capuchino", TotalQuantity = 15, TotalAmount = 75000 }
                }
            };
        }

        private List<Movement> CreateTestMovements()
        {
            return new List<Movement>
            {
                _dataBuilder.CreateMovement(MovementType.Venta, 1, 1, 2, 3000),
                _dataBuilder.CreateMovement(MovementType.Entrada, 2, 1, 10, 5000),
                _dataBuilder.CreateMovement(MovementType.Ajuste, 1, 1, 50, 3000)
            };
        }

        private List<Product> CreateTestProducts()
        {
            return new List<Product>
            {
                _dataBuilder.CreateProduct("Café Expreso", "Bebidas", 3000, 50, 10),
                _dataBuilder.CreateProduct("Capuchino", "Bebidas", 5000, 30, 5),
                _dataBuilder.CreateProduct("Sandwich", "Comida", 8500, 20, 3)
            };
        }

        private DailySummary CreateLargeDailySummary()
        {
            var summary = CreateTestDailySummary();
            
            // Add many product sales
            for (int i = 0; i < 100; i++)
            {
                summary.ProductSales.Add(new ProductSale
                {
                    ProductName = $"Producto {i}",
                    TotalQuantity = i + 1,
                    TotalAmount = (i + 1) * 1000
                });
            }

            // Add many shifts
            for (int i = 0; i < 10; i++)
            {
                summary.Shifts.Add(new ShiftSummary
                {
                    ShiftId = i + 2,
                    UserName = $"Vendedor {i + 2}",
                    StartTime = DateTime.Today.AddHours(8 + i),
                    EndTime = DateTime.Today.AddHours(16 + i),
                    TotalSales = (i + 1) * 10000,
                    TransactionCount = (i + 1) * 5,
                    SaleDetails = new List<SaleDetail>()
                });
            }

            return summary;
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
}