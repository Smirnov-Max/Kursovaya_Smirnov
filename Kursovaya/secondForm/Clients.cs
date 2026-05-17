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
    public partial class ClientsForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool isEditMode = false;
        private int currentClientId = 0;
        private bool phoneVisible = false;

        // Если форма открыта из заказа — вернуть клиента
        public int SelectedClientId { get; private set; } = 0;
        public string SelectedClientName { get; private set; } = "";
        public string SelectedClientPhone { get; private set; } = "";
        public bool OpenedFromOrder { get; set; } = false;

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

        public ClientsForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeControls();
            LoadClients();
        }

        private void InitializeControls()
        {
            ConfigureGrid(clientsDataGridView);
            ApplyCoralButtonStyle();

            // Видимость кнопок выбора задаётся в режиме «выбор клиента»
            addToOrderButton.Visible = OpenedFromOrder;
            cancelSelectionButton.Visible = OpenedFromOrder;

            SetupResponsiveLayout();
        }

        private void SetupResponsiveLayout()
        {
            this.MinimumSize = new Size(900, 600);
            this.WindowState = FormWindowState.Maximized;

            if (clientsDataGridView != null)
                clientsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
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

        // ===== Применение режима «выбор клиента» =====
        // Когда форма открыта из «Нового заказа» — скрываем удаление и кнопку «Меню»,
        // показываем «Подтвердить выбор» и «Отмена», меняем заголовок.
        private void ApplyOrderSelectionMode()
        {
            if (OpenedFromOrder)
            {
                this.Text = "Выбор клиента";
                if (label1 != null) label1.Text = "Выбор клиента";

                addToOrderButton.Visible = true;
                cancelSelectionButton.Visible = true;

                // В режиме выбора нельзя удалять и нечего открывать в меню
                deleteButton.Visible = false;
                menuButton.Visible = false;
            }
            else
            {
                addToOrderButton.Visible = false;
                cancelSelectionButton.Visible = false;
                deleteButton.Visible = true;
                menuButton.Visible = true;
            }
        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            ApplyOrderSelectionMode();
        }

        private void LoadClients()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, fio, phone FROM clients ORDER BY fio";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        clientsDataGridView.DataSource = dt;

                        if (clientsDataGridView.Columns.Count > 0)
                        {
                            clientsDataGridView.Columns["id"].Visible = false;
                            clientsDataGridView.Columns["fio"].HeaderText = "ФИО";
                            clientsDataGridView.Columns["fio"].DisplayIndex = 0;
                            clientsDataGridView.Columns["phone"].HeaderText = "Телефон";
                            clientsDataGridView.Columns["phone"].DisplayIndex = 1;
                        }
                    }
                }
                MaskPhoneColumn();
                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Маскируем телефон в таблице: +7(ХХХ)ХХХ-99-99
        private void MaskPhoneColumn()
        {
            if (clientsDataGridView.Columns.Contains("phone"))
            {
                clientsDataGridView.CellFormatting -= clientsDataGridView_CellFormatting;
                clientsDataGridView.CellFormatting += clientsDataGridView_CellFormatting;
            }
        }

        private void clientsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (clientsDataGridView.Columns[e.ColumnIndex].Name == "phone" && e.Value != null && !phoneVisible)
            {
                string phone = e.Value.ToString();
                // Формат: +7(ХХХ)ХХХ-99-99  → последние 2 цифры показываем, остальные маскируем
                if (phone.Length >= 11)
                {
                    string digits = Regex.Replace(phone, @"\D", "");
                    if (digits.Length >= 11)
                        e.Value = $"+7(ХХХ)ХХХ-{digits.Substring(9, 2)}";
                    else if (digits.Length >= 10)
                        e.Value = $"+7(ХХХ)ХХХ-{digits.Substring(8, 2)}";
                    else
                        e.Value = "+7(ХХХ)ХХХ-ХХ";
                }
                else
                    e.Value = "+7(ХХХ)ХХХ-ХХ";
                e.FormattingApplied = true;
            }
        }

        private void UpdateRecordCount()
        {
            recordCountLabel.Text = $"Записей: {clientsDataGridView.RowCount}";
        }

        private bool ValidateClientInput()
        {
            string fio = fioTextBox.Text.Trim();
            if (string.IsNullOrEmpty(fio))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Regex.IsMatch(fio, @"^[а-яА-ЯёЁ\s-]+$"))
            {
                MessageBox.Show("ФИО только русские буквы, пробелы, дефис", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string phone = GetCleanPhone(phoneTextBox.Text);
            if (phone.Length != 10)
            {
                MessageBox.Show("Телефон должен содержать 10 цифр после +7", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private string GetCleanPhone(string raw)
        {
            return Regex.Replace(raw, @"\D", "").TrimStart('7').TrimStart('8');
        }

        private string FormatPhone(string tenDigits)
        {
            if (tenDigits.Length == 10)
                return $"+7({tenDigits.Substring(0, 3)}){tenDigits.Substring(3, 3)}-{tenDigits.Substring(6, 2)}-{tenDigits.Substring(8, 2)}";
            return tenDigits;
        }

        // ===== Добавить клиента (первым — поля очищаются) =====
        private void addButton_Click(object sender, EventArgs e)
        {
            if (isEditMode) { UpdateClient(); return; }
            if (!ValidateClientInput()) return;

            string phone = FormatPhone(GetCleanPhone(phoneTextBox.Text));
            if (MessageBox.Show($"Добавить клиента '{fioTextBox.Text.Trim()}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string check = "SELECT COUNT(*) FROM clients WHERE phone = @phone";
                    using (var cmd = new MySqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phone);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Клиент с таким телефоном уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = "INSERT INTO clients (fio, phone) VALUES (@fio, @phone)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fio", fioTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Клиент добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataGridViewRow row = clientsDataGridView.SelectedRows[0];
            currentClientId = Convert.ToInt32(row.Cells["id"].Value);
            fioTextBox.Text = row.Cells["fio"].Value.ToString();
            fioTextBox.ForeColor = Color.Black;
            // Берём реальный телефон из БД
            string realPhone = GetRealPhone(currentClientId);
            phoneTextBox.Text = realPhone;
            phoneTextBox.ForeColor = Color.Black;
            isEditMode = true;
            addButton.Text = "Сохранить";
        }

        private string GetRealPhone(int clientId)
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string q = "SELECT phone FROM clients WHERE id = @id";
                    using (var cmd = new MySqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", clientId);
                        return cmd.ExecuteScalar()?.ToString() ?? "";
                    }
                }
            }
            catch { return ""; }
        }

        private void UpdateClient()
        {
            if (!ValidateClientInput()) return;
            string phone = FormatPhone(GetCleanPhone(phoneTextBox.Text));
            if (MessageBox.Show($"Сохранить изменения для клиента '{fioTextBox.Text.Trim()}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string check = "SELECT COUNT(*) FROM clients WHERE phone = @phone AND id != @id";
                    using (var cmd = new MySqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@id", currentClientId);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Телефон уже используется другим клиентом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = "UPDATE clients SET fio = @fio, phone = @phone WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fio", fioTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@id", currentClientId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные обновлены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadClients();
                ResetFormMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = Convert.ToInt32(clientsDataGridView.SelectedRows[0].Cells["id"].Value);
            string name = clientsDataGridView.SelectedRows[0].Cells["fio"].Value.ToString();
            if (MessageBox.Show($"Удалить {name}?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string check = "SELECT COUNT(*) FROM orders WHERE client_id = @id";
                    using (var cmd = new MySqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Нельзя удалить клиента с заказами", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string query = "DELETE FROM clients WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Клиент удалён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            fioTextBox.Text = "";
            phoneTextBox.Text = "";
        }

        private void ResetFormMode()
        {
            isEditMode = false;
            currentClientId = 0;
            addButton.Text = "Добавить";
        }

        // ===== Поиск по телефону строго с начала строки =====
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string txt = searchTextBox.Text.Trim();
            if (clientsDataGridView.DataSource is DataTable dt)
            {
                if (string.IsNullOrEmpty(txt))
                    dt.DefaultView.RowFilter = "";
                else
                {
                    // Для поиска используется реальное значение из БД, поэтому
                    // фильтруем по полю phone и шаблону «начинается с».
                    string safe = txt.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
                    dt.DefaultView.RowFilter = $"phone LIKE '{safe}%'";
                }
            }
            UpdateRecordCount();
        }

        // ===== Сортировка А-Я / Я-А =====
        private bool sortAsc = true;
        private void sortButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.DataSource is DataTable dt)
            {
                dt.DefaultView.Sort = sortAsc ? "fio ASC" : "fio DESC";
                sortButton.Text = sortAsc ? "Сортировка Я-А" : "Сортировка А-Я";
                sortAsc = !sortAsc;
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "";
            if (clientsDataGridView.DataSource is DataTable dt)
            {
                dt.DefaultView.RowFilter = "";
                dt.DefaultView.Sort = "";
            }
            sortAsc = true;
            sortButton.Text = "Сортировка А-Я";
            UpdateRecordCount();
        }

        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        // ===== Кнопка «Подтвердить выбор» — возвращает выбранного клиента в форму заказа =====
        private void addToOrderButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента из таблицы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataGridViewRow row = clientsDataGridView.SelectedRows[0];
            SelectedClientId = Convert.ToInt32(row.Cells["id"].Value);
            SelectedClientName = row.Cells["fio"].Value.ToString();
            SelectedClientPhone = GetRealPhone(SelectedClientId);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ===== Кнопка «Отмена» в режиме выбора =====
        private void cancelSelectionButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ===== Кнопка «глаз» для телефона =====
        private void showPhoneButton_Click(object sender, EventArgs e)
        {
            phoneVisible = !phoneVisible;
            showPhoneButton.Text = phoneVisible ? "🙈" : "👁";
            // Обновляем отображение таблицы
            clientsDataGridView.Refresh();
        }

        // ===== Валидация ФИО =====
        private void fioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ\s-]"))
            {
                e.Handled = true;
                return;
            }
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
            TextBox tb = fioTextBox;
            int sel = tb.SelectionStart;
            string text = tb.Text;
            if (string.IsNullOrEmpty(text)) return;

            char[] chars = text.ToCharArray();
            bool makeUpper = true;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ' ' || chars[i] == '-') { makeUpper = true; }
                else if (makeUpper) { chars[i] = char.ToUpper(chars[i]); makeUpper = false; }
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

        // ===== Маска и валидация телефона =====
        private void phoneTextBox_TextChanged(object sender, EventArgs e)
        {
            string digits = Regex.Replace(phoneTextBox.Text, @"\D", "");
            if (digits.StartsWith("8")) digits = digits.Substring(1);
            else if (digits.StartsWith("7")) digits = digits.Substring(1);

            if (digits.Length > 10) digits = digits.Substring(0, 10);

            string formatted = "+7(";
            if (digits.Length > 0) formatted += digits.Substring(0, Math.Min(3, digits.Length));
            if (digits.Length >= 3) formatted += ")";
            if (digits.Length > 3) formatted += digits.Substring(3, Math.Min(3, digits.Length - 3));
            if (digits.Length > 6) formatted += "-" + digits.Substring(6, Math.Min(2, digits.Length - 6));
            if (digits.Length > 8) formatted += "-" + digits.Substring(8, Math.Min(2, digits.Length - 8));

            if (phoneTextBox.Text != formatted)
            {
                phoneTextBox.TextChanged -= phoneTextBox_TextChanged;
                phoneTextBox.Text = formatted;
                phoneTextBox.SelectionStart = formatted.Length;
                phoneTextBox.TextChanged += phoneTextBox_TextChanged;
            }
        }

        private void phoneTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }
    }
}
