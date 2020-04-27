﻿using System;
using System.Collections.Generic;
using System.Drawing;
using OftenColorBotLibrary.KMeans;

namespace OftenColorBotLibrary
{
    public class WorkWithImage
    {
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
            using (Graphics flagGraphics = Graphics.FromImage(image))
            {
                // Заливаем картинку белым цветом.
                flagGraphics.Clear(Color.White);
                // Задаем начало координат.
                int y = 4;
                // По очереди создаем прямоугольники с определенным цветом.
                for (int i = 0; i < colors.Length; i++)
                {
                    flagGraphics.FillRectangle(new SolidBrush(colors[i]), 4, y, xSize, ySize);

                    // TODO: Сделать для разной палитры.
                    // Записывает текст на палитру.
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
