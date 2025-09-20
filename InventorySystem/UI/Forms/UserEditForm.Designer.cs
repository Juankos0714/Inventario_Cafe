namespace InventorySystem.UI.Forms
{
    partial class UserEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtName;
        private TextBox txtPassword;
        private ComboBox cmbRole;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnCancel;
        private Label lblPassword;
        
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
            this.txtName = new TextBox();
            this.txtPassword = new TextBox();
            this.cmbRole = new ComboBox();
            this.chkActive = new CheckBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.lblPassword = new Label();
            
            this.SuspendLayout();
            
            // Labels
            var lblName = new Label();
            lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblName.Location = new Point(20, 20);
            lblName.Size = new Size(100, 25);
            lblName.Text = "Nombre:";
            
            this.lblPassword = new Label();
            this.lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblPassword.Location = new Point(20, 60);
            this.lblPassword.Size = new Size(100, 25);
            this.lblPassword.Text = "Contraseña:";
            
            var lblRole = new Label();
            lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRole.Location = new Point(20, 100);
            lblRole.Size = new Size(100, 25);
            lblRole.Text = "Rol:";
            
            // txtName
            this.txtName.Font = new Font("Segoe UI", 10F);
            this.txtName.Location = new Point(130, 20);
            this.txtName.Size = new Size(200, 25);
            
            // txtPassword
            this.txtPassword.Font = new Font("Segoe UI", 10F);
            this.txtPassword.Location = new Point(130, 60);
            this.txtPassword.Size = new Size(200, 25);
            this.txtPassword.UseSystemPasswordChar = true;
            
            // cmbRole
            this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRole.Font = new Font("Segoe UI", 10F);
            this.cmbRole.Location = new Point(130, 100);
            this.cmbRole.Size = new Size(150, 25);
            
            // chkActive
            this.chkActive.Checked = true;
            this.chkActive.CheckState = CheckState.Checked;
            this.chkActive.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.chkActive.Location = new Point(130, 140);
            this.chkActive.Size = new Size(100, 24);
            this.chkActive.Text = "Activo";
            this.chkActive.UseVisualStyleBackColor = true;
            
            // btnSave
            this.btnSave.BackColor = Color.FromArgb(0, 153, 76);
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(130, 190);
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.Text = "Guardar";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.BackColor = Color.FromArgb(153, 153, 153);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(240, 190);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            
            // UserEditForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(380, 250);
            this.Controls.Add(lblName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(lblRole);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Usuario";
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}