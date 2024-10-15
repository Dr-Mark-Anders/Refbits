using System.Collections.Generic;

namespace Extensions
{
    public static partial class Ext
    {
        public static double[] CumulateArray(this double[] list)
        {
            double[] res = new double[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0)
                    res[i] = list[i];
                else
                    res[i] = res[i - 1] + list[i];
            }

            return res;
        }

        public static List<double> CumulateArray(this List<double> list)
        {
            if (list is null)
                return null;

            List<double> res = new List<double>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                    res.Add(list[i]);
                else
                    res.Add(res[i - 1] + list[i]);
            }

            return res;
        }

        public static double[] DeCumulateArray(this double[] list)
        {
            double[] res = new double[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0)
                    res[i] = list[i];
                else
                    res[i] = list[i] - list[i - 1];
            }

            return res;
        }

        public static double[] DeCumulateArray<T>(this double[] list)
        {
            double[] res = new double[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0)
                    res[i] = list[i];
                else
                    res[i] = list[i] - list[i - 1];
            }
            return res;
        }

        public static List<double> DeCumulateArray<T>(this List<double> list)
        {
            if (list is null)
                return null;

            List<double> res = new List<double>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                    res.Add(list[i]);
                else
                    res.Add(list[i] - list[i - 1]);
            }

            return res;
        }
    }
}