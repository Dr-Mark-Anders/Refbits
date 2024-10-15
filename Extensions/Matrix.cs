using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        public static T[,] Transpose<T>(this T[,] l)
        {
            int dim = l.GetLength(0);

            T[,] res = new T[dim, dim];

            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                    res[i, j] = l[j, i];

            return res;
        }

        public static T[] addreduce<T>(this T?[][] l, int v) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T addreduce<T>(this T?[] l, int v) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T[] addreduce<T>(this T[][] l, int v) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T addreduce<T>(this T[] l, int v) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T addreduce<T>(this T[] l) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T[] addreduce<T>(this T[][] l) where T : struct
        {
            throw new NotImplementedException();
        }

        public static double[] Mult<T>(this double[][] a, double[] b)
        {
            double[] res = new double[a.GetUpperBound(0)];

            for (int i = 0; i < a.GetUpperBound(0); i++)
                for (int y = 0; y < a.GetUpperBound(1); y++)
                    res[i] += (a[i][y] * b[i]);

            return res;
        }

        /// <summary>
        /// Yields a column from a jagged array.
        /// An exception will be thrown if the column is out of bounds, and return default in places where there are no elements from inner arrays.
        /// Note: There is no equivalent GetRow method, as you can use array[row] to retrieve.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="rectarray">The source array.</param>
        /// <param name="column">Column record to retrieve, 0-based index.</param>
        /// <returns>Yielded enumerable of column elements for given column, and default values for smaller inner arrays.</returns>
        public static void SetColumn<T>(this T[][] rectarray, int column, T[] values)
        {
            if (column < 0 || column >= rectarray.Max(array => array.Length))
                throw new ArgumentOutOfRangeException(nameof(column));

            for (int r = 0; r < rectarray.GetLength(0); r++)
                rectarray[r][column] = values[r];
        }

        public static int[] ones(int numStages)
        {
            int[] res = new int[numStages];
            for (int i = 0; i < numStages; i++)
                res[i] = 1;

            return res;
        }

        public enum accum
        { simple, add, Max, Min, Mult, Div }

        public static double[] accumulate(double[] a, accum acc = accum.simple)
        {
            double[] res = new double[a.Length];
            switch (acc)
            {
                case accum.simple:
                    res[0] = a[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        res[i] = a[i - 1] + a[i];
                    }
                    break;

                case accum.Max:
                    res[0] = a[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        if (a[i - 1] > a[i])
                            res[i] = a[i - 1];
                    }
                    break;

                case accum.Min:
                    res[0] = a[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        if (a[i] < res[i - 1])
                            res[i] = a[i];
                    }
                    break;

                case accum.Mult:
                    res[0] = a[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        res[i] = res[i - 1] * a[i];
                    }
                    break;
            }
            return res;
        }

        public static double[] accumulate(List<double> a, accum acc)
        {
            List<double> res = new List<double>();
            switch (acc)
            {
                case accum.simple:
                    res.Add(a[0]);
                    for (int i = 1; i < a.Count; i++)
                    {
                        res.Add(a[i - 1] + a[i]);
                    }
                    break;

                case accum.add:
                    res.Add(a[0]);
                    for (int i = 1; i < a.Count; i++)
                    {
                        res.Add(res[i - 1] + a[i]);
                    }
                    break;

                case accum.Max:
                    res[0] = a[0];
                    for (int i = 1; i < a.Count; i++)
                    {
                        if (a[i - 1] > a[i])
                            res.Add(a[i - 1]);
                    }
                    break;

                case accum.Min:
                    res[0] = a[0];
                    for (int i = 1; i < a.Count; i++)
                    {
                        if (a[i] < res[i - 1])
                            res.Add(a[i]);
                    }
                    break;

                case accum.Mult:
                    res[0] = a[0];
                    for (int i = 1; i < a.Count; i++)
                    {
                        res.Add(a[i - 1] * a[i]);
                    }
                    break;
            }
            return res.ToArray();
        }

        public static double Mult(this double[] a, List<double> b)
        {
            double res = 0;
            for (int i = 0; i < a.Length; i++)
            {
                res += a[i] * b[i];
            }
            return res;
        }

        public static double[] Mult(this double[] a, double[] b)
        {
            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] * b[i];
            }
            return res;
        }

        public static double[] Mult(this double[] a, double b)
        {
            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] * b;
            }
            return res;
        }

        public static double[] Mult(this double b, double[] a)
        {
            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] * b;
            }
            return res;
        }

        internal static double[] MatrixMultiply(double[][] a, double[] b)
        {
            double[] res = new double[a.Length];
            for (int x = 0; x < a.Length; x++)
            {
                for (int y = 0; y < a[x].Length; y++)
                {
                    res[x] = a[x][y] * b[x];
                }
            }
            return res;
        }

        internal static double[][] MatrixMultiply(double[][] a, double[][] b)
        {
            double[][] res = new double[a.Length][];
            for (int x = 0; x < a.Length; x++)
            {
                res[x] = new double[b.Length];

                for (int y = 0; y < a[x].Length; y++)
                {
                    res[x][y] = (double)a[x][y] * b[x][y];
                }
            }
            return res;
        }

        internal static double[] MatrixMultiply(double a, double[] b)
        {
            double[] res = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                res[i] = a * b[i];
            };
            return res;
        }

        public static double[] VectorMinus(this double[] a, double[] b)
        {
            if (a.Length != b.Length)
                return null;

            double[] res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] - b[i];
            }
            return res;
        }

        public static double[,] Column(this double[,] a, int c, double[] b)
        {
            for (int i = 0; i < b.Length; i++)
            {
                a[c, i] = (double)b[i];
            }
            return a;
        }

        public static double[][] Column(this double[][] a, int c, double[] b)
        {
            for (int r = 0; r < b.Length; r++)
            {
                a[r][c] = b[r];
            }
            return a;
        }

        internal static double[] MultiplyArray(this double[] a, double[] b)
        {
            double[] res = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] * b[i];
            }
            return res;
        }

        internal static double[][] Outerproduct(double[] a, double[] b)
        {
            double[][] res = new double[a.Length][];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = new double[b.Length];

                for (int j = 0; j < b.Length; j++)
                {
                    res[i][j] = a[i] * b[j];
                }
            }

            return res;
        }

        internal static List<double> ToNullList(this double[] list)
        {
            var res = new List<double>();
            for (int i = 0; i < list.Length; i++)
            {
                res.Add(list[i]);
            }
            return res;
        }

        internal static double[] ToNullArray(this List<double> list)
        {
            var res = new double[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                res[i] = (double)list[i];
            }
            return res;
        }

        internal static double[] ToDouble(this double[] list)
        {
            var res = new double[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                res[i] = (double)list[i];
            }
            return res;
        }

        internal static double[][] ToDouble(this double[][] list)
        {
            var res = new double[list.GetUpperBound(0)][];
            for (int i = 0; i < list.Length; i++)
            {
                res[i] = new double[list.GetUpperBound(1)];
                for (int y = 0; y < list.GetUpperBound(1); y++)
                {
                    res[i][y] = (double)list[i][y];
                }
            }
            return res;
        }

        internal static double[][] ToNullDouble(this double[][] list)
        {
            var res = new double[list.GetUpperBound(0)][];
            for (int i = 0; i < list.Length; i++)
            {
                res[i] = new double[list.GetUpperBound(1)];
                for (int y = 0; y < list.GetUpperBound(1); y++)
                {
                    res[i][y] = list[i][y];
                }
            }
            return res;
        }

        internal static double[][] Resize(this double[][] list, Tuple<int, int> size)
        {
            var res = new double[size.Item1][];
            for (int i = 0; i < list.GetUpperBound(0); i++)
            {
                res[i] = new double[size.Item2];
                for (int y = 0; y < list.GetUpperBound(1); y++)
                {
                    res[i][y] = list[i][y];
                }
            }
            return res;
        }

        internal static double[] ToNullDouble(this double[] list)
        {
            var res = new double[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                res[i] = list[i];
            }
            return res;
        }
    }
}