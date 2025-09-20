using InventorySystem.BLL;
using InventorySystem.Models.Entities;

namespace InventorySystem.UI.Forms
{
    public partial class ProductsForm : Form
    {
        private readonly AuthService _authService;
        private readonly ProductService _productService;
        private readonly List<Product> _products;
        
        public ProductsForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _productService = new ProductService(_authService);
            _products = new List<Product>();
            
            ConfigureByRole();
            LoadProducts();
        }
        
        private void ConfigureByRole()
        {
            // Solo administradores pueden crear/editar productos
            if (_authService.CurrentUser?.Role != Models.Enums.UserRole.Admin)
            {
                btnNew.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }
        
        private void LoadProducts()
        {
            _products.Clear();
            _products.AddRange(_productService.GetAllProducts());
            RefreshGrid();
        }
        
        private void RefreshGrid()
        {
            var displayProducts = _products.Select(p => new
            {
                p.Id,
                Nombre = p.Name,
                Categoría = p.Category,
                Precio = p.Price,
                Stock = p.StockActual,
                StockMínimo = p.StockMinimo,
                Estado = p.IsActive ? "Activo" : "Inactivo"
            }).ToList();
            
            dgvProducts.DataSource = displayProducts;
            
            if (dgvProducts.Columns.Count > 0)
            {
                dgvProducts.Columns["Id"].Visible = false;
                dgvProducts.Columns["Precio"].DefaultCellStyle.Format = "C";
                
                // Resaltar productos con stock bajo
                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    var productId = (int)row.Cells["Id"].Value;
                    var product = _products.FirstOrDefault(p => p.Id == productId);
                    
                    if (product != null && product.StockActual <= product.StockMinimo)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightPink;
                    }
                }
            }
        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            using var form = new ProductEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (_productService.CreateProduct(form.Product))
                {
                    MessageBox.Show("Producto creado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Error al crear el producto.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para editar.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var productId = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
            var product = _products.FirstOrDefault(p => p.Id == productId);
            
            if (product != null)
            {
                using var form = new ProductEditForm(product);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (_productService.UpdateProduct(form.Product))
                    {
                        MessageBox.Show("Producto actualizado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el producto.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para desactivar.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var productId = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
            var product = _products.FirstOrDefault(p => p.Id == productId);
            
            if (product != null)
            {
                var result = MessageBox.Show($"¿Está seguro que desea desactivar '{product.Name}'?", 
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    product.IsActive = false;
                    if (_productService.UpdateProduct(product))
                    {
                        MessageBox.Show("Producto desactivado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Error al desactivar el producto.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }
        
        private void btnLowStock_Click(object sender, EventArgs e)
        {
            var lowStockProducts = _productService.GetLowStockProducts();
            
            if (lowStockProducts.Count == 0)
            {
                MessageBox.Show("No hay productos con stock bajo.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var message = "Productos con stock bajo:\n\n";
            foreach (var product in lowStockProducts)
            {
                message += $"• {product.Name}: {product.StockActual} (mín: {product.StockMinimo})\n";
            }
            
            MessageBox.Show(message, "Stock Bajo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}