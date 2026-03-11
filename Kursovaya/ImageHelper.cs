using System;
using System.Drawing;
using System.IO;

namespace Smirnov_kursovaya.Helpers
{
    public static class ImageHelper
    {
        // Путь к изображению-заглушке (создайте в проекте папку Resources и добавьте туда placeholder.png)
        private static readonly string PlaceholderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "placeholder.png");

        // Путь для сохранения изображений
        private static readonly string ImagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");

        // Изображение-заглушка
        private static Image _placeholder;
        public static Image Placeholder
        {
            get
            {
                if (_placeholder == null)
                {
                    try
                    {
                        if (File.Exists(PlaceholderPath))
                        {
                            _placeholder = Image.FromFile(PlaceholderPath);
                        }
                        else
                        {
                            // Создаем изображение-заглушку программно
                            _placeholder = CreatePlaceholderImage();
                        }
                    }
                    catch
                    {
                        _placeholder = CreatePlaceholderImage();
                    }
                }
                return _placeholder;
            }
        }

        // Создание изображения-заглушки
        private static Image CreatePlaceholderImage()
        {
            Bitmap bmp = new Bitmap(150, 150);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 20, FontStyle.Bold))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString("NO IMAGE", font, Brushes.DarkGray,
                        new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
                }
            }
            return bmp;
        }

        // Преобразование изображения в байты
        public static byte[] ImageToByteArray(Image image)
        {
            if (image == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        // Преобразование байтов в изображение
        public static Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return Placeholder;

            try
            {
                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return Placeholder;
            }
        }

        // Сохранение изображения в файл
        public static string SaveImageToFile(Image image, string fileName)
        {
            if (!Directory.Exists(ImagesFolder))
                Directory.CreateDirectory(ImagesFolder);

            string filePath = Path.Combine(ImagesFolder, fileName);
            image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return filePath;
        }

        // Загрузка изображения из файла
        public static Image LoadImageFromFile(string fileName)
        {
            string filePath = Path.Combine(ImagesFolder, fileName);
            if (File.Exists(filePath))
                return Image.FromFile(filePath);

            return Placeholder;
        }

        // Проверка существования папки для изображений
        public static void EnsureImagesFolderExists()
        {
            if (!Directory.Exists(ImagesFolder))
                Directory.CreateDirectory(ImagesFolder);
        }
    }
}