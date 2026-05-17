using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;
using Smirnov_kursovaya.Helpers;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ProductsForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool isEditMode = false;
        private int currentProductId = 0;
        private bool readOnlyMode = false;

        // Поля для пагинации
        private int currentPage = 1;
        private int pageSize = 20;
        private int totalRecords = 0;
        private int totalPages = 1;
        private string currentSearchText = "";
        private string currentCategory = "";

        // Хранит временный путь к изображению до сохранения
        private string tempImagePath = null;

        // Путь к папке с пользовательскими изображениями (AppData)
        private readonly string imagesFolderPath;
        // Путь к папке Resources со стандартными изображениями товаров
        private readonly string imagesResourceFolder;

        // Для временного перемещения товара наверх после добавления/редактирования
        private bool moveToTop = false;
        private int? highlightProductId = null;

        // Лимиты на размер изображения
        private const long IMAGE_TARGET_BYTES = 2L * 1024 * 1024;       // 2 МБ — целевой размер при сжатии
        private const long IMAGE_HARD_LIMIT_BYTES = (long)(2.5 * 1024 * 1024); // 2.5 МБ — жёсткий лимит, если сжатие не удалось

        public ProductsForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Smirnov_kursovaya"
            );
            imagesFolderPath = Path.Combine(appDataPath, "ProductImages");
            if (!Directory.Exists(imagesFolderPath))
                Directory.CreateDirectory(imagesFolderPath);

            // Путь к папке Resources (изображения по умолчанию)
            imagesResourceFolder = Path.Combine(Application.StartupPath, "Resources");
            // Если папка не найдена, пробуем относительный путь (для разработки)
            if (!Directory.Exists(imagesResourceFolder))
            {
                string devPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources"));
                if (Directory.Exists(devPath))
                    imagesResourceFolder = devPath;
            }

            InitializeControls();
            LoadCategories();
            LoadProductsPage(1);
            this.Resize += ProductsForm_Resize;
        }

        private void ProductsForm_Load(object sender, EventArgs e) { }

        // ==================== Инициализация ====================
        private void InitializeControls()
        {
            SetPlaceholderText(searchTextBox, "Поиск по названию...");
            SetPlaceholderText(nameTextBox, "Название товара");

            articleTextBox.ReadOnly = true;
            articleTextBox.BackColor = Color.FromArgb(240, 240, 240);
            articleTextBox.TabStop = false;
            articleTextBox.Text = "Авто";
            articleTextBox.ForeColor = Color.Gray;

            priceTextBox.MaxLength = 3;
            priceTextBox.Text = "";
            SetPlaceholderText(priceTextBox, "Цена (до 999)");

            nameTextBox.MaxLength = 100;
            descriptionTextBox.MaxLength = 500;

            SetupDataGridView();

            productPictureBox.BorderStyle = BorderStyle.FixedSingle;
            productPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            ApplyCoralButtonStyle();
            SetupResponsiveLayout();

            if (productsDataGridView.Columns.Contains("image"))
                productsDataGridView.Columns["image"].Visible = false;
        }

        // Обработчики для соответствия дизайнеру
        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e) { }
        private void articleTextBox_KeyPress(object sender, KeyPressEventArgs e) { }
        private void priceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем цифры, управляющие символы и один разделитель (запятую или точку)
            if (char.IsControl(e.KeyChar)) return;
            if (char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == ',' || e.KeyChar == '.')
            {
                if (priceTextBox.Text.Contains(",") || priceTextBox.Text.Contains(".")) e.Handled = true;
                return;
            }
            e.Handled = true;
        }

        public void SetReadOnlyMode()
        {
            readOnlyMode = true;

            addButton.Visible = false;
            editButton.Visible = false;
            deleteButton.Visible = false;
            addImageButton.Visible = false;
            removeImageButton.Visible = false;

            if (groupBox1 != null) groupBox1.Visible = false;
            if (productPictureBox != null) productPictureBox.Visible = false;

            nameTextBox.ReadOnly = true;
            articleTextBox.ReadOnly = true;
            priceTextBox.ReadOnly = true;
            descriptionTextBox.ReadOnly = true;
            categoryComboBox.Enabled = false;

            nameTextBox.BackColor = Color.FromArgb(240, 240, 240);
            articleTextBox.BackColor = Color.FromArgb(240, 240, 240);
            priceTextBox.BackColor = Color.FromArgb(240, 240, 240);
            descriptionTextBox.BackColor = Color.FromArgb(240, 240, 240);
            categoryComboBox.BackColor = Color.FromArgb(240, 240, 240);

            ClearPlaceholderText(nameTextBox, "Название товара");
            ClearPlaceholderText(priceTextBox, "Цена (до 999)");

            this.Text = "Просмотр товаров (режим продавца)";
            label1.Text = "Просмотр товаров";

            if (productsDataGridView.Columns.Contains("image"))
                productsDataGridView.Columns["image"].Visible = true;

            AdjustLayoutForResize();
        }

        private void ClearPlaceholderText(TextBox textBox, string placeholder)
        {
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        private void AdjustLayoutForResize()
        {
            if (readOnlyMode)
            {
                if (productsDataGridView != null)
                {
                    productsDataGridView.Top = categoryFilterComboBox.Bottom + 20;
                    productsDataGridView.Left = 10;
                    productsDataGridView.Width = this.ClientSize.Width - 20;
                    productsDataGridView.Height = this.ClientSize.Height - productsDataGridView.Top - 60;
                    productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            else
            {
                if (productsDataGridView != null)
                    productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void ProductsForm_Resize(object sender, EventArgs e) => AdjustLayoutForResize();

        private void SetupDataGridView()
        {
            productsDataGridView.Columns.Clear();

            DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
            imageCol.Name = "image";
            imageCol.HeaderText = "Изображение";
            imageCol.DataPropertyName = "image"; // будет переопределено в CellFormatting
            imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageCol.Width = 120;
            productsDataGridView.Columns.Add(imageCol);

            productsDataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "id", HeaderText = "ID", DataPropertyName = "id" });
            productsDataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "article", HeaderText = "Артикул", DataPropertyName = "article" });
            productsDataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "name", HeaderText = "Название", DataPropertyName = "name" });
            productsDataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "category_name", HeaderText = "Категория", DataPropertyName = "category_name" });
            productsDataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "price", HeaderText = "Цена", DataPropertyName = "price" });
            productsDataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "description", HeaderText = "Описание", DataPropertyName = "description" });

            productsDataGridView.Columns["id"].Visible = false;
            if (productsDataGridView.Columns.Contains("category_id"))
                productsDataGridView.Columns["category_id"].Visible = false;

            // Формат цены: "14,00 руб." (русская локаль, две цифры после запятой)
            productsDataGridView.Columns["price"].DefaultCellStyle.Format = "0.00\" руб.\"";

            if (productsDataGridView.Columns.Contains("name"))
                productsDataGridView.Columns["name"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            if (productsDataGridView.Columns.Contains("description"))
                productsDataGridView.Columns["description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Ширина колонок: описание шире, цена/артикул уже
            productsDataGridView.Columns["article"].FillWeight = 60;
            productsDataGridView.Columns["name"].FillWeight = 110;
            productsDataGridView.Columns["category_name"].FillWeight = 90;
            productsDataGridView.Columns["price"].FillWeight = 70;
            productsDataGridView.Columns["description"].FillWeight = 240;

            productsDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            productsDataGridView.RowTemplate.MinimumHeight = 40;

            productsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);
            productsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            productsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            productsDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            productsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            productsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            productsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            productsDataGridView.ColumnHeadersHeight = 40;
            productsDataGridView.EnableHeadersVisualStyles = false;

            productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            productsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productsDataGridView.ReadOnly = true;
            productsDataGridView.RowHeadersVisible = false;
            productsDataGridView.AllowUserToAddRows = false;
            productsDataGridView.AllowUserToDeleteRows = false;
            productsDataGridView.MultiSelect = false;
            productsDataGridView.GridColor = Color.LightGray;
            productsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            productsDataGridView.RowTemplate.Height = 130;

            // Подмена изображения в ячейке
            productsDataGridView.CellFormatting += (s, e) =>
            {
                if (productsDataGridView.Columns[e.ColumnIndex].Name == "image" && e.RowIndex >= 0)
                {
                    string article = productsDataGridView.Rows[e.RowIndex].Cells["article"].Value?.ToString();
                    if (!string.IsNullOrEmpty(article))
                    {
                        e.Value = GetProductImage(article);
                    }
                    else
                    {
                        e.Value = ImageHelper.Placeholder.Clone();
                    }
                    e.FormattingApplied = true;
                }
            };
        }

        // ==================== Получение изображения товара ====================
        private Image GetProductImage(string article)
        {
            string[] extensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            // 1. Пользовательские изображения (AppData)
            foreach (var ext in extensions)
            {
                string filePath = Path.Combine(imagesFolderPath, article + ext);
                if (File.Exists(filePath))
                {
                    try
                    {
                        return Image.FromFile(filePath);
                    }
                    catch { }
                }
            }
            // 2. Стандартные изображения из Resources
            foreach (var ext in extensions)
            {
                string filePath = Path.Combine(imagesResourceFolder, article + ext);
                if (File.Exists(filePath))
                {
                    try
                    {
                        return Image.FromFile(filePath);
                    }
                    catch { }
                }
            }
            // 3. Заглушка
            return ImageHelper.Placeholder.Clone() as Image;
        }

        // ==================== Загрузка данных ====================
        private void LoadCategories()
        {
            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, name FROM categories ORDER BY name";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        categoryComboBox.Items.Clear();
                        categoryFilterComboBox.Items.Clear();
                        categoryFilterComboBox.Items.Add("Все категории");

                        while (reader.Read())
                        {
                            var item = new { Id = reader["id"], Name = reader["name"].ToString() };
                            categoryComboBox.Items.Add(item);
                            categoryFilterComboBox.Items.Add(item);
                        }
                        categoryComboBox.DisplayMember = "Name";
                        categoryComboBox.ValueMember = "Id";
                        categoryFilterComboBox.DisplayMember = "Name";
                        categoryFilterComboBox.ValueMember = "Id";
                        categoryFilterComboBox.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Загрузка товаров с собственной пагинацией =====
        // Поиск по названию — совпадение с начала поля.
        private void LoadProductsPage(int page)
        {
            try
            {
                DataTable dt = LoadProductsFromDb(page, pageSize, currentSearchText, currentCategory, out totalRecords);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages == 0) totalPages = 1;
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;
                currentPage = page;

                // Временное перемещение выделенного товара наверх
                if (moveToTop && highlightProductId.HasValue)
                {
                    bool found = MoveRowToTop(dt, highlightProductId.Value);
                    if (!found)
                    {
                        DataRow productRow = GetProductRowById(highlightProductId.Value);
                        if (productRow != null)
                        {
                            DataRow newRow = dt.NewRow();
                            newRow.ItemArray = productRow.ItemArray;
                            dt.Rows.InsertAt(newRow, 0);
                            if (dt.Rows.Count > pageSize)
                                dt.Rows.RemoveAt(dt.Rows.Count - 1);
                        }
                    }
                    moveToTop = false;
                    highlightProductId = null;
                }

                productsDataGridView.DataSource = dt;

                if (productsDataGridView.Columns.Contains("id"))
                    productsDataGridView.Columns["id"].Visible = false;
                if (productsDataGridView.Columns.Contains("category_id"))
                    productsDataGridView.Columns["category_id"].Visible = false;
                if (productsDataGridView.Columns.Contains("image"))
                    productsDataGridView.Columns["image"].Visible = readOnlyMode;

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Загрузка с поиском по началу названия (LIKE 'text%') и фильтром категории
        private DataTable LoadProductsFromDb(int page, int size, string search, string category, out int total)
        {
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();

                string whereSearch = string.IsNullOrEmpty(search) ? "" : " AND p.name LIKE @search";
                string whereCategory = string.IsNullOrEmpty(category) ? "" : " AND c.name = @category";

                string countQuery = $@"SELECT COUNT(*) FROM products p
                                       LEFT JOIN categories c ON p.category_id = c.id
                                       WHERE 1=1 {whereSearch} {whereCategory}";
                using (var cntCmd = new MySqlCommand(countQuery, conn))
                {
                    if (!string.IsNullOrEmpty(search)) cntCmd.Parameters.AddWithValue("@search", search + "%");
                    if (!string.IsNullOrEmpty(category)) cntCmd.Parameters.AddWithValue("@category", category);
                    total = Convert.ToInt32(cntCmd.ExecuteScalar());
                }

                int offset = (page - 1) * size;
                string query = $@"SELECT p.id, p.article, p.name, c.name AS category_name, p.price, p.description
                                  FROM products p
                                  LEFT JOIN categories c ON p.category_id = c.id
                                  WHERE 1=1 {whereSearch} {whereCategory}
                                  ORDER BY p.id DESC
                                  LIMIT @size OFFSET @offset";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@search", search + "%");
                    if (!string.IsNullOrEmpty(category)) cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@size", size);
                    cmd.Parameters.AddWithValue("@offset", offset);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private bool MoveRowToTop(DataTable table, int productId)
        {
            if (table == null || table.Rows.Count == 0) return false;
            DataRow targetRow = null;
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["id"]) == productId)
                {
                    targetRow = row;
                    break;
                }
            }
            if (targetRow != null)
            {
                DataRow newRow = table.NewRow();
                newRow.ItemArray = targetRow.ItemArray;
                table.Rows.Remove(targetRow);
                table.Rows.InsertAt(newRow, 0);
                return true;
            }
            return false;
        }

        private DataRow GetProductRowById(int productId)
        {
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT p.id, p.article, p.name, c.name AS category_name, p.price, p.description
                                 FROM products p
                                 LEFT JOIN categories c ON p.category_id = c.id
                                 WHERE p.id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", productId);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
            }
        }

        private void UpdatePaginationInfo()
        {
            if (lblPageInfo != null)
                lblPageInfo.Text = $"Страница {currentPage} из {totalPages}";
            if (txtPageNumber != null)
                txtPageNumber.Text = currentPage.ToString();
            if (lblRecordInfo != null)
            {
                int startRecord = (currentPage - 1) * pageSize + 1;
                int endRecord = Math.Min(currentPage * pageSize, totalRecords);
                lblRecordInfo.Text = totalRecords > 0 ? $"Записей: {startRecord}-{endRecord} из {totalRecords}" : "Записей: 0 из 0";
            }
            if (btnFirstPage != null) btnFirstPage.Enabled = currentPage > 1;
            if (btnPrevPage != null) btnPrevPage.Enabled = currentPage > 1;
            if (btnNextPage != null) btnNextPage.Enabled = currentPage < totalPages;
            if (btnLastPage != null) btnLastPage.Enabled = currentPage < totalPages;
        }

        // Обработчики кнопок пагинации
        private void BtnFirstPage_Click(object sender, EventArgs e) => LoadProductsPage(1);
        private void BtnPrevPage_Click(object sender, EventArgs e) { if (currentPage > 1) LoadProductsPage(currentPage - 1); }
        private void BtnNextPage_Click(object sender, EventArgs e) { if (currentPage < totalPages) LoadProductsPage(currentPage + 1); }
        private void BtnLastPage_Click(object sender, EventArgs e) => LoadProductsPage(totalPages);

        private void BtnGoToPage_Click(object sender, EventArgs e)
        {
            if (txtPageNumber != null && int.TryParse(txtPageNumber.Text, out int page))
            {
                if (page >= 1 && page <= totalPages)
                    LoadProductsPage(page);
                else
                {
                    MessageBox.Show($"Введите номер страницы от 1 до {totalPages}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPageNumber.Text = currentPage.ToString();
                }
            }
        }

        private void TxtPageNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnGoToPage_Click(sender, e);
                e.Handled = true;
            }
        }

        // ==================== Поиск и фильтрация ====================
        // Поиск по названию — совпадение с начала поля.
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            currentSearchText = searchTextBox.Text == "Поиск по названию..." ? "" : searchTextBox.Text;
            LoadProductsPage(1);
        }

        private void categoryFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryFilterComboBox.SelectedIndex > 0)
                currentCategory = (categoryFilterComboBox.SelectedItem as dynamic).Name;
            else
                currentCategory = "";
            LoadProductsPage(1);
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            DataTable dt = productsDataGridView.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.Sort = "name ASC";
                productsDataGridView.DataSource = dt.DefaultView.ToTable();
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "Поиск по названию...";
            searchTextBox.ForeColor = Color.Gray;
            categoryFilterComboBox.SelectedIndex = 0;
            currentSearchText = "";
            currentCategory = "";
            LoadProductsPage(1);
            ClearForm();
            ResetFormMode();
        }

        private void menuButton_Click(object sender, EventArgs e) => this.Close();

        // ==================== Валидация ====================
        private bool ValidateProductInput()
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) || nameTextBox.Text == "Название товара")
            {
                MessageBox.Show("Введите название товара", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (nameTextBox.Text.Length > 100)
            {
                MessageBox.Show("Название не может быть длиннее 100 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (categoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию товара", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(priceTextBox.Text) || priceTextBox.Text == "Цена (до 999)")
            {
                MessageBox.Show("Введите цену товара", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!TryParsePrice(priceTextBox.Text, out decimal price) || price <= 0 || price > 999)
            {
                MessageBox.Show("Цена должна быть числом от 1 до 999 (например 14 или 14,00)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Парсинг цены с поддержкой и точки и запятой как разделителя.
        private static bool TryParsePrice(string text, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(text)) return false;
            string normalized = text.Trim().Replace(',', '.');
            return decimal.TryParse(normalized,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        // Готовит цену для записи в БД. Возвращает decimal — БД сама приведет к нужному типу столбца.
        private static decimal ParsePriceForDb(string text)
        {
            return TryParsePrice(text, out decimal value) ? value : 0m;
        }

        // ==================== Работа с изображениями ====================
        private string SaveImageToTemp(Image image)
        {
            if (image == null || image == ImageHelper.Placeholder) return null;
            string tempPath = Path.Combine(Path.GetTempPath(), $"temp_{Guid.NewGuid():N}.jpg");
            image.Save(tempPath, ImageFormat.Jpeg);
            return tempPath;
        }

        private string SaveImageFromTemp(string tempPath, string article)
        {
            if (string.IsNullOrEmpty(tempPath) || !File.Exists(tempPath)) return null;
            try
            {
                string fileName = $"{article}.jpg"; // Сохраняем просто по артикулу, перезаписывая
                string newPath = Path.Combine(imagesFolderPath, fileName);
                File.Copy(tempPath, newPath, true);
                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Сжатие изображения до целевого размера в байтах. Возвращает путь к временному
        // jpg-файлу, размер которого <= maxBytes, либо null если уложиться не удалось.
        private string CompressImageToMaxSize(Image sourceImage, long maxBytes)
        {
            if (sourceImage == null) return null;
            string tempFile = Path.Combine(Path.GetTempPath(), $"compressed_{Guid.NewGuid():N}.jpg");
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);

            for (long quality = 95; quality >= 10; quality -= 5)
            {
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                {
                    sourceImage.Save(fs, jpegCodec, encoderParams);
                }
                FileInfo fi = new FileInfo(tempFile);
                if (fi.Length <= maxBytes)
                    return tempFile;
                File.Delete(tempFile);
            }
            return null;
        }

        // Сохраняет исходное изображение в jpg без сжатия (для случая, когда сжатие
        // до 2 МБ не удалось, но размер исходника <= 2.5 МБ — оставляем как есть).
        private string SaveImageAsIs(Image sourceImage)
        {
            if (sourceImage == null) return null;
            string tempFile = Path.Combine(Path.GetTempPath(), $"asis_{Guid.NewGuid():N}.jpg");
            sourceImage.Save(tempFile, ImageFormat.Jpeg);
            return tempFile;
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
                if (codec.MimeType == mimeType) return codec;
            return null;
        }

        private void DeleteImageFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
            try { File.Delete(path); } catch { }
        }

        private void LoadImageToPictureBox(string path)
        {
            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            try
            {
                productPictureBox.Image = !string.IsNullOrEmpty(path) && File.Exists(path)
                    ? Image.FromFile(path) : ImageHelper.Placeholder?.Clone() as Image;
            }
            catch
            {
                productPictureBox.Image = ImageHelper.Placeholder?.Clone() as Image;
            }
        }

        // ==================== Генерация артикула ====================
        private string GetNextArticle()
        {
            using (var conn = dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT COALESCE(MAX(CAST(article AS UNSIGNED)), 0) + 1 FROM products";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    long nextNum = Convert.ToInt64(cmd.ExecuteScalar());
                    return "0000" + nextNum.ToString();
                }
            }
        }

        // ==================== CRUD ====================
        private void addButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;
            if (isEditMode)
            {
                UpdateProduct(currentProductId);
                return;
            }
            if (!ValidateProductInput()) return;

            try
            {
                string newArticle = GetNextArticle();
                dynamic selectedCategory = categoryComboBox.SelectedItem;

                string savedImagePath = null;
                if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                    savedImagePath = SaveImageFromTemp(tempImagePath, newArticle);

                int newProductId;
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM products WHERE article = @article";
                    using (var cmd = new MySqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@article", newArticle);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Сгенерированный артикул уже существует. Попробуйте ещё раз.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (!string.IsNullOrEmpty(savedImagePath)) DeleteImageFile(savedImagePath);
                            return;
                        }
                    }

                    string insertQuery = @"INSERT INTO products (article, name, category_id, price, description, image)
                                           VALUES (@article, @name, @category_id, @price, @description, @image);
                                           SELECT LAST_INSERT_ID();";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@article", newArticle);
                        cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
                        cmd.Parameters.AddWithValue("@category_id", selectedCategory.Id);
                        cmd.Parameters.AddWithValue("@price", ParsePriceForDb(priceTextBox.Text));
                        cmd.Parameters.AddWithValue("@description", descriptionTextBox.Text);
                        cmd.Parameters.AddWithValue("@image", DBNull.Value); // изображение храним отдельно
                        newProductId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                MessageBox.Show("Товар успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                moveToTop = true;
                highlightProductId = newProductId;

                ClearForm();
                ResetFormMode();
                LoadProductsPage(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;
            if (productsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow row = productsDataGridView.SelectedRows[0];
            currentProductId = Convert.ToInt32(row.Cells["id"].Value);

            nameTextBox.Text = row.Cells["name"].Value.ToString();
            nameTextBox.ForeColor = Color.Black;
            nameTextBox.BackColor = Color.White;
            nameTextBox.ReadOnly = false;

            articleTextBox.Text = row.Cells["article"].Value.ToString();
            articleTextBox.ForeColor = Color.Black;
            articleTextBox.BackColor = Color.FromArgb(240, 240, 240);
            articleTextBox.ReadOnly = true;

            // В поле цена показываем "14,00" (две цифры после запятой, русская локаль)
            decimal _editPrice;
            if (row.Cells["price"].Value != null && decimal.TryParse(
                    row.Cells["price"].Value.ToString().Replace(',', '.'),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture, out _editPrice))
                priceTextBox.Text = _editPrice.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
            else
                priceTextBox.Text = row.Cells["price"].Value?.ToString() ?? "";
            priceTextBox.ForeColor = Color.Black;
            priceTextBox.BackColor = Color.White;
            priceTextBox.ReadOnly = false;

            descriptionTextBox.Text = row.Cells["description"].Value?.ToString() ?? "";
            descriptionTextBox.ForeColor = Color.Black;
            descriptionTextBox.BackColor = Color.White;
            descriptionTextBox.ReadOnly = false;

            string catName = row.Cells["category_name"].Value.ToString();
            foreach (var item in categoryComboBox.Items)
            {
                if ((item as dynamic).Name == catName)
                {
                    categoryComboBox.SelectedItem = item;
                    break;
                }
            }
            categoryComboBox.Enabled = true;

            // Загружаем изображение из GetProductImage
            string article = row.Cells["article"].Value.ToString();
            Image img = GetProductImage(article);
            if (productPictureBox.Image != null) productPictureBox.Image.Dispose();
            productPictureBox.Image = img;

            DeleteImageFile(tempImagePath);
            tempImagePath = null;

            isEditMode = true;
            addButton.Text = "Сохранить";
            addImageButton.Visible = true;
            removeImageButton.Visible = true;
        }

        private void UpdateProduct(int productId)
        {
            if (!ValidateProductInput()) return;
            try
            {
                dynamic selectedCategory = categoryComboBox.SelectedItem;
                bool imageChanged = !string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath);

                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();

                    string updateQuery = @"UPDATE products SET name = @name, category_id = @category_id,
                                           price = @price, description = @description
                                           WHERE id = @id";
                    using (var cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
                        cmd.Parameters.AddWithValue("@category_id", selectedCategory.Id);
                        cmd.Parameters.AddWithValue("@price", ParsePriceForDb(priceTextBox.Text));
                        cmd.Parameters.AddWithValue("@description", descriptionTextBox.Text);
                        cmd.Parameters.AddWithValue("@id", productId);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Если изображение было изменено, сохраняем его в папку
                if (imageChanged)
                {
                    string article = articleTextBox.Text;
                    SaveImageFromTemp(tempImagePath, article);
                }

                MessageBox.Show("Товар успешно обновлён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                moveToTop = true;
                highlightProductId = productId;

                ClearForm();
                ResetFormMode();
                LoadProductsPage(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;
            if (productsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int productId = Convert.ToInt32(productsDataGridView.SelectedRows[0].Cells["id"].Value);
            string productName = productsDataGridView.SelectedRows[0].Cells["name"].Value.ToString();

            try
            {
                using (var conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string checkOrderQuery = "SELECT COUNT(*) FROM order_items WHERE product_id = @productId";
                    using (var cmd = new MySqlCommand(checkOrderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Невозможно удалить товар, так как он присутствует в заказах.", "Удаление невозможно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (MessageBox.Show($"Удалить товар '{productName}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string deleteQuery = "DELETE FROM products WHERE id = @id";
                        using (var cmd = new MySqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", productId);
                            cmd.ExecuteNonQuery();
                        }
                        // Удаляем файл изображения, если есть
                        string article = productsDataGridView.SelectedRows[0].Cells["article"].Value.ToString();
                        DeleteImageFile(Path.Combine(imagesFolderPath, article + ".jpg"));
                        DeleteImageFile(Path.Combine(imagesFolderPath, article + ".png"));
                        MessageBox.Show("Товар удалён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProductsPage(1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== Кнопки изображений ====================
        // Сжимаем картинку до 2 МБ. Если уложиться не удалось, но исходник <= 2.5 МБ —
        // принимаем его как есть. Если исходник > 2.5 МБ — отказываем.
        private void addImageButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Выберите изображение товара";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        long originalSize = new FileInfo(ofd.FileName).Length;

                        Image img = Image.FromFile(ofd.FileName);

                        // Пробуем сжать до 2 МБ
                        string compressed = CompressImageToMaxSize(img, IMAGE_TARGET_BYTES);

                        if (compressed == null)
                        {
                            // Сжать до 2 МБ не получилось.
                            // Если исходник укладывается в 2.5 МБ — оставляем как есть.
                            if (originalSize <= IMAGE_HARD_LIMIT_BYTES)
                            {
                                compressed = SaveImageAsIs(img);
                            }
                            else
                            {
                                img.Dispose();
                                MessageBox.Show(
                                    "Не удалось сжать изображение до 2 МБ, а исходный файл больше 2,5 МБ. Выберите другое изображение.",
                                    "Ошибка размера", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        img.Dispose();

                        DeleteImageFile(tempImagePath);
                        tempImagePath = compressed;
                        LoadImageToPictureBox(tempImagePath);
                        MessageBox.Show("Изображение выбрано. Оно будет сохранено после нажатия «Добавить» или «Сохранить».", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void removeImageButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;
            DeleteImageFile(tempImagePath);
            tempImagePath = null;
            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            productPictureBox.Image = ImageHelper.Placeholder?.Clone() as Image;
        }

        // ==================== Вспомогательные методы ====================
        private void ClearForm()
        {
            nameTextBox.Text = "Название товара";
            nameTextBox.ForeColor = Color.Gray;
            nameTextBox.BackColor = Color.White;
            nameTextBox.ReadOnly = false;

            articleTextBox.Text = "Авто";
            articleTextBox.ForeColor = Color.Gray;
            articleTextBox.BackColor = Color.FromArgb(240, 240, 240);

            priceTextBox.Text = "Цена (до 999)";
            priceTextBox.ForeColor = Color.Gray;
            priceTextBox.BackColor = Color.White;

            descriptionTextBox.Text = "";
            categoryComboBox.SelectedIndex = -1;

            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            productPictureBox.Image = ImageHelper.Placeholder?.Clone() as Image;

            DeleteImageFile(tempImagePath);
            tempImagePath = null;
        }

        private void ResetFormMode()
        {
            isEditMode = false;
            currentProductId = 0;
            addButton.Text = "Добавить";
            DeleteImageFile(tempImagePath);
            tempImagePath = null;
            if (!readOnlyMode)
            {
                addImageButton.Visible = true;
                removeImageButton.Visible = true;
            }
        }

        private void productsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count > 0 && !isEditMode)
            {
                DataGridViewRow row = productsDataGridView.SelectedRows[0];
                string article = row.Cells["article"].Value?.ToString();
                if (!string.IsNullOrEmpty(article))
                {
                    Image img = GetProductImage(article);
                    if (productPictureBox.Image != null) productPictureBox.Image.Dispose();
                    productPictureBox.Image = img;
                }
                else
                {
                    if (productPictureBox.Image != null) productPictureBox.Image.Dispose();
                    productPictureBox.Image = ImageHelper.Placeholder?.Clone() as Image;
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DeleteImageFile(tempImagePath);
            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            base.OnFormClosing(e);
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }

        // ==================== Стилизация кнопок ====================
        private void ApplyCoralButtonStyle()
        {
            Color coral = Color.FromArgb(255, 127, 80);
            Color coralLight = Color.FromArgb(255, 147, 100);
            Color coralDark = Color.FromArgb(235, 107, 60);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                    ApplyButtonStyle(btn, coral, coralLight, coralDark);
                else if (ctrl is GroupBox group)
                    foreach (Control groupInner in group.Controls)
                        if (groupInner is Button groupBtn)
                            ApplyButtonStyle(groupBtn, coral, coralLight, coralDark);
                        else if (ctrl is Panel panel)
                            foreach (Control panelInner in panel.Controls)
                                if (panelInner is Button panelBtn)
                                    ApplyButtonStyle(panelBtn, coral, coralLight, coralDark);
            }

            if (menuButton != null)
            {
                menuButton.BackColor = Color.Red;
                menuButton.FlatStyle = FlatStyle.Flat;
                menuButton.FlatAppearance.BorderColor = Color.DarkRed;
                menuButton.FlatAppearance.BorderSize = 1;
                menuButton.ForeColor = Color.Black;
                menuButton.Font = new Font(menuButton.Font, FontStyle.Regular);
                menuButton.MouseEnter += (s, e) => menuButton.BackColor = Color.IndianRed;
                menuButton.MouseLeave += (s, e) => menuButton.BackColor = Color.Red;
                menuButton.MouseDown += (s, e) => menuButton.BackColor = Color.OrangeRed;
                menuButton.MouseUp += (s, e) => menuButton.BackColor = Color.OrangeRed;
            }
        }

        private void ApplyButtonStyle(Button button, Color normal, Color hover, Color pressed)
        {
            button.BackColor = normal;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(235, 107, 60);
            button.FlatAppearance.BorderSize = 1;
            button.ForeColor = Color.Black;
            button.Font = new Font(button.Font, FontStyle.Regular);
            button.MouseEnter += (s, e) => button.BackColor = hover;
            button.MouseLeave += (s, e) => button.BackColor = normal;
            button.MouseDown += (s, e) => button.BackColor = pressed;
            button.MouseUp += (s, e) => button.BackColor = hover;
        }

        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
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

        // Растяжение на весь экран — главная сетка тянется, панель пагинации якорится снизу,
        // правая часть (категория/сортировка/сброс) уезжает вправо при росте окна.
        private void SetupResponsiveLayout()
        {
            this.MinimumSize = new Size(1000, 700);
            this.WindowState = FormWindowState.Maximized;

            if (productsDataGridView != null)
                productsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            if (paginationPanel != null)
                paginationPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            if (categoryFilterComboBox != null) categoryFilterComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            if (categoryFilterLabel != null) categoryFilterLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            if (sortButton != null) sortButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            if (resetButton != null) resetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
    }
}