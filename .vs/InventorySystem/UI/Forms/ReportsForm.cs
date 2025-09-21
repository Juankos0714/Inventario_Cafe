using InventorySystem.BLL;
using InventorySystem.Services;
using InventorySystem.Models.Enums;

namespace InventorySystem.UI.Forms
{
    public partial class ReportsForm : Form
    {
        private readonly AuthService _authService;
        private readonly ReportService _reportService;
        private readonly ProductService _productService;
        private readonly ExcelExportService _excelService;
        private readonly Services.CloseInvoiceService? _closeInvoiceService;
        
        public ReportsForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _reportService = new ReportService(_authService);
            _productService = new ProductService(_authService);
            _excelService = new ExcelExportService();
            
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today;
        }
        
        private void btnDailySummary_Click(object sender, EventArgs e)
        {
            var date = dtpReportDate.Value.Date;
            var summary = _reportService.GenerateDailySummary(date);
            
            if (summary == null)
            {
                MessageBox.Show("No se pudo generar el resumen diario.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Mostrar resumen en el RichTextBox
            var report = $"RESUMEN DIARIO - {date:dd/MM/yyyy}\n\n";
            report += $"Total Ventas: {summary.TotalSales:C}\n";
            report += $"Total Transacciones: {summary.TotalTransactions}\n";
            report += $"Turnos Cerrados: {summary.Shifts.Count}\n\n";
            
            if (summary.Shifts.Any())
            {
                report += "TURNOS:\n";
                foreach (var shift in summary.Shifts)
                {
                    report += $"- {shift.UserName}: {shift.TotalSales:C} ({shift.TransactionCount} transacciones)\n";
                }
                report += "\n";
            }
            
            if (summary.ProductSales.Any())
            {
                report += "PRODUCTOS MÁS VENDIDOS:\n";
                foreach (var product in summary.ProductSales.Take(10))
                {
                    report += $"- {product.ProductName}: {product.TotalQuantity} unidades - {product.TotalAmount:C}\n";
                }
            }
            
            txtReport.Text = report;
        }
        
        private void btnMovements_Click(object sender, EventArgs e)
        {
            var startDate = dtpStartDate.Value.Date;
            var endDate = dtpEndDate.Value.Date;
            
            MovementType? type = null;
            if (cmbMovementType.SelectedIndex > 0)
            {
                type = (MovementType)(cmbMovementType.SelectedIndex - 1);
            }
            
            var movements = _reportService.GetMovementReport(startDate, endDate, type);
            
            var report = $"REPORTE DE MOVIMIENTOS - {startDate:dd/MM/yyyy} al {endDate:dd/MM/yyyy}\n";
            if (type.HasValue)
                report += $"Tipo: {type}\n";
            report += "\n";
            
            report += $"Total de Movimientos: {movements.Count}\n";
            if (type == MovementType.Venta || !type.HasValue)
            {
                var totalSales = movements.Where(m => m.Type == MovementType.Venta).Sum(m => m.TotalAmount);
                report += $"Total en Ventas: {totalSales:C}\n";
            }
            report += "\n";
            
            foreach (var movement in movements.Take(50)) // Mostrar solo los primeros 50
            {
                report += $"{movement.Date:dd/MM/yyyy HH:mm} - {movement.Type} - {movement.Product?.Name} - ";
                report += $"Cant: {movement.Quantity} - Total: {movement.TotalAmount:C} - Usuario: {movement.User?.Name}\n";
            }
            
            if (movements.Count > 50)
            {
                report += $"\n... y {movements.Count - 50} movimientos más. Use la exportación para ver todos.";
            }
            
            txtReport.Text = report;
        }
        
        private void btnInventoryReport_Click(object sender, EventArgs e)
        {
            var products = _productService.GetAllProducts();
            var lowStockProducts = _productService.GetLowStockProducts();
            
            var report = $"REPORTE DE INVENTARIO - {DateTime.Now:dd/MM/yyyy}\n\n";
            report += $"Total de Productos: {products.Count}\n";
            report += $"Productos con Stock Bajo: {lowStockProducts.Count}\n";
            report += $"Valor Total del Inventario: {products.Sum(p => p.Price * p.StockActual):C}\n\n";
            
            if (lowStockProducts.Any())
            {
                report += "PRODUCTOS CON STOCK BAJO:\n";
                foreach (var product in lowStockProducts)
                {
                    report += $"- {product.Name}: {product.StockActual} (mín: {product.StockMinimo}) - Categoría: {product.Category}\n";
                }
                report += "\n";
            }
            
            report += "TODOS LOS PRODUCTOS:\n";
            foreach (var product in products.OrderBy(p => p.Category).ThenBy(p => p.Name))
            {
                var status = product.StockActual <= product.StockMinimo ? " ⚠️ BAJO" : "";
                report += $"- {product.Name} ({product.Category}): Stock {product.StockActual} - Precio {product.Price:C}{status}\n";
            }
            
            txtReport.Text = report;
        }
        
        private void btnExportDaily_Click(object sender, EventArgs e)
        {
            var date = dtpReportDate.Value.Date;
            var summary = _reportService.GenerateDailySummary(date);
            
            if (summary == null)
            {
                MessageBox.Show("No se pudo generar el resumen diario.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Excel Files|*.xlsx";
            dialog.FileName = $"Resumen_Diario_{date:yyyyMMdd}.xlsx";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _excelService.ExportDailySummary(summary, dialog.FileName);
                    MessageBox.Show("Reporte exportado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnExportMovements_Click(object sender, EventArgs e)
        {
            var startDate = dtpStartDate.Value.Date;
            var endDate = dtpEndDate.Value.Date;
            
            MovementType? type = null;
            if (cmbMovementType.SelectedIndex > 0)
            {
                type = (MovementType)(cmbMovementType.SelectedIndex - 1);
            }
            
            var movements = _reportService.GetMovementReport(startDate, endDate, type);
            
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Excel Files|*.xlsx";
            dialog.FileName = $"Movimientos_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _excelService.ExportMovements(movements, dialog.FileName);
                    MessageBox.Show("Reporte exportado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnExportInventory_Click(object sender, EventArgs e)
        {
            var products = _productService.GetAllProducts();
            
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Excel Files|*.xlsx";
            dialog.FileName = $"Inventario_{DateTime.Now:yyyyMMdd}.xlsx";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _excelService.ExportInventory(products, dialog.FileName);
                    MessageBox.Show("Reporte exportado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnCreateDailyClose_Click(object sender, EventArgs e)
        {
            var date = dtpReportDate.Value.Date;
            
            var result = MessageBox.Show($"¿Crear cierre diario para {date:dd/MM/yyyy}?", 
                "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                if (_reportService.CreateDailyClose(date))
                {
                    MessageBox.Show("Cierre diario creado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al crear el cierre diario. Puede que ya exista uno para esta fecha.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnCreatePartialClose_Click(object sender, EventArgs e)
        {
            var date = dtpReportDate.Value.Date;
            
            if (!_reportService.CanCreatePartialClose(date))
            {
                var closesCount = _reportService.GetClosesForDate(date).Count;
                MessageBox.Show($"Ya se han realizado {closesCount} cierres para hoy. Máximo permitido: 2", 
                    "Límite Alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var closesExisting = _reportService.GetClosesForDate(date).Count;
            var closeType = closesExisting == 0 ? "PARCIAL" : "FINAL";
            
            var result = MessageBox.Show($"¿Crear cierre {closeType.ToLower()} para {date:dd/MM/yyyy}?", 
                "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                if (_reportService.CreatePartialClose(date, closeType))
                {
                    var closeData = _reportService.GetLastCloseForDate(date);
                    
                    MessageBox.Show($"Cierre {closeType.ToLower()} creado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Mostrar opción de imprimir
                    var printResult = MessageBox.Show("¿Desea imprimir el resumen del cierre?", 
                        "Imprimir Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (printResult == DialogResult.Yes && closeData != null)
                    {
                        using var closeInvoice = new Services.CloseInvoiceService(closeData);
                        closeInvoice.ShowPrintPreview();
                    }
                }
                else
                {
                    MessageBox.Show("Error al crear el cierre. Verifique que tenga permisos suficientes.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}