using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.UI.Forms
{
    public partial class UserEditForm : Form
    {
        public string UserName => txtName.Text.Trim();
        public string Password => txtPassword.Text;
        public UserRole Role => (UserRole)cmbRole.SelectedValue!;
        public bool IsActive => chkActive.Checked;
        
        private readonly bool _isEdit;
        
        public UserEditForm(User? user = null)
        {
            InitializeComponent();
            
            _isEdit = user != null;
            LoadRoles();
            
            if (_isEdit && user != null)
            {
                LoadUserData(user);
                txtPassword.Visible = false;
                lblPassword.Visible = false;
                this.Text = "Editar Usuario";
            }
            else
            {
                this.Text = "Nuevo Usuario";
                chkActive.Checked = true;
            }
        }
        
        private void LoadRoles()
        {
            var roles = Enum.GetValues<UserRole>()
                .Select(r => new { Text = r.ToString(), Value = r })
                .ToList();
            
            cmbRole.DataSource = roles;
            cmbRole.DisplayMember = "Text";
            cmbRole.ValueMember = "Value";
        }
        
        private void LoadUserData(User user)
        {
            txtName.Text = user.Name;
            cmbRole.SelectedValue = user.Role;
            chkActive.Checked = user.IsActive;
        }
        
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre de usuario es requerido.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }
            
            if (!_isEdit && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("La contraseña es requerida.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }
            
            if (!_isEdit && txtPassword.Text.Length < 4)
            {
                MessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }
            
            return true;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            
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