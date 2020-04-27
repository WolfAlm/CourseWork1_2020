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

            if (user.Settings["modePalette"].ToString() == "без изображения")
            {
                return CreatePalette(colors, 
                    new Bitmap(508, 100 * colors.Length + 4 * (colors.Length + 1)), 
                    user.Settings["mode"].ToString());
            }
            else
            {
                return CreatePalettePhoto(colors, image, user.Settings["modePalette"].ToString());
            }
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
        /// <param name="image">Изображение, где будет записана палитра.</param>
        /// <param name="mode">Какой режим у пользователя.</param>
        /// <returns>Возвращает палитру в виде изображения.</returns>
        static private Bitmap CreatePalette(Color[] colors, Bitmap image, string mode, 
            int xSize = 500, int ySize = 100)
        {
            using (Graphics flagGraphics = Graphics.FromImage(image))
            {
                flagGraphics.Clear(Color.White);
                int y = 4;
                for (int i = 0; i < colors.Length; i++)
                {
                    // Создаем прямоугольники с определенным цветом.
                    flagGraphics.FillRectangle(new SolidBrush(colors[i]), 4, y, xSize, ySize);

                    if (mode == "профи")
                    {
                        flagGraphics.FillRectangle(Brushes.White, new Rectangle(164, 30 + y, 180, 40));
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = stringFormat.LineAlignment = StringAlignment.Center;
                        flagGraphics.DrawString($"#{colors[i].Name.Substring(2).ToUpper()}",
                            new Font("Arial", 25), Brushes.Black,
                            new Rectangle(164, 30 + y, 180, 40), stringFormat);
                    }
                    y += ySize + 4;
                }
            }

            return image;
        }

        /// <summary>
        /// Создает палитру из доминирующих цветов и пришивает ее к фотографии.
        /// </summary>
        /// <param name="colors">Из каких цветов создавать палитру.</param>
        /// <param name="originalImage">Изображение, с чем будем склеивать палитру.</param>
        /// <param name="mode">Какой режим у пользователя.</param>
        /// <returns>Возвращает исходное изображение с палитрой.</returns>
        static private Bitmap CreatePalettePhoto(Color[] colors, Bitmap originalImage, string mode)
        {
            Bitmap imageResult = new Bitmap(1, 1);
            int sizeForColor = 0;

            switch (mode)
            {
                case "справа":
                    imageResult = new Bitmap(originalImage.Width + 200, originalImage.Height);
                    sizeForColor = (originalImage.Height - 4 * (colors.Length + 1)) / colors.Length;
                    break;
                case "снизу":
                    break;
            }

            Bitmap imagePalette = CreatePalette(colors, new Bitmap(200, originalImage.Height), 
                mode, 192, sizeForColor);

            using (Graphics final = Graphics.FromImage(imageResult))
            {
                final.DrawImage(originalImage, 0, 0);
                final.DrawImage(imagePalette, originalImage.Width, 0);
            }

            return imageResult;
        }
    }
}
