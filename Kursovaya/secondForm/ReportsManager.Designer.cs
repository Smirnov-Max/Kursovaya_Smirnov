namespace Smirnov_kursovaya.secondForm
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===== Шапка =====
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;

        // ===== Панель фильтров =====
        private System.Windows.Forms.Panel filtersPanel;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.DateTimePicker reportFromDatePicker;
        private System.Windows.Forms.DateTimePicker reportToDatePicker;
        private System.Windows.Forms.Button presetWeekButton;
        private System.Windows.Forms.Button presetMonthButton;
        private System.Windows.Forms.Button presetQuarterButton;
        private System.Windows.Forms.Button presetYearButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button pdfButton;

        // ===== Полоса KPI (4 карточки) =====
        private System.Windows.Forms.Panel kpiPanel;
        private System.Windows.Forms.Panel kpiCard1;
        private System.Windows.Forms.Label kpi1Title;
        private System.Windows.Forms.Label kpi1Value;
        private System.Windows.Forms.Label kpi1Delta;
        private System.Windows.Forms.Panel kpiCard2;
        private System.Windows.Forms.Label kpi2Title;
        private System.Windows.Forms.Label kpi2Value;
        private System.Windows.Forms.Label kpi2Delta;
        private System.Windows.Forms.Panel kpiCard3;
        private System.Windows.Forms.Label kpi3Title;
        private System.Windows.Forms.Label kpi3Value;
        private System.Windows.Forms.Label kpi3Delta;
        private System.Windows.Forms.Panel kpiCard4;
        private System.Windows.Forms.Label kpi4Title;
        private System.Windows.Forms.Label kpi4Value;
        private System.Windows.Forms.Label kpi4Delta;

        // ===== Графики и таблица топ-клиентов =====
        private System.Windows.Forms.TableLayoutPanel chartsLayout;
        private System.Windows.Forms.Panel revenueChartPanel;
        private System.Windows.Forms.Panel topProductsChartPanel;
        private System.Windows.Forms.Panel statusPiePanel;
        private System.Windows.Forms.Panel topClientsPanel;
        private System.Windows.Forms.Label topClientsTitleLabel;
        private System.Windows.Forms.DataGridView topClientsGridView;

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
            this.filtersPanel = new System.Windows.Forms.Panel();
            this.fromLabel = new System.Windows.Forms.Label();
            this.reportFromDatePicker = new System.Windows.Forms.DateTimePicker();
            this.toLabel = new System.Windows.Forms.Label();
            this.reportToDatePicker = new System.Windows.Forms.DateTimePicker();
            this.presetWeekButton = new System.Windows.Forms.Button();
            this.presetMonthButton = new System.Windows.Forms.Button();
            this.presetQuarterButton = new System.Windows.Forms.Button();
            this.presetYearButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.pdfButton = new System.Windows.Forms.Button();
            this.kpiPanel = new System.Windows.Forms.Panel();
            this.kpiCard1 = new System.Windows.Forms.Panel();
            this.kpi1Delta = new System.Windows.Forms.Label();
            this.kpi1Value = new System.Windows.Forms.Label();
            this.kpi1Title = new System.Windows.Forms.Label();
            this.kpiCard2 = new System.Windows.Forms.Panel();
            this.kpi2Delta = new System.Windows.Forms.Label();
            this.kpi2Value = new System.Windows.Forms.Label();
            this.kpi2Title = new System.Windows.Forms.Label();
            this.kpiCard3 = new System.Windows.Forms.Panel();
            this.kpi3Delta = new System.Windows.Forms.Label();
            this.kpi3Value = new System.Windows.Forms.Label();
            this.kpi3Title = new System.Windows.Forms.Label();
            this.kpiCard4 = new System.Windows.Forms.Panel();
            this.kpi4Delta = new System.Windows.Forms.Label();
            this.kpi4Value = new System.Windows.Forms.Label();
            this.kpi4Title = new System.Windows.Forms.Label();
            this.chartsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.revenueChartPanel = new System.Windows.Forms.Panel();
            this.statusPiePanel = new System.Windows.Forms.Panel();
            this.topProductsChartPanel = new System.Windows.Forms.Panel();
            this.topClientsPanel = new System.Windows.Forms.Panel();
            this.topClientsGridView = new System.Windows.Forms.DataGridView();
            this.topClientsTitleLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.filtersPanel.SuspendLayout();
            this.kpiPanel.SuspendLayout();
            this.kpiCard1.SuspendLayout();
            this.kpiCard2.SuspendLayout();
            this.kpiCard3.SuspendLayout();
            this.kpiCard4.SuspendLayout();
            this.chartsLayout.SuspendLayout();
            this.topClientsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topClientsGridView)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(1011, 55);
            this.panel1.TabIndex = 0;
            // 
            // menuButton
            // 
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuButton.Location = new System.Drawing.Point(912, 15);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(86, 28);
            this.menuButton.TabIndex = 1;
            this.menuButton.Text = "Меню";
            this.menuButton.UseVisualStyleBackColor = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(893, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Аналитика и отчёты";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // filtersPanel
            // 
            this.filtersPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(238)))), ((int)(((byte)(228)))));
            this.filtersPanel.Controls.Add(this.fromLabel);
            this.filtersPanel.Controls.Add(this.reportFromDatePicker);
            this.filtersPanel.Controls.Add(this.toLabel);
            this.filtersPanel.Controls.Add(this.reportToDatePicker);
            this.filtersPanel.Controls.Add(this.presetWeekButton);
            this.filtersPanel.Controls.Add(this.presetMonthButton);
            this.filtersPanel.Controls.Add(this.presetQuarterButton);
            this.filtersPanel.Controls.Add(this.presetYearButton);
            this.filtersPanel.Controls.Add(this.refreshButton);
            this.filtersPanel.Controls.Add(this.pdfButton);
            this.filtersPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filtersPanel.Location = new System.Drawing.Point(0, 55);
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.filtersPanel.Size = new System.Drawing.Size(1011, 55);
            this.filtersPanel.TabIndex = 1;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.fromLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.fromLabel.Location = new System.Drawing.Point(14, 9);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(19, 17);
            this.fromLabel.TabIndex = 0;
            this.fromLabel.Text = "С:";
            // 
            // reportFromDatePicker
            // 
            this.reportFromDatePicker.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.reportFromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.reportFromDatePicker.Location = new System.Drawing.Point(14, 28);
            this.reportFromDatePicker.Name = "reportFromDatePicker";
            this.reportFromDatePicker.Size = new System.Drawing.Size(103, 24);
            this.reportFromDatePicker.TabIndex = 1;
            this.reportFromDatePicker.ValueChanged += new System.EventHandler(this.reportFromDatePicker_ValueChanged);
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.toLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.toLabel.Location = new System.Drawing.Point(125, 9);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(28, 17);
            this.toLabel.TabIndex = 2;
            this.toLabel.Text = "По:";
            // 
            // reportToDatePicker
            // 
            this.reportToDatePicker.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.reportToDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.reportToDatePicker.Location = new System.Drawing.Point(125, 28);
            this.reportToDatePicker.Name = "reportToDatePicker";
            this.reportToDatePicker.Size = new System.Drawing.Size(103, 24);
            this.reportToDatePicker.TabIndex = 3;
            this.reportToDatePicker.ValueChanged += new System.EventHandler(this.reportToDatePicker_ValueChanged);
            // 
            // presetWeekButton
            // 
            this.presetWeekButton.BackColor = System.Drawing.Color.White;
            this.presetWeekButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetWeekButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.presetWeekButton.Location = new System.Drawing.Point(245, 26);
            this.presetWeekButton.Name = "presetWeekButton";
            this.presetWeekButton.Size = new System.Drawing.Size(67, 23);
            this.presetWeekButton.TabIndex = 4;
            this.presetWeekButton.Text = "Неделя";
            this.presetWeekButton.UseVisualStyleBackColor = false;
            this.presetWeekButton.Click += new System.EventHandler(this.presetWeekButton_Click);
            // 
            // presetMonthButton
            // 
            this.presetMonthButton.BackColor = System.Drawing.Color.White;
            this.presetMonthButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetMonthButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.presetMonthButton.Location = new System.Drawing.Point(314, 26);
            this.presetMonthButton.Name = "presetMonthButton";
            this.presetMonthButton.Size = new System.Drawing.Size(67, 23);
            this.presetMonthButton.TabIndex = 5;
            this.presetMonthButton.Text = "Месяц";
            this.presetMonthButton.UseVisualStyleBackColor = false;
            this.presetMonthButton.Click += new System.EventHandler(this.presetMonthButton_Click);
            // 
            // presetQuarterButton
            // 
            this.presetQuarterButton.BackColor = System.Drawing.Color.White;
            this.presetQuarterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetQuarterButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.presetQuarterButton.Location = new System.Drawing.Point(382, 26);
            this.presetQuarterButton.Name = "presetQuarterButton";
            this.presetQuarterButton.Size = new System.Drawing.Size(67, 23);
            this.presetQuarterButton.TabIndex = 6;
            this.presetQuarterButton.Text = "Квартал";
            this.presetQuarterButton.UseVisualStyleBackColor = false;
            this.presetQuarterButton.Click += new System.EventHandler(this.presetQuarterButton_Click);
            // 
            // presetYearButton
            // 
            this.presetYearButton.BackColor = System.Drawing.Color.White;
            this.presetYearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetYearButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.presetYearButton.Location = new System.Drawing.Point(451, 26);
            this.presetYearButton.Name = "presetYearButton";
            this.presetYearButton.Size = new System.Drawing.Size(67, 23);
            this.presetYearButton.TabIndex = 7;
            this.presetYearButton.Text = "Год";
            this.presetYearButton.UseVisualStyleBackColor = false;
            this.presetYearButton.Click += new System.EventHandler(this.presetYearButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.BackColor = System.Drawing.Color.Coral;
            this.refreshButton.FlatAppearance.BorderColor = System.Drawing.Color.Coral;
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.refreshButton.ForeColor = System.Drawing.Color.Black;
            this.refreshButton.Location = new System.Drawing.Point(771, 23);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(106, 26);
            this.refreshButton.TabIndex = 8;
            this.refreshButton.Text = "Обновить";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // pdfButton
            // 
            this.pdfButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pdfButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.pdfButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.pdfButton.FlatAppearance.BorderSize = 0;
            this.pdfButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pdfButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pdfButton.ForeColor = System.Drawing.Color.Black;
            this.pdfButton.Location = new System.Drawing.Point(881, 23);
            this.pdfButton.Name = "pdfButton";
            this.pdfButton.Size = new System.Drawing.Size(120, 26);
            this.pdfButton.TabIndex = 9;
            this.pdfButton.Text = "Печать PDF";
            this.pdfButton.UseVisualStyleBackColor = false;
            this.pdfButton.Click += new System.EventHandler(this.pdfButton_Click);
            // 
            // kpiPanel
            // 
            this.kpiPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.kpiPanel.Controls.Add(this.kpiCard1);
            this.kpiPanel.Controls.Add(this.kpiCard2);
            this.kpiPanel.Controls.Add(this.kpiCard3);
            this.kpiPanel.Controls.Add(this.kpiCard4);
            this.kpiPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.kpiPanel.Location = new System.Drawing.Point(0, 110);
            this.kpiPanel.Name = "kpiPanel";
            this.kpiPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 3);
            this.kpiPanel.Size = new System.Drawing.Size(1011, 95);
            this.kpiPanel.TabIndex = 2;
            // 
            // kpiCard1
            // 
            this.kpiCard1.BackColor = System.Drawing.Color.White;
            this.kpiCard1.Controls.Add(this.kpi1Delta);
            this.kpiCard1.Controls.Add(this.kpi1Value);
            this.kpiCard1.Controls.Add(this.kpi1Title);
            this.kpiCard1.Location = new System.Drawing.Point(10, 10);
            this.kpiCard1.Name = "kpiCard1";
            this.kpiCard1.Size = new System.Drawing.Size(242, 80);
            this.kpiCard1.TabIndex = 0;
            // 
            // kpi1Delta
            // 
            this.kpi1Delta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.kpi1Delta.ForeColor = System.Drawing.Color.Gray;
            this.kpi1Delta.Location = new System.Drawing.Point(12, 57);
            this.kpi1Delta.Name = "kpi1Delta";
            this.kpi1Delta.Size = new System.Drawing.Size(221, 16);
            this.kpi1Delta.TabIndex = 2;
            this.kpi1Delta.Text = "—";
            // 
            // kpi1Value
            // 
            this.kpi1Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.kpi1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.kpi1Value.Location = new System.Drawing.Point(10, 26);
            this.kpi1Value.Name = "kpi1Value";
            this.kpi1Value.Size = new System.Drawing.Size(223, 29);
            this.kpi1Value.TabIndex = 1;
            this.kpi1Value.Text = "0,00 ₽";
            // 
            // kpi1Title
            // 
            this.kpi1Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kpi1Title.ForeColor = System.Drawing.Color.Gray;
            this.kpi1Title.Location = new System.Drawing.Point(12, 9);
            this.kpi1Title.Name = "kpi1Title";
            this.kpi1Title.Size = new System.Drawing.Size(221, 16);
            this.kpi1Title.TabIndex = 0;
            this.kpi1Title.Text = "Выручка";
            // 
            // kpiCard2
            // 
            this.kpiCard2.BackColor = System.Drawing.Color.White;
            this.kpiCard2.Controls.Add(this.kpi2Delta);
            this.kpiCard2.Controls.Add(this.kpi2Value);
            this.kpiCard2.Controls.Add(this.kpi2Title);
            this.kpiCard2.Location = new System.Drawing.Point(257, 10);
            this.kpiCard2.Name = "kpiCard2";
            this.kpiCard2.Size = new System.Drawing.Size(242, 80);
            this.kpiCard2.TabIndex = 1;
            // 
            // kpi2Delta
            // 
            this.kpi2Delta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.kpi2Delta.ForeColor = System.Drawing.Color.Gray;
            this.kpi2Delta.Location = new System.Drawing.Point(12, 57);
            this.kpi2Delta.Name = "kpi2Delta";
            this.kpi2Delta.Size = new System.Drawing.Size(221, 16);
            this.kpi2Delta.TabIndex = 2;
            this.kpi2Delta.Text = "—";
            // 
            // kpi2Value
            // 
            this.kpi2Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.kpi2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.kpi2Value.Location = new System.Drawing.Point(10, 26);
            this.kpi2Value.Name = "kpi2Value";
            this.kpi2Value.Size = new System.Drawing.Size(223, 29);
            this.kpi2Value.TabIndex = 1;
            this.kpi2Value.Text = "0";
            // 
            // kpi2Title
            // 
            this.kpi2Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kpi2Title.ForeColor = System.Drawing.Color.Gray;
            this.kpi2Title.Location = new System.Drawing.Point(12, 9);
            this.kpi2Title.Name = "kpi2Title";
            this.kpi2Title.Size = new System.Drawing.Size(221, 16);
            this.kpi2Title.TabIndex = 0;
            this.kpi2Title.Text = "Заказы";
            // 
            // kpiCard3
            // 
            this.kpiCard3.BackColor = System.Drawing.Color.White;
            this.kpiCard3.Controls.Add(this.kpi3Delta);
            this.kpiCard3.Controls.Add(this.kpi3Value);
            this.kpiCard3.Controls.Add(this.kpi3Title);
            this.kpiCard3.Location = new System.Drawing.Point(504, 10);
            this.kpiCard3.Name = "kpiCard3";
            this.kpiCard3.Size = new System.Drawing.Size(242, 80);
            this.kpiCard3.TabIndex = 2;
            // 
            // kpi3Delta
            // 
            this.kpi3Delta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.kpi3Delta.ForeColor = System.Drawing.Color.Gray;
            this.kpi3Delta.Location = new System.Drawing.Point(12, 57);
            this.kpi3Delta.Name = "kpi3Delta";
            this.kpi3Delta.Size = new System.Drawing.Size(221, 16);
            this.kpi3Delta.TabIndex = 2;
            this.kpi3Delta.Text = "—";
            // 
            // kpi3Value
            // 
            this.kpi3Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.kpi3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.kpi3Value.Location = new System.Drawing.Point(10, 26);
            this.kpi3Value.Name = "kpi3Value";
            this.kpi3Value.Size = new System.Drawing.Size(223, 29);
            this.kpi3Value.TabIndex = 1;
            this.kpi3Value.Text = "0,00 ₽";
            // 
            // kpi3Title
            // 
            this.kpi3Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kpi3Title.ForeColor = System.Drawing.Color.Gray;
            this.kpi3Title.Location = new System.Drawing.Point(12, 9);
            this.kpi3Title.Name = "kpi3Title";
            this.kpi3Title.Size = new System.Drawing.Size(221, 16);
            this.kpi3Title.TabIndex = 0;
            this.kpi3Title.Text = "Средний чек";
            // 
            // kpiCard4
            // 
            this.kpiCard4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kpiCard4.BackColor = System.Drawing.Color.White;
            this.kpiCard4.Controls.Add(this.kpi4Delta);
            this.kpiCard4.Controls.Add(this.kpi4Value);
            this.kpiCard4.Controls.Add(this.kpi4Title);
            this.kpiCard4.Location = new System.Drawing.Point(751, 10);
            this.kpiCard4.Name = "kpiCard4";
            this.kpiCard4.Size = new System.Drawing.Size(250, 80);
            this.kpiCard4.TabIndex = 3;
            // 
            // kpi4Delta
            // 
            this.kpi4Delta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.kpi4Delta.ForeColor = System.Drawing.Color.Gray;
            this.kpi4Delta.Location = new System.Drawing.Point(12, 57);
            this.kpi4Delta.Name = "kpi4Delta";
            this.kpi4Delta.Size = new System.Drawing.Size(230, 16);
            this.kpi4Delta.TabIndex = 2;
            this.kpi4Delta.Text = "—";
            // 
            // kpi4Value
            // 
            this.kpi4Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.kpi4Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.kpi4Value.Location = new System.Drawing.Point(10, 26);
            this.kpi4Value.Name = "kpi4Value";
            this.kpi4Value.Size = new System.Drawing.Size(231, 29);
            this.kpi4Value.TabIndex = 1;
            this.kpi4Value.Text = "0";
            // 
            // kpi4Title
            // 
            this.kpi4Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kpi4Title.ForeColor = System.Drawing.Color.Gray;
            this.kpi4Title.Location = new System.Drawing.Point(12, 9);
            this.kpi4Title.Name = "kpi4Title";
            this.kpi4Title.Size = new System.Drawing.Size(230, 16);
            this.kpi4Title.TabIndex = 0;
            this.kpi4Title.Text = "Активные клиенты";
            // 
            // chartsLayout
            // 
            this.chartsLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.chartsLayout.ColumnCount = 2;
            this.chartsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.chartsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.chartsLayout.Controls.Add(this.revenueChartPanel, 0, 0);
            this.chartsLayout.Controls.Add(this.statusPiePanel, 1, 0);
            this.chartsLayout.Controls.Add(this.topProductsChartPanel, 0, 1);
            this.chartsLayout.Controls.Add(this.topClientsPanel, 1, 1);
            this.chartsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartsLayout.Location = new System.Drawing.Point(0, 205);
            this.chartsLayout.Name = "chartsLayout";
            this.chartsLayout.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.chartsLayout.RowCount = 2;
            this.chartsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.chartsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.chartsLayout.Size = new System.Drawing.Size(1011, 436);
            this.chartsLayout.TabIndex = 3;
            // 
            // revenueChartPanel
            // 
            this.revenueChartPanel.BackColor = System.Drawing.Color.White;
            this.revenueChartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.revenueChartPanel.Location = new System.Drawing.Point(13, 6);
            this.revenueChartPanel.Name = "revenueChartPanel";
            this.revenueChartPanel.Size = new System.Drawing.Size(608, 226);
            this.revenueChartPanel.TabIndex = 0;
            // 
            // statusPiePanel
            // 
            this.statusPiePanel.BackColor = System.Drawing.Color.White;
            this.statusPiePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusPiePanel.Location = new System.Drawing.Point(627, 6);
            this.statusPiePanel.Name = "statusPiePanel";
            this.statusPiePanel.Size = new System.Drawing.Size(371, 226);
            this.statusPiePanel.TabIndex = 2;
            // 
            // topProductsChartPanel
            // 
            this.topProductsChartPanel.BackColor = System.Drawing.Color.White;
            this.topProductsChartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topProductsChartPanel.Location = new System.Drawing.Point(13, 238);
            this.topProductsChartPanel.Name = "topProductsChartPanel";
            this.topProductsChartPanel.Size = new System.Drawing.Size(608, 185);
            this.topProductsChartPanel.TabIndex = 1;
            // 
            // topClientsPanel
            // 
            this.topClientsPanel.BackColor = System.Drawing.Color.White;
            this.topClientsPanel.Controls.Add(this.topClientsGridView);
            this.topClientsPanel.Controls.Add(this.topClientsTitleLabel);
            this.topClientsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topClientsPanel.Location = new System.Drawing.Point(627, 238);
            this.topClientsPanel.Name = "topClientsPanel";
            this.topClientsPanel.Size = new System.Drawing.Size(371, 185);
            this.topClientsPanel.TabIndex = 3;
            // 
            // topClientsGridView
            // 
            this.topClientsGridView.AllowUserToAddRows = false;
            this.topClientsGridView.AllowUserToDeleteRows = false;
            this.topClientsGridView.AllowUserToResizeColumns = false;
            this.topClientsGridView.AllowUserToResizeRows = false;
            this.topClientsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.topClientsGridView.BackgroundColor = System.Drawing.Color.White;
            this.topClientsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.topClientsGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.topClientsGridView.ColumnHeadersHeight = 30;
            this.topClientsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.topClientsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topClientsGridView.Location = new System.Drawing.Point(0, 28);
            this.topClientsGridView.Name = "topClientsGridView";
            this.topClientsGridView.ReadOnly = true;
            this.topClientsGridView.RowHeadersVisible = false;
            this.topClientsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.topClientsGridView.Size = new System.Drawing.Size(371, 157);
            this.topClientsGridView.TabIndex = 1;
            // 
            // topClientsTitleLabel
            // 
            this.topClientsTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topClientsTitleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.topClientsTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.topClientsTitleLabel.Location = new System.Drawing.Point(0, 0);
            this.topClientsTitleLabel.Name = "topClientsTitleLabel";
            this.topClientsTitleLabel.Padding = new System.Windows.Forms.Padding(12, 9, 10, 3);
            this.topClientsTitleLabel.Size = new System.Drawing.Size(371, 28);
            this.topClientsTitleLabel.TabIndex = 0;
            this.topClientsTitleLabel.Text = "Топ-5 клиентов по выручке";
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1011, 641);
            this.Controls.Add(this.chartsLayout);
            this.Controls.Add(this.kpiPanel);
            this.Controls.Add(this.filtersPanel);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(945, 629);
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Аналитика и отчёты";
            this.Load += new System.EventHandler(this.ReportsForm_Load);
            this.panel1.ResumeLayout(false);
            this.filtersPanel.ResumeLayout(false);
            this.filtersPanel.PerformLayout();
            this.kpiPanel.ResumeLayout(false);
            this.kpiCard1.ResumeLayout(false);
            this.kpiCard2.ResumeLayout(false);
            this.kpiCard3.ResumeLayout(false);
            this.kpiCard4.ResumeLayout(false);
            this.chartsLayout.ResumeLayout(false);
            this.topClientsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.topClientsGridView)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
