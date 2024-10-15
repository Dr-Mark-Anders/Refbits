using System;
using System.Collections.Generic;

namespace Extensions
{
    /// <summary>
    /// Basic T Methods
    /// </summary>
    public partial class Ext
    {
        public static List<T> Divide<T>(this List<T> a, T b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                res[i] = (T)(An / b);
            }
            return res;
        }

        public static List<T> Normalise<T>(this List<T> a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            Number<T> Tot = new Number<T>();

            for (int i = 0; i < a.Count; i++)
            {
                Tot = (T)(res[i] + Tot);
            }

            for (int i = 0; i < a.Count; i++)
            {
                res[i] = (T)(res[i] / Tot);
            }

            return res;
        }
    }

    /// <summary>
    /// Basic T and Int Methods
    /// </summary>
    public partial class Ext
    {
        public static int[][] Mult<T>(this List<T> a, int[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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
                    res[i][y] = (An * Bn);
                }
            }
            return res;
        }

        public static int[] Mult<T>(this List<T> a, int b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            int[] res = new int[a.Count];

            Number<T> An;

            for (int i = 0; i < a.Count; i++)
            {
                An = a[i];
                res[i] = (An * b);
            }
            return res;
        }
    }
}