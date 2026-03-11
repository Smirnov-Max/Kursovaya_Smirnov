// ProductsForm.cs
using System;
using System.Collections.Generic;
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
        private DataGridViewImageColumn imageColumn;

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
            LoadProducts();
            LoadCategories();

            // Подписываемся на событие изменения размера формы
            this.Resize += (s, e) => {
                if (readOnlyMode)
                {
                    OnResize(e);
                }
            };
        }

        // Обработчик загрузки формы для исправления ошибки
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
                // Устанавливаем позицию DataGridView сразу под панелью фильтрации
                productsDataGridView.Top = categoryFilterComboBox.Bottom + 30; // Отступ после фильтра

                // Растягиваем на всю доступную ширину
                productsDataGridView.Left = 10;
                productsDataGridView.Width = this.ClientSize.Width - 20; // Отступы по краям

                // Растягиваем на всю доступную высоту
                productsDataGridView.Height = this.ClientSize.Height - productsDataGridView.Top - 10;

                // Устанавливаем режим заполнения для колонок
                productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            // Блокируем поля ввода (на всякий случай, хотя они скрыты)
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

            // Убираем плейсхолдеры и устанавливаем обычный текст
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

            // Настройка DataGridView с изображениями
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

        private void SetupDataGridView()
        {
            // Очищаем все колонки
            productsDataGridView.Columns.Clear();

            // Добавляем колонку с изображением (увеличиваем ширину для лучшего просмотра)
            imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "image";
            imageColumn.HeaderText = "Изображение";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 120; // Увеличиваем ширину колонки изображения
            imageColumn.Visible = !readOnlyMode; // Скрываем колонку с изображением в режиме продавца
            productsDataGridView.Columns.Add(imageColumn);

            // Добавляем остальные колонки
            productsDataGridView.Columns.Add("id", "ID");
            productsDataGridView.Columns.Add("article", "Артикул");
            productsDataGridView.Columns.Add("name", "Название");
            productsDataGridView.Columns.Add("category_name", "Категория");
            productsDataGridView.Columns.Add("price", "Цена");
            productsDataGridView.Columns.Add("description", "Описание");

            // Настройка колонок
            productsDataGridView.Columns["id"].Visible = false;
            productsDataGridView.Columns["price"].DefaultCellStyle.Format = "C2";

            // Устанавливаем чередование цветов строк
            productsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255); // Очень светлый фиолетовый

            // Цвет выделенной строки - очень светлый фиолетовый
            productsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250); // Светлый фиолетовый для выделения
            productsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Черный текст для контраста

            // Цвет заголовков
            productsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80); // Coral цвет
            productsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            productsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            productsDataGridView.ColumnHeadersHeight = 40;
            productsDataGridView.EnableHeadersVisualStyles = false; // Отключаем стандартные стили Windows

            // Устанавливаем режим заполнения для всех колонок
            productsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Настройка пропорций колонок
            UpdateDataGridViewColumnsVisibility();

            productsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productsDataGridView.ReadOnly = true;
            productsDataGridView.RowHeadersVisible = false;
            productsDataGridView.AllowUserToAddRows = false;
            productsDataGridView.AllowUserToDeleteRows = false;
            productsDataGridView.MultiSelect = false;

            // Настройка стиля сетки
            productsDataGridView.GridColor = Color.LightGray;
            productsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            productsDataGridView.RowTemplate.Height = 130; // Увеличиваем высоту строк для изображений
        }

        private void UpdateDataGridViewColumnsVisibility()
        {
            if (!productsDataGridView.Columns.Contains("image")) return;

            if (readOnlyMode)
            {
                // В режиме продавца скрываем колонку с изображением
                productsDataGridView.Columns["image"].Visible = true;
                productsDataGridView.Columns["image"].Width = 100; // Фиксированная ширина для изображений

                // Перераспределяем пропорции колонок
                productsDataGridView.Columns["article"].FillWeight = 12;
                productsDataGridView.Columns["name"].FillWeight = 35;
                productsDataGridView.Columns["category_name"].FillWeight = 23;
                productsDataGridView.Columns["price"].FillWeight = 12;
                productsDataGridView.Columns["description"].FillWeight = 18;
            }
            else
            {
                // В режиме администратора показываем колонку с изображением
                productsDataGridView.Columns["image"].Visible = true;
                productsDataGridView.Columns["image"].Width = 30; // Фиксированная ширина для изображений

                // Возвращаем стандартные пропорции с учетом колонки изображения
                productsDataGridView.Columns["article"].FillWeight = 10;
                productsDataGridView.Columns["name"].FillWeight = 30; // Увеличиваем вес названия
                productsDataGridView.Columns["category_name"].FillWeight = 15;
                productsDataGridView.Columns["price"].FillWeight = 10;
                productsDataGridView.Columns["description"].FillWeight = 20;
            }
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

        private void LoadProducts()
        {
            try
            {
                using (var connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT p.id, p.article, p.name, c.name as category_name, 
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

                            // Загрузка изображения из BLOB
                            byte[] imageData = reader["image"] != DBNull.Value ?
                                (byte[])reader["image"] : null;

                            Image productImage = ImageHelper.ByteArrayToImage(imageData);

                            row.Cells["image"].Value = productImage ?? ImageHelper.Placeholder;

                            // Остальные данные
                            row.Cells["id"].Value = reader["id"];
                            row.Cells["article"].Value = reader["article"];
                            row.Cells["name"].Value = reader["name"];
                            row.Cells["category_name"].Value = reader["category_name"];
                            row.Cells["price"].Value = reader["price"];
                            row.Cells["description"].Value = reader["description"]?.ToString() ?? "";
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

            // Проверка артикула
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

        /// <summary>
        /// Сохраняет изображение из временного файла в постоянное место с именем на основе артикула
        /// </summary>
        private string SaveImageFromTemp(string tempPath, string article)
        {
            if (string.IsNullOrEmpty(tempPath) || !File.Exists(tempPath))
                return null;

            try
            {
                // Генерируем новое имя файла на основе артикула
                string fileName = $"product_{article}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string newPath = Path.Combine(imagesFolderPath, fileName);

                // Копируем временный файл (не перемещаем, так как он может быть ещё нужен)
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

        /// <summary>
        /// Сохраняет изображение из PictureBox во временный файл при выборе
        /// </summary>
        private string SaveImageToTemp(Image image)
        {
            if (image == null || image == ImageHelper.Placeholder)
                return null;

            string tempPath = null;
            try
            {
                // Создаем временный файл
                string tempFileName = $"temp_{Guid.NewGuid():N}.png";
                tempPath = Path.Combine(Path.GetTempPath(), tempFileName);

                // Сохраняем изображение (создаем копию, чтобы не блокировать оригинал)
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

                // Если произошла ошибка, пытаемся удалить временный файл
                if (!string.IsNullOrEmpty(tempPath) && File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); } catch { }
                }
                return null;
            }
        }

        /// <summary>
        /// Удаляет файл изображения с несколькими попытками
        /// </summary>
        private void DeleteImageFile(string imagePath, int maxAttempts = 3)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    File.Delete(imagePath);
                    break; // Успешно удалили, выходим из цикла
                }
                catch (IOException)
                {
                    if (attempt == maxAttempts)
                    {
                        // Последняя попытка не удалась, логируем
                        System.Diagnostics.Debug.WriteLine($"Не удалось удалить файл {imagePath} после {maxAttempts} попыток");
                    }
                    else
                    {
                        // Ждем немного перед следующей попыткой
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

        /// <summary>
        /// Загружает изображение из файла в PictureBox
        /// </summary>
        private void LoadImageToPictureBox(string imagePath)
        {
            try
            {
                // Освобождаем текущее изображение
                if (productPictureBox.Image != null)
                {
                    productPictureBox.Image.Dispose();
                    productPictureBox.Image = null;
                }

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    // Загружаем изображение с использованием FileStream для снятия блокировки
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

                // Сохраняем изображение в постоянное место только при нажатии кнопки
                string savedImagePath = null;
                if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                {
                    savedImagePath = SaveImageFromTemp(tempImagePath, articleTextBox.Text);
                }

                // Получаем изображение для сохранения в БД
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

                            // Если есть сохраненное изображение, удаляем его
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
                        LoadProducts();
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
            Image image = (Image)selectedRow.Cells["image"].Value;
            if (image != null && image != ImageHelper.Placeholder)
            {
                // Создаем копию изображения, чтобы не зависеть от DataGridView
                productPictureBox.Image = image.Clone() as Image;
                currentImagePathInDb = null; // Изображение из БД, пути к файлу нет
            }
            else
            {
                productPictureBox.Image = ImageHelper.Placeholder != null ?
                    (Image)ImageHelper.Placeholder.Clone() : null;
                currentImagePathInDb = null;
            }

            // Очищаем временный путь
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

                    // Если изображение изменилось, сохраняем его в постоянное место
                    if (imageChanged)
                    {
                        savedImagePath = SaveImageFromTemp(tempImagePath, articleTextBox.Text);
                    }

                    // Получаем изображение для сохранения в БД
                    byte[] imageData = null;
                    if (!string.IsNullOrEmpty(savedImagePath) && File.Exists(savedImagePath))
                    {
                        imageData = File.ReadAllBytes(savedImagePath);
                    }
                    else if (!imageChanged && productPictureBox.Image != null &&
                             productPictureBox.Image != ImageHelper.Placeholder)
                    {
                        // Изображение не изменилось - берем его из PictureBox
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
                        LoadProducts();
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
                                // 1. Получаем список заказов, содержащих этот товар
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

                                // 2. Удаляем товар из корзин заказов (order_items)
                                string deleteOrderItemsQuery = "DELETE FROM order_items WHERE product_id = @product_id";
                                using (var deleteOrderItemsCommand = new MySqlCommand(deleteOrderItemsQuery, connection, transaction))
                                {
                                    deleteOrderItemsCommand.Parameters.AddWithValue("@product_id", productId);
                                    deleteOrderItemsCommand.ExecuteNonQuery();
                                }

                                // 3. Удаляем товар из таблицы products
                                string deleteProductQuery = "DELETE FROM products WHERE id = @id";
                                using (var deleteProductCommand = new MySqlCommand(deleteProductQuery, connection, transaction))
                                {
                                    deleteProductCommand.Parameters.AddWithValue("@id", productId);
                                    deleteProductCommand.ExecuteNonQuery();
                                }

                                // 4. Проверяем и удаляем "пустые" заказы (где нет товаров)
                                foreach (int orderId in orderIds)
                                {
                                    // Проверяем, есть ли еще товары в заказе
                                    string checkOrderQuery = "SELECT COUNT(*) FROM order_items WHERE order_id = @order_id";
                                    using (var checkCommand = new MySqlCommand(checkOrderQuery, connection, transaction))
                                    {
                                        checkCommand.Parameters.AddWithValue("@order_id", orderId);
                                        int remainingItems = Convert.ToInt32(checkCommand.ExecuteScalar());

                                        // Если в заказе не осталось товаров, удаляем заказ
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

                                // Сообщение о количестве удаленных заказов
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

                                LoadProducts();
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
                    // Загружаем изображение с использованием FileStream для снятия блокировки
                    Image selectedImage;
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        selectedImage = Image.FromStream(fs);
                    }

                    // Масштабируем изображение до оптимального размера
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

                    // Сохраняем изображение во временный файл
                    if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                    {
                        DeleteImageFile(tempImagePath);
                    }

                    tempImagePath = SaveImageToTemp(resizedImage);
                    resizedImage.Dispose();

                    // Загружаем изображение в PictureBox из временного файла
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

            // Удаляем временный файл, если он есть
            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                DeleteImageFile(tempImagePath);
                tempImagePath = null;
            }

            // Очищаем PictureBox
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

            // Очищаем изображение
            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }
            productPictureBox.Image = ImageHelper.Placeholder != null ?
                (Image)ImageHelper.Placeholder.Clone() : null;

            // Удаляем временный файл, если он есть
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

            // Очищаем временный файл
            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                DeleteImageFile(tempImagePath);
                tempImagePath = null;
            }
            currentImagePathInDb = null;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (searchText == "Поиск по названию...")
                return;

            foreach (DataGridViewRow row in productsDataGridView.Rows)
            {
                string name = row.Cells["name"].Value?.ToString() ?? "";
                string article = row.Cells["article"].Value?.ToString() ?? "";

                bool visible = name.ToLower().Contains(searchText.ToLower()) ||
                              article.ToLower().Contains(searchText.ToLower());

                row.Visible = visible;
            }
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            // Сортировка по названию
            productsDataGridView.Sort(productsDataGridView.Columns["name"],
                System.ComponentModel.ListSortDirection.Ascending);
        }

        private void categoryFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryFilterComboBox.SelectedIndex > 0)
            {
                dynamic selectedCategory = categoryFilterComboBox.SelectedItem;
                string categoryName = selectedCategory.Name;

                foreach (DataGridViewRow row in productsDataGridView.Rows)
                {
                    string rowCategory = row.Cells["category_name"].Value?.ToString() ?? "";
                    row.Visible = rowCategory == categoryName;
                }
            }
            else
            {
                // Показываем все
                foreach (DataGridViewRow row in productsDataGridView.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "Поиск по названию...";
            searchTextBox.ForeColor = Color.Gray;
            categoryFilterComboBox.SelectedIndex = 0;

            foreach (DataGridViewRow row in productsDataGridView.Rows)
            {
                row.Visible = true;
            }

            productsDataGridView.ClearSelection();
            ClearForm();
            ResetFormMode();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void productsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count > 0 && !isEditMode)
            {
                DataGridViewRow selectedRow = productsDataGridView.SelectedRows[0];

                // Показываем изображение в PictureBox
                Image image = (Image)selectedRow.Cells["image"].Value;
                if (image != null && image != ImageHelper.Placeholder)
                {
                    if (productPictureBox.Image != null)
                    {
                        productPictureBox.Image.Dispose();
                    }
                    productPictureBox.Image = image.Clone() as Image;
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

        // Валидация ввода
        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем все символы, кроме специальных управляющих
            if (char.IsControl(e.KeyChar))
                return;
        }

        private void articleTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void priceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры, точку/запятую и Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Заменяем запятую на точку
            if (e.KeyChar == ',')
            {
                e.KeyChar = '.';
            }

            // Проверяем, что точка только одна
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        // Обработчик закрытия формы для очистки временных файлов
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Удаляем временный файл, если он есть
            if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
            {
                try
                {
                    DeleteImageFile(tempImagePath);
                }
                catch { }
            }

            // Освобождаем изображение в PictureBox
            if (productPictureBox.Image != null)
            {
                productPictureBox.Image.Dispose();
                productPictureBox.Image = null;
            }

            base.OnFormClosing(e);
        }

        // Обработчик изменения размера формы
        protected override void OnResize(EventArgs e)
        {

            // Если мы в режиме продавца, обновляем размеры DataGridView
            if (readOnlyMode && productsDataGridView != null && groupBox1 != null && !groupBox1.Visible)
            {
                // Обновляем позицию и размеры DataGridView
                productsDataGridView.Top = categoryFilterComboBox.Bottom + 30;
                productsDataGridView.Left = 10;
                productsDataGridView.Width = this.ClientSize.Width - 20;
                productsDataGridView.Height = this.ClientSize.Height - productsDataGridView.Top - 10;
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}