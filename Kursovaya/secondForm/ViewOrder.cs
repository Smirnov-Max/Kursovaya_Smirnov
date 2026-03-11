using System;
using System.Data;
using System.Drawing;
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

                // Загружаем статусы для выпадающего списка
                LoadStatuses();
            }
            else
            {
                Text = "Просмотр заказов";
                updateStatusButton.Visible = false;
                statusComboBox.Enabled = false;
                applyDiscountButton.Visible = false;
                discountComboBox.Enabled = false;
            }

            // Заполняем комбобокс скидок
            discountComboBox.Items.Clear();
            discountComboBox.Items.AddRange(new object[] { "0%", "5%", "10%", "15%", "20%" });
            discountComboBox.SelectedIndex = 0;

            // Настраиваем DataGridView для списка заказов
            ordersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ordersDataGridView.ReadOnly = true;
            ordersDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            ordersDataGridView.GridColor = Color.LightGray;
            ordersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Устанавливаем чередование цветов строк
            ordersDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255); // Очень светлый фиолетовый

            // Цвет выделенной строки - очень светлый фиолетовый
            ordersDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250); // Светлый фиолетовый для выделения
            ordersDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Черный текст для контраста

            // Цвет заголовков
            ordersDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80); // Coral цвет
            ordersDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ordersDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            ordersDataGridView.ColumnHeadersHeight = 40;
            ordersDataGridView.EnableHeadersVisualStyles = false; // Отключаем стандартные стили Windows

            // Настраиваем DataGridView для товаров заказа
            orderItemsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            orderItemsDataGridView.ReadOnly = true;
            orderItemsDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            orderItemsDataGridView.GridColor = Color.LightGray;
            orderItemsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Устанавливаем чередование цветов строк
            orderItemsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255); // Очень светлый фиолетовый

            // Цвет выделенной строки - очень светлый фиолетовый
            orderItemsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250); // Светлый фиолетовый для выделения
            orderItemsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Черный текст для контраста

            // Цвет заголовков
            orderItemsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80); // Coral цвет
            orderItemsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            orderItemsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            orderItemsDataGridView.ColumnHeadersHeight = 40;
            orderItemsDataGridView.EnableHeadersVisualStyles = false; // Отключаем стандартные стили Windows

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

            // Устанавливаем стиль
            ApplyCoralButtonStyle();

        }

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80); // Coral цвет
            Color coralLightColor = Color.FromArgb(255, 147, 100); // Светлее для hover
            Color coralDarkColor = Color.FromArgb(235, 107, 60); // Темнее для нажатия

            // Рекурсивно применяем стиль ко всем кнопкам
            ApplyStyleToAllButtons(this, coralColor, coralLightColor, coralDarkColor);

            // Особый стиль для кнопки меню (можно сделать другого цвета)
            if (menuButton != null)
            {
                ApplyMenuButtonStyle();
            }
        }

        private void ApplyStyleToAllButtons(Control parent, Color normalColor, Color hoverColor, Color pressedColor)
        {
            foreach (Control control in parent.Controls)
            {
                // Если это кнопка - применяем стиль
                if (control is Button button && button != menuButton) // Исключаем кнопку меню
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

        private void ShowSingleOrderMode(int orderId)
        {
            // Скрываем элементы для списка заказов
            ordersDataGridView.Visible = false;
            searchOrderTextBox.Visible = false;
            searchLabel.Visible = false;
            refreshButton.Visible = false;

            // Показываем панель деталей на всю форму
            orderDetailsPanel.Visible = true;
            orderDetailsPanel.Dock = DockStyle.Fill;
            orderDetailsPanel.Location = new System.Drawing.Point(0, 60);
            orderDetailsPanel.Size = new System.Drawing.Size(884, 511);

            // Загружаем детали заказа
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

                        // Автоматически выбираем первую строку
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
                                // Обновляем информацию о заказе
                                orderNumberLabel.Text = reader["order_number"]?.ToString() ?? "Не указан";
                                orderDateLabel.Text = reader["date_of_creation"]?.ToString() ?? "Не указана";
                                completionDateLabel.Text = reader["date_of_completion"]?.ToString() ?? "Не установлена";
                                clientNameLabel.Text = reader["client_name"]?.ToString() ?? "Не указан";
                                clientPhoneLabel.Text = reader["phone"]?.ToString() ?? "Не указан";
                                statusLabel.Text = reader["status_name"]?.ToString() ?? "Не указан";

                                // Суммы и скидки
                                decimal totalAmount = reader["total_amount"] != DBNull.Value ?
                                    Convert.ToDecimal(reader["total_amount"]) : 0;
                                decimal finalAmount = reader["final_amount"] != DBNull.Value ?
                                    Convert.ToDecimal(reader["final_amount"]) : 0;

                                subtotalLabel.Text = totalAmount.ToString("C2");
                                totalLabel.Text = finalAmount.ToString("C2");

                                decimal discountAmount = totalAmount - finalAmount;
                                discountLabel.Text = discountAmount.ToString("C2");

                                // Устанавливаем текущую скидку в ComboBox
                                if (reader["discount"] != DBNull.Value)
                                {
                                    string discountStr = reader["discount"].ToString().Replace("%", "");
                                    if (int.TryParse(discountStr, out int discountValue))
                                    {
                                        int index = discountValue / 5; // 0%, 5%, 10%, 15%, 20%
                                        if (index >= 0 && index < discountComboBox.Items.Count)
                                            discountComboBox.SelectedIndex = index;
                                    }
                                }
                                else
                                {
                                    discountComboBox.SelectedIndex = 0; // 0% по умолчанию
                                }

                                // Устанавливаем статус в ComboBox
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

                    // Загружаем товары заказа
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

                        // Форматируем столбцы с суммами
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

                            // Обновляем данные
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
                // Получаем выбранную скидку из комбобокса
                string discountText = discountComboBox.SelectedItem.ToString();
                discountText = discountText.Replace("%", "");
                decimal discount = decimal.Parse(discountText);

                // Получаем текущую сумму заказа из метки
                string subtotalText = subtotalLabel.Text.Replace("₽", "").Replace("$", "").Replace(" ", "").Replace("руб", "");
                decimal totalAmount = decimal.Parse(subtotalText);

                // Рассчитываем новую сумму со скидкой
                decimal discountAmount = totalAmount * (discount / 100);
                decimal finalAmount = totalAmount - discountAmount;

                // Обновляем заказ в базе данных
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

                        // Обновляем отображение на форме
                        discountLabel.Text = discountAmount.ToString("C2");
                        totalLabel.Text = finalAmount.ToString("C2");

                        // Обновляем список заказов если он видим
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
            // Загружаем статусы для менеджера
            if (isManagerMode)
            {
                LoadStatuses();
            }
        }

        private void searchOrderTextBox_TextChanged(object sender, EventArgs e)
        {
            // Поиск по таблице заказов
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

        // Метод для генерации содержимого чека
        private string GenerateReceiptContent()
        {
            string itemsText = "";
            if (orderItemsDataGridView.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["Товар"]?.ToString() ?? "Неизвестный товар";
                    string quantity = row["Количество"]?.ToString() ?? "0";

                    // Пытаемся получить цену и сумму
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

        // Метод для печати чека
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

                // Создаем диалог сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
                saveFileDialog.FileName = $"Чек_заказа_{orderNumberLabel.Text.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                saveFileDialog.Title = "Сохранить чек";
                saveFileDialog.DefaultExt = "txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Сохраняем чек в файл
                    System.IO.File.WriteAllText(saveFileDialog.FileName, receiptContent, System.Text.Encoding.UTF8);

                    // Показываем сообщение об успешном сохранении
                    MessageBox.Show($"Чек успешно сохранен в файл:\n{saveFileDialog.FileName}", "Чек сохранен",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Опционально: открываем файл для просмотра
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
    }
}