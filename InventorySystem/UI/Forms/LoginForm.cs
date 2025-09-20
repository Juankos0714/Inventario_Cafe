using InventorySystem.BLL;
using InventorySystem.Models.Enums;

namespace InventorySystem.UI.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;
        
        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor ingrese usuario y contraseña.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_authService.Login(username, password))
            {
                this.Hide();
                var mainForm = new MainForm(_authService);
                mainForm.ShowDialog();
                this.Show();
                
                // Limpiar campos después del logout
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtUsername.Focus();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtPassword.Focus();
            }
        }
        
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
        
        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }
    }
}