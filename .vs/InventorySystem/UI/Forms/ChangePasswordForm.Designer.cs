namespace InventorySystem.UI.Forms
{
    partial class ChangePasswordForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtCurrentPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnChange;
        private Button btnCancel;
        
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
            this.txtCurrentPassword = new TextBox();
            this.txtNewPassword = new TextBox();
            this.txtConfirmPassword = new TextBox();
            this.btnChange = new Button();
            this.btnCancel = new Button();
            
            this.SuspendLayout();
            
            // Labels
            var lblCurrent = new Label();
            lblCurrent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrent.Location = new Point(20, 20);
            lblCurrent.Size = new Size(130, 25);
            lblCurrent.Text = "Contraseña Actual:";
            
            var lblNew = new Label();
            lblNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNew.Location = new Point(20, 60);
            lblNew.Size = new Size(130, 25);
            lblNew.Text = "Nueva Contraseña:";
            
            var lblConfirm = new Label();
            lblConfirm.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblConfirm.Location = new Point(20, 100);
            lblConfirm.Size = new Size(130, 25);
            lblConfirm.Text = "Confirmar:";
            
            // TextBoxes
            this.txtCurrentPassword.Font = new Font("Segoe UI", 10F);
            this.txtCurrentPassword.Location = new Point(160, 20);
            this.txtCurrentPassword.Size = new Size(180, 25);
            this.txtCurrentPassword.UseSystemPasswordChar = true;
            
            this.txtNewPassword.Font = new Font("Segoe UI", 10F);
            this.txtNewPassword.Location = new Point(160, 60);
            this.txtNewPassword.Size = new Size(180, 25);
            this.txtNewPassword.UseSystemPasswordChar = true;
            
            this.txtConfirmPassword.Font = new Font("Segoe UI", 10F);
            this.txtConfirmPassword.Location = new Point(160, 100);
            this.txtConfirmPassword.Size = new Size(180, 25);
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            
            // Buttons
            this.btnChange.BackColor = Color.FromArgb(0, 153, 76);
            this.btnChange.FlatStyle = FlatStyle.Flat;
            this.btnChange.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnChange.ForeColor = Color.White;
            this.btnChange.Location = new Point(160, 150);
            this.btnChange.Size = new Size(100, 35);
            this.btnChange.Text = "Cambiar";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new EventHandler(this.btnChange_Click);
            
            this.btnCancel.BackColor = Color.FromArgb(153, 153, 153);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(270, 150);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            
            // ChangePasswordForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(400, 210);
            this.Controls.Add(lblCurrent);
            this.Controls.Add(lblNew);
            this.Controls.Add(lblConfirm);
            this.Controls.Add(this.txtCurrentPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cambiar Contraseña";
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}