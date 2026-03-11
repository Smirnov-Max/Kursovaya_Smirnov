namespace Smirnov_kursovaya.secondForm
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.DataGridView reportsDataGridView;
        private System.Windows.Forms.DateTimePicker reportFromDatePicker;
        private System.Windows.Forms.DateTimePicker reportToDatePicker;
        private System.Windows.Forms.ComboBox reportTypeComboBox;
        private System.Windows.Forms.Button generateReportButton;
        private System.Windows.Forms.Button printToWordButton;
        private System.Windows.Forms.Button printToExcelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label totalsLabel;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.reportsDataGridView = new System.Windows.Forms.DataGridView();
            this.reportFromDatePicker = new System.Windows.Forms.DateTimePicker();
            this.reportToDatePicker = new System.Windows.Forms.DateTimePicker();
            this.reportTypeComboBox = new System.Windows.Forms.ComboBox();
            this.generateReportButton = new System.Windows.Forms.Button();
            this.printToWordButton = new System.Windows.Forms.Button();
            this.printToExcelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.totalsLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSalmon;
            this.panel1.Controls.Add(this.menuButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(824, 60);
            this.panel1.TabIndex = 0;
            // 
            // menuButton
            // 
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.Location = new System.Drawing.Point(712, 15);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(100, 30);
            this.menuButton.TabIndex = 1;
            this.menuButton.Text = "Меню";
            this.menuButton.UseVisualStyleBackColor = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(692, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Отчеты";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // reportsDataGridView
            // 
            this.reportsDataGridView.AllowUserToAddRows = false;
            this.reportsDataGridView.AllowUserToDeleteRows = false;
            this.reportsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reportsDataGridView.Location = new System.Drawing.Point(245, 100);
            this.reportsDataGridView.Name = "reportsDataGridView";
            this.reportsDataGridView.ReadOnly = true;
            this.reportsDataGridView.RowHeadersVisible = false;
            this.reportsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.reportsDataGridView.Size = new System.Drawing.Size(567, 389);
            this.reportsDataGridView.TabIndex = 1;
            // 
            // reportFromDatePicker
            // 
            this.reportFromDatePicker.Location = new System.Drawing.Point(24, 158);
            this.reportFromDatePicker.Name = "reportFromDatePicker";
            this.reportFromDatePicker.Size = new System.Drawing.Size(200, 20);
            this.reportFromDatePicker.TabIndex = 2;
            this.reportFromDatePicker.ValueChanged += new System.EventHandler(this.reportFromDatePicker_ValueChanged);
            // 
            // reportToDatePicker
            // 
            this.reportToDatePicker.Location = new System.Drawing.Point(24, 201);
            this.reportToDatePicker.Name = "reportToDatePicker";
            this.reportToDatePicker.Size = new System.Drawing.Size(200, 20);
            this.reportToDatePicker.TabIndex = 3;
            this.reportToDatePicker.ValueChanged += new System.EventHandler(this.reportToDatePicker_ValueChanged);
            // 
            // reportTypeComboBox
            // 
            this.reportTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportTypeComboBox.FormattingEnabled = true;
            this.reportTypeComboBox.Location = new System.Drawing.Point(24, 246);
            this.reportTypeComboBox.Name = "reportTypeComboBox";
            this.reportTypeComboBox.Size = new System.Drawing.Size(200, 21);
            this.reportTypeComboBox.TabIndex = 4;
            // 
            // generateReportButton
            // 
            this.generateReportButton.BackColor = System.Drawing.Color.Coral;
            this.generateReportButton.Location = new System.Drawing.Point(17, 286);
            this.generateReportButton.Name = "generateReportButton";
            this.generateReportButton.Size = new System.Drawing.Size(215, 30);
            this.generateReportButton.TabIndex = 5;
            this.generateReportButton.Text = "Сформировать отчет";
            this.generateReportButton.UseVisualStyleBackColor = false;
            this.generateReportButton.Click += new System.EventHandler(this.generateReportButton_Click);
            // 
            // printToWordButton
            // 
            this.printToWordButton.BackColor = System.Drawing.Color.Coral;
            this.printToWordButton.Location = new System.Drawing.Point(17, 326);
            this.printToWordButton.Name = "printToWordButton";
            this.printToWordButton.Size = new System.Drawing.Size(215, 30);
            this.printToWordButton.TabIndex = 6;
            this.printToWordButton.Text = "Печать в Word";
            this.printToWordButton.UseVisualStyleBackColor = false;
            this.printToWordButton.Click += new System.EventHandler(this.printToWordButton_Click);
            // 
            // printToExcelButton
            // 
            this.printToExcelButton.BackColor = System.Drawing.Color.Coral;
            this.printToExcelButton.Location = new System.Drawing.Point(17, 366);
            this.printToExcelButton.Name = "printToExcelButton";
            this.printToExcelButton.Size = new System.Drawing.Size(215, 30);
            this.printToExcelButton.TabIndex = 7;
            this.printToExcelButton.Text = "Печать в Excel";
            this.printToExcelButton.UseVisualStyleBackColor = false;
            this.printToExcelButton.Click += new System.EventHandler(this.printToExcelButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Период с:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Период по:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Тип отчета:";
            // 
            // totalsLabel
            // 
            this.totalsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalsLabel.AutoSize = true;
            this.totalsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.totalsLabel.Location = new System.Drawing.Point(314, 450);
            this.totalsLabel.Name = "totalsLabel";
            this.totalsLabel.Size = new System.Drawing.Size(0, 15);
            this.totalsLabel.TabIndex = 11;
            // 
            // ReportsForm
            // 
            this.ClientSize = new System.Drawing.Size(824, 501);
            this.Controls.Add(this.totalsLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.printToExcelButton);
            this.Controls.Add(this.printToWordButton);
            this.Controls.Add(this.generateReportButton);
            this.Controls.Add(this.reportTypeComboBox);
            this.Controls.Add(this.reportToDatePicker);
            this.Controls.Add(this.reportFromDatePicker);
            this.Controls.Add(this.reportsDataGridView);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(840, 540);
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчеты";
            this.Load += new System.EventHandler(this.ReportsForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reportsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}