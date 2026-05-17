namespace Smirnov_kursovaya.secondForm
{
    partial class ViewOrderForm
    {
        private System.ComponentModel.IContainer components = null;

        // Шапка формы (растянута на всю ширину).
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;

        // Поиск заказа по номеру.
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchOrderTextBox;

        // Фильтр по статусам.
        private System.Windows.Forms.Label statusFilterLabel;
        private System.Windows.Forms.ComboBox statusFilterComboBox;

        // Легенда цветов статусов.
        private System.Windows.Forms.Panel legendPanel;
        private System.Windows.Forms.Label legendTitleLabel;
        private System.Windows.Forms.Panel legendGreenBox;
        private System.Windows.Forms.Label legendGreenLabel;
        private System.Windows.Forms.Panel legendYellowBox;
        private System.Windows.Forms.Label legendYellowLabel;
        private System.Windows.Forms.Panel legendOrangeBox;
        private System.Windows.Forms.Label legendOrangeLabel;
        private System.Windows.Forms.Panel legendRedBox;
        private System.Windows.Forms.Label legendRedLabel;

        // Таблица всех заказов.
        private System.Windows.Forms.DataGridView ordersDataGridView;

        // Нижняя панель действий — на главной форме осталась только «Подробнее о заказе».
        private System.Windows.Forms.Panel actionsPanel;
        private System.Windows.Forms.Button showDetailsButton;

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
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchOrderTextBox = new System.Windows.Forms.TextBox();
            this.statusFilterLabel = new System.Windows.Forms.Label();
            this.statusFilterComboBox = new System.Windows.Forms.ComboBox();
            this.legendPanel = new System.Windows.Forms.Panel();
            this.legendTitleLabel = new System.Windows.Forms.Label();
            this.legendGreenBox = new System.Windows.Forms.Panel();
            this.legendGreenLabel = new System.Windows.Forms.Label();
            this.legendYellowBox = new System.Windows.Forms.Panel();
            this.legendYellowLabel = new System.Windows.Forms.Label();
            this.legendOrangeBox = new System.Windows.Forms.Panel();
            this.legendOrangeLabel = new System.Windows.Forms.Label();
            this.legendRedBox = new System.Windows.Forms.Panel();
            this.legendRedLabel = new System.Windows.Forms.Label();
            this.ordersDataGridView = new System.Windows.Forms.DataGridView();
            this.actionsPanel = new System.Windows.Forms.Panel();
            this.showDetailsButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.legendPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).BeginInit();
            this.actionsPanel.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(943, 52);
            this.panel1.TabIndex = 0;
            // 
            // menuButton
            // 
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.menuButton.ForeColor = System.Drawing.Color.Black;
            this.menuButton.Location = new System.Drawing.Point(847, 13);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(86, 26);
            this.menuButton.TabIndex = 1;
            this.menuButton.Text = "Меню";
            this.menuButton.UseVisualStyleBackColor = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(830, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Учет заказов";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.searchLabel.Location = new System.Drawing.Point(10, 62);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(121, 16);
            this.searchLabel.TabIndex = 3;
            this.searchLabel.Text = "Поиск по номеру:";
            // 
            // searchOrderTextBox
            // 
            this.searchOrderTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.searchOrderTextBox.Location = new System.Drawing.Point(137, 59);
            this.searchOrderTextBox.Name = "searchOrderTextBox";
            this.searchOrderTextBox.Size = new System.Drawing.Size(241, 23);
            this.searchOrderTextBox.TabIndex = 2;
            this.searchOrderTextBox.TextChanged += new System.EventHandler(this.searchOrderTextBox_TextChanged);
            // 
            // statusFilterLabel
            // 
            this.statusFilterLabel.AutoSize = true;
            this.statusFilterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusFilterLabel.Location = new System.Drawing.Point(395, 62);
            this.statusFilterLabel.Name = "statusFilterLabel";
            this.statusFilterLabel.Size = new System.Drawing.Size(101, 16);
            this.statusFilterLabel.TabIndex = 6;
            this.statusFilterLabel.Text = "Фильтр статуса:";
            // 
            // statusFilterComboBox
            // 
            this.statusFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statusFilterComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.statusFilterComboBox.Location = new System.Drawing.Point(503, 58);
            this.statusFilterComboBox.Name = "statusFilterComboBox";
            this.statusFilterComboBox.Size = new System.Drawing.Size(210, 24);
            this.statusFilterComboBox.TabIndex = 7;
            this.statusFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.statusFilterComboBox_SelectedIndexChanged);
            // 
            // legendPanel
            // 
            this.legendPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.legendPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(238)))), ((int)(((byte)(228)))));
            this.legendPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendPanel.Controls.Add(this.legendTitleLabel);
            this.legendPanel.Controls.Add(this.legendGreenBox);
            this.legendPanel.Controls.Add(this.legendGreenLabel);
            this.legendPanel.Controls.Add(this.legendYellowBox);
            this.legendPanel.Controls.Add(this.legendYellowLabel);
            this.legendPanel.Controls.Add(this.legendOrangeBox);
            this.legendPanel.Controls.Add(this.legendOrangeLabel);
            this.legendPanel.Controls.Add(this.legendRedBox);
            this.legendPanel.Controls.Add(this.legendRedLabel);
            this.legendPanel.Location = new System.Drawing.Point(0, 519);
            this.legendPanel.Name = "legendPanel";
            this.legendPanel.Size = new System.Drawing.Size(943, 50);
            this.legendPanel.TabIndex = 8;
            // 
            // legendTitleLabel
            // 
            this.legendTitleLabel.AutoSize = true;
            this.legendTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.legendTitleLabel.Location = new System.Drawing.Point(10, 17);
            this.legendTitleLabel.Name = "legendTitleLabel";
            this.legendTitleLabel.Size = new System.Drawing.Size(112, 16);
            this.legendTitleLabel.TabIndex = 0;
            this.legendTitleLabel.Text = "Цвета статусов:";
            // 
            // legendGreenBox
            // 
            this.legendGreenBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(239)))), ((int)(((byte)(206)))));
            this.legendGreenBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendGreenBox.Location = new System.Drawing.Point(140, 17);
            this.legendGreenBox.Name = "legendGreenBox";
            this.legendGreenBox.Size = new System.Drawing.Size(18, 16);
            this.legendGreenBox.TabIndex = 1;
            // 
            // legendGreenLabel
            // 
            this.legendGreenLabel.AutoSize = true;
            this.legendGreenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.legendGreenLabel.Location = new System.Drawing.Point(163, 18);
            this.legendGreenLabel.Name = "legendGreenLabel";
            this.legendGreenLabel.Size = new System.Drawing.Size(74, 15);
            this.legendGreenLabel.TabIndex = 2;
            this.legendGreenLabel.Text = "Выполнен";
            // 
            // legendYellowBox
            // 
            this.legendYellowBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(176)))));
            this.legendYellowBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendYellowBox.Location = new System.Drawing.Point(263, 17);
            this.legendYellowBox.Name = "legendYellowBox";
            this.legendYellowBox.Size = new System.Drawing.Size(18, 16);
            this.legendYellowBox.TabIndex = 3;
            // 
            // legendYellowLabel
            // 
            this.legendYellowLabel.AutoSize = true;
            this.legendYellowLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.legendYellowLabel.Location = new System.Drawing.Point(286, 18);
            this.legendYellowLabel.Name = "legendYellowLabel";
            this.legendYellowLabel.Size = new System.Drawing.Size(90, 15);
            this.legendYellowLabel.TabIndex = 4;
            this.legendYellowLabel.Text = "Подтвержден";
            // 
            // legendOrangeBox
            // 
            this.legendOrangeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(196)))), ((int)(((byte)(137)))));
            this.legendOrangeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendOrangeBox.Location = new System.Drawing.Point(400, 17);
            this.legendOrangeBox.Name = "legendOrangeBox";
            this.legendOrangeBox.Size = new System.Drawing.Size(18, 16);
            this.legendOrangeBox.TabIndex = 5;
            // 
            // legendOrangeLabel
            // 
            this.legendOrangeLabel.AutoSize = true;
            this.legendOrangeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.legendOrangeLabel.Location = new System.Drawing.Point(423, 18);
            this.legendOrangeLabel.Name = "legendOrangeLabel";
            this.legendOrangeLabel.Size = new System.Drawing.Size(245, 15);
            this.legendOrangeLabel.TabIndex = 6;
            this.legendOrangeLabel.Text = "Подтвержден (за день до отмены)";
            // 
            // legendRedBox
            // 
            this.legendRedBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(206)))));
            this.legendRedBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendRedBox.Location = new System.Drawing.Point(700, 17);
            this.legendRedBox.Name = "legendRedBox";
            this.legendRedBox.Size = new System.Drawing.Size(18, 16);
            this.legendRedBox.TabIndex = 7;
            // 
            // legendRedLabel
            // 
            this.legendRedLabel.AutoSize = true;
            this.legendRedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.legendRedLabel.Location = new System.Drawing.Point(723, 18);
            this.legendRedLabel.Name = "legendRedLabel";
            this.legendRedLabel.Size = new System.Drawing.Size(70, 15);
            this.legendRedLabel.TabIndex = 8;
            this.legendRedLabel.Text = "Отменен";
            // 
            // ordersDataGridView
            // 
            this.ordersDataGridView.AllowUserToAddRows = false;
            this.ordersDataGridView.AllowUserToDeleteRows = false;
            this.ordersDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ordersDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ordersDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ordersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ordersDataGridView.Location = new System.Drawing.Point(10, 87);
            this.ordersDataGridView.Name = "ordersDataGridView";
            this.ordersDataGridView.ReadOnly = true;
            this.ordersDataGridView.RowHeadersVisible = false;
            this.ordersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ordersDataGridView.Size = new System.Drawing.Size(922, 425);
            this.ordersDataGridView.TabIndex = 1;
            this.ordersDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OrdersDataGridView_CellClick);
            // 
            // actionsPanel
            // 
            this.actionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(238)))), ((int)(((byte)(228)))));
            this.actionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionsPanel.Controls.Add(this.showDetailsButton);
            this.actionsPanel.Location = new System.Drawing.Point(0, 572);
            this.actionsPanel.Name = "actionsPanel";
            this.actionsPanel.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.actionsPanel.Size = new System.Drawing.Size(943, 52);
            this.actionsPanel.TabIndex = 5;
            // 
            // showDetailsButton
            // 
            this.showDetailsButton.BackColor = System.Drawing.Color.Coral;
            this.showDetailsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.showDetailsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showDetailsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.showDetailsButton.ForeColor = System.Drawing.Color.Black;
            this.showDetailsButton.Location = new System.Drawing.Point(10, 11);
            this.showDetailsButton.Name = "showDetailsButton";
            this.showDetailsButton.Size = new System.Drawing.Size(220, 29);
            this.showDetailsButton.TabIndex = 10;
            this.showDetailsButton.Text = "Подробнее о заказе";
            this.showDetailsButton.UseVisualStyleBackColor = false;
            this.showDetailsButton.Click += new System.EventHandler(this.showDetailsButton_Click);
            // 
            // ViewOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 624);
            this.Controls.Add(this.ordersDataGridView);
            this.Controls.Add(this.legendPanel);
            this.Controls.Add(this.actionsPanel);
            this.Controls.Add(this.statusFilterComboBox);
            this.Controls.Add(this.statusFilterLabel);
            this.Controls.Add(this.searchOrderTextBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(945, 612);
            this.Name = "ViewOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Учет заказов";
            this.Load += new System.EventHandler(this.ViewOrderForm_Load);
            this.panel1.ResumeLayout(false);
            this.legendPanel.ResumeLayout(false);
            this.legendPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).EndInit();
            this.actionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
