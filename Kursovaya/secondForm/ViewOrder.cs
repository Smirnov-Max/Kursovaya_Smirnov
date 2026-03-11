using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;
using Smirnov_kursovaya.mainForm;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ViewOrderForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool isManagerMode;
        private int selectedOrderId = 0;
        private int currentClientId = 0;

        // Конструктор для менеджера/продавца (список заказов)
        public ViewOrderForm(bool isManagerMode = false)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.isManagerMode = isManagerMode;
            InitializeControls();
            LoadAllOrders();
        }

        // Конструктор для совместимости (просмотр одного заказа)
        public ViewOrderForm(int orderId, bool isManagerMode = false)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.isManagerMode = isManagerMode;
            this.selectedOrderId = orderId;
            InitializeControls();
            ShowSingleOrderMode(orderId);
        }

        private void InitializeControls()
        {
            // Режим менеджера
            if (isManagerMode)
            {
                Text = "Просмотр и управление заказами (Менеджер)";
                updateStatusButton.Visible = true;
                statusComboBox.Enabled = true;
                applyDiscountButton.Visible = true;
                discountComboBox.Enabled = true;
                btnClientDetails.Visible = false;

                LoadStatuses();
            }
            else
            {
                Text = "Просмотр заказов";
                updateStatusButton.Visible = false;
                statusComboBox.Enabled = false;
                applyDiscountButton.Visible = false;
                discountComboBox.Enabled = false;
                btnClientDetails.Visible = true;
            }

            discountComboBox.Items.Clear();
            discountComboBox.Items.AddRange(new object[] { "0%", "5%", "10%", "15%", "20%" });
            discountComboBox.SelectedIndex = 0;

            // Настройка DataGridView для списка заказов
            ordersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ordersDataGridView.ReadOnly = true;
            ordersDataGridView.RowHeadersVisible = false;

            ordersDataGridView.GridColor = Color.LightGray;
            ordersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            ordersDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
            ordersDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            ordersDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            ordersDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            ordersDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ordersDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            ordersDataGridView.ColumnHeadersHeight = 40;
            ordersDataGridView.EnableHeadersVisualStyles = false;

            // Настройка DataGridView для товаров заказа
            orderItemsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            orderItemsDataGridView.ReadOnly = true;
            orderItemsDataGridView.RowHeadersVisible = false;

            orderItemsDataGridView.GridColor = Color.LightGray;
            orderItemsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            orderItemsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
            orderItemsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            orderItemsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            orderItemsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            orderItemsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            orderItemsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            orderItemsDataGridView.ColumnHeadersHeight = 40;
            orderItemsDataGridView.EnableHeadersVisualStyles = false;

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

            ApplyCoralButtonStyle();
        }

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80);
            Color coralLightColor = Color.FromArgb(255, 147, 100);
            Color coralDarkColor = Color.FromArgb(235, 107, 60);

            ApplyStyleToAllButtons(this, coralColor, coralLightColor, coralDarkColor);

            if (menuButton != null)
            {
                ApplyMenuButtonStyle();
            }
        }

        private void ApplyStyleToAllButtons(Control parent, Color normalColor, Color hoverColor, Color pressedColor)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button button && button != menuButton)
                {
                    ApplyButtonStyle(button, normalColor, hoverColor, pressedColor);
                }
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
            menuButton.Font = new Font(menuButton.Font, FontStyle.Regular);

            menuButton.MouseEnter += (s, e) => { menuButton.BackColor = Color.IndianRed; };
            menuButton.MouseLeave += (s, e) => { menuButton.BackColor = Color.Red; };
            menuButton.MouseDown += (s, e) => { menuButton.BackColor = Color.OrangeRed; };
            menuButton.MouseUp += (s, e) => { menuButton.BackColor = Color.OrangeRed; };
        }

        private void ShowSingleOrderMode(int orderId)
        {
            ordersDataGridView.Visible = false;
            searchOrderTextBox.Visible = false;
            searchLabel.Visible = false;
            refreshButton.Visible = false;

            orderDetailsPanel.Visible = true;
            orderDetailsPanel.Dock = DockStyle.Fill;
            orderDetailsPanel.Location = new System.Drawing.Point(0, 60);
            orderDetailsPanel.Size = new System.Drawing.Size(884, 511);

            LoadOrderDetails(orderId);
        }

        private void LoadAllOrders()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            o.id as 'ID',
                            o.order_number as 'Номер заказа',
                            DATE_FORMAT(o.date_of_creation, '%d.%m.%Y %H:%i') as 'Дата создания',
                            c.fio as 'Клиент',
                            s.name as 'Статус',
                            o.total_amount as 'Сумма'
                        FROM orders o
                        INNER JOIN clients c ON o.client_id = c.id
                        INNER JOIN statuses s ON o.status_id = s.id
                        ORDER BY o.date_of_creation DESC";

                    using (var adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        ordersDataGridView.DataSource = dt;

                        if (ordersDataGridView.Rows.Count > 0)
                        {
                            ordersDataGridView.Rows[0].Selected = true;
                            selectedOrderId = Convert.ToInt32(ordersDataGridView.Rows[0].Cells["ID"].Value);
                            LoadOrderDetails(selectedOrderId);
                            orderDetailsPanel.Visible = true;
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
                selectedOrderId = Convert.ToInt32(ordersDataGridView.Rows[e.RowIndex].Cells["ID"].Value);
                LoadOrderDetails(selectedOrderId);
                orderDetailsPanel.Visible = true;
            }
        }

        // ---- Методы маскировки ----

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
                if (parts[i].Length >= 3)
                {
                    parts[i] = parts[i].Substring(0, 3) + new string('*', parts[i].Length - 3);
                }
                else
                {
                    parts[i] = parts[i] + new string('*', 3 - parts[i].Length);
                }
            }
            return string.Join(" ", parts);
        }

        // ---------------------------

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
                                currentClientId = Convert.ToInt32(reader["client_id"]);

                                orderNumberLabel.Text = reader["order_number"]?.ToString() ?? "Не указан";
                                orderDateLabel.Text = reader["date_of_creation"]?.ToString() ?? "Не указана";
                                completionDateLabel.Text = reader["date_of_completion"]?.ToString() ?? "Не установлена";

                                if (isManagerMode)
                                {
                                    clientNameLabel.Text = reader["client_name"]?.ToString() ?? "Не указан";
                                    clientPhoneLabel.Text = reader["phone"]?.ToString() ?? "Не указан";
                                }
                                else
                                {
                                    clientNameLabel.Text = MaskFIO(reader["client_name"]?.ToString() ?? "Не указан");
                                    clientPhoneLabel.Text = MaskPhone(reader["phone"]?.ToString() ?? "Не указан");
                                }

                                statusLabel.Text = reader["status_name"]?.ToString() ?? "Не указан";

                                decimal totalAmount = reader["total_amount"] != DBNull.Value ?
                                    Convert.ToDecimal(reader["total_amount"]) : 0;
                                decimal finalAmount = reader["final_amount"] != DBNull.Value ?
                                    Convert.ToDecimal(reader["final_amount"]) : 0;

                                subtotalLabel.Text = totalAmount.ToString("C2");
                                totalLabel.Text = finalAmount.ToString("C2");

                                decimal discountAmount = totalAmount - finalAmount;
                                discountLabel.Text = discountAmount.ToString("C2");

                                if (reader["discount"] != DBNull.Value)
                                {
                                    string discountStr = reader["discount"].ToString().Replace("%", "");
                                    if (int.TryParse(discountStr, out int discountValue))
                                    {
                                        int index = discountValue / 5;
                                        if (index >= 0 && index < discountComboBox.Items.Count)
                                            discountComboBox.SelectedIndex = index;
                                    }
                                }
                                else
                                {
                                    discountComboBox.SelectedIndex = 0;
                                }

                                int statusId = reader["status_id"] != DBNull.Value ?
                                    Convert.ToInt32(reader["status_id"]) : 0;

                                if (statusComboBox.Items.Count > 0)
                                {
                                    for (int i = 0; i < statusComboBox.Items.Count; i++)
                                    {
                                        dynamic item = statusComboBox.Items[i];
                                        if (item.Id == statusId)
                                        {
                                            statusComboBox.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    LoadOrderItems(orderId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки деталей заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                        orderItemsDataGridView.DataSource = dt;

                        if (orderItemsDataGridView.Columns.Contains("Цена"))
                        {
                            orderItemsDataGridView.Columns["Цена"].DefaultCellStyle.Format = "C2";
                        }

                        if (orderItemsDataGridView.Columns.Contains("Сумма"))
                        {
                            orderItemsDataGridView.Columns["Сумма"].DefaultCellStyle.Format = "C2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров заказа: {ex.Message}", "Ошибка",
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
                    string query = "SELECT id, name FROM statuses ORDER BY id";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        statusComboBox.Items.Clear();
                        while (reader.Read())
                        {
                            statusComboBox.Items.Add(new
                            {
                                Id = reader["id"],
                                Name = reader["name"].ToString()
                            });
                        }
                        statusComboBox.DisplayMember = "Name";
                        statusComboBox.ValueMember = "Id";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateStatusButton_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Выберите заказ из списка", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (statusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите новый статус", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                dynamic selectedStatus = statusComboBox.SelectedItem;

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE orders SET status_id = @status_id WHERE id = @order_id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status_id", selectedStatus.Id);
                        command.Parameters.AddWithValue("@order_id", selectedOrderId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Статус заказа успешно обновлен", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (ordersDataGridView.Visible)
                                LoadAllOrders();
                            else
                                LoadOrderDetails(selectedOrderId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void applyDiscountButton_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Выберите заказ для применения скидки", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (discountComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите размер скидки из списка", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string discountText = discountComboBox.SelectedItem.ToString();
                discountText = discountText.Replace("%", "");
                decimal discount = decimal.Parse(discountText);

                string subtotalText = subtotalLabel.Text.Replace("₽", "").Replace("$", "").Replace(" ", "").Replace("руб", "");
                decimal totalAmount = decimal.Parse(subtotalText);

                decimal discountAmount = totalAmount * (discount / 100);
                decimal finalAmount = totalAmount - discountAmount;

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string updateQuery = @"UPDATE orders SET 
                                        discount = @discount, 
                                        final_amount = @final_amount 
                                        WHERE id = @order_id";

                    using (var updateCmd = new MySqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@discount", discount);
                        updateCmd.Parameters.AddWithValue("@final_amount", finalAmount);
                        updateCmd.Parameters.AddWithValue("@order_id", selectedOrderId);

                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show($"Скидка {discount}% успешно применена", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        discountLabel.Text = discountAmount.ToString("C2");
                        totalLabel.Text = finalAmount.ToString("C2");

                        if (ordersDataGridView.Visible)
                            LoadAllOrders();
                        else
                            LoadOrderDetails(selectedOrderId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка применения скидки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            LoadAllOrders();
        }

        private void ViewOrderForm_Load(object sender, EventArgs e)
        {
            if (isManagerMode)
            {
                LoadStatuses();
            }
        }

        private void searchOrderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ordersDataGridView.DataSource is DataTable dt)
            {
                string searchText = searchOrderTextBox.Text.ToLower();

                if (string.IsNullOrEmpty(searchText))
                {
                    dt.DefaultView.RowFilter = "";
                }
                else
                {
                    string filter = $"CONVERT([Номер заказа], 'System.String') LIKE '%{searchText}%' OR " +
                                   $"[Клиент] LIKE '%{searchText}%' OR " +
                                   $"[Статус] LIKE '%{searchText}%'";
                    dt.DefaultView.RowFilter = filter;
                }
            }
        }

        private string GenerateReceiptContent()
        {
            string itemsText = "";
            if (orderItemsDataGridView.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["Товар"]?.ToString() ?? "Неизвестный товар";
                    string quantity = row["Количество"]?.ToString() ?? "0";

                    decimal price = 0;
                    decimal total = 0;

                    if (row["Цена"] != DBNull.Value && row["Цена"] != null)
                    {
                        price = Convert.ToDecimal(row["Цена"]);
                    }

                    if (row["Сумма"] != DBNull.Value && row["Сумма"] != null)
                    {
                        total = Convert.ToDecimal(row["Сумма"]);
                    }

                    itemsText += $"{productName} - {quantity} x {price:C2} = {total:C2}\n";
                }
            }
            else
            {
                itemsText = "Товары не загружены\n";
            }

            return $@"ЧЕК ЗАКАЗА
====================
Магазин воздушных шаров 'Воздушный мир'
Дата печати: {DateTime.Now:dd.MM.yyyy HH:mm}
----------------------------------------
Номер заказа: {orderNumberLabel.Text}
Дата создания: {orderDateLabel.Text}
Дата выполнения: {completionDateLabel.Text}
Клиент: {clientNameLabel.Text}
Телефон: {clientPhoneLabel.Text}
Статус: {statusLabel.Text}
----------------------------------------
ТОВАРЫ:
{itemsText}
----------------------------------------
Подытог: {subtotalLabel.Text}
Скидка: {discountLabel.Text}
ИТОГО: {totalLabel.Text}
----------------------------------------
Спасибо за покупку!
Ждем Вас снова!
====================";
        }

        private void PrintReceipt()
        {
            try
            {
                if (selectedOrderId == 0)
                {
                    MessageBox.Show("Выберите заказ для печати чека", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string receiptContent = GenerateReceiptContent();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
                saveFileDialog.FileName = $"Чек_заказа_{orderNumberLabel.Text.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                saveFileDialog.Title = "Сохранить чек";
                saveFileDialog.DefaultExt = "txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, receiptContent, System.Text.Encoding.UTF8);

                    MessageBox.Show($"Чек успешно сохранен в файл:\n{saveFileDialog.FileName}", "Чек сохранен",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Открыть чек для просмотра?", "Просмотр чека",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании чека: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Выберите заказ для печати", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintReceipt();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClientDetails_Click(object sender, EventArgs e)
        {
            if (currentClientId == 0)
            {
                MessageBox.Show("Клиент не выбран", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT fio, phone FROM clients WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", currentClientId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fio = reader["fio"].ToString();
                                string phone = reader["phone"].ToString();

                                MessageBox.Show(
                                    $"ФИО: {fio}\nТелефон: {phone}",
                                    "Полные данные клиента",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                            }
                            else
                            {
                                MessageBox.Show("Клиент не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных клиента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}