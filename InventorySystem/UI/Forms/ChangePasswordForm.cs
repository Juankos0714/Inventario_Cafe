using InventorySystem.BLL;

namespace InventorySystem.UI.Forms
{
    public partial class ChangePasswordForm : Form
    {
        private readonly AuthService _authService;
        
        public ChangePasswordForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }
        
        private void btnChange_Click(object sender, EventArgs e)
        {
            var currentPassword = txtCurrentPassword.Text;
            var newPassword = txtNewPassword.Text;
            var confirmPassword = txtConfirmPassword.Text;
            
            if (string.IsNullOrEmpty(currentPassword))
            {
                MessageBox.Show("Ingrese la contraseña actual.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrentPassword.Focus();
                return;
            }
            
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Ingrese la nueva contraseña.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }
            
            if (newPassword.Length < 4)
            {
                MessageBox.Show("La nueva contraseña debe tener al menos 4 caracteres.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }
            
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("La confirmación no coincide con la nueva contraseña.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }
            
            if (_authService.ChangePassword(currentPassword, newPassword))
            {
                MessageBox.Show("Contraseña cambiada correctamente.", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Contraseña actual incorrecta.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCurrentPassword.Focus();
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}