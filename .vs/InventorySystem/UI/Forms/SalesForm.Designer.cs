namespace InventorySystem.UI.Forms
{
    partial class SalesForm
    {
        private System.ComponentModel.IContainer components = null;

        // Panel de ventas pendientes
        private Panel pnlVentasPendientes;
        private ListBox lstVentasPendientes;
        private Button btnNuevaVenta;
        private Button btnEliminarVenta;

        // Panel principal de venta
        private Panel pnlVentaPrincipal;
        private TextBox txtNombreVenta;
        private TextBox txtBuscarProducto;
        private ListBox lstProductosFiltrados;
        private NumericUpDown txtQuantity;
        private TextBox txtPrice;
        private TextBox txtMontoRecibido;
        private Label lblStock;
        private Label lblCambio;
        private Button btnAddItem;
        private DataGridView dgvSaleItems;
        private Button btnRemoveItem;
        private Label lblTotal;
        private Button btnGuardarVenta;
        private Button btnProcesarVenta;
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
            this.pnlVentasPendientes = new Panel();
            this.lstVentasPendientes = new ListBox();
            this.btnNuevaVenta = new Button();
            this.btnEliminarVenta = new Button();
            this.pnlVentaPrincipal = new Panel();
            this.txtNombreVenta = new TextBox();
            this.txtBuscarProducto = new TextBox();
            this.lstProductosFiltrados = new ListBox();
            this.txtQuantity = new NumericUpDown();
            this.txtPrice = new TextBox();
            this.txtMontoRecibido = new TextBox();
            this.lblStock = new Label();
            this.lblCambio = new Label();
            this.btnAddItem = new Button();
            this.dgvSaleItems = new DataGridView();
            this.btnRemoveItem = new Button();
            this.lblTotal = new Label();
            this.btnGuardarVenta = new Button();
            this.btnProcesarVenta = new Button();
            this.btnCancel = new Button();

            this.pnlVentasPendientes.SuspendLayout();
            this.pnlVentaPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaleItems)).BeginInit();
            this.SuspendLayout();

            // pnlVentasPendientes
            this.pnlVentasPendientes.BackColor = Color.FromArgb(245, 245, 245);
            this.pnlVentasPendientes.BorderStyle = BorderStyle.FixedSingle;
            this.pnlVentasPendientes.Controls.Add(new Label { Text = "Ventas Pendientes", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(10, 10), Size = new Size(200, 20) });
            this.pnlVentasPendientes.Controls.Add(this.btnNuevaVenta);
            this.pnlVentasPendientes.Controls.Add(this.lstVentasPendientes);
            this.pnlVentasPendientes.Controls.Add(this.btnEliminarVenta);
            this.pnlVentasPendientes.Location = new Point(10, 10);
            this.pnlVentasPendientes.Size = new Size(250, 500);

            // btnNuevaVenta
            this.btnNuevaVenta.BackColor = Color.FromArgb(0, 153, 76);
            this.btnNuevaVenta.FlatStyle = FlatStyle.Flat;
            this.btnNuevaVenta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnNuevaVenta.ForeColor = Color.White;
            this.btnNuevaVenta.Location = new Point(10, 40);
            this.btnNuevaVenta.Size = new Size(100, 30);
            this.btnNuevaVenta.Text = "+ Nueva Venta";
            this.btnNuevaVenta.UseVisualStyleBackColor = false;
            this.btnNuevaVenta.Click += new EventHandler(this.btnNuevaVenta_Click);

            // lstVentasPendientes
            this.lstVentasPendientes.Font = new Font("Segoe UI", 9F);
            this.lstVentasPendientes.Location = new Point(10, 80);
            this.lstVentasPendientes.Size = new Size(220, 350);
            this.lstVentasPendientes.SelectedIndexChanged += new EventHandler(this.lstVentasPendientes_SelectedIndexChanged);

            // btnEliminarVenta
            this.btnEliminarVenta.BackColor = Color.FromArgb(204, 0, 0);
            this.btnEliminarVenta.FlatStyle = FlatStyle.Flat;
            this.btnEliminarVenta.Font = new Font("Segoe UI", 9F);
            this.btnEliminarVenta.ForeColor = Color.White;
            this.btnEliminarVenta.Location = new Point(130, 450);
            this.btnEliminarVenta.Size = new Size(100, 30);
            this.btnEliminarVenta.Text = "Eliminar";
            this.btnEliminarVenta.UseVisualStyleBackColor = false;
            this.btnEliminarVenta.Click += new EventHandler(this.btnEliminarVenta_Click);

            // pnlVentaPrincipal
            this.pnlVentaPrincipal.BackColor = Color.White;
            this.pnlVentaPrincipal.BorderStyle = BorderStyle.FixedSingle;
            this.pnlVentaPrincipal.Controls.Add(new Label { Text = "Nombre de Venta:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(120, 20) });
            this.pnlVentaPrincipal.Controls.Add(this.txtNombreVenta);
            this.pnlVentaPrincipal.Controls.Add(new Label { Text = "Buscar Producto:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(20, 55), Size = new Size(120, 20) });
            this.pnlVentaPrincipal.Controls.Add(this.txtBuscarProducto);
            this.pnlVentaPrincipal.Controls.Add(this.lstProductosFiltrados);
            this.pnlVentaPrincipal.Controls.Add(new Label { Text = "Cantidad:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(350, 55), Size = new Size(70, 20) });
            this.pnlVentaPrincipal.Controls.Add(this.txtQuantity);
            this.pnlVentaPrincipal.Controls.Add(new Label { Text = "Precio:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(450, 55), Size = new Size(50, 20) });
            this.pnlVentaPrincipal.Controls.Add(this.txtPrice);
            this.pnlVentaPrincipal.Controls.Add(this.lblStock);
            this.pnlVentaPrincipal.Controls.Add(this.btnAddItem);
            this.pnlVentaPrincipal.Controls.Add(this.dgvSaleItems);
            this.pnlVentaPrincipal.Controls.Add(this.btnRemoveItem);
            this.pnlVentaPrincipal.Controls.Add(this.lblTotal);
            this.pnlVentaPrincipal.Controls.Add(new Label { Text = "Monto Recibido:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(20, 400), Size = new Size(120, 20) });
            this.pnlVentaPrincipal.Controls.Add(this.txtMontoRecibido);
            this.pnlVentaPrincipal.Controls.Add(this.lblCambio);
            this.pnlVentaPrincipal.Controls.Add(this.btnGuardarVenta);
            this.pnlVentaPrincipal.Controls.Add(this.btnProcesarVenta);
            this.pnlVentaPrincipal.Controls.Add(this.btnCancel);
            this.pnlVentaPrincipal.Location = new Point(270, 10);
            this.pnlVentaPrincipal.Size = new Size(650, 500);

            // txtNombreVenta
            this.txtNombreVenta.Font = new Font("Segoe UI", 10F);
            this.txtNombreVenta.Location = new Point(150, 15);
            this.txtNombreVenta.Size = new Size(200, 25);
            this.txtNombreVenta.TextChanged += new EventHandler(this.txtNombreVenta_TextChanged);

            // txtBuscarProducto
            this.txtBuscarProducto.Font = new Font("Segoe UI", 10F);
            this.txtBuscarProducto.Location = new Point(20, 80);
            this.txtBuscarProducto.Size = new Size(300, 25);

            // lstProductosFiltrados
            this.lstProductosFiltrados.Font = new Font("Segoe UI", 9F);
            this.lstProductosFiltrados.Location = new Point(20, 105);
            this.lstProductosFiltrados.Size = new Size(300, 80);
            this.lstProductosFiltrados.Visible = false;

            // txtQuantity
            this.txtQuantity.Font = new Font("Segoe UI", 10F);
            this.txtQuantity.Location = new Point(350, 80);
            this.txtQuantity.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.txtQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.txtQuantity.Size = new Size(80, 25);
            this.txtQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // txtPrice
            this.txtPrice.Font = new Font("Segoe UI", 10F);
            this.txtPrice.Location = new Point(450, 80);
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new Size(80, 25);
            this.txtPrice.TextAlign = HorizontalAlignment.Right;

            // lblStock
            this.lblStock.Font = new Font("Segoe UI", 9F);
            this.lblStock.Location = new Point(350, 110);
            this.lblStock.Size = new Size(100, 20);
            this.lblStock.Text = "Stock: 0";

            // btnAddItem
            this.btnAddItem.BackColor = Color.FromArgb(0, 122, 204);
            this.btnAddItem.FlatStyle = FlatStyle.Flat;
            this.btnAddItem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAddItem.ForeColor = Color.White;
            this.btnAddItem.Location = new Point(550, 80);
            this.btnAddItem.Size = new Size(100, 35);
            this.btnAddItem.Text = "Agregar";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.btnAddItem.Click += new EventHandler(this.btnAddItem_Click);

            // dgvSaleItems
            this.dgvSaleItems.AllowUserToAddRows = false;
            this.dgvSaleItems.AllowUserToDeleteRows = false;
            this.dgvSaleItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSaleItems.Location = new Point(20, 140);
            this.dgvSaleItems.MultiSelect = false;
            this.dgvSaleItems.ReadOnly = true;
            this.dgvSaleItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSaleItems.Size = new Size(610, 200);

            // btnRemoveItem
            this.btnRemoveItem.BackColor = Color.FromArgb(204, 0, 0);
            this.btnRemoveItem.FlatStyle = FlatStyle.Flat;
            this.btnRemoveItem.Font = new Font("Segoe UI", 9F);
            this.btnRemoveItem.ForeColor = Color.White;
            this.btnRemoveItem.Location = new Point(530, 350);
            this.btnRemoveItem.Size = new Size(100, 30);
            this.btnRemoveItem.Text = "Quitar";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.btnRemoveItem.Click += new EventHandler(this.btnRemoveItem_Click);

            // lblTotal
            this.lblTotal.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTotal.Location = new Point(350, 350);
            this.lblTotal.Size = new Size(200, 30);
            this.lblTotal.Text = "Total: $0.00";
            this.lblTotal.TextAlign = ContentAlignment.MiddleLeft;

            // txtMontoRecibido
            this.txtMontoRecibido.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.txtMontoRecibido.Location = new Point(20, 425);
            this.txtMontoRecibido.Size = new Size(120, 29);
            this.txtMontoRecibido.TextAlign = HorizontalAlignment.Right;
            this.txtMontoRecibido.TextChanged += new EventHandler(this.txtMontoRecibido_TextChanged);

            // lblCambio
            this.lblCambio.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCambio.Location = new Point(160, 425);
            this.lblCambio.Size = new Size(150, 25);
            this.lblCambio.Text = "Cambio: $0.00";
            this.lblCambio.TextAlign = ContentAlignment.MiddleLeft;

            // btnGuardarVenta
            this.btnGuardarVenta.BackColor = Color.FromArgb(255, 153, 0);
            this.btnGuardarVenta.FlatStyle = FlatStyle.Flat;
            this.btnGuardarVenta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnGuardarVenta.ForeColor = Color.White;
            this.btnGuardarVenta.Location = new Point(350, 420);
            this.btnGuardarVenta.Size = new Size(100, 35);
            this.btnGuardarVenta.Text = "Guardar";
            this.btnGuardarVenta.UseVisualStyleBackColor = false;
            this.btnGuardarVenta.Click += new EventHandler(this.btnGuardarVenta_Click);

            // btnProcesarVenta
            this.btnProcesarVenta.BackColor = Color.FromArgb(0, 153, 76);
            this.btnProcesarVenta.FlatStyle = FlatStyle.Flat;
            this.btnProcesarVenta.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnProcesarVenta.ForeColor = Color.White;
            this.btnProcesarVenta.Location = new Point(460, 420);
            this.btnProcesarVenta.Size = new Size(120, 35);
            this.btnProcesarVenta.Text = "Procesar";
            this.btnProcesarVenta.UseVisualStyleBackColor = false;
            this.btnProcesarVenta.Click += new EventHandler(this.btnProcesarVenta_Click);

            // btnCancel
            this.btnCancel.BackColor = Color.FromArgb(153, 153, 153);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(20, 460);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Text = "Limpiar";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // SalesForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.ClientSize = new Size(940, 530);
            this.Controls.Add(this.pnlVentasPendientes);
            this.Controls.Add(this.pnlVentaPrincipal);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Ventas - Gestión Temporal";

            this.pnlVentasPendientes.ResumeLayout(false);
            this.pnlVentaPrincipal.ResumeLayout(false);
            this.pnlVentaPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaleItems)).EndInit();
            this.ResumeLayout(false);
        }
    }
}