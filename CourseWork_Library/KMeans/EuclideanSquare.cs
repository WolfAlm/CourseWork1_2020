namespace OftenColorBotLibrary.KMeans
{
    class EuclideanSquare
    {
        /// <summary>
        /// Мы создаем евклидовую метрику на основе данных кластеров и tolerance
        /// </summary>
        /// <param name="clusters">Коллекция кластеров.</param>
        /// <param name="tolerance">На каком моменте нужно остановить вычисления.</param>
        public EuclideanSquare(ClusterCollection clusters, double tolerance = 0.00001)
        {
            Tolerance = tolerance;
            Clusters = clusters;
        }

        /// <summary>
        /// Коллекция кластеров.
        /// </summary>
        private ClusterCollection Clusters { get; set; }

        /// <summary>
        /// На какой разнице между центроидами кластеров нужно остановить вычисления 
        /// новых центроидов кластеров.
        /// </summary>
        internal double Tolerance { get; }

        /// <summary>
        /// Мы вычисляем расстояние между точкой(RGB) данного пикселя и центроидом кластера.
        /// </summary>
        /// <param name="pixel">Точка пикселя.</param>
        /// <param name="centroid">Центроид кластера.</param>
        /// <returns>Возвращает расстояние между ними.</returns>
        public double Distance(double[] pixel, double[] centroid)
        {
            double sum = 0.0;

            for (int i = 0; i < pixel.Length; i++)
            {
                double u = pixel[i] - centroid[i];
                sum += u * u;
            }

            return sum;
        }

        /// <summary>
        /// Подбирает к точке самый ближайший кластер.
        /// </summary>
        /// <param name="pixel">Точка(пиксель), к которой подбирается наиближайший кластер.</param>
        /// <returns>Возвращает индекс кластера.</returns>
        public int PickClusterForPoint(double[] pixel)
        {
            // Для начала вычисляется первый минимум.
            double min = Distance(pixel, Clusters.Centroids[0]);
            int indexMin = 0;
            // После чего вычисялем последующие расстояния от новых кластеров.
            for (int i = 1; i < Clusters.K; i++)
            {
                double newMin = Distance(pixel, Clusters.Centroids[i]);

                if (newMin < min)
                {
                    min = newMin;
                    indexMin = i;
                }
            }
            // Возвращаем индекс кластера, где было самое минимальное расстояние для пикселя.
            return indexMin;
        }


        //public int[] PickClusterForPoints(double[][] pixels)
        //{
        //    int[] result = new int[pixels.Length];

        //    for (int i = 0; i < pixels.Length; i++)
        //    {
        //        result[i] = PickClusterForPoint(pixels[i]);
        //    }

        //    return result;
        //}
    }
}