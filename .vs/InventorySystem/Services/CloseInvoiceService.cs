using System.Drawing;
using System.Drawing.Printing;
using InventorySystem.Models.DTOs;

namespace InventorySystem.Services
{
    public class CloseInvoiceService : IDisposable
    {
        private readonly CloseData _closeData;
        private Font _headerFont;
        private Font _normalFont;
        private Font _boldFont;
        private Brush _blackBrush;
        
        public CloseInvoiceService(CloseData closeData)
        {
            _closeData = closeData;
            
            _headerFont = new Font("Arial", 10, FontStyle.Bold);
            _normalFont = new Font("Arial", 8);
            _boldFont = new Font("Arial", 8, FontStyle.Bold);
            _blackBrush = new SolidBrush(Color.Black);
        }
        
        public void PrintClose()
        {
            try
            {
                var printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;
                
                // Configurar para impresora térmica (papel de 80mm)
                var paperSize = new PaperSize("Thermal", 315, 600);
                printDocument.DefaultPageSettings.PaperSize = paperSize;
                printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
                
                var printDialog = new PrintDialog();
                printDialog.Document = printDocument;
                
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir: {ex.Message}", "Error de Impresión", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void ShowPrintPreview()
        {
            try
            {
                var printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;
                
                var paperSize = new PaperSize("Thermal", 315, 600);
                printDocument.DefaultPageSettings.PaperSize = paperSize;
                printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
                
                var printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument;
                printPreviewDialog.Width = 400;
                printPreviewDialog.Height = 600;
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en vista previa: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var graphics = e.Graphics;
            float yPosition = 10;
            float leftMargin = 10;
            float rightMargin = e.MarginBounds.Right;
            
            // Encabezado de la empresa
            yPosition = PrintCenteredText(graphics, "Palo de Café", _headerFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, "Café de origen", _normalFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, "NIT 41904612-7", _normalFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, "Régimen simplificado", _normalFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, "CRA 13 # 1N35", _normalFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, "Clínica Central del Quindío", _normalFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, "Cel 3244213193", _normalFont, yPosition, leftMargin, rightMargin);
            
            // Línea separadora
            yPosition += 5;
            graphics.DrawString(new string('-', 38), _normalFont, _blackBrush, leftMargin, yPosition);
            yPosition += 20;
            
            // Información del cierre
            yPosition = PrintCenteredText(graphics, $"CIERRE {_closeData.CloseType}", _headerFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintLeftText(graphics, $"Fecha: {_closeData.Date:dd/MM/yyyy}", _normalFont, yPosition, leftMargin);
            yPosition = PrintLeftText(graphics, $"Hora: {_closeData.CreatedAt:HH:mm}", _normalFont, yPosition, leftMargin);
            yPosition = PrintLeftText(graphics, $"Cierre #{_closeData.CloseSequence}", _normalFont, yPosition, leftMargin);
            yPosition = PrintLeftText(graphics, $"Usuario: {_closeData.UserName}", _normalFont, yPosition, leftMargin);
            
            // Línea separadora
            yPosition += 5;
            graphics.DrawString(new string('-', 38), _normalFont, _blackBrush, leftMargin, yPosition);
            yPosition += 15;
            
            // Totales principales
            yPosition = PrintRightAlignedTotal(graphics, "Total Ventas:", _closeData.TotalSales.ToString("C0"), _boldFont, yPosition, leftMargin);
            yPosition = PrintRightAlignedTotal(graphics, "Transacciones:", _closeData.TransactionCount.ToString(), _normalFont, yPosition, leftMargin);
            
            // Turnos si hay
            if (_closeData.Shifts.Any())
            {
                yPosition += 10;
                yPosition = PrintLeftText(graphics, "TURNOS:", _boldFont, yPosition, leftMargin);
                
                foreach (var shift in _closeData.Shifts.Take(5)) // Máximo 5 turnos
                {
                    var shiftLine = $"{shift.UserName}: {shift.TotalSales:C0}";
                    yPosition = PrintLeftText(graphics, shiftLine, _normalFont, yPosition, leftMargin);
                }
            }
            
            // Productos más vendidos
            if (_closeData.TopProducts.Any())
            {
                yPosition += 10;
                yPosition = PrintLeftText(graphics, "TOP PRODUCTOS:", _boldFont, yPosition, leftMargin);
                
                foreach (var product in _closeData.TopProducts.Take(5)) // Top 5
                {
                    var productLine = $"{product.ProductName}: {product.TotalQuantity}u";
                    yPosition = PrintLeftText(graphics, productLine, _normalFont, yPosition, leftMargin);
                }
            }
            
            // Línea separadora final
            yPosition += 10;
            graphics.DrawString(new string('-', 38), _normalFont, _blackBrush, leftMargin, yPosition);
            yPosition += 15;
            
            // Footer
            yPosition = PrintCenteredText(graphics, "Cierre Registrado", _boldFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintCenteredText(graphics, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), _normalFont, yPosition, leftMargin, rightMargin);
        }
        
        private float PrintCenteredText(Graphics graphics, string text, Font font, float yPosition, float leftMargin, float rightMargin)
        {
            var textSize = graphics.MeasureString(text, font);
            var xPosition = leftMargin + (rightMargin - leftMargin - textSize.Width) / 2;
            graphics.DrawString(text, font, _blackBrush, xPosition, yPosition);
            return yPosition + textSize.Height + 2;
        }
        
        private float PrintLeftText(Graphics graphics, string text, Font font, float yPosition, float leftMargin)
        {
            graphics.DrawString(text, font, _blackBrush, leftMargin, yPosition);
            var textSize = graphics.MeasureString(text, font);
            return yPosition + textSize.Height + 2;
        }
        
        private float PrintRightAlignedTotal(Graphics graphics, string label, string value, Font font, float yPosition, float leftMargin)
        {
            graphics.DrawString(label, font, _blackBrush, leftMargin, yPosition);
            
            var valueSize = graphics.MeasureString(value, font);
            graphics.DrawString(value, font, _blackBrush, 280 - valueSize.Width, yPosition);
            
            var textSize = graphics.MeasureString(label, font);
            return yPosition + textSize.Height + 2;
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _headerFont?.Dispose();
                _normalFont?.Dispose();
                _boldFont?.Dispose();
                _blackBrush?.Dispose();
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}