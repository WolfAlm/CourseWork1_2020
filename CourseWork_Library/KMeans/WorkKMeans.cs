using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace OftenColorBotLibrary.KMeans
{
    public class WorkKMeans
    {
        static Random random = new Random();

        /// <summary>
        /// База пикселей, на которой нужно вычислить центры кластеров и сопоставить их с этой базой.
        /// </summary>
        public double[][] Pixels { get; private set; }

        /// <summary>
        /// Расстояние Евклида.
        /// </summary>
        private EuclideanSquare Euclidian { get; }

        /// <summary>
        /// Коллекция кластеров.
        /// </summary>
        private ClusterCollection Clusters { get; }

        /// <summary>
        /// База меток, которая обозначает, какой кластер нужен для каждого
        /// пикселя.
        /// </summary>
        public int[] Labels { get; private set; }

        /// <summary>
        /// Конструктор создает все нужные объекты и запускает работу для k means алгоритма. После
        /// чего получает весь результат от этого алгоритма и проецирует каждый кластер на каждую
        /// точку.
        /// </summary>
        /// <param name="k">На сколько кластеров нужно разделить.</param>
        /// <param name="pixels">На основе какой базе пикселей нужно обучать.</param>
        /// <param name="tolerance">Насколько сильная должна быть разница между итерациями генерации
        /// кластеров для остановки алгоритма.</param>
        public WorkKMeans(int k, double[][] pixels, double tolerance = 0.00001)
        {
            Pixels = pixels;
            Clusters = new ClusterCollection(k);
            Euclidian = new EuclideanSquare(Clusters, tolerance);

            Labels = KMeansAlgorithm();
        }

        /// <summary>
        /// Считаем количество пикселей и исходя из количества упорядочиваем их по убыванию.
        /// </summary>
        /// <returns>Вовзращает список цветов с их количеством.</returns>
        public Dictionary<Color, int> QuanitityPixels()
        {
            Dictionary<Color, int> quanitityPixels = new Dictionary<Color, int>();

            for (int i = 0; i < Labels.Length; i++)
            {
                Color color = Color.FromArgb((int)Clusters.Centroids[Labels[i]][0], 
                    (int)Clusters.Centroids[Labels[i]][1], (int)Clusters.Centroids[Labels[i]][2]);

                if (quanitityPixels.ContainsKey(color))
                {
                    quanitityPixels[color]++;
                }
                else
                {
                    quanitityPixels.Add(color, 1);
                }
            }

            return quanitityPixels.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Сначала мы приводим в необходимый вид центры кластеров в представление вида их 
        /// координатов от 0 до 255, чтобы не было дробных выражений, так как иначе Color не будет 
        /// считывать нормально.
        /// </summary>
        private void NecessaryView()
        {
            // Приводим координаты центроида кластеров в необходимый вид.
            for (int i = 0; i < Clusters.K; i++)
            {
                for (int j = 0; j < Clusters.Centroids[i].Length; j++)
                {
                    Clusters.Centroids[i][j] = (int)Clusters.Centroids[i][j];
                }
            }
        }

        /// <summary>
        /// Проецирует для каждого пикселя самый ближайший кластер, который был
        /// уже вычислен в основном методе KMeans.
        /// </summary>
        /// <returns>Возвращает массив пикселей с кластерами.</returns>
        public double[][] ProjectionsToPixels()
        {
            // Присваиваем каждому пикселю ближайший кластер.
            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i] = Clusters.Centroids[Labels[i]];
            }

            return Pixels;
        }

        /// <summary>
        /// Реализация KMeans алгоритма. Он разделяет входные данные на несколько кластеров. Для
        /// начала мы инициализируем начальные кластеры.
        /// </summary>
        /// <returns>Возвращает массив, где будут индексы кластеров для каждой точки.</returns>
        private int[] KMeansAlgorithm()
        {
            // Инициализация K-Means++.
            DoKMeansPlusPlusSeeding();

            int rows = Pixels.Length;
            int k = Clusters.K;

            var count = new double[k]; // какая-то параша
            double[] weights = new double[rows]; // какая то параша
            for (int i = 0; i < rows; i++)
            {
                weights[i] = 1;
            }

            // Создаем базу меток для каждого пикселя, чтобы присваивать номера ближайших кластеров.
            int[] labels = new int[rows];

            // Для упрощения писанины, мы присвоили этому массиву центроиды.
            double[][] centroids = Clusters.Centroids;
            // Создаем полностью новые центроиды с нулевыми значениями.
            double[][] newCentroids = new double[k][];
            for (int i = 0; i < k; i++)
            {
                newCentroids[i] = new double[3];
            }
            // Флажок для того, чтобы знать, нужно ли останавливать инициализацию или нет.
            bool flagShouldStop = false;

            object[] syncObjects = new object[k];
            for (int i = 0; i < syncObjects.Length; i++)
            {
                syncObjects[i] = new object();
            }

            while (!flagShouldStop)
            {
                // Перед новой итерацией мы очищаем все счетчики и массив новых кластеров.
                Array.Clear(count, 0, count.Length); // параша какая-то
                for (int i = 0; i < newCentroids.Length; i++)
                {
                    Array.Clear(newCentroids[i], 0, newCentroids[i].Length);
                }

                // Сначала мы будем накапливать точки в ближайших кластерах, сохраняя эту информацию
                // в новых кластерах.
                
                // Цикл для каждого пикселя.
                Parallel.For(0, rows, i =>
                {
                    // Получаем пиксель.
                    double[] pixel = Pixels[i];
                    // Ее весовую категорию.
                    double weight = weights[i]; // какая-то параша тоже

                    // Получаем ближайший кластер для этой точки.
                    int c = labels[i] = Euclidian.PickClusterForPoint(pixel);

                    // Теперь получаем центр этого ближайшего кластера.
                    double[] centroid = newCentroids[c];

                    lock (syncObjects[c])
                    {
                        // Increase the cluster's sample counter
                        count[c] += weight; // точно такая же параша

                        // Накапливаем в центрах кластеров.
                        for (int j = 0; j < pixel.Length; j++)
                        {
                            centroid[j] += pixel[j];
                        }
                    }
                });

                // Next we will compute each cluster's new centroid
                //  by dividing the accumulated sums by the number of
                //  samples in each cluster, thus averaging its members.
                Parallel.For(0, k, i =>
                {
                    double sum = count[i];

                    if (sum > 0)
                    {
                        for (int j = 0; j < newCentroids[i].Length; j++)
                            newCentroids[i][j] /= sum;
                    }
                });

                // Алгоритм остановится, когда больше не будут существенных изменений
                // в центроидах и это регулируется с помощью параметра tolerance.
                flagShouldStop = AlgorithmConverged(centroids, newCentroids);

                // Присваиваем центроидам координаты новых центроидов.
                Parallel.For(0, k, i =>
                {
                    for (int j = 0; j < centroids[i].Length; j++)
                    {
                        centroids[i][j] = newCentroids[i][j];
                    }
                });
            }

            return labels;
        }

        /// <summary>
        ///   Определяет, сошелся ли алгоритм путем сравнения центроидов между двумя
        ///   последовательными итерациями.
        /// </summary>
        /// <param name="centroids">Центроид кластера.</param>
        /// <param name="newCentroids">Новый центроид кластера.</param>
        /// <returns>Возвращает <see langword="true"/> если все кластеры имеют меньше, чем
        /// <see param="threshold"/>. Иначе возвращает <see langword="false"/>.</returns>
        private bool AlgorithmConverged(double[][] centroids, double[][] newCentroids)
        {
            for (int i = 0; i < centroids.Length; i++)
            {
                double[] centroid = centroids[i];
                double[] newCentroid = newCentroids[i];

                for (int j = 0; j < centroid.Length; j++)
                {
                    if (Math.Abs((centroid[j] - newCentroid[j]) / centroid[j]) >= Euclidian.Tolerance)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Расширенный алгоритм k-means++, который позволяет выбрать для старта самые оптимиальные
        /// центры кластера, между которыми большая разница в расстоянии, что позволяет со всех угол
        /// подходить. Это всяко лучше, чем выбрать 10 случайных кластеров, т.к. это уменьшает
        /// количество итераций.
        /// </summary>
        private void DoKMeansPlusPlusSeeding()
        {
            // Создаем индексы будущих выбранных кластеров.
            List<int> index = new List<int>();

            // Выбираем случайно любую точек из данных нам точек для первого кластера.
            int idx = random.Next(0, Pixels.Length);
            index.Add(idx);
            Clusters.Centroids[0] = (double[])Pixels[idx].Clone();

            // Сколько кластеров должно быть создано.
            for (int c = 1; c < Clusters.K; c++)
            {
                double sum = 0;
                var D = new double[Pixels.Length];
                for (int i = 0; i < D.Length; i++)
                {
                    // Для каждой точки мы считаем расстояние между точкой и центрами кластеров.
                    var x = Pixels[i];
                    double min = Euclidian.Distance(x, Clusters.Centroids[0]);
                    // При наличии более одного кластера.
                    for (int j = 1; j < c; j++)
                    {
                        double d = Euclidian.Distance(x, Clusters.Centroids[j]);
                        // Если есть расстояние между точкой и центром меньшее, то присваиваем его.
                        if (d < min)
                        {
                            min = d;
                        }
                    }
                    // Для каждой точки мы присваиваем самые минимальные расстояния между ней и центром.
                    D[i] = min;
                    // Также суммируем все расстояния.
                    sum += min;
                }

                if (sum == 0)
                {
                    // Исключительный случай: все точки одинаковые.
                    idx = random.Next(0, Pixels.Length);
                }
                else
                {
                    // Мы считаем вероятности для каждого растояния. Чем выше вероятность -- тем
                    // больше шанса быть выбранным.
                    for (int i = 0; i < D.Length; i++)
                    {
                        D[i] /= sum;
                    }

                    // В качестве ограничения используем рандом.
                    double uniform = random.NextDouble();
                    double cumulativeSum = 0;
                    // Находим почти самый дальний кластер.
                    for (int i = 0; i < D.Length; i++)
                    {
                        cumulativeSum += D[i];
                        if (uniform < cumulativeSum && !index.Contains(i))
                        {
                            idx = i;
                            break;
                        }
                    }
                }

                // Присваиваем по индексу найденный кластер в массив. 
                Clusters.Centroids[c] = (double[])Pixels[idx].Clone();
                // Добавляем его индекс, чтобы не выбирать одного и того же.
                index.Add(idx);
            }
        }
    }
}