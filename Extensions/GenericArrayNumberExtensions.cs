using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    /// <summary>
    /// Basic T Methods
    /// </summary>
    public partial class Ext
    {
        public static IEnumerable<T> Minus<T>(this IEnumerable<T> a, IEnumerable<T> b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            T[] res = new T[a.Count()];

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.Count(); i++)
            {
                An = a.ElementAt(i);
                Bn = b.ElementAt(i);
                res[i] = (T)(An - Bn);
            }
            return res;
        }

        public static T[] Minus<T>(this T[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (T)(An - b);
            }
            return res;
        }

        public static T[] Normalise<T>(this T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> Tot = new Number<T>();

            for (int i = 0; i < a.Length; i++)
            {
                Tot = (T)(res[i] + Tot);
            }

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = (T)(res[i] / Tot);
            }

            return res;
        }

        public static T[][] Inverse<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            throw new NotImplementedException();
        }

        public static T[] Inverse<T>(this T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            throw new NotImplementedException();
        }

        public static T[] ChangeSign<T>(this T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            T[] res = new T[a.Length];

            Number<T> N;

            for (int i = 0; i < a.Length; i++)
            {
                N = res[i];
                res[i] = (T)(-N);
            }
            return res;
        }

        public static T[][] ChangeSign<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            T[][] res = new T[a.GetUpperBound(0)][];

            Number<T> N;

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new T[a.GetUpperBound(1)];
                for (int j = 0; j < a.GetUpperBound(1); j++)
                {
                    N = res[i][j];
                    res[i][j] = (T)(-N);
                }
            }
            return res;
        }

        public static double[] ChangeSign(this double[] a) //where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            double[] res = new double[a.Length];

            double N;

            for (int i = 0; i < a.Length; i++)
            {
                N = (double)res[i];
                res[i] = (-N);
            }
            return res;
        }

        public static double[][] ABS<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            double[][] res = new double[a.GetUpperBound(0)][];

            Number<T> N;

            if (typeof(T) == typeof(double))
            {
                for (int i = 0; i < a.GetUpperBound(0); i++)
                {
                    res[i] = new double[a.GetUpperBound(1)];
                    for (int y = 0; y < a.GetUpperBound(1); y++)
                    {
                        N = a[i][y];
                        res[i][y] = Math.Abs((double)N);
                    }
                }
                return res;
            }
            return null;
        }

        /// <summary>
        /// Returned falttened position of largest number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int? ArgMax<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            int res = 0;
            double temp = 0;

            Number<T> N;

            if (typeof(T) == typeof(double))
            {
                for (int i = 0; i < a.GetUpperBound(0); i++)
                {
                    for (int y = 0; y < a.GetUpperBound(1); y++)
                    {
                        N = a[i][y];
                        if (N > temp)
                            res = i + y;
                    }
                }
                return res;
            }
            return null;
        }

        /// <summary>
        /// Returned falttened position of largest number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int? ArgMax<T>(this T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            int res = 0;
            double temp = 0;

            Number<T> N;

            if (typeof(T) == typeof(double))
            {
                for (int i = 0; i < a.GetUpperBound(0); i++)
                {
                    N = a[i];
                    if (N > temp)
                        res = i;
                }
                return res;
            }
            return null;
        }

        public static T[] Flat<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            T[] res = new T[a.LongLength];

            for (int i = 0; i < a.GetUpperBound(1); i++)
            {
                for (int y = 0; y < a.GetUpperBound(0); y++)
                {
                    res[i + y] = a[i][y];
                }
            }
            return res;
        }

        /*  public static T[] Exp<T>(this T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

              for (int i = 0; i < a.Length; i++)
              {
                  res[i] = (T)(Math.Exp(a[i]));
              }

              return res;
          }*/
    }

    /// <summary>
    /// Basic T and Null Double Methods
    /// </summary>
    public partial class Ext
    {
        public static double[] Mult<T>(this T[] a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> An;
            double Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i] = (An * Bn);
                }
            }
            return res;
        }

        public static double[] Mult<T>(this T[] a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (An * b);
            }
            return res;
        }

        public static Tuple<int, int> Shape(this double[][] a)
        {
            int av = a.GetUpperBound(0);
            int bv = a.GetUpperBound(1);

            return Tuple.Create(av, bv);
        }

        public static Tuple<int, int> Shape<T>(this T[][] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int av = a.GetUpperBound(0);
            int bv = a.GetUpperBound(1);

            return Tuple.Create(av, bv);
        }

        public static Tuple<int, int> Shape<T>(this T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int av = a.GetUpperBound(0);
            int bv = a.GetUpperBound(1);

            return Tuple.Create(av, bv);
        }
    }

    /// <summary>
    /// Basic Null Double and T Methods
    /// </summary>
    public partial class Ext
    {
        public static double[] Mult<T>(this double[] b, T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> An;
            double Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i] = (An * Bn);
                }
            }
            return res;
        }

        public static double[] Mult<T>(this double[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> an = b;

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] * an;
            }
            return res;
        }
    }

    /// <summary>
    /// Basic T and Int Methods
    /// </summary>
    public partial class Ext
    {
        public static double[] Mult<T>(this T[] a, int[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> An;
            int Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i] = (An * Bn);
                }
            }
            return res;
        }

        public static int[] Mult<T>(this T[] a, int b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(int) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int[] res = new int[a.Length];

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (An * b);
            }
            return res;
        }

        public static int[][] Divide<T>(this T[] a, int[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int[][] res = new int[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new int[b.Length];
            }

            Number<T> An;
            int Bn;

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
    }

    /// <summary>
    /// Basic T and Nullable Int Methods
    /// </summary>
    public partial class Ext
    {
        public static int?[][] Mult<T>(this T[] a, int?[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int?) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int?[][] res = new int?[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new int?[b.Length];
            }

            Number<T> An;
            int? Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i][y] = (An * Bn);
                }
            }
            return res;
        }

        public static int?[] Mult<T>(this T[] a, int? b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int?) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int?[] res = new int?[a.Length];

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (An * b);
            }
            return res;
        }

        public static int?[][] Divide<T>(this T[] a, int?[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int?) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int?[][] res = new int?[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new int?[b.Length];
            }

            Number<T> An;
            int? Bn;

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

        public static int?[] Divide<T>(this T[] a, int? b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (typeof(int?) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(decimal) != typeof(T) &&
                typeof(float) != typeof(T) &&
                typeof(double) != typeof(T) &&
                typeof(int?) != typeof(T) &&
                typeof(long) != typeof(T))
            {
                throw new ArgumentException(string.Format("The type {0} is not valid", typeof(T).Name));
            }

            int?[] res = new int?[a.Length];

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (An / b);
            }
            return res;
        }

        public static double[] Mult<T>(this double a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            double[] res = new double[b.Length];

            Number<T> bn;

            for (int i = 0; i < b.Length; i++)
            {
                bn = b[i];
                res[i] = a * bn;
            }
            return res;
        }
    }
}