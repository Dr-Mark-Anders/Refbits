using System.Collections.Generic;
using Units.UOM;

namespace Units
{
    public static class UOMExtension
    {
        public static double[] ToDouble(this Temperature[] T)
        {
            double[] res = new double[T.Length];

            for (int i = 0; i < T.Length; i++)
                res[i] = T[i].BaseValue;

            return res;
        }

        public static List<double> ToDouble(this List<Temperature> T)
        {
            List<double> res = new List<double>();

            for (int i = 0; i < T.Count; i++)
                res.Add(T[i].BaseValue);

            return res;
        }
    }
}