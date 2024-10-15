using System.Collections.Generic;

namespace Extensions
{
    public static partial class Ext
    {
        public static double[] Max(double[] x, double highBound)
        {
            double[] res = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] > highBound)
                    res[i] = (double)highBound;
                else
                    res[i] = x[i];
            }
            return res;
        }

        public static double[] Min(double[] x, double lowBound)
        {
            double[] res = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] < lowBound)
                    res[i] = (double)lowBound;
                else
                    res[i] = x[i];
            }
            return res;
        }

        public static double[] Max(double[] a, double[] b)
        {
            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > b[i])
                    res[i] = a[i];
                else
                    res[i] = b[i];
            }
            return res;
        }

        public static double[] Max(List<int> c, double v)
        {
            double[] res = new double[c.Count];
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i] > v)
                    res[i] = c[i];
                else
                    res[i] = v;
            };
            return res;
        }

        public static double[] Max(List<double> c, double v)
        {
            double[] res = new double[c.Count];
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i] > v)
                    res[i] = c[i];
                else
                    res[i] = v;
            };
            return res;
        }

        public static double Max(this double[] m)
        {
            double res = 0;
            res = m[0];

            for (int i = 0; i < m.Length; i++)
            {
                if (m[i] > res)
                    res = m[i];
            }
            return res;
        }

        public static double Min(this double[] m)
        {
            double res = 0;
            res = m[0];

            for (int i = 0; i < m.Length; i++)
            {
                if (m[i] < res)
                    res = m[i];
            }
            return res;
        }

        public static object apply(object classObj, List<object> args)
        {
            return classObj;
        }

        public static IEnumerable<T> Combine<T>(Dictionary<string, T>.ValueCollection values1, Dictionary<string, T>.ValueCollection values2)
        {
            List<T> temp = new List<T>();
            temp.AddRange(values1);
            temp.AddRange(values2);
            return temp;
        }
    }
}