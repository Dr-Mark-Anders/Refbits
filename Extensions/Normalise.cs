using System.Collections.Generic;

namespace Extensions
{
    public static partial class Ext
    {
        public static double[] Normalise(this double[] data)
        {
            double[] res = new double[data.Length];
            double sum = data.Sum();

            if (sum == 0)
                sum = 1;

                for (int i = 0; i < data.Length; i++)
                    res[i] = data[i] / sum;

            return res;
        }

        public static void Normalise(this double[] data, double val)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = data[i] / val;
        }

        public static List<double> Normalise<T>(this List<double> data)
        {
            List<double> res = new List<double>();
            double sum = 0;

            sum = data.Sum();
            int Count = data.Count;

            for (int i = 0; i < Count; i++)
                res[i] = data[i] / sum;

            return res;
        }
    }
}