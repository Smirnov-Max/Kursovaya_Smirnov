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
        /// </summary>
        /// <returns>Количество успешно импортированных записей</returns>
        public int ImportCsv(string tableName, string filePath, char delimiter = ';')
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден.");

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length == 0)
                return 0;

            int columnCount = GetColumnCount(tableName);
            var columnNames = GetColumnNames(tableName);
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

                            string cols = string.Join(",", columnNames);
                            string placeholders = string.Join(",", values.Select((_, i) => "@p" + i));
                            string query = $"INSERT INTO {tableName} ({cols}) VALUES ({placeholders})";

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
    }
}