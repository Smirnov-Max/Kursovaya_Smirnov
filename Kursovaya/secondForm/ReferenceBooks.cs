using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ReferencesForm : Form
    {
        private DatabaseHelper dbHelper;

        public ReferencesForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeControls();
            LoadReferenceTables();
            this.Load += (s, e) => ApplyCoralButtonStyle();
        }

        private void InitializeControls()
        {
            ConfigureGrid(categoriesDataGridView);
            ConfigureGrid(statusesDataGridView);
            ShowPanel("categories");
            SetupResponsiveLayout();

            // Гарантируем, что нижняя панель кнопок всегда сверху Z-стека.
            if (bottomActionsPanel != null) bottomActionsPanel.BringToFront();
            if (panel1 != null) panel1.BringToFront();
        }

        private void SetupResponsiveLayout()
        {
            this.MinimumSize = new Size(916, 640);
            this.WindowState = FormWindowState.Maximized;

            // Главные панели должны тянуться по всему окну (под верхним меню),
            // но НЕ перекрывать нижнюю панель действий (Dock=Bottom).
            if (categoriesPanel != null)
                categoriesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            if (statusesPanel != null)
                statusesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            if (categoriesDataGridView != null)
                categoriesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            if (statusesDataGridView != null)
                statusesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Resize += (s, e) => AdjustReferencePanels();
            AdjustReferencePanels();
        }

        // Подгоняем размеры панелей категорий/статусов так, чтобы они
        // не перекрывали bottomActionsPanel (Dock=Bottom, высота 60).
        private void AdjustReferencePanels()
        {
            int topOffset = categoriesPanel != null ? categoriesPanel.Top : 102;
            int bottomReserve = bottomActionsPanel != null ? bottomActionsPanel.Height : 60;

            int availableHeight = this.ClientSize.Height - topOffset - bottomReserve;
            if (availableHeight < 200) availableHeight = 200;

            if (categoriesPanel != null)
            {
                categoriesPanel.Size = new Size(this.ClientSize.Width, availableHeight);
                int gridLeft = categoriesDataGridView != null ? categoriesDataGridView.Left : 330;
                int margin = 12;
                if (categoriesDataGridView != null)
                    categoriesDataGridView.Size = new Size(categoriesPanel.ClientSize.Width - gridLeft - margin,
                                                           categoriesPanel.ClientSize.Height - margin * 2);
            }
            if (statusesPanel != null)
            {
                statusesPanel.Size = new Size(this.ClientSize.Width, availableHeight);
                int gridLeft = statusesDataGridView != null ? statusesDataGridView.Left : 330;
                int margin = 12;
                if (statusesDataGridView != null)
                    statusesDataGridView.Size = new Size(statusesPanel.ClientSize.Width - gridLeft - margin,
                                                         statusesPanel.ClientSize.Height - margin * 2);
            }
        }

        private void ConfigureGrid(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.GridColor = Color.LightGray;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.EnableHeadersVisualStyles = false;
        }

        private void ApplyCoralButtonStyle()
        {
            Color coral = Color.FromArgb(255, 127, 80);
            Color coralLight = Color.FromArgb(255, 147, 100);
            Color coralDark = Color.FromArgb(235, 107, 60);
            ApplyStyleToAllButtons(this, coral, coralLight, coralDark);

            if (menuButton != null)
            {
                menuButton.BackColor = Color.Red;
                menuButton.FlatStyle = FlatStyle.Flat;
                menuButton.FlatAppearance.BorderColor = Color.DarkRed;
                menuButton.FlatAppearance.BorderSize = 1;
                menuButton.ForeColor = Color.Black;
                menuButton.MouseEnter += (s, e) => menuButton.BackColor = Color.IndianRed;
                menuButton.MouseLeave += (s, e) => menuButton.BackColor = Color.Red;
                menuButton.MouseDown += (s, e) => menuButton.BackColor = Color.OrangeRed;
                menuButton.MouseUp += (s, e) => menuButton.BackColor = Color.IndianRed;
            }
        }

        private void ApplyStyleToAllButtons(Control parent, Color normal, Color hover, Color pressed)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn && btn != menuButton)
                    ApplyButtonStyle(btn, normal, hover, pressed);
                else if (c.HasChildren)
                    ApplyStyleToAllButtons(c, normal, hover, pressed);
            }
        }

        private void ApplyButtonStyle(Button btn, Color normal, Color hover, Color pressed)
        {
            btn.BackColor = normal;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(235, 107, 60);
            btn.FlatAppearance.BorderSize = 1;
            btn.ForeColor = Color.Black;
            btn.MouseEnter += (s, e) => btn.BackColor = hover;
            btn.MouseLeave += (s, e) => btn.BackColor = normal;
            btn.MouseDown += (s, e) => btn.BackColor = pressed;
            btn.MouseUp += (s, e) => btn.BackColor = hover;
        }

        private void ReferencesForm_Load(object sender, EventArgs e) { }

        private void LoadReferenceTables()
        {
            LoadCategories();
            LoadStatuses();
        }

        private void LoadCategories()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, name FROM categories ORDER BY name";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        categoriesDataGridView.DataSource = dt;
                        if (categoriesDataGridView.Columns.Count > 0)
                        {
                            categoriesDataGridView.Columns["id"].Visible = false;
                            categoriesDataGridView.Columns["name"].HeaderText = "Название";
                        }
                    }
                }
                categoryCountLabel.Text = $"Записей: {categoriesDataGridView.RowCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatuses()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, name FROM statuses ORDER BY name";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        statusesDataGridView.DataSource = dt;
                        if (statusesDataGridView.Columns.Count > 0)
                        {
                            statusesDataGridView.Columns["id"].Visible = false;
                            statusesDataGridView.Columns["name"].HeaderText = "Название";
                        }
                    }
                }
                statusCountLabel.Text = $"Записей: {statusesDataGridView.RowCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== Категории =====
        private void addCategoryButton_Click(object sender, EventArgs e)
        {
            AddItem("categories", "категорию", categoryNameTextBox, LoadCategories);
        }
        private void editCategoryButton_Click(object sender, EventArgs e)
        {
            EditItem(categoriesDataGridView, "categories", "категорию", categoryNameTextBox, LoadCategories);
        }
        private void deleteCategoryButton_Click(object sender, EventArgs e)
        {
            DeleteItem(categoriesDataGridView, "categories", "категорию", LoadCategories);
        }

        // ===== Статусы =====
        private void addStatusButton_Click(object sender, EventArgs e)
        {
            AddItem("statuses", "статус", statusNameTextBox, LoadStatuses);
        }
        private void editStatusButton_Click(object sender, EventArgs e)
        {
            EditItem(statusesDataGridView, "statuses", "статус", statusNameTextBox, LoadStatuses);
        }
        private void deleteStatusButton_Click(object sender, EventArgs e)
        {
            DeleteItem(statusesDataGridView, "statuses", "статус", LoadStatuses);
        }

        private void AddItem(string tableName, string itemType, TextBox nameTextBox, Action loadMethod)
        {
            string name = nameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show($"Введите название ({itemType})", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show($"Добавить {itemType} \"{name}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string check = $"SELECT COUNT(*) FROM {tableName} WHERE name = @name";
                    using (var cmd = new MySqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Такое название уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = $"INSERT INTO {tableName} (name) VALUES (@name)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Запись добавлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Text = "";
                loadMethod();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditItem(DataGridView grid, string tableName, string itemType, TextBox nameTextBox, Action loadMethod)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["id"].Value);
            if (IsSystemReference(tableName, id))
            {
                MessageBox.Show("Нельзя редактировать системную запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = nameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите новое название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show($"Сохранить изменения для {itemType} \"{name}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string check = $"SELECT COUNT(*) FROM {tableName} WHERE name = @name AND id != @id";
                    using (var cmd = new MySqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@id", id);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Такое название уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = $"UPDATE {tableName} SET name = @name WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Запись обновлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Text = "";
                loadMethod();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteItem(DataGridView grid, string tableName, string itemType, Action loadMethod)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["id"].Value);
            string name = grid.SelectedRows[0].Cells["name"].Value.ToString();
            if (IsSystemReference(tableName, id))
            {
                MessageBox.Show("Нельзя удалять системную запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show($"Удалить запись \"{name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = $"DELETE FROM {tableName} WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Запись удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadMethod();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Проверяем, что текущий пользователь — системный администратор. Бекап и восстановление БД
        // должны быть доступны только ему.
        private bool IsCurrentUserAdmin()
        {
            var current = mainForm.UserContext.CurrentUser;
            if (current == null) return false;
            return current.Role == "Системный администратор";
        }

        private bool IsSystemReference(string tableName, int id)
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = $"SELECT name FROM {tableName} WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        string name = cmd.ExecuteScalar()?.ToString();
                        if (tableName == "roles")
                        {
                            string[] sysRoles = { "Системный администратор", "Менеджер", "Продавец-консультант" };
                            return Array.Exists(sysRoles, r => r == name);
                        }
                        else if (tableName == "statuses")
                        {
                            string[] sysStatuses = { "Новый", "В обработке", "Выполнен", "Отменен", "Принят", "Завершен", "Отменён" };
                            return Array.Exists(sysStatuses, s => s == name);
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        // ===== Экспорт CSV =====
        private void btnExportAll_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку для сохранения CSV-файлов";
                folderDialog.ShowNewFolderButton = true;
                if (folderDialog.ShowDialog() != DialogResult.OK) return;

                string folderPath = folderDialog.SelectedPath;
                var tables = dbHelper.GetTableList();
                int success = 0, failed = 0;
                var errors = new List<string>();

                foreach (string table in tables)
                {
                    try
                    {
                        ExportTableToCsv(table, Path.Combine(folderPath, table + ".csv"));
                        success++;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        errors.Add($"{table}: {ex.Message}");
                    }
                }

                string message = $"Экспорт завершён.\nУспешно: {success}\nОшибок: {failed}";
                if (errors.Count > 0) message += "\n\n" + string.Join("\n", errors);
                MessageBox.Show(message, "Экспорт", MessageBoxButtons.OK, errors.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
        }

        private void ExportTableToCsv(string tableName, string filePath)
        {
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                string query = $"SELECT * FROM {tableName}";
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sw.Write(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1) sw.Write(";");
                        }
                        sw.WriteLine();
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                string val = row[i] == DBNull.Value ? "" : row[i].ToString();
                                if (val.Contains(";") || val.Contains("\"") || val.Contains("\n"))
                                    val = "\"" + val.Replace("\"", "\"\"") + "\"";
                                sw.Write(val);
                                if (i < dt.Columns.Count - 1) sw.Write(";");
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }
        }

        // ===== Импорт CSV =====
        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv", Title = "Выберите CSV-файл" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;

                // Определяем таблицу по имени файла
                string fileName = Path.GetFileNameWithoutExtension(ofd.FileName).ToLower();
                string tableName = null;
                if (fileName.Contains("categor")) tableName = "categories";
                else if (fileName.Contains("status")) tableName = "statuses";
                else
                {
                    // Спросить пользователя
                    Form prompt = new Form { Width = 320, Height = 150, Text = "Выберите таблицу", StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.FixedDialog };
                    ComboBox cb = new ComboBox { Left = 20, Top = 20, Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
                    cb.Items.AddRange(new object[] { "categories", "statuses" });
                    cb.SelectedIndex = 0;
                    Button ok = new Button { Text = "OK", Left = 210, Top = 60, Width = 80, DialogResult = DialogResult.OK };
                    prompt.Controls.AddRange(new Control[] { cb, ok });
                    prompt.AcceptButton = ok;
                    if (prompt.ShowDialog() != DialogResult.OK) return;
                    tableName = cb.SelectedItem.ToString();
                }

                try
                {
                    int imported = ImportCsvToTable(tableName, ofd.FileName);
                    MessageBox.Show($"Импорт завершён. Добавлено записей: {imported}", "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReferenceTables();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int ImportCsvToTable(string tableName, string filePath)
        {
            int count = 0;
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 2) return 0;
            string[] headers = lines[0].Split(';');

            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    string[] values = lines[i].Split(';');
                    int nameIdx = -1;
                    for (int h = 0; h < headers.Length; h++)
                        if (headers[h].Trim().ToLower() == "name") { nameIdx = h; break; }
                    if (nameIdx < 0 || nameIdx >= values.Length) continue;
                    string name = values[nameIdx].Trim().Trim('"');
                    if (string.IsNullOrEmpty(name)) continue;

                    string check = $"SELECT COUNT(*) FROM {tableName} WHERE name = @name";
                    using (var cmd = new MySqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) continue;
                    }
                    string query = $"INSERT INTO {tableName} (name) VALUES (@name)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                        count++;
                    }
                }
            }
            return count;
        }

        // ===== Резервное копирование =====
        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (!IsCurrentUserAdmin())
            {
                MessageBox.Show("Резервное копирование доступно только системному администратору",
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (var sfd = new SaveFileDialog { Filter = "SQL files (*.sql)|*.sql", FileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql", Title = "Сохранить резервную копию" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    string backup = CreateDatabaseBackup();
                    File.WriteAllText(sfd.FileName, backup, Encoding.UTF8);
                    MessageBox.Show("Резервная копия успешно создана", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания резервной копии: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ===== Восстановление БД =====
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!IsCurrentUserAdmin())
            {
                MessageBox.Show("Восстановление БД доступно только системному администратору",
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (var ofd = new OpenFileDialog { Filter = "SQL files (*.sql)|*.sql", Title = "Выберите файл резервной копии" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;

                if (MessageBox.Show("Внимание! Это действие заменит текущие данные. Продолжить?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                try
                {
                    string script = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                    dbHelper.ExecuteScript(script);
                    MessageBox.Show("База данных успешно восстановлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReferenceTables();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка восстановления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ===== Выбор панели =====
        private void ShowPanel(string panelName)
        {
            categoriesPanel.Visible = false;
            statusesPanel.Visible = false;
            categoriesButton.BackColor = Color.FromArgb(255, 127, 80);
            statusesButton.BackColor = Color.FromArgb(255, 127, 80);

            switch (panelName)
            {
                case "categories":
                    categoriesPanel.Visible = true;
                    categoriesButton.BackColor = Color.FromArgb(220, 220, 220);
                    break;
                case "statuses":
                    statusesPanel.Visible = true;
                    statusesButton.BackColor = Color.FromArgb(220, 220, 220);
                    break;
            }

            // Гарантируем, что панель действий не закрыта вкладочными панелями.
            if (bottomActionsPanel != null) bottomActionsPanel.BringToFront();
            if (panel1 != null) panel1.BringToFront();
        }

        private void categoriesButton_Click(object sender, EventArgs e) => ShowPanel("categories");
        private void statusesButton_Click(object sender, EventArgs e) => ShowPanel("statuses");
        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        private void categoriesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (categoriesDataGridView.SelectedRows.Count > 0)
            {
                categoryNameTextBox.Text = categoriesDataGridView.SelectedRows[0].Cells["name"].Value.ToString();
                categoryNameTextBox.ForeColor = Color.Black;
            }
        }

        private void statusesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (statusesDataGridView.SelectedRows.Count > 0)
            {
                statusNameTextBox.Text = statusesDataGridView.SelectedRows[0].Cells["name"].Value.ToString();
                statusNameTextBox.ForeColor = Color.Black;
            }
        }

        // Создание резервной копии текущей БД средствами SQL: SHOW CREATE TABLE + INSERT'ы по всем таблицам.
        private string CreateDatabaseBackup()
        {
            var sb = new StringBuilder();
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                string dbName = conn.Database;

                sb.AppendLine($"-- Backup database: {dbName}");
                sb.AppendLine($"-- Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine("SET FOREIGN_KEY_CHECKS=0;");
                sb.AppendLine();

                var tables = new List<string>();
                using (var cmd = new MySqlCommand("SHOW TABLES", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) tables.Add(reader.GetString(0));
                }

                foreach (var table in tables)
                {
                    sb.AppendLine($"-- ----- Table: {table} -----");
                    sb.AppendLine($"DROP TABLE IF EXISTS `{table}`;");

                    string createSql = "";
                    using (var cmd = new MySqlCommand($"SHOW CREATE TABLE `{table}`", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) createSql = reader.GetString(1);
                    }
                    sb.AppendLine(createSql + ";");
                    sb.AppendLine();

                    using (var cmd = new MySqlCommand($"SELECT * FROM `{table}`", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) { sb.AppendLine(); continue; }

                        var cols = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++) cols.Add($"`{reader.GetName(i)}`");
                        string colList = string.Join(",", cols);

                        while (reader.Read())
                        {
                            var values = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.IsDBNull(i)) values.Add("NULL");
                                else
                                {
                                    object v = reader.GetValue(i);
                                    if (v is byte[] bytes)
                                        values.Add("0x" + BitConverter.ToString(bytes).Replace("-", ""));
                                    else if (v is DateTime dt)
                                        values.Add($"'{dt:yyyy-MM-dd HH:mm:ss}'");
                                    else if (v is bool b)
                                        values.Add(b ? "1" : "0");
                                    else if (v is int || v is long || v is short || v is decimal || v is double || v is float)
                                        values.Add(Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture));
                                    else
                                        values.Add("'" + v.ToString().Replace("\\", "\\\\").Replace("'", "''") + "'");
                                }
                            }
                            sb.AppendLine($"INSERT INTO `{table}` ({colList}) VALUES ({string.Join(",", values)});");
                        }
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("SET FOREIGN_KEY_CHECKS=1;");
            }
            return sb.ToString();
        }
    }
}
