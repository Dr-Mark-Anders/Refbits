using System;
using System.Collections.Generic;

namespace Extensions
{
    public partial class Ext
    {
        public static T[] Add<T>(this T[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (T)(An + Bn);
            }
            return res;
        }

        public static T[] Add<T>(this T[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (T)(An + b);
            }
            return res;
        }

        public static double[][] Add(this double[][] a, double[][] x)
        {
            double[][] res = new double[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new double[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    res[i][j] = a[i][j] + x[i][j];
                }
            }
            return res;
        }

        public static double[][] Add(this double[][] a, double x)
        {
            double[][] res = new double[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new double[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    res[i][j] = a[i][j] + x;
                }
            }
            return res;
        }

        public static double[] Add(this double[] a, double x)
        {
            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] + x;
            }
            return res;
        }

        public static double[] Add(this double[] a, double[] x)
        {
            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] + x[i];
            }
            return res;
        }

        public static int?[] Add<T>(this List<T> a, int?[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int?[] res = new int?[a.Count];

            Number<T> An;
            int? Bn;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (An + Bn);
            }
            return res;
        }

        public static int?[] Add<T>(this List<T> a, int? b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int?[] res = new int?[a.Count];

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (An + b);
            }
            return res;
        }

        public static int[] Add<T>(this List<T> a, int[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int[] res = new int[a.Count];

            Number<T> An;
            int Bn;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (An + Bn);
            }
            return res;
        }

        public static int[] Add<T>(this List<T> a, int b)
            where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int[] res = new int[a.Count];

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (An + b);
            }
            return res;
        }

        public static double[] Add<T>(this double[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (a[i] + Bn);
            }
            return res;
        }

        public static double[] Add<T>(this double[] a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = An + a[i];
            }
            return res;
        }

        public static double[] Add<T>(this T[] a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (An + Bn);
            }
            return res;
        }

        public static double[] Add<T>(this T[] a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (An + b);
            }
            return res;
        }

        public static int?[] Add<T>(this T[] a, int?[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
            int? Bn;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (An + Bn);
            }
            return res;
        }

        public static int?[] Add<T>(this T[] a, int? b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (An + b);
            }
            return res;
        }

        public static int[] Add<T>(this T[] a, int[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int[] res = new int[a.Length];

            Number<T> An;
            int Bn;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (An + Bn);
            }
            return res;
        }

        public static int[] Add<T>(this T[] a, int b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int[] res = new int[a.Length];

            Number<T> An;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                res[i] = (An + b);
            }
            return res;
        }

        public static double[] Add<T>(this List<T> a, double[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
            double Bn;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (An + Bn);
            }
            return res;
        }

        public static double[] Add<T>(this List<T> a, double b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (An + b);
            }
            return res;
        }

        public static List<T> Add<T>(this List<T> a, List<T> b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
            Number<T> Bn;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                Bn = b[i];
                res[i] = (T)(An + Bn);
            }
            return res;
        }

        public static List<T> Add<T>(this List<T> a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (T)(An + b);
            }
            return res;
        }
    }
}