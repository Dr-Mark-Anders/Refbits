using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        internal static void RemoveFromTo<T>(this List<T> list, int lowBound, int uppBound)
        {
            for (int i = lowBound; i < uppBound; i++)
            {
                list.RemoveRange(lowBound, uppBound - lowBound);
            }
        }

        internal static double MinMinus(List<double> a, double b)
        {
            List<double> res = new List<double>(a.Count());
            for (int i = 0; i < a.Count; i++)
            {
                res[i] = a[i] - (double)b;
            }
            return res.Min();
        }
    }
}