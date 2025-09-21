namespace InventorySystem.UI.Forms
{
    public partial class VentaAliasForm : Form
    {
        public string NombreVenta => txtNombreVenta.Text.Trim();
        
        public VentaAliasForm()
        {
            InitializeComponent();
            txtNombreVenta.Text = $"Venta {DateTime.Now:HHmm}";
            txtNombreVenta.SelectAll();
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreVenta.Text))
            {
                MessageBox.Show("Ingrese un nombre para la venta.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreVenta.Focus();
                return;
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void txtNombreVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}