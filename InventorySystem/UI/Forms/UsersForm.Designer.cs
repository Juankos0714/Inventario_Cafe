namespace InventorySystem.UI.Forms
{
    partial class UsersForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvUsers;
        private Button btnNew;
        private Button btnEdit;
        private Button btnResetPassword;
        private Button btnRefresh;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            this.dgvUsers = new DataGridView();
            this.btnNew = new Button();
            this.btnEdit = new Button();
            this.btnResetPassword = new Button();
            this.btnRefresh = new Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            
            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.Location = new Point(20, 20);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new Size(640, 350);
            
            // btnNew
            this.btnNew.BackColor = Color.FromArgb(0, 153, 76);
            this.btnNew.FlatStyle = FlatStyle.Flat;
            this.btnNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnNew.ForeColor = Color.White;
            this.btnNew.Location = new Point(20, 390);
            this.btnNew.Size = new Size(100, 35);
            this.btnNew.Text = "Nuevo";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new EventHandler(this.btnNew_Click);
            
            // btnEdit
            this.btnEdit.BackColor = Color.FromArgb(0, 122, 204);
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Location = new Point(130, 390);
            this.btnEdit.Size = new Size(100, 35);
            this.btnEdit.Text = "Editar";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            
            // btnResetPassword
            this.btnResetPassword.BackColor = Color.FromArgb(255, 153, 0);
            this.btnResetPassword.FlatStyle = FlatStyle.Flat;
            this.btnResetPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnResetPassword.ForeColor = Color.White;
            this.btnResetPassword.Location = new Point(240, 390);
            this.btnResetPassword.Size = new Size(130, 35);
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.UseVisualStyleBackColor = false;
            this.btnResetPassword.Click += new EventHandler(this.btnResetPassword_Click);
            
            // btnRefresh
            this.btnRefresh.BackColor = Color.FromArgb(153, 153, 153);
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Font = new Font("Segoe UI", 10F);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(560, 390);
            this.btnRefresh.Size = new Size(100, 35);
            this.btnRefresh.Text = "Actualizar";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            
            // UsersForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(680, 450);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.btnRefresh);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gestión de Usuarios";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }
    }
}