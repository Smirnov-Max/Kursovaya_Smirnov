namespace Smirnov_kursovaya.secondForm
{
    partial class ClientsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.DataGridView clientsDataGridView;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button sortButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label recordCountLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox fioTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox phoneTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button showPhoneButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addToOrderButton;
        private System.Windows.Forms.Button cancelSelectionButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.clientsDataGridView = new System.Windows.Forms.DataGridView();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.sortButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.recordCountLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fioTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.phoneTextBox = new System.Windows.Forms.TextBox();
            this.showPhoneButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addToOrderButton = new System.Windows.Forms.Button();
            this.cancelSelectionButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientsDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(900, 60);
            this.panel1.TabIndex = 0;
            // 
            // menuButton
            // 
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.menuButton.Location = new System.Drawing.Point(788, 15);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(100, 30);
            this.menuButton.TabIndex = 1;
            this.menuButton.Text = "Меню";
            this.menuButton.UseVisualStyleBackColor = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(768, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Клиенты";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clientsDataGridView
            // 
            this.clientsDataGridView.AllowUserToAddRows = false;
            this.clientsDataGridView.AllowUserToDeleteRows = false;
            this.clientsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientsDataGridView.Location = new System.Drawing.Point(351, 76);
            this.clientsDataGridView.Name = "clientsDataGridView";
            this.clientsDataGridView.ReadOnly = true;
            this.clientsDataGridView.RowHeadersVisible = false;
            this.clientsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clientsDataGridView.Size = new System.Drawing.Size(537, 364);
            this.clientsDataGridView.TabIndex = 1;
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.searchLabel.Location = new System.Drawing.Point(12, 76);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(141, 17);
            this.searchLabel.TabIndex = 11;
            this.searchLabel.Text = "Поиск по телефону:";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.searchTextBox.Location = new System.Drawing.Point(12, 96);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(333, 23);
            this.searchTextBox.TabIndex = 2;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // sortButton
            // 
            this.sortButton.BackColor = System.Drawing.Color.Coral;
            this.sortButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.sortButton.Location = new System.Drawing.Point(12, 128);
            this.sortButton.Name = "sortButton";
            this.sortButton.Size = new System.Drawing.Size(180, 30);
            this.sortButton.TabIndex = 3;
            this.sortButton.Text = "Сортировка А-Я";
            this.sortButton.UseVisualStyleBackColor = false;
            this.sortButton.Click += new System.EventHandler(this.sortButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.Coral;
            this.resetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.resetButton.Location = new System.Drawing.Point(231, 128);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(114, 30);
            this.resetButton.TabIndex = 4;
            this.resetButton.Text = "Сброс";
            this.resetButton.UseVisualStyleBackColor = false;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // recordCountLabel
            // 
            this.recordCountLabel.AutoSize = true;
            this.recordCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.recordCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.recordCountLabel.Location = new System.Drawing.Point(12, 168);
            this.recordCountLabel.Name = "recordCountLabel";
            this.recordCountLabel.Size = new System.Drawing.Size(79, 15);
            this.recordCountLabel.TabIndex = 10;
            this.recordCountLabel.Text = "Записей: 0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.fioTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.phoneTextBox);
            this.groupBox1.Controls.Add(this.showPhoneButton);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox1.Location = new System.Drawing.Point(12, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 110);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Данные клиента";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(10, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "ФИО:";
            // 
            // fioTextBox
            // 
            this.fioTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.fioTextBox.Location = new System.Drawing.Point(85, 27);
            this.fioTextBox.Name = "fioTextBox";
            this.fioTextBox.Size = new System.Drawing.Size(242, 23);
            this.fioTextBox.TabIndex = 1;
            this.fioTextBox.TextChanged += new System.EventHandler(this.fioTextBox_TextChanged);
            this.fioTextBox.Enter += new System.EventHandler(this.fioTextBox_Enter);
            this.fioTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fioTextBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Телефон:";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.phoneTextBox.Location = new System.Drawing.Point(85, 59);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(206, 23);
            this.phoneTextBox.TabIndex = 2;
            this.phoneTextBox.Text = "+7(";
            this.phoneTextBox.TextChanged += new System.EventHandler(this.phoneTextBox_TextChanged);
            this.phoneTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.phoneTextBox_KeyPress);
            // 
            // showPhoneButton
            // 
            this.showPhoneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.showPhoneButton.Location = new System.Drawing.Point(297, 57);
            this.showPhoneButton.Name = "showPhoneButton";
            this.showPhoneButton.Size = new System.Drawing.Size(30, 25);
            this.showPhoneButton.TabIndex = 3;
            this.showPhoneButton.Text = "👁";
            this.showPhoneButton.UseVisualStyleBackColor = false;
            this.showPhoneButton.Click += new System.EventHandler(this.showPhoneButton_Click);
            // 
            // addButton
            // 
            this.addButton.BackColor = System.Drawing.Color.Coral;
            this.addButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.addButton.Location = new System.Drawing.Point(12, 320);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 32);
            this.addButton.TabIndex = 6;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = false;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // editButton
            // 
            this.editButton.BackColor = System.Drawing.Color.Coral;
            this.editButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.editButton.Location = new System.Drawing.Point(128, 320);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(123, 32);
            this.editButton.TabIndex = 7;
            this.editButton.Text = "Редактировать";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.Coral;
            this.deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.deleteButton.Location = new System.Drawing.Point(257, 320);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(88, 32);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // addToOrderButton
            // 
            this.addToOrderButton.BackColor = System.Drawing.Color.Coral;
            this.addToOrderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.addToOrderButton.Location = new System.Drawing.Point(12, 365);
            this.addToOrderButton.Name = "addToOrderButton";
            this.addToOrderButton.Size = new System.Drawing.Size(333, 38);
            this.addToOrderButton.TabIndex = 9;
            this.addToOrderButton.Text = "Подтвердить выбор";
            this.addToOrderButton.UseVisualStyleBackColor = false;
            this.addToOrderButton.Visible = false;
            this.addToOrderButton.Click += new System.EventHandler(this.addToOrderButton_Click);
            // 
            // cancelSelectionButton
            // 
            this.cancelSelectionButton.BackColor = System.Drawing.Color.Gainsboro;
            this.cancelSelectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cancelSelectionButton.Location = new System.Drawing.Point(12, 410);
            this.cancelSelectionButton.Name = "cancelSelectionButton";
            this.cancelSelectionButton.Size = new System.Drawing.Size(333, 30);
            this.cancelSelectionButton.TabIndex = 10;
            this.cancelSelectionButton.Text = "Отмена";
            this.cancelSelectionButton.UseVisualStyleBackColor = false;
            this.cancelSelectionButton.Visible = false;
            this.cancelSelectionButton.Click += new System.EventHandler(this.cancelSelectionButton_Click);
            // 
            // ClientsForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 470);
            this.Controls.Add(this.cancelSelectionButton);
            this.Controls.Add(this.addToOrderButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.recordCountLabel);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.sortButton);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.clientsDataGridView);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(916, 509);
            this.Name = "ClientsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Клиенты";
            this.Load += new System.EventHandler(this.ClientsForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientsDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
