using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        public static string Slice(this string li, int? start, int? end = null)
        {
            if (end is null)
                end = li.Length;
            if (start is null)
                start = 0;

            if (start < 0)    // support negative indexing
            {
                start = li.Length + start;
            }
            if (end < 0)      // support negative indexing
            {
                end = li.Length + end;
            }
            if (start > li.Length)    // if the start value is too high
            {
                start = li.Length;
            }
            if (end > li.Length)      // if the end value is too high
            {
                end = li.Length;
            }
            var count = (int)end - (int)start;             // calculate count (number of elements)
            return li.Substring((int)start, count);    // return a shallow copy of li of count elements
        }

        public static List<T> Slice<T>(this List<T> li, int start, int end)
        {
            if (start < 0)    // support negative indexing
            {
                start = li.Count + start;
            }
            if (end < 0)      // support negative indexing
            {
                end = li.Count + end;
            }
            if (start > li.Count)    // if the start value is too high
            {
                start = li.Count;
            }
            if (end > li.Count)      // if the end value is too high
            {
                end = li.Count;
            }
            var count = end - start;             // calculate count (number of elements)
            return li.GetRange(start, count);    // return a shallow copy of li of count elements
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int? start, int? end)
        {
            if (end is null)
                end = source.Length;
            if (start is null)
                start = 0;

            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = (int)end - (int)start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + (int)start];
            }
            return res;
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static double[] Slice(this double[][] source, int? start, int? end)
        {
            if (end is null)
                end = source.Length;

            if (start is null)
            {
                start = 0;
            }

            // Handles negative ends.
            if (end < 0)
                end = source.Length + end;

            int len = source.Length - (int)start;

            // Return new array.
            double[] res = new double[len];

            for (int i = 0; i < len; i++)
                res[i] = source[i][(int)end];

            return res;
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static void SliceAndSet<T>(this T[] source, int start, int end, T[] val)
        {
            if (!double.IsNaN(start))
                start = 0;
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - (int)start;

            // Return new array.
            //T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                source[i] = val[i + (int)start];
            }
            return;// res;
        }

        /// <summary>
		/// Slices the specified collection like Python does
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me">the collection</param>
		/// <param name="start">The start point.</param>
		/// <param name="end">The end point.</param>
		/// <returns></returns>
		/// <example><code>
		/// var numbers=new[]{1,2,3,4,5};
		/// var eg = numbers.Slice(-3,-1);
		/// foreach (var i in eg)
		///	{
		///		Console.Write("{0}, ", i);
		///	}
		///	Console.Writeline();	// Output: 3, 4,</code></example>
		/// <exception cref="System.ArgumentException">starting point must be less than or equal to ending point;start</exception>
		public static T[][] Slice<T>(this T[][] me, int? row, int? colstart = null, int? colend = null)
        {
            if (colstart.HasValue && colend.HasValue && colstart.Value > colend.Value && colend.Value >= 0)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            T[][] res = new T[me.GetUpperBound(0)][];

            if (row is null)
            {
                for (int r = (int)colstart; r < colend; colstart++)
                {
                    res[r] = me.GetColumn(r).ToArray();
                }
            }
            else
            {
                for (int c = (int)colstart; c < colend; colstart++)
                {
                    res[0][c] = me[(int)row][c];
                }
            }

            return res;
        }

        /// <summary>
        /// Slices the specified collection like Python does
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me">the collection</param>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <returns></returns>
        /// <example><code>
        /// var numbers=new[]{1,2,3,4,5};
        /// var eg = numbers.Slice(-3,-1);
        /// foreach (var i in eg)
        ///	{
        ///		Console.Write("{0}, ", i);
        ///	}
        ///	Console.Writeline();	// Output: 3, 4,</code></example>
        /// <exception cref="System.ArgumentException">starting point must be less than or equal to ending point;start</exception>
        public static T[][] SliceAndSet<T>(this T[][] me, int? row, int? colstart, int? colend, T[][] values)
        {
            if (colstart.HasValue && colend.HasValue && colstart.Value > colend.Value && colend.Value >= 0)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            if (colstart is null)
                colstart = 0;
            if (colend is null)
                colend = me.GetUpperBound(1);

            T[][] res = new T[me.GetUpperBound(0)][];

            if (row is null)
            {
                for (int r = (int)colstart; r < colend; colstart++)
                {
                    res[r] = new T[(int)colend - (int)colstart];

                    for (int c = 0; c < me.GetUpperBound(0); c++)
                    {
                        res[r][c] = values[r][c];
                    }
                }
            }
            return res;
        }

        public static T[][] SliceAndSet<T>(this T[][] me, int? row, int? colstart, int? colend, T[] values)
        {
            if (colstart.HasValue && colend.HasValue && colstart.Value > colend.Value && colend.Value >= 0)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            if (colstart is null)
                colstart = 0;
            if (colend is null)
                colend = me.GetUpperBound(1);

            T[][] res = new T[me.GetUpperBound(0)][];

            if (row is null)
            {
                for (int r = (int)colstart; r < colend; colstart++)
                {
                    res[r] = new T[(int)colend - (int)colstart];

                    for (int c = 0; c < me.GetUpperBound(0); c++)
                    {
                        res[r][c] = values[r];
                    }
                }
            }
            return res;
        }

        public static T[][] SliceAndSet<T>(this T[][] me, int? row, int? colstart, int? colend, T value)
        {
            if (colstart.HasValue && colend.HasValue && colstart.Value > colend.Value && colend.Value >= 0)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            if (colstart is null)
                colstart = 0;
            if (colend is null)
                colend = me.GetUpperBound(1);

            T[][] res = new T[me.GetUpperBound(0)][];

            if (row is null)
            {
                for (int r = (int)colstart; r < colend; colstart++)
                {
                    res[r] = new T[(int)colend - (int)colstart];

                    for (int c = 0; c < me.GetUpperBound(0); c++)
                    {
                        res[r][c] = value;
                    }
                }
            }
            return res;
        }

        public static T[] SliceAndSet<T>(this T[] me, int? colstart, int? colend, T value)
        {
            if (colstart.HasValue && colend.HasValue && colstart.Value > colend.Value && colend.Value >= 0)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            if (colstart is null)
                colstart = 0;
            if (colend is null)
                colend = me.GetUpperBound(1);

            T[] res = new T[me.GetUpperBound(0)];

            for (int c = (int)colstart; c < (int)colend; c++)
            {
                res[c] = value;
            }

            return res;
        }

        public static T?[] SliceAndSet<T>(this T?[] me, int? colstart, int? colend, T? value) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (colstart.HasValue && colend.HasValue && colstart.Value > colend.Value && colend.Value >= 0)
            {
                throw new ArgumentException("starting point must be less than or equal to ending point", "start");
            }

            if (colstart is null)
                colstart = 0;
            if (colend is null)
                colend = me.GetUpperBound(1);

            T?[] res = new T?[me.GetUpperBound(0)];

            for (int c = (int)colstart; c < (int)colend; c++)
            {
                res[c] = value;
            }

            return res;
        }

        public static void SliceAndSet(double[][] jacobian, int j, double v)
        {
            throw new NotImplementedException();
        }

        public static void SliceAndSet(double[][] jacobian, int j, double[] v)
        {
            throw new NotImplementedException();
        }

        public static string join(this string val, List<string> b)
        {
            string res = val;
            for (int i = 0; i < b.Count; i++)
            {
                res = res + b[i];
            }
            return val;
        }

        public static string join(this string val, string[] b)
        {
            string res = val;
            for (int i = 0; i < b.Length; i++)
            {
                res = res + b[i];
            }
            return val;
        }

        public static List<string> split(this string val, char b)
        {
            List<string> res = new List<string>();
            res.AddRange(val.Split(new char[] { b }));
            return res;
        }

        public static List<string> split(this string val, string b)
        {
            List<string> res = new List<string>();
            res.AddRange(val.Split(new string[] { b }, StringSplitOptions.None));
            return res;
        }

        public static List<string> split(this string val, string b, int count)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < b.Length; i++)
                res.AddRange(val.Split(new string[] { b }, count, StringSplitOptions.None));

            return res;
        }
    }
}