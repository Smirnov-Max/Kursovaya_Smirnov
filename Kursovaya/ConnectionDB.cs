using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Smirnov_kursovaya.Database
{
    public class DatabaseHelper
    {
        //private string connectionString = "Server=10.207.106.12;Database=db102;Uid=user102;Pwd=qk44;";
        private string connectionString = "Server=127.0.0.1;Database=db102;Uid=root;Pwd=root;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ========== НОВЫЕ МЕТОДЫ ДЛЯ ЗАДАНИЯ ==========

        /// <summary>
        /// Выполняет SQL-скрипт (создание таблиц и т.п.). Скрипт может содержать несколько команд, разделённых ';'.
        /// </summary>
        public void ExecuteScript(string script)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var commands = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var commandText in commands)
                {
                    if (!string.IsNullOrWhiteSpace(commandText))
                    {
                        using (var cmd = new MySqlCommand(commandText, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает список имён всех таблиц в базе данных.
        /// </summary>
        public List<string> GetTableList()
        {
            var tables = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SHOW TABLES", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }
            return tables;
        }

        /// <summary>
        /// Возвращает количество столбцов в указанной таблице.
        /// </summary>
        public int GetColumnCount(string tableName)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var schema = conn.GetSchema("Columns", new[] { null, null, tableName, null });
                return schema.Rows.Count;
            }
        }

        /// <summary>
        /// Возвращает список имён столбцов таблицы.
        /// </summary>
        public List<string> GetColumnNames(string tableName)
        {
            var columns = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var schema = conn.GetSchema("Columns", new[] { null, null, tableName, null });
                foreach (DataRow row in schema.Rows)
                {
                    columns.Add(row["COLUMN_NAME"].ToString());
                }
            }
            return columns;
        }

        /// <summary>
        /// Импортирует данные из CSV-файла в указанную таблицу.
        /// Разделитель по умолчанию — точка с запятой.
        /// Файл должен содержать все поля таблицы, включая id (в порядке, определённом в БД).
        /// Использует INSERT IGNORE для пропуска строк с дублирующимся id.
        /// </summary>
        /// <returns>Количество успешно импортированных записей</returns>
        public int ImportCsv(string tableName, string filePath, char delimiter = ';')
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден.");

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length == 0)
                return 0;

            // Получаем все колонки таблицы (включая id)
            var allColumnNames = GetColumnNames(tableName);
            int columnCount = allColumnNames.Count;
            int imported = 0;

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var line in lines)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            var values = line.Split(delimiter);
                            if (values.Length != columnCount)
                                throw new Exception($"Несоответствие числа столбцов: ожидалось {columnCount}, получено {values.Length} в строке: {line}");

                            string cols = string.Join(",", allColumnNames);
                            string placeholders = string.Join(",", values.Select((_, i) => "@p" + i));
                            // Используем INSERT IGNORE, чтобы пропускать дубликаты без ошибки
                            string query = $"INSERT IGNORE INTO {tableName} ({cols}) VALUES ({placeholders})";

                            using (var cmd = new MySqlCommand(query, conn, transaction))
                            {
                                for (int i = 0; i < values.Length; i++)
                                {
                                    // Пустые строки превращаем в DBNull
                                    if (string.IsNullOrEmpty(values[i]))
                                        cmd.Parameters.AddWithValue($"@p{i}", DBNull.Value);
                                    else
                                        cmd.Parameters.AddWithValue($"@p{i}", values[i]);
                                }
                                cmd.ExecuteNonQuery();
                            }
                            imported++;
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return imported;
        }
        public DataTable GetProductsWithPagination(int page, int pageSize, string searchText, string category, out int totalRecords)
        {
            DataTable dt = new DataTable();
            totalRecords = 0;

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();

                    string countQuery = "SELECT COUNT(*) FROM products p LEFT JOIN categories c ON p.category_id = c.id WHERE 1=1";

                    if (!string.IsNullOrEmpty(searchText))
                        countQuery += " AND (p.name LIKE @search OR p.article LIKE @search)";

                    if (!string.IsNullOrEmpty(category) && category != "Все категории")
                        countQuery += " AND c.name = @category";

                    using (var cmd = new MySqlCommand(countQuery, conn))
                    {
                        if (!string.IsNullOrEmpty(searchText))
                            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        if (!string.IsNullOrEmpty(category) && category != "Все категории")
                            cmd.Parameters.AddWithValue("@category", category);

                        totalRecords = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // УБИРАЕМ quantity ИЗ ЗАПРОСА
                    string query = @"
                SELECT 
                    p.id, 
                    p.article, 
                    p.name, 
                    p.description, 
                    p.price, 
                    p.image, 
                    c.name as category_name,
                    c.id as category_id
                FROM products p 
                LEFT JOIN categories c ON p.category_id = c.id 
                WHERE 1=1";

                    if (!string.IsNullOrEmpty(searchText))
                        query += " AND (p.name LIKE @search OR p.article LIKE @search)";

                    if (!string.IsNullOrEmpty(category) && category != "Все категории")
                        query += " AND c.name = @category";

                    query += " ORDER BY p.id LIMIT @offset, @limit";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(searchText))
                            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        if (!string.IsNullOrEmpty(category) && category != "Все категории")
                            cmd.Parameters.AddWithValue("@category", category);

                        cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                        cmd.Parameters.AddWithValue("@limit", pageSize);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при загрузке данных: " + ex.Message);
            }

            return dt;
        }
    }
}