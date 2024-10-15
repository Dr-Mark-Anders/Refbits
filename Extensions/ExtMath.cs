using System;
using System.Collections.Generic;

namespace Extensions
{
    public static partial class Ext
    {
        public static List<double> Take(this List<double> thelist, List<int> indexes)
        {
            return new List<double>();
        }

        public static double CubeRoot(this double x)
        {
            if (x < 0)
                return -Math.Pow(-x, (1.00 / 3.0));
            else
                return Math.Pow(x, (1.00 / 3.0));
        }
    }
}