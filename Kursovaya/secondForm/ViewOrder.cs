using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ViewOrderForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool isManagerMode;
        private int selectedOrderId = 0;

        // Сводные поля выбранного заказа — нужны и для модалки «Подробнее», и для печати/PDF.
        private int currentClientId = 0;
        private DateTime currentOrderCreated = DateTime.MinValue;
        private string currentOrderNumber = "";
        private string currentOrderCreatedText = "";
        private string currentCompletionDateText = "";
        private string currentClientName = "";
        private string currentClientPhone = "";
        private string currentStatusName = "";
        private decimal currentSubtotal = 0m;
        private decimal currentTotal = 0m;
        private decimal currentDiscountAmount = 0m;
        private decimal currentDiscountPct = 0m;
        private DataTable currentOrderItems = null;

        // Содержимое для печати чека / бланка PDF.
        private string printContent = "";

        // Конструктор для менеджера/продавца (список заказов)
        public ViewOrderForm(bool isManagerMode = false)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.isManagerMode = isManagerMode;
            InitializeControls();
            LoadAllOrders();
        }

        // Конструктор для просмотра одного заказа — тот же список, но открываем модалку сразу.
        public ViewOrderForm(int orderId, bool isManagerMode = false)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.isManagerMode = isManagerMode;
            this.selectedOrderId = orderId;
            InitializeControls();
            LoadAllOrders();
            this.Shown += (s, e) =>
            {
                LoadOrderDetails(orderId);
                ShowOrderDetailsDialog();
            };
        }

        private void InitializeControls()
        {
            Text = isManagerMode ? "Учет заказов (Менеджер)" : "Учет заказов";

            ConfigureDataGridView(ordersDataGridView);

            // Подсказка в поле поиска (placeholder) — поиск только по номеру заказа.
            SetPlaceholderText(searchOrderTextBox, "Поиск по номеру заказа...");

            LoadStatusesIntoFilter();

            ApplyCoralButtonStyle();
            SetupResponsiveLayout();
        }

        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
            textBox.Tag = placeholder;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
            }
            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };
            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void SetupResponsiveLayout()
        {
            this.MinimumSize = new Size(1100, 700);
            this.WindowState = FormWindowState.Maximized;
        }

        // ===== безопасные конвертации =====
        private static int SafeToInt(object v)
        {
            if (v == null || v == DBNull.Value) return 0;
            if (v is int i) return i;
            if (v is long l) return (int)l;
            if (v is short sh) return sh;
            if (v is byte b) return b;
            if (v is decimal dec) return (int)dec;
            if (v is double dd) return (int)dd;
            if (v is float ff) return (int)ff;
            string s = v.ToString().Trim().Replace(',', '.');
            if (int.TryParse(s, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture, out int r)) return r;
            if (decimal.TryParse(s, System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out decimal r2)) return (int)r2;
            return 0;
        }

        private static decimal SafeToDecimal(object v)
        {
            if (v == null || v == DBNull.Value) return 0m;
            if (v is decimal d) return d;
            if (v is double dd) return (decimal)dd;
            if (v is float ff) return (decimal)ff;
            if (v is int i) return i;
            if (v is long l) return l;
            if (v is short sh) return sh;
            if (v is byte b) return b;
            // Строки вида «5%» / «5,5%» / « 5 » — очищаем от всего лишнего (%, пробелы),
            // потому что в старых заказах скидка могла храниться видом «5%» в VARCHAR колонке.
            string s = v.ToString().Trim().Replace(',', '.').Replace("%", "").Replace(" ", "");
            if (decimal.TryParse(s, System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out decimal r)) return r;
            return 0m;
        }

        private static DateTime SafeToDateTime(object v)
        {
            if (v == null || v == DBNull.Value) return DateTime.MinValue;
            if (v is DateTime dt) return dt;
            string s = v.ToString();
            if (DateTime.TryParse(s, out DateTime r)) return r;
            return DateTime.MinValue;
        }

        private static string FormatRub(decimal v)
        {
            return v.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("ru-RU")) + " руб.";
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
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
            // Раскраска строк по статусу — подвешиваем один раз.
            dgv.CellFormatting -= OrdersDataGridView_CellFormatting;
            dgv.CellFormatting += OrdersDataGridView_CellFormatting;
        }

        // Цвета статусов: зелёный — выполнен, красный — отменён,
        // жёлтый — подтверждён, оранжевый — подтверждён, до автоматической отмены ≤ 1 дня.
        private static readonly Color StatusGreen = Color.FromArgb(198, 239, 206);
        private static readonly Color StatusYellow = Color.FromArgb(255, 243, 176);
        private static readonly Color StatusOrange = Color.FromArgb(255, 196, 137);
        private static readonly Color StatusRed = Color.FromArgb(255, 199, 206);

        // Возвращает цвет строки по имени статуса и плановой дате окончания заказа.
        // completionDate — это плановая дата выполнения заказа из БД (date_of_completion).
        // Для подтверждённых заказов, у которых до этой даты осталось ≤ 24ч (или дата уже прошла),
        // подсвечиваем оранжевым — заказ на грани автоматической отмены.
        private static Color GetStatusRowColor(string statusName, DateTime completionDate)
        {
            string s = (statusName ?? "").Trim().ToLower();
            if (s.StartsWith("заверш") || s.Contains("выполн"))
                return StatusGreen;
            if (s.StartsWith("отмен"))
                return StatusRed;
            if (completionDate != DateTime.MinValue &&
                (completionDate - DateTime.Now).TotalHours <= 24.0)
                return StatusOrange;
            return StatusYellow;
        }

        private static Color DarkenColor(Color c)
        {
            return Color.FromArgb(
                Math.Max(0, c.R - 30),
                Math.Max(0, c.G - 30),
                Math.Max(0, c.B - 30));
        }

        private void OrdersDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= ordersDataGridView.Rows.Count) return;
            var row = ordersDataGridView.Rows[e.RowIndex];
            if (!ordersDataGridView.Columns.Contains("Статус")) return;
            string status = row.Cells["Статус"].Value?.ToString() ?? "";
            DateTime completion = DateTime.MinValue;
            if (ordersDataGridView.Columns.Contains("_RawCompletion"))
                completion = SafeToDateTime(row.Cells["_RawCompletion"].Value);
            Color color = GetStatusRowColor(status, completion);
            e.CellStyle.BackColor = color;
            e.CellStyle.SelectionBackColor = DarkenColor(color);
            e.CellStyle.SelectionForeColor = Color.Black;
        }

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80);
            Color coralLightColor = Color.FromArgb(255, 147, 100);
            Color coralDarkColor = Color.FromArgb(235, 107, 60);

            ApplyStyleToAllButtons(this, coralColor, coralLightColor, coralDarkColor);

            if (menuButton != null) ApplyMenuButtonStyle();
        }

        private void ApplyStyleToAllButtons(Control parent, Color normalColor, Color hoverColor, Color pressedColor)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button button && button != menuButton)
                    ApplyButtonStyle(button, normalColor, hoverColor, pressedColor);
                else if (control.HasChildren)
                    ApplyStyleToAllButtons(control, normalColor, hoverColor, pressedColor);
            }
        }

        private void ApplyButtonStyle(Button button, Color normalColor, Color hoverColor, Color pressedColor)
        {
            button.BackColor = normalColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(235, 107, 60);
            button.FlatAppearance.BorderSize = 1;
            button.ForeColor = Color.Black;

            button.MouseEnter += (s, e) => { button.BackColor = hoverColor; };
            button.MouseLeave += (s, e) => { button.BackColor = normalColor; };
            button.MouseDown += (s, e) => { button.BackColor = pressedColor; };
            button.MouseUp += (s, e) => { button.BackColor = hoverColor; };
        }

        private void ApplyMenuButtonStyle()
        {
            menuButton.BackColor = Color.Red;
            menuButton.FlatStyle = FlatStyle.Flat;
            menuButton.FlatAppearance.BorderColor = Color.DarkRed;
            menuButton.FlatAppearance.BorderSize = 1;
            menuButton.ForeColor = Color.Black;

            menuButton.MouseEnter += (s, e) => { menuButton.BackColor = Color.IndianRed; };
            menuButton.MouseLeave += (s, e) => { menuButton.BackColor = Color.Red; };
            menuButton.MouseDown += (s, e) => { menuButton.BackColor = Color.OrangeRed; };
            menuButton.MouseUp += (s, e) => { menuButton.BackColor = Color.OrangeRed; };
        }

        private void LoadAllOrders()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    // Добавлены 'Дата окончания' и скрытое '_RawCompletion' — для раскраски и отображения.
                    string query = @"
                        SELECT 
                            o.id as 'ID',
                            o.order_number as 'Номер заказа',
                            DATE_FORMAT(o.date_of_creation, '%d.%m.%Y %H:%i') as 'Дата создания',
                            DATE_FORMAT(o.date_of_completion, '%d.%m.%Y') as 'Дата окончания',
                            c.fio as 'Клиент',
                            s.name as 'Статус',
                            o.total_amount as 'Сумма',
                            o.date_of_completion as '_RawCompletion'
                        FROM orders o
                        INNER JOIN clients c ON o.client_id = c.id
                        INNER JOIN statuses s ON o.status_id = s.id
                        ORDER BY o.date_of_creation DESC";

                    using (var adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ordersDataGridView.DataSource = dt;

                        if (ordersDataGridView.Columns.Contains("ID"))
                            ordersDataGridView.Columns["ID"].Visible = false;
                        if (ordersDataGridView.Columns.Contains("_RawCompletion"))
                            ordersDataGridView.Columns["_RawCompletion"].Visible = false;
                        if (ordersDataGridView.Columns.Contains("Сумма"))
                            ordersDataGridView.Columns["Сумма"].DefaultCellStyle.Format = "0.00\" руб.\"";

                        // Применяем сохранённые поиск + фильтр статуса к новой DataTable.
                        ApplyFilters();

                        if (ordersDataGridView.Rows.Count > 0)
                        {
                            // Если уже задан selectedOrderId (single-order mode) — выделяем его.
                            int targetIdx = 0;
                            if (selectedOrderId > 0)
                            {
                                for (int i = 0; i < ordersDataGridView.Rows.Count; i++)
                                {
                                    if (SafeToInt(ordersDataGridView.Rows[i].Cells["ID"].Value) == selectedOrderId)
                                    { targetIdx = i; break; }
                                }
                            }
                            ordersDataGridView.ClearSelection();
                            ordersDataGridView.Rows[targetIdx].Selected = true;
                            selectedOrderId = SafeToInt(ordersDataGridView.Rows[targetIdx].Cells["ID"].Value);
                            if (selectedOrderId > 0) LoadOrderDetails(selectedOrderId);
                        }
                        else
                        {
                            selectedOrderId = 0;
                            UpdateActionButtonsState();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OrdersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedOrderId = SafeToInt(ordersDataGridView.Rows[e.RowIndex].Cells["ID"].Value);
                if (selectedOrderId <= 0) return;
                LoadOrderDetails(selectedOrderId);
            }
        }

        // ---- Маскировки (для роли «продавец») ----
        private string MaskPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return "***";
            string digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length < 4) return "***";
            return new string('*', digits.Length - 4) + digits.Substring(digits.Length - 4);
        }

        private string MaskFIO(string fio)
        {
            if (string.IsNullOrEmpty(fio)) return "***";
            var parts = fio.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length >= 3) parts[i] = parts[i].Substring(0, 3) + new string('*', parts[i].Length - 3);
                else parts[i] = parts[i] + new string('*', 3 - parts[i].Length);
            }
            return string.Join(" ", parts);
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string orderQuery = @"
                        SELECT 
                            o.order_number,
                            o.date_of_creation as raw_created,
                            DATE_FORMAT(o.date_of_creation, '%d.%m.%Y %H:%i') as date_of_creation,
                            DATE_FORMAT(o.date_of_completion, '%d.%m.%Y') as date_of_completion,
                            o.discount,
                            o.total_amount,
                            o.final_amount,
                            o.notes,
                            c.id as client_id,
                            c.fio as client_name,
                            c.phone,
                            s.name as status_name,
                            s.id as status_id
                        FROM orders o
                        INNER JOIN clients c ON o.client_id = c.id
                        INNER JOIN statuses s ON o.status_id = s.id
                        WHERE o.id = @order_id";

                    using (var orderCommand = new MySqlCommand(orderQuery, connection))
                    {
                        orderCommand.Parameters.AddWithValue("@order_id", orderId);
                        using (var reader = orderCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentClientId = SafeToInt(reader["client_id"]);
                                currentOrderNumber = reader["order_number"]?.ToString() ?? "";
                                currentStatusName = reader["status_name"]?.ToString() ?? "";
                                currentOrderCreated = SafeToDateTime(reader["raw_created"]);
                                currentOrderCreatedText = reader["date_of_creation"]?.ToString() ?? "Не указана";
                                currentCompletionDateText = reader["date_of_completion"]?.ToString() ?? "Не установлена";

                                // Маскировка фио/телефона убрана во всех режимах (было требование заказчика).
                                string fio = reader["client_name"]?.ToString() ?? "Не указан";
                                string phone = reader["phone"]?.ToString() ?? "Не указан";
                                currentClientName = fio;
                                currentClientPhone = phone;

                                currentSubtotal = SafeToDecimal(reader["total_amount"]);
                                currentTotal = SafeToDecimal(reader["final_amount"]);
                                currentDiscountPct = SafeToDecimal(reader["discount"]);
                                currentDiscountAmount = currentSubtotal - currentTotal;
                            }
                        }
                    }

                    LoadOrderItems(orderId);
                    UpdateActionButtonsState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки деталей заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // На главной форме осталась только кнопка «Подробнее о заказе».
        // Оформление/отмена/печать чека перенесены в подформу «Подробнее о заказе».
        private void UpdateActionButtonsState()
        {
            showDetailsButton.Enabled = true;
        }

        private void LoadOrderItems(int orderId)
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            p.name as 'Товар',
                            oi.quantity as 'Количество',
                            oi.price as 'Цена',
                            oi.total as 'Сумма'
                        FROM order_items oi
                        INNER JOIN products p ON oi.product_id = p.id
                        WHERE oi.order_id = @order_id";

                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        command.Parameters.AddWithValue("@order_id", orderId);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        currentOrderItems = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обновление статуса заказа на завершен / отменен.
        private bool SetOrderStatusByName(string statusName)
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    int statusId = 0;
                    using (var c = new MySqlCommand("SELECT id FROM statuses WHERE LOWER(name)=LOWER(@n) LIMIT 1", connection))
                    {
                        c.Parameters.AddWithValue("@n", statusName);
                        var r = c.ExecuteScalar();
                        if (r == null || r == DBNull.Value)
                        {
                            MessageBox.Show($"В справочнике статусов не найден '{statusName}'", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        statusId = Convert.ToInt32(r);
                    }
                    string upd = "UPDATE orders SET status_id = @sid WHERE id = @oid";
                    using (var cmd = new MySqlCommand(upd, connection))
                    {
                        cmd.Parameters.AddWithValue("@sid", statusId);
                        cmd.Parameters.AddWithValue("@oid", selectedOrderId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ==================== Кнопка «Подробнее о заказе» ====================
        private void showDetailsButton_Click(object sender, EventArgs e)
        {
            if (selectedOrderId <= 0)
            {
                MessageBox.Show("Выберите заказ из списка", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ShowOrderDetailsDialog();
        }

        // Открывает модальное окно с подробной информацией о заказе.
        private void ShowOrderDetailsDialog()
        {
            // Подформа умеет оформлять/отменять заказ через переданные коллбэки.
            // После успешного оформления автоматически печатается чек в PDF.
            using (var dlg = new OrderDetailsDialog(
                currentOrderNumber,
                currentOrderCreatedText,
                currentCompletionDateText,
                currentClientName,
                currentClientPhone,
                currentStatusName,
                currentSubtotal,
                currentDiscountAmount,
                currentDiscountPct,
                currentTotal,
                currentOrderItems,
                onComplete: () =>
                {
                    if (!TryCompleteOrder()) return false;
                    PrintReceipt();
                    return true;
                },
                onCancel: () => TryCancelOrder()))
            {
                dlg.ShowDialog(this);
            }
        }

        // ==================== Оформление заказа (вызывается из подформы «Подробнее») ====================
        // Возвращает true, если статус успешно изменён на «завершен».
        private bool TryCompleteOrder()
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Выберите заказ из списка.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string s = (currentStatusName ?? "").Trim().ToLower();
            if (s.StartsWith("заверш") || s.Contains("выполн"))
            {
                MessageBox.Show("Этот заказ уже оформлен.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (s.StartsWith("отмен"))
            {
                MessageBox.Show("Заказ отменён, оформить его нельзя.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (MessageBox.Show("Оформить (завершить) заказ? После этого редактирование невозможно.",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return false;

            if (!SetOrderStatusByName("завершен")) return false;

            UpdateInventoryOnCompletion();

            MessageBox.Show("Заказ завершён.", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadAllOrders();
            LoadOrderDetails(selectedOrderId);
            return true;
        }

        // ==================== Отмена заказа (вызывается из подформы «Подробнее») ====================
        private bool TryCancelOrder()
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Выберите заказ из списка.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string s = (currentStatusName ?? "").Trim().ToLower();
            if (s.StartsWith("заверш") || s.Contains("выполн"))
            {
                MessageBox.Show("Заказ уже завершён, отмена невозможна.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (s.StartsWith("отмен"))
            {
                MessageBox.Show("Этот заказ уже отменён.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // Для продавца — срок 1 день. Для менеджера ограничения нет.
            if (!isManagerMode &&
                (currentOrderCreated == DateTime.MinValue ||
                 (DateTime.Now - currentOrderCreated).TotalDays > 1.0))
            {
                MessageBox.Show("Срок отмены заказа истёк (более 1 дня с момента создания).",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (MessageBox.Show("Отменить заказ? После этого редактирование невозможно.",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return false;

            if (!SetOrderStatusByName("отменен")) return false;

            UpdateInventoryOnCancellation();

            MessageBox.Show("Заказ отменён.", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadAllOrders();
            LoadOrderDetails(selectedOrderId);
            return true;
        }

        private void UpdateInventoryOnCompletion()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string upd = @"UPDATE orders SET date_of_completion = @dc WHERE id = @oid";
                    using (var cmd = new MySqlCommand(upd, connection))
                    {
                        cmd.Parameters.AddWithValue("@dc", DateTime.Now);
                        cmd.Parameters.AddWithValue("@oid", selectedOrderId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private void UpdateInventoryOnCancellation()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string upd = @"UPDATE orders SET date_of_completion = @dc WHERE id = @oid";
                    using (var cmd = new MySqlCommand(upd, connection))
                    {
                        cmd.Parameters.AddWithValue("@dc", DateTime.Now);
                        cmd.Parameters.AddWithValue("@oid", selectedOrderId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private string BuildItemsText()
        {
            string itemsText = "";
            if (currentOrderItems != null)
            {
                foreach (DataRow row in currentOrderItems.Rows)
                {
                    string productName = row["Товар"]?.ToString() ?? "Неизвестный товар";
                    string quantity = row["Количество"]?.ToString() ?? "0";
                    decimal price = row["Цена"] != DBNull.Value ? Convert.ToDecimal(row["Цена"]) : 0;
                    decimal total = row["Сумма"] != DBNull.Value ? Convert.ToDecimal(row["Сумма"]) : 0;
                    itemsText += $"{productName} - {quantity} x {price:0.00} руб. = {total:0.00} руб.\n";
                }
            }
            return itemsText;
        }

        private string GenerateReceiptContent()
        {
            string itemsText = BuildItemsText();
            return $@"ЧЕК ЗАКАЗА
====================
Магазин воздушных шаров 'Воздушный мир'
Дата печати: {DateTime.Now:dd.MM.yyyy HH:mm}
----------------------------------------
Номер заказа: {currentOrderNumber}
Дата создания: {currentOrderCreatedText}
Дата выполнения: {currentCompletionDateText}
Клиент: {currentClientName}
Телефон: {currentClientPhone}
Статус: {currentStatusName}
----------------------------------------
ТОВАРЫ:
{itemsText}
----------------------------------------
Подытог: {FormatRub(currentSubtotal)}
Скидка: {FormatRub(currentDiscountAmount)} ({(int)Math.Round(currentDiscountPct)}%)
ИТОГО: {FormatRub(currentTotal)}
----------------------------------------
Спасибо за покупку!
====================";
        }

        // Рисует текущий printContent на странице печати — общий обработчик и для чека, и для бланка.
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            using (Font font = new Font("Consolas", 10))
            {
                e.Graphics.DrawString(printContent, font, Brushes.Black,
                    new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top,
                                   e.MarginBounds.Width, e.MarginBounds.Height));
            }
        }

        // Ищем установленный PDF-принтер. На Windows 10/11 всегда есть «Microsoft Print to PDF».
        private static string FindPdfPrinterName()
        {
            try
            {
                foreach (string p in PrinterSettings.InstalledPrinters)
                {
                    if (string.Equals(p, "Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase))
                        return p;
                }
                foreach (string p in PrinterSettings.InstalledPrinters)
                {
                    if (p.IndexOf("PDF", StringComparison.OrdinalIgnoreCase) >= 0)
                        return p;
                }
            }
            catch { }
            return null;
        }

        // Универсальный сохранятор PDF: SaveFileDialog с фильтром *.pdf, печать в файл через
        // Microsoft Print to PDF (PrintToFile = true). Сам файл НЕ открываем внешним приложением
        // — именно из-за ShellExecute окно приложения раньше «скрывалось» за PDF-вьювером.
        // После сохранения возвращаем фокус форме явными BringToFront/Activate.
        private void SaveContentAsPdf(string suggestedFileName, string title)
        {
            string savedPath = null;
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PDF файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*";
                    sfd.FileName = suggestedFileName;
                    sfd.Title = title;
                    sfd.DefaultExt = "pdf";
                    sfd.AddExtension = true;
                    sfd.CheckFileExists = false;   // мы только создаём файл — никаких «файл не найден»
                    sfd.CheckPathExists = true;
                    sfd.OverwritePrompt = true;
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    if (sfd.ShowDialog(this) != DialogResult.OK) return;

                    string path = sfd.FileName;
                    if (!path.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        path += ".pdf";

                    // Гарантированно создаём родительский каталог.
                    string dir = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    // Если файл существует — удалим, иначе принтер допишет _2 в имя.
                    if (File.Exists(path))
                    {
                        try { File.Delete(path); } catch { }
                    }

                    string pdfPrinter = FindPdfPrinterName();
                    if (string.IsNullOrEmpty(pdfPrinter))
                    {
                        string txtPath = Path.ChangeExtension(path, ".txt");
                        File.WriteAllText(txtPath, printContent, System.Text.Encoding.UTF8);
                        MessageBox.Show(this,
                            $"В системе не найден PDF-принтер (Microsoft Print to PDF).\nФайл сохранён как текстовый: {txtPath}",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        savedPath = txtPath;
                    }
                    else
                    {
                        using (PrintDocument doc = new PrintDocument())
                        {
                            doc.DocumentName = Path.GetFileNameWithoutExtension(path);
                            doc.PrinterSettings.PrinterName = pdfPrinter;
                            doc.PrinterSettings.PrintToFile = true;
                            doc.PrinterSettings.PrintFileName = path;
                            doc.PrintPage += PrintDocument_PrintPage;
                            doc.Print();
                        }
                        savedPath = path;
                    }

                    // Информируем путём, НЕ открывая файл внешним приложением — чтобы приложение не уходило в фон.
                    MessageBox.Show(this, $"Файл сохранён:\n{savedPath}",
                        "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Ошибка сохранения PDF: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Гарантируем, что форма НЕ осталась спрятанной за PDF-вьювером или окном печати.
                try
                {
                    if (this.WindowState == FormWindowState.Minimized)
                        this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.BringToFront();
                    this.Activate();
                    this.Focus();
                }
                catch { }
            }
        }

        // Чек печатается автоматически после успешного оформления заказа из подформы «Подробнее».
        // Статус к этому моменту уже изменён на «завершен», поэтому проверки тут не нужны.
        private void PrintReceipt()
        {
            if (selectedOrderId == 0) return;
            printContent = GenerateReceiptContent();
            string safeOrderNumber = SanitizeFileName(currentOrderNumber);
            string suggestedName = $"Чек_заказа_{safeOrderNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            SaveContentAsPdf(suggestedName, "Сохранить чек (PDF)");
        }

        private static string SanitizeFileName(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return "order";
            string invalid = new string(Path.GetInvalidFileNameChars()) + "№\\/:*?\"<>| ";
            string s = raw;
            foreach (char ch in invalid) s = s.Replace(ch.ToString(), "_");
            s = Regex.Replace(s, "_+", "_").Trim('_');
            return string.IsNullOrEmpty(s) ? "order" : s;
        }

        private void ViewOrderForm_Load(object sender, EventArgs e)
        {
            UpdateActionButtonsState();
        }

        // Поиск заказа — только по номеру заказа (строго с начала).
        private void searchOrderTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // Выпадающий список фильтрации по статусам.
        private void statusFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // Загрузка справочника статусов в комбобокс фильтра. Первая позиция — «Все статусы».
        private void LoadStatusesIntoFilter()
        {
            statusFilterComboBox.Items.Clear();
            statusFilterComboBox.Items.Add("Все статусы");
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand("SELECT name FROM statuses ORDER BY id", connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            statusFilterComboBox.Items.Add(reader["name"]?.ToString() ?? "");
                    }
                }
            }
            catch
            {
                // Если справочник недоступен — оставляем только «Все статусы», фильтр станет no-op.
            }
            statusFilterComboBox.SelectedIndex = 0;
        }

        // Применение комбинированного фильтра «поиск по номеру + выбранный статус» к DataTable грида.
        private void ApplyFilters()
        {
            if (!(ordersDataGridView.DataSource is DataTable dt)) return;

            var conditions = new List<string>();

            // Поиск по номеру заказа.
            string txt = searchOrderTextBox.Text;
            string placeholder = searchOrderTextBox.Tag as string;
            if (!string.IsNullOrEmpty(txt) && txt != placeholder)
            {
                string s = txt.Trim();
                if (!string.IsNullOrEmpty(s))
                {
                    string esc = s.Replace("'", "''");
                    conditions.Add($"CONVERT([Номер заказа], 'System.String') LIKE '{esc}%'");
                }
            }

            // Фильтр по статусу. Индекс 0 — «Все статусы».
            if (statusFilterComboBox.SelectedIndex > 0)
            {
                string status = statusFilterComboBox.SelectedItem?.ToString() ?? "";
                if (!string.IsNullOrEmpty(status))
                {
                    string esc = status.Replace("'", "''");
                    conditions.Add($"[Статус] = '{esc}'");
                }
            }

            dt.DefaultView.RowFilter = string.Join(" AND ", conditions);
        }

        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        // ==================== Модальное окно «Подробнее о заказе» ====================
        // Внутренний класс, чтобы не плодить отдельный designer-файл.
        // Кнопки «Оформить заказ» и «Отменить заказ» вызывают коллбэки родительской формы
        // (родитель содержит логику работы с БД и обновления грида).
        private class OrderDetailsDialog : Form
        {
            public OrderDetailsDialog(
                string orderNumber, string orderCreated, string completion,
                string clientName, string clientPhone, string statusName,
                decimal subtotal, decimal discountAmount, decimal discountPct, decimal total,
                DataTable items,
                Func<bool> onComplete = null, Func<bool> onCancel = null)
            {
                Text = $"Заказ № {orderNumber}";
                StartPosition = FormStartPosition.CenterParent;
                // Размеры диалога увеличены: все шрифты в окне выросли на ≈10пт (требование заказчика).
                MinimumSize = new Size(960, 720);
                ClientSize = new Size(1080, 760);
                BackColor = Color.White;

                // Шапка
                var header = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 50,
                    BackColor = Color.DarkSalmon
                };
                var lblHeader = new Label
                {
                    Text = $"  Заказ № {orderNumber}",
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                header.Controls.Add(lblHeader);

                // Левая колонка инфо — расширена под большие шрифты
                var info = new TableLayoutPanel
                {
                    ColumnCount = 2,
                    RowCount = 6,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Location = new Point(16, 70),
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None
                };
                info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
                info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
                AddInfoRow(info, "Номер заказа:", orderNumber, true);
                AddInfoRow(info, "Дата создания:", orderCreated, false);
                AddInfoRow(info, "Дата выполнения:", completion, false);
                AddInfoRow(info, "Клиент:", clientName, false);
                AddInfoRow(info, "Телефон:", clientPhone, false);
                AddInfoRow(info, "Статус:", statusName, true);

                // Правая колонка — суммы. Исправлен вывод процента скидки:
                // если discountPct по какой-то причине 0, а discountAmount и subtotal > 0 — вычисляем
                // фактический процент из сумм (старые записи без числового процента).
                decimal effectivePct = discountPct;
                if (effectivePct <= 0m && discountAmount > 0m && subtotal > 0m)
                    effectivePct = Math.Round(discountAmount / subtotal * 100m, 0);

                var totals = new GroupBox
                {
                    Text = "Итоги",
                    Location = new Point(620, 60),
                    Size = new Size(440, 220),
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(235, 107, 60)
                };
                var lblSubtotalCap = new Label { Text = "Подытог:", Location = new Point(16, 40), AutoSize = true, Font = new Font("Microsoft Sans Serif", 14), ForeColor = Color.Black };
                var lblSubtotalVal = new Label { Text = FormatRub(subtotal), Location = new Point(180, 40), AutoSize = false, Size = new Size(240, 28), TextAlign = ContentAlignment.MiddleRight, Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold), ForeColor = Color.Black };
                var lblDiscountCap = new Label { Text = "Скидка:", Location = new Point(16, 90), AutoSize = true, Font = new Font("Microsoft Sans Serif", 14), ForeColor = Color.Black };
                var lblDiscountVal = new Label { Text = $"{FormatRub(discountAmount)} ({(int)Math.Round(effectivePct)}%)", Location = new Point(140, 90), AutoSize = false, Size = new Size(280, 28), TextAlign = ContentAlignment.MiddleRight, Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold), ForeColor = Color.Black };
                var lblTotalCap = new Label { Text = "ИТОГО:", Location = new Point(16, 150), AutoSize = true, Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold), ForeColor = Color.Black };
                var lblTotalVal = new Label { Text = FormatRub(total), Location = new Point(140, 148), AutoSize = false, Size = new Size(280, 32), TextAlign = ContentAlignment.MiddleRight, Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold), ForeColor = Color.FromArgb(235, 107, 60) };
                totals.Controls.AddRange(new Control[] { lblSubtotalCap, lblSubtotalVal, lblDiscountCap, lblDiscountVal, lblTotalCap, lblTotalVal });

                // Грид с товарами. Шрифт ячеек («данные в колонках») увеличен до 18пт —
                // это стандартный размер (8пт) + 10пт по требованию заказчика.
                var itemsLabel = new Label
                {
                    Text = "Товары:",
                    Location = new Point(16, 300),
                    AutoSize = true,
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold)
                };
                var grid = new DataGridView
                {
                    Location = new Point(16, 336),
                    Size = new Size(1044, 320),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    BackgroundColor = Color.White,
                    RowTemplate = { Height = 38 }
                };
                // Данные в ячейках — +10пт (было базовых ~8пт → 18пт).
                grid.DefaultCellStyle.Font = new Font("Segoe UI", 18);
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                grid.ColumnHeadersHeight = 50;
                grid.EnableHeadersVisualStyles = false;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
                grid.DataSource = items;
                grid.DataBindingComplete += (s, e) =>
                {
                    if (grid.Columns.Contains("Цена"))
                        grid.Columns["Цена"].DefaultCellStyle.Format = "0.00\" руб.\"";
                    if (grid.Columns.Contains("Сумма"))
                        grid.Columns["Сумма"].DefaultCellStyle.Format = "0.00\" руб.\"";
                };

                // Кнопки действий: «Оформить заказ», «Отменить заказ», «Назад».
                // Доступность кнопок зависит от статуса заказа: оформлять/отменять можно
                // только активные (не «завершен» и не «отменён») заказы.
                string sLower = (statusName ?? "").Trim().ToLower();
                bool isFinal = sLower.StartsWith("заверш") || sLower.Contains("выполн") || sLower.StartsWith("отмен");

                var btnComplete = new Button
                {
                    Text = "Оформить заказ",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                    Location = new Point(16, 690),
                    Size = new Size(240, 56),
                    BackColor = Color.Coral,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    Enabled = !isFinal && onComplete != null
                };
                btnComplete.FlatAppearance.BorderColor = Color.Gray;
                btnComplete.Click += (s, e) =>
                {
                    if (onComplete == null) return;
                    if (onComplete()) Close();
                };

                var btnCancel = new Button
                {
                    Text = "Отменить заказ",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                    Location = new Point(270, 690),
                    Size = new Size(220, 56),
                    BackColor = Color.Coral,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    Enabled = !isFinal && onCancel != null
                };
                btnCancel.FlatAppearance.BorderColor = Color.Gray;
                btnCancel.Click += (s, e) =>
                {
                    if (onCancel == null) return;
                    if (onCancel()) Close();
                };

                // Нижняя кнопка «Назад». Шрифт +10пт (~8пт → 18пт) по требованию заказчика.
                var btnClose = new Button
                {
                    Text = "Назад",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new Point(880, 690),
                    Size = new Size(180, 56),
                    BackColor = Color.Coral,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold)
                };
                btnClose.FlatAppearance.BorderColor = Color.Gray;
                btnClose.Click += (s, e) => Close();

                Controls.Add(grid);
                Controls.Add(itemsLabel);
                Controls.Add(totals);
                Controls.Add(info);
                Controls.Add(header);
                Controls.Add(btnComplete);
                Controls.Add(btnCancel);
                Controls.Add(btnClose);
            }

            // Шрифт левой панели инфо также увеличен, чтобы весь диалог выглядел однородно.
            private static void AddInfoRow(TableLayoutPanel grid, string caption, string value, bool bold)
            {
                var lblCap = new Label
                {
                    Text = caption,
                    AutoSize = true,
                    Margin = new Padding(0, 6, 12, 6),
                    Font = new Font("Microsoft Sans Serif", 14)
                };
                var lblVal = new Label
                {
                    Text = value,
                    AutoSize = true,
                    Margin = new Padding(0, 6, 0, 6),
                    Font = new Font("Microsoft Sans Serif", 14, bold ? FontStyle.Bold : FontStyle.Regular)
                };
                grid.Controls.Add(lblCap);
                grid.Controls.Add(lblVal);
            }
        }
    }
}
