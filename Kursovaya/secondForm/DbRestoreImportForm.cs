using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Smirnov_kursovaya.Database;
using Smirnov_kursovaya.mainForm;

namespace Smirnov_kursovaya.secondForm
{
    public partial class DbRestoreImportForm : Form
    {
        private DatabaseHelper dbHelper;

        public DbRestoreImportForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadTableList();
            ApplyCoralButtonStyle();
        }

        private void ApplyCoralButtonStyle()
        {
            Color coralColor = Color.FromArgb(255, 127, 80);
            Color coralLightColor = Color.FromArgb(255, 147, 100);
            Color coralDarkColor = Color.FromArgb(235, 107, 60);

            // Применяем стиль ко всем кнопкам на форме
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

            // Кнопка меню — красная (как в других формах)
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
                menuButton.MouseUp += (s, e) => menuButton.BackColor = Color.IndianRed;
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

            button.MouseEnter += (s, e) => button.BackColor = hoverColor;
            button.MouseLeave += (s, e) => button.BackColor = normalColor;
            button.MouseDown += (s, e) => button.BackColor = pressedColor;
            button.MouseUp += (s, e) => button.BackColor = hoverColor;
        }

        private void DbRestoreImportForm_Load(object sender, EventArgs e)
        {
            // Дополнительные настройки при загрузке
        }

        private void LoadTableList()
        {
            try
            {
                var tables = dbHelper.GetTableList();
                cmbTables.Items.Clear();
                foreach (var table in tables)
                {
                    cmbTables.Items.Add(table);
                }
                if (cmbTables.Items.Count > 0)
                    cmbTables.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Ошибка загрузки списка таблиц: " + ex.Message;
            }
        }

        private void btnBrowseScript_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "SQL files (*.sql)|*.sql";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtScriptPath.Text = openFileDialog.FileName;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Внимание! Это приведет к созданию новой структуры базы данных.\n" +
                    "Все существующие данные будут потеряны!\n\nПродолжить?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string scriptPath = txtScriptPath.Text;
                    if (!File.Exists(scriptPath))
                    {
                        // Если файл не найден, используем встроенный скрипт
                        scriptPath = Path.Combine(Application.StartupPath, "CreateTables.sql");
                        if (!File.Exists(scriptPath))
                        {
                            MessageBox.Show("Файл скрипта не найден: " + txtScriptPath.Text, "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string script = File.ReadAllText(scriptPath, Encoding.UTF8);
                    dbHelper.ExecuteScript(script);
                    MessageBox.Show("Структура базы данных успешно создана.", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Структура восстановлена";
                    LoadTableList(); // Обновляем список таблиц
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Ошибка при восстановлении";
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу для импорта.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string tableName = cmbTables.SelectedItem.ToString();
                    int imported = dbHelper.ImportCsv(tableName, openFileDialog.FileName);
                    MessageBox.Show($"Импорт завершен!\nДобавлено записей: {imported}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = $"Импортировано {imported} записей в таблицу {tableName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка импорта: " + ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Ошибка при импорте";
                }
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            Authentication authForm = new Authentication();
            authForm.Show();

            // Закрываем текущую форму (форму восстановления/импорта)
            this.Close();
        }
    }
}