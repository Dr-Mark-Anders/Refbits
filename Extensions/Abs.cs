using System;
using System.Collections.Generic;

namespace Extensions
{
    public partial class Ext
    {
        public static double Abs(this double val)
        {
            return Math.Abs((double)val);
        }

        public static double[] AbsWhere(this double[] val, string s, double d)
        {
            var res = (double[])val.Clone();
            for (int i = 0; i < val.Length; i++)
            {
                switch (s)
                {
                    case "<":
                        break;

                    case ">":
                        res[i] = Math.Abs(val[i]);
                        break;

                    case "=":
                        res[i] = Math.Abs(val[i]);
                        break;

                    case "==":
                        res[i] = Math.Abs(val[i]);
                        break;

                    default:
                        res[i] = val[i];
                        break;
                }
            }
            return res;
        }

        public static double[,] Abs(this double[,] val)
        {
            new NotImplementedException();
            return null;
        }

        public static double[] Abs(this double[] val)
        {
            var res = new double[val.Length];
            for (int i = 0; i < val.Length; i++)
            {
                res[i] = Math.Abs((double)val[i]);
            }
            return res;
        }

        public static double AbsMax(this double[] val)
        {
            var res = new double[val.Length];
            for (int i = 0; i < val.Length; i++)
            {
                res[i] = Math.Abs((double)val[i]);
            }
            return res.Max();
        }

        public static double AbsMin(this double[] val)
        {
            var res = new double[val.Length];
            for (int i = 0; i < val.Length; i++)
            {
                res[i] = Math.Abs((double)val[i]);
            }
            return res.Min();
        }

        public static List<double> Abs(this List<double> val)
        {
            for (int i = 0; i < val.Count; i++)
            {
                val[i] = Math.Abs(val[i]);
            }
            return val;
        }
    }
}