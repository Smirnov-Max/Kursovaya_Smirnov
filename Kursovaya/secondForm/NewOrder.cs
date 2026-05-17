using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class NewOrderForm : Form
    {
        private DatabaseHelper dbHelper;
        private List<OrderItem> cartItems;
        private decimal subtotal = 0;
        private decimal discountPercent = 0;
        private decimal discountAmount = 0;
        private string orderNumber = "";

        // Выбранный клиент. Заполняется только из формы «Клиенты» (режим выбора).
        private int selectedClientId = 0;
        private string selectedClientName = "";
        private string selectedClientPhone = "";

        private readonly string imagesFolderPath;
        private readonly string imagesResourceFolder;

        public class OrderItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }

        public NewOrderForm()
        {
            InitializeComponent();

            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Smirnov_kursovaya"
            );
            imagesFolderPath = Path.Combine(appDataPath, "ProductImages");
            if (!Directory.Exists(imagesFolderPath))
                Directory.CreateDirectory(imagesFolderPath);

            imagesResourceFolder = Path.Combine(Application.StartupPath, "Resources");
            if (!Directory.Exists(imagesResourceFolder))
            {
                string devPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources"));
                if (Directory.Exists(devPath))
                    imagesResourceFolder = devPath;
            }

            dbHelper = new DatabaseHelper();
            cartItems = new List<OrderItem>();
            EnsureOrderDateTimeColumn();
            InitializeControls();
            LoadProducts();
        }

        private void InitializeControls()
        {
            SetupProductsDataGridView();
            SetupCartDataGridView();

            orderDateLabel.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            completionDatePicker.Value = DateTime.Now.AddDays(1);
            completionDatePicker.MinDate = DateTime.Now;

            // Сразу показываем сгенерированный номер заказа в формате 000001
            orderNumber = GenerateNextOrderNumber();
            orderNumberValueLabel.Text = orderNumber;

            SetPlaceholderText(searchProductsTextBox, "Поиск по названию или артикулу...");

            // Корзина: управление количеством — только клавишами вверх / вниз
            cartDataGridView.KeyDown += CartDataGridView_KeyDown;
            cartDataGridView.PreviewKeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                    e.IsInputKey = true;
            };

            ApplyCoralButtonStyle();
            SetupResponsiveLayout();

            foreach (Control ctrl in this.Controls)
                if (ctrl is DataGridView dgv)
                    dgv.DataBindingComplete += (s, e) => { if (dgv.Columns.Contains("id")) dgv.Columns["id"].Visible = false; };

            UpdateClientInfoLabel();
            CalculateTotals();
        }

        private void SetupProductsDataGridView()
        {
            productsDataGridView.Columns.Clear();

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
            {
                Name = "image",
                HeaderText = "Фото",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 110,
                MinimumWidth = 90
            };
            productsDataGridView.Columns.Add(imageColumn);

            productsDataGridView.Columns.Add("id", "ID");
            productsDataGridView.Columns.Add("article", "Артикул");
            productsDataGridView.Columns.Add("name", "Название");
            productsDataGridView.Columns.Add("category", "Категория");
            productsDataGridView.Columns.Add("price", "Цена");
            productsDataGridView.Columns.Add("description", "Описание");

            productsDataGridView.Columns["id"].Visible = false;
            productsDataGridView.Columns["price"].DefaultCellStyle.Format = "C2";
            productsDataGridView.Columns["description"].Visible = false;

            // Колонки распределены по всей ширине грида (Fill).
            productsDataGridView.Columns["image"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            productsDataGridView.Columns["image"].Width = 110;
            productsDataGridView.Columns["article"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            productsDataGridView.Columns["article"].FillWeight = 18;
            productsDataGridView.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            productsDataGridView.Columns["name"].FillWeight = 38;
            productsDataGridView.Columns["category"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            productsDataGridView.Columns["category"].FillWeight = 26;
            productsDataGridView.Columns["price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            productsDataGridView.Columns["price"].FillWeight = 18;

            productsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productsDataGridView.ReadOnly = true;
            productsDataGridView.RowHeadersVisible = false;
            productsDataGridView.RowTemplate.Height = 100;

            productsDataGridView.GridColor = Color.LightGray;
            productsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            productsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
            productsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            productsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            productsDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            productsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            productsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            productsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            productsDataGridView.ColumnHeadersHeight = 40;
            productsDataGridView.EnableHeadersVisualStyles = false;
        }

        private void SetupCartDataGridView()
        {
            cartDataGridView.Columns.Clear();
            cartDataGridView.Columns.Add("ProductId", "ID товара");
            cartDataGridView.Columns.Add("ProductName", "Название");
            cartDataGridView.Columns.Add("Price", "Цена");
            cartDataGridView.Columns.Add("Quantity", "Кол-во (↑/↓)");
            cartDataGridView.Columns.Add("Total", "Сумма");

            cartDataGridView.Columns["ProductId"].Visible = false;
            cartDataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
            cartDataGridView.Columns["Total"].DefaultCellStyle.Format = "C2";

            // Колонки растянуты на всю ширину
            cartDataGridView.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cartDataGridView.Columns["ProductName"].FillWeight = 50;
            cartDataGridView.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cartDataGridView.Columns["Price"].FillWeight = 16;
            cartDataGridView.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cartDataGridView.Columns["Quantity"].FillWeight = 17;
            cartDataGridView.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            cartDataGridView.Columns["Total"].FillWeight = 17;

            cartDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cartDataGridView.ReadOnly = true;
            cartDataGridView.RowHeadersVisible = false;
            cartDataGridView.GridColor = Color.LightGray;
            cartDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            cartDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
            cartDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            cartDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            cartDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            cartDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            cartDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            cartDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            cartDataGridView.ColumnHeadersHeight = 40;
            cartDataGridView.EnableHeadersVisualStyles = false;
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

        private void ApplyCoralButtonStyle()
        {
            Color coral = Color.FromArgb(255, 127, 80);
            Color coralLight = Color.FromArgb(255, 147, 100);
            Color coralDark = Color.FromArgb(235, 107, 60);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                    ApplyButtonStyle(btn, coral, coralLight, coralDark);
                else if (ctrl is GroupBox grp)
                    foreach (Control sub in grp.Controls)
                        if (sub is Button b) ApplyButtonStyle(b, coral, coralLight, coralDark);
            }

            if (menuButton != null)
            {
                menuButton.BackColor = Color.Red;
                menuButton.FlatAppearance.BorderColor = Color.DarkRed;
                menuButton.MouseEnter += (s, e) => menuButton.BackColor = Color.IndianRed;
                menuButton.MouseLeave += (s, e) => menuButton.BackColor = Color.Red;
            }
        }

        private void ApplyButtonStyle(Button button, Color normal, Color hover, Color pressed)
        {
            button.BackColor = normal;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(235, 107, 60);
            button.FlatAppearance.BorderSize = 1;
            button.ForeColor = Color.Black;

            button.MouseEnter += (s, e) => button.BackColor = hover;
            button.MouseLeave += (s, e) => button.BackColor = normal;
            button.MouseDown += (s, e) => button.BackColor = pressed;
            button.MouseUp += (s, e) => button.BackColor = hover;
        }

        // ==================== Генерация номера заказа ====================
        // Формат: 000001, 000002, ..., 000010, ... (минимум 6 цифр).
        private string GenerateNextOrderNumber()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT COALESCE(MAX(CAST(order_number AS UNSIGNED)), 0) + 1
                                     FROM orders
                                     WHERE order_number REGEXP '^[0-9]+$'";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        long next = Convert.ToInt64(cmd.ExecuteScalar());
                        return next.ToString("D6");
                    }
                }
            }
            catch
            {
                return "000001";
            }
        }

        // ==================== Загрузка товаров ====================
        private void LoadProducts()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT p.id, p.article, p.name, c.name as category, p.price, p.description
                                    FROM products p 
                                    INNER JOIN categories c ON p.category_id = c.id
                                    ORDER BY p.name";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        productsDataGridView.Rows.Clear();
                        while (reader.Read())
                        {
                            int idx = productsDataGridView.Rows.Add();
                            DataGridViewRow row = productsDataGridView.Rows[idx];

                            string article = reader["article"].ToString();
                            Image img = GetProductImageFromFileSystem(article);

                            row.Cells["image"].Value = img ?? CreatePlaceholderImage("Нет фото");
                            row.Cells["id"].Value = reader["id"];
                            row.Cells["article"].Value = article;
                            row.Cells["name"].Value = reader["name"];
                            row.Cells["category"].Value = reader["category"];
                            row.Cells["price"].Value = reader["price"];
                            row.Cells["description"].Value = reader["description"]?.ToString() ?? "";
                            row.Height = 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image GetProductImageFromFileSystem(string article)
        {
            string[] exts = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            foreach (var ext in exts)
            {
                string path = Path.Combine(imagesFolderPath, article + ext);
                if (File.Exists(path))
                    try { return Image.FromFile(path); } catch { }
            }
            foreach (var ext in exts)
            {
                string path = Path.Combine(imagesResourceFolder, article + ext);
                if (File.Exists(path))
                    try { return Image.FromFile(path); } catch { }
            }
            return null;
        }

        private Image CreatePlaceholderImage(string text)
        {
            Bitmap bmp = new Bitmap(100, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (Pen pen = new Pen(Color.Gray, 1))
                    g.DrawRectangle(pen, 0, 0, bmp.Width - 1, bmp.Height - 1);
                using (Font font = new Font("Arial", 8))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (Brush brush = new SolidBrush(Color.DimGray))
                    g.DrawString(text, font, brush, new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
            }
            return bmp;
        }

        // ==================== Кнопка «Выбрать клиента» ====================
        // Открывает форму «Клиенты» в режиме выбора. После закрытия по «Подтвердить выбор»
        // забираем выбранного клиента и пишем его в поля формы.
        private void clientsButton_Click(object sender, EventArgs e)
        {
            using (var clientsForm = new ClientsForm())
            {
                clientsForm.OpenedFromOrder = true;
                if (clientsForm.ShowDialog(this) == DialogResult.OK && clientsForm.SelectedClientId > 0)
                {
                    selectedClientId = clientsForm.SelectedClientId;
                    selectedClientName = clientsForm.SelectedClientName;
                    selectedClientPhone = clientsForm.SelectedClientPhone;
                    UpdateClientInfoLabel();
                }
            }
        }

        private void UpdateClientInfoLabel()
        {
            if (selectedClientId > 0)
            {
                clientInfoLabel.Text = $"  {selectedClientName} — {selectedClientPhone}";
                clientInfoLabel.ForeColor = Color.Black;
            }
            else
            {
                clientInfoLabel.Text = "  клиент не выбран";
                clientInfoLabel.ForeColor = Color.DimGray;
            }
        }

        // ==================== Корзина ====================
        private void addToCartButton_Click(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count == 0) return;
            var row = productsDataGridView.SelectedRows[0];
            int pid = Convert.ToInt32(row.Cells["id"].Value);
            string name = row.Cells["name"].Value.ToString();
            decimal price = Convert.ToDecimal(row.Cells["price"].Value);

            var existing = cartItems.FirstOrDefault(i => i.ProductId == pid);
            if (existing != null)
            {
                existing.Quantity++;
                existing.Total = existing.Quantity * existing.Price;
            }
            else
            {
                cartItems.Add(new OrderItem { ProductId = pid, ProductName = name, Price = price, Quantity = 1, Total = price });
            }

            UpdateCartDisplay();
            CalculateTotals();
        }

        private void removeFromCartButton_Click(object sender, EventArgs e)
        {
            if (cartDataGridView.SelectedRows.Count == 0) return;
            int pid = Convert.ToInt32(cartDataGridView.SelectedRows[0].Cells["ProductId"].Value);
            cartItems.RemoveAll(i => i.ProductId == pid);
            UpdateCartDisplay();
            CalculateTotals();
        }

        // Управление количеством товара клавишами ↑ / ↓.
        // ↑ — добавить (+1); ↓ — убрать (-1). Минимум 0; на 0 убрать нельзя.
        private void CartDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (cartDataGridView.SelectedRows.Count == 0) return;
            int pid = Convert.ToInt32(cartDataGridView.SelectedRows[0].Cells["ProductId"].Value);
            var item = cartItems.FirstOrDefault(i => i.ProductId == pid);
            if (item == null) return;

            if (e.KeyCode == Keys.Up)
            {
                item.Quantity++;
                item.Total = item.Quantity * item.Price;
                UpdateCartDisplay();
                CalculateTotals();
                ReselectCartRow(pid);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (item.Quantity > 0)
                {
                    item.Quantity--;
                    item.Total = item.Quantity * item.Price;
                    UpdateCartDisplay();
                    CalculateTotals();
                    ReselectCartRow(pid);
                }
                e.Handled = true;
            }
        }

        private void ReselectCartRow(int productId)
        {
            foreach (DataGridViewRow row in cartDataGridView.Rows)
            {
                if (row.Cells["ProductId"].Value != null && Convert.ToInt32(row.Cells["ProductId"].Value) == productId)
                {
                    row.Selected = true;
                    cartDataGridView.CurrentCell = row.Cells["Quantity"];
                    break;
                }
            }
        }

        private void UpdateCartDisplay()
        {
            cartDataGridView.Rows.Clear();
            foreach (var item in cartItems)
            {
                int idx = cartDataGridView.Rows.Add();
                cartDataGridView.Rows[idx].Cells["ProductId"].Value = item.ProductId;
                cartDataGridView.Rows[idx].Cells["ProductName"].Value = item.ProductName;
                cartDataGridView.Rows[idx].Cells["Price"].Value = item.Price;
                cartDataGridView.Rows[idx].Cells["Quantity"].Value = item.Quantity;
                cartDataGridView.Rows[idx].Cells["Total"].Value = item.Total;
            }
        }

        // ==================== Автоматический расчёт скидки ====================
        // 1) скидка от количества позиций; 2) скидка от суммы покупки; 3) день недели — пн = 20%.
        // Берём максимальную из применимых.
        private decimal CalculateAutoDiscount(int positionsCount, decimal sumValue, DateTime now)
        {
            decimal byPositions = 0;
            if (positionsCount >= 10) byPositions = 10;
            else if (positionsCount >= 5) byPositions = 5;

            decimal bySum = 0;
            if (sumValue >= 15000m) bySum = 10;
            else if (sumValue >= 5000m) bySum = 5;

            decimal byDay = (now.DayOfWeek == DayOfWeek.Monday) ? 20m : 0m;

            return Math.Max(byDay, Math.Max(byPositions, bySum));
        }

        private void CalculateTotals()
        {
            subtotal = cartItems.Sum(i => i.Total);
            int positions = cartItems.Count(i => i.Quantity > 0);
            discountPercent = CalculateAutoDiscount(positions, subtotal, DateTime.Now);
            discountAmount = subtotal * discountPercent / 100m;
            decimal total = subtotal - discountAmount;

            subtotalLabel.Text = subtotal.ToString("C2");
            discountAmountLabel.Text = $"{discountAmount.ToString("C2")} ({discountPercent}%)";
            totalLabel.Text = total.ToString("C2");
        }

        // ==================== Оформление заказа ====================
        private void createOrderButton_Click(object sender, EventArgs e)
        {
            if (selectedClientId <= 0)
            {
                MessageBox.Show("Выберите клиента (кнопка «Выбрать клиента»)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cartItems.Count == 0 || cartItems.All(i => i.Quantity <= 0))
            {
                MessageBox.Show("Корзина пуста", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int clientId = selectedClientId;
                long orderId;

                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        // Перегенерируем номер на момент сохранения, чтобы избежать гонки
                        orderNumber = GenerateNextOrderNumber();
                        decimal totalAmount = subtotal;
                        decimal finalAmount = totalAmount - discountAmount;

                        string orderQuery = @"INSERT INTO orders (client_id, product_id, date_of_creation, date_of_completion, status_id, discount, total_amount, final_amount, notes, order_number)
                                              VALUES (@client_id, @product_id, @date_of_creation, @date_of_completion, 1, @discount, @total_amount, @final_amount, @notes, @order_number);
                                              SELECT LAST_INSERT_ID();";
                        using (var cmd = new MySqlCommand(orderQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@client_id", clientId);
                            cmd.Parameters.AddWithValue("@product_id", cartItems[0].ProductId);
                            // Явный DateTime с временем. EnsureOrderDateTimeColumn() выше гарантирует DATETIME колонку
                            // в БД, иначе MySQL усекал бы H:M в 00:00.
                            cmd.Parameters.AddWithValue("@date_of_creation", DateTime.Now);
                            cmd.Parameters.AddWithValue("@date_of_completion", completionDatePicker.Value);
                            // Скидку храним числом (без символа %), чтобы ViewOrder распарсивал её в decimal.
                            cmd.Parameters.AddWithValue("@discount", discountPercent);
                            cmd.Parameters.AddWithValue("@total_amount", totalAmount);
                            cmd.Parameters.AddWithValue("@final_amount", finalAmount);
                            cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                            cmd.Parameters.AddWithValue("@order_number", orderNumber);
                            orderId = Convert.ToInt64(cmd.ExecuteScalar());
                        }

                        foreach (var item in cartItems)
                        {
                            if (item.Quantity <= 0) continue;
                            string itemQuery = "INSERT INTO order_items (order_id, product_id, quantity, price, total) VALUES (@oid, @pid, @qty, @price, @total)";
                            using (var cmd = new MySqlCommand(itemQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@oid", orderId);
                                cmd.Parameters.AddWithValue("@pid", item.ProductId);
                                cmd.Parameters.AddWithValue("@qty", item.Quantity);
                                cmd.Parameters.AddWithValue("@price", item.Price);
                                cmd.Parameters.AddWithValue("@total", item.Total);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                }

                MessageBox.Show($"Заказ №{orderNumber} оформлен. Статус: принят.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Переход на форму просмотра заказа
                using (var view = new ViewOrderForm((int)orderId, false))
                {
                    view.ShowDialog(this);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Прочие обработчики ====================
        private void NewOrderForm_Load(object sender, EventArgs e)
        {
            UpdateCartDisplay();
        }

        // Гарантируем, что колонка date_of_creation — именно DATETIME, иначе MySQL
        // будет усекать время и в списке заказов всегда будет 00:00.
        // Для date_of_completion — также DATETIME (храним выбранную дату).
        // Операция идемпотентна: если колонки уже DATETIME — ALTER ничего не меняет.
        private void EnsureOrderDateTimeColumn()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("ALTER TABLE orders MODIFY date_of_creation DATETIME NULL", conn))
                        cmd.ExecuteNonQuery();
                    using (var cmd = new MySqlCommand("ALTER TABLE orders MODIFY date_of_completion DATETIME NULL", conn))
                        cmd.ExecuteNonQuery();
                }
            }
            catch { /* колонки могут быть уже корректного типа — это ожидаемый случай */ }
        }

        private void productsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cartDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cartDataGridView_SelectionChanged(object sender, EventArgs e) { }

        // Поиск товаров строго с начала названия / артикула.
        private void searchProductsTextBox_TextChanged(object sender, EventArgs e)
        {
            string txt = searchProductsTextBox.Text;
            if (txt == "Поиск по названию или артикулу...") return;
            string q = (txt ?? "").ToLower();

            foreach (DataGridViewRow row in productsDataGridView.Rows)
            {
                if (row.Cells["name"].Value == null) continue;
                string name = row.Cells["name"].Value.ToString().ToLower();
                string art = row.Cells["article"].Value.ToString().ToLower();
                row.Visible = string.IsNullOrEmpty(q) || name.StartsWith(q) || art.StartsWith(q);
            }
        }

        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        private void clearCartButton_Click(object sender, EventArgs e)
        {
            if (cartItems.Count > 0 && MessageBox.Show("Очистить корзину?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cartItems.Clear();
                UpdateCartDisplay();
                CalculateTotals();
            }
        }

        // Растяжение на весь экран — расставляем якоря всем основным контролам.
        private void SetupResponsiveLayout()
        {
            this.MinimumSize = new Size(1040, 740);
            this.WindowState = FormWindowState.Maximized;

            this.Resize += (s, e) => AdjustNewOrderLayout();
            AdjustNewOrderLayout();
        }

        // Программная подгонка под макет (как на скриншоте заказчика):
        // сверху — большой грид «Товары», под ним кнопка «Добавить в корзину» (слева),
        // далее строка «Корзина:» + подсказка, ниже компактный грид корзины и панель «Итоги»
        // справа от корзины, в самом низу — три кнопки (Удалить / Очистить / Оформить).
        private void AdjustNewOrderLayout()
        {
            const int margin = 12;
            const int rightPanelWidth = 283;
            const int gap = 8;
            // Зазор между нижним краем корзины/totalsPanel и верхним краем нижней
            // строки кнопок. Делаем большим, чтобы totalsPanel визуально не «приклеивалась»
            // к кнопке «Оформить заказ» — между ними должно быть свободное пространство.
            const int bottomGap = 32;
            const int bottomBtnH = 32;
            const int createBtnH = 40;
            const int cartH = 70;
            const int cartLabelGap = 26;
            const int addBtnH = 32;

            int rightX = this.ClientSize.Width - rightPanelWidth - margin;
            int clientH = this.ClientSize.Height;

            // ==== Нижняя строка кнопок (выровнены по нижней границе формы) ====
            int btnBottom = clientH - margin;
            int bottomBtnTop = btnBottom - bottomBtnH;
            int createBtnTop = btnBottom - createBtnH;

            if (removeFromCartButton != null)
            {
                removeFromCartButton.Top = bottomBtnTop;
                removeFromCartButton.Left = margin;
            }
            if (clearCartButton != null)
            {
                clearCartButton.Top = bottomBtnTop;
                clearCartButton.Left = margin + 178;
            }
            if (createOrderButton != null)
            {
                createOrderButton.Top = createBtnTop;
                createOrderButton.Left = rightX;
                createOrderButton.Width = rightPanelWidth;
            }

            // ==== Грид корзины + панель «Итоги» (над нижней строкой кнопок) ====
            // Корзина и totalsPanel — одинаковой высоты и одной горизонтальной полосой.
            // Высоту корзины увеличиваем (= высота totalsPanel), чтобы было видно
            // несколько строк сразу. Над ними — заголовок «Корзина:» и подсказка.
            int cartBottom = createBtnTop - bottomGap;
            int rowH = cartH + cartLabelGap;  // итоговая высота полосы (корзина = totalsPanel)
            int rowTop = cartBottom - rowH;

            if (cartDataGridView != null)
            {
                cartDataGridView.Left = margin;
                cartDataGridView.Top = rowTop;
                cartDataGridView.Width = rightX - margin - gap;
                cartDataGridView.Height = rowH;
            }

            // Панель «Итоги» — на той же горизонтали, та же высота что и корзина.
            if (totalsPanel != null)
            {
                totalsPanel.Top = rowTop;
                totalsPanel.Left = rightX;
                totalsPanel.Width = rightPanelWidth;
                totalsPanel.Height = rowH;
            }

            // Надпись «Корзина:» и подсказка в одной строке НАД гридом корзины.
            int cartLabelTop = rowTop - cartLabelGap;
            if (label7 != null)
            {
                label7.Top = cartLabelTop + 4;
                label7.Left = margin;
            }
            if (hintLabel != null)
            {
                hintLabel.Top = cartLabelTop + 7;
                hintLabel.Left = margin + 318;
            }

            // ==== Кнопка «Добавить в корзину» — под гридом товаров слева ====
            int addBtnTop = cartLabelTop - gap - addBtnH;
            if (addToCartButton != null)
            {
                addToCartButton.Top = addBtnTop;
                addToCartButton.Left = margin;
            }

            // ==== Грид «Товары» — занимает всё свободное пространство сверху ====
            if (productsDataGridView != null)
            {
                productsDataGridView.Left = margin;
                productsDataGridView.Width = this.ClientSize.Width - 2 * margin;
                int top = productsDataGridView.Top;
                int newHeight = addBtnTop - gap - top;
                if (newHeight < 180) newHeight = 180;
                productsDataGridView.Height = newHeight;
            }
        }
    }
}
