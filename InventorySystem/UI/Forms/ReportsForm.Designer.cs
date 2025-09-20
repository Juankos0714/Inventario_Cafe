namespace InventorySystem.UI.Forms
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private DateTimePicker dtpReportDate;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private ComboBox cmbMovementType;
        private Button btnDailySummary;
        private Button btnMovements;
        private Button btnInventoryReport;
        private Button btnExportDaily;
        private Button btnExportMovements;
        private Button btnExportInventory;
        private Button btnCreateDailyClose;
        private RichTextBox txtReport;
        
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
            this.dtpReportDate = new DateTimePicker();
            this.dtpStartDate = new DateTimePicker();
            this.dtpEndDate = new DateTimePicker();
            this.cmbMovementType = new ComboBox();
            this.btnDailySummary = new Button();
            this.btnMovements = new Button();
            this.btnInventoryReport = new Button();
            this.btnExportDaily = new Button();
            this.btnExportMovements = new Button();
            this.btnExportInventory = new Button();
            this.btnCreateDailyClose = new Button();
            this.txtReport = new RichTextBox();
            
            this.SuspendLayout();
            
            // Labels
            var lblReportDate = new Label();
            lblReportDate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblReportDate.Location = new Point(20, 20);
            lblReportDate.Size = new Size(100, 20);
            lblReportDate.Text = "Fecha:";
            
            var lblDateRange = new Label();
            lblDateRange.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDateRange.Location = new Point(20, 60);
            lblDateRange.Size = new Size(100, 20);
            lblDateRange.Text = "Rango:";
            
            var lblMovementType = new Label();
            lblMovementType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMovementType.Location = new Point(400, 60);
            lblMovementType.Size = new Size(80, 20);
            lblMovementType.Text = "Tipo:";
            
            // Controls
            this.dtpReportDate = new DateTimePicker();
            this.dtpReportDate.Font = new Font("Segoe UI", 9F);
            this.dtpReportDate.Format = DateTimePickerFormat.Short;
            this.dtpReportDate.Location = new Point(130, 20);
            this.dtpReportDate.Size = new Size(120, 23);
            
            this.dtpStartDate = new DateTimePicker();
            this.dtpStartDate.Font = new Font("Segoe UI", 9F);
            this.dtpStartDate.Format = DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new Point(130, 60);
            this.dtpStartDate.Size = new Size(120, 23);
            
            this.dtpEndDate = new DateTimePicker();
            this.dtpEndDate.Font = new Font("Segoe UI", 9F);
            this.dtpEndDate.Format = DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new Point(260, 60);
            this.dtpEndDate.Size = new Size(120, 23);
            
            this.cmbMovementType = new ComboBox();
            this.cmbMovementType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMovementType.Font = new Font("Segoe UI", 9F);
            this.cmbMovementType.Items.AddRange(new object[] { "Todos", "Venta", "Entrada", "Ajuste" });
            this.cmbMovementType.Location = new Point(490, 60);
            this.cmbMovementType.SelectedIndex = 0;
            this.cmbMovementType.Size = new Size(100, 23);
            
            // Buttons - First Row
            this.btnDailySummary = new Button();
            this.btnDailySummary.BackColor = Color.FromArgb(0, 122, 204);
            this.btnDailySummary.FlatStyle = FlatStyle.Flat;
            this.btnDailySummary.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnDailySummary.ForeColor = Color.White;
            this.btnDailySummary.Location = new Point(20, 110);
            this.btnDailySummary.Size = new Size(120, 35);
            this.btnDailySummary.Text = "Resumen Diario";
            this.btnDailySummary.UseVisualStyleBackColor = false;
            this.btnDailySummary.Click += new EventHandler(this.btnDailySummary_Click);
            
            this.btnMovements = new Button();
            this.btnMovements.BackColor = Color.FromArgb(0, 122, 204);
            this.btnMovements.FlatStyle = FlatStyle.Flat;
            this.btnMovements.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnMovements.ForeColor = Color.White;
            this.btnMovements.Location = new Point(150, 110);
            this.btnMovements.Size = new Size(120, 35);
            this.btnMovements.Text = "Movimientos";
            this.btnMovements.UseVisualStyleBackColor = false;
            this.btnMovements.Click += new EventHandler(this.btnMovements_Click);
            
            this.btnInventoryReport = new Button();
            this.btnInventoryReport.BackColor = Color.FromArgb(0, 122, 204);
            this.btnInventoryReport.FlatStyle = FlatStyle.Flat;
            this.btnInventoryReport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnInventoryReport.ForeColor = Color.White;
            this.btnInventoryReport.Location = new Point(280, 110);
            this.btnInventoryReport.Size = new Size(120, 35);
            this.btnInventoryReport.Text = "Inventario";
            this.btnInventoryReport.UseVisualStyleBackColor = false;
            this.btnInventoryReport.Click += new EventHandler(this.btnInventoryReport_Click);
            
            this.btnCreateDailyClose = new Button();
            this.btnCreateDailyClose.BackColor = Color.FromArgb(0, 153, 76);
            this.btnCreateDailyClose.FlatStyle = FlatStyle.Flat;
            this.btnCreateDailyClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnCreateDailyClose.ForeColor = Color.White;
            this.btnCreateDailyClose.Location = new Point(410, 110);
            this.btnCreateDailyClose.Size = new Size(120, 35);
            this.btnCreateDailyClose.Text = "Cierre Diario";
            this.btnCreateDailyClose.UseVisualStyleBackColor = false;
            this.btnCreateDailyClose.Click += new EventHandler(this.btnCreateDailyClose_Click);
            
            // Export Buttons - Second Row
            this.btnExportDaily = new Button();
            this.btnExportDaily.BackColor = Color.FromArgb(255, 153, 0);
            this.btnExportDaily.FlatStyle = FlatStyle.Flat;
            this.btnExportDaily.Font = new Font("Segoe UI", 9F);
            this.btnExportDaily.ForeColor = Color.White;
            this.btnExportDaily.Location = new Point(20, 155);
            this.btnExportDaily.Size = new Size(120, 30);
            this.btnExportDaily.Text = "Exportar Diario";
            this.btnExportDaily.UseVisualStyleBackColor = false;
            this.btnExportDaily.Click += new EventHandler(this.btnExportDaily_Click);
            
            this.btnExportMovements = new Button();
            this.btnExportMovements.BackColor = Color.FromArgb(255, 153, 0);
            this.btnExportMovements.FlatStyle = FlatStyle.Flat;
            this.btnExportMovements.Font = new Font("Segoe UI", 9F);
            this.btnExportMovements.ForeColor = Color.White;
            this.btnExportMovements.Location = new Point(150, 155);
            this.btnExportMovements.Size = new Size(120, 30);
            this.btnExportMovements.Text = "Exportar Mov.";
            this.btnExportMovements.UseVisualStyleBackColor = false;
            this.btnExportMovements.Click += new EventHandler(this.btnExportMovements_Click);
            
            this.btnExportInventory = new Button();
            this.btnExportInventory.BackColor = Color.FromArgb(255, 153, 0);
            this.btnExportInventory.FlatStyle = FlatStyle.Flat;
            this.btnExportInventory.Font = new Font("Segoe UI", 9F);
            this.btnExportInventory.ForeColor = Color.White;
            this.btnExportInventory.Location = new Point(280, 155);
            this.btnExportInventory.Size = new Size(120, 30);
            this.btnExportInventory.Text = "Exportar Inv.";
            this.btnExportInventory.UseVisualStyleBackColor = false;
            this.btnExportInventory.Click += new EventHandler(this.btnExportInventory_Click);
            
            // Report Text Area
            this.txtReport = new RichTextBox();
            this.txtReport.Font = new Font("Consolas", 9F);
            this.txtReport.Location = new Point(20, 200);
            this.txtReport.ReadOnly = true;
            this.txtReport.Size = new Size(740, 300);
            
            // ReportsForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(780, 520);
            this.Controls.Add(lblReportDate);
            this.Controls.Add(lblDateRange);
            this.Controls.Add(lblMovementType);
            this.Controls.Add(this.dtpReportDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.cmbMovementType);
            this.Controls.Add(this.btnDailySummary);
            this.Controls.Add(this.btnMovements);
            this.Controls.Add(this.btnInventoryReport);
            this.Controls.Add(this.btnCreateDailyClose);
            this.Controls.Add(this.btnExportDaily);
            this.Controls.Add(this.btnExportMovements);
            this.Controls.Add(this.btnExportInventory);
            this.Controls.Add(this.txtReport);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Reportes y Análisis";
            
            this.ResumeLayout(false);
        }
    }
}