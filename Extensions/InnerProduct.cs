using System;

namespace Extensions
{
    public partial class Ext
    {
        public static double InnerProduct<T>(this double[] b, T[] a) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
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

            double res = 0;

            Number<T> An;
            double Bn;

            for (int i = 0; i < a.Length; i++)
            {
                An = a[i];
                Bn = b[i];
                res += (An * Bn);
            }
            return res;
        }

        public static double InnerProduct(this double[] b, double[] a)
        {
            double res = 0;

            for (int i = 0; i < a.Length; i++)
                res += a[i] * b[i];

            return res;
        }
    }
}