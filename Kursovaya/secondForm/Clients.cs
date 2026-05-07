using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ClientsForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool isEditMode = false;
        private int currentClientId = 0;

        public ClientsForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeControls();
            LoadClients();
        }

        private void InitializeControls()
        {
            // Устанавливаем подсказки
            SetPlaceholderText(searchTextBox, "Введите телефон...");

            // Настройка DataGridView
            clientsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            clientsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            clientsDataGridView.ReadOnly = true;
            clientsDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            clientsDataGridView.GridColor = Color.LightGray;
            clientsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Устанавливаем чередование цветов строк
            clientsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255); // Очень светлый фиолетовый

            // Цвет выделенной строки - очень светлый фиолетовый
            clientsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250); // Светлый фиолетовый для выделения
            clientsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Черный текст для контраста

            // Цвет заголовков
            clientsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80); // Coral цвет
            clientsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            clientsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            clientsDataGridView.ColumnHeadersHeight = 40;
            clientsDataGridView.EnableHeadersVisualStyles = false; // Отключаем стандартные стили Windows

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

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            // Настройка маски телефона
            phoneTextBox.TextChanged += PhoneTextBox_TextChanged;
        }

        private void LoadClients()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT id, fio, phone 
                                    FROM clients 
                                    ORDER BY fio";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adapter.Fill(dt);
                        clientsDataGridView.DataSource = dt;

                        // Настройка заголовков
                        if (clientsDataGridView.Columns.Count > 0)
                        {
                            clientsDataGridView.Columns["id"].HeaderText = "ID";
                            clientsDataGridView.Columns["fio"].HeaderText = "ФИО";
                            clientsDataGridView.Columns["phone"].HeaderText = "Телефон";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateClientInput()
        {
            if (string.IsNullOrEmpty(fioTextBox.Text) || fioTextBox.Text == "ФИО")
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(phoneTextBox.Text) || phoneTextBox.Text == "Телефон")
            {
                MessageBox.Show("Введите телефон клиента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка ФИО на русские буквы
            if (!Regex.IsMatch(fioTextBox.Text, @"^[а-яА-ЯёЁ\s-]+$"))
            {
                MessageBox.Show("ФИО должно содержать только русские буквы, пробелы и дефисы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Проверка телефона
            string phone = phoneTextBox.Text.Replace("+7", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("Телефон должен содержать 10 цифр", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!ValidateClientInput())
                return;

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    // Проверка на дублирование телефона
                    string checkQuery = "SELECT COUNT(*) FROM clients WHERE phone = @phone";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@phone", phoneTextBox.Text);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Клиент с таким номером телефона уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = @"INSERT INTO clients (fio, phone) 
                                   VALUES (@fio, @phone)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fio", fioTextBox.Text);
                        command.Parameters.AddWithValue("@phone", phoneTextBox.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Клиент успешно добавлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearForm();
                        LoadClients();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления клиента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow selectedRow = clientsDataGridView.SelectedRows[0];
            currentClientId = Convert.ToInt32(selectedRow.Cells["id"].Value);

            // Заполняем форму данными клиента
            fioTextBox.Text = selectedRow.Cells["fio"].Value.ToString();
            phoneTextBox.Text = selectedRow.Cells["phone"].Value.ToString();

            isEditMode = true;
            addButton.Text = "Сохранить";
            addButton.Click -= addButton_Click;
            addButton.Click += updateButton_Click;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (!ValidateClientInput())
                return;

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    // Проверка на дублирование телефона (исключая текущего клиента)
                    string checkQuery = "SELECT COUNT(*) FROM clients WHERE phone = @phone AND id != @id";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@phone", phoneTextBox.Text);
                        checkCommand.Parameters.AddWithValue("@id", currentClientId);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Клиент с таким номером телефона уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = @"UPDATE clients SET fio = @fio, phone = @phone 
                                   WHERE id = @id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fio", fioTextBox.Text);
                        command.Parameters.AddWithValue("@phone", phoneTextBox.Text);
                        command.Parameters.AddWithValue("@id", currentClientId);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Данные клиента обновлены", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearForm();
                        LoadClients();
                        ResetFormMode();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления клиента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int clientId = Convert.ToInt32(clientsDataGridView.SelectedRows[0].Cells["id"].Value);
            string clientName = clientsDataGridView.SelectedRows[0].Cells["fio"].Value.ToString();

            if (MessageBox.Show($"Вы уверены, что хотите удалить клиента {clientName}?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = dbHelper.GetConnection())
                    {
                        connection.Open();

                        // Проверка на наличие заказов у клиента
                        string checkQuery = "SELECT COUNT(*) FROM orders WHERE client_id = @client_id";
                        using (var checkCommand = new MySqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@client_id", clientId);
                            int orderCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (orderCount > 0)
                            {
                                MessageBox.Show("Нельзя удалить клиента, у которого есть заказы", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        string deleteQuery = "DELETE FROM clients WHERE id = @id";
                        using (var command = new MySqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@id", clientId);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Клиент удален", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadClients();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления клиента: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            fioTextBox.Text = "ФИО";
            fioTextBox.ForeColor = Color.Gray;
            phoneTextBox.Text = "Телефон";
            phoneTextBox.ForeColor = Color.Gray;
        }

        private void ResetFormMode()
        {
            isEditMode = false;
            currentClientId = 0;
            addButton.Text = "Добавить";
            addButton.Click -= updateButton_Click;
            addButton.Click += addButton_Click;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (searchText == "Введите телефон...")
                return;

            if (clientsDataGridView.DataSource is System.Data.DataTable dt)
            {
                dt.DefaultView.RowFilter = $"phone LIKE '%{searchText}%' OR fio LIKE '%{searchText}%'";
            }
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.DataSource is System.Data.DataTable dt)
            {
                dt.DefaultView.Sort = "fio ASC";
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "Введите телефон...";
            searchTextBox.ForeColor = Color.Gray;

            if (clientsDataGridView.DataSource is System.Data.DataTable dt)
            {
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.Sort = "";
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void phoneTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры, +, (, ), - и управляющие символы
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '+' && e.KeyChar != '(' && e.KeyChar != ')' && e.KeyChar != '-' && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void PhoneTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(phoneTextBox.Text) || phoneTextBox.Text == "Телефон")
                return;

            string phone = phoneTextBox.Text.Replace("+7", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (phone.Length > 10)
            {
                phone = phone.Substring(0, 10);
            }

            if (phone.Length == 10)
            {
                phoneTextBox.Text = $"+7 ({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 2)}-{phone.Substring(8, 2)}";
                phoneTextBox.SelectionStart = phoneTextBox.Text.Length;
            }
        }

        private void clientsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isEditMode && clientsDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = clientsDataGridView.SelectedRows[0];
                currentClientId = Convert.ToInt32(selectedRow.Cells["id"].Value);
                fioTextBox.Text = selectedRow.Cells["fio"].Value.ToString();
                phoneTextBox.Text = selectedRow.Cells["phone"].Value.ToString();
            }
        }
    }
}