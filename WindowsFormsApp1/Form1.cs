using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            Bitmap image = new Bitmap(@"C:\Users\user\Desktop\file (3).jpg");
            string result = string.Empty;
            Dictionary<Color, ulong> arrayPixels = WorkWithPhoto(image);
            int step = 0;
            Color[] colors = new Color[5];
            foreach (var elem in arrayPixels)
            {
                result += elem.Key + " - " + elem.Key.Name.Substring(2) + " - " + elem.Value + "\n";
                colors[step] = elem.Key;
                step++;
                if (step == 5)
                {
                    break;
                }
            }

            CreateBitmapAtRuntime(colors);
            bool res = colors[0] == Color.DarkGreen;
            richTextBox1.Text = result;
        }

        public void CreateBitmapAtRuntime(Color[] colors)
        {
            Pixels.Size = new Size(508, 100 * colors.Length + 4 * (colors.Length + 1));
            Bitmap flag = new Bitmap(508, 100 * colors.Length + 4 * (colors.Length + 1));

            using (Graphics flagGraphics = Graphics.FromImage(flag))
            {
                flagGraphics.Clear(Color.White);
                int y = 4;
                for (int i = 0; i < colors.Length; i++)
                {
                    flagGraphics.FillRectangle(new SolidBrush(colors[i]), 4, y, 500, 100);
                    flagGraphics.FillRectangle(Brushes.White, new Rectangle(164, 30 + y, 180, 40));
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = stringFormat.LineAlignment = StringAlignment.Center;
                    flagGraphics.DrawString($"#{colors[i].Name.Substring(2).ToUpper()}",
                        new Font("Arial", 25), Brushes.Black,
                        new Rectangle(164, 30 + y, 180, 40), stringFormat);

                    y += 104;
                }
            }

            Pixels.Image = flag;
        }

        public Dictionary<Color, ulong> WorkWithPhoto(Bitmap image)
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
    }
}
