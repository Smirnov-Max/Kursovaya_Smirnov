using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Smirnov_kursovaya.Database;
using Smirnov_kursovaya.secondForm;

namespace Smirnov_kursovaya.mainForm
{
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
            ApplyCoralButtonStyle();
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            statusLabel.Text = "Менеджер";
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Authentication auth = new Authentication();
                auth.Show();
                this.Close();
            }
        }

        private void ordersButton_Click(object sender, EventArgs e)
        {
            ViewOrderForm viewOrderForm = new ViewOrderForm(true); // true - режим менеджера
            viewOrderForm.ShowDialog();
        }

        private void reportsButton_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void productsButton_Click(object sender, EventArgs e)
        {
            ProductsForm productsForm = new ProductsForm();
            productsForm.ShowDialog();
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

            // Особый стиль для кнопки выход (можно сделать другого цвета)
            if (exitButton != null)
            {
                exitButton.BackColor = Color.Red; // Cornflower Blue
                exitButton.FlatStyle = FlatStyle.Flat;
                exitButton.FlatAppearance.BorderColor = Color.DarkRed;
                exitButton.FlatAppearance.BorderSize = 1;
                exitButton.ForeColor = Color.Black;
                exitButton.Font = new Font(exitButton.Font, FontStyle.Regular);

                // Убираем старые обработчики и добавляем новые
                exitButton.MouseEnter -= (s, e) => { };
                exitButton.MouseLeave -= (s, e) => { };
                exitButton.MouseDown -= (s, e) => { };
                exitButton.MouseUp -= (s, e) => { };

                exitButton.MouseEnter += (s, e) => {
                    exitButton.BackColor = Color.IndianRed;
                };
                exitButton.MouseLeave += (s, e) => {
                    exitButton.BackColor = Color.Red;
                };
                exitButton.MouseDown += (s, e) => {
                    exitButton.BackColor = Color.OrangeRed;
                };
                exitButton.MouseUp += (s, e) => {
                    exitButton.BackColor = Color.OrangeRed;
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
    }
}