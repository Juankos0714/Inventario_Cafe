namespace InventorySystem.UI.Forms
{
    public partial class TempSaleAliasForm : Form
    {
        public string Alias => txtAlias.Text.Trim();
        
        public TempSaleAliasForm()
        {
            InitializeComponent();
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAlias.Text))
            {
                MessageBox.Show("Ingrese un nombre para la venta.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlias.Focus();
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
        
        private void txtAlias_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}