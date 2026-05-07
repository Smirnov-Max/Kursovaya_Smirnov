using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ReferencesForm : Form
    {
        private DatabaseHelper dbHelper;
        private string currentTable = "";
        private Panel currentPanel;

        public ReferencesForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeControls();
            LoadReferenceTables();

            // Применяем стиль после инициализации всех элементов
            this.Load += (s, e) => ApplyCoralButtonStyle();
        }

        private void InitializeControls()
        {
            // Настройка DataGridView
            categoriesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            categoriesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            categoriesDataGridView.ReadOnly = true;
            categoriesDataGridView.RowHeadersVisible = false;

            statusesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            statusesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            statusesDataGridView.ReadOnly = true;
            statusesDataGridView.RowHeadersVisible = false;

            rolesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            rolesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            rolesDataGridView.ReadOnly = true;
            rolesDataGridView.RowHeadersVisible = false;

            customTableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            customTableDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customTableDataGridView.ReadOnly = true;
            customTableDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            customTableDataGridView.GridColor = Color.LightGray;
            customTableDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Подсказки для текстовых полей
            SetPlaceholderText(categoryNameTextBox, "Название категории");
            SetPlaceholderText(categoryDescTextBox, "Описание категории");
            SetPlaceholderText(statusNameTextBox, "Название статуса");
            SetPlaceholderText(statusDescTextBox, "Описание статуса");
            SetPlaceholderText(roleNameTextBox, "Название роли");
            SetPlaceholderText(roleDescTextBox, "Описание роли");
            SetPlaceholderText(newTableNameTextBox, "Имя новой таблицы");
            SetPlaceholderText(customNameTextBox, "Название записи");
            SetPlaceholderText(customDescTextBox, "Описание записи");

            // Изначально показываем панель категорий
            ShowPanel("categories");

            foreach (Control control in this.Controls)
            {
                if (control is DataGridView dgv)
                {
                    dgv.DataBindingComplete += (s, e) => {
                        if (dgv.Columns.Contains("id"))
                        {
                            dgv.Columns["id"].Visible = false;
                        }
                    };
                }
            }
        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку для сохранения CSV-файлов";
                folderDialog.ShowNewFolderButton = true;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderDialog.SelectedPath;
                    ExportAllTablesToCsv(folderPath);
                }
            }
        }

        /// <summary>
        /// Экспортирует все таблицы из базы данных в CSV-файлы.
        /// Каждая таблица сохраняется в отдельный файл с именем <имя_таблицы>.csv.
        /// </summary>
        private void ExportAllTablesToCsv(string folderPath)
        {
            var tables = dbHelper.GetTableList();
            int success = 0;
            int failed = 0;
            var errors = new List<string>();

            foreach (string table in tables)
            {
                try
                {
                    string filePath = Path.Combine(folderPath, table + ".csv");
                    ExportTableToCsv(table, filePath);
                    success++;
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add($"Ошибка экспорта таблицы {table}: {ex.Message}");
                }
            }

            string message = $"Экспорт завершен.\nУспешно экспортировано таблиц: {success}\nОшибок: {failed}";
            if (errors.Count > 0)
            {
                message += "\n\nДетали ошибок:\n" + string.Join("\n", errors);
            }
            MessageBox.Show(message, "Результат экспорта", MessageBoxButtons.OK, errors.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        /// <summary>
        /// Экспортирует одну таблицу в CSV-файл.
        /// </summary>
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
                        // Запись заголовков столбцов
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sw.Write(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1)
                                sw.Write(";");
                        }
                        sw.WriteLine();

                        // Запись строк данных
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                object value = row[i];
                                string strValue = value == DBNull.Value ? "" : value.ToString();

                                // Экранирование, если значение содержит разделитель или кавычки
                                if (strValue.Contains(";") || strValue.Contains("\"") || strValue.Contains("\n"))
                                {
                                    strValue = "\"" + strValue.Replace("\"", "\"\"") + "\"";
                                }

                                sw.Write(strValue);
                                if (i < dt.Columns.Count - 1)
                                    sw.Write(";");
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }
        }

        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
            }

            textBox.Enter += (s, e) => {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80); // Coral цвет
            Color coralLightColor = Color.FromArgb(255, 147, 100); // Светлее для hover
            Color coralDarkColor = Color.FromArgb(235, 107, 60); // Темнее для нажатия

            // Рекурсивно применяем стиль ко всем кнопкам
            ApplyStyleToAllButtons(this, coralColor, coralLightColor, coralDarkColor);

            // Особый стиль для кнопки меню (красная)
            if (menuButton != null)
            {
                ApplyMenuButtonStyle();
            }
        }

        private void ApplyStyleToAllButtons(Control parent, Color normalColor, Color hoverColor, Color pressedColor)
        {
            foreach (Control control in parent.Controls)
            {
                // Если это кнопка и не кнопка меню - применяем стиль
                if (control is Button button && button != menuButton)
                {
                    ApplyButtonStyle(button, normalColor, hoverColor, pressedColor);
                }
                // Если это контейнер - рекурсивно обрабатываем его содержимое
                else if (control.HasChildren)
                {
                    ApplyStyleToAllButtons(control, normalColor, hoverColor, pressedColor);
                }
            }
        }

        private void ApplyButtonStyle(Button button, Color normalColor, Color hoverColor, Color pressedColor)
        {
            button.BackColor = normalColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(235, 107, 60);
            button.FlatAppearance.BorderSize = 1;
            button.ForeColor = Color.Black;
            button.Font = new Font(button.Font, FontStyle.Regular);

            // Убираем старые обработчики
            button.MouseEnter -= (s, e) => { };
            button.MouseLeave -= (s, e) => { };
            button.MouseDown -= (s, e) => { };
            button.MouseUp -= (s, e) => { };

            // Добавляем новые обработчики
            button.MouseEnter += (s, e) => {
                button.BackColor = hoverColor;
            };
            button.MouseLeave += (s, e) => {
                button.BackColor = normalColor;
            };
            button.MouseDown += (s, e) => {
                button.BackColor = pressedColor;
            };
            button.MouseUp += (s, e) => {
                button.BackColor = hoverColor;
            };
        }

        private void ApplyMenuButtonStyle()
        {
            menuButton.BackColor = Color.Red;
            menuButton.FlatStyle = FlatStyle.Flat;
            menuButton.FlatAppearance.BorderColor = Color.DarkRed;
            menuButton.FlatAppearance.BorderSize = 1;
            menuButton.ForeColor = Color.Black;
            menuButton.Font = new Font(menuButton.Font, FontStyle.Regular);

            // Убираем старые обработчики и добавляем новые
            menuButton.MouseEnter -= (s, e) => { };
            menuButton.MouseLeave -= (s, e) => { };
            menuButton.MouseDown -= (s, e) => { };
            menuButton.MouseUp -= (s, e) => { };

            menuButton.MouseEnter += (s, e) => {
                menuButton.BackColor = Color.IndianRed;
            };
            menuButton.MouseLeave += (s, e) => {
                menuButton.BackColor = Color.Red;
            };
            menuButton.MouseDown += (s, e) => {
                menuButton.BackColor = Color.OrangeRed;
            };
            menuButton.MouseUp += (s, e) => {
                menuButton.BackColor = Color.OrangeRed;
            };
        }

        private void ReferencesForm_Load(object sender, EventArgs e)
        {
            LoadTablesToList();
        }

        private void LoadReferenceTables()
        {
            LoadCategories();
            LoadStatuses();
            LoadRoles();
        }

        private void LoadTablesToList()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SHOW TABLES";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        tablesListBox.Items.Clear();
                        while (reader.Read())
                        {
                            tablesListBox.Items.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблиц: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT id, name, description FROM categories ORDER BY name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        categoriesDataGridView.DataSource = dt;

                        if (categoriesDataGridView.Columns.Count > 0)
                        {
                            categoriesDataGridView.Columns["id"].HeaderText = "ID";
                            categoriesDataGridView.Columns["name"].HeaderText = "Название";
                            categoriesDataGridView.Columns["description"].HeaderText = "Описание";
                            categoriesDataGridView.RowHeadersVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatuses()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT id, name, description FROM statuses ORDER BY name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        statusesDataGridView.DataSource = dt;

                        if (statusesDataGridView.Columns.Count > 0)
                        {
                            statusesDataGridView.Columns["id"].HeaderText = "ID";
                            statusesDataGridView.Columns["name"].HeaderText = "Название";
                            statusesDataGridView.Columns["description"].HeaderText = "Описание";
                            statusesDataGridView.RowHeadersVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT id, name, description FROM roles ORDER BY name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        rolesDataGridView.DataSource = dt;

                        if (rolesDataGridView.Columns.Count > 0)
                        {
                            rolesDataGridView.Columns["id"].HeaderText = "ID";
                            rolesDataGridView.Columns["name"].HeaderText = "Название";
                            rolesDataGridView.Columns["description"].HeaderText = "Описание";
                            rolesDataGridView.RowHeadersVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addCategoryButton_Click(object sender, EventArgs e)
        {
            AddReferenceItem("категорию", "categories", categoryNameTextBox, categoryDescTextBox, LoadCategories);
        }

        private void editCategoryButton_Click(object sender, EventArgs e)
        {
            EditReferenceItem(categoriesDataGridView, "категорию", "categories", categoryNameTextBox, categoryDescTextBox, LoadCategories);
        }

        private void deleteCategoryButton_Click(object sender, EventArgs e)
        {
            DeleteReferenceItem(categoriesDataGridView, "категорию", "categories", LoadCategories);
        }

        private void addStatusButton_Click(object sender, EventArgs e)
        {
            AddReferenceItem("статус", "statuses", statusNameTextBox, statusDescTextBox, LoadStatuses);
        }

        private void editStatusButton_Click(object sender, EventArgs e)
        {
            EditReferenceItem(statusesDataGridView, "статус", "statuses", statusNameTextBox, statusDescTextBox, LoadStatuses);
        }

        private void deleteStatusButton_Click(object sender, EventArgs e)
        {
            DeleteReferenceItem(statusesDataGridView, "статус", "statuses", LoadStatuses);
        }

        private void addRoleButton_Click(object sender, EventArgs e)
        {
            AddReferenceItem("роль", "roles", roleNameTextBox, roleDescTextBox, LoadRoles);
        }

        private void editRoleButton_Click(object sender, EventArgs e)
        {
            EditReferenceItem(rolesDataGridView, "роль", "roles", roleNameTextBox, roleDescTextBox, LoadRoles);
        }

        private void deleteRoleButton_Click(object sender, EventArgs e)
        {
            DeleteReferenceItem(rolesDataGridView, "роль", "roles", LoadRoles);
        }

        private void AddReferenceItem(string itemType, string tableName, TextBox nameTextBox, TextBox descTextBox, Action loadMethod)
        {
            string name = nameTextBox.Text == "Название " + itemType ? "" : nameTextBox.Text;
            string description = descTextBox.Text == "Описание " + itemType ? "" : descTextBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show($"Введите название {itemType}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    // Проверка на уникальность
                    string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE name = @name";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@name", name);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show($"Такое название уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = $"INSERT INTO {tableName} (name, description) VALUES (@name, @description)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@description", description);
                        command.ExecuteNonQuery();
                        MessageBox.Show($"Запись добавлена", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        nameTextBox.Text = "Название " + itemType;
                        nameTextBox.ForeColor = Color.Gray;
                        descTextBox.Text = "Описание " + itemType;
                        descTextBox.ForeColor = Color.Gray;
                        loadMethod();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления {itemType}: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditReferenceItem(DataGridView grid, string itemType, string tableName, TextBox nameTextBox, TextBox descTextBox, Action loadMethod)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show($"Выберите запись для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["id"].Value);
            string oldName = grid.SelectedRows[0].Cells["name"].Value.ToString();

            // Проверка основных справочников
            if (IsSystemReference(tableName, id))
            {
                MessageBox.Show($"Нельзя редактировать системную запись", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = nameTextBox.Text == "Название " + itemType ? "" : nameTextBox.Text;
            string description = descTextBox.Text == "Описание " + itemType ? "" : descTextBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show($"Введите новое название записи", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE name = @name AND id != @id";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@name", name);
                        checkCommand.Parameters.AddWithValue("@id", id);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show($"Такое название уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = $"UPDATE {tableName} SET name = @name, description = @description WHERE id = @id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                        MessageBox.Show($"Запись обновлена", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        nameTextBox.Text = "Название " + itemType;
                        nameTextBox.ForeColor = Color.Gray;
                        descTextBox.Text = "Описание " + itemType;
                        descTextBox.ForeColor = Color.Gray;
                        loadMethod();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования записи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteReferenceItem(DataGridView grid, string itemType, string tableName, Action loadMethod)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show($"Выберите таблицу для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["id"].Value);
            string name = grid.SelectedRows[0].Cells["name"].Value.ToString();

            // Проверка основных справочников
            if (IsSystemReference(tableName, id))
            {
                MessageBox.Show($"Нельзя удалять системную запись", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Вы уверены, что хотите удалить запись \"{name}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = dbHelper.GetConnection())
                    {
                        connection.Open();
                        string query = $"DELETE FROM {tableName} WHERE id = @id";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                            MessageBox.Show($"Запись удалена", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadMethod();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления записи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsSystemReference(string tableName, int id)
        {
            // Проверка системных справочников
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    if (tableName == "roles")
                    {
                        string query = "SELECT name FROM roles WHERE id = @id";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            var name = command.ExecuteScalar()?.ToString();

                            // Системные роли, которые нельзя удалять/редактировать
                            string[] systemRoles = { "Системный администратор", "Менеджер", "Продавец-консультант" };
                            return Array.Exists(systemRoles, role => role == name);
                        }
                    }
                    else if (tableName == "statuses")
                    {
                        string query = "SELECT name FROM statuses WHERE id = @id";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            var name = command.ExecuteScalar()?.ToString();

                            // Системные статусы, которые нельзя удалять/редактировать
                            string[] systemStatuses = { "Новый", "В обработке", "Выполнен", "Отменен" };
                            return Array.Exists(systemStatuses, status => status == name);
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        private void categoriesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (categoriesDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = categoriesDataGridView.SelectedRows[0];
                categoryNameTextBox.Text = selectedRow.Cells["name"].Value.ToString();
                categoryNameTextBox.ForeColor = Color.Black;
                categoryDescTextBox.Text = selectedRow.Cells["description"].Value.ToString();
                categoryDescTextBox.ForeColor = Color.Black;
            }
        }

        private void statusesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (statusesDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = statusesDataGridView.SelectedRows[0];
                statusNameTextBox.Text = selectedRow.Cells["name"].Value.ToString();
                statusNameTextBox.ForeColor = Color.Black;
                statusDescTextBox.Text = selectedRow.Cells["description"].Value.ToString();
                statusDescTextBox.ForeColor = Color.Black;
            }
        }

        private void rolesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (rolesDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = rolesDataGridView.SelectedRows[0];
                roleNameTextBox.Text = selectedRow.Cells["name"].Value.ToString();
                roleNameTextBox.ForeColor = Color.Black;
                roleDescTextBox.Text = selectedRow.Cells["description"].Value.ToString();
                roleDescTextBox.ForeColor = Color.Black;
            }
        }

        private void createTableButton_Click(object sender, EventArgs e)
        {
            string tableName = newTableNameTextBox.Text == "Имя новой таблицы" ? "" : newTableNameTextBox.Text;

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Введите название таблицы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = $"CREATE TABLE {tableName} (" +
                                  "id INT AUTO_INCREMENT PRIMARY KEY, " +
                                  "name VARCHAR(255) NOT NULL, " +
                                  "description TEXT, " +
                                  "created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show($"Таблица {tableName} создана", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        newTableNameTextBox.Text = "Имя новой таблицы";
                        newTableNameTextBox.ForeColor = Color.Gray;
                        LoadTablesToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedItem != null)
            {
                currentTable = tablesListBox.SelectedItem.ToString();
                LoadTableData(currentTable);
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = $"SELECT * FROM {tableName}";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        customTableDataGridView.DataSource = dt;
                        customTableDataGridView.RowHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных таблицы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addToCustomTableButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentTable))
            {
                MessageBox.Show("Выберите таблицу", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = customNameTextBox.Text == "Название записи" ? "" : customNameTextBox.Text;
            string description = customDescTextBox.Text == "Описание записи" ? "" : customDescTextBox.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = $"INSERT INTO {currentTable} (name, description) VALUES (@name, @description)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@description", description);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Запись добавлена", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        customNameTextBox.Text = "Название записи";
                        customNameTextBox.ForeColor = Color.Gray;
                        customDescTextBox.Text = "Описание записи";
                        customDescTextBox.ForeColor = Color.Gray;
                        LoadTableData(currentTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления записи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteFromCustomTableButton_Click(object sender, EventArgs e)
        {
            if (customTableDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(customTableDataGridView.SelectedRows[0].Cells["id"].Value);
            string name = customTableDataGridView.SelectedRows[0].Cells["name"].Value.ToString();

            if (MessageBox.Show($"Вы уверены, что хотите удалить запись \"{name}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = dbHelper.GetConnection())
                    {
                        connection.Open();
                        string query = $"DELETE FROM {currentTable} WHERE id = @id";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Запись удалена", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTableData(currentTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления записи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowPanel(string panelName)
        {
            // Скрыть все панели
            categoriesPanel.Visible = false;
            statusesPanel.Visible = false;
            rolesPanel.Visible = false;
            customTablesPanel.Visible = false;

            // Сбросить выделение всех кнопок
            categoriesButton.BackColor = Color.FromArgb(255, 127, 80);
            statusesButton.BackColor = Color.FromArgb(255, 127, 80);
            rolesButton.BackColor = Color.FromArgb(255, 127, 80);
            customTablesButton.BackColor = Color.FromArgb(255, 127, 80);

            // Показать нужную панель
            switch (panelName)
            {
                case "categories":
                    categoriesPanel.Visible = true;
                    categoriesButton.BackColor = Color.FromArgb(220, 220, 220); // Серый для активной
                    currentPanel = categoriesPanel;
                    break;
                case "statuses":
                    statusesPanel.Visible = true;
                    statusesButton.BackColor = Color.FromArgb(220, 220, 220); // Серый для активной
                    currentPanel = statusesPanel;
                    break;
                case "roles":
                    rolesPanel.Visible = true;
                    rolesButton.BackColor = Color.FromArgb(220, 220, 220); // Серый для активной
                    currentPanel = rolesPanel;
                    break;
                case "custom":
                    customTablesPanel.Visible = true;
                    customTablesButton.BackColor = Color.FromArgb(220, 220, 220); // Серый для активной
                    currentPanel = customTablesPanel;
                    break;
            }
        }

        private void categoriesButton_Click(object sender, EventArgs e)
        {
            ShowPanel("categories");
        }

        private void statusesButton_Click(object sender, EventArgs e)
        {
            ShowPanel("statuses");
        }

        private void rolesButton_Click(object sender, EventArgs e)
        {
            ShowPanel("roles");
        }

        private void customTablesButton_Click(object sender, EventArgs e)
        {
            ShowPanel("custom");
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}