using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        public static double Sum(this List<double> list)
        {
            double res = 0;

            for (int i = 0; i < list.Count; i++)
            {
                res += list[i];
            }
            return res;
        }

        /*   public static T Sum<T>(this T[] list) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
           {
               for (int i = 0; i < list.Length; i++)
               {
                   res += (T)list[i];
               }
               returnlist.sum ;
           }*/

        public static double Sum(this double[][] list)
        {
            double res = 0;
            for (int i = 0; i <= list.GetUpperBound(0); i++)
            {
                for (int y = 0; y < list[i].Length; y++)
                {
                    res += list[i][y];
                }
            }

            return res;
        }

        public static double SumProduct(this double[] a, double[] b)
        {
            double res = 0;

            for (int i = 0; i < a.Length; i++)
            {
                res += a[i] * b[i];
            }
            return res;
        }

        public static double SumProduct(this double[] a)
        {
            double res = 0;

            for (int i = 0; i < a.Length; i++)
            {
                res += a[i] * a[i];
            }
            return res;
        }

        public static double SumDifference(this double[] a, double[] b)
        {
            double res = 0;

            for (int i = 0; i < a.Length; i++)
            {
                res += Math.Abs(a[i] - b[i]);
            }
            return res;
        }

        public static T Sum<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            Number<T> N = new Number<T>();

            if (typeof(T) == typeof(double) || typeof(T) == typeof(double) || typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                for (int i = 0; i < a.GetUpperBound(0); i++)
                {
                    for (int y = 0; y < a.GetUpperBound(1); y++)
                    {
                        N += a[i][y];
                    }
                }
                return (T)N;
            }
            return default(T);
        }

        public static T Sum<T>(this IEnumerable<T> a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            Number<T> N = new Number<T>();

            if (typeof(T) == typeof(double) || typeof(T) == typeof(double) || typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                for (int i = 0; i < a.Count(); i++)
                {
                    N += a.ElementAt(i);
                }
                return (T)N;
            }
            return default(T);
        }

        public static T[] Sum<T>(this T[][] a, int axis) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            T[] N = new T[a.GetUpperBound(0)];
            Number<T> r;

            if (typeof(T) == typeof(double) || typeof(T) == typeof(double) || typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                for (int i = 0; i < a.GetUpperBound(0); i++)
                {
                    for (int y = 0; y < a.GetUpperBound(1); y++)
                    {
                        r = a[i][y];
                        N[i] = (T)((Number<T>)N[i] + r);
                    }
                }
                return N;
            }
            return null;
        }

        public static int Sum(this IEnumerable<bool> a)
        {
            int sum = 0;
            for (int i = 0; i < a.Count(); i++)
            {
                if (a.ElementAt(i) is true)
                    sum += 1;
            }
            return sum;
        }

        public static double SumAbs(this double[,] m)
        {
            double res = 0;
            for (int i = 0; i < m.GetUpperBound(0); i++)
            {
                for (int l = 0; l < m.GetUpperBound(1); l++)
                {
                    res += Math.Abs(m[i, l]);
                }
            }
            return res;
        }

        public static double SumSQR(this double[] m)
        {
            if (m is null)
                return double.NaN;

            double res = new double();

            for (int i = 0; i < m.Length; i++)
            {
                res += Math.Pow(m[i], 2);
            }
            return res;
        }

        public static double SumABS(this double[] m)
        {
            double res = 0;

            for (int i = 0; i < m.Length; i++)
            {
                res += Math.Abs(m[i]);
            }
            return res;
        }

        public static int Sum(this int[] m)
        {
            int res = 0;

            for (int i = 0; i < m.Length; i++)
            {
                res += m[i];
            }
            return res;
        }

        public static int Sum(this List<int> m)
        {
            int res = 0;

            for (int i = 0; i < m.Count; i++)
            {
                res += m[i];
            }
            return res;
        }

        public static double SumAndDivide(this List<double> data, double div)
        {
            double res;
            res = data.Sum() / div;
            return res;
        }

        public static double SumAndDivide(this double[] data, double div)
        {
            double res;
            res = data.Sum() / div;
            return res;
        }
    }
}