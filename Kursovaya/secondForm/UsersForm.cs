using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class UsersForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool isEditMode = false;
        private int currentUserId = 0;

        public UsersForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeControls();
            LoadUsers();
            LoadRoles();
        }

        private void InitializeControls()
        {
            // Настройка DataGridView
            usersDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersDataGridView.ReadOnly = true;
            usersDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            usersDataGridView.GridColor = Color.LightGray;
            usersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Устанавливаем чередование цветов строк
            usersDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255); // Очень светлый фиолетовый

            // Цвет выделенной строки - очень светлый фиолетовый
            usersDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250); // Светлый фиолетовый для выделения
            usersDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Черный текст для контраста

            // Цвет заголовков
            usersDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80); // Coral цвет
            usersDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            usersDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            usersDataGridView.ColumnHeadersHeight = 40;
            usersDataGridView.EnableHeadersVisualStyles = false; // Отключаем стандартные стили Windows

            // Устанавливаем подсказки
            SetPlaceholderText(searchTextBox, "Поиск по логину или ФИО...");

            // Для поля пароля при создании пользователя
            SetPasswordPlaceholder(passwordTextBox, "Пароль (мин. 3 символа)");

            // Устанавливаем стиль
            ApplyCoralButtonStyle();

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

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80); // Coral цвет
            Color coralLightColor = Color.FromArgb(255, 147, 100); // Светлее для hover
            Color coralDarkColor = Color.FromArgb(235, 107, 60); // Темнее для нажатия

            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    ApplyButtonStyle(button, coralColor, coralLightColor, coralDarkColor);
                }
                else if (control is GroupBox groupBox)
                {
                    foreach (Control subControl in groupBox.Controls)
                    {
                        if (subControl is Button subButton)
                        {
                            ApplyButtonStyle(subButton, coralColor, coralLightColor, coralDarkColor);
                        }
                    }
                }
            }

            // Особый стиль для кнопки меню (можно сделать другого цвета)
            if (menuButton != null)
            {
                menuButton.BackColor = Color.Red; // Cornflower Blue
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

        private void SetPasswordPlaceholder(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
                textBox.UseSystemPasswordChar = false;
            }

            textBox.Enter += (s, e) => {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    textBox.UseSystemPasswordChar = true;
                }
            };

            textBox.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    textBox.UseSystemPasswordChar = false;
                }
            };
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            // При загрузке формы показываем поле пароля для создания нового пользователя
            labelPassword.Visible = true;
            passwordTextBox.Visible = true;
            passwordTextBox.Text = "Пароль (мин. 3 символа)";
            passwordTextBox.ForeColor = Color.Gray;
            passwordTextBox.UseSystemPasswordChar = false;
            resetPasswordButton.Visible = false;
        }

        private void LoadUsers()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT u.id, u.login, u.fio, r.name as role_name
                                    FROM users u 
                                    INNER JOIN roles r ON u.role_id = r.id 
                                    ORDER BY u.id";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adapter.Fill(dt);
                        usersDataGridView.DataSource = dt;

                        // Настройка заголовков
                        if (usersDataGridView.Columns.Count > 0)
                        {
                            usersDataGridView.Columns["id"].HeaderText = "ID";
                            usersDataGridView.Columns["login"].HeaderText = "Логин";
                            usersDataGridView.Columns["fio"].HeaderText = "ФИО";
                            usersDataGridView.Columns["role_name"].HeaderText = "Роль";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка",
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
                    string query = "SELECT id, name FROM roles ORDER BY name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        roleComboBox.Items.Clear();
                        while (reader.Read())
                        {
                            roleComboBox.Items.Add(new
                            {
                                Id = reader["id"],
                                Name = reader["name"].ToString()
                            });
                        }
                        roleComboBox.DisplayMember = "Name";
                        roleComboBox.ValueMember = "Id";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateUserInput(bool isAdding = false)
        {
            // Проверка ФИО
            if (string.IsNullOrEmpty(fioTextBox.Text) || fioTextBox.Text == "ФИО")
            {
                MessageBox.Show("Введите ФИО пользователя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(fioTextBox.Text, @"^[а-яА-ЯёЁ\s-]+$"))
            {
                MessageBox.Show("ФИО должно содержать только русские буквы, пробелы и дефисы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка логина
            if (string.IsNullOrEmpty(loginTextBox.Text) || loginTextBox.Text == "Логин")
            {
                MessageBox.Show("Введите логин пользователя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(loginTextBox.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (loginTextBox.Text.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка роли
            if (roleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль пользователя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка пароля при добавлении нового пользователя
            if (isAdding)
            {
                string password = passwordTextBox.Text;
                if (string.IsNullOrEmpty(password) || password == "Пароль (мин. 3 символа)")
                {
                    MessageBox.Show("Введите пароль для нового пользователя", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (password.Length < 3)
                {
                    MessageBox.Show("Пароль должен содержать минимум 3 символа", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Проверка пароля при редактировании (если пароль изменяется)
            if (!isAdding && !string.IsNullOrEmpty(passwordTextBox.Text) &&
                passwordTextBox.Text != "Пароль (оставьте пустым, чтобы не менять)")
            {
                if (passwordTextBox.Text.Length < 3)
                {
                    MessageBox.Show("Пароль должен содержать минимум 3 символа", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                UpdateUser();
                return;
            }

            if (!ValidateUserInput(true))
                return;

            try
            {
                string password = passwordTextBox.Text;
                dynamic selectedRole = roleComboBox.SelectedItem;

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    // Проверка на уникальность логина
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE login = @login";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@login", loginTextBox.Text);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = @"INSERT INTO users (login, password, fio, role_id) 
                                   VALUES (@login, @password, @fio, @role_id)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", loginTextBox.Text);
                        command.Parameters.AddWithValue("@password", HashPassword(password));
                        command.Parameters.AddWithValue("@fio", fioTextBox.Text);
                        command.Parameters.AddWithValue("@role_id", selectedRole.Id);

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show($"Пользователь {loginTextBox.Text} успешно добавлен.", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ClearForm();
                            LoadUsers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow selectedRow = usersDataGridView.SelectedRows[0];
            currentUserId = Convert.ToInt32(selectedRow.Cells["id"].Value);
            string login = selectedRow.Cells["login"].Value.ToString();

            // Проверка, что не редактируем администратора
            if (login == "admin")
            {
                MessageBox.Show("Нельзя редактировать системного администратора", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Заполняем форму данными пользователя
            fioTextBox.Text = selectedRow.Cells["fio"].Value.ToString();
            loginTextBox.Text = login;

            // Устанавливаем роль
            string roleName = selectedRow.Cells["role_name"].Value.ToString();
            foreach (var item in roleComboBox.Items)
            {
                dynamic roleItem = item;
                if (roleItem.Name == roleName)
                {
                    roleComboBox.SelectedItem = item;
                    break;
                }
            }

            // Изменяем подсказку для пароля
            passwordTextBox.Text = "Пароль (оставьте пустым, чтобы не менять)";
            passwordTextBox.ForeColor = Color.Gray;
            passwordTextBox.UseSystemPasswordChar = false;

            // Показываем кнопку сброса пароля
            resetPasswordButton.Visible = true;

            isEditMode = true;
            addButton.Text = "Сохранить";
        }

        private void UpdateUser()
        {
            if (!ValidateUserInput(false))
                return;

            try
            {
                dynamic selectedRole = roleComboBox.SelectedItem;
                string passwordHash = null;

                // Если пароль введен и это не подсказка, хэшируем его
                if (!string.IsNullOrEmpty(passwordTextBox.Text) &&
                    passwordTextBox.Text != "Пароль (оставьте пустым, чтобы не менять)" &&
                    passwordTextBox.Text != "Пароль (мин. 3 символа)")
                {
                    passwordHash = HashPassword(passwordTextBox.Text);
                }

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    // Проверка на уникальность логина (исключая текущего пользователя)
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE login = @login AND id != @id";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@login", loginTextBox.Text);
                        checkCommand.Parameters.AddWithValue("@id", currentUserId);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Обновление с паролем или без
                    string query;
                    if (!string.IsNullOrEmpty(passwordHash))
                    {
                        query = @"UPDATE users SET login = @login, fio = @fio, 
                               role_id = @role_id, password = @password
                               WHERE id = @id";
                    }
                    else
                    {
                        query = @"UPDATE users SET login = @login, fio = @fio, 
                               role_id = @role_id
                               WHERE id = @id";
                    }

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", loginTextBox.Text);
                        command.Parameters.AddWithValue("@fio", fioTextBox.Text);
                        command.Parameters.AddWithValue("@role_id", selectedRole.Id);
                        command.Parameters.AddWithValue("@id", currentUserId);

                        if (!string.IsNullOrEmpty(passwordHash))
                        {
                            command.Parameters.AddWithValue("@password", passwordHash);
                        }

                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Данные пользователя успешно обновлены", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ClearForm();
                            LoadUsers();
                            ResetFormMode();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId = Convert.ToInt32(usersDataGridView.SelectedRows[0].Cells["id"].Value);
            string login = usersDataGridView.SelectedRows[0].Cells["login"].Value.ToString();

            // Проверка, что не удаляем администратора
            if (login == "admin")
            {
                MessageBox.Show("Нельзя удалить системного администратора", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя {login}?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = dbHelper.GetConnection())
                    {
                        connection.Open();
                        string query = "DELETE FROM users WHERE id = @id";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", userId);
                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Пользователь успешно удален", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadUsers();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления пользователя: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void resetPasswordButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для сброса пароля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId = Convert.ToInt32(usersDataGridView.SelectedRows[0].Cells["id"].Value);
            string login = usersDataGridView.SelectedRows[0].Cells["login"].Value.ToString();

            // Создаем диалог для ввода нового пароля
            string newPassword = ShowPasswordInputDialog();
            if (string.IsNullOrEmpty(newPassword))
                return;

            if (newPassword.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать минимум 3 символа", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Сбросить пароль для пользователя {login}?", "Сброс пароля",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = dbHelper.GetConnection())
                    {
                        connection.Open();
                        string query = "UPDATE users SET password = @password WHERE id = @id";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@password", HashPassword(newPassword));
                            command.Parameters.AddWithValue("@id", userId);
                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show($"Пароль для пользователя {login} успешно сброшен.", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Обновляем поле пароля в форме
                                passwordTextBox.Text = "Пароль (оставьте пустым, чтобы не менять)";
                                passwordTextBox.ForeColor = Color.Gray;
                                passwordTextBox.UseSystemPasswordChar = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сброса пароля: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ShowPasswordInputDialog()
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Введите новый пароль",
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Новый пароль:", Width = 100 };
            TextBox textBox = new TextBox() { Left = 20, Top = 45, Width = 240, UseSystemPasswordChar = true };
            Label confirmLabel = new Label() { Left = 20, Top = 70, Text = "Подтвердите пароль:", Width = 120 };
            TextBox confirmTextBox = new TextBox() { Left = 20, Top = 95, Width = 240, UseSystemPasswordChar = true };
            Button confirmation = new Button() { Text = "OK", Left = 185, Width = 75, Top = 125, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    prompt.DialogResult = DialogResult.None;
                }
                else if (textBox.Text.Length < 3)
                {
                    MessageBox.Show("Пароль должен содержать минимум 3 символа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    prompt.DialogResult = DialogResult.None;
                }
                else if (textBox.Text != confirmTextBox.Text)
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    prompt.DialogResult = DialogResult.None;
                }
                else
                {
                    prompt.Close();
                }
            };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmTextBox);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(confirmLabel);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }

        private void ClearForm()
        {
            fioTextBox.Text = "ФИО";
            fioTextBox.ForeColor = Color.Gray;
            loginTextBox.Text = "Логин";
            loginTextBox.ForeColor = Color.Gray;
            passwordTextBox.Text = "Пароль (мин. 3 символа)";
            passwordTextBox.ForeColor = Color.Gray;
            passwordTextBox.UseSystemPasswordChar = false;
            roleComboBox.SelectedIndex = -1;

            // Скрываем кнопку сброса пароля
            resetPasswordButton.Visible = false;
        }

        private void ResetFormMode()
        {
            isEditMode = false;
            currentUserId = 0;
            addButton.Text = "Добавить";

            // Возвращаем стандартное состояние поля пароля
            passwordTextBox.Text = "Пароль (мин. 3 символа)";
            passwordTextBox.ForeColor = Color.Gray;
            passwordTextBox.UseSystemPasswordChar = false;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (searchText == "Поиск по логину или ФИО...")
                return;

            if (usersDataGridView.DataSource is System.Data.DataTable dt)
            {
                dt.DefaultView.RowFilter = $"login LIKE '%{searchText}%' OR fio LIKE '%{searchText}%'";
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.DataSource is System.Data.DataTable dt)
            {
                dt.DefaultView.Sort = "fio ASC";
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "Поиск по логину или ФИО...";
            searchTextBox.ForeColor = Color.Gray;

            if (usersDataGridView.DataSource is System.Data.DataTable dt)
            {
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.Sort = "";
            }
        }

        // Валидация ввода
        private void fioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только русские буквы, пробел и дефис
            if (!char.IsControl(e.KeyChar) && !Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ\s-]"))
            {
                e.Handled = true;
            }
        }

        private void loginTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только английские буквы и цифры
            if (!char.IsControl(e.KeyChar) && !Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9]"))
            {
                e.Handled = true;
            }
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем все символы для пароля
            // Можно добавить ограничения, если нужно
        }

        private void usersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Этот метод может быть оставлен, если нужна синхронизация с выбранной строкой
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void roleComboBox_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}