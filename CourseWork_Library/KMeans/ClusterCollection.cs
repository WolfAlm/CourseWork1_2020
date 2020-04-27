namespace OftenColorBotLibrary.KMeans
{
    /// <summary>
    /// Коллекция кластеров с центроидами.
    /// </summary>
    class ClusterCollection
    {
        /// <summary>
        /// Получаем количество кластеров и создаем центроиды.
        /// </summary>
        /// <param name="k">Количество кластеров.</param>
        public ClusterCollection(int k)
        {
            Centroids = new double[k][];
            K = k;
        }

        /// <summary>
        /// Центроиды.
        /// </summary>
        public double[][] Centroids { get; private set; }

        /// <summary>
        /// Количество кластеров.
        /// </summary>
        public int K { get; }
    }
}