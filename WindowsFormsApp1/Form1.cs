using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Dictionary<Color, ulong> arrayPixels = WorkWithPhoto(new Bitmap(@"C:\Users\user\Desktop\file (3)DSC01279.jpg"));
            string result = string.Empty;
            Bitmap image = new Bitmap(@"C:\Users\user\Desktop\Google_Videos_logo.png");
            Dictionary<Color, ulong> arrayPixels = WorkWithPhotoFast(image);
            int step = 0;
            Color[] colors = new Color[10];
            foreach (var elem in arrayPixels)
            {
                result += elem.Key + " - " + elem.Key.Name.Substring(2) + " - " + elem.Value + "\n";
                colors[step] = elem.Key;
                step++;
                if (step == 10)
                {
                    break;
                }
            }

            CreateBitmapAtRuntime(colors);
            richTextBox1.Text = result;
        }

        public void CreateBitmapAtRuntime(Color[] colors)
        {
            Pixels.Size = new Size(204, 50 * colors.Length + 2 * (colors.Length + 1));
            Bitmap flag = new Bitmap(204, 50 * colors.Length + 2 * (colors.Length + 1));

            using (Graphics flagGraphics = Graphics.FromImage(flag))
            {
                flagGraphics.Clear(Color.White);
                int y = 2;
                for (int i = 0; i < colors.Length; i++)
                {
                    flagGraphics.FillRectangle(new SolidBrush(colors[i]), 2, y, 250, 50);
                    flagGraphics.FillRectangle(Brushes.White, new Rectangle(82, 15 + y, 90, 20));
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = stringFormat.LineAlignment = StringAlignment.Center;
                    flagGraphics.DrawString($"#{colors[i].Name.Substring(2).ToUpper()}",
                        new Font("Arial", 16), Brushes.Black,
                        new Rectangle(82, 15 + y, 90, 20), stringFormat);

                    y += 52;
                }
            }

            Pixels.Image = flag;
        }

        public Dictionary<Color, ulong> WorkWithPhotoFast(Bitmap image)
        {
            Dictionary<Color, ulong> arrayPixels = new Dictionary<Color, ulong>();

            using (AlternativeBitmap wr = new AlternativeBitmap(image))
            {
                for (int i = 0; i < wr.Width; i++)
                {
                    for (int j = 0; j < wr.Height; j++)
                    {
                        if (arrayPixels.ContainsKey(wr[i, j]))
                        {
                            arrayPixels[wr[i, j]]++;
                        }
                        else
                        {
                            arrayPixels.Add(wr[i, j], 1);
                        }
                    }
                }
            }

            return arrayPixels.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

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
}
