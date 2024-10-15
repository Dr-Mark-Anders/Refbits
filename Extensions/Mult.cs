using System;
using System.Collections.Generic;

namespace Extensions
{
    public partial class Ext
    {
        public static T[] Mult<T>(this T[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i] = (T)(An * Bn);
                }
            }
            return res;
        }

        public static T[] Mult<T>(this T[][] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i][y];
                    Bn = b[y];
                    res[i] = (T)(res[i] + (An * Bn));
                }
            }
            return res;
        }

        public static T[][] Mult<T>(this T[][] a, object b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            T[][] res = new T[a.GetUpperBound(0)][];

            Number<T> An;
            Number<T> Bn = (T)b;

            for (int i = 0; i < a.GetUpperBound(0); i++)

            {
                res[i] = new T[a.GetUpperBound(1)];
                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    res[i][y] = (T)(An * Bn);
                }
            }
            return res;
        }

        public static T[][] Mult<T>(this T[][] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    res[i][y] = (T)(An * b);
                }
            }
            return res;
        }

        public static T[][] Mult<T>(this T[][] a, T[][] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> An, Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = b[y][i];
                    res[i][y] = (T)(An * Bn);
                }
            }
            return res;
        }

        /*  public static T[,] Mult<T>(this T[,] a, T[,] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
          {
              int len = a.GetLength(0);

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

              T[,] res = new T[len,len];

              Number<T> An, Bn;

              for (int i = 0; i < len; i++)
              {
                  for (int y = 0; y < len; y++)
                  {
                      An = a[i,y];
                      Bn = b[y,i];
                      res[i,y] = (T)(An * Bn);
                  }
              }
              return res;
          }*/

        public static double[,] Mult(this double[,] a, double[,] b)
        {
            int ACols = a.GetLength(0);
            int ResRows = a.GetLength(1);
            int ResCols = b.GetLength(0);

            double[,] res = new double[ResCols, ResRows];

            for (int rescol = 0; rescol < ResCols; rescol++)
            {
                for (int resrow = 0; resrow < ResRows; resrow++)
                {
                    for (int acol = 0; acol < ACols; acol++)
                    {
                        res[rescol, resrow] += a[acol, resrow] * b[rescol, acol];
                    }
                }
            }
            return res;
        }

        public static double[][] Mult(this double[][] a, double[][] b)
        {
            double[][] res = new double[a.Length][];

            double An;
            double Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < a.GetUpperBound(1); y++)
                {
                    An = a[i][y];
                    Bn = b[y][i];
                    res[i][y] += An * Bn;
                }
            }
            return res;
        }

        public static T[] Mult<T>(this T[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (T)(An * b);
            }
            return res;
        }

        public static List<T> Mult<T>(this List<T> a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            List<T> res = new List<T>();

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (T)(An * b);
            }
            return res;
        }

        public static double[][] Mult<T>(this List<T> a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                    res[i][y] = (An * Bn);
                }
            }
            return res;
        }

        public static double[] Mult<T>(this List<T> a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            double[] res = new double[a.Count];

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (An * b);
            }
            return res;
        }

        public static double[][] MultRows(this double[][] data, double[] mult)
        {
            double[][] res = new double[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                res[i] = new double[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                {
                    res[i][j] += data[i][j] * mult[j];
                }
            }
            return res;
        }

        //fixed
        public static double[] Mult(this double[][] data, double[] mult)
        {
            double[] res = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                //res[i] = new double[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                    res[i] += data[i][j] * mult[j];
            }
            return res;
        }

        public static double[] Mult(this double[] mult, double[][] data)
        {
            double[] res = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                // res[i] = new double[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                {
                    res[i] += data[i][j] * mult[j];
                }
            }
            return res;
        }

        public static double[] MultSimple<T>(this double[] b, T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                An = a[i];
                Bn = b[i];
                res[i] = (An * Bn);
            }
            return res;
        }

        public static int[] Multiply(this int[] data, int mult)
        {
            int[] res = new int[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                res[i] = data[i] * mult;
            }

            return res;
        }

        public static double[] Multiply(this int[] data, double mult)
        {
            double[] res = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                res[i] = data[i] * mult;
            }

            return res;
        }

        public static List<double> Multiply(List<double> data, int mult)
        {
            List<double> res = new List<double>(new double[data.Count]);

            for (int i = 0; i < data.Count; i++)
            {
                res[i] = data[i] * mult;
            }

            return res;
        }

        public static List<double> Mult(this List<double> list, List<double> m)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] *= m[i];
            }
            return list;
        }

        public static List<int> Mult(this List<int> list, int m)
        {
            List<int> res = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i] * m);
            }
            return res;
        }

        public static List<double> Mult(this List<double> list, double m)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i] * m);
            }
            return list;
        }

        public static List<double> MultNull(this List<double> list, double m)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i] * m);
            }
            return res;
        }

        public static List<double> Multiply(List<double> data, double mult)
        {
            List<double> res = new List<double>(data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                res[i] = data[i] * mult;
            }

            return res;
        }
    }
}