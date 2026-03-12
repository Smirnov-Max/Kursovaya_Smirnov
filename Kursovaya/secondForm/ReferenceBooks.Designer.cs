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
        private System.Windows.Forms.Button rolesButton;
        private System.Windows.Forms.Button customTablesButton;

        // Панель категорий
        private System.Windows.Forms.Panel categoriesPanel;
        private System.Windows.Forms.DataGridView categoriesDataGridView;
        private System.Windows.Forms.TextBox categoryNameTextBox;
        private System.Windows.Forms.TextBox categoryDescTextBox;
        private System.Windows.Forms.Button addCategoryButton;
        private System.Windows.Forms.Button editCategoryButton;
        private System.Windows.Forms.Button deleteCategoryButton;
        private System.Windows.Forms.Label categoryNameLabel;
        private System.Windows.Forms.Label categoryDescLabel;

        // Панель статусов
        private System.Windows.Forms.Panel statusesPanel;
        private System.Windows.Forms.DataGridView statusesDataGridView;
        private System.Windows.Forms.TextBox statusNameTextBox;
        private System.Windows.Forms.TextBox statusDescTextBox;
        private System.Windows.Forms.Button addStatusButton;
        private System.Windows.Forms.Button editStatusButton;
        private System.Windows.Forms.Button deleteStatusButton;
        private System.Windows.Forms.Label statusNameLabel;
        private System.Windows.Forms.Label statusDescLabel;

        // Панель ролей
        private System.Windows.Forms.Panel rolesPanel;
        private System.Windows.Forms.DataGridView rolesDataGridView;
        private System.Windows.Forms.TextBox roleNameTextBox;
        private System.Windows.Forms.TextBox roleDescTextBox;
        private System.Windows.Forms.Button addRoleButton;
        private System.Windows.Forms.Button editRoleButton;
        private System.Windows.Forms.Button deleteRoleButton;
        private System.Windows.Forms.Label roleNameLabel;
        private System.Windows.Forms.Label roleDescLabel;

        // Панель пользовательских таблиц
        private System.Windows.Forms.Panel customTablesPanel;
        private System.Windows.Forms.DataGridView customTableDataGridView;
        private System.Windows.Forms.ListBox tablesListBox;
        private System.Windows.Forms.TextBox newTableNameTextBox;
        private System.Windows.Forms.Button createTableButton;
        private System.Windows.Forms.TextBox customNameTextBox;
        private System.Windows.Forms.TextBox customDescTextBox;
        private System.Windows.Forms.Button addToCustomTableButton;
        private System.Windows.Forms.Button deleteFromCustomTableButton;
        private System.Windows.Forms.Label tablesListLabel;
        private System.Windows.Forms.Label newTableLabel;
        private System.Windows.Forms.Label customNameLabel;
        private System.Windows.Forms.Label customDescLabel;

        // Кнопка экспорта в csv
        private System.Windows.Forms.Button btnExportAll;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.categoriesButton = new System.Windows.Forms.Button();
            this.statusesButton = new System.Windows.Forms.Button();
            this.rolesButton = new System.Windows.Forms.Button();
            this.customTablesButton = new System.Windows.Forms.Button();
            this.categoriesPanel = new System.Windows.Forms.Panel();
            this.categoriesDataGridView = new System.Windows.Forms.DataGridView();
            this.categoryNameLabel = new System.Windows.Forms.Label();
            this.categoryNameTextBox = new System.Windows.Forms.TextBox();
            this.categoryDescLabel = new System.Windows.Forms.Label();
            this.categoryDescTextBox = new System.Windows.Forms.TextBox();
            this.addCategoryButton = new System.Windows.Forms.Button();
            this.editCategoryButton = new System.Windows.Forms.Button();
            this.deleteCategoryButton = new System.Windows.Forms.Button();
            this.statusesPanel = new System.Windows.Forms.Panel();
            this.statusesDataGridView = new System.Windows.Forms.DataGridView();
            this.statusNameLabel = new System.Windows.Forms.Label();
            this.statusNameTextBox = new System.Windows.Forms.TextBox();
            this.statusDescLabel = new System.Windows.Forms.Label();
            this.statusDescTextBox = new System.Windows.Forms.TextBox();
            this.addStatusButton = new System.Windows.Forms.Button();
            this.editStatusButton = new System.Windows.Forms.Button();
            this.deleteStatusButton = new System.Windows.Forms.Button();
            this.rolesPanel = new System.Windows.Forms.Panel();
            this.rolesDataGridView = new System.Windows.Forms.DataGridView();
            this.roleNameLabel = new System.Windows.Forms.Label();
            this.roleNameTextBox = new System.Windows.Forms.TextBox();
            this.roleDescLabel = new System.Windows.Forms.Label();
            this.roleDescTextBox = new System.Windows.Forms.TextBox();
            this.addRoleButton = new System.Windows.Forms.Button();
            this.editRoleButton = new System.Windows.Forms.Button();
            this.deleteRoleButton = new System.Windows.Forms.Button();
            this.customTablesPanel = new System.Windows.Forms.Panel();
            this.customTableDataGridView = new System.Windows.Forms.DataGridView();
            this.tablesListBox = new System.Windows.Forms.ListBox();
            this.newTableLabel = new System.Windows.Forms.Label();
            this.newTableNameTextBox = new System.Windows.Forms.TextBox();
            this.createTableButton = new System.Windows.Forms.Button();
            this.customNameLabel = new System.Windows.Forms.Label();
            this.customNameTextBox = new System.Windows.Forms.TextBox();
            this.customDescLabel = new System.Windows.Forms.Label();
            this.customDescTextBox = new System.Windows.Forms.TextBox();
            this.addToCustomTableButton = new System.Windows.Forms.Button();
            this.deleteFromCustomTableButton = new System.Windows.Forms.Button();
            this.tablesListLabel = new System.Windows.Forms.Label();
            this.btnExportAll = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.categoriesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.categoriesDataGridView)).BeginInit();
            this.statusesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusesDataGridView)).BeginInit();
            this.rolesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rolesDataGridView)).BeginInit();
            this.customTablesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customTableDataGridView)).BeginInit();
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
            this.label1.Text = "Справочники";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // categoriesButton
            // 
            this.categoriesButton.BackColor = System.Drawing.Color.Coral;
            this.categoriesButton.Location = new System.Drawing.Point(12, 66);
            this.categoriesButton.Name = "categoriesButton";
            this.categoriesButton.Size = new System.Drawing.Size(120, 30);
            this.categoriesButton.TabIndex = 1;
            this.categoriesButton.Text = "Категории";
            this.categoriesButton.UseVisualStyleBackColor = false;
            this.categoriesButton.Click += new System.EventHandler(this.categoriesButton_Click);
            // 
            // statusesButton
            // 
            this.statusesButton.BackColor = System.Drawing.Color.Coral;
            this.statusesButton.Location = new System.Drawing.Point(138, 66);
            this.statusesButton.Name = "statusesButton";
            this.statusesButton.Size = new System.Drawing.Size(120, 30);
            this.statusesButton.TabIndex = 2;
            this.statusesButton.Text = "Статусы";
            this.statusesButton.UseVisualStyleBackColor = false;
            this.statusesButton.Click += new System.EventHandler(this.statusesButton_Click);
            // 
            // rolesButton
            // 
            this.rolesButton.BackColor = System.Drawing.Color.Coral;
            this.rolesButton.Location = new System.Drawing.Point(264, 66);
            this.rolesButton.Name = "rolesButton";
            this.rolesButton.Size = new System.Drawing.Size(120, 30);
            this.rolesButton.TabIndex = 3;
            this.rolesButton.Text = "Роли";
            this.rolesButton.UseVisualStyleBackColor = false;
            this.rolesButton.Click += new System.EventHandler(this.rolesButton_Click);
            // 
            // customTablesButton
            // 
            this.customTablesButton.BackColor = System.Drawing.Color.Coral;
            this.customTablesButton.Location = new System.Drawing.Point(390, 66);
            this.customTablesButton.Name = "customTablesButton";
            this.customTablesButton.Size = new System.Drawing.Size(183, 30);
            this.customTablesButton.TabIndex = 4;
            this.customTablesButton.Text = "Пользовательские таблицы";
            this.customTablesButton.UseVisualStyleBackColor = false;
            this.customTablesButton.Click += new System.EventHandler(this.customTablesButton_Click);
            // 
            // categoriesPanel
            // 
            this.categoriesPanel.Controls.Add(this.categoriesDataGridView);
            this.categoriesPanel.Controls.Add(this.categoryNameLabel);
            this.categoriesPanel.Controls.Add(this.categoryNameTextBox);
            this.categoriesPanel.Controls.Add(this.categoryDescLabel);
            this.categoriesPanel.Controls.Add(this.categoryDescTextBox);
            this.categoriesPanel.Controls.Add(this.addCategoryButton);
            this.categoriesPanel.Controls.Add(this.editCategoryButton);
            this.categoriesPanel.Controls.Add(this.deleteCategoryButton);
            this.categoriesPanel.Location = new System.Drawing.Point(0, 102);
            this.categoriesPanel.Name = "categoriesPanel";
            this.categoriesPanel.Size = new System.Drawing.Size(884, 459);
            this.categoriesPanel.TabIndex = 5;
            this.categoriesPanel.Visible = false;
            // 
            // categoriesDataGridView
            // 
            this.categoriesDataGridView.AllowUserToAddRows = false;
            this.categoriesDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.categoriesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.categoriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.categoriesDataGridView.Location = new System.Drawing.Point(324, 12);
            this.categoriesDataGridView.MultiSelect = false;
            this.categoriesDataGridView.Name = "categoriesDataGridView";
            this.categoriesDataGridView.ReadOnly = true;
            this.categoriesDataGridView.RowHeadersVisible = false;
            this.categoriesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.categoriesDataGridView.Size = new System.Drawing.Size(548, 435);
            this.categoriesDataGridView.TabIndex = 0;
            this.categoriesDataGridView.SelectionChanged += new System.EventHandler(this.categoriesDataGridView_SelectionChanged);
            // 
            // categoryNameLabel
            // 
            this.categoryNameLabel.AutoSize = true;
            this.categoryNameLabel.Location = new System.Drawing.Point(12, 15);
            this.categoryNameLabel.Name = "categoryNameLabel";
            this.categoryNameLabel.Size = new System.Drawing.Size(60, 13);
            this.categoryNameLabel.TabIndex = 1;
            this.categoryNameLabel.Text = "Название:";
            // 
            // categoryNameTextBox
            // 
            this.categoryNameTextBox.Location = new System.Drawing.Point(12, 31);
            this.categoryNameTextBox.Name = "categoryNameTextBox";
            this.categoryNameTextBox.Size = new System.Drawing.Size(300, 20);
            this.categoryNameTextBox.TabIndex = 2;
            // 
            // categoryDescLabel
            // 
            this.categoryDescLabel.AutoSize = true;
            this.categoryDescLabel.Location = new System.Drawing.Point(12, 65);
            this.categoryDescLabel.Name = "categoryDescLabel";
            this.categoryDescLabel.Size = new System.Drawing.Size(60, 13);
            this.categoryDescLabel.TabIndex = 3;
            this.categoryDescLabel.Text = "Описание:";
            // 
            // categoryDescTextBox
            // 
            this.categoryDescTextBox.Location = new System.Drawing.Point(12, 81);
            this.categoryDescTextBox.Multiline = true;
            this.categoryDescTextBox.Name = "categoryDescTextBox";
            this.categoryDescTextBox.Size = new System.Drawing.Size(300, 80);
            this.categoryDescTextBox.TabIndex = 4;
            // 
            // addCategoryButton
            // 
            this.addCategoryButton.Location = new System.Drawing.Point(12, 180);
            this.addCategoryButton.Name = "addCategoryButton";
            this.addCategoryButton.Size = new System.Drawing.Size(90, 30);
            this.addCategoryButton.TabIndex = 5;
            this.addCategoryButton.Text = "Добавить";
            this.addCategoryButton.UseVisualStyleBackColor = true;
            this.addCategoryButton.Click += new System.EventHandler(this.addCategoryButton_Click);
            // 
            // editCategoryButton
            // 
            this.editCategoryButton.Location = new System.Drawing.Point(108, 180);
            this.editCategoryButton.Name = "editCategoryButton";
            this.editCategoryButton.Size = new System.Drawing.Size(100, 30);
            this.editCategoryButton.TabIndex = 6;
            this.editCategoryButton.Text = "Редактировать";
            this.editCategoryButton.UseVisualStyleBackColor = true;
            this.editCategoryButton.Click += new System.EventHandler(this.editCategoryButton_Click);
            // 
            // deleteCategoryButton
            // 
            this.deleteCategoryButton.Location = new System.Drawing.Point(214, 180);
            this.deleteCategoryButton.Name = "deleteCategoryButton";
            this.deleteCategoryButton.Size = new System.Drawing.Size(90, 30);
            this.deleteCategoryButton.TabIndex = 7;
            this.deleteCategoryButton.Text = "Удалить";
            this.deleteCategoryButton.UseVisualStyleBackColor = true;
            this.deleteCategoryButton.Click += new System.EventHandler(this.deleteCategoryButton_Click);
            // 
            // statusesPanel
            // 
            this.statusesPanel.Controls.Add(this.statusesDataGridView);
            this.statusesPanel.Controls.Add(this.statusNameLabel);
            this.statusesPanel.Controls.Add(this.statusNameTextBox);
            this.statusesPanel.Controls.Add(this.statusDescLabel);
            this.statusesPanel.Controls.Add(this.statusDescTextBox);
            this.statusesPanel.Controls.Add(this.addStatusButton);
            this.statusesPanel.Controls.Add(this.editStatusButton);
            this.statusesPanel.Controls.Add(this.deleteStatusButton);
            this.statusesPanel.Location = new System.Drawing.Point(0, 102);
            this.statusesPanel.Name = "statusesPanel";
            this.statusesPanel.Size = new System.Drawing.Size(884, 459);
            this.statusesPanel.TabIndex = 6;
            this.statusesPanel.Visible = false;
            // 
            // statusesDataGridView
            // 
            this.statusesDataGridView.AllowUserToAddRows = false;
            this.statusesDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.statusesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.statusesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statusesDataGridView.Location = new System.Drawing.Point(324, 12);
            this.statusesDataGridView.MultiSelect = false;
            this.statusesDataGridView.Name = "statusesDataGridView";
            this.statusesDataGridView.ReadOnly = true;
            this.statusesDataGridView.RowHeadersVisible = false;
            this.statusesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.statusesDataGridView.Size = new System.Drawing.Size(548, 435);
            this.statusesDataGridView.TabIndex = 0;
            this.statusesDataGridView.SelectionChanged += new System.EventHandler(this.statusesDataGridView_SelectionChanged);
            // 
            // statusNameLabel
            // 
            this.statusNameLabel.AutoSize = true;
            this.statusNameLabel.Location = new System.Drawing.Point(12, 15);
            this.statusNameLabel.Name = "statusNameLabel";
            this.statusNameLabel.Size = new System.Drawing.Size(60, 13);
            this.statusNameLabel.TabIndex = 1;
            this.statusNameLabel.Text = "Название:";
            // 
            // statusNameTextBox
            // 
            this.statusNameTextBox.Location = new System.Drawing.Point(12, 31);
            this.statusNameTextBox.Name = "statusNameTextBox";
            this.statusNameTextBox.Size = new System.Drawing.Size(300, 20);
            this.statusNameTextBox.TabIndex = 2;
            // 
            // statusDescLabel
            // 
            this.statusDescLabel.AutoSize = true;
            this.statusDescLabel.Location = new System.Drawing.Point(12, 65);
            this.statusDescLabel.Name = "statusDescLabel";
            this.statusDescLabel.Size = new System.Drawing.Size(60, 13);
            this.statusDescLabel.TabIndex = 3;
            this.statusDescLabel.Text = "Описание:";
            // 
            // statusDescTextBox
            // 
            this.statusDescTextBox.Location = new System.Drawing.Point(12, 81);
            this.statusDescTextBox.Multiline = true;
            this.statusDescTextBox.Name = "statusDescTextBox";
            this.statusDescTextBox.Size = new System.Drawing.Size(300, 80);
            this.statusDescTextBox.TabIndex = 4;
            // 
            // addStatusButton
            // 
            this.addStatusButton.Location = new System.Drawing.Point(12, 180);
            this.addStatusButton.Name = "addStatusButton";
            this.addStatusButton.Size = new System.Drawing.Size(90, 30);
            this.addStatusButton.TabIndex = 5;
            this.addStatusButton.Text = "Добавить";
            this.addStatusButton.UseVisualStyleBackColor = true;
            this.addStatusButton.Click += new System.EventHandler(this.addStatusButton_Click);
            // 
            // editStatusButton
            // 
            this.editStatusButton.Location = new System.Drawing.Point(108, 180);
            this.editStatusButton.Name = "editStatusButton";
            this.editStatusButton.Size = new System.Drawing.Size(100, 30);
            this.editStatusButton.TabIndex = 6;
            this.editStatusButton.Text = "Редактировать";
            this.editStatusButton.UseVisualStyleBackColor = true;
            this.editStatusButton.Click += new System.EventHandler(this.editStatusButton_Click);
            // 
            // deleteStatusButton
            // 
            this.deleteStatusButton.Location = new System.Drawing.Point(214, 180);
            this.deleteStatusButton.Name = "deleteStatusButton";
            this.deleteStatusButton.Size = new System.Drawing.Size(90, 30);
            this.deleteStatusButton.TabIndex = 7;
            this.deleteStatusButton.Text = "Удалить";
            this.deleteStatusButton.UseVisualStyleBackColor = true;
            this.deleteStatusButton.Click += new System.EventHandler(this.deleteStatusButton_Click);
            // 
            // rolesPanel
            // 
            this.rolesPanel.Controls.Add(this.rolesDataGridView);
            this.rolesPanel.Controls.Add(this.roleNameLabel);
            this.rolesPanel.Controls.Add(this.roleNameTextBox);
            this.rolesPanel.Controls.Add(this.roleDescLabel);
            this.rolesPanel.Controls.Add(this.roleDescTextBox);
            this.rolesPanel.Controls.Add(this.addRoleButton);
            this.rolesPanel.Controls.Add(this.editRoleButton);
            this.rolesPanel.Controls.Add(this.deleteRoleButton);
            this.rolesPanel.Location = new System.Drawing.Point(0, 102);
            this.rolesPanel.Name = "rolesPanel";
            this.rolesPanel.Size = new System.Drawing.Size(884, 459);
            this.rolesPanel.TabIndex = 7;
            this.rolesPanel.Visible = false;
            // 
            // rolesDataGridView
            // 
            this.rolesDataGridView.AllowUserToAddRows = false;
            this.rolesDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.rolesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.rolesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rolesDataGridView.Location = new System.Drawing.Point(324, 12);
            this.rolesDataGridView.MultiSelect = false;
            this.rolesDataGridView.Name = "rolesDataGridView";
            this.rolesDataGridView.ReadOnly = true;
            this.rolesDataGridView.RowHeadersVisible = false;
            this.rolesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.rolesDataGridView.Size = new System.Drawing.Size(548, 435);
            this.rolesDataGridView.TabIndex = 0;
            this.rolesDataGridView.SelectionChanged += new System.EventHandler(this.rolesDataGridView_SelectionChanged);
            // 
            // roleNameLabel
            // 
            this.roleNameLabel.AutoSize = true;
            this.roleNameLabel.Location = new System.Drawing.Point(12, 15);
            this.roleNameLabel.Name = "roleNameLabel";
            this.roleNameLabel.Size = new System.Drawing.Size(60, 13);
            this.roleNameLabel.TabIndex = 1;
            this.roleNameLabel.Text = "Название:";
            // 
            // roleNameTextBox
            // 
            this.roleNameTextBox.Location = new System.Drawing.Point(12, 31);
            this.roleNameTextBox.Name = "roleNameTextBox";
            this.roleNameTextBox.Size = new System.Drawing.Size(300, 20);
            this.roleNameTextBox.TabIndex = 2;
            // 
            // roleDescLabel
            // 
            this.roleDescLabel.AutoSize = true;
            this.roleDescLabel.Location = new System.Drawing.Point(12, 65);
            this.roleDescLabel.Name = "roleDescLabel";
            this.roleDescLabel.Size = new System.Drawing.Size(60, 13);
            this.roleDescLabel.TabIndex = 3;
            this.roleDescLabel.Text = "Описание:";
            // 
            // roleDescTextBox
            // 
            this.roleDescTextBox.Location = new System.Drawing.Point(12, 81);
            this.roleDescTextBox.Multiline = true;
            this.roleDescTextBox.Name = "roleDescTextBox";
            this.roleDescTextBox.Size = new System.Drawing.Size(300, 80);
            this.roleDescTextBox.TabIndex = 4;
            // 
            // addRoleButton
            // 
            this.addRoleButton.Location = new System.Drawing.Point(12, 180);
            this.addRoleButton.Name = "addRoleButton";
            this.addRoleButton.Size = new System.Drawing.Size(90, 30);
            this.addRoleButton.TabIndex = 5;
            this.addRoleButton.Text = "Добавить";
            this.addRoleButton.UseVisualStyleBackColor = true;
            this.addRoleButton.Click += new System.EventHandler(this.addRoleButton_Click);
            // 
            // editRoleButton
            // 
            this.editRoleButton.Location = new System.Drawing.Point(108, 180);
            this.editRoleButton.Name = "editRoleButton";
            this.editRoleButton.Size = new System.Drawing.Size(100, 30);
            this.editRoleButton.TabIndex = 6;
            this.editRoleButton.Text = "Редактировать";
            this.editRoleButton.UseVisualStyleBackColor = true;
            this.editRoleButton.Click += new System.EventHandler(this.editRoleButton_Click);
            // 
            // deleteRoleButton
            // 
            this.deleteRoleButton.Location = new System.Drawing.Point(214, 180);
            this.deleteRoleButton.Name = "deleteRoleButton";
            this.deleteRoleButton.Size = new System.Drawing.Size(90, 30);
            this.deleteRoleButton.TabIndex = 7;
            this.deleteRoleButton.Text = "Удалить";
            this.deleteRoleButton.UseVisualStyleBackColor = true;
            this.deleteRoleButton.Click += new System.EventHandler(this.deleteRoleButton_Click);
            // 
            // customTablesPanel
            // 
            this.customTablesPanel.Controls.Add(this.customTableDataGridView);
            this.customTablesPanel.Controls.Add(this.tablesListBox);
            this.customTablesPanel.Controls.Add(this.newTableLabel);
            this.customTablesPanel.Controls.Add(this.newTableNameTextBox);
            this.customTablesPanel.Controls.Add(this.createTableButton);
            this.customTablesPanel.Controls.Add(this.customNameLabel);
            this.customTablesPanel.Controls.Add(this.customNameTextBox);
            this.customTablesPanel.Controls.Add(this.customDescLabel);
            this.customTablesPanel.Controls.Add(this.customDescTextBox);
            this.customTablesPanel.Controls.Add(this.addToCustomTableButton);
            this.customTablesPanel.Controls.Add(this.deleteFromCustomTableButton);
            this.customTablesPanel.Controls.Add(this.tablesListLabel);
            this.customTablesPanel.Location = new System.Drawing.Point(0, 102);
            this.customTablesPanel.Name = "customTablesPanel";
            this.customTablesPanel.Size = new System.Drawing.Size(884, 459);
            this.customTablesPanel.TabIndex = 8;
            this.customTablesPanel.Visible = false;
            // 
            // customTableDataGridView
            // 
            this.customTableDataGridView.AllowUserToAddRows = false;
            this.customTableDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customTableDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.customTableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customTableDataGridView.Location = new System.Drawing.Point(324, 12);
            this.customTableDataGridView.MultiSelect = false;
            this.customTableDataGridView.Name = "customTableDataGridView";
            this.customTableDataGridView.ReadOnly = true;
            this.customTableDataGridView.RowHeadersVisible = false;
            this.customTableDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customTableDataGridView.Size = new System.Drawing.Size(548, 412);
            this.customTableDataGridView.TabIndex = 0;
            // 
            // tablesListBox
            // 
            this.tablesListBox.FormattingEnabled = true;
            this.tablesListBox.Location = new System.Drawing.Point(12, 31);
            this.tablesListBox.Name = "tablesListBox";
            this.tablesListBox.Size = new System.Drawing.Size(300, 95);
            this.tablesListBox.TabIndex = 1;
            this.tablesListBox.SelectedIndexChanged += new System.EventHandler(this.tablesListBox_SelectedIndexChanged);
            // 
            // newTableLabel
            // 
            this.newTableLabel.AutoSize = true;
            this.newTableLabel.Location = new System.Drawing.Point(12, 145);
            this.newTableLabel.Name = "newTableLabel";
            this.newTableLabel.Size = new System.Drawing.Size(106, 13);
            this.newTableLabel.TabIndex = 2;
            this.newTableLabel.Text = "Название таблицы:";
            // 
            // newTableNameTextBox
            // 
            this.newTableNameTextBox.Location = new System.Drawing.Point(12, 161);
            this.newTableNameTextBox.Name = "newTableNameTextBox";
            this.newTableNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.newTableNameTextBox.TabIndex = 3;
            // 
            // createTableButton
            // 
            this.createTableButton.BackColor = System.Drawing.Color.Coral;
            this.createTableButton.Location = new System.Drawing.Point(218, 159);
            this.createTableButton.Name = "createTableButton";
            this.createTableButton.Size = new System.Drawing.Size(94, 23);
            this.createTableButton.TabIndex = 4;
            this.createTableButton.Text = "Создать";
            this.createTableButton.UseVisualStyleBackColor = false;
            this.createTableButton.Click += new System.EventHandler(this.createTableButton_Click);
            // 
            // customNameLabel
            // 
            this.customNameLabel.AutoSize = true;
            this.customNameLabel.Location = new System.Drawing.Point(12, 195);
            this.customNameLabel.Name = "customNameLabel";
            this.customNameLabel.Size = new System.Drawing.Size(60, 13);
            this.customNameLabel.TabIndex = 5;
            this.customNameLabel.Text = "Название:";
            // 
            // customNameTextBox
            // 
            this.customNameTextBox.Location = new System.Drawing.Point(12, 211);
            this.customNameTextBox.Name = "customNameTextBox";
            this.customNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.customNameTextBox.TabIndex = 6;
            // 
            // customDescLabel
            // 
            this.customDescLabel.AutoSize = true;
            this.customDescLabel.Location = new System.Drawing.Point(12, 245);
            this.customDescLabel.Name = "customDescLabel";
            this.customDescLabel.Size = new System.Drawing.Size(60, 13);
            this.customDescLabel.TabIndex = 7;
            this.customDescLabel.Text = "Описание:";
            // 
            // customDescTextBox
            // 
            this.customDescTextBox.Location = new System.Drawing.Point(12, 261);
            this.customDescTextBox.Multiline = true;
            this.customDescTextBox.Name = "customDescTextBox";
            this.customDescTextBox.Size = new System.Drawing.Size(300, 80);
            this.customDescTextBox.TabIndex = 8;
            // 
            // addToCustomTableButton
            // 
            this.addToCustomTableButton.BackColor = System.Drawing.Color.Coral;
            this.addToCustomTableButton.Location = new System.Drawing.Point(12, 360);
            this.addToCustomTableButton.Name = "addToCustomTableButton";
            this.addToCustomTableButton.Size = new System.Drawing.Size(146, 30);
            this.addToCustomTableButton.TabIndex = 9;
            this.addToCustomTableButton.Text = "Добавить запись";
            this.addToCustomTableButton.UseVisualStyleBackColor = false;
            this.addToCustomTableButton.Click += new System.EventHandler(this.addToCustomTableButton_Click);
            // 
            // deleteFromCustomTableButton
            // 
            this.deleteFromCustomTableButton.BackColor = System.Drawing.Color.Coral;
            this.deleteFromCustomTableButton.Location = new System.Drawing.Point(164, 360);
            this.deleteFromCustomTableButton.Name = "deleteFromCustomTableButton";
            this.deleteFromCustomTableButton.Size = new System.Drawing.Size(148, 30);
            this.deleteFromCustomTableButton.TabIndex = 10;
            this.deleteFromCustomTableButton.Text = "Удалить запись";
            this.deleteFromCustomTableButton.UseVisualStyleBackColor = false;
            this.deleteFromCustomTableButton.Click += new System.EventHandler(this.deleteFromCustomTableButton_Click);
            // 
            // tablesListLabel
            // 
            this.tablesListLabel.AutoSize = true;
            this.tablesListLabel.Location = new System.Drawing.Point(12, 15);
            this.tablesListLabel.Name = "tablesListLabel";
            this.tablesListLabel.Size = new System.Drawing.Size(134, 13);
            this.tablesListLabel.TabIndex = 11;
            this.tablesListLabel.Text = "Существующие таблицы:";
            // 
            // btnExportAll
            // 
            this.btnExportAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportAll.BackColor = System.Drawing.Color.Coral;
            this.btnExportAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(107)))), ((int)(((byte)(60)))));
            this.btnExportAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExportAll.ForeColor = System.Drawing.Color.Black;
            this.btnExportAll.Location = new System.Drawing.Point(700, 568);
            this.btnExportAll.Name = "btnExportAll";
            this.btnExportAll.Size = new System.Drawing.Size(150, 30);
            this.btnExportAll.TabIndex = 20;
            this.btnExportAll.Text = "Экспорт всех таблиц";
            this.btnExportAll.UseVisualStyleBackColor = false;
            this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
            // 
            // ReferencesForm
            // 
            this.ClientSize = new System.Drawing.Size(884, 609);
            this.Controls.Add(this.customTablesPanel);
            this.Controls.Add(this.rolesPanel);
            this.Controls.Add(this.statusesPanel);
            this.Controls.Add(this.categoriesPanel);
            this.Controls.Add(this.customTablesButton);
            this.Controls.Add(this.rolesButton);
            this.Controls.Add(this.statusesButton);
            this.Controls.Add(this.categoriesButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnExportAll);
            this.MinimumSize = new System.Drawing.Size(900, 648);
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
            this.rolesPanel.ResumeLayout(false);
            this.rolesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rolesDataGridView)).EndInit();
            this.customTablesPanel.ResumeLayout(false);
            this.customTablesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customTableDataGridView)).EndInit();
            this.ResumeLayout(false);

        }
    }
}