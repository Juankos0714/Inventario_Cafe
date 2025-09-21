namespace InventorySystem.UI.Forms
{
    partial class ProductEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtName;
        private TextBox txtCategory;
        private NumericUpDown txtPrice;
        private NumericUpDown txtStock;
        private NumericUpDown txtMinStock;
        private CheckBox chkActive;
        private Button btnSave;
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
            this.txtName = new TextBox();
            this.txtCategory = new TextBox();
            this.txtPrice = new NumericUpDown();
            this.txtStock = new NumericUpDown();
            this.txtMinStock = new NumericUpDown();
            this.chkActive = new CheckBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinStock)).BeginInit();
            this.SuspendLayout();
            
            // Labels
            var lblName = new Label();
            lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblName.Location = new Point(20, 20);
            lblName.Size = new Size(100, 25);
            lblName.Text = "Nombre:";
            
            var lblCategory = new Label();
            lblCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategory.Location = new Point(20, 60);
            lblCategory.Size = new Size(100, 25);
            lblCategory.Text = "Categoría:";
            
            var lblPrice = new Label();
            lblPrice.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPrice.Location = new Point(20, 100);
            lblPrice.Size = new Size(100, 25);
            lblPrice.Text = "Precio:";
            
            var lblStock = new Label();
            lblStock.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStock.Location = new Point(20, 140);
            lblStock.Size = new Size(100, 25);
            lblStock.Text = "Stock Actual:";
            
            var lblMinStock = new Label();
            lblMinStock.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMinStock.Location = new Point(20, 180);
            lblMinStock.Size = new Size(100, 25);
            lblMinStock.Text = "Stock Mínimo:";
            
            // txtName
            this.txtName.Font = new Font("Segoe UI", 10F);
            this.txtName.Location = new Point(130, 20);
            this.txtName.Size = new Size(200, 25);
            
            // txtCategory
            this.txtCategory.Font = new Font("Segoe UI", 10F);
            this.txtCategory.Location = new Point(130, 60);
            this.txtCategory.Size = new Size(200, 25);
            
            // txtPrice
            this.txtPrice.DecimalPlaces = 2;
            this.txtPrice.Font = new Font("Segoe UI", 10F);
            this.txtPrice.Location = new Point(130, 100);
            this.txtPrice.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.txtPrice.Size = new Size(120, 25);
            
            // txtStock
            this.txtStock.Font = new Font("Segoe UI", 10F);
            this.txtStock.Location = new Point(130, 140);
            this.txtStock.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.txtStock.Size = new Size(120, 25);
            
            // txtMinStock
            this.txtMinStock.Font = new Font("Segoe UI", 10F);
            this.txtMinStock.Location = new Point(130, 180);
            this.txtMinStock.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.txtMinStock.Size = new Size(120, 25);
            
            // chkActive
            this.chkActive.Checked = true;
            this.chkActive.CheckState = CheckState.Checked;
            this.chkActive.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.chkActive.Location = new Point(130, 220);
            this.chkActive.Size = new Size(100, 24);
            this.chkActive.Text = "Activo";
            this.chkActive.UseVisualStyleBackColor = true;
            
            // btnSave
            this.btnSave.BackColor = Color.FromArgb(0, 153, 76);
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(130, 270);
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.Text = "Guardar";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.BackColor = Color.FromArgb(153, 153, 153);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(240, 270);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            
            // ProductEditForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(380, 330);
            this.Controls.Add(lblName);
            this.Controls.Add(lblCategory);
            this.Controls.Add(lblPrice);
            this.Controls.Add(lblStock);
            this.Controls.Add(lblMinStock);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.txtMinStock);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Producto";
            
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}