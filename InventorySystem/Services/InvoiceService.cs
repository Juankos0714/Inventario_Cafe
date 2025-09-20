using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using InventorySystem.Models.Entities;
using InventorySystem.BLL;

namespace InventorySystem.Services
{
    public class InvoiceService : IDisposable
    {
        private readonly List<SaleItem> _saleItems;
        private readonly decimal _total;
        private readonly decimal _paid;
        private readonly decimal _change;
        private readonly string _vendorName;
        private readonly DateTime _saleDate;

        private readonly PrintDocument _printDocument;
        private readonly Font _headerFont;
        private readonly Font _normalFont;
        private readonly Font _boldFont;
        private readonly Brush _blackBrush;

        public InvoiceService(List<SaleItem> saleItems, decimal total, decimal paid, decimal change, string vendorName)
        {
            _saleItems = saleItems;
            _total = total;
            _paid = paid;
            _change = change;
            _vendorName = vendorName;
            _saleDate = DateTime.Now;

            // Inicializar recursos que deben ser liberados
            _printDocument = new PrintDocument();
            _headerFont = new Font("Arial", 10, FontStyle.Bold);
            _normalFont = new Font("Arial", 8);
            _boldFont = new Font("Arial", 8, FontStyle.Bold);
            _blackBrush = new SolidBrush(Color.Black);

            // Suscribir el evento PrintPage
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        /// <summary>
        /// Muestra un diálogo de vista previa de impresión para la factura.
        /// </summary>
        public void ShowPrintPreview()
        {
            try
            {
                // Configurar para impresora térmica (papel de 80mm)
                var paperSize = new PaperSize("Thermal", 315, 600); // 80mm width
                _printDocument.DefaultPageSettings.PaperSize = paperSize;
                _printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);

                var printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = _printDocument;
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en vista previa: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Muestra un diálogo de impresión y envía la factura a la impresora seleccionada.
        /// </summary>
        public void PrintInvoice()
        {
            try
            {
                // Configurar para impresora térmica (papel de 80mm)
                var paperSize = new PaperSize("Thermal", 315, 600);
                _printDocument.DefaultPageSettings.PaperSize = paperSize;
                _printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);

                var printDialog = new PrintDialog
                {
                    Document = _printDocument
                };

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    _printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir: {ex.Message}", "Error de Impresión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Método de evento para dibujar el contenido de la página de impresión.
        /// </summary>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var graphics = e.Graphics;
            float yPosition = 10;
            float leftMargin = 10;
            float rightMargin = e.PageBounds.Width - 10;

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
            graphics.DrawLine(Pens.Black, leftMargin, yPosition, rightMargin, yPosition);
            yPosition += 15;

            // Fecha y hora
            yPosition = PrintLeftText(graphics, $"Fecha: {_saleDate:dd/MM/yyyy HH:mm}", _normalFont, yPosition, leftMargin);
            yPosition += 10;

            // Encabezado de productos
            graphics.DrawString("Producto", _boldFont, _blackBrush, leftMargin, yPosition);
            graphics.DrawString("Cant.", _boldFont, _blackBrush, leftMargin + 140, yPosition);
            graphics.DrawString("Precio", _boldFont, _blackBrush, leftMargin + 190, yPosition);
            graphics.DrawString("Total", _boldFont, _blackBrush, leftMargin + 250, yPosition);
            yPosition += 15;

            // Línea separadora
            graphics.DrawLine(Pens.Black, leftMargin, yPosition, rightMargin, yPosition);
            yPosition += 15;

            // Productos
            foreach (var item in _saleItems)
            {
                var productName = item.Product?.Name ?? "Producto Desconocido";
                if (productName.Length > 18)
                    productName = productName.Substring(0, 15) + "...";

                graphics.DrawString(productName, _normalFont, _blackBrush, leftMargin, yPosition);
                graphics.DrawString($"{item.Quantity}", _normalFont, _blackBrush, leftMargin + 140, yPosition);
                graphics.DrawString($"{item.UnitPrice:C0}", _normalFont, _blackBrush, leftMargin + 190, yPosition);
                graphics.DrawString($"{(item.Quantity * item.UnitPrice):C0}", _normalFont, _blackBrush, leftMargin + 250, yPosition);
                yPosition += 15;
            }

            // Línea separadora
            graphics.DrawLine(Pens.Black, leftMargin, yPosition, rightMargin, yPosition);
            yPosition += 15;

            // Totales
            yPosition = PrintRightAlignedTotal(graphics, "Total a pagar:", _total.ToString("C0"), _boldFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintRightAlignedTotal(graphics, "Pagado:", _paid.ToString("C0"), _normalFont, yPosition, leftMargin, rightMargin);
            yPosition = PrintRightAlignedTotal(graphics, "Devuelta:", _change.ToString("C0"), _normalFont, yPosition, leftMargin, rightMargin);

            // Línea separadora
            yPosition += 5;
            graphics.DrawLine(Pens.Black, leftMargin, yPosition, rightMargin, yPosition);
            yPosition += 15;

            // Vendedor
            yPosition = PrintLeftText(graphics, $"Vendedor: {_vendorName}", _normalFont, yPosition, leftMargin);
            yPosition += 10;

            // Mensaje final
            yPosition = PrintCenteredText(graphics, "¡Gracias por su compra!", _boldFont, yPosition, leftMargin, rightMargin);
            yPosition += 10;
            yPosition = PrintCenteredText(graphics, "¡Vuelva pronto!", _boldFont, yPosition, leftMargin, rightMargin);
        }

        #region Métodos de Utilidad

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

        private float PrintRightAlignedTotal(Graphics graphics, string label, string value, Font font, float yPosition, float leftMargin, float rightMargin)
        {
            var labelSize = graphics.MeasureString(label, font);
            var valueSize = graphics.MeasureString(value, font);

            var labelX = leftMargin + 180;
            var valueX = rightMargin - valueSize.Width;

            graphics.DrawString(label, font, _blackBrush, labelX, yPosition);
            graphics.DrawString(value, font, _blackBrush, valueX, yPosition);

            return yPosition + Math.Max(labelSize.Height, valueSize.Height) + 2;
        }

        #endregion

        #region Implementación de IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Liberar los recursos de impresión
                _printDocument?.Dispose();
                _headerFont?.Dispose();
                _normalFont?.Dispose();
                _boldFont?.Dispose();
                _blackBrush?.Dispose();
            }
        }

        #endregion
    }
}