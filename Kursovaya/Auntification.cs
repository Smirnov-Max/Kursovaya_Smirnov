using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.IO;

namespace Kursovaya
{
    public partial class Auntification : Form
    {
        int AuthAtt = 0;
        string ConnStr = ConnectionDB.GetConnectionString();
      
        public Auntification()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            // Проверка длины логина и пароля
            if (loginTextBox.Text.Trim().Length < 3 || pwdTextBox.Text.Trim().Length < 3)
            {
                MessageBox.Show("Логин и пароль должны содержать минимум 5 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginTextBox.Text = "";
                pwdTextBox.Text = "";
                return;
            }

            // Проверка пользователя
            CheckUser();
        }

        private void CheckUser()
        {
            // Исправленный запрос - объединяем таблицы Users и Roles
            string query = @"SELECT u.password, r.name
                     FROM Users u 
                     INNER JOIN roles r ON u.role_id = r.ID 
                     WHERE u.login = @login;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@login", loginTextBox.Text.Trim());

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string pwd_db = reader["password"].ToString();
                            string role = reader["name"].ToString();
                            string actual_pwd = GetHashPwd(pwdTextBox.Text);

                            if (pwd_db == actual_pwd)
                            {
                                // Успешная авторизация
                                AuthAtt = 0; // Сбрасываем счетчик попыток

                                // Сохраняем информацию о пользователе
                                UserSession.CurrentUser = new UserInfo
                                {
                                    Login = loginTextBox.Text.Trim(),
                                    Role = role
                                };

                                // Открываем соответствующую форму в зависимости от роли
                                OpenRoleBasedForm(role);
                            }
                            else
                            {
                                HandleFailedAuth();
                            }
                        }
                        else
                        {
                            HandleFailedAuth();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearFields();
            }
        }

        private void HandleFailedAuth()
        {
            MessageBox.Show("Логин или пароль введены не верно!", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            AuthAtt++;
            ClearFields();
        }

        private void OpenRoleBasedForm(string role)
        {
            // Закрываем текущую форму авторизации
            this.Hide();

            switch (role.ToLower())
            {

                case "системный-администратор":
                case "1":
                    Administrator administrator = new Administrator();
                    administrator.Show();
                    break;

                case "менеджер":
                case "2":
                    Manager manager = new Manager();
                    manager.Show();
                    break;

                case "продавец-консультант":
                case "3":
                    Consultant consultant = new Consultant();
                    consultant.Show();
                    break;

                default:
                    MessageBox.Show($"Неизвестная роль: {role}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Show();
                    break;
            }
        }

        private string GetHashPwd(string pwd)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(pwd);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }

        private void ClearFields()
        {
            loginTextBox.Text = "";
            pwdTextBox.Text = "";
        }

        // Добавьте обработчик для кнопки Enter в текстовых полях
        private void LoginTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                pwdTextBox.Focus();
                e.Handled = true;
            }
        }

        private void PwdTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LogInButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    // Класс для хранения информации о текущем пользователе
    public static class UserSession
    {
        public static UserInfo CurrentUser { get; set; }
    }

    public class UserInfo
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public int UserID { get; set; }
    }
}