using System;

namespace Extensions
{
    public partial class Ext
    {
        public static T[][] OuterProduct<T>(this T[] a, T[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new T[b.Length];
            }

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = a[i];
                    Bn = b[y];
                    res[i][y] = (T)(An * Bn);
                }
            }
            return res;
        }

        public static T?[][] OuterProduct<T>(this T?[] a, T?[] b) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            T?[][] res = new T?[a.Length][];

            for (int i = 0; i < b.Length; i++)
            {
                res[i] = new T?[b.Length];
            }

            Number<T> An;
            Number<T> Bn;

            for (int i = 0; i < a.Length; i++)
            {
                for (int y = 0; y < b.Length; y++)
                {
                    An = (T)a[i];
                    Bn = (T)b[y];
                    res[i][y] = (T)(An * Bn);
                }
            }
            return res;
        }
    }
}