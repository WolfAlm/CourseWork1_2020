using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using OftenColorBotLibrary.KMeans;

namespace OftenColorBotLibrary
{
    public class CollectionKnowColor
    {
        private static CollectionKnowColor instance;

        public Color[] KnowColors { get; }

        public double[][] RGBColors { get; }

        private CollectionKnowColor()
        {
            KnownColor[] knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));

            KnowColors = new Color[knownColors.Length];
            for (int i = 0; i < knownColors.Length; i++)
            {
                KnowColors[i] = Color.FromKnownColor(knownColors[i]);
            }

            RGBColors = new double[knownColors.Length][];
            for (int i = 0; i < knownColors.Length; i++)
            {
                RGBColors[i] = new double[] { KnowColors[i].R, KnowColors[i].G, KnowColors[i].B };
            }
        }

        public static CollectionKnowColor getInstance()
        {
            if (instance == null)
                instance = new CollectionKnowColor();
            return instance;
        }
    }

    public class WorkWithImage
    {
        static EuclideanSquare euclideanSquare = new EuclideanSquare();

        /// <summary>
        /// Получает изображение и анализирует его на доминирующие цвета, после чего, исходя из
        /// параметров пользователя, будет сформировано изображение под требования пользователя.
        /// </summary>
        /// <param name="image">Изображение, которое нужно анализировать.</param>
        /// <param name="user">Чьи параметры и настройки будем принимать.</param>
        /// <returns>Возвращает палитру с заданными параметрами пользователя.</returns>
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
                return CreatePalettePhoto(colors, image, user.Settings["modePalette"].ToString(),
                    user.Settings["mode"].ToString());
            }
        }

        /// <summary>
        /// Получает весь список пикселей с изображения, записывая их данные в виде RGB.
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
            using (Graphics imageGraphics = Graphics.FromImage(image))
            {
                imageGraphics.SmoothingMode = SmoothingMode.HighQuality;
                imageGraphics.InterpolationMode = InterpolationMode.High;
                imageGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                imageGraphics.CompositingQuality = CompositingQuality.HighQuality;
                // Заливаем картинку белым цветом.
                imageGraphics.Clear(Color.White);
                // Задаем начало координат.
                int y = 4;
                
                for (int i = 0; i < colors.Length; i++)
                {
                    // По очереди создаем прямоугольники с определенным цветом.
                    imageGraphics.FillRectangle(new SolidBrush(colors[i]), 4, y, xSize, ySize);

                    // Записывает текст на палитру.
                    if (mode == "профи" || mode == "дальтоник")
                    {
                        // Высчитаем положение фигуры для помещения на центр.
                        int xText = xSize / 2;
                        int yText = ySize / 2;

                        // Задаем форматирование текста.
                        StringFormat stringFormat = new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        // Считаем размер шрифта.
                        float sizeFont = image.Height > 80 ? 30f : (float)(image.Height / 2 * 0.75);
                        // Задаем шрифт.
                        Font font = new Font("Arial Rounded MT", sizeFont, FontStyle.Bold);
                        string info = mode == "профи" 
                            ? $"#{colors[i].Name.Substring(2).ToUpper()}" 
                            : GetNormalName(colors[i]);

                        // Обрезываем канал A с помощью SubString. И рисуем текст.
                        imageGraphics.DrawString(info,
                            font, Brushes.White, new Point(xText, yText + y), stringFormat);

                        // Создаем текст для того, чтобы можно было обвести его.
                        GraphicsPath penGraphics = new GraphicsPath();
                        penGraphics.AddString(info,
                            font.FontFamily, (int)FontStyle.Bold, imageGraphics.DpiY * font.Size / 72,
                            new Point(xText, yText + y), stringFormat);
                        // Обводим ее черной ручкой.
                        Pen pen = new Pen(Brushes.Black, 2f) { LineJoin = LineJoin.Round };
                        imageGraphics.DrawPath(pen, penGraphics);
                    }

                    y += ySize + 4;
                }
            }

            return image;
        }

        static private string GetNormalName(Color color)
        {
            CollectionKnowColor collectionKnowColor = CollectionKnowColor.getInstance();
            int index = euclideanSquare.PickClusterForPoint(new double[] { color.R, color.G, color.B }, 
                collectionKnowColor.RGBColors);

            return collectionKnowColor.KnowColors[index].Name;
        }

        /// <summary>
        /// Создает палитру из доминирующих цветов и пришивает ее к фотографии.
        /// </summary>
        /// <param name="colors">Из каких цветов создавать палитру.</param>
        /// <param name="imageOriginal">Изображение, с чем будем склеивать палитру.</param>
        /// <param name="modePalette">Какое расположение палитры у пользователя.</param>
        /// <param name="mode">Какой режим у пользователя.</param>
        /// <returns>Возвращает исходное изображение с палитрой.</returns>
        static private Bitmap CreatePalettePhoto(Color[] colors, Bitmap imageOriginal, string modePalette,
            string mode)
        {
            // Здесь будем сохранять два объединенные изображения: палитра и оригинал
            Bitmap imageResult = new Bitmap(1, 1);
            // Создаем палитру.
            Bitmap imagePalette = new Bitmap(1, 1);

            // Здесь задаем координаты начала.
            int startXoriginal = 0, startXpalette = 0, startYoriginal = 0,
                startYpalette = 0;
            // По режиму пользователя будет определяться, с какой стороны записывать палитру.
            switch (modePalette)
            {
                case "справа":
                case "слева":
                    imageResult = new Bitmap(imageOriginal.Width + 200, imageOriginal.Height);

                    if (modePalette == "справа")
                    {
                        startXpalette = imageOriginal.Width;
                    }
                    else
                    {
                        startXoriginal = 200;
                    }
                    
                    imagePalette = CreatePalette(colors, new Bitmap(200, imageOriginal.Height),
                            mode, 192, (imageOriginal.Height - 4 * (colors.Length + 1)) / colors.Length);
                    break;
                case "сверху":
                case "cнизу":
                    imageResult = new Bitmap(imageOriginal.Width, imageOriginal.Height + 200);

                    if (modePalette == "сверху")
                    {
                        startYoriginal = 200;
                    }
                    else
                    {
                        startYpalette = imageOriginal.Height;
                    }

                    imagePalette = CreatePalette(colors, new Bitmap(200, imageOriginal.Width),
                            mode, 192, (imageOriginal.Width - 4 * (colors.Length + 1)) / colors.Length);
                    imagePalette.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }

            // Объединяем изображения.
            using (Graphics final = Graphics.FromImage(imageResult))
            {
                final.DrawImage(imageOriginal, startXoriginal, startYoriginal, 
                    imageOriginal.Width, imageOriginal.Height);
                final.DrawImage(imagePalette, startXpalette, startYpalette, imagePalette.Width,
                    imagePalette.Height);
            }

            return imageResult;
        }
    }
}
