using InventorySystem.BLL;
using InventorySystem.Models.Entities;
using InventorySystem.Services;
using System.Linq;

namespace InventorySystem.UI.Forms
{
    public partial class SalesForm : Form
    {
        private readonly AuthService _authService;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
        private readonly ShiftService _shiftService;
        private readonly TempSalesService _tempSalesService;

        // Gestión temporal de ventas
        private List<VentaTemporal> ventasPendientes;
        private VentaTemporal? ventaActual;
        private List<Product> todosLosProductos;

        // Bandera para prevenir recursión en el cambio de selección
        private bool _isUpdatingSelection = false;

        public SalesForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _productService = new ProductService(_authService);
            _salesService = new SalesService(_authService);
            _shiftService = new ShiftService(_authService);
            _tempSalesService = new TempSalesService(_authService);

            ventasPendientes = new List<VentaTemporal>();
            todosLosProductos = new List<Product>();

            LoadProducts();
            LoadTempSalesFromDatabase();
            ConfigurarBusquedaProductos();
        }

        private void LoadProducts()
        {
            todosLosProductos = _productService.GetAllProducts()
                .Where(p => p.StockActual > 0)
                .ToList();
        }

        private void ConfigurarBusquedaProductos()
        {
            txtBuscarProducto.TextChanged += FiltrarProductos;
            lstProductosFiltrados.Click += SeleccionarProducto;
            lstProductosFiltrados.Visible = false;
        }

        private void LoadTempSalesFromDatabase()
        {
            try
            {
                var ventasGuardadas = _tempSalesService.LoadTempSales();
                if (ventasGuardadas.Any())
                {
                    ventasPendientes = ventasGuardadas;
                    CargarVenta(ventasPendientes.First());
                }
                else
                {
                    CrearNuevaVenta();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas guardadas: {ex.Message}", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CrearNuevaVenta();
            }
            ActualizarListaVentas();
        }

        private void SaveAllTempSales()
        {
            try
            {
                // Guardar ventas con items
                var ventasConItems = ventasPendientes.Where(v => v.Items.Any()).ToList();
                _tempSalesService.SaveAllTempSales(ventasConItems);

                // Eliminar ventas vacías de la persistencia
                var ventasVacias = ventasPendientes.Where(v => !v.Items.Any()).ToList();
                foreach (var venta in ventasVacias)
                {
                    _tempSalesService.DeleteTempSale(venta.Id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar ventas: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveAllTempSales();
            base.OnFormClosing(e);
        }

        private void FiltrarProductos(object? sender, EventArgs e)
        {
            var texto = txtBuscarProducto.Text.ToLower();

            if (string.IsNullOrWhiteSpace(texto))
            {
                lstProductosFiltrados.Visible = false;
                LimpiarSeleccionProducto();
                return;
            }

            var productosFiltrados = todosLosProductos
                .Where(p => p.Name.ToLower().Contains(texto))
                .Take(10)
                .Select(p => new {
                    Display = $"{p.Name} - {p.Price:C} (Stock: {GetStockDisponible(p.Id)})",
                    Product = p
                })
                .ToList();

            lstProductosFiltrados.Items.Clear();
            foreach (var item in productosFiltrados)
            {
                lstProductosFiltrados.Items.Add(item);
            }

            lstProductosFiltrados.DisplayMember = "Display";
            lstProductosFiltrados.Visible = productosFiltrados.Any();
        }

        private void SeleccionarProducto(object? sender, EventArgs e)
        {
            if (lstProductosFiltrados.SelectedItem != null)
            {
                dynamic selectedItem = lstProductosFiltrados.SelectedItem;
                Product product = selectedItem.Product;

                txtBuscarProducto.Text = product.Name;
                txtPrice.Text = product.Price.ToString("F2");

                var stockDisponible = GetStockDisponible(product.Id);
                lblStock.Text = $"Stock: {stockDisponible}";
                txtQuantity.Maximum = stockDisponible;
                txtQuantity.Value = stockDisponible > 0 ? 1 : 0;

                txtBuscarProducto.Tag = product;
                lstProductosFiltrados.Visible = false;
            }
        }

        private void LimpiarSeleccionProducto()
        {
            txtPrice.Text = "0.00";
            lblStock.Text = "Stock: 0";
            txtQuantity.Maximum = 0;
            txtQuantity.Value = 0;
            txtBuscarProducto.Tag = null;
        }

        private int GetStockDisponible(int productId)
        {
            var product = todosLosProductos.FirstOrDefault(p => p.Id == productId);
            if (product == null) return 0;

            // Calcular stock reservado en todas las ventas pendientes
            var stockReservado = ventasPendientes
                .SelectMany(v => v.Items)
                .Where(i => i.ProductId == productId)
                .Sum(i => i.Quantity);

            return Math.Max(0, product.StockActual - stockReservado);
        }

        private void CrearNuevaVenta()
        {
            var venta = new VentaTemporal
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = $"Venta {ventasPendientes.Count + 1:000}",
                Items = new List<SaleItem>(),
                FechaCreacion = DateTime.Now
            };

            ventasPendientes.Add(venta);
            CargarVenta(venta);
            ActualizarListaVentas();
        }

        private void CargarVenta(VentaTemporal venta)
        {
            // Protege contra la recursión infinita
            _isUpdatingSelection = true;
            try
            {
                ventaActual = venta;
                txtNombreVenta.Text = venta.Nombre;
                RefreshSaleItems();
                txtMontoRecibido.Text = venta.MontoRecibido > 0 ? venta.MontoRecibido.ToString("F2") : "";
                CalcularCambio();
                // ActualizarListaVentas se llama al final de los métodos de acción, no aquí.
            }
            finally
            {
                _isUpdatingSelection = false;
            }
        }

        private void ActualizarListaVentas()
        {
            lstVentasPendientes.Items.Clear();

            foreach (var venta in ventasPendientes)
            {
                lstVentasPendientes.Items.Add(venta);
            }

            // Marcar la venta actual como seleccionada
            if (ventaActual != null)
            {
                var index = ventasPendientes.IndexOf(ventaActual);
                if (index >= 0)
                {
                    lstVentasPendientes.SelectedIndex = index;
                }
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            var product = txtBuscarProducto.Tag as Product;
            if (product == null)
            {
                MessageBox.Show("Seleccione un producto.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var quantity = (int)txtQuantity.Value;
            if (quantity <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var stockDisponible = GetStockDisponible(product.Id);
            if (quantity > stockDisponible)
            {
                MessageBox.Show("No hay suficiente stock disponible.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ventaActual == null)
            {
                CrearNuevaVenta();
            }

            // Verificar si el producto ya está en la venta actual
            var existingItem = ventaActual!.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + quantity;
                if (newQuantity > stockDisponible)
                {
                    MessageBox.Show("La cantidad total excede el stock disponible.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existingItem.Quantity = newQuantity;
            }
            else
            {
                ventaActual.Items.Add(new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    Product = product
                });
            }

            RefreshSaleItems();
            CalcularCambio();
            txtQuantity.Value = 1;
            txtBuscarProducto.Text = "";
            LimpiarSeleccionProducto();
            txtBuscarProducto.Focus();
        }

        private void RefreshSaleItems()
        {
            dgvSaleItems.DataSource = null;

            if (ventaActual == null || !ventaActual.Items.Any())
            {
                lblTotal.Text = "Total: $0.00";
                return;
            }

            var displayItems = ventaActual.Items.Select(item =>
            {
                return new
                {
                    ProductId = item.ProductId,
                    Producto = item.Product?.Name ?? "",
                    Cantidad = item.Quantity,
                    PrecioUnitario = item.UnitPrice,
                    Total = item.Quantity * item.UnitPrice
                };
            }).ToList();

            dgvSaleItems.DataSource = displayItems;

            // Configurar columnas
            if (dgvSaleItems.Columns.Count > 0)
            {
                dgvSaleItems.Columns["ProductId"].Visible = false;
                dgvSaleItems.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C";
                dgvSaleItems.Columns["Total"].DefaultCellStyle.Format = "C";
            }
        }

        private void CalcularCambio()
        {
            if (ventaActual == null)
            {
                lblTotal.Text = "Total: $0.00";
                lblCambio.Text = "Cambio: $0.00";
                lblCambio.ForeColor = Color.Black;
                btnProcesarVenta.Enabled = false;
                return;
            }

            lblTotal.Text = $"Total: {ventaActual.Total:C}";

            if (decimal.TryParse(txtMontoRecibido.Text, out decimal recibido))
            {
                ventaActual.MontoRecibido = recibido;
                lblCambio.Text = $"Cambio: {ventaActual.Cambio:C}";

                // Validación visual
                if (ventaActual.Cambio < 0)
                {
                    lblCambio.ForeColor = Color.Red;
                    btnProcesarVenta.Enabled = false;
                }
                else
                {
                    lblCambio.ForeColor = Color.Green;
                    btnProcesarVenta.Enabled = ventaActual.Items.Any();
                }
            }
            else
            {
                lblCambio.Text = "Cambio: $0.00";
                lblCambio.ForeColor = Color.Black;
                btnProcesarVenta.Enabled = false;
            }
        }

        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            using var form = new VentaAliasForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var venta = new VentaTemporal
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = form.NombreVenta,
                    Items = new List<SaleItem>(),
                    FechaCreacion = DateTime.Now
                };

                ventasPendientes.Add(venta);
                CargarVenta(venta);
                ActualizarListaVentas();
            }
        }

        private void lstVentasPendientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Previene la recursión infinita
            if (_isUpdatingSelection) return;

            if (lstVentasPendientes.SelectedItem is VentaTemporal venta)
            {
                CargarVenta(venta);
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvSaleItems.SelectedRows.Count > 0 && ventaActual != null)
            {
                var productId = (int)dgvSaleItems.SelectedRows[0].Cells["ProductId"].Value;
                ventaActual.Items.RemoveAll(i => i.ProductId == productId);
                RefreshSaleItems();
                CalcularCambio();
            }
        }

        private void btnEliminarVenta_Click(object sender, EventArgs e)
        {
            if (ventaActual == null || ventasPendientes.Count <= 1)
            {
                MessageBox.Show("Debe mantener al menos una venta activa.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show($"¿Está seguro que desea eliminar '{ventaActual.Nombre}'?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _tempSalesService.DeleteTempSale(ventaActual.Id); // Eliminar de persistencia
                ventasPendientes.Remove(ventaActual);

                if (ventasPendientes.Any())
                {
                    CargarVenta(ventasPendientes.First());
                }
                else
                {
                    CrearNuevaVenta();
                }
                ActualizarListaVentas();
            }
        }

        private void txtNombreVenta_TextChanged(object sender, EventArgs e)
        {
            if (ventaActual != null)
            {
                ventaActual.Nombre = txtNombreVenta.Text;
                ActualizarListaVentas();
            }
        }

        private void txtMontoRecibido_TextChanged(object sender, EventArgs e)
        {
            CalcularCambio();
        }

        private void btnProcesarVenta_Click(object sender, EventArgs e)
        {
            if (ventaActual == null || !ventaActual.Items.Any())
            {
                MessageBox.Show("Agregue productos a la venta.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ventaActual.EsValida)
            {
                MessageBox.Show("El monto pagado debe ser mayor o igual al total de la venta.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMontoRecibido.Focus();
                return;
            }

            // Verificar que hay turno activo
            var activeShift = _shiftService.GetActiveShift();
            if (activeShift == null)
            {
                var result = MessageBox.Show("No hay un turno activo. ¿Desea iniciar uno ahora?",
                    "Sin Turno Activo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_shiftService.StartShift())
                    {
                        MessageBox.Show("Turno iniciado correctamente.", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo iniciar el turno.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            var confirmResult = MessageBox.Show($"¿Confirmar venta '{ventaActual.Nombre}' por {ventaActual.Total:C}?",
                "Confirmar Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                if (_salesService.ProcessSale(ventaActual.Items))
                {
                    // Mostrar opción de imprimir factura
                    var printResult = MessageBox.Show($"Venta procesada correctamente.\n\nTotal: {ventaActual.Total:C}\nPagado: {ventaActual.MontoRecibido:C}\nDevuelta: {ventaActual.Cambio:C}\n\n¿Desea imprimir la factura?",
                        "Venta Exitosa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (printResult == DialogResult.Yes)
                    {
                        using var invoiceService = new InvoiceService(ventaActual.Items, ventaActual.Total, ventaActual.MontoRecibido, ventaActual.Cambio, _authService.CurrentUser?.Name ?? "");
                        invoiceService.ShowPrintPreview();
                    }

                    MessageBox.Show("Venta procesada correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Eliminar la venta de la persistencia
                    _tempSalesService.DeleteTempSale(ventaActual.Id);

                    // Eliminar la venta procesada y crear una nueva
                    ventasPendientes.Remove(ventaActual);

                    if (ventasPendientes.Any())
                    {
                        CargarVenta(ventasPendientes.First());
                    }
                    else
                    {
                        CrearNuevaVenta();
                    }

                    LoadProducts(); // Actualizar stock disponible
                }
                else
                {
                    MessageBox.Show("Error al procesar la venta. Verifique el stock disponible.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ActualizarListaVentas();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var hasItems = ventaActual?.Items.Any() == true;

            if (hasItems)
            {
                var result = MessageBox.Show($"¿Está seguro que desea limpiar la venta '{ventaActual?.Nombre}'?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (ventaActual != null)
                    {
                        // Eliminar de persistencia
                        _tempSalesService.DeleteTempSale(ventaActual.Id);

                        ventaActual.Items.Clear();
                        ventaActual.MontoRecibido = 0;
                        RefreshSaleItems();
                        CalcularCambio();
                        txtMontoRecibido.Text = "";
                    }
                    ActualizarListaVentas();
                }
            }
        }

        private void btnGuardarVenta_Click(object sender, EventArgs e)
        {
            if (ventaActual != null && ventaActual.Items.Any())
            {
                try
                {
                    _tempSalesService.SaveTempSale(ventaActual);
                    ActualizarListaVentas();
                    MessageBox.Show("Venta guardada correctamente.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar venta: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}