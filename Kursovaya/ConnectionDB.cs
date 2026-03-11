using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Smirnov_kursovaya.Database
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=10.207.106.12;Database=db102;Uid=user102;Pwd=qk44;";
        //private string connectionString = "Server=127.0.0.1;Database=db102;Uid=root;Pwd=root;";

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
    }
}