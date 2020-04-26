using System.Collections.Generic;
using System.Drawing;
using OftenColorBotLibrary.KMeans;

namespace OftenColorBotLibrary
{
    public class WorkWithImage
    {
        public static Bitmap GetResult(Bitmap image, UserBot user)
        {
            int amountColor = int.Parse(user.Settings["amount"].ToString());
            WorkKMeans kMeans = new WorkKMeans(10, GetArrayPixelsFromPhoto(image), 0.05);
            Dictionary<Color, int> arrayPixels = kMeans.QuanitityPixels();

            int step = 0;
            Color[] colors = new Color[amountColor];

            foreach (var pixel in arrayPixels)
            {
                colors[step] = pixel.Key;
                ++step;
                if (step == amountColor)
                {
                    break;
                }
            }

            return CreateBitmap(colors, user.Settings["mode"].ToString());
        }

        /// <summary>
        /// Получает весь список пикселей и их количество с изображения, записывая их данные в виде
        /// RGB.
        /// </summary>
        /// <param name="image">Изображение, из которого нужно вытягивать массив пикселей.</param>
        /// <returns>Получаем массив пикселей с их данными в виде RGB.</returns>
        static private double[][] GetArrayPixelsFromPhoto(Bitmap image)
        {
            double[][] arrayPixels = new double[image.Width * image.Height][];

            using (AlternativeBitmap wr = new AlternativeBitmap(image))
            {
                int step = 0;
                for (int i = 0; i < wr.Width; i++)
                {
                    for (int j = 0; j < wr.Height; j++)
                    {
                        Color color = wr[i, j];
                        arrayPixels[step] = new double[] { color.R, color.G, color.B };
                        step++;
                    }
                }
            }

            return arrayPixels;
        }

        /// <summary>
        /// Создает палитру из доминирующих цветов.
        /// </summary>
        /// <param name="colors">Из каких цветов создавать палитру.</param>
        /// <param name="mode">Какой режим у пользователя.</param>
        /// <returns>Возвращает палитру в виде изображения.</returns>
        static private Bitmap CreateBitmap(Color[] colors, string mode)
        {
            Bitmap image = new Bitmap(508, 100 * colors.Length + 4 * (colors.Length + 1));

            using (Graphics flagGraphics = Graphics.FromImage(image))
            {
                flagGraphics.Clear(Color.White);
                int y = 4;
                for (int i = 0; i < colors.Length; i++)
                {
                    flagGraphics.FillRectangle(new SolidBrush(colors[i]), 4, y, 500, 100);

                    if (mode == "artist")
                    {
                        flagGraphics.FillRectangle(Brushes.White, new Rectangle(164, 30 + y, 180, 40));
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = stringFormat.LineAlignment = StringAlignment.Center;
                        flagGraphics.DrawString($"#{colors[i].Name.Substring(2).ToUpper()}",
                            new Font("Arial", 25), Brushes.Black,
                            new Rectangle(164, 30 + y, 180, 40), stringFormat);
                    }
                    y += 104;
                }
            }

            return image;
        }
    }
}
