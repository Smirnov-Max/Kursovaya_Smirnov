using System.Windows.Forms;

namespace Smirnov_kursovaya.mainForm
{
    partial class Authentication
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        // CAPTCHA: новые элементы
        private System.Windows.Forms.PictureBox pictureBoxCaptcha;
        private System.Windows.Forms.TextBox txtCaptcha;
        private System.Windows.Forms.Button btnRefreshCaptcha;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Authentication));
            this.panel1 = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            // CAPTCHA: создание элементов
            this.pictureBoxCaptcha = new System.Windows.Forms.PictureBox();
            this.txtCaptcha = new System.Windows.Forms.TextBox();
            this.btnRefreshCaptcha = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaptcha)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.titleLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 60);
            this.panel1.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.DarkSalmon;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(434, 60);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Авторизация в системе";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRefreshCaptcha);
            this.panel2.Controls.Add(this.txtCaptcha);
            this.panel2.Controls.Add(this.pictureBoxCaptcha);
            this.panel2.Controls.Add(this.cancelButton);
            this.panel2.Controls.Add(this.loginButton);
            this.panel2.Controls.Add(this.passwordTextBox);
            this.panel2.Controls.Add(this.usernameTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 60);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20);
            this.panel2.Size = new System.Drawing.Size(434, 200); // Начальная высота для обычного режима
            this.panel2.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.BackColor = System.Drawing.Color.Coral;
            this.cancelButton.Location = new System.Drawing.Point(324, 160);
            this.cancelButton.MinimumSize = new System.Drawing.Size(90, 30);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Выйти";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loginButton.BackColor = System.Drawing.Color.Coral;
            this.loginButton.Location = new System.Drawing.Point(228, 160);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(90, 30);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(124, 80);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(290, 20);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordTextBox_KeyPress);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usernameTextBox.Location = new System.Drawing.Point(124, 40);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(290, 20);
            this.usernameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин:";
            // 
            // pictureBoxCaptcha
            // 
            this.pictureBoxCaptcha.Location = new System.Drawing.Point(124, 110);
            this.pictureBoxCaptcha.Name = "pictureBoxCaptcha";
            this.pictureBoxCaptcha.Size = new System.Drawing.Size(200, 50);
            this.pictureBoxCaptcha.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxCaptcha.TabIndex = 6;
            this.pictureBoxCaptcha.TabStop = false;
            this.pictureBoxCaptcha.Visible = false;
            // 
            // txtCaptcha
            // 
            this.txtCaptcha.Location = new System.Drawing.Point(124, 170);
            this.txtCaptcha.Name = "txtCaptcha";
            this.txtCaptcha.Size = new System.Drawing.Size(200, 20);
            this.txtCaptcha.TabIndex = 7;
            this.txtCaptcha.Visible = false;
            // 
            // btnRefreshCaptcha
            // 
            this.btnRefreshCaptcha.BackColor = System.Drawing.Color.Coral;
            this.btnRefreshCaptcha.Location = new System.Drawing.Point(330, 168);
            this.btnRefreshCaptcha.Name = "btnRefreshCaptcha";
            this.btnRefreshCaptcha.Size = new System.Drawing.Size(80, 23);
            this.btnRefreshCaptcha.TabIndex = 8;
            this.btnRefreshCaptcha.Text = "Обновить";
            this.btnRefreshCaptcha.UseVisualStyleBackColor = false;
            this.btnRefreshCaptcha.Visible = false;
            this.btnRefreshCaptcha.Click += new System.EventHandler(this.btnRefreshCaptcha_Click);
            // 
            // Authentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 260); // Обычный размер (без капчи)
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запрещаем ручное изменение размера
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 340); // Максимальный размер для режима с капчей
            this.MinimumSize = new System.Drawing.Size(450, 260); // Минимальный размер (обычный)
            this.Name = "Authentication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.Authentication_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaptcha)).EndInit();
            this.ResumeLayout(false);

        }
    }
}