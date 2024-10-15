using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        internal static List<double> MinMinus(List<double> a, List<double> b)
        {
            List<double> res = new List<double>(a.Count());
            for (int i = 0; i < a.Count; i++)
            {
                res[i] = a[i] - b[i];
            }
            return res;
        }

        internal static List<double> MinMinus(double a, List<double> b)
        {
            List<double> res = new List<double>(b.Count());
            for (int i = 0; i < b.Count; i++)
            {
                res[i] = (double)a - b[i];
            }
            return res;
        }
    }
}