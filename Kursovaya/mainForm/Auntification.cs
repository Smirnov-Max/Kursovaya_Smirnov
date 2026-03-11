using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using Smirnov_kursovaya.mainForm;
using Smirnov_kursovaya.Database;
using System.Configuration;
using Smirnov_kursovaya.Helpers;
using Smirnov_kursovaya.secondForm;

namespace Smirnov_kursovaya.mainForm
{
    public partial class Authentication : Form
    {
        private DatabaseHelper dbHelper;

        // CAPTCHA: новые поля
        private int failedAttempts = 0;
        private string currentCaptcha;
        private bool isCaptchaVisible = false;
        private DateTime lockUntil = DateTime.MinValue;
        private Timer lockTimer;

        // Размеры формы
        private Size normalSize;
        private Size expandedSize;

        // Константа для количества попыток до появления капчи
        private const int MAX_ATTEMPTS_BEFORE_CAPTCHA = 3;

        public Authentication()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Запоминаем исходный размер формы (без капчи)
            normalSize = this.Size;

            // Рассчитываем расширенный размер (увеличенный на 80 пикселей по высоте)
            expandedSize = new Size(this.Width, this.Height + 80);

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

            // CAPTCHA: инициализация таймера и скрытие элементов
            InitializeCaptchaControls();

            // Применяем стиль после инициализации всех элементов
            this.Load += (s, e) => ApplyCoralButtonStyle();
        }

        public void ClearInputFields()
        {
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";

            // Сброс капчи
            if (txtCaptcha != null)
                txtCaptcha.Text = "";

            // Сброс счётчика попыток и скрытие капчи
            failedAttempts = 0;
            isCaptchaVisible = false;
            if (pictureBoxCaptcha != null)
            {
                pictureBoxCaptcha.Visible = false;
                txtCaptcha.Visible = false;
                btnRefreshCaptcha.Visible = false;
            }
        }

        // CAPTCHA: инициализация элементов управления капчи и таймера
        private void InitializeCaptchaControls()
        {
            // Элементы уже должны быть добавлены в дизайнере, но мы настроим их видимость
            pictureBoxCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            btnRefreshCaptcha.Visible = false;

            // Таймер для разблокировки
            lockTimer = new Timer();
            lockTimer.Interval = 1000; // проверка каждую секунду
            lockTimer.Tick += LockTimer_Tick;
        }

        // CAPTCHA: генерация новой капчи
        private void GenerateCaptcha()
        {
            pictureBoxCaptcha.Image = CaptchaGenerator.Generate(4, out currentCaptcha);
        }

        // CAPTCHA: обработчик таймера разблокировки
        private void LockTimer_Tick(object sender, EventArgs e)
        {
            if (lockUntil <= DateTime.Now)
            {
                lockTimer.Stop();
                // Включаем элементы управления обратно
                usernameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
                loginButton.Enabled = true;
                cancelButton.Enabled = true;

                // Если капча была видна, оставляем её
                if (isCaptchaVisible)
                {
                    pictureBoxCaptcha.Visible = true;
                    txtCaptcha.Visible = true;
                    btnRefreshCaptcha.Visible = true;

                    // Сообщаем пользователю о разблокировке
                    MessageBox.Show("Вход разблокирован. Введите капчу.", "Разблокировка",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Метод для отображения капчи (увеличивает форму)
        private void ShowCaptcha()
        {
            isCaptchaVisible = true;
            pictureBoxCaptcha.Visible = true;
            txtCaptcha.Visible = true;
            btnRefreshCaptcha.Visible = true;
            GenerateCaptcha();

            // Увеличиваем размер формы
            this.Size = expandedSize;
            // Центрируем форму после изменения размера
            this.CenterToScreen();

            // Очищаем поле ввода капчи
            txtCaptcha.Clear();
        }

        // Метод для скрытия капчи (возвращает исходный размер)
        private void HideCaptcha()
        {
            isCaptchaVisible = false;
            pictureBoxCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            btnRefreshCaptcha.Visible = false;
            txtCaptcha.Clear();

            // Возвращаем исходный размер
            this.Size = normalSize;
            this.CenterToScreen();
        }

        // Метод для блокировки входа
        private void LockLogin(int seconds)
        {
            lockUntil = DateTime.Now.AddSeconds(seconds);
            lockTimer.Start();

            // Отключаем элементы управления
            usernameTextBox.Enabled = false;
            passwordTextBox.Enabled = false;
            loginButton.Enabled = false;
            cancelButton.Enabled = false;
        }

        // Стилизация (как было в исходном проекте)
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

        // Основной обработчик входа
        private void loginButton_Click(object sender, EventArgs e)
        {
            // CAPTCHA: проверка блокировки
            if (lockUntil > DateTime.Now)
            {
                int secondsLeft = (int)(lockUntil - DateTime.Now).TotalSeconds;
                MessageBox.Show($"Система заблокирована. Подождите {secondsLeft} сек.", "Блокировка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            // Если введены admin/admin – открываем форму восстановления/импорта (без капчи)
            if (username == adminLogin && password == adminPass)
            {
                // Перед переходом убеждаемся, что капча скрыта и форма обычного размера
                HideCaptcha();
                failedAttempts = 0;

                DbRestoreImportForm restoreForm = new DbRestoreImportForm();
                restoreForm.Show();
                this.Hide();
                return;
            }

            // Если капча видима, проверяем её
            if (isCaptchaVisible)
            {
                if (string.IsNullOrEmpty(txtCaptcha.Text) || txtCaptcha.Text != currentCaptcha)
                {
                    // Неверная капча - блокировка на 10 секунд
                    MessageBox.Show("Неверная капча! Доступ заблокирован на 10 секунд.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Блокируем вход на 10 секунд
                    LockLogin(10);

                    // Генерируем новую капчу для следующей попытки после разблокировки
                    GenerateCaptcha();
                    txtCaptcha.Clear();
                    return;
                }
                // Если капча верна, продолжаем проверку логина/пароля
            }

            try
            {
                string hashedPassword = HashPassword(password);

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT u.*, r.name as RoleName FROM users u " +
                                   "INNER JOIN roles r ON u.role_id = r.id " +
                                   "WHERE u.login = @login AND u.password = @password";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", username);
                        command.Parameters.AddWithValue("@password", hashedPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Успешный вход
                                string role = reader["RoleName"].ToString();
                                string userFio = reader["fio"].ToString();

                                UserContext.CurrentUser = new UserInfo
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Login = reader["login"].ToString(),
                                    Fio = userFio,
                                    Role = role
                                };

                                // Сбрасываем счётчик и скрываем капчу
                                failedAttempts = 0;
                                HideCaptcha();

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
                                        MessageBox.Show("Неизвестная роль пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                }

                                this.Hide();
                            }
                            else
                            {
                                // Неудачная попытка входа (неверный логин/пароль)
                                failedAttempts++;

                                // Показываем сообщение о неверном логине/пароле
                                int remainingAttempts = MAX_ATTEMPTS_BEFORE_CAPTCHA - failedAttempts;

                                if (failedAttempts < MAX_ATTEMPTS_BEFORE_CAPTCHA)
                                {
                                    // До появления капчи остались попытки
                                    MessageBox.Show($"Неверный логин или пароль. Осталось попыток: {remainingAttempts}",
                                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else if (failedAttempts >= MAX_ATTEMPTS_BEFORE_CAPTCHA && !isCaptchaVisible)
                                {
                                    // Достигнут лимит попыток - показываем капчу
                                    ShowCaptcha();
                                    MessageBox.Show("Превышено количество попыток. Введите капчу.",
                                        "Требуется подтверждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                // Очищаем поля для новой попытки
                                passwordTextBox.Clear();
                                passwordTextBox.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CAPTCHA: обработчик кнопки обновления капчи
        private void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
            txtCaptcha.Clear();
        }

        // Метод для хеширования пароля SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
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