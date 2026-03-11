namespace Smirnov_kursovaya.secondForm
{
    partial class DbRestoreImportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.GroupBox grpRestore;
        private System.Windows.Forms.Label lblScript;
        private System.Windows.Forms.TextBox txtScriptPath;
        private System.Windows.Forms.Button btnBrowseScript;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.GroupBox grpImport;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog;

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
            this.grpRestore = new System.Windows.Forms.GroupBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnBrowseScript = new System.Windows.Forms.Button();
            this.txtScriptPath = new System.Windows.Forms.TextBox();
            this.lblScript = new System.Windows.Forms.Label();
            this.grpImport = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();

            this.panel1.SuspendLayout();
            this.grpRestore.SuspendLayout();
            this.grpImport.SuspendLayout();
            this.SuspendLayout();

            // panel1
            this.panel1.BackColor = System.Drawing.Color.DarkSalmon;
            this.panel1.Controls.Add(this.menuButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 60);
            this.panel1.TabIndex = 0;

            // menuButton
            this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.menuButton.BackColor = System.Drawing.Color.Coral;
            this.menuButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuButton.ForeColor = System.Drawing.Color.Black;
            this.menuButton.Location = new System.Drawing.Point(572, 15);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(100, 30);
            this.menuButton.TabIndex = 1;
            this.menuButton.Text = "Меню";
            this.menuButton.UseVisualStyleBackColor = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);

            // label1
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(552, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Восстановление и импорт данных";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // grpRestore
            this.grpRestore.Controls.Add(this.btnRestore);
            this.grpRestore.Controls.Add(this.btnBrowseScript);
            this.grpRestore.Controls.Add(this.txtScriptPath);
            this.grpRestore.Controls.Add(this.lblScript);
            this.grpRestore.Location = new System.Drawing.Point(12, 66);
            this.grpRestore.Name = "grpRestore";
            this.grpRestore.Size = new System.Drawing.Size(660, 100);
            this.grpRestore.TabIndex = 1;
            this.grpRestore.TabStop = false;
            this.grpRestore.Text = "Восстановление структуры БД";

            // lblScript
            this.lblScript.AutoSize = true;
            this.lblScript.Location = new System.Drawing.Point(10, 30);
            this.lblScript.Name = "lblScript";
            this.lblScript.Size = new System.Drawing.Size(80, 13);
            this.lblScript.TabIndex = 0;
            this.lblScript.Text = "Файл скрипта:";

            // txtScriptPath
            this.txtScriptPath.Location = new System.Drawing.Point(100, 27);
            this.txtScriptPath.Name = "txtScriptPath";
            this.txtScriptPath.Size = new System.Drawing.Size(400, 20);
            this.txtScriptPath.TabIndex = 1;
            this.txtScriptPath.Text = "CreateTables.sql";

            // btnBrowseScript
            this.btnBrowseScript.BackColor = System.Drawing.Color.Coral;
            this.btnBrowseScript.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBrowseScript.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseScript.Location = new System.Drawing.Point(510, 25);
            this.btnBrowseScript.Name = "btnBrowseScript";
            this.btnBrowseScript.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseScript.TabIndex = 2;
            this.btnBrowseScript.Text = "Обзор...";
            this.btnBrowseScript.UseVisualStyleBackColor = false;
            this.btnBrowseScript.Click += new System.EventHandler(this.btnBrowseScript_Click);

            // btnRestore
            this.btnRestore.BackColor = System.Drawing.Color.Coral;
            this.btnRestore.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Location = new System.Drawing.Point(100, 60);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(200, 30);
            this.btnRestore.TabIndex = 3;
            this.btnRestore.Text = "Восстановить структуру";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);

            // grpImport
            this.grpImport.Controls.Add(this.btnImport);
            this.grpImport.Controls.Add(this.cmbTables);
            this.grpImport.Controls.Add(this.lblTable);
            this.grpImport.Location = new System.Drawing.Point(12, 172);
            this.grpImport.Name = "grpImport";
            this.grpImport.Size = new System.Drawing.Size(660, 100);
            this.grpImport.TabIndex = 2;
            this.grpImport.TabStop = false;
            this.grpImport.Text = "Импорт данных из CSV";

            // lblTable
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(10, 30);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(91, 13);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Выберите таблицу:";

            // cmbTables
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(110, 27);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(200, 21);
            this.cmbTables.TabIndex = 1;

            // btnImport
            this.btnImport.BackColor = System.Drawing.Color.Coral;
            this.btnImport.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Location = new System.Drawing.Point(110, 60);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(150, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Импортировать CSV";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 285);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(89, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Готов к работе";

            // openFileDialog
            this.openFileDialog.Filter = "SQL files (*.sql)|*.sql|CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            // DbRestoreImportForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpImport);
            this.Controls.Add(this.grpRestore);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(700, 350);
            this.Name = "DbRestoreImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Восстановление и импорт данных";
            this.Load += new System.EventHandler(this.DbRestoreImportForm_Load);
            this.panel1.ResumeLayout(false);
            this.grpRestore.ResumeLayout(false);
            this.grpRestore.PerformLayout();
            this.grpImport.ResumeLayout(false);
            this.grpImport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}