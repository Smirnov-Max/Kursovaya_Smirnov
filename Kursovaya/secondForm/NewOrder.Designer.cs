namespace Smirnov_kursovaya.secondForm
{
    partial class NewOrderForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;

        private System.Windows.Forms.Label orderNumberTitleLabel;
        private System.Windows.Forms.Label orderNumberValueLabel;

        // Блок «Клиент» — без выпадающего списка, работаем только через кнопку.
        private System.Windows.Forms.Label label2;             // "Клиент:"
        private System.Windows.Forms.Label clientInfoLabel;    // ФИО + телефон выбранного клиента
        private System.Windows.Forms.Button clientsButton;     // открывает форму «Клиенты» в режиме выбора

        private System.Windows.Forms.Label label3;             // "Дата заказа:"
        private System.Windows.Forms.Label orderDateLabel;
        private System.Windows.Forms.Label label4;             // "Дата исполнения:"
        private System.Windows.Forms.DateTimePicker completionDatePicker;

        private System.Windows.Forms.Label label5;             // "Товары:"
        private System.Windows.Forms.DataGridView productsDataGridView;
        private System.Windows.Forms.Button addToCartButton;

        private System.Windows.Forms.DataGridView cartDataGridView;
        private System.Windows.Forms.Button removeFromCartButton;
        private System.Windows.Forms.Button clearCartButton;

        private System.Windows.Forms.Label label7;             // "Корзина"
        private System.Windows.Forms.Label hintLabel;          // подсказка ↑/↓

        // Сводный блок «Итого» — отдельная панель внизу справа
        private System.Windows.Forms.Panel totalsPanel;
        private System.Windows.Forms.Label label8;             // "Подытог:"
        private System.Windows.Forms.Label subtotalLabel;
        private System.Windows.Forms.Label label9;             // "Скидка:"
        private System.Windows.Forms.Label discountAmountLabel;
        private System.Windows.Forms.Label label10;            // "ИТОГО:"
        private System.Windows.Forms.Label totalLabel;

        private System.Windows.Forms.Button createOrderButton; // "Оформить заказ"

        private System.Windows.Forms.TextBox searchProductsTextBox;
        private System.Windows.Forms.Label searchProductsLabel;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.orderNumberTitleLabel = new System.Windows.Forms.Label();
            this.orderNumberValueLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.clientInfoLabel = new System.Windows.Forms.Label();
            this.clientsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.orderDateLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.completionDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.productsDataGridView = new System.Windows.Forms.DataGridView();
            this.addToCartButton = new System.Windows.Forms.Button();
            this.cartDataGridView = new System.Windows.Forms.DataGridView();
            this.removeFromCartButton = new System.Windows.Forms.Button();
            this.clearCartButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.totalsPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.subtotalLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.discountAmountLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.totalLabel = new System.Windows.Forms.Label();
            this.createOrderButton = new System.Windows.Forms.Button();
            this.searchProductsTextBox = new System.Windows.Forms.TextBox();
            this.searchProductsLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cartDataGridView)).BeginInit();
            this.totalsPanel.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(891, 52);
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
            this.menuButton.Location = new System.Drawing.Point(795, 13);
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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(778, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Новый заказ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // orderNumberTitleLabel
            // 
            this.orderNumberTitleLabel.AutoSize = true;
            this.orderNumberTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.orderNumberTitleLabel.Location = new System.Drawing.Point(10, 65);
            this.orderNumberTitleLabel.Name = "orderNumberTitleLabel";
            this.orderNumberTitleLabel.Size = new System.Drawing.Size(126, 20);
            this.orderNumberTitleLabel.TabIndex = 30;
            this.orderNumberTitleLabel.Text = "Номер заказа";
            // 
            // orderNumberValueLabel
            // 
            this.orderNumberValueLabel.AutoSize = true;
            this.orderNumberValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.orderNumberValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.orderNumberValueLabel.Location = new System.Drawing.Point(142, 65);
            this.orderNumberValueLabel.Name = "orderNumberValueLabel";
            this.orderNumberValueLabel.Size = new System.Drawing.Size(69, 20);
            this.orderNumberValueLabel.TabIndex = 31;
            this.orderNumberValueLabel.Text = "000001";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(10, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Клиент:";
            // 
            // clientInfoLabel
            // 
            this.clientInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientInfoLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.clientInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.clientInfoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.clientInfoLabel.Location = new System.Drawing.Point(83, 91);
            this.clientInfoLabel.Name = "clientInfoLabel";
            this.clientInfoLabel.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.clientInfoLabel.Size = new System.Drawing.Size(537, 25);
            this.clientInfoLabel.TabIndex = 32;
            this.clientInfoLabel.Text = "клиент не выбран";
            this.clientInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clientsButton
            // 
            this.clientsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clientsButton.BackColor = System.Drawing.Color.Coral;
            this.clientsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.clientsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clientsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.clientsButton.ForeColor = System.Drawing.Color.Black;
            this.clientsButton.Location = new System.Drawing.Point(628, 88);
            this.clientsButton.Name = "clientsButton";
            this.clientsButton.Size = new System.Drawing.Size(253, 33);
            this.clientsButton.TabIndex = 33;
            this.clientsButton.Text = "Выбрать клиента";
            this.clientsButton.UseVisualStyleBackColor = false;
            this.clientsButton.Click += new System.EventHandler(this.clientsButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(10, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Дата заказа:";
            // 
            // orderDateLabel
            // 
            this.orderDateLabel.AutoSize = true;
            this.orderDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.orderDateLabel.Location = new System.Drawing.Point(111, 128);
            this.orderDateLabel.Name = "orderDateLabel";
            this.orderDateLabel.Size = new System.Drawing.Size(17, 17);
            this.orderDateLabel.TabIndex = 4;
            this.orderDateLabel.Text = "—";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(298, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Дата исполнения:";
            // 
            // completionDatePicker
            // 
            this.completionDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.completionDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.completionDatePicker.Location = new System.Drawing.Point(433, 127);
            this.completionDatePicker.Name = "completionDatePicker";
            this.completionDatePicker.Size = new System.Drawing.Size(121, 23);
            this.completionDatePicker.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(10, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "Товары:";
            // 
            // productsDataGridView
            // 
            this.productsDataGridView.AllowUserToAddRows = false;
            this.productsDataGridView.AllowUserToDeleteRows = false;
            this.productsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.productsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.productsDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.productsDataGridView.Location = new System.Drawing.Point(10, 186);
            this.productsDataGridView.Name = "productsDataGridView";
            this.productsDataGridView.ReadOnly = true;
            this.productsDataGridView.RowHeadersVisible = false;
            this.productsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.productsDataGridView.Size = new System.Drawing.Size(871, 247);
            this.productsDataGridView.TabIndex = 8;
            this.productsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.productsDataGridView_CellContentClick);
            // 
            // addToCartButton
            // 
            this.addToCartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addToCartButton.BackColor = System.Drawing.Color.Coral;
            this.addToCartButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.addToCartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addToCartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.addToCartButton.ForeColor = System.Drawing.Color.Black;
            this.addToCartButton.Location = new System.Drawing.Point(10, 442);
            this.addToCartButton.Name = "addToCartButton";
            this.addToCartButton.Size = new System.Drawing.Size(163, 28);
            this.addToCartButton.TabIndex = 9;
            this.addToCartButton.Text = "Добавить в корзину";
            this.addToCartButton.UseVisualStyleBackColor = false;
            this.addToCartButton.Click += new System.EventHandler(this.addToCartButton_Click);
            // 
            // cartDataGridView
            // 
            this.cartDataGridView.AllowUserToAddRows = false;
            this.cartDataGridView.AllowUserToDeleteRows = false;
            this.cartDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cartDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cartDataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.cartDataGridView.Location = new System.Drawing.Point(10, 503);
            this.cartDataGridView.Name = "cartDataGridView";
            this.cartDataGridView.ReadOnly = true;
            this.cartDataGridView.RowHeadersVisible = false;
            this.cartDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cartDataGridView.Size = new System.Drawing.Size(617, 61);
            this.cartDataGridView.TabIndex = 14;
            this.cartDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cartDataGridView_CellContentClick);
            this.cartDataGridView.SelectionChanged += new System.EventHandler(this.cartDataGridView_SelectionChanged);
            // 
            // removeFromCartButton
            // 
            this.removeFromCartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeFromCartButton.BackColor = System.Drawing.Color.Coral;
            this.removeFromCartButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.removeFromCartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeFromCartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.removeFromCartButton.ForeColor = System.Drawing.Color.Black;
            this.removeFromCartButton.Location = new System.Drawing.Point(10, 572);
            this.removeFromCartButton.Name = "removeFromCartButton";
            this.removeFromCartButton.Size = new System.Drawing.Size(163, 28);
            this.removeFromCartButton.TabIndex = 15;
            this.removeFromCartButton.Text = "Удалить из корзины";
            this.removeFromCartButton.UseVisualStyleBackColor = false;
            this.removeFromCartButton.Click += new System.EventHandler(this.removeFromCartButton_Click);
            // 
            // clearCartButton
            // 
            this.clearCartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearCartButton.BackColor = System.Drawing.Color.Coral;
            this.clearCartButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.clearCartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearCartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.clearCartButton.ForeColor = System.Drawing.Color.Black;
            this.clearCartButton.Location = new System.Drawing.Point(217, 572);
            this.clearCartButton.Name = "clearCartButton";
            this.clearCartButton.Size = new System.Drawing.Size(141, 28);
            this.clearCartButton.TabIndex = 16;
            this.clearCartButton.Text = "Очистить корзину";
            this.clearCartButton.UseVisualStyleBackColor = false;
            this.clearCartButton.Click += new System.EventHandler(this.clearCartButton_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(10, 481);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "Корзина:";
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hintLabel.AutoSize = true;
            this.hintLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic);
            this.hintLabel.ForeColor = System.Drawing.Color.DimGray;
            this.hintLabel.Location = new System.Drawing.Point(247, 484);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(380, 15);
            this.hintLabel.TabIndex = 36;
            this.hintLabel.Text = "Управление количеством: ↑ — добавить, ↓ — убрать (минимум 0).";
            // 
            // totalsPanel
            // 
            this.totalsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totalsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(238)))), ((int)(((byte)(228)))));
            this.totalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.totalsPanel.Controls.Add(this.label8);
            this.totalsPanel.Controls.Add(this.subtotalLabel);
            this.totalsPanel.Controls.Add(this.label9);
            this.totalsPanel.Controls.Add(this.discountAmountLabel);
            this.totalsPanel.Controls.Add(this.label10);
            this.totalsPanel.Controls.Add(this.totalLabel);
            this.totalsPanel.Location = new System.Drawing.Point(639, 503);
            this.totalsPanel.Name = "totalsPanel";
            this.totalsPanel.Size = new System.Drawing.Size(243, 61);
            this.totalsPanel.TabIndex = 50;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(9, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Подытог:";
            // 
            // subtotalLabel
            // 
            this.subtotalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.subtotalLabel.Location = new System.Drawing.Point(116, 7);
            this.subtotalLabel.Name = "subtotalLabel";
            this.subtotalLabel.Size = new System.Drawing.Size(116, 16);
            this.subtotalLabel.TabIndex = 18;
            this.subtotalLabel.Text = "0,00 ₽";
            this.subtotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label9.Location = new System.Drawing.Point(9, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Скидка:";
            // 
            // discountAmountLabel
            // 
            this.discountAmountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.discountAmountLabel.Location = new System.Drawing.Point(81, 24);
            this.discountAmountLabel.Name = "discountAmountLabel";
            this.discountAmountLabel.Size = new System.Drawing.Size(150, 16);
            this.discountAmountLabel.TabIndex = 20;
            this.discountAmountLabel.Text = "0,00 ₽ (0%)";
            this.discountAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(9, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 20);
            this.label10.TabIndex = 21;
            this.label10.Text = "ИТОГО:";
            // 
            // totalLabel
            // 
            this.totalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.totalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.totalLabel.Location = new System.Drawing.Point(94, 41);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(137, 19);
            this.totalLabel.TabIndex = 22;
            this.totalLabel.Text = "0,00 ₽";
            this.totalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // createOrderButton
            // 
            this.createOrderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createOrderButton.BackColor = System.Drawing.Color.Coral;
            this.createOrderButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.createOrderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createOrderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.createOrderButton.ForeColor = System.Drawing.Color.Black;
            this.createOrderButton.Location = new System.Drawing.Point(639, 572);
            this.createOrderButton.Name = "createOrderButton";
            this.createOrderButton.Size = new System.Drawing.Size(243, 35);
            this.createOrderButton.TabIndex = 23;
            this.createOrderButton.Text = "Оформить заказ";
            this.createOrderButton.UseVisualStyleBackColor = false;
            this.createOrderButton.Click += new System.EventHandler(this.createOrderButton_Click);
            // 
            // searchProductsTextBox
            // 
            this.searchProductsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.searchProductsTextBox.Location = new System.Drawing.Point(195, 158);
            this.searchProductsTextBox.Name = "searchProductsTextBox";
            this.searchProductsTextBox.Size = new System.Drawing.Size(251, 23);
            this.searchProductsTextBox.TabIndex = 35;
            this.searchProductsTextBox.TextChanged += new System.EventHandler(this.searchProductsTextBox_TextChanged);
            // 
            // searchProductsLabel
            // 
            this.searchProductsLabel.AutoSize = true;
            this.searchProductsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.searchProductsLabel.Location = new System.Drawing.Point(143, 161);
            this.searchProductsLabel.Name = "searchProductsLabel";
            this.searchProductsLabel.Size = new System.Drawing.Size(52, 17);
            this.searchProductsLabel.TabIndex = 34;
            this.searchProductsLabel.Text = "Поиск:";
            // 
            // NewOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 617);
            this.Controls.Add(this.createOrderButton);
            this.Controls.Add(this.totalsPanel);
            this.Controls.Add(this.clearCartButton);
            this.Controls.Add(this.removeFromCartButton);
            this.Controls.Add(this.cartDataGridView);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.addToCartButton);
            this.Controls.Add(this.productsDataGridView);
            this.Controls.Add(this.searchProductsTextBox);
            this.Controls.Add(this.searchProductsLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.completionDatePicker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.orderDateLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clientsButton);
            this.Controls.Add(this.clientInfoLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.orderNumberValueLabel);
            this.Controls.Add(this.orderNumberTitleLabel);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(894, 647);
            this.Name = "NewOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Новый заказ";
            this.Load += new System.EventHandler(this.NewOrderForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cartDataGridView)).EndInit();
            this.totalsPanel.ResumeLayout(false);
            this.totalsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
