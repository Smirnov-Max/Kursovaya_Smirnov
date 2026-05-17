namespace Smirnov_kursovaya.secondForm
{
    partial class ReferencesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.Button categoriesButton;
        private System.Windows.Forms.Button statusesButton;

        // Панель категорий
        private System.Windows.Forms.Panel categoriesPanel;
        private System.Windows.Forms.DataGridView categoriesDataGridView;
        private System.Windows.Forms.TextBox categoryNameTextBox;
        private System.Windows.Forms.Button addCategoryButton;
        private System.Windows.Forms.Button editCategoryButton;
        private System.Windows.Forms.Button deleteCategoryButton;
        private System.Windows.Forms.Label categoryNameLabel;
        private System.Windows.Forms.Label categoryCountLabel;

        // Панель статусов
        private System.Windows.Forms.Panel statusesPanel;
        private System.Windows.Forms.DataGridView statusesDataGridView;
        private System.Windows.Forms.TextBox statusNameTextBox;
        private System.Windows.Forms.Button addStatusButton;
        private System.Windows.Forms.Button editStatusButton;
        private System.Windows.Forms.Button deleteStatusButton;
        private System.Windows.Forms.Label statusNameLabel;
        private System.Windows.Forms.Label statusCountLabel;

        // Нижняя панель (Dock=Bottom) — здесь живут кнопки спец. возможностей,
        // чтобы их не перекрывали categoriesPanel/statusesPanel при ресайзе.
        private System.Windows.Forms.Panel bottomActionsPanel;
        private System.Windows.Forms.Button btnExportAll;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnRestore;

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
            this.categoriesButton = new System.Windows.Forms.Button();
            this.statusesButton = new System.Windows.Forms.Button();
            this.categoriesPanel = new System.Windows.Forms.Panel();
            this.categoriesDataGridView = new System.Windows.Forms.DataGridView();
            this.categoryNameLabel = new System.Windows.Forms.Label();
            this.categoryNameTextBox = new System.Windows.Forms.TextBox();
            this.addCategoryButton = new System.Windows.Forms.Button();
            this.editCategoryButton = new System.Windows.Forms.Button();
            this.deleteCategoryButton = new System.Windows.Forms.Button();
            this.categoryCountLabel = new System.Windows.Forms.Label();
            this.statusesPanel = new System.Windows.Forms.Panel();
            this.statusesDataGridView = new System.Windows.Forms.DataGridView();
            this.statusNameLabel = new System.Windows.Forms.Label();
            this.statusNameTextBox = new System.Windows.Forms.TextBox();
            this.addStatusButton = new System.Windows.Forms.Button();
            this.editStatusButton = new System.Windows.Forms.Button();
            this.deleteStatusButton = new System.Windows.Forms.Button();
            this.statusCountLabel = new System.Windows.Forms.Label();
            this.bottomActionsPanel = new System.Windows.Forms.Panel();
            this.btnExportAll = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.categoriesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.categoriesDataGridView)).BeginInit();
            this.statusesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusesDataGridView)).BeginInit();
            this.bottomActionsPanel.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(771, 52);
            this.panel1.TabIndex = 0;
            // 
            // menuButton
            // 
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.menuButton.Location = new System.Drawing.Point(675, 13);
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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(658, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Справочники";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // categoriesButton
            // 
            this.categoriesButton.BackColor = System.Drawing.Color.Coral;
            this.categoriesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.categoriesButton.Location = new System.Drawing.Point(10, 57);
            this.categoriesButton.Name = "categoriesButton";
            this.categoriesButton.Size = new System.Drawing.Size(103, 26);
            this.categoriesButton.TabIndex = 1;
            this.categoriesButton.Text = "Категории";
            this.categoriesButton.UseVisualStyleBackColor = false;
            this.categoriesButton.Click += new System.EventHandler(this.categoriesButton_Click);
            // 
            // statusesButton
            // 
            this.statusesButton.BackColor = System.Drawing.Color.Coral;
            this.statusesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.statusesButton.Location = new System.Drawing.Point(118, 57);
            this.statusesButton.Name = "statusesButton";
            this.statusesButton.Size = new System.Drawing.Size(103, 26);
            this.statusesButton.TabIndex = 2;
            this.statusesButton.Text = "Статусы";
            this.statusesButton.UseVisualStyleBackColor = false;
            this.statusesButton.Click += new System.EventHandler(this.statusesButton_Click);
            // 
            // categoriesPanel
            // 
            this.categoriesPanel.Controls.Add(this.categoriesDataGridView);
            this.categoriesPanel.Controls.Add(this.categoryNameLabel);
            this.categoriesPanel.Controls.Add(this.categoryNameTextBox);
            this.categoriesPanel.Controls.Add(this.addCategoryButton);
            this.categoriesPanel.Controls.Add(this.editCategoryButton);
            this.categoriesPanel.Controls.Add(this.deleteCategoryButton);
            this.categoriesPanel.Controls.Add(this.categoryCountLabel);
            this.categoriesPanel.Location = new System.Drawing.Point(0, 98);
            this.categoriesPanel.Name = "categoriesPanel";
            this.categoriesPanel.Size = new System.Drawing.Size(771, 329);
            this.categoriesPanel.TabIndex = 5;
            this.categoriesPanel.Visible = false;
            // 
            // categoriesDataGridView
            // 
            this.categoriesDataGridView.AllowUserToAddRows = false;
            this.categoriesDataGridView.AllowUserToDeleteRows = false;
            this.categoriesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.categoriesDataGridView.Location = new System.Drawing.Point(323, 9);
            this.categoriesDataGridView.MultiSelect = false;
            this.categoriesDataGridView.Name = "categoriesDataGridView";
            this.categoriesDataGridView.ReadOnly = true;
            this.categoriesDataGridView.RowHeadersVisible = false;
            this.categoriesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.categoriesDataGridView.Size = new System.Drawing.Size(438, 312);
            this.categoriesDataGridView.TabIndex = 0;
            this.categoriesDataGridView.SelectionChanged += new System.EventHandler(this.categoriesDataGridView_SelectionChanged);
            // 
            // categoryNameLabel
            // 
            this.categoryNameLabel.AutoSize = true;
            this.categoryNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.categoryNameLabel.Location = new System.Drawing.Point(9, 13);
            this.categoryNameLabel.Name = "categoryNameLabel";
            this.categoryNameLabel.Size = new System.Drawing.Size(76, 17);
            this.categoryNameLabel.TabIndex = 1;
            this.categoryNameLabel.Text = "Название:";
            // 
            // categoryNameTextBox
            // 
            this.categoryNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.categoryNameTextBox.Location = new System.Drawing.Point(9, 30);
            this.categoryNameTextBox.Name = "categoryNameTextBox";
            this.categoryNameTextBox.Size = new System.Drawing.Size(308, 23);
            this.categoryNameTextBox.TabIndex = 1;
            // 
            // addCategoryButton
            // 
            this.addCategoryButton.BackColor = System.Drawing.Color.Coral;
            this.addCategoryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.addCategoryButton.Location = new System.Drawing.Point(9, 61);
            this.addCategoryButton.Name = "addCategoryButton";
            this.addCategoryButton.Size = new System.Drawing.Size(100, 26);
            this.addCategoryButton.TabIndex = 2;
            this.addCategoryButton.Text = "Добавить";
            this.addCategoryButton.UseVisualStyleBackColor = false;
            this.addCategoryButton.Click += new System.EventHandler(this.addCategoryButton_Click);
            // 
            // editCategoryButton
            // 
            this.editCategoryButton.BackColor = System.Drawing.Color.Coral;
            this.editCategoryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.editCategoryButton.Location = new System.Drawing.Point(115, 61);
            this.editCategoryButton.Name = "editCategoryButton";
            this.editCategoryButton.Size = new System.Drawing.Size(119, 26);
            this.editCategoryButton.TabIndex = 3;
            this.editCategoryButton.Text = "Редактировать";
            this.editCategoryButton.UseVisualStyleBackColor = false;
            this.editCategoryButton.Click += new System.EventHandler(this.editCategoryButton_Click);
            // 
            // deleteCategoryButton
            // 
            this.deleteCategoryButton.BackColor = System.Drawing.Color.Coral;
            this.deleteCategoryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.deleteCategoryButton.Location = new System.Drawing.Point(240, 61);
            this.deleteCategoryButton.Name = "deleteCategoryButton";
            this.deleteCategoryButton.Size = new System.Drawing.Size(77, 26);
            this.deleteCategoryButton.TabIndex = 4;
            this.deleteCategoryButton.Text = "Удалить";
            this.deleteCategoryButton.UseVisualStyleBackColor = false;
            this.deleteCategoryButton.Click += new System.EventHandler(this.deleteCategoryButton_Click);
            // 
            // categoryCountLabel
            // 
            this.categoryCountLabel.AutoSize = true;
            this.categoryCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.categoryCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.categoryCountLabel.Location = new System.Drawing.Point(9, 95);
            this.categoryCountLabel.Name = "categoryCountLabel";
            this.categoryCountLabel.Size = new System.Drawing.Size(79, 15);
            this.categoryCountLabel.TabIndex = 5;
            this.categoryCountLabel.Text = "Записей: 0";
            // 
            // statusesPanel
            // 
            this.statusesPanel.Controls.Add(this.statusesDataGridView);
            this.statusesPanel.Controls.Add(this.statusNameLabel);
            this.statusesPanel.Controls.Add(this.statusNameTextBox);
            this.statusesPanel.Controls.Add(this.addStatusButton);
            this.statusesPanel.Controls.Add(this.editStatusButton);
            this.statusesPanel.Controls.Add(this.deleteStatusButton);
            this.statusesPanel.Controls.Add(this.statusCountLabel);
            this.statusesPanel.Location = new System.Drawing.Point(0, 101);
            this.statusesPanel.Name = "statusesPanel";
            this.statusesPanel.Size = new System.Drawing.Size(771, 329);
            this.statusesPanel.TabIndex = 6;
            this.statusesPanel.Visible = false;
            // 
            // statusesDataGridView
            // 
            this.statusesDataGridView.AllowUserToAddRows = false;
            this.statusesDataGridView.AllowUserToDeleteRows = false;
            this.statusesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statusesDataGridView.Location = new System.Drawing.Point(323, 9);
            this.statusesDataGridView.MultiSelect = false;
            this.statusesDataGridView.Name = "statusesDataGridView";
            this.statusesDataGridView.ReadOnly = true;
            this.statusesDataGridView.RowHeadersVisible = false;
            this.statusesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.statusesDataGridView.Size = new System.Drawing.Size(438, 312);
            this.statusesDataGridView.TabIndex = 0;
            this.statusesDataGridView.SelectionChanged += new System.EventHandler(this.statusesDataGridView_SelectionChanged);
            // 
            // statusNameLabel
            // 
            this.statusNameLabel.AutoSize = true;
            this.statusNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.statusNameLabel.Location = new System.Drawing.Point(9, 13);
            this.statusNameLabel.Name = "statusNameLabel";
            this.statusNameLabel.Size = new System.Drawing.Size(76, 17);
            this.statusNameLabel.TabIndex = 1;
            this.statusNameLabel.Text = "Название:";
            // 
            // statusNameTextBox
            // 
            this.statusNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.statusNameTextBox.Location = new System.Drawing.Point(9, 30);
            this.statusNameTextBox.Name = "statusNameTextBox";
            this.statusNameTextBox.Size = new System.Drawing.Size(308, 23);
            this.statusNameTextBox.TabIndex = 1;
            // 
            // addStatusButton
            // 
            this.addStatusButton.BackColor = System.Drawing.Color.Coral;
            this.addStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.addStatusButton.Location = new System.Drawing.Point(9, 61);
            this.addStatusButton.Name = "addStatusButton";
            this.addStatusButton.Size = new System.Drawing.Size(100, 26);
            this.addStatusButton.TabIndex = 2;
            this.addStatusButton.Text = "Добавить";
            this.addStatusButton.UseVisualStyleBackColor = false;
            this.addStatusButton.Click += new System.EventHandler(this.addStatusButton_Click);
            // 
            // editStatusButton
            // 
            this.editStatusButton.BackColor = System.Drawing.Color.Coral;
            this.editStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.editStatusButton.Location = new System.Drawing.Point(115, 61);
            this.editStatusButton.Name = "editStatusButton";
            this.editStatusButton.Size = new System.Drawing.Size(119, 26);
            this.editStatusButton.TabIndex = 3;
            this.editStatusButton.Text = "Редактировать";
            this.editStatusButton.UseVisualStyleBackColor = false;
            this.editStatusButton.Click += new System.EventHandler(this.editStatusButton_Click);
            // 
            // deleteStatusButton
            // 
            this.deleteStatusButton.BackColor = System.Drawing.Color.Coral;
            this.deleteStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.deleteStatusButton.Location = new System.Drawing.Point(240, 61);
            this.deleteStatusButton.Name = "deleteStatusButton";
            this.deleteStatusButton.Size = new System.Drawing.Size(77, 26);
            this.deleteStatusButton.TabIndex = 4;
            this.deleteStatusButton.Text = "Удалить";
            this.deleteStatusButton.UseVisualStyleBackColor = false;
            this.deleteStatusButton.Click += new System.EventHandler(this.deleteStatusButton_Click);
            // 
            // statusCountLabel
            // 
            this.statusCountLabel.AutoSize = true;
            this.statusCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.statusCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.statusCountLabel.Location = new System.Drawing.Point(9, 95);
            this.statusCountLabel.Name = "statusCountLabel";
            this.statusCountLabel.Size = new System.Drawing.Size(79, 15);
            this.statusCountLabel.TabIndex = 5;
            this.statusCountLabel.Text = "Записей: 0";
            // 
            // bottomActionsPanel
            // 
            this.bottomActionsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(238)))), ((int)(((byte)(228)))));
            this.bottomActionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomActionsPanel.Controls.Add(this.btnExportAll);
            this.bottomActionsPanel.Controls.Add(this.btnImport);
            this.bottomActionsPanel.Controls.Add(this.btnBackup);
            this.bottomActionsPanel.Controls.Add(this.btnRestore);
            this.bottomActionsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomActionsPanel.Location = new System.Drawing.Point(0, 469);
            this.bottomActionsPanel.Name = "bottomActionsPanel";
            this.bottomActionsPanel.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.bottomActionsPanel.Size = new System.Drawing.Size(771, 52);
            this.bottomActionsPanel.TabIndex = 9;
            // 
            // btnExportAll
            // 
            this.btnExportAll.BackColor = System.Drawing.Color.Coral;
            this.btnExportAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.btnExportAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportAll.ForeColor = System.Drawing.Color.Black;
            this.btnExportAll.Location = new System.Drawing.Point(10, 11);
            this.btnExportAll.Name = "btnExportAll";
            this.btnExportAll.Size = new System.Drawing.Size(171, 29);
            this.btnExportAll.TabIndex = 10;
            this.btnExportAll.Text = "Экспорт таблиц (CSV)";
            this.btnExportAll.UseVisualStyleBackColor = false;
            this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.Coral;
            this.btnImport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnImport.ForeColor = System.Drawing.Color.Black;
            this.btnImport.Location = new System.Drawing.Point(190, 11);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(154, 29);
            this.btnImport.TabIndex = 11;
            this.btnImport.Text = "Импорт из CSV";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.BackColor = System.Drawing.Color.Coral;
            this.btnBackup.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBackup.ForeColor = System.Drawing.Color.Black;
            this.btnBackup.Location = new System.Drawing.Point(353, 11);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(219, 29);
            this.btnBackup.TabIndex = 12;
            this.btnBackup.Text = "Резервное копирование";
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.Coral;
            this.btnRestore.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRestore.ForeColor = System.Drawing.Color.Black;
            this.btnRestore.Location = new System.Drawing.Point(578, 11);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(180, 29);
            this.btnRestore.TabIndex = 13;
            this.btnRestore.Text = "Восстановление БД";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // ReferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 521);
            this.Controls.Add(this.categoriesPanel);
            this.Controls.Add(this.statusesPanel);
            this.Controls.Add(this.statusesButton);
            this.Controls.Add(this.categoriesButton);
            this.Controls.Add(this.bottomActionsPanel);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(787, 560);
            this.Name = "ReferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочники";
            this.Load += new System.EventHandler(this.ReferencesForm_Load);
            this.panel1.ResumeLayout(false);
            this.categoriesPanel.ResumeLayout(false);
            this.categoriesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.categoriesDataGridView)).EndInit();
            this.statusesPanel.ResumeLayout(false);
            this.statusesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusesDataGridView)).EndInit();
            this.bottomActionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
