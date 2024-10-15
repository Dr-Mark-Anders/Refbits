using System;
using System.Collections.Generic;

namespace Extensions
{
    public partial class Ext
    {
        public static T[] Divide<T>(this T[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
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

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (T)(An / b);
            }
            return res;
        }

        public static T[] Divide<T>(this T[] a, int b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
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

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (T)(object)(An / b);
            }
            return res;
        }

        public static T[][] Divide<T>(this T[][] a, T[][] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
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

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new T[b.Length];
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i][y];
                    Bn = b[i][y];

                    res[i][y] = (T)(An / Bn);
                }
            }
            return res;
        }

        public static T[][] Divide<T>(this T[][] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
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

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new T[b.Length];

                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i][y];
                    Bn = b[y];
                    res[i][y] = (T)(An / Bn);
                }
            }
            return res;
        }

        public static T[] Divide<T>(this T[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            T[] res = new T[a.Length];

            Number<T> An, Bn;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (T)(An / Bn);
            }
            return res;
        }

        public static double[] Divide<T>(this double[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[] res = new double[a.Length];

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = b[i];
                res[i] = (a[i] / An);
            }
            return res;
        }

        public static double[] Divide<T>(this double[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
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
                res[i] = (a[i] / An);
            }
            return res;
        }

        public static double[][] Divide(this double[][] a, double x)
        {
            double[][] res = new double[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new double[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    res[i][j] = a[i][j] / x;
                }
            }
            return res;
        }

        public static double[][] Divide<T>(this T[][] a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[][] res = new double[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new double[b.Length];
            }

            Number<T> An;
            double Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i][y];
                    Bn = b[y];
                    res[i][y] = (An / Bn);
                }
            }
            return res;
        }

        public static double[][] Divide(this double[][] a, double[] b)
        {
            double[][] res = new double[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new double[b.Length];
            }

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    res[i][y] = (a[i][y] / b[y]);
                }
            }
            return res;
        }

        public static double[][] Divide<T>(this T[] a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[][] res = new double[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new double[b.Length];
            }

            Number<T> An;
            double Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i][y] = (An / Bn);
                }
            }
            return res;
        }

        public static double[] Divide(this double[] a, double x)
        {
            double[] res = new double[a.Length];

            if (x == 0)
                return null;

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] / x;
            }
            return res;
        }

        public static double[] Divide(this double[] a, double[] x)
        {
            double[] res = new double[a.Length];

            if (x is null)
                return null;

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] / x[i];
            }
            return res;
        }

        public static double[] Divide<T>(this T[] a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[] res = new double[a.Length];

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (An / b);
            }
            return res;
        }

        public static double[][] Divide<T>(this List<T> a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[][] res = new double[a.Count][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new double[b.Length];
            }

            Number<T> An;
            double Bn;

            for (int i = 0; i < a.Count; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i][y] = (An / Bn);
                }
            }
            return res;
        }

        public static double[] Divide<T>(this List<T> a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            double[] res = new double[a.Count];

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (An / b);
            }
            return res;
        }

        public static int[][] Divide<T>(this List<T> a, int[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int[][] res = new int[a.Count][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new int[b.Length];
            }

            Number<T> An;
            int Bn;

            for (int i = 0; i < a.Count; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i][y] = (An / Bn);
                }
            }
            return res;
        }

        public static int[] Divide<T>(this List<T> a, int b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int[] res = new int[a.Count];

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (An / b);
            }
            return res;
        }

        public static List<double> Divide(this List<double> data, double div)
        {
            List<double> res = new List<double>(new double[data.Count]);

            for (int i = 0; i < data.Count; i++)
            {
                res[i] = data[i] / div;
            }

            return res;
        }
    }
}