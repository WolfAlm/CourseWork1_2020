using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        static private Dictionary<Color, ulong> GetArrayPixels(Bitmap image)
        {
            Dictionary<Color, ulong> arrayPixels = new Dictionary<Color, ulong>();

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (arrayPixels.ContainsKey(image.GetPixel(i, j)))
                    {
                        arrayPixels[image.GetPixel(i, j)]++;
                    }
                    else
                    {
                        arrayPixels.Add(image.GetPixel(i, j), 1);
                    }
                }
            }

            return arrayPixels.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

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
