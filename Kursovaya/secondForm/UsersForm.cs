using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
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
        private bool passwordVisible = false;

        // ===== Переключение раскладки на русскую =====
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void SwitchToRussianLayout()
        {
            try
            {
                IntPtr russianLayout = LoadKeyboardLayout("00000419", 1);
                PostMessage(GetForegroundWindow(), 0x0050, IntPtr.Zero, russianLayout);
            }
            catch { }
        }

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
            ConfigureDataGridView(usersDataGridView);
            ApplyCoralButtonStyle();
            SetupResponsiveLayout();
        }

        private void SetupResponsiveLayout()
        {
            this.MinimumSize = new Size(900, 600);
            this.WindowState = FormWindowState.Maximized;

            if (usersDataGridView != null)
                usersDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void ConfigureDataGridView(DataGridView dgv)
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

        private void UsersForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
            resetPasswordButton.Visible = false;
        }

        private void LoadUsers()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    // Порядок столбцов: Логин, ФИО, Роль (ID скрыт)
                    string query = @"SELECT u.id, u.login, u.fio, r.name AS role_name
                                    FROM users u
                                    INNER JOIN roles r ON u.role_id = r.id
                                    ORDER BY u.fio";
                    using (var cmd = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        usersDataGridView.DataSource = dt;

                        if (usersDataGridView.Columns.Count > 0)
                        {
                            usersDataGridView.Columns["id"].Visible = false;
                            usersDataGridView.Columns["login"].HeaderText = "Логин";
                            usersDataGridView.Columns["login"].DisplayIndex = 0;
                            usersDataGridView.Columns["fio"].HeaderText = "ФИО";
                            usersDataGridView.Columns["fio"].DisplayIndex = 1;
                            usersDataGridView.Columns["role_name"].HeaderText = "Роль";
                            usersDataGridView.Columns["role_name"].DisplayIndex = 2;
                        }
                    }
                }
                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRecordCount()
        {
            int total = usersDataGridView.RowCount;
            recordCountLabel.Text = $"Записей: {total}";
        }

        private void LoadRoles()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT id, name FROM roles ORDER BY name";
                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        roleComboBox.Items.Clear();
                        while (reader.Read())
                        {
                            roleComboBox.Items.Add(new { Id = reader["id"], Name = reader["name"].ToString() });
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

        private bool ValidateUserInput(bool isAdding)
        {
            string fio = fioTextBox.Text.Trim();
            if (string.IsNullOrEmpty(fio))
            {
                MessageBox.Show("Введите ФИО пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(fio, @"^[а-яА-ЯёЁ\s-]+$"))
            {
                MessageBox.Show("ФИО должно содержать только русские буквы, пробелы и дефисы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string login = loginTextBox.Text.Trim();
            if (string.IsNullOrEmpty(login) || login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (roleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (isAdding)
            {
                string pwd = passwordTextBox.Text;
                if (string.IsNullOrEmpty(pwd) || pwd.Length < 3)
                {
                    MessageBox.Show("Введите пароль (минимум 3 символа)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                string pwd = passwordTextBox.Text;
                if (!string.IsNullOrEmpty(pwd) && pwd.Length < 3)
                {
                    MessageBox.Show("Пароль должен содержать минимум 3 символа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (isEditMode) { UpdateUser(); return; }
            if (!ValidateUserInput(true)) return;
            try
            {
                dynamic selectedRole = roleComboBox.SelectedItem;
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE login = @login";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@login", loginTextBox.Text.Trim());
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = "INSERT INTO users (login, password, fio, role_id) VALUES (@login, @password, @fio, @role_id)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@login", loginTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", HashPassword(passwordTextBox.Text));
                        cmd.Parameters.AddWithValue("@fio", fioTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@role_id", selectedRole.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Пользователь успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataGridViewRow row = usersDataGridView.SelectedRows[0];
            currentUserId = Convert.ToInt32(row.Cells["id"].Value);
            string login = row.Cells["login"].Value.ToString();
            if (login == "admin")
            {
                MessageBox.Show("Нельзя редактировать системного администратора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fioTextBox.Text = row.Cells["fio"].Value.ToString();
            fioTextBox.ForeColor = Color.Black;
            loginTextBox.Text = login;
            loginTextBox.ForeColor = Color.Black;
            string roleName = row.Cells["role_name"].Value.ToString();
            foreach (var item in roleComboBox.Items)
            {
                dynamic roleItem = item;
                if (roleItem.Name == roleName) { roleComboBox.SelectedItem = item; break; }
            }
            passwordTextBox.Text = "";
            passwordTextBox.UseSystemPasswordChar = true;
            resetPasswordButton.Visible = true;
            isEditMode = true;
            addButton.Text = "Сохранить";
        }

        private void UpdateUser()
        {
            if (!ValidateUserInput(false)) return;
            try
            {
                dynamic selectedRole = roleComboBox.SelectedItem;
                string passwordHash = null;
                if (!string.IsNullOrEmpty(passwordTextBox.Text))
                    passwordHash = HashPassword(passwordTextBox.Text);

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE login = @login AND id != @id";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@login", loginTextBox.Text.Trim());
                        checkCmd.Parameters.AddWithValue("@id", currentUserId);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = passwordHash != null
                        ? "UPDATE users SET login=@login, fio=@fio, role_id=@role_id, password=@password WHERE id=@id"
                        : "UPDATE users SET login=@login, fio=@fio, role_id=@role_id WHERE id=@id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@login", loginTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@fio", fioTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@role_id", selectedRole.Id);
                        cmd.Parameters.AddWithValue("@id", currentUserId);
                        if (passwordHash != null) cmd.Parameters.AddWithValue("@password", passwordHash);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные пользователя обновлены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUsers();
                ResetFormMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int userId = Convert.ToInt32(usersDataGridView.SelectedRows[0].Cells["id"].Value);
            string login = usersDataGridView.SelectedRows[0].Cells["login"].Value.ToString();
            if (login == "admin")
            {
                MessageBox.Show("Нельзя удалить системного администратора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show($"Удалить пользователя {login}?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM users WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Пользователь удалён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resetPasswordButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count == 0) return;
            int userId = Convert.ToInt32(usersDataGridView.SelectedRows[0].Cells["id"].Value);
            string login = usersDataGridView.SelectedRows[0].Cells["login"].Value.ToString();
            string newPassword = ShowPasswordInputDialog();
            if (string.IsNullOrEmpty(newPassword)) return;
            if (newPassword.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать минимум 3 символа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show($"Сбросить пароль для пользователя {login}?", "Сброс пароля", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE users SET password = @password WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@password", HashPassword(newPassword));
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Пароль успешно сброшен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                passwordTextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сброса пароля: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ShowPasswordInputDialog()
        {
            Form prompt = new Form
            {
                Width = 320,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Введите новый пароль",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };
            Label lbl1 = new Label { Left = 20, Top = 20, Text = "Новый пароль:", Width = 130 };
            TextBox tb1 = new TextBox { Left = 20, Top = 40, Width = 260, UseSystemPasswordChar = true };
            Label lbl2 = new Label { Left = 20, Top = 70, Text = "Подтвердите пароль:", Width = 130 };
            TextBox tb2 = new TextBox { Left = 20, Top = 90, Width = 260, UseSystemPasswordChar = true };
            Button ok = new Button { Text = "OK", Left = 185, Width = 95, Top = 130, DialogResult = DialogResult.OK };
            ok.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(tb1.Text) || tb1.Text.Length < 3)
                { MessageBox.Show("Пароль минимум 3 символа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); prompt.DialogResult = DialogResult.None; }
                else if (tb1.Text != tb2.Text)
                { MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); prompt.DialogResult = DialogResult.None; }
                else { prompt.Close(); }
            };
            prompt.Controls.AddRange(new Control[] { lbl1, tb1, lbl2, tb2, ok });
            prompt.AcceptButton = ok;
            return prompt.ShowDialog() == DialogResult.OK ? tb1.Text : null;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var sb = new System.Text.StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void ClearForm()
        {
            fioTextBox.Text = "";
            loginTextBox.Text = "";
            passwordTextBox.Text = "";
            passwordTextBox.UseSystemPasswordChar = true;
            roleComboBox.SelectedIndex = -1;
            resetPasswordButton.Visible = false;
        }

        private void ResetFormMode()
        {
            isEditMode = false;
            currentUserId = 0;
            addButton.Text = "Добавить";
        }

        // ===== Поиск =====
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string txt = searchTextBox.Text.Trim();
            if (usersDataGridView.DataSource is DataTable dt)
                dt.DefaultView.RowFilter = string.IsNullOrEmpty(txt) ? "" : $"login LIKE '%{txt}%' OR fio LIKE '%{txt}%'";
            UpdateRecordCount();
        }

        // ===== Сортировка по ФИО =====
        private void sortButton_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.DataSource is DataTable dt)
                dt.DefaultView.Sort = dt.DefaultView.Sort == "fio ASC" ? "fio DESC" : "fio ASC";
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "";
            if (usersDataGridView.DataSource is DataTable dt)
            {
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.Sort = "";
            }
            UpdateRecordCount();
        }

        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        // ===== Кнопка "глаз" для пароля =====
        private void showPasswordButton_Click(object sender, EventArgs e)
        {
            passwordVisible = !passwordVisible;
            passwordTextBox.UseSystemPasswordChar = !passwordVisible;
            showPasswordButton.Text = passwordVisible ? "🙈" : "👁";
        }

        // ===== Валидация ФИО: русские буквы, заглавная первая буква слова, не более 2 пробелов =====
        private void fioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            // Только русские буквы, пробел, дефис
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ\s-]"))
            {
                e.Handled = true;
                return;
            }
            // Не более 2 пробелов
            if (e.KeyChar == ' ')
            {
                int spaces = 0;
                foreach (char c in fioTextBox.Text) if (c == ' ') spaces++;
                if (spaces >= 2) { e.Handled = true; return; }
            }
        }

        private void fioTextBox_Enter(object sender, EventArgs e)
        {
            SwitchToRussianLayout();
        }

        private void fioTextBox_TextChanged(object sender, EventArgs e)
        {
            // Автоматическая заглавная буква в начале каждого слова
            TextBox tb = fioTextBox;
            int sel = tb.SelectionStart;
            string text = tb.Text;
            if (string.IsNullOrEmpty(text)) return;

            char[] chars = text.ToCharArray();
            bool makeUpper = true;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ' ' || chars[i] == '-') { makeUpper = true; }
                else if (makeUpper)
                {
                    chars[i] = char.ToUpper(chars[i]);
                    makeUpper = false;
                }
            }
            string newText = new string(chars);
            if (newText != text)
            {
                tb.TextChanged -= fioTextBox_TextChanged;
                tb.Text = newText;
                tb.SelectionStart = Math.Min(sel, tb.Text.Length);
                tb.TextChanged += fioTextBox_TextChanged;
            }
        }

        private void loginTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9]"))
                e.Handled = true;
        }

        private void usersDataGridView_SelectionChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void roleComboBox_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}