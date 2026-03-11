using Smirnov_kursovaya.mainForm;
using System;
using System.Windows.Forms;

namespace Kursovaya
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Проверка подключения к базе данных
            try
            {
                var dbHelper = new Smirnov_kursovaya.Database.DatabaseHelper();
                if (dbHelper.TestConnection())
                {
                    Application.Run(new Authentication());
                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к базе данных. Проверьте:\n" +
                        "1. Запущен ли MySQL сервер\n" +
                        "2. Правильность параметров подключения\n" +
                        "3. Существование базы данных db102",
                        "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}