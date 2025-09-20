using InventorySystem.BLL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.UI.Forms
{
    public partial class UsersForm : Form
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly List<User> _users;
        
        public UsersForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _userService = new UserService(_authService);
            _users = new List<User>();
            
            LoadUsers();
        }
        
        private void LoadUsers()
        {
            _users.Clear();
            _users.AddRange(_userService.GetAllUsers());
            RefreshGrid();
        }
        
        private void RefreshGrid()
        {
            var displayUsers = _users.Select(u => new
            {
                u.Id,
                Nombre = u.Name,
                Rol = u.Role.ToString(),
                Estado = u.IsActive ? "Activo" : "Inactivo",
                FechaCreación = u.CreatedAt
            }).ToList();
            
            dgvUsers.DataSource = displayUsers;
            
            if (dgvUsers.Columns.Count > 0)
            {
                dgvUsers.Columns["Id"].Visible = false;
                dgvUsers.Columns["FechaCreación"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            using var form = new UserEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (_userService.CreateUser(form.UserName, form.Password, form.Role))
                {
                    MessageBox.Show("Usuario creado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Error al crear el usuario. Verifique que el nombre no exista.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para editar.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var userId = (int)dgvUsers.SelectedRows[0].Cells["Id"].Value;
            var user = _users.FirstOrDefault(u => u.Id == userId);
            
            if (user != null)
            {
                using var form = new UserEditForm(user);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (_userService.UpdateUser(user.Id, form.UserName, form.Role, form.IsActive))
                    {
                        MessageBox.Show("Usuario actualizado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el usuario.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para resetear la contraseña.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var userId = (int)dgvUsers.SelectedRows[0].Cells["Id"].Value;
            var user = _users.FirstOrDefault(u => u.Id == userId);
            
            if (user != null)
            {
                var result = MessageBox.Show($"¿Resetear contraseña de '{user.Name}' a '123456'?", 
                    "Confirmar Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    if (_userService.ResetPassword(user.Id, "123456"))
                    {
                        MessageBox.Show("Contraseña reseteada correctamente. Nueva contraseña: 123456", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al resetear la contraseña.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}