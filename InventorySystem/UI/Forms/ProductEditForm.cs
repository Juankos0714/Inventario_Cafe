using InventorySystem.Models.Entities;

namespace InventorySystem.UI.Forms
{
    public partial class ProductEditForm : Form
    {
        public Product Product { get; private set; }
        private readonly bool _isEdit;
        
        public ProductEditForm(Product? product = null)
        {
            InitializeComponent();
            
            _isEdit = product != null;
            Product = product ?? new Product();
            
            if (_isEdit)
            {
                LoadProductData();
                this.Text = "Editar Producto";
            }
            else
            {
                this.Text = "Nuevo Producto";
            }
        }
        
        private void LoadProductData()
        {
            txtName.Text = Product.Name;
            txtCategory.Text = Product.Category;
            txtPrice.Value = Product.Price;
            txtStock.Value = Product.StockActual;
            txtMinStock.Value = Product.StockMinimo;
            chkActive.Checked = Product.IsActive;
        }
        
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre del producto es requerido.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                MessageBox.Show("La categoría es requerida.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategory.Focus();
                return false;
            }
            
            if (txtPrice.Value <= 0)
            {
                MessageBox.Show("El precio debe ser mayor a cero.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }
            
            return true;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            
            Product.Name = txtName.Text.Trim();
            Product.Category = txtCategory.Text.Trim();
            Product.Price = txtPrice.Value;
            Product.StockActual = (int)txtStock.Value;
            Product.StockMinimo = (int)txtMinStock.Value;
            Product.IsActive = chkActive.Checked;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}