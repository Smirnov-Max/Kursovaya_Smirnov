namespace Smirnov_kursovaya.secondForm
{
    partial class ViewOrderForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.DataGridView ordersDataGridView;
        private System.Windows.Forms.TextBox searchOrderTextBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Panel orderDetailsPanel;
        private System.Windows.Forms.Label orderNumberLabel;
        private System.Windows.Forms.Label orderDateLabel;
        private System.Windows.Forms.Label completionDateLabel;
        private System.Windows.Forms.Label clientNameLabel;
        private System.Windows.Forms.Label clientPhoneLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label subtotalLabel;
        private System.Windows.Forms.Label discountLabel;
        private System.Windows.Forms.Label totalLabel;
        private System.Windows.Forms.DataGridView orderItemsDataGridView;
        private System.Windows.Forms.ComboBox statusComboBox;
        private System.Windows.Forms.Button applyDiscountButton;
        private System.Windows.Forms.Button updateStatusButton;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox discountComboBox;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ordersDataGridView = new System.Windows.Forms.DataGridView();
            this.searchOrderTextBox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.refreshButton = new System.Windows.Forms.Button();
            this.orderDetailsPanel = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.printButton = new System.Windows.Forms.Button();
            this.updateStatusButton = new System.Windows.Forms.Button();
            this.applyDiscountButton = new System.Windows.Forms.Button();
            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.discountComboBox = new System.Windows.Forms.ComboBox();
            this.orderItemsDataGridView = new System.Windows.Forms.DataGridView();
            this.totalLabel = new System.Windows.Forms.Label();
            this.discountLabel = new System.Windows.Forms.Label();
            this.subtotalLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.clientPhoneLabel = new System.Windows.Forms.Label();
            this.clientNameLabel = new System.Windows.Forms.Label();
            this.completionDateLabel = new System.Windows.Forms.Label();
            this.orderDateLabel = new System.Windows.Forms.Label();
            this.orderNumberLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).BeginInit();
            this.orderDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orderItemsDataGridView)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(884, 60);
            this.panel1.TabIndex = 0;
            // 
            // menuButton
            // 
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.Location = new System.Drawing.Point(772, 15);
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
            this.label1.Size = new System.Drawing.Size(752, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Просмотр заказов";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ordersDataGridView
            // 
            this.ordersDataGridView.AllowUserToAddRows = false;
            this.ordersDataGridView.AllowUserToDeleteRows = false;
            this.ordersDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ordersDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ordersDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ordersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ordersDataGridView.Location = new System.Drawing.Point(12, 90);
            this.ordersDataGridView.Name = "ordersDataGridView";
            this.ordersDataGridView.ReadOnly = true;
            this.ordersDataGridView.RowHeadersVisible = false;
            this.ordersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ordersDataGridView.Size = new System.Drawing.Size(860, 200);
            this.ordersDataGridView.TabIndex = 1;
            this.ordersDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OrdersDataGridView_CellClick);
            // 
            // searchOrderTextBox
            // 
            this.searchOrderTextBox.Location = new System.Drawing.Point(60, 65);
            this.searchOrderTextBox.Name = "searchOrderTextBox";
            this.searchOrderTextBox.Size = new System.Drawing.Size(200, 20);
            this.searchOrderTextBox.TabIndex = 2;
            this.searchOrderTextBox.TextChanged += new System.EventHandler(this.searchOrderTextBox_TextChanged);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(12, 68);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(42, 13);
            this.searchLabel.TabIndex = 3;
            this.searchLabel.Text = "Поиск:";
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.Coral;
            this.refreshButton.Location = new System.Drawing.Point(266, 63);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(84, 23);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "Обновить";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // orderDetailsPanel
            // 
            this.orderDetailsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orderDetailsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.orderDetailsPanel.Controls.Add(this.label11);
            this.orderDetailsPanel.Controls.Add(this.label10);
            this.orderDetailsPanel.Controls.Add(this.label9);
            this.orderDetailsPanel.Controls.Add(this.label8);
            this.orderDetailsPanel.Controls.Add(this.label7);
            this.orderDetailsPanel.Controls.Add(this.label6);
            this.orderDetailsPanel.Controls.Add(this.label5);
            this.orderDetailsPanel.Controls.Add(this.label4);
            this.orderDetailsPanel.Controls.Add(this.label3);
            this.orderDetailsPanel.Controls.Add(this.label2);
            this.orderDetailsPanel.Controls.Add(this.printButton);
            this.orderDetailsPanel.Controls.Add(this.updateStatusButton);
            this.orderDetailsPanel.Controls.Add(this.applyDiscountButton);
            this.orderDetailsPanel.Controls.Add(this.statusComboBox);
            this.orderDetailsPanel.Controls.Add(this.discountComboBox);
            this.orderDetailsPanel.Controls.Add(this.orderItemsDataGridView);
            this.orderDetailsPanel.Controls.Add(this.totalLabel);
            this.orderDetailsPanel.Controls.Add(this.discountLabel);
            this.orderDetailsPanel.Controls.Add(this.subtotalLabel);
            this.orderDetailsPanel.Controls.Add(this.statusLabel);
            this.orderDetailsPanel.Controls.Add(this.clientPhoneLabel);
            this.orderDetailsPanel.Controls.Add(this.clientNameLabel);
            this.orderDetailsPanel.Controls.Add(this.completionDateLabel);
            this.orderDetailsPanel.Controls.Add(this.orderDateLabel);
            this.orderDetailsPanel.Controls.Add(this.orderNumberLabel);
            this.orderDetailsPanel.Location = new System.Drawing.Point(12, 296);
            this.orderDetailsPanel.Name = "orderDetailsPanel";
            this.orderDetailsPanel.Size = new System.Drawing.Size(860, 270);
            this.orderDetailsPanel.TabIndex = 5;
            this.orderDetailsPanel.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(600, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Применить %:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(600, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 17);
            this.label10.TabIndex = 26;
            this.label10.Text = "ИТОГО:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(600, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Скидка:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(600, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 15);
            this.label8.TabIndex = 24;
            this.label8.Text = "Подытог:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Статус:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Телефон:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Клиент:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Дата выполнения:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Дата создания заказа:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Номер заказа: №";
            // 
            // printButton
            // 
            this.printButton.BackColor = System.Drawing.Color.Coral;
            this.printButton.Location = new System.Drawing.Point(10, 235);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(121, 30);
            this.printButton.TabIndex = 16;
            this.printButton.Text = "Печать чека";
            this.printButton.UseVisualStyleBackColor = false;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // updateStatusButton
            // 
            this.updateStatusButton.BackColor = System.Drawing.Color.Coral;
            this.updateStatusButton.Location = new System.Drawing.Point(680, 205);
            this.updateStatusButton.Name = "updateStatusButton";
            this.updateStatusButton.Size = new System.Drawing.Size(131, 25);
            this.updateStatusButton.TabIndex = 15;
            this.updateStatusButton.Text = "Обновить статус";
            this.updateStatusButton.UseVisualStyleBackColor = false;
            this.updateStatusButton.Click += new System.EventHandler(this.updateStatusButton_Click);
            // 
            // applyDiscountButton
            // 
            this.applyDiscountButton.BackColor = System.Drawing.Color.Coral;
            this.applyDiscountButton.Location = new System.Drawing.Point(770, 169);
            this.applyDiscountButton.Name = "applyDiscountButton";
            this.applyDiscountButton.Size = new System.Drawing.Size(85, 25);
            this.applyDiscountButton.TabIndex = 14;
            this.applyDiscountButton.Text = "Применить";
            this.applyDiscountButton.UseVisualStyleBackColor = false;
            this.applyDiscountButton.Click += new System.EventHandler(this.applyDiscountButton_Click);
            // 
            // statusComboBox
            // 
            this.statusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statusComboBox.FormattingEnabled = true;
            this.statusComboBox.Location = new System.Drawing.Point(683, 115);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new System.Drawing.Size(170, 21);
            this.statusComboBox.TabIndex = 13;
            // 
            // discountComboBox
            // 
            this.discountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.discountComboBox.FormattingEnabled = true;
            this.discountComboBox.Items.AddRange(new object[] {
            "0%",
            "5%",
            "10%",
            "15%",
            "20%"});
            this.discountComboBox.Location = new System.Drawing.Point(680, 171);
            this.discountComboBox.Name = "discountComboBox";
            this.discountComboBox.Size = new System.Drawing.Size(80, 21);
            this.discountComboBox.TabIndex = 12;
            // 
            // orderItemsDataGridView
            // 
            this.orderItemsDataGridView.AllowUserToAddRows = false;
            this.orderItemsDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.orderItemsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.orderItemsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.orderItemsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.orderItemsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.orderItemsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.orderItemsDataGridView.Location = new System.Drawing.Point(10, 130);
            this.orderItemsDataGridView.Name = "orderItemsDataGridView";
            this.orderItemsDataGridView.ReadOnly = true;
            this.orderItemsDataGridView.RowHeadersVisible = false;
            this.orderItemsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.orderItemsDataGridView.Size = new System.Drawing.Size(580, 100);
            this.orderItemsDataGridView.TabIndex = 10;
            // 
            // totalLabel
            // 
            this.totalLabel.AutoSize = true;
            this.totalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.totalLabel.Location = new System.Drawing.Point(680, 143);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(59, 20);
            this.totalLabel.TabIndex = 9;
            this.totalLabel.Text = "0.00 ₽";
            // 
            // discountLabel
            // 
            this.discountLabel.AutoSize = true;
            this.discountLabel.Location = new System.Drawing.Point(680, 90);
            this.discountLabel.Name = "discountLabel";
            this.discountLabel.Size = new System.Drawing.Size(22, 13);
            this.discountLabel.TabIndex = 8;
            this.discountLabel.Text = "0 ₽";
            // 
            // subtotalLabel
            // 
            this.subtotalLabel.AutoSize = true;
            this.subtotalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.subtotalLabel.Location = new System.Drawing.Point(680, 60);
            this.subtotalLabel.Name = "subtotalLabel";
            this.subtotalLabel.Size = new System.Drawing.Size(47, 15);
            this.subtotalLabel.TabIndex = 7;
            this.subtotalLabel.Text = "0.00 ₽";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(140, 110);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 13);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "label7";
            // 
            // clientPhoneLabel
            // 
            this.clientPhoneLabel.AutoSize = true;
            this.clientPhoneLabel.Location = new System.Drawing.Point(140, 90);
            this.clientPhoneLabel.Name = "clientPhoneLabel";
            this.clientPhoneLabel.Size = new System.Drawing.Size(35, 13);
            this.clientPhoneLabel.TabIndex = 5;
            this.clientPhoneLabel.Text = "label6";
            // 
            // clientNameLabel
            // 
            this.clientNameLabel.AutoSize = true;
            this.clientNameLabel.Location = new System.Drawing.Point(140, 70);
            this.clientNameLabel.Name = "clientNameLabel";
            this.clientNameLabel.Size = new System.Drawing.Size(35, 13);
            this.clientNameLabel.TabIndex = 4;
            this.clientNameLabel.Text = "label5";
            // 
            // completionDateLabel
            // 
            this.completionDateLabel.AutoSize = true;
            this.completionDateLabel.Location = new System.Drawing.Point(140, 50);
            this.completionDateLabel.Name = "completionDateLabel";
            this.completionDateLabel.Size = new System.Drawing.Size(35, 13);
            this.completionDateLabel.TabIndex = 3;
            this.completionDateLabel.Text = "label4";
            // 
            // orderDateLabel
            // 
            this.orderDateLabel.AutoSize = true;
            this.orderDateLabel.Location = new System.Drawing.Point(140, 30);
            this.orderDateLabel.Name = "orderDateLabel";
            this.orderDateLabel.Size = new System.Drawing.Size(35, 13);
            this.orderDateLabel.TabIndex = 2;
            this.orderDateLabel.Text = "label3";
            // 
            // orderNumberLabel
            // 
            this.orderNumberLabel.AutoSize = true;
            this.orderNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.orderNumberLabel.Location = new System.Drawing.Point(140, 10);
            this.orderNumberLabel.Name = "orderNumberLabel";
            this.orderNumberLabel.Size = new System.Drawing.Size(52, 17);
            this.orderNumberLabel.TabIndex = 1;
            this.orderNumberLabel.Text = "label2";
            // 
            // ViewOrderForm
            // 
            this.ClientSize = new System.Drawing.Size(884, 571);
            this.Controls.Add(this.orderDetailsPanel);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchOrderTextBox);
            this.Controls.Add(this.ordersDataGridView);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(900, 610);
            this.Name = "ViewOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр заказов";
            this.Load += new System.EventHandler(this.ViewOrderForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).EndInit();
            this.orderDetailsPanel.ResumeLayout(false);
            this.orderDetailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orderItemsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}