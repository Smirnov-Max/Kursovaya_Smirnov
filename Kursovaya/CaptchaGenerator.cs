using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Smirnov_kursovaya.Helpers
{
    public static class CaptchaGenerator
    {
        private static readonly Random random = new Random();
        private const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";

        public static Bitmap Generate(int length, out string captchaText)
        {
            // Генерация случайного текста
            captchaText = "";
            for (int i = 0; i < length; i++)
                captchaText += chars[random.Next(chars.Length)];

            Bitmap bmp = new Bitmap(200, 60);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Рисуем символы с искажениями
                for (int i = 0; i < captchaText.Length; i++)
                {
                    float x = 20 + i * 35 + random.Next(-5, 5);
                    float y = 15 + random.Next(-5, 5);
                    using (Font font = new Font("Arial", 22, FontStyle.Bold | FontStyle.Italic))
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(captchaText[i].ToString(), font, brush, x, y);
                    }
                }

                // Добавляем шум: линии
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(bmp.Width);
                    int y1 = random.Next(bmp.Height);
                    int x2 = random.Next(bmp.Width);
                    int y2 = random.Next(bmp.Height);
                    g.DrawLine(new Pen(Color.LightGray, 1), x1, y1, x2, y2);
                }

                // Точки
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(bmp.Width);
                    int y = random.Next(bmp.Height);
                    bmp.SetPixel(x, y, Color.LightGray);
                }
            }
            return bmp;
        }
    }
}