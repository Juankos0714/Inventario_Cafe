using InventorySystem.BLL;
using InventorySystem.Models.Enums;

namespace InventorySystem.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly AuthService _authService;
        private readonly ShiftService _shiftService;
        
        public MainForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _shiftService = new ShiftService(_authService);
            
            ConfigureMenuByRole();
            UpdateStatusBar();
        }
        
        private void ConfigureMenuByRole()
        {
            if (_authService.CurrentUser == null) return;
            
            var role = _authService.CurrentUser.Role;
            
            // Configurar menús según rol
            switch (role)
            {
                case UserRole.Admin:
                    // Administrador tiene acceso a todo
                    break;
                    
                case UserRole.Vendedor:
                    // Vendedor solo puede vender y cerrar turno
                    mnuUsers.Visible = false;
                    mnuProducts.Enabled = false; // Solo lectura
                    mnuReports.Visible = false;
                    break;
                    
                case UserRole.Contador:
                    // Contador solo reportes
                    mnuUsers.Visible = false;
                    mnuProducts.Enabled = false;
                    mnuSales.Enabled = false;
                    mnuShifts.Enabled = false;
                    break;
            }
            
            lblUser.Text = $"Usuario: {_authService.CurrentUser.Name} ({role})";
        }
        
        private void UpdateStatusBar()
        {
            var activeShift = _shiftService.GetActiveShift();
            if (activeShift != null)
            {
                lblShiftStatus.Text = $"Turno activo desde: {activeShift.StartTime:HH:mm}";
                lblShiftStatus.BackColor = Color.LightGreen;
            }
            else
            {
                lblShiftStatus.Text = "Sin turno activo";
                lblShiftStatus.BackColor = Color.LightCoral;
            }
        }
        
        private void mnuSales_Click(object sender, EventArgs e)
        {
            using var form = new SalesForm(_authService);
            form.ShowDialog();
            UpdateStatusBar();
        }
        
        private void mnuProducts_Click(object sender, EventArgs e)
        {
            using var form = new ProductsForm(_authService);
            form.ShowDialog();
        }
        
        private void mnuUsers_Click(object sender, EventArgs e)
        {
            using var form = new UsersForm(_authService);
            form.ShowDialog();
        }
        
        private void mnuReports_Click(object sender, EventArgs e)
        {
            using var form = new ReportsForm(_authService);
            form.ShowDialog();
        }
        
        private void mnuStartShift_Click(object sender, EventArgs e)
        {
            if (_shiftService.StartShift())
            {
                MessageBox.Show("Turno iniciado correctamente.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatusBar();
            }
            else
            {
                MessageBox.Show("No se pudo iniciar el turno. Verifique que no tenga un turno activo.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void mnuCloseShift_Click(object sender, EventArgs e)
        {
            var activeShift = _shiftService.GetActiveShift();
            if (activeShift == null)
            {
                MessageBox.Show("No hay un turno activo para cerrar.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var result = MessageBox.Show("¿Está seguro que desea cerrar el turno actual?", 
                "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                if (_shiftService.CloseShift())
                {
                    MessageBox.Show("Turno cerrado correctamente.", "Información", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatusBar();
                }
                else
                {
                    MessageBox.Show("Error al cerrar el turno.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void mnuChangePassword_Click(object sender, EventArgs e)
        {
            using var form = new ChangePasswordForm(_authService);
            form.ShowDialog();
        }
        
        private void mnuLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", 
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                _authService.Logout();
                this.Close();
            }
        }
        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Verificar si hay turno activo
            var activeShift = _shiftService.GetActiveShift();
            if (activeShift != null)
            {
                var result = MessageBox.Show("Tiene un turno activo. ¿Desea cerrarlo antes de salir?", 
                    "Turno Activo", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    _shiftService.CloseShift();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}