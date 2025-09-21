using ClosedXML.Excel;
using InventorySystem.Models.DTOs;
using InventorySystem.Models.Entities;

namespace InventorySystem.Services
{
    public class ExcelExportService
    {
        public void ExportDailySummary(DailySummary summary, string filePath)
        {
            using var workbook = new XLWorkbook();
            
            // Hoja de resumen general
            var summarySheet = workbook.Worksheets.Add("Resumen Diario");
            CreateDailySummarySheet(summarySheet, summary);
            
            // Hoja de turnos
            var shiftsSheet = workbook.Worksheets.Add("Turnos");
            CreateShiftsSheet(shiftsSheet, summary.Shifts);
            
            // Hoja de productos vendidos
            var productsSheet = workbook.Worksheets.Add("Productos");
            CreateProductSalesSheet(productsSheet, summary.ProductSales);
            
            workbook.SaveAs(filePath);
        }
        
        public void ExportMovements(List<Movement> movements, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Movimientos");
            
            // Headers
            worksheet.Cell(1, 1).Value = "Fecha";
            worksheet.Cell(1, 2).Value = "Tipo";
            worksheet.Cell(1, 3).Value = "Producto";
            worksheet.Cell(1, 4).Value = "Cantidad";
            worksheet.Cell(1, 5).Value = "Precio Unitario";
            worksheet.Cell(1, 6).Value = "Total";
            worksheet.Cell(1, 7).Value = "Usuario";
            worksheet.Cell(1, 8).Value = "Notas";
            
            // Header style
            var headerRange = worksheet.Range(1, 1, 1, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            
            // Data
            int row = 2;
            foreach (var movement in movements)
            {
                worksheet.Cell(row, 1).Value = movement.Date;
                worksheet.Cell(row, 2).Value = movement.Type.ToString();
                worksheet.Cell(row, 3).Value = movement.Product?.Name ?? "";
                worksheet.Cell(row, 4).Value = movement.Quantity;
                worksheet.Cell(row, 5).Value = movement.UnitPrice;
                worksheet.Cell(row, 6).Value = movement.TotalAmount;
                worksheet.Cell(row, 7).Value = movement.User?.Name ?? "";
                worksheet.Cell(row, 8).Value = movement.Notes ?? "";
                row++;
            }
            
            worksheet.ColumnsUsed().AdjustToContents();
            workbook.SaveAs(filePath);
        }
        
        public void ExportInventory(List<Product> products, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Inventario");
            
            // Headers
            worksheet.Cell(1, 1).Value = "Nombre";
            worksheet.Cell(1, 2).Value = "Categoría";
            worksheet.Cell(1, 3).Value = "Precio";
            worksheet.Cell(1, 4).Value = "Stock Actual";
            worksheet.Cell(1, 5).Value = "Stock Mínimo";
            worksheet.Cell(1, 6).Value = "Estado";
            
            // Header style
            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
            
            // Data
            int row = 2;
            foreach (var product in products)
            {
                worksheet.Cell(row, 1).Value = product.Name;
                worksheet.Cell(row, 2).Value = product.Category;
                worksheet.Cell(row, 3).Value = product.Price;
                worksheet.Cell(row, 4).Value = product.StockActual;
                worksheet.Cell(row, 5).Value = product.StockMinimo;
                worksheet.Cell(row, 6).Value = product.StockActual <= product.StockMinimo ? "STOCK BAJO" : "OK";
                
                // Highlight low stock
                if (product.StockActual <= product.StockMinimo)
                {
                    worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.LightPink;
                }
                
                row++;
            }
            
            worksheet.ColumnsUsed().AdjustToContents();
            workbook.SaveAs(filePath);
        }
        
        private void CreateDailySummarySheet(IXLWorksheet worksheet, DailySummary summary)
        {
            worksheet.Cell(1, 1).Value = "RESUMEN DIARIO";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            
            worksheet.Cell(3, 1).Value = "Fecha:";
            worksheet.Cell(3, 2).Value = summary.Date.ToString("dd/MM/yyyy");
            
            worksheet.Cell(4, 1).Value = "Total Ventas:";
            worksheet.Cell(4, 2).Value = summary.TotalSales;
            worksheet.Cell(4, 2).Style.NumberFormat.Format = "$#,##0.00";
            
            worksheet.Cell(5, 1).Value = "Total Transacciones:";
            worksheet.Cell(5, 2).Value = summary.TotalTransactions;
            
            worksheet.Cell(7, 1).Value = "Turnos Cerrados:";
            worksheet.Cell(7, 2).Value = summary.Shifts.Count;
            
            worksheet.ColumnsUsed().AdjustToContents();
        }
        
        private void CreateShiftsSheet(IXLWorksheet worksheet, List<ShiftSummary> shifts)
        {
            // Headers
            worksheet.Cell(1, 1).Value = "Usuario";
            worksheet.Cell(1, 2).Value = "Inicio";
            worksheet.Cell(1, 3).Value = "Fin";
            worksheet.Cell(1, 4).Value = "Ventas";
            worksheet.Cell(1, 5).Value = "Transacciones";
            
            var headerRange = worksheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
            
            int row = 2;
            foreach (var shift in shifts)
            {
                worksheet.Cell(row, 1).Value = shift.UserName;
                worksheet.Cell(row, 2).Value = shift.StartTime;
                worksheet.Cell(row, 3).Value = shift.EndTime;
                worksheet.Cell(row, 4).Value = shift.TotalSales;
                worksheet.Cell(row, 5).Value = shift.TransactionCount;
                row++;
            }
            
            worksheet.ColumnsUsed().AdjustToContents();
        }
        
        private void CreateProductSalesSheet(IXLWorksheet worksheet, List<ProductSale> productSales)
        {
            // Headers
            worksheet.Cell(1, 1).Value = "Producto";
            worksheet.Cell(1, 2).Value = "Cantidad Vendida";
            worksheet.Cell(1, 3).Value = "Monto Total";
            
            var headerRange = worksheet.Range(1, 1, 1, 3);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            
            int row = 2;
            foreach (var sale in productSales)
            {
                worksheet.Cell(row, 1).Value = sale.ProductName;
                worksheet.Cell(row, 2).Value = sale.TotalQuantity;
                worksheet.Cell(row, 3).Value = sale.TotalAmount;
                row++;
            }
            
            worksheet.ColumnsUsed().AdjustToContents();
        }
    }
}