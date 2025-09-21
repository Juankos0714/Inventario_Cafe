namespace InventorySystem.UI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuSales;
        private ToolStripMenuItem mnuProducts;
        private ToolStripMenuItem mnuUsers;
        private ToolStripMenuItem mnuShifts;
        private ToolStripMenuItem mnuStartShift;
        private ToolStripMenuItem mnuCloseShift;
        private ToolStripMenuItem mnuReports;
        private ToolStripMenuItem mnuSystem;
        private ToolStripMenuItem mnuChangePassword;
        private ToolStripMenuItem mnuLogout;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblUser;
        private ToolStripStatusLabel lblShiftStatus;
        
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
            this.menuStrip1 = new MenuStrip();
            this.mnuSales = new ToolStripMenuItem();
            this.mnuProducts = new ToolStripMenuItem();
            this.mnuUsers = new ToolStripMenuItem();
            this.mnuShifts = new ToolStripMenuItem();
            this.mnuStartShift = new ToolStripMenuItem();
            this.mnuCloseShift = new ToolStripMenuItem();
            this.mnuReports = new ToolStripMenuItem();
            this.mnuSystem = new ToolStripMenuItem();
            this.mnuChangePassword = new ToolStripMenuItem();
            this.mnuLogout = new ToolStripMenuItem();
            this.statusStrip1 = new StatusStrip();
            this.lblUser = new ToolStripStatusLabel();
            this.lblShiftStatus = new ToolStripStatusLabel();
            
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // menuStrip1
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
                this.mnuSales,
                this.mnuProducts,
                this.mnuUsers,
                this.mnuShifts,
                this.mnuReports,
                this.mnuSystem});
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Size = new Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            
            // mnuSales
            this.mnuSales.Text = "&Ventas";
            this.mnuSales.Click += new EventHandler(this.mnuSales_Click);
            
            // mnuProducts
            this.mnuProducts.Text = "&Productos";
            this.mnuProducts.Click += new EventHandler(this.mnuProducts_Click);
            
            // mnuUsers
            this.mnuUsers.Text = "&Usuarios";
            this.mnuUsers.Click += new EventHandler(this.mnuUsers_Click);
            
            // mnuShifts
            this.mnuShifts.Text = "&Turnos";
            this.mnuShifts.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuStartShift,
                this.mnuCloseShift});
            
            // mnuStartShift
            this.mnuStartShift.Text = "&Iniciar Turno";
            this.mnuStartShift.Click += new EventHandler(this.mnuStartShift_Click);
            
            // mnuCloseShift
            this.mnuCloseShift.Text = "&Cerrar Turno";
            this.mnuCloseShift.Click += new EventHandler(this.mnuCloseShift_Click);
            
            // mnuReports
            this.mnuReports.Text = "&Reportes";
            this.mnuReports.Click += new EventHandler(this.mnuReports_Click);
            
            // mnuSystem
            this.mnuSystem.Text = "&Sistema";
            this.mnuSystem.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuChangePassword,
                new ToolStripSeparator(),
                this.mnuLogout});
            
            // mnuChangePassword
            this.mnuChangePassword.Text = "Cambiar &Contraseña";
            this.mnuChangePassword.Click += new EventHandler(this.mnuChangePassword_Click);
            
            // mnuLogout
            this.mnuLogout.Text = "Cerrar &Sesión";
            this.mnuLogout.Click += new EventHandler(this.mnuLogout_Click);
            
            // statusStrip1
            this.statusStrip1.Items.AddRange(new ToolStripItem[] {
                this.lblUser,
                this.lblShiftStatus});
            this.statusStrip1.Location = new Point(0, 428);
            this.statusStrip1.Size = new Size(800, 22);
            
            // lblUser
            this.lblUser.Text = "Usuario: ";
            
            // lblShiftStatus
            this.lblShiftStatus.Text = "Estado del turno";
            this.lblShiftStatus.Spring = true;
            this.lblShiftStatus.TextAlign = ContentAlignment.MiddleRight;
            
            // MainForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = false;
            this.MainMenuStrip = this.menuStrip1;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Inventario - Menú Principal";
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}