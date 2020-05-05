using System;
using System.Drawing;

namespace OftenColorBotLibrary
{
    internal class CollectionKnowColor
    {
        // Делаем паттерн синглтона.
        private static CollectionKnowColor instance;

        // Вся коллекция всех известных цветов.
        public Color[] KnowColors { get; }

        // Вся коллекция в RGB всех известных цветов.
        public double[][] RGBColors { get; }

        private CollectionKnowColor()
        {
            // Получаем всю коллекцию.
            KnownColor[] knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            
            // Преобразуем в Color.
            KnowColors = new Color[knownColors.Length];
            for (int i = 0; i < knownColors.Length; i++)
            {
                KnowColors[i] = Color.FromKnownColor(knownColors[i]);
            }

            // Преобразуем в RGB коллекцию каждого цвета
            RGBColors = new double[knownColors.Length][];
            for (int i = 0; i < knownColors.Length; i++)
            {
                RGBColors[i] = new double[] { KnowColors[i].R, KnowColors[i].G, KnowColors[i].B };
            }
        }
        
        /// <summary>
        /// Проверяем на то, что класс был ли создан или нет.
        /// </summary>
        /// <returns>Возвращаем созданный класс.</returns>
        public static CollectionKnowColor getInstance()
        {
            if (instance == null)
            {
                instance = new CollectionKnowColor();
            }

            return instance;
        }
    }
}
