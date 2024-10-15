using System;
using System.Collections.Generic;

namespace Extensions
{
    public static partial class Ext
    {
        public delegate dynamic F1(double val);

        public delegate double F2(double val);

        public delegate double F3(int val1, int val2, int val3, double val4);

        internal static List<dynamic> map(F1 func, List<dynamic> vals)
        {
            List<dynamic> res = new List<dynamic>();
            for (int i = 0; i < vals.Count; i++)
            {
                res.Add(func(vals[i]));
            }
            return res;
        }

        internal static List<double> map(F2 func, List<double> vals)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < vals.Count; i++)
            {
                res.Add(func(vals[i]));
            }
            return res;
        }

        internal static double[] map(F2 func, double[] vals)
        {
            double[] res = new double[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                res.Add(func(vals[i]));
            }
            return res;
        }

        internal static List<double> map(F3 func, int prop1, int prop2, int phase, double[] vals)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < vals.Length; i++)
            {
                res.Add(func(prop1, prop2, phase, vals[i]));
            }
            return res;
        }

        internal static List<double> map(Func<double, double, double, double[], List<double>> propertiesForIter, List<string> prop1, List<string> prop2, int phase, double[] frac)
        {
            throw new NotImplementedException();
        }

        internal static List<double> map(Func<int, int, int, double[], List<double>> propertiesForIter, double[] prop1, double[] prop2, int phase, double[] frac)
        {
            throw new NotImplementedException();
        }

        internal static List<double> map(Func<double, int, bool> func, List<double> vals, int val2)
        {
            throw new NotImplementedException();
        }

        internal static List<double> map(Func<int, int, int, double[], List<double>> propertiesForIter, List<double> prop1, List<double> prop2, int phase, double[] frac)
        {
            throw new NotImplementedException();
        }

        internal static List<double> map(Func<int, int, int, double[], List<double>> propertiesForIter, int prop1, int prop2, int phase, double[] frac)
        {
            throw new NotImplementedException();
        }

        internal static dynamic map(Func<string, string> list, List<string> ipVals)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < ipVals.Count; i++)
            {
                res.Add(ipVals[i]);
            }
            return res;
        }

        internal static double WhereLessThan(double[] newvars, double[] minV, double[] v1)
        {
            throw new NotImplementedException();
        }

        internal static double WhereGreaterThan(double[] newvars, double[] maxV, double[] v1)
        {
            throw new NotImplementedException();
        }

        internal static List<double> concatenate(List<double> a, List<double> b)
        {
            b.InsertRange(0, a);

            return b;
        }

        internal static double[] Concatenate(double[] a, double[] b)
        {
            double[] res = new double[a.Length + b.Length];
            for (int i = 0; i < a.Length + b.Length; i++)
            {
                if (i < a.Length)
                    res[i] = a[i];
                else
                    res[i] = b[i];
            }
            return res;
        }

        internal static double[] Concatenate(double[] a, double b)
        {
            double[] res = new double[a.Length + 1];
            for (int i = 0; i < a.Length + 1; i++)
            {
                if (i < a.Length)
                    res[i] = a[i];
                else
                    res[i] = b;
            }
            return res;
        }

        internal static int[] Concatenate(int[] a, int[] b)
        {
            int[] res = new int[a.Length + b.Length];
            for (int i = 0; i < a.Length + b.Length; i++)
            {
                if (i < a.Length)
                    res[i] = a[i];
                else
                    res[i] = b[i];
            }
            return res;
        }

        internal static int[] Concatenate(int[] a, int b)
        {
            int[] res = new int[a.Length + 1];
            for (int i = 0; i < a.Length + 1; i++)
            {
                if (i < a.Length)
                    res[i] = a[i];
                else
                    res[i] = b;
            }
            return res;
        }

        internal static double[] Concatenate(List<double> a, List<double> b)
        {
            double[] res = new double[a.Count + b.Count];
            for (int i = 0; i < a.Count + b.Count; i++)
            {
                if (i < a.Count)
                    res[i] = a[i];
                else
                    res[i] = b[i];
            }
            return res;
        }

        internal static List<int> shape(object value)
        {
            List<int> res = new List<int>();

            if (value == null)
            {
                return null;
            }
            else
            {
                if (value is double[])
                    return new List<int> { 1, ((double[])value).Length };
                if (value is double[][])// || value is double[,])
                    return new List<int> { 2, ((double[][])value).GetUpperBound(0), ((double[][])value).GetUpperBound(1) };
                if (value is double[,])// || value is double[,])
                    return new List<int> { 2, ((double[,])value).GetUpperBound(0), ((double[,])value).GetUpperBound(1) };
                if (value is double[,])// || value is double[,])
                    return new List<int> { 3, ((double[,])value).GetUpperBound(0), ((double[,])value).GetUpperBound(1), ((double[,])value).GetUpperBound(2) };
            }
            return null;
        }

        internal static double[] map2(Func<double, double, double, double, double> calcLMTD, IEnumerable<double> enumerable1, IEnumerable<double> enumerable2, double v1, double v2)
        {
            throw new NotImplementedException();
        }

        internal static double[] map3(string v1, double v2, IEnumerable<double> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}