using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;
using Smirnov_kursovaya.Helpers;
using Smirnov_kursovaya.Models;

namespace Smirnov_kursovaya.secondForm
{
    public partial class NewOrderForm : Form
    {
        private DatabaseHelper dbHelper;
        private List<OrderItem> cartItems; // ИСПРАВЛЕНО: указан тип
        private decimal subtotal = 0;
        private decimal discountPercent = 0;
        private DataTable clientsData;

        // Класс для хранения элементов корзины
        public class OrderItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }

        // Класс для элемента ComboBox
        private class ClientItem
        {
            public int Id { get; set; }
            public string Display { get; set; }

            public override string ToString()
            {
                return Display;
            }
        }

        public NewOrderForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            cartItems = new List<OrderItem>(); // ИСПРАВЛЕНО: указан тип
            InitializeControls();
            LoadData();
        }

        private void InitializeControls()
        {
            // Настройка DataGridView для товаров с изображениями
            SetupProductsDataGridView();

            // Настройка DataGridView для корзины
            SetupCartDataGridView();

            // Настройка дат
            orderDateLabel.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            completionDatePicker.Value = DateTime.Now.AddDays(1);
            completionDatePicker.MinDate = DateTime.Now;

            // Настройка скидок
            discountComboBox.Items.AddRange(new object[] { "0%", "5%", "10%", "15%", "20%" });
            discountComboBox.SelectedIndex = 0;

            // Подсказки
            SetPlaceholderText(searchProductsTextBox, "Поиск по названию или артикулу...");
            SetPlaceholderText(searchClientTextBox, "Поиск клиента по ФИО или телефону...");

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

        private void SetupProductsDataGridView()
        {
            // Очищаем все колонки
            productsDataGridView.Columns.Clear();

            // Добавляем колонку с изображением - УВЕЛИЧЕННЫЕ РАЗМЕРЫ
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "image";
            imageColumn.HeaderText = "Фото";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 120; // УВЕЛИЧЕНО: было 60, стало 120
            imageColumn.MinimumWidth = 100; // Минимальная ширина
            imageColumn.FillWeight = 15; // Вес колонки при автоматическом заполнении

            // Настройка высоты строк для лучшего отображения изображений
            productsDataGridView.RowTemplate.Height = 100; // Устанавливаем высоту строки 100 пикселей
            productsDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; // Отключаем авторазмер

            productsDataGridView.Columns.Add(imageColumn);

            // Добавляем остальные колонки
            productsDataGridView.Columns.Add("id", "ID");
            productsDataGridView.Columns.Add("article", "Артикул");
            productsDataGridView.Columns.Add("name", "Название");
            productsDataGridView.Columns.Add("category", "Категория");
            productsDataGridView.Columns.Add("price", "Цена");
            productsDataGridView.Columns.Add("description", "Описание");

            // Настройка колонок
            productsDataGridView.Columns["id"].Visible = false;
            productsDataGridView.Columns["price"].DefaultCellStyle.Format = "C2";
            productsDataGridView.Columns["description"].Visible = false;

            // Настройка ширины остальных колонок
            productsDataGridView.Columns["article"].Width = 100;
            productsDataGridView.Columns["name"].Width = 200;
            productsDataGridView.Columns["category"].Width = 150;
            productsDataGridView.Columns["price"].Width = 100;

            productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // ИЗМЕНЕНО: было Fill, теперь None для ручного контроля
            productsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productsDataGridView.ReadOnly = true;
            productsDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            productsDataGridView.GridColor = Color.LightGray;
            productsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Устанавливаем чередование цветов строк
            productsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);

            // Цвет выделенной строки
            productsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            productsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Цвет заголовков
            productsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            productsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            productsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            productsDataGridView.ColumnHeadersHeight = 50; // УВЕЛИЧЕНО: было 40, стало 50
            productsDataGridView.EnableHeadersVisualStyles = false;
        }

        private void SetupCartDataGridView()
        {
            cartDataGridView.Columns.Clear();

            // Создаем колонки для корзины
            cartDataGridView.Columns.Add("ProductId", "ID товара");
            cartDataGridView.Columns.Add("ProductName", "Название");
            cartDataGridView.Columns.Add("Price", "Цена");
            cartDataGridView.Columns.Add("Quantity", "Количество");
            cartDataGridView.Columns.Add("Total", "Сумма");

            // Скрываем колонку ID товара
            cartDataGridView.Columns["ProductId"].Visible = false;

            // Форматируем колонки с деньгами
            cartDataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
            cartDataGridView.Columns["Total"].DefaultCellStyle.Format = "C2";

            cartDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cartDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cartDataGridView.ReadOnly = true;
            cartDataGridView.RowHeadersVisible = false;
        }

        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
            textBox.Tag = placeholder; // Сохраняем placeholder в Tag

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

        private void LoadData()
        {
            LoadClients();
            LoadProducts();
        }

        private void LoadClients()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT id, fio, phone FROM clients ORDER BY fio";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        clientsData = new DataTable();
                        adapter.Fill(clientsData);
                        UpdateClientComboBox("");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateClientComboBox(string searchText)
        {
            clientComboBox.Items.Clear();

            if (clientsData != null)
            {
                var rows = clientsData.Select($"fio LIKE '%{searchText}%' OR phone LIKE '%{searchText}%'");

                foreach (DataRow row in rows)
                {
                    clientComboBox.Items.Add(new ClientItem
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Display = $"{row["fio"]} ({row["phone"]})"
                    });
                }

                if (clientComboBox.Items.Count > 0)
                {
                    clientComboBox.DisplayMember = "Display";
                    clientComboBox.ValueMember = "Id";
                }
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT p.id, p.article, p.name, c.name as category, 
                            p.price, p.description, p.image
                            FROM products p 
                            INNER JOIN categories c ON p.category_id = c.id
                            ORDER BY p.name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        productsDataGridView.Rows.Clear();

                        while (reader.Read())
                        {
                            int rowIndex = productsDataGridView.Rows.Add();
                            DataGridViewRow row = productsDataGridView.Rows[rowIndex];

                            // Изображение - УЛУЧШЕННАЯ ОБРАБОТКА
                            byte[] imageData = reader["image"] != DBNull.Value ?
                                (byte[])reader["image"] : null;

                            if (imageData != null && imageData.Length > 0)
                            {
                                try
                                {
                                    using (var ms = new System.IO.MemoryStream(imageData))
                                    {
                                        // Загружаем оригинальное изображение
                                        Image originalImage = Image.FromStream(ms);

                                        // Создаем копию изображения, чтобы избежать проблем с блокировкой потока
                                        Image displayImage = new Bitmap(originalImage);

                                        // Устанавливаем изображение в ячейку
                                        row.Cells["image"].Value = displayImage;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Если не удалось загрузить изображение, создаем заглушку
                                    row.Cells["image"].Value = CreatePlaceholderImage("Нет фото");
                                    Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                                }
                            }
                            else
                            {
                                // Создаем изображение-заглушку
                                row.Cells["image"].Value = CreatePlaceholderImage("Нет фото");
                            }

                            // Остальные данные
                            row.Cells["id"].Value = reader["id"];
                            row.Cells["article"].Value = reader["article"];
                            row.Cells["name"].Value = reader["name"];
                            row.Cells["category"].Value = reader["category"];
                            row.Cells["price"].Value = reader["price"];
                            row.Cells["description"].Value = reader["description"]?.ToString() ?? "";

                            // Устанавливаем высоту строки индивидуально (опционально)
                            row.Height = 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Вспомогательный метод для создания изображения-заглушки
        private Image CreatePlaceholderImage(string text)
        {
            Bitmap placeholder = new Bitmap(100, 80); // Размер под нашу ячейку
            using (Graphics g = Graphics.FromImage(placeholder))
            {
                // Заливаем фон светло-серым
                g.Clear(Color.LightGray);

                // Рисуем рамку
                using (Pen pen = new Pen(Color.Gray, 1))
                {
                    g.DrawRectangle(pen, 0, 0, placeholder.Width - 1, placeholder.Height - 1);
                }

                // Рисуем текст
                using (Font font = new Font("Arial", 8, FontStyle.Regular))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    using (Brush brush = new SolidBrush(Color.DimGray))
                    {
                        g.DrawString(text, font, brush,
                            new RectangleF(0, 0, placeholder.Width, placeholder.Height), sf);
                    }
                }
            }
            return placeholder;
        }

        private void NewOrderForm_Load(object sender, EventArgs e)
        {
            UpdateCartDisplay();
        }

        private void addToCartButton_Click(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для добавления в корзину", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = productsDataGridView.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["id"].Value);
            string productName = selectedRow.Cells["name"].Value.ToString();
            decimal price = Convert.ToDecimal(selectedRow.Cells["price"].Value);

            // Проверяем, есть ли товар уже в корзине
            OrderItem existingItem = cartItems.Find(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.Total = existingItem.Quantity * existingItem.Price;
            }
            else
            {
                OrderItem newItem = new OrderItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = 1,
                    Total = price
                };
                cartItems.Add(newItem);
            }

            UpdateCartDisplay();
            CalculateTotals();
        }

        private void removeFromCartButton_Click(object sender, EventArgs e)
        {
            if (cartDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления из корзины", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = cartDataGridView.SelectedRows[0];
            if (selectedRow.Cells["ProductId"].Value == null)
            {
                return;
            }

            int productId = Convert.ToInt32(selectedRow.Cells["ProductId"].Value);

            cartItems.RemoveAll(item => item.ProductId == productId);
            UpdateCartDisplay();
            CalculateTotals();
        }

        private void updateQuantityButton_Click(object sender, EventArgs e)
        {
            if (cartDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для изменения количества", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string quantityText = quantityTextBox.Text;
            if (quantityText == "1" || quantityText == quantityTextBox.Tag?.ToString())
            {
                MessageBox.Show("Введите количество", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(quantityText, out int newQuantity) || newQuantity <= 0)
            {
                MessageBox.Show("Введите корректное количество (больше 0)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow selectedRow = cartDataGridView.SelectedRows[0];
            if (selectedRow.Cells["ProductId"].Value == null)
            {
                return;
            }

            int productId = Convert.ToInt32(selectedRow.Cells["ProductId"].Value);

            OrderItem item = cartItems.Find(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                item.Total = item.Quantity * item.Price;
            }

            UpdateCartDisplay();
            CalculateTotals();
        }

        private void UpdateCartDisplay()
        {
            cartDataGridView.Rows.Clear();

            foreach (var item in cartItems)
            {
                int rowIndex = cartDataGridView.Rows.Add();
                DataGridViewRow row = cartDataGridView.Rows[rowIndex];

                row.Cells["ProductId"].Value = item.ProductId;
                row.Cells["ProductName"].Value = item.ProductName;
                row.Cells["Price"].Value = item.Price;
                row.Cells["Quantity"].Value = item.Quantity;
                row.Cells["Total"].Value = item.Total;
            }
        }

        private void CalculateTotals()
        {
            subtotal = 0;
            foreach (var item in cartItems)
            {
                subtotal += item.Total;
            }

            decimal discountAmount = subtotal * (discountPercent / 100);
            decimal total = subtotal - discountAmount;

            subtotalLabel.Text = total.ToString("C2"); // ИСПРАВЛЕНО: было subtotalLabel, теперь total
            discountAmountLabel.Text = discountAmount.ToString("C2");
            totalLabel.Text = total.ToString("C2");
        }

        private void discountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (discountComboBox.SelectedItem != null)
            {
                string discountText = discountComboBox.SelectedItem.ToString();
                discountText = discountText.Replace("%", "");
                if (decimal.TryParse(discountText, out discountPercent))
                {
                    CalculateTotals();
                }
            }
        }

        private void createOrderButton_Click(object sender, EventArgs e)
        {
            if (clientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Добавьте товары в корзину", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ClientItem selectedClient = (ClientItem)clientComboBox.SelectedItem; // ИСПРАВЛЕНО: явное приведение типа
                int clientId = selectedClient.Id;

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Создаем заказ
                            string orderQuery = @"INSERT INTO orders (client_id, product_id, date_of_creation, 
                                                date_of_completion, status_id, discount, total_amount, 
                                                final_amount, notes, order_number) 
                                                VALUES (@client_id, @product_id, @date_of_creation, @date_of_completion, 
                                                1, @discount, @total_amount, @final_amount, @notes, @order_number);
                                                SELECT LAST_INSERT_ID();";

                            string orderNumber = GenerateOrderNumber();
                            decimal totalAmount = subtotal;

                            // Получаем итоговую сумму из totalLabel
                            string totalText = totalLabel.Text.Replace("₽", "").Replace("$", "").Replace(" ", "").Replace("руб", "").Trim();

                            if (!decimal.TryParse(totalText, out decimal finalAmount))
                            {
                                finalAmount = subtotal * (1 - discountPercent / 100);
                            }

                            long orderId;

                            using (var orderCommand = new MySqlCommand(orderQuery, connection, transaction))
                            {
                                orderCommand.Parameters.AddWithValue("@client_id", clientId);
                                orderCommand.Parameters.AddWithValue("@product_id", cartItems[0].ProductId); // Берем первый товар
                                orderCommand.Parameters.AddWithValue("@date_of_creation", DateTime.Now);
                                orderCommand.Parameters.AddWithValue("@date_of_completion", completionDatePicker.Value);
                                orderCommand.Parameters.AddWithValue("@discount", $"{discountPercent}%");
                                orderCommand.Parameters.AddWithValue("@total_amount", totalAmount);
                                orderCommand.Parameters.AddWithValue("@final_amount", finalAmount);
                                orderCommand.Parameters.AddWithValue("@notes", DBNull.Value);
                                orderCommand.Parameters.AddWithValue("@order_number", orderNumber);

                                orderId = Convert.ToInt64(orderCommand.ExecuteScalar());
                            }

                            // Добавляем товары в заказ
                            foreach (var item in cartItems)
                            {
                                string itemQuery = @"INSERT INTO order_items (order_id, product_id, 
                                                   quantity, price, total) 
                                                   VALUES (@order_id, @product_id, @quantity, 
                                                   @price, @total)";

                                using (var itemCommand = new MySqlCommand(itemQuery, connection, transaction))
                                {
                                    itemCommand.Parameters.AddWithValue("@order_id", orderId);
                                    itemCommand.Parameters.AddWithValue("@product_id", item.ProductId);
                                    itemCommand.Parameters.AddWithValue("@quantity", item.Quantity);
                                    itemCommand.Parameters.AddWithValue("@price", item.Price);
                                    itemCommand.Parameters.AddWithValue("@total", item.Total);

                                    itemCommand.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();

                            MessageBox.Show($"Заказ №{orderNumber} успешно создан!", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ClearForm();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Ошибка создания заказа: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateOrderNumber()
        {
            return $"ORD{DateTime.Now:yyyyMMddHHmmss}";
        }

        private void ClearForm()
        {
            cartItems.Clear();
            UpdateCartDisplay();
            CalculateTotals();
            discountComboBox.SelectedIndex = 0;

            // Сброс поисковых полей
            searchProductsTextBox.Text = "Поиск по названию или артикулу...";
            searchProductsTextBox.ForeColor = Color.Gray;

            searchClientTextBox.Text = "Поиск клиента по ФИО или телефону...";
            searchClientTextBox.ForeColor = Color.Gray;

            clientComboBox.SelectedIndex = -1;
            quantityTextBox.Text = "1";
            quantityTextBox.ForeColor = Color.Gray;
        }

        private void searchProductsTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchProductsTextBox.Text;
            if (searchText == "Поиск по названию или артикулу...")
                return;

            foreach (DataGridViewRow row in productsDataGridView.Rows)
            {
                if (row.Cells["name"].Value == null) continue;

                string name = row.Cells["name"].Value?.ToString() ?? "";
                string article = row.Cells["article"].Value?.ToString() ?? "";

                bool visible = string.IsNullOrEmpty(searchText) ||
                              name.ToLower().Contains(searchText.ToLower()) ||
                              article.ToLower().Contains(searchText.ToLower());

                row.Visible = visible;
            }
        }

        private void searchClientTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchClientTextBox.Text;
            if (searchText == "Поиск клиента по ФИО или телефону...")
                return;

            UpdateClientComboBox(searchText);
        }

        private void cartDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (cartDataGridView.SelectedRows.Count > 0 &&
                cartDataGridView.SelectedRows[0].Cells["Quantity"].Value != null)
            {
                DataGridViewRow selectedRow = cartDataGridView.SelectedRows[0];
                quantityTextBox.Text = selectedRow.Cells["Quantity"].Value.ToString();
                quantityTextBox.ForeColor = Color.Black;
            }
            else
            {
                quantityTextBox.Text = "1";
                quantityTextBox.ForeColor = Color.Gray;
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void clearCartButton_Click(object sender, EventArgs e)
        {
            if (cartItems.Count > 0)
            {
                DialogResult result = MessageBox.Show("Очистить всю корзину?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cartItems.Clear();
                    UpdateCartDisplay();
                    CalculateTotals();
                }
            }
        }

        private void quantityTextBox_Enter(object sender, EventArgs e)
        {
            if (quantityTextBox.Text == "1" || quantityTextBox.Text == quantityTextBox.Tag?.ToString())
            {
                quantityTextBox.Text = "";
                quantityTextBox.ForeColor = Color.Black;
            }
        }

        private void quantityTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(quantityTextBox.Text))
            {
                quantityTextBox.Text = "1";
                quantityTextBox.ForeColor = Color.Gray;
            }
        }

        private void searchProductsTextBox_Enter(object sender, EventArgs e)
        {
            if (searchProductsTextBox.Text == "Поиск по названию или артикулу...")
            {
                searchProductsTextBox.Text = "";
                searchProductsTextBox.ForeColor = Color.Black;
            }
        }

        private void searchProductsTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchProductsTextBox.Text))
            {
                searchProductsTextBox.Text = "Поиск по названию или артикулу...";
                searchProductsTextBox.ForeColor = Color.Gray;
            }
        }

        private void searchClientTextBox_Enter(object sender, EventArgs e)
        {
            if (searchClientTextBox.Text == "Поиск клиента по ФИО или телефону...")
            {
                searchClientTextBox.Text = "";
                searchClientTextBox.ForeColor = Color.Black;
            }
        }

        private void searchClientTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchClientTextBox.Text))
            {
                searchClientTextBox.Text = "Поиск клиента по ФИО или телефону...";
                searchClientTextBox.ForeColor = Color.Gray;
            }
        }
    }
}