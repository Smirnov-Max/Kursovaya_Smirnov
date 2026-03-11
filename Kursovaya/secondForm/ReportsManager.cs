using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;

namespace Smirnov_kursovaya.secondForm
{
    public partial class ReportsForm : Form
    {
        private DatabaseHelper dbHelper;

        public ReportsForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Настройка DataGridView
            reportsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reportsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            reportsDataGridView.ReadOnly = true;
            reportsDataGridView.RowHeadersVisible = false;

            // Настройка стиля сетки
            reportsDataGridView.GridColor = Color.LightGray;
            reportsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Устанавливаем чередование цветов строк
            reportsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 240, 255); // Очень светлый фиолетовый

            // Цвет выделенной строки - очень светлый фиолетовый
            reportsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 210, 250); // Светлый фиолетовый для выделения
            reportsDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Черный текст для контраста

            // Цвет заголовков
            reportsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 127, 80); // Coral цвет
            reportsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            reportsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            reportsDataGridView.ColumnHeadersHeight = 40;
            reportsDataGridView.EnableHeadersVisualStyles = false; // Отключаем стандартные стили Windows

            // Настройка дат
            reportFromDatePicker.Value = DateTime.Today.AddDays(-30);
            reportToDatePicker.Value = DateTime.Today;
            reportFromDatePicker.MaxDate = DateTime.Today.AddDays(-1); // Не позже вчера
            reportToDatePicker.MaxDate = DateTime.Today; // Не позже сегодня

            // Настройка типов отчетов
            reportTypeComboBox.Items.AddRange(new object[] {
                "Отчет по заказам",
                "Отчет по продажам",
                "Отчет по клиентам",
                "Отчет по товарам"
            });
            reportTypeComboBox.SelectedIndex = 0;

            // Устанавливаем стиль
            ApplyCoralButtonStyle();
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

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            // Дополнительная инициализация
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
            if (reportTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип отчета", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string reportType = reportTypeComboBox.SelectedItem.ToString();
                DataTable reportData = null;

                switch (reportType)
                {
                    case "Отчет по заказам":
                        reportData = GenerateOrdersReport();
                        break;
                    case "Отчет по продажам":
                        reportData = GenerateSalesReport();
                        break;
                    case "Отчет по клиентам":
                        reportData = GenerateClientsReport();
                        break;
                    case "Отчет по товарам":
                        reportData = GenerateProductsReport();
                        break;
                }

                if (reportData != null && reportData.Rows.Count > 0)
                {
                    reportsDataGridView.DataSource = reportData;
                    ApplyColumnHeaders(reportType);
                    CalculateTotals(reportData, reportType);
                }
                else
                {
                    MessageBox.Show("Нет данных для выбранного отчета за указанный период", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyColumnHeaders(string reportType)
        {
            switch (reportType)
            {
                case "Отчет по заказам":
                    if (reportsDataGridView.Columns.Contains("order_number"))
                        reportsDataGridView.Columns["order_number"].HeaderText = "Номер заказа";
                    if (reportsDataGridView.Columns.Contains("client_name"))
                        reportsDataGridView.Columns["client_name"].HeaderText = "Клиент";
                    if (reportsDataGridView.Columns.Contains("product_name"))
                        reportsDataGridView.Columns["product_name"].HeaderText = "Товар";
                    if (reportsDataGridView.Columns.Contains("status_name"))
                        reportsDataGridView.Columns["status_name"].HeaderText = "Статус";
                    if (reportsDataGridView.Columns.Contains("date_of_creation"))
                        reportsDataGridView.Columns["date_of_creation"].HeaderText = "Дата создания";
                    if (reportsDataGridView.Columns.Contains("date_of_completion"))
                        reportsDataGridView.Columns["date_of_completion"].HeaderText = "Дата завершения";
                    if (reportsDataGridView.Columns.Contains("discount"))
                        reportsDataGridView.Columns["discount"].HeaderText = "Скидка (%)";
                    if (reportsDataGridView.Columns.Contains("total_amount"))
                        reportsDataGridView.Columns["total_amount"].HeaderText = "Сумма (руб.)";
                    if (reportsDataGridView.Columns.Contains("final_amount"))
                        reportsDataGridView.Columns["final_amount"].HeaderText = "Итог (руб.)";
                    break;

                case "Отчет по продажам":
                    if (reportsDataGridView.Columns.Contains("product_name"))
                        reportsDataGridView.Columns["product_name"].HeaderText = "Товар";
                    if (reportsDataGridView.Columns.Contains("category_name"))
                        reportsDataGridView.Columns["category_name"].HeaderText = "Категория";
                    if (reportsDataGridView.Columns.Contains("total_quantity"))
                        reportsDataGridView.Columns["total_quantity"].HeaderText = "Количество";
                    if (reportsDataGridView.Columns.Contains("total_revenue"))
                        reportsDataGridView.Columns["total_revenue"].HeaderText = "Выручка (руб.)";
                    if (reportsDataGridView.Columns.Contains("avg_price"))
                        reportsDataGridView.Columns["avg_price"].HeaderText = "Средняя цена (руб.)";
                    break;

                case "Отчет по клиентам":
                    if (reportsDataGridView.Columns.Contains("fio"))
                        reportsDataGridView.Columns["fio"].HeaderText = "ФИО";
                    if (reportsDataGridView.Columns.Contains("phone"))
                        reportsDataGridView.Columns["phone"].HeaderText = "Телефон";
                    if (reportsDataGridView.Columns.Contains("orders_count"))
                        reportsDataGridView.Columns["orders_count"].HeaderText = "Кол-во заказов";
                    if (reportsDataGridView.Columns.Contains("total_spent"))
                        reportsDataGridView.Columns["total_spent"].HeaderText = "Потрачено (руб.)";
                    if (reportsDataGridView.Columns.Contains("last_order_date"))
                        reportsDataGridView.Columns["last_order_date"].HeaderText = "Последний заказ";
                    break;

                case "Отчет по товарам":
                    if (reportsDataGridView.Columns.Contains("name"))
                        reportsDataGridView.Columns["name"].HeaderText = "Наименование";
                    if (reportsDataGridView.Columns.Contains("article"))
                        reportsDataGridView.Columns["article"].HeaderText = "Артикул";
                    if (reportsDataGridView.Columns.Contains("category_name"))
                        reportsDataGridView.Columns["category_name"].HeaderText = "Категория";
                    if (reportsDataGridView.Columns.Contains("price"))
                        reportsDataGridView.Columns["price"].HeaderText = "Цена (руб.)";
                    if (reportsDataGridView.Columns.Contains("sold_quantity"))
                        reportsDataGridView.Columns["sold_quantity"].HeaderText = "Продано (шт.)";
                    break;
            }

            // Скрываем служебные поля
            HideTechnicalColumns();
        }

        private void HideTechnicalColumns()
        {
            string[] technicalColumns = { "id", "client_id", "product_id", "status_id", "category_id" };

            foreach (var colName in technicalColumns)
            {
                if (reportsDataGridView.Columns.Contains(colName))
                {
                    reportsDataGridView.Columns[colName].Visible = false;
                }
            }
        }

        private DataTable GenerateOrdersReport()
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT o.order_number, c.fio as client_name, 
                                p.name as product_name, s.name as status_name,
                                o.date_of_creation, o.date_of_completion,
                                o.discount, o.total_amount, o.final_amount
                                FROM orders o
                                INNER JOIN clients c ON o.client_id = c.id
                                INNER JOIN products p ON o.product_id = p.id
                                INNER JOIN statuses s ON o.status_id = s.id
                                WHERE o.date_of_creation BETWEEN @from_date AND @to_date
                                ORDER BY o.date_of_creation DESC";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@from_date", reportFromDatePicker.Value.Date);
                    command.Parameters.AddWithValue("@to_date", reportToDatePicker.Value.Date.AddDays(1));

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private DataTable GenerateSalesReport()
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT p.name as product_name, c.name as category_name,
                                SUM(oi.quantity) as total_quantity,
                                SUM(oi.total) as total_revenue,
                                AVG(oi.price) as avg_price
                                FROM order_items oi
                                INNER JOIN products p ON oi.product_id = p.id
                                INNER JOIN categories c ON p.category_id = c.id
                                INNER JOIN orders o ON oi.order_id = o.id
                                WHERE o.date_of_creation BETWEEN @from_date AND @to_date
                                GROUP BY p.name, c.name
                                ORDER BY total_revenue DESC";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@from_date", reportFromDatePicker.Value.Date);
                    command.Parameters.AddWithValue("@to_date", reportToDatePicker.Value.Date.AddDays(1));

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private DataTable GenerateClientsReport()
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT c.fio, c.phone,
                                COUNT(o.id) as orders_count,
                                SUM(o.final_amount) as total_spent,
                                MAX(o.date_of_creation) as last_order_date
                                FROM clients c
                                LEFT JOIN orders o ON c.id = o.client_id
                                WHERE (o.date_of_creation BETWEEN @from_date AND @to_date 
                                       OR o.date_of_creation IS NULL)
                                GROUP BY c.fio, c.phone
                                ORDER BY total_spent DESC";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@from_date", reportFromDatePicker.Value.Date);
                    command.Parameters.AddWithValue("@to_date", reportToDatePicker.Value.Date.AddDays(1));

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private DataTable GenerateProductsReport()
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT p.name, p.article, c.name as category_name,
                                p.price,
                                (SELECT SUM(quantity) FROM order_items oi 
                                 INNER JOIN orders o ON oi.order_id = o.id 
                                 WHERE oi.product_id = p.id 
                                 AND o.date_of_creation BETWEEN @from_date AND @to_date) as sold_quantity
                                FROM products p
                                INNER JOIN categories c ON p.category_id = c.id
                                ORDER BY p.name";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@from_date", reportFromDatePicker.Value.Date);
                    command.Parameters.AddWithValue("@to_date", reportToDatePicker.Value.Date.AddDays(1));

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private void CalculateTotals(DataTable data, string reportType)
        {
            decimal totalAmount = 0;
            int totalCount = data.Rows.Count;

            switch (reportType)
            {
                case "Отчет по заказам":
                    foreach (DataRow row in data.Rows)
                    {
                        if (row["final_amount"] != DBNull.Value)
                        {
                            totalAmount += Convert.ToDecimal(row["final_amount"]);
                        }
                    }
                    break;

                case "Отчет по продажам":
                    foreach (DataRow row in data.Rows)
                    {
                        if (row["total_revenue"] != DBNull.Value)
                        {
                            totalAmount += Convert.ToDecimal(row["total_revenue"]);
                        }
                    }
                    break;

                case "Отчет по клиентам":
                    foreach (DataRow row in data.Rows)
                    {
                        if (row["total_spent"] != DBNull.Value)
                        {
                            totalAmount += Convert.ToDecimal(row["total_spent"]);
                        }
                    }
                    break;
            }

            totalsLabel.Text = $"Всего записей: {totalCount} | Общая сумма: {totalAmount:C2}";
        }

        private void printToWordButton_Click(object sender, EventArgs e)
        {
            if (reportsDataGridView.DataSource == null)
            {
                MessageBox.Show("Нет данных для печати", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ExportToWord();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Word: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printToExcelButton_Click(object sender, EventArgs e)
        {
            if (reportsDataGridView.DataSource == null)
            {
                MessageBox.Show("Нет данных для печати", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ExportToExcel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToWord()
        {
            try
            {
                // Используем Interop для работы с Word
                var wordApp = new Microsoft.Office.Interop.Word.Application();
                var doc = wordApp.Documents.Add();
                wordApp.Visible = true;

                // Заголовок отчета
                var reportType = reportTypeComboBox.SelectedItem.ToString();
                var range = doc.Range(0, 0);
                range.Text = $"{reportType}\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 16;
                range.Font.Bold = 1;
                range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                // Период
                range = doc.Range(range.End, range.End);
                range.Text = $"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy}\n\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 12;
                range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                // Таблица с данными
                DataTable dataTable = (DataTable)reportsDataGridView.DataSource;

                // Создаем таблицу в Word
                range = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
                var table = doc.Tables.Add(range, dataTable.Rows.Count + 1, dataTable.Columns.Count);
                table.Borders.Enable = 1;
                table.Range.Font.Name = "Times New Roman";
                table.Range.Font.Size = 10;

                // Заполняем заголовки таблицы
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (reportsDataGridView.Columns[i].Visible)
                    {
                        table.Cell(1, i + 1).Range.Text = reportsDataGridView.Columns[i].HeaderText;
                        table.Cell(1, i + 1).Range.Font.Bold = 1;
                    }
                }

                // Заполняем данные таблицы
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        if (reportsDataGridView.Columns[col].Visible)
                        {
                            var cellValue = dataTable.Rows[row][col].ToString();
                            table.Cell(row + 2, col + 1).Range.Text = cellValue;
                        }
                    }
                }

                // Итоги
                range = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
                range.Text = $"\n{totalsLabel.Text}\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 11;
                range.Font.Bold = 1;

                // Дата создания отчета
                range = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
                range.Text = $"Отчет создан: {DateTime.Now:dd.MM.yyyy HH:mm}\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 10;
                range.Font.Italic = 1;

                MessageBox.Show("Отчет успешно сформирован в Word", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // Если Interop не доступен, создаем простой текстовый файл
                SaveToTextFile("Word");
            }
        }

        private void ExportToExcel()
        {
            if (reportsDataGridView.DataSource == null)
            {
                MessageBox.Show("Нет данных для экспорта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Создаем временный файл
                string tempFile = Path.GetTempPath() + $"{reportTypeComboBox.SelectedItem}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                // Используем Excel Interop для полного контроля
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                var workbook = excelApp.Workbooks.Add();
                var worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

                // Делаем Excel видимым
                excelApp.Visible = true;

                DataTable dataTable = (DataTable)reportsDataGridView.DataSource;

                // Заголовок отчета
                worksheet.Cells[1, 1] = $"{reportTypeComboBox.SelectedItem}";
                worksheet.Range["A1", "Z1"].Merge();
                worksheet.Cells[1, 1].Font.Bold = true;
                worksheet.Cells[1, 1].Font.Size = 14;
                worksheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // Период
                worksheet.Cells[2, 1] = $"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy}";
                worksheet.Range["A2", "Z2"].Merge();
                worksheet.Cells[2, 1].Font.Size = 12;
                worksheet.Cells[2, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // Пустая строка
                int startRow = 4;

                // Заголовки столбцов
                int col = 1;
                for (int i = 0; i < reportsDataGridView.Columns.Count; i++)
                {
                    if (reportsDataGridView.Columns[i].Visible)
                    {
                        worksheet.Cells[startRow, col] = reportsDataGridView.Columns[i].HeaderText;
                        worksheet.Cells[startRow, col].Font.Bold = true;
                        worksheet.Cells[startRow, col].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        col++;
                    }
                }

                // Данные
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    col = 1;
                    for (int i = 0; i < reportsDataGridView.Columns.Count; i++)
                    {
                        if (reportsDataGridView.Columns[i].Visible)
                        {
                            var columnName = reportsDataGridView.Columns[i].DataPropertyName ?? reportsDataGridView.Columns[i].Name;
                            worksheet.Cells[startRow + row + 1, col] = dataTable.Rows[row][columnName];
                            col++;
                        }
                    }
                }

                // РАСТЯГИВАЕМ СТОЛБЦЫ ПО СОДЕРЖИМОМУ
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
                usedRange.Columns.AutoFit();

                // Дополнительные настройки автоподбора
                foreach (Microsoft.Office.Interop.Excel.Range column in usedRange.Columns)
                {
                    column.ColumnWidth = column.ColumnWidth * 1.2; // Добавляем 20% запаса
                }

                // Сохраняем и закрываем
                workbook.SaveAs(tempFile);

                // Оставляем Excel открытым
                // workbook.Close(); // Не закрываем, чтобы пользователь видел файл
                // excelApp.Quit();

                MessageBox.Show("Отчет открыт в Excel с автоподбором ширины столбцов", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nУбедитесь, что Microsoft Excel установлен.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string filePath)
        {
            DataTable dataTable = (DataTable)reportsDataGridView.DataSource;

            using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Добавляем BOM для правильного отображения кириллицы в Excel
                writer.Write('\uFEFF');

                // Собираем видимые столбцы
                List<DataGridViewColumn> visibleColumns = new List<DataGridViewColumn>();
                for (int i = 0; i < reportsDataGridView.Columns.Count; i++)
                {
                    if (reportsDataGridView.Columns[i].Visible)
                    {
                        visibleColumns.Add(reportsDataGridView.Columns[i]);
                    }
                }

                // Заголовки столбцов
                List<string> headers = new List<string>();
                foreach (var column in visibleColumns)
                {
                    headers.Add(column.HeaderText);
                }
                writer.WriteLine(string.Join(";", headers));

                // Данные
                foreach (DataRow row in dataTable.Rows)
                {
                    List<string> values = new List<string>();
                    foreach (var column in visibleColumns)
                    {
                        var columnName = column.DataPropertyName;
                        if (string.IsNullOrEmpty(columnName))
                        {
                            columnName = column.Name;
                        }

                        var value = row[columnName].ToString();
                        // Экранируем специальные символы
                        if (value.Contains(";") || value.Contains("\"") || value.Contains("\n"))
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }
                        values.Add(value);
                    }
                    writer.WriteLine(string.Join(";", values));
                }

                // Добавляем информацию об отчете
                writer.WriteLine();
                writer.WriteLine(";;"); // Пустая строка
                writer.WriteLine($"{totalsLabel.Text};");
                writer.WriteLine($"Отчет создан: {DateTime.Now:dd.MM.yyyy HH:mm};");
                writer.WriteLine($"Тип отчета: {reportTypeComboBox.SelectedItem};");
                writer.WriteLine($"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy};");
            }
        }

        private void ExportToExcelUsingInterop(string filePath)
        {
            try
            {
                // Используем Excel Interop
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                var workbook = excelApp.Workbooks.Add();
                var worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

                DataTable dataTable = (DataTable)reportsDataGridView.DataSource;

                // Заголовок отчета
                var reportType = reportTypeComboBox.SelectedItem.ToString();
                worksheet.Cells[1, 1] = $"ОТЧЕТ: {reportType}";
                var headerRange = worksheet.Range["A1", GetExcelColumnName(reportsDataGridView.Columns.Count) + "1"];
                headerRange.Merge();
                headerRange.Font.Bold = true;
                headerRange.Font.Size = 16;
                headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // Период
                worksheet.Cells[2, 1] = $"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy}";
                var periodRange = worksheet.Range["A2", GetExcelColumnName(reportsDataGridView.Columns.Count) + "2"];
                periodRange.Merge();
                periodRange.Font.Size = 12;
                periodRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // Заголовки столбцов (строка 4)
                int row = 4;
                int col = 1;

                // Собираем видимые столбцы
                List<DataGridViewColumn> visibleColumns = new List<DataGridViewColumn>();
                for (int i = 0; i < reportsDataGridView.Columns.Count; i++)
                {
                    if (reportsDataGridView.Columns[i].Visible)
                    {
                        visibleColumns.Add(reportsDataGridView.Columns[i]);
                    }
                }

                // Записываем заголовки
                foreach (var column in visibleColumns)
                {
                    worksheet.Cells[row, col] = column.HeaderText;
                    var cell = worksheet.Cells[row, col];
                    cell.Font.Bold = true;
                    cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    cell.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    col++;
                }

                // Записываем данные
                row++;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    col = 1;
                    foreach (var column in visibleColumns)
                    {
                        var columnName = column.DataPropertyName;
                        if (string.IsNullOrEmpty(columnName))
                        {
                            columnName = column.Name;
                        }

                        var value = dataRow[columnName];
                        worksheet.Cells[row, col] = value;

                        // Форматирование для числовых полей
                        if (value is decimal || value is double || value is int || value is float)
                        {
                            var cell = worksheet.Cells[row, col];
                            cell.NumberFormat = "#,##0.00";

                            // Если это денежное поле
                            if (column.HeaderText.Contains("руб.") ||
                                column.HeaderText.Contains("Сумма") ||
                                column.HeaderText.Contains("Цена") ||
                                column.HeaderText.Contains("Итог"))
                            {
                                cell.NumberFormat = "#,##0.00 ₽";
                            }
                            else if (column.HeaderText.Contains("Скидка"))
                            {
                                cell.NumberFormat = "0%";
                            }
                        }

                        var dataCell = worksheet.Cells[row, col];
                        dataCell.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        col++;
                    }
                    row++;
                }

                // Автонастройка ширины столбцов
                worksheet.Columns.AutoFit();

                // Итоги
                worksheet.Cells[row + 1, 1] = totalsLabel.Text;
                var totalsRange = worksheet.Range[worksheet.Cells[row + 1, 1], worksheet.Cells[row + 1, visibleColumns.Count]];
                totalsRange.Merge();
                totalsRange.Font.Bold = true;
                totalsRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                // Дата создания отчета
                worksheet.Cells[row + 2, 1] = $"Отчет создан: {DateTime.Now:dd.MM.yyyy HH:mm}";
                var dateRange = worksheet.Range[worksheet.Cells[row + 2, 1], worksheet.Cells[row + 2, visibleColumns.Count]];
                dateRange.Merge();
                dateRange.Font.Italic = true;

                // Тип отчета
                worksheet.Cells[row + 3, 1] = $"Тип отчета: {reportType}";
                var typeRange = worksheet.Range[worksheet.Cells[row + 3, 1], worksheet.Cells[row + 3, visibleColumns.Count]];
                typeRange.Merge();

                // Период
                worksheet.Cells[row + 4, 1] = $"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy}";
                var periodRange2 = worksheet.Range[worksheet.Cells[row + 4, 1], worksheet.Cells[row + 4, visibleColumns.Count]];
                periodRange2.Merge();

                // Сохраняем файл
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                // Освобождаем ресурсы COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception)
            {
                // Если Interop не работает, сохраняем как CSV
                string csvPath = Path.ChangeExtension(filePath, ".csv");
                ExportToCsv(csvPath);
                throw new Exception($"Не удалось сохранить как Excel файл. Файл сохранен как CSV: {csvPath}");
            }
        }

        // Вспомогательный метод для получения имени столбца Excel
        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }
            return columnName;
        }

        private void ExportToExcelFallback()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV файлы (*.csv)|*.csv|Текстовые файлы (*.txt)|*.txt";
            saveFileDialog.FileName = $"{reportTypeComboBox.SelectedItem}_{DateTime.Now:yyyyMMdd_HHmmss}";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dataTable = (DataTable)reportsDataGridView.DataSource;

                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Заголовок отчета
                    writer.WriteLine($"ОТЧЕТ: {reportTypeComboBox.SelectedItem}");
                    writer.WriteLine($"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy}");
                    writer.WriteLine();

                    // Заголовки столбцов
                    List<string> headers = new List<string>();
                    for (int i = 0; i < reportsDataGridView.Columns.Count; i++)
                    {
                        if (reportsDataGridView.Columns[i].Visible)
                        {
                            headers.Add(reportsDataGridView.Columns[i].HeaderText);
                        }
                    }
                    writer.WriteLine(string.Join(";", headers));

                    // Данные
                    foreach (DataRow row in dataTable.Rows)
                    {
                        List<string> values = new List<string>();
                        for (int i = 0; i < reportsDataGridView.Columns.Count; i++)
                        {
                            if (reportsDataGridView.Columns[i].Visible)
                            {
                                values.Add(row[i].ToString());
                            }
                        }
                        writer.WriteLine(string.Join(";", values));
                    }

                    // Итоги
                    writer.WriteLine();
                    writer.WriteLine(totalsLabel.Text);
                    writer.WriteLine($"Отчет создан: {DateTime.Now:dd.MM.yyyy HH:mm}");
                }

                MessageBox.Show($"Отчет сохранен как CSV: {saveFileDialog.FileName}\nОткройте файл в Excel.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveToTextFile(string formatType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (formatType == "Word")
            {
                saveFileDialog.Filter = "Документ Word (*.doc)|*.doc|Текстовый файл (*.txt)|*.txt";
            }
            else // Excel
            {
                saveFileDialog.Filter = "Excel файл (*.csv)|*.csv|Текстовый файл (*.txt)|*.txt";
            }

            saveFileDialog.FileName = $"{reportTypeComboBox.SelectedItem}_{DateTime.Now:yyyyMMdd_HHmmss}";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Заголовок
                    writer.WriteLine($"{reportTypeComboBox.SelectedItem}");
                    writer.WriteLine($"Период: {reportFromDatePicker.Value:dd.MM.yyyy} - {reportToDatePicker.Value:dd.MM.yyyy}");
                    writer.WriteLine(new string('-', 80));
                    writer.WriteLine();

                    // Заголовки таблицы
                    DataTable dataTable = (DataTable)reportsDataGridView.DataSource;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (reportsDataGridView.Columns[column.ColumnName].Visible)
                        {
                            writer.Write($"{reportsDataGridView.Columns[column.ColumnName].HeaderText}\t");
                        }
                    }
                    writer.WriteLine();
                    writer.WriteLine(new string('-', 80));

                    // Данные таблицы
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            if (reportsDataGridView.Columns[column.ColumnName].Visible)
                            {
                                writer.Write($"{row[column].ToString()}\t");
                            }
                        }
                        writer.WriteLine();
                    }

                    // Итоги
                    writer.WriteLine();
                    writer.WriteLine(new string('-', 80));
                    writer.WriteLine(totalsLabel.Text);
                    writer.WriteLine($"Отчет создан: {DateTime.Now:dd.MM.yyyy HH:mm}");
                }

                MessageBox.Show($"Отчет успешно сохранен: {saveFileDialog.FileName}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reportFromDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (reportFromDatePicker.Value > reportToDatePicker.Value)
            {
                reportToDatePicker.Value = reportFromDatePicker.Value;
            }
        }

        private void reportToDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (reportToDatePicker.Value < reportFromDatePicker.Value)
            {
                reportFromDatePicker.Value = reportToDatePicker.Value;
            }
        }
    }
}
