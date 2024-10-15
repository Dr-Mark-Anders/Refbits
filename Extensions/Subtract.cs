using System;
using System.Collections.Generic;

namespace Extensions
{
    public partial class Ext
    {
        public static T[] Minus<T>(this T[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[] res = new T[a.Length];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (T)(An - Bn);
            }
            return res;
        }

        public static T[][] Minus<T>(this T[][] a, T[][] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[][] res = new T[a.Length][];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.GetUpperBound(0); i++)
            {
                res[i] = new T[a.GetUpperBound(1)];

                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = b[i][y];
                    res[i][y] = (T)(An - Bn);
                }
            }
            return res;
        }

        public static T[][] Minus<T>(this T[][] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[][] res = new T[a.Length][];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.GetUpperBound(0); i++)
            {
                res[i] = new T[a.GetUpperBound(1)];

                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = b[i];
                    res[i][y] = (T)(An - Bn);
                }
            }
            return res;
        }

        public static T[][] Minus<T>(this T[][] a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[][] res = new T[a.Length][];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.GetUpperBound(0); i++)
            {
                res[i] = new T[a.GetUpperBound(1)];

                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = (T)(object)b[i];
                    res[i][y] = (T)(An - Bn);
                }
            }
            return res;
        }

        public static T[][] Minus<T>(this T[][] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[][] res = new T[a.Length][];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.GetUpperBound(0); i++)
            {
                res[i] = new T[a.GetUpperBound(1)];

                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = b;
                    res[i][y] = (T)(An - Bn);
                }
            }
            return res;
        }

        public static T[][] Minus<T>(this T[][] a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[][] res = new T[a.Length][];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.GetUpperBound(0); i++)
            {
                res[i] = new T[a.GetUpperBound(1)];

                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = (T)(object)b;
                    res[i][y] = (T)(An - Bn);
                }
            }
            return res;
        }

        public static double[] Minus<T>(this double[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[] res = new double[a.Length];

            Number<T> Bn;

            for (int i = 0; i < a.Length; i++)
            {
                Bn = b[i];
                res[i] = (a[i] - Bn);
            }
            return res;
        }

        public static double[] Minus<T>(this double[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[] res = new double[a.Length];

            Number<T> An = b;

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = An - a[i];
            }
            return res;
        }

        public static double[] Minus(this double[] a, double[] b)
        {
            double[] res = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] - b[i];
            }
            return res;
        }

        public static List<double> Minus(List<double> thelist, List<double> thelist2)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < thelist.Count; i++)
            {
                res.Add(thelist[i] - thelist2[i]);
            }
            return res;
        }

        public static double[] Minus<T>(this int a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[] res = new double[b.Length];

            Number<T> Bn;

            for (int i = 0; i < b.Length; i++)
            {
                Bn = b[i];
                res[i] = a - Bn;
            }
            return res;
        }

        public static double[] Minus(this double a, double[] b)
        {
            double[] res = new double[b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = a - b[i];
            }
            return res;
        }
    }
}