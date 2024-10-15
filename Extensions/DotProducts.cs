namespace Extensions
{
    public static partial class Ext
    {
        /// <summary>
        /// Dot products
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double[] dot(double[][] a, double[][] b)
        {
            double[] res = new double[a.GetUpperBound(0)];

            for (int i = 0; i < a.GetUpperBound(0); i++)
            {
                for (int j = 0; j < a.GetUpperBound(1); j++)
                    res[i] += a[i][j] * b[j][i];
            }
            return res;
        }

        public static double dot(double[] a, double[] b)
        {
            double res = 0;
            for (int i = 0; i < a.Length; i++)
            {
                res += a[i] * b[i];
            };
            return res;
        }

        public static double[] dot(double[] a, double[][] b)
        {
            double[] res = new double[a.Length];
            for (int x = 0; x < a.Length; x++)
            {
                for (int y = 0; y < b.GetUpperBound(1); y++)
                {
                    res[x] += a[x] * b[x][y];
                }
            }
            return res;
        }

        public static double[] dot(double[][] a, double[] b)
        {
            double[] res = new double[a.Length];
            for (int x = 0; x < a.Length; x++)
                for (int y = 0; y < b.Length; y++)
                    res[x] += b[x] * a[x][y];

            return res;
        }
    }
}