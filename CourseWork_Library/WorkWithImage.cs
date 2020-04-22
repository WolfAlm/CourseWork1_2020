using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace CourseWork_Library
{
    public class WorkWithImage
    {
        public static Bitmap GetResult(Bitmap image, UserBot user)
        {
            int amountColor = int.Parse(user.Settings["amount"].ToString());
            Dictionary<Color, ulong> arrayPixels = GetArrayPixels(image);

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
        /// Получает весь список пикселей и их количество с изображения.
        /// </summary>
        /// <param name="image">Изображение, из которого нужно вытягивать массив пикселей.</param>
        /// <returns>Возвращает результат работы.</returns>
        static private Dictionary<Color, ulong> GetArrayPixels(Bitmap image)
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
