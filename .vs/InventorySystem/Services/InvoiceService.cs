using InventorySystem.BLL;
using InventorySystem.Models.Entities;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace InventorySystem.Services
{
    // 1. Implementar la interfaz IDisposable
    public class InvoiceService : IDisposable
    {
        private readonly List<SaleItem> _saleItems;
        private readonly decimal _total;
        private readonly decimal _paid;
        private readonly decimal _change;
        private readonly string _vendorName;
        private readonly DateTime _saleDate;
        private Font _headerFont;
        private Font _normalFont;
        private Font _boldFont;
        private Brush _blackBrush;
        private bool disposed = false; // To detect redundant calls

        public InvoiceService(List<SaleItem> saleItems, decimal total, decimal paid, decimal change, string vendorName)
        {
            _saleItems = saleItems;
            _total = total;
            _paid = paid;
            _change = change;
            _vendorName = vendorName;
            _saleDate = DateTime.Now;

            // Initialize disposable objects
            _headerFont = new Font("Arial", 10, FontStyle.Bold);
            _normalFont = new Font("Arial", 8);
            _boldFont = new Font("Arial", 8, FontStyle.Bold);
            _blackBrush = new SolidBrush(Color.Black);
        }

        public void PrintInvoice()
        {
            try
            {
                var printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;

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
            if (graphics == null) return;
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

            // Fecha y hora
            yPosition = PrintLeftText(graphics, $"Fecha: {_saleDate:dd/MM/yyyy HH:mm}", _normalFont, yPosition, leftMargin);
            yPosition += 10;

            // Encabezado de productos
            graphics.DrawString("Producto", _boldFont, _blackBrush, leftMargin, yPosition);
            graphics.DrawString("Precio", _boldFont, _blackBrush, leftMargin + 140, yPosition);
            graphics.DrawString("Cant", _boldFont, _blackBrush, leftMargin + 200, yPosition);
            yPosition += 15;

            // Línea separadora
            graphics.DrawString(new string('-', 38), _normalFont, _blackBrush, leftMargin, yPosition);
            yPosition += 15;

            // Productos
            foreach (var item in _saleItems)
            {
                var productName = item.Product?.Name ?? "Producto";
                if (productName.Length > 18)
                    productName = productName.Substring(0, 15) + "...";

                graphics.DrawString(productName, _normalFont, _blackBrush, leftMargin, yPosition);
                graphics.DrawString($"{item.UnitPrice:C0}", _normalFont, _blackBrush, leftMargin + 140, yPosition);
                graphics.DrawString($"{item.Quantity}", _normalFont, _blackBrush, leftMargin + 200, yPosition);
                yPosition += 15;
            }

            // Línea separadora
            yPosition += 5;
            graphics.DrawString(new string('-', 38), _normalFont, _blackBrush, leftMargin, yPosition);
            yPosition += 15;

            // Totales
            yPosition = PrintLeftText(graphics, $"Total a pagar: {_total:C0}", _boldFont, yPosition, leftMargin);
            yPosition = PrintLeftText(graphics, $"Pagado: {_paid:C0}", _normalFont, yPosition, leftMargin);
            yPosition = PrintLeftText(graphics, $"Devuelta: {_change:C0}", _normalFont, yPosition, leftMargin);

            // Línea separadora
            yPosition += 5;
            graphics.DrawString(new string('-', 38), _normalFont, _blackBrush, leftMargin, yPosition);
            yPosition += 15;

            // Vendedor
            yPosition = PrintLeftText(graphics, $"Vendedor: {_vendorName}", _normalFont, yPosition, leftMargin);
            yPosition += 10;

            // Mensaje final
            yPosition = PrintCenteredText(graphics, "Gracias por su compra", _boldFont, yPosition, leftMargin, rightMargin);
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

        // 2. Implementar el método Dispose()
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // 3. Implementar el método Dispose(bool disposing) para la lógica de limpieza
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Clean up managed resources
                    _headerFont?.Dispose();
                    _normalFont?.Dispose();
                    _boldFont?.Dispose();
                    _blackBrush?.Dispose();
                }

                // Clean up unmanaged resources (if any)
                disposed = true;
            }
        }

        // 4. Implementar el finalizador (destructor)
        ~InvoiceService()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
    }
}