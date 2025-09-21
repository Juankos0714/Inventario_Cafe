namespace InventorySystem.UI.Forms
{
    partial class ProductsForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvProducts;
        private Button btnNew;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnLowStock;
        
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
            this.dgvProducts = new DataGridView();
            this.btnNew = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnRefresh = new Button();
            this.btnLowStock = new Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();
            
            // dgvProducts
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.Location = new Point(20, 20);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new Size(740, 400);
            
            // btnNew
            this.btnNew.BackColor = Color.FromArgb(0, 153, 76);
            this.btnNew.FlatStyle = FlatStyle.Flat;
            this.btnNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnNew.ForeColor = Color.White;
            this.btnNew.Location = new Point(20, 440);
            this.btnNew.Size = new Size(100, 35);
            this.btnNew.Text = "Nuevo";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new EventHandler(this.btnNew_Click);
            
            // btnEdit
            this.btnEdit.BackColor = Color.FromArgb(0, 122, 204);
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Location = new Point(130, 440);
            this.btnEdit.Size = new Size(100, 35);
            this.btnEdit.Text = "Editar";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            
            // btnDelete
            this.btnDelete.BackColor = Color.FromArgb(204, 0, 0);
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(240, 440);
            this.btnDelete.Size = new Size(100, 35);
            this.btnDelete.Text = "Desactivar";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            
            // btnRefresh
            this.btnRefresh.BackColor = Color.FromArgb(153, 153, 153);
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Font = new Font("Segoe UI", 10F);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(560, 440);
            this.btnRefresh.Size = new Size(100, 35);
            this.btnRefresh.Text = "Actualizar";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            
            // btnLowStock
            this.btnLowStock.BackColor = Color.FromArgb(255, 153, 0);
            this.btnLowStock.FlatStyle = FlatStyle.Flat;
            this.btnLowStock.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLowStock.ForeColor = Color.White;
            this.btnLowStock.Location = new Point(660, 440);
            this.btnLowStock.Size = new Size(100, 35);
            this.btnLowStock.Text = "Stock Bajo";
            this.btnLowStock.UseVisualStyleBackColor = false;
            this.btnLowStock.Click += new EventHandler(this.btnLowStock_Click);
            
            // ProductsForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnLowStock);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gestión de Productos";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}