using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CourseWork_Library
{
    /// <summary>
    /// Этот класс был создан для замены прямой работы с Bitmap.GetPixels(), так как была
    /// довольно долгая обработка огромных изображений, а этот класс уменьшал время обработки
    /// в 5-10 раз.
    /// </summary>
    public class AlternativeBitmap : IDisposable
    {
        /// <summary>
        /// Ширина изображения.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Высота изображения.
        /// </summary>
        public int Height { get; private set; }

        // Буфер исходного изображения.
        private byte[] data;
        // Количество байт в одной строке.
        private int stride;
        private BitmapData imageData;
        private Bitmap image;

        /// <summary>
        /// Сохраняем все необходимые данные изображения для работы с ним.
        /// </summary>
        /// <param name="image">Изображение, откуда нужно вытягивать данные.</param>
        public AlternativeBitmap(Bitmap image)
        {
            Width = image.Width;
            Height = image.Height;
            this.image = image;
            imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            stride = imageData.Stride;

            data = new byte[stride * Height];
            System.Runtime.InteropServices.Marshal.Copy(imageData.Scan0, data, 0, data.Length);
        }

        /// <summary>
        /// Возвращает пиксел из исходнго изображения.
        /// Либо заносит пиксел в выходной буфер.
        /// </summary>
        public Color this[int x, int y]
        {
            get
            {
                long i = GetIndex(x, y);
                // В одном байте заключены 4 бита ARGB, а именно: 
                // i+3 - A, i+2 - R, i+1 - G, i - B.
                return Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
            }
        }

        /// <summary>
        /// Получает индекс пикселя в массиве байтов.
        /// </summary>
        /// <param name="x">Координаты по ширине.</param>
        /// <param name="y">Координаты по высоте.</param>
        /// <returns>Возвращает точку пикселя.</returns>
        long GetIndex(int x, int y)
        {
            return x * 4 + y * stride;
        }

        /// <summary>
        /// Заносит в bitmap выходной буфер и снимает лок.
        /// Этот метод обязателен к исполнению (либо явно, лмбо через using)
        /// </summary>
        public void Dispose()
        {
            image.UnlockBits(imageData);
        }
    }
}
