using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        /* public static double[] Exp(this double[] data)
        {
             double[] sum = new double[data.Length];

	        for (int i = 0; i < data.Length; i++) // number of rows (componenets)
	        {
	            sum[i] = Math.Exp(data[i]);
	        }

             return sum;
         }*/

        public static T[,] ToMultiArray<T>(this IList<T[]> arrays)
        {
            var length = arrays[0].Length;
            var result = new T[arrays.Count, length];
            for (var i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];
                if (array.Length != length)
                {
                    throw new ArgumentException("Misaligned arrays");
                }

                for (var j = 0; j < length; j++)
                {
                    result[i, j] = array[j];
                }
            }

            return result;
        }

        public static T[][] ToJaggedArray<T>(this IList<T[]> arrays)
        {
            var result = new T[arrays.Count][];
            for (var i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];
                var length = array.Length;
                result[i] = new T[length];
                for (var j = 0; j < length; j++)
                {
                    result[i][j] = array[j];
                }
            }

            return result;
        }

        public static double[] Ln(this double[] data)
        {
            double[] sum = new double[data.Length];

            for (int i = 0; i < data.Length; i++) // number of rows (componenets)
            {
                sum[i] = Math.Log(data[i]);
            }

            return sum;
        }

        public static int CountValidColumnEntries(this List<double[]> data, int Col)
        {
            int sum = 0;

            for (int i = 0; i < data.Count; i++) // number of rows (componenets)
            {
                if (!double.IsNaN(data[i][Col]))
                    sum += 1;
            }

            return sum;
        }

        public static int CountValidRowEntries(this List<double[]> data, int Row)
        {
            int sum = 0;

            for (int i = 0; i < data.Count; i++) // number of rows (componenets)
            {
                if (!double.IsNaN(data[Row][i]))
                    sum += 1;
            }

            return sum;
        }

        public static int CountValidColumnEntries(this double[][] data, int Col)
        {
            int sum = 0;

            for (int i = 0; i < data.Length; i++) // number of rows (componenets)
            {
                if (!double.IsNaN(data[i][Col]))
                    sum += 1;
            }

            return sum;
        }

        public static int CountValidRowEntries(this double[][] data, int Row)
        {
            int sum = 0;

            for (int i = 0; i < data.Length; i++) // number of rows (componenets)
            {
                if (!double.IsNaN(data[Row][i]))
                    sum += 1;
            }

            return sum;
        }

        public static bool CompareGTE(this double[][] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                for (int y = 0; y < p.GetUpperBound(1); y++)
                {
                    if (p[i][y] >= val)
                        return true;
                }
            }
            return false;
        }

        public static IEnumerable<T> GetColumn<T>(this T[,] rectarray, int column)
        {
            if (column < 0 || column >= rectarray.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (int r = 0; r < rectarray.GetLength(0); r++)
            {
                yield return rectarray[r, column];
            }
        }

        public static IEnumerable<T> GetColumn<T>(this T[][] rectarray, int column)
        {
            if (column < 0 || column >= rectarray.Max(array => array.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (int r = 0; r < rectarray.GetLength(0); r++)
            {
                if (column >= rectarray[r].Length)
                {
                    yield return default(T);

                    continue;
                }

                yield return rectarray[r][column];
            }
        }

        public static bool CompareGTE(this double[] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                if (p[i] >= val)
                    return true;
            }
            return false;
        }

        public static bool CompareGT(this double[] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                if (p[i] > val)
                    return true;
            }
            return false;
        }

        public static bool CompareLTE(this double[] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                if (p[i] <= val)
                    return true;
            }
            return false;
        }

        public static bool CompareLTE(this double[][] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                for (int y = 0; y < p.GetUpperBound(1); y++)
                {
                    if (p[i][y] >= val)
                        return true;
                }
            }
            return false;
        }

        public static bool CompareLT(this double[][] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                for (int y = 0; y < p.GetUpperBound(1); y++)
                {
                    if (p[i][y] < val)
                        return true;
                }
            }
            return false;
        }

        public static bool CompareLT(this double[] p, double val)
        {
            for (int i = 0; i < p.GetUpperBound(0); i++)
                if (p[i] < val)
                    return true;

            return false;
        }

        public static int[] CompareLT(this double[] p, double[] val)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < p.GetUpperBound(0); i++)
                if (p[i] < val[i])
                    res.Add(i);

            return res.ToArray();
        }

        public static double[] Put(this double[] p, int[] index, double[] data)
        {
            double[] res = p;
            int pos = 0;

            for (int i = 0; i < index.Length; i++)
                //pos = GetPos(p, index[i]);
                res[pos] = data[pos];

            return res;
        }

        public static double[][] Put(this double[][] p, int[] index, double[][] data)
        {
            double[][] res = p;
            Tuple<int, int> pos;

            for (int i = 0; i < index.Length; i++)
            {
                pos = GetPos(p, index[i]);
                res[pos.Item1][pos.Item2] = data[pos.Item1][pos.Item2];
            }
            return res;
        }

        public static Tuple<int, int> GetPos(double[][] arr, int y)
        {
            int total = 0, previoustotal = 0;
            int r = 0, c = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                previoustotal = total;
                total += arr[i].Length;
                if (y < total)
                {
                    r = i;
                    c = y - previoustotal;
                    break;
                }
            }
            return Tuple.Create(r, c);
        }

        public static double[] Exp(this double[] p)
        {
            double[] res = new double[p.Length];

            for (int i = 0; i < p.Length; i++)
                res[i] = Math.Exp(p[i]);

            return res;
        }

        public static double[] Exp<T>(this IEnumerable<T> p) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            double[] res = new double[p.Count()];

            Number<T> N;

            for (int i = 0; i < p.Count(); i++)
            {
                N = p.ElementAt(i);
                res[i] = Math.Exp(N);
            }
            return res;
        }

        public static double[][] Exp<T>(this T[][] p) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            double[][] res = new double[p.Length][];

            Number<T> N;

            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                res[i] = new double[p.GetUpperBound(1)];

                for (int y = 0; y < p.GetUpperBound(1); y++)
                {
                    N = p[i][y];
                    res[i][y] = Math.Exp((double)N);
                }
            }
            return res;
        }

        public static double[][] Exp(this double[][] p)
        {
            double[][] res = new double[p.Length][];

            for (int i = 0; i < p.GetUpperBound(0); i++)
            {
                res[i] = new double[p.GetUpperBound(1)];

                for (int y = 0; y < p.GetUpperBound(1); y++)
                {
                    //N = p[i][y];
                    res[i][y] = Math.Exp((double)p[i][y]);
                }
            }
            return res;
        }
    }
}