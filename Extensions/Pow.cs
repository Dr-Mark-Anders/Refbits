using System;

namespace Extensions
{
    public partial class Ext
    {
        public static double Pow(this double d, double Power)
        {
            return Math.Pow(d, Power); ;
        }

        public static double[] Pow(this double[] a, double p)
        {
            double[] res = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = Math.Pow(a[i], (double)p);
            }
            return res;
        }

        public static double[][] Pow(this double[][] a, double p)
        {
            double[][] res = new double[a.GetUpperBound(0)][];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new double[a.GetUpperBound(1)];
                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    res[i][y] = Math.Pow(a[i][y], (double)p);
                }
            }
            return res;
        }
    }
}