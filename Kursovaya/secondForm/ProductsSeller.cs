// ProductsForm.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        // Хранит текущий путь к изображению в БД (для режима редактирования)
        private string currentImagePathInDb = null;

        // Путь к папке с изображениями в AppData
        private readonly string imagesFolderPath;

        public ProductsForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Инициализация пути к папке с изображениями
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Smirnov_kursovaya"
            );
            imagesFolderPath = Path.Combine(appDataPath, "ProductImages");

            // Создаем папку, если её нет
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            InitializeControls();
            LoadCategories();
            LoadProductsPage(1); // Загружаем первую страницу

            // Подписываемся на событие изменения размера формы
            this.Resize += (s, e) => {
                if (readOnlyMode)
                {
                    OnResize(e);
                }
            };
        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            // Инициализация при загрузке формы
        }

        public void SetReadOnlyMode()
        {
            readOnlyMode = true;

            // Скрываем все кнопки управления
            addButton.Visible = false;
            editButton.Visible = false;
            deleteButton.Visible = false;
            addImageButton.Visible = false;
            removeImageButton.Visible = false;

            // Скрываем всю панель groupBox1 (панель с полями ввода)
            if (groupBox1 != null)
            {
                groupBox1.Visible = false;
            }

            // Скрываем PictureBox с изображением
            if (productPictureBox != null)
            {
                productPictureBox.Visible = false;
            }

            // Обновляем видимость колонок в DataGridView
            UpdateDataGridViewColumnsVisibility();

            // Перемещаем и растягиваем DataGridView
            if (productsDataGridView != null)
            {
                productsDataGridView.Top = categoryFilterComboBox.Bottom + 30;
                productsDataGridView.Left = 10;
                productsDataGridView.Width = this.ClientSize.Width - 20;
                productsDataGridView.Height = this.ClientSize.Height - productsDataGridView.Top - 10;
                productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            // Блокируем поля ввода
            nameTextBox.ReadOnly = true;
            articleTextBox.ReadOnly = true;
            priceTextBox.ReadOnly = true;
            descriptionTextBox.ReadOnly = true;
            categoryComboBox.Enabled = false;

            // Делаем поля серыми/неактивными
            nameTextBox.BackColor = Color.FromArgb(240, 240, 240);
            articleTextBox.BackColor = Color.FromArgb(240, 240, 240);
            priceTextBox.BackColor = Color.FromArgb(240, 240, 240);
            descriptionTextBox.BackColor = Color.FromArgb(240, 240, 240);
            categoryComboBox.BackColor = Color.FromArgb(240, 240, 240);

            // Убираем плейсхолдеры
            if (nameTextBox.Text == "Название товара")
                nameTextBox.Text = "";
            if (articleTextBox.Text == "Артикул (только цифры)")
                articleTextBox.Text = "";
            if (priceTextBox.Text == "Цена (например: 1000.50)")
                priceTextBox.Text = "";

            // Меняем заголовок формы
            this.Text = "Просмотр товаров (режим продавца)";
            label1.Text = "Просмотр товаров";
        }

        private void InitializeControls()
        {
            SetPlaceholderText(searchTextBox, "Поиск по названию...");
            SetPlaceholderText(nameTextBox, "Название товара");
            SetPlaceholderText(articleTextBox, "Артикул (только цифры)");
            SetPlaceholderText(priceTextBox, "Цена (например: 1000.50)");

            // Настройка DataGridView (стилизация и русские заголовки)
            SetupDataGridView();

            productPictureBox.BorderStyle = BorderStyle.FixedSingle;
            productPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

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

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80);
            Color coralLightColor = Color.FromArgb(255, 147, 100);
            Color coralDarkColor = Color.FromArgb(235, 107, 60);

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
                else if (control is Panel panel) // для панели пагинации
                {
                    foreach (Control subControl in panel.Controls)
                    {
                        if (subControl is Button subButton)
                        {
                            ApplyButtonStyle(subButton, coralColor, coralLightColor, coralDarkColor);
                        }
                    }
                }
            }

            // Особый стиль для кнопки меню
            if (menuButton != null)
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

        private void SetupDataGridView()
        {
            productsDataGridView.Columns.Clear();

            DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
            imageCol.Name = "image";
            imageCol.HeaderText = "Изображение";
            imageCol.DataPropertyName = "image";
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

            productsDataGridView.Columns["price"].DefaultCellStyle.Format = "C2";

            // Устанавливаем чередование цветов строк
            productsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255);

            // Цвет выделенной строки
            productsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250);
            productsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Цвет заголовков
            productsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80);
            productsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            productsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            productsDataGridView.ColumnHeadersHeight = 40;
            productsDataGridView.EnableHeadersVisualStyles = false;

            // Режим заполнения
            productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Настройка выделения
            productsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productsDataGridView.ReadOnly = true;
            productsDataGridView.RowHeadersVisible = false;
            productsDataGridView.AllowUserToAddRows = false;
            productsDataGridView.AllowUserToDeleteRows = false;
            productsDataGridView.MultiSelect = false;

            // Настройка сетки
            productsDataGridView.GridColor = Color.LightGray;
            productsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            productsDataGridView.RowTemplate.Height = 130;

            // Обработчик для преобразования байтов в изображение
            productsDataGridView.CellFormatting += (s, e) =>
            {
                if (productsDataGridView.Columns[e.ColumnIndex].Name == "image")
                {
                    if (e.Value is byte[] bytes && bytes.Length > 0)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                e.Value = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            e.Value = ImageHelper.Placeholder.Clone();
                        }
                    }
                    else
                    {
                        e.Value = ImageHelper.Placeholder.Clone();
                    }
                }
            };
        }
    

        private void UpdateDataGridViewColumnsVisibility()
        {
            // Можно оставить пустым или настроить видимость колонок в зависимости от режима
        }

        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
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

        private void LoadCategories()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT id, name FROM categories ORDER BY name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        categoryComboBox.Items.Clear();
                        categoryFilterComboBox.Items.Clear();
                        categoryFilterComboBox.Items.Add("Все категории");

                        while (reader.Read())
                        {
                            var item = new
                            {
                                Id = reader["id"],
                                Name = reader["name"].ToString()
                            };
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
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== МЕТОДЫ ПАГИНАЦИИ ==========

        private void LoadProductsPage(int page)
        {
            try
            {
                DataTable dt = dbHelper.GetProductsWithPagination(page, pageSize, currentSearchText, currentCategory, out totalRecords);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages == 0) totalPages = 1;
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;
                currentPage = page;

                // Если в таблице нет колонки image, добавлять не нужно, но метод GetProductsWithPagination должен её возвращать
                productsDataGridView.DataSource = dt;

                // Скрываем служебные колонки
                if (productsDataGridView.Columns.Contains("id"))
                    productsDataGridView.Columns["id"].Visible = false;
                if (productsDataGridView.Columns.Contains("category_id"))
                    productsDataGridView.Columns["category_id"].Visible = false;

                // Форматируем цену
                if (productsDataGridView.Columns.Contains("price"))
                    productsDataGridView.Columns["price"].DefaultCellStyle.Format = "C2";

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (totalRecords > 0)
                    lblRecordInfo.Text = $"Записей: {startRecord}-{endRecord} из {totalRecords}";
                else
                    lblRecordInfo.Text = "Записей: 0 из 0";
            }

            if (btnFirstPage != null) btnFirstPage.Enabled = currentPage > 1;
            if (btnPrevPage != null) btnPrevPage.Enabled = currentPage > 1;
            if (btnNextPage != null) btnNextPage.Enabled = currentPage < totalPages;
            if (btnLastPage != null) btnLastPage.Enabled = currentPage < totalPages;
        }

        private void BtnFirstPage_Click(object sender, EventArgs e) => LoadProductsPage(1);
        private void BtnPrevPage_Click(object sender, EventArgs e) { if (currentPage > 1) LoadProductsPage(currentPage - 1); }
        private void BtnNextPage_Click(object sender, EventArgs e) { if (currentPage < totalPages) LoadProductsPage(currentPage + 1); }
        private void BtnLastPage_Click(object sender, EventArgs e) => LoadProductsPage(totalPages);

        private void BtnGoToPage_Click(object sender, EventArgs e)
        {
            if (txtPageNumber != null && int.TryParse(txtPageNumber.Text, out int page))
            {
                if (page >= 1 && page <= totalPages)
                {
                    LoadProductsPage(page);
                }
                else
                {
                    MessageBox.Show($"Введите номер страницы от 1 до {totalPages}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPageNumber.Text = currentPage.ToString();
                }
            }
        }

        private void TxtPageNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnGoToPage_Click(sender, e);
                e.Handled = true;
            }
        }

        // ========== ОБРАБОТЧИКИ ПОИСКА И ФИЛЬТРАЦИИ ==========

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Поиск по названию...")
                currentSearchText = "";
            else
                currentSearchText = searchTextBox.Text;

            LoadProductsPage(1);
        }

        private void categoryFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryFilterComboBox.SelectedIndex > 0)
            {
                dynamic selectedCategory = categoryFilterComboBox.SelectedItem;
                currentCategory = selectedCategory.Name;
            }
            else
            {
                currentCategory = "";
            }

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

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ========== МЕТОДЫ ДЛЯ РАБОТЫ С ТОВАРАМИ (ДОБАВЛЕНИЕ/РЕДАКТИРОВАНИЕ) ==========

        private bool ValidateProductInput()
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) || nameTextBox.Text == "Название товара")
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(articleTextBox.Text) || articleTextBox.Text == "Артикул (только цифры)")
            {
                MessageBox.Show("Введите артикул товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (categoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(priceTextBox.Text) || priceTextBox.Text == "Цена (например: 1000.50)")
            {
                MessageBox.Show("Введите цену товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!long.TryParse(articleTextBox.Text, out long article))
            {
                MessageBox.Show("Артикул должен содержать только цифры", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(priceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private string SaveImageFromTemp(string tempPath, string article)
        {
            if (string.IsNullOrEmpty(tempPath) || !File.Exists(tempPath))
                return null;

            try
            {
                string fileName = $"product_{article}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string newPath = Path.Combine(imagesFolderPath, fileName);
                File.Copy(tempPath, newPath, true);
                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private string SaveImageToTemp(Image image)
        {
            if (image == null || image == ImageHelper.Placeholder)
                return null;

            string tempPath = null;
            try
            {
                string tempFileName = $"temp_{Guid.NewGuid():N}.png";
                tempPath = Path.Combine(Path.GetTempPath(), tempFileName);

                using (Bitmap bitmap = new Bitmap(image))
                {
                    bitmap.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                }

                return tempPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения временного изображения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (!string.IsNullOrEmpty(tempPath) && File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); } catch { }
                }
                return null;
            }
        }

        private void DeleteImageFile(string imagePath, int maxAttempts = 3)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    File.Delete(imagePath);
                    break;
                }
                catch (IOException)
                {
                    if (attempt == maxAttempts)
                    {
                        System.Diagnostics.Debug.WriteLine($"Не удалось удалить файл {imagePath} после {maxAttempts} попыток");
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка удаления файла: {ex.Message}");
                    break;
                }
            }
        }

        private void LoadImageToPictureBox(string imagePath)
        {
            try
            {
                if (productPictureBox.Image != null)
                {
                    productPictureBox.Image.Dispose();
                    productPictureBox.Image = null;
                }

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        productPictureBox.Image = Image.FromStream(fs);
                    }
                }
                else
                {
                    productPictureBox.Image = ImageHelper.Placeholder != null ?
                        (Image)ImageHelper.Placeholder.Clone() : null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                productPictureBox.Image = ImageHelper.Placeholder != null ?
                    (Image)ImageHelper.Placeholder.Clone() : null;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;

            if (isEditMode)
            {
                UpdateProduct(currentProductId);
                return;
            }

            if (!ValidateProductInput())
                return;

            try
            {
                dynamic selectedCategory = categoryComboBox.SelectedItem;

                string savedImagePath = null;
                if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                {
                    savedImagePath = SaveImageFromTemp(tempImagePath, articleTextBox.Text);
                }

                byte[] imageData = null;
                if (!string.IsNullOrEmpty(savedImagePath) && File.Exists(savedImagePath))
                {
                    imageData = File.ReadAllBytes(savedImagePath);
                }

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM products WHERE article = @article";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@article", articleTextBox.Text);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Товар с таким артикулом уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                            if (!string.IsNullOrEmpty(savedImagePath))
                            {
                                DeleteImageFile(savedImagePath);
                            }
                            return;
                        }
                    }

                    string query = @"INSERT INTO products (article, name, category_id, price, 
                                   description, image) 
                                   VALUES (@article, @name, @category_id, @price, 
                                   @description, @image)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@article", articleTextBox.Text);
                        command.Parameters.AddWithValue("@name", nameTextBox.Text);
                        command.Parameters.AddWithValue("@category_id", selectedCategory.Id);
                        command.Parameters.AddWithValue("@price", decimal.Parse(priceTextBox.Text));
                        command.Parameters.AddWithValue("@description", descriptionTextBox.Text);
                        command.Parameters.AddWithValue("@image", imageData ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Товар успешно добавлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearForm();
                        LoadProductsPage(1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;

            if (productsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow selectedRow = productsDataGridView.SelectedRows[0];
            currentProductId = Convert.ToInt32(selectedRow.Cells["id"].Value);

            articleTextBox.Text = selectedRow.Cells["article"].Value.ToString();
            nameTextBox.Text = selectedRow.Cells["name"].Value.ToString();
            priceTextBox.Text = selectedRow.Cells["price"].Value.ToString();
            descriptionTextBox.Text = selectedRow.Cells["description"].Value?.ToString() ?? "";

            string categoryName = selectedRow.Cells["category_name"].Value.ToString();
            foreach (var item in categoryComboBox.Items)
            {
                dynamic categoryItem = item;
                if (categoryItem.Name == categoryName)
                {
                    categoryComboBox.SelectedItem = item;
                    break;
                }
            }

            // Загрузка изображения
            if (selectedRow.Cells["image"].Value != null && selectedRow.Cells["image"].Value != DBNull.Value)
            {
                try
                {
                    byte[] imageData = (byte[])selectedRow.Cells["image"].Value;
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        productPictureBox.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    productPictureBox.Image = ImageHelper.Placeholder != null ?
                        (Image)ImageHelper.Placeholder.Clone() : null;
                }
            }
            else
            {
                productPictureBox.Image = ImageHelper.Placeholder != null ?
                    (Image)ImageHelper.Placeholder.Clone() : null;
            }

            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                DeleteImageFile(tempImagePath);
                tempImagePath = null;
            }

            isEditMode = true;
            addButton.Text = "Сохранить";
        }

        private void UpdateProduct(int productId)
        {
            if (!ValidateProductInput())
                return;

            try
            {
                dynamic selectedCategory = categoryComboBox.SelectedItem;
                bool imageChanged = !string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath);
                string savedImagePath = null;

                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM products WHERE article = @article AND id != @id";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@article", articleTextBox.Text);
                        checkCommand.Parameters.AddWithValue("@id", productId);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Товар с таким артикулом уже существует", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (imageChanged)
                    {
                        savedImagePath = SaveImageFromTemp(tempImagePath, articleTextBox.Text);
                    }

                    byte[] imageData = null;
                    if (!string.IsNullOrEmpty(savedImagePath) && File.Exists(savedImagePath))
                    {
                        imageData = File.ReadAllBytes(savedImagePath);
                    }
                    else if (!imageChanged && productPictureBox.Image != null &&
                             productPictureBox.Image != ImageHelper.Placeholder)
                    {
                        imageData = ImageHelper.ImageToByteArray(productPictureBox.Image);
                    }

                    string query = @"UPDATE products SET article = @article, name = @name, 
                                   category_id = @category_id, price = @price, 
                                   description = @description, image = @image 
                                   WHERE id = @id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@article", articleTextBox.Text);
                        command.Parameters.AddWithValue("@name", nameTextBox.Text);
                        command.Parameters.AddWithValue("@category_id", selectedCategory.Id);
                        command.Parameters.AddWithValue("@price", decimal.Parse(priceTextBox.Text));
                        command.Parameters.AddWithValue("@description", descriptionTextBox.Text);
                        command.Parameters.AddWithValue("@image", imageData ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id", productId);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Товар успешно обновлен", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearForm();
                        LoadProductsPage(1);
                        ResetFormMode();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;

            if (productsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int productId = Convert.ToInt32(productsDataGridView.SelectedRows[0].Cells["id"].Value);
            string productName = productsDataGridView.SelectedRows[0].Cells["name"].Value.ToString();

            if (MessageBox.Show($"Вы уверены, что хотите удалить товар '{productName}'?\n" +
                               "Все заказы, содержащие этот товар, также будут удалены!", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = dbHelper.GetConnection())
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string getOrdersQuery = @"SELECT DISTINCT oi.order_id 
                                                FROM order_items oi 
                                                WHERE oi.product_id = @product_id";

                                List<int> orderIds = new List<int>();
                                using (var getOrdersCommand = new MySqlCommand(getOrdersQuery, connection, transaction))
                                {
                                    getOrdersCommand.Parameters.AddWithValue("@product_id", productId);
                                    using (var reader = getOrdersCommand.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            orderIds.Add(reader.GetInt32(0));
                                        }
                                    }
                                }

                                string deleteOrderItemsQuery = "DELETE FROM order_items WHERE product_id = @product_id";
                                using (var deleteOrderItemsCommand = new MySqlCommand(deleteOrderItemsQuery, connection, transaction))
                                {
                                    deleteOrderItemsCommand.Parameters.AddWithValue("@product_id", productId);
                                    deleteOrderItemsCommand.ExecuteNonQuery();
                                }

                                string deleteProductQuery = "DELETE FROM products WHERE id = @id";
                                using (var deleteProductCommand = new MySqlCommand(deleteProductQuery, connection, transaction))
                                {
                                    deleteProductCommand.Parameters.AddWithValue("@id", productId);
                                    deleteProductCommand.ExecuteNonQuery();
                                }

                                foreach (int orderId in orderIds)
                                {
                                    string checkOrderQuery = "SELECT COUNT(*) FROM order_items WHERE order_id = @order_id";
                                    using (var checkCommand = new MySqlCommand(checkOrderQuery, connection, transaction))
                                    {
                                        checkCommand.Parameters.AddWithValue("@order_id", orderId);
                                        int remainingItems = Convert.ToInt32(checkCommand.ExecuteScalar());

                                        if (remainingItems == 0)
                                        {
                                            string deleteOrderQuery = "DELETE FROM orders WHERE id = @order_id";
                                            using (var deleteOrderCommand = new MySqlCommand(deleteOrderQuery, connection, transaction))
                                            {
                                                deleteOrderCommand.Parameters.AddWithValue("@order_id", orderId);
                                                deleteOrderCommand.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }

                                transaction.Commit();

                                if (orderIds.Count > 0)
                                {
                                    MessageBox.Show($"Товар удален.\n" +
                                                  $"Было удалено {orderIds.Count} заказов, содержащих этот товар.", "Успех",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Товар удален", "Успех",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                LoadProductsPage(1);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Ошибка удаления товара: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void addImageButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Выберите изображение товара";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image selectedImage;
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        selectedImage = Image.FromStream(fs);
                    }

                    int maxWidth = 150;
                    int maxHeight = 150;

                    double ratioX = (double)maxWidth / selectedImage.Width;
                    double ratioY = (double)maxHeight / selectedImage.Height;
                    double ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(selectedImage.Width * ratio);
                    int newHeight = (int)(selectedImage.Height * ratio);

                    Bitmap resizedImage = new Bitmap(newWidth, newHeight);
                    using (Graphics g = Graphics.FromImage(resizedImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(selectedImage, 0, 0, newWidth, newHeight);
                    }

                    selectedImage.Dispose();

                    if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                    {
                        DeleteImageFile(tempImagePath);
                    }

                    tempImagePath = SaveImageToTemp(resizedImage);
                    resizedImage.Dispose();

                    if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                    {
                        LoadImageToPictureBox(tempImagePath);
                    }

                    MessageBox.Show("Изображение выбрано. Оно будет сохранено после нажатия кнопки 'Добавить' или 'Сохранить'",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void removeImageButton_Click(object sender, EventArgs e)
        {
            if (readOnlyMode) return;

            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                DeleteImageFile(tempImagePath);
                tempImagePath = null;
            }

            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            productPictureBox.Image = ImageHelper.Placeholder != null ?
                (Image)ImageHelper.Placeholder.Clone() : null;
        }

        private void ClearForm()
        {
            nameTextBox.Text = "Название товара";
            nameTextBox.ForeColor = Color.Gray;
            articleTextBox.Text = "Артикул (только цифры)";
            articleTextBox.ForeColor = Color.Gray;
            priceTextBox.Text = "Цена (например: 1000.50)";
            priceTextBox.ForeColor = Color.Gray;
            descriptionTextBox.Clear();
            categoryComboBox.SelectedIndex = -1;

            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            productPictureBox.Image = ImageHelper.Placeholder != null ?
                (Image)ImageHelper.Placeholder.Clone() : null;

            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                DeleteImageFile(tempImagePath);
                tempImagePath = null;
            }

            currentImagePathInDb = null;
        }

        private void ResetFormMode()
        {
            isEditMode = false;
            currentProductId = 0;
            addButton.Text = "Добавить";

            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                DeleteImageFile(tempImagePath);
                tempImagePath = null;
            }
            currentImagePathInDb = null;
        }

        private void productsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count > 0 && !isEditMode)
            {
                DataGridViewRow selectedRow = productsDataGridView.SelectedRows[0];

                if (selectedRow.Cells["image"].Value != null && selectedRow.Cells["image"].Value != DBNull.Value)
                {
                    try
                    {
                        byte[] imageData = (byte[])selectedRow.Cells["image"].Value;
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            if (productPictureBox.Image != null)
                            {
                                productPictureBox.Image.Dispose();
                            }
                            productPictureBox.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        if (productPictureBox.Image != null)
                        {
                            productPictureBox.Image.Dispose();
                        }
                        productPictureBox.Image = ImageHelper.Placeholder != null ?
                            (Image)ImageHelper.Placeholder.Clone() : null;
                    }
                }
                else
                {
                    if (productPictureBox.Image != null)
                    {
                        productPictureBox.Image.Dispose();
                    }
                    productPictureBox.Image = ImageHelper.Placeholder != null ?
                        (Image)ImageHelper.Placeholder.Clone() : null;
                }
            }
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // разрешаем все символы
        }

        private void articleTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void priceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',')
            {
                e.KeyChar = '.';
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                try
                {
                    DeleteImageFile(tempImagePath);
                }
                catch { }
            }

            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }

            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (readOnlyMode && productsDataGridView != null && groupBox1 != null && !groupBox1.Visible)
            {
                productsDataGridView.Top = categoryFilterComboBox.Bottom + 30;
                productsDataGridView.Left = 10;
                productsDataGridView.Width = this.ClientSize.Width - 20;
                productsDataGridView.Height = this.ClientSize.Height - productsDataGridView.Top - 10;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }
    }
}