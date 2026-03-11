using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using Smirnov_kursovaya.mainForm;
using Smirnov_kursovaya.Database;
using Kursovaya;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing;
using System.IO;
using System.Configuration;
using Smirnov_kursovaya.secondForm;

namespace Smirnov_kursovaya.mainForm
{
    public partial class Authentication : Form
    {
        private DatabaseHelper dbHelper;

        public Authentication()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Установка иконки
            string iconPath = Path.Combine(Application.StartupPath, "10252815.ico");
            if (File.Exists(iconPath))
            {
                this.Icon = new Icon(iconPath);
            }

            // Тестируем подключение к БД
            if (!dbHelper.TestConnection())
            {
                MessageBox.Show("Не удалось подключиться к базе данных. Приложение будет закрыто.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // Применяем стиль после инициализации всех элементов
            this.Load += (s, e) => ApplyCoralButtonStyle();
        }

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80); // Coral цвет
            Color coralLightColor = Color.FromArgb(255, 147, 100); // Светлее для hover
            Color coralDarkColor = Color.FromArgb(235, 107, 60); // Темнее для нажатия

            // Рекурсивно применяем стиль ко всем кнопкам
            ApplyStyleToAllButtons(this, coralColor, coralLightColor, coralDarkColor);

            // Особый стиль для кнопки выхода (красная)
            if (cancelButton != null)
            {
                ApplyCancelButtonStyle();
            }
        }

        private void ApplyStyleToAllButtons(Control parent, Color normalColor, Color hoverColor, Color pressedColor)
        {
            foreach (Control control in parent.Controls)
            {
                // Если это кнопка и не кнопка выхода - применяем стиль
                if (control is Button button && button != cancelButton)
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

        private void ApplyCancelButtonStyle()
        {
            cancelButton.BackColor = Color.Red;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.FlatAppearance.BorderColor = Color.DarkRed;
            cancelButton.FlatAppearance.BorderSize = 1;
            cancelButton.ForeColor = Color.Black;
            cancelButton.Font = new Font(cancelButton.Font, FontStyle.Regular);

            // Убираем старые обработчики и добавляем новые
            cancelButton.MouseEnter -= (s, e) => { };
            cancelButton.MouseLeave -= (s, e) => { };
            cancelButton.MouseDown -= (s, e) => { };
            cancelButton.MouseUp -= (s, e) => { };

            cancelButton.MouseEnter += (s, e) => {
                cancelButton.BackColor = Color.IndianRed;
            };
            cancelButton.MouseLeave += (s, e) => {
                cancelButton.BackColor = Color.Red;
            };
            cancelButton.MouseDown += (s, e) => {
                cancelButton.BackColor = Color.OrangeRed;
            };
            cancelButton.MouseUp += (s, e) => {
                cancelButton.BackColor = Color.OrangeRed;
            };
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Читаем учётные данные администратора из App.config
            string adminLogin = ConfigurationManager.AppSettings["AdminLogin"];
            string adminPass = ConfigurationManager.AppSettings["AdminPassword"];

            // Если введены admin/admin – открываем форму восстановления/импорта
            if (username == adminLogin && password == adminPass)
            {
                DbRestoreImportForm restoreForm = new DbRestoreImportForm();
                restoreForm.Show();
                this.Hide();
                return;
            }

            // Далее стандартная авторизация через БД
            try
            {
                string hashedPassword = HashPassword(password);
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT u.*, r.name as RoleName 
                             FROM users u 
                             INNER JOIN roles r ON u.role_id = r.id 
                             WHERE u.login = @login AND u.password = @password";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", username);
                        command.Parameters.AddWithValue("@password", hashedPassword);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["RoleName"].ToString();
                                string userFio = reader["fio"].ToString();

                                UserContext.CurrentUser = new UserInfo
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Login = reader["login"].ToString(),
                                    Fio = userFio,
                                    Role = role
                                };

                                switch (role)
                                {
                                    case "Системный администратор":
                                        Administrator adminForm = new Administrator();
                                        adminForm.Show();
                                        break;
                                    case "Менеджер":
                                        Manager managerForm = new Manager();
                                        managerForm.Show();
                                        break;
                                    case "Продавец-консультант":
                                        Seller sellerForm = new Seller();
                                        sellerForm.Show();
                                        break;
                                    default:
                                        MessageBox.Show("Неизвестная роль пользователя", "Ошибка",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                }
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                usernameTextBox.Clear();
                                passwordTextBox.Clear();
                                usernameTextBox.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для хеширования пароля SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Вычисляем хеш - получаем массив байтов
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Преобразуем массив байтов в строку hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Authentication_Load(object sender, EventArgs e)
        {
            usernameTextBox.Focus();
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginButton_Click(sender, e);
            }
        }
    }

    // Класс для хранения информации о текущем пользователе
    public static class UserContext
    {
        public static UserInfo CurrentUser { get; set; }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Fio { get; set; }
        public string Role { get; set; }
    }
}