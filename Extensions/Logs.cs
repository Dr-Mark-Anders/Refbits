using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public partial class Ext
    {
        internal static double Log(double v)
        {
            return Math.Log((double)v);
        }

        public static double[] Log(this double[] p)
        {
            double[] res = new double[p.Length];

            for (int i = 0; i < p.Length; i++)
            {
                res[i] = Math.Log((double)p[i]);
            }
            return res;
        }

        public static double[][] Log(this double[][] p)
        {
            double[][] res = new double[p.GetUpperBound(0)][];

            for (int i = 0; i < p.Length; i++)
            {
                res[i] = new double[p.GetUpperBound(1)];
                for (int y = 0; y < p.GetUpperBound(1); y++)
                {
                    res[i][y] = Math.Log(p[i][y]);
                }
            }
            return res;
        }

        public static double[] Log<T>(this IEnumerable<T> p) where T : struct, IComparable<T>, IEquatable<T>
        {
            double[] res = new double[p.Count()];

            Number<T> N;

            for (int i = 0; i < p.Count(); i++)
            {
                N = p.ElementAt(i);
                res[i] = Math.Log(N);
            }
            return res;
        }
    }
}