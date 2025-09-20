namespace InventorySystem.UI.Forms
{
    partial class VentaAliasForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtNombreVenta;
        private Button btnOK;
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
            this.txtNombreVenta = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            
            this.SuspendLayout();
            
            // Label
            var lblNombre = new Label();
            lblNombre.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNombre.Location = new Point(20, 20);
            lblNombre.Size = new Size(150, 25);
            lblNombre.Text = "Nombre de la venta:";
            
            // txtNombreVenta
            this.txtNombreVenta.Font = new Font("Segoe UI", 10F);
            this.txtNombreVenta.Location = new Point(20, 50);
            this.txtNombreVenta.Size = new Size(250, 25);
            this.txtNombreVenta.KeyPress += new KeyPressEventHandler(this.txtNombreVenta_KeyPress);
            
            // btnOK
            this.btnOK.BackColor = Color.FromArgb(0, 153, 76);
            this.btnOK.FlatStyle = FlatStyle.Flat;
            this.btnOK.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.Location = new Point(95, 90);
            this.btnOK.Size = new Size(80, 35);
            this.btnOK.Text = "Crear";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            
            // btnCancel
            this.btnCancel.BackColor = Color.FromArgb(153, 153, 153);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(185, 90);
            this.btnCancel.Size = new Size(80, 35);
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            
            // VentaAliasForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(290, 150);
            this.Controls.Add(lblNombre);
            this.Controls.Add(this.txtNombreVenta);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Nueva Venta";
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}