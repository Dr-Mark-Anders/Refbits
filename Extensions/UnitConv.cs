using System.Linq;

namespace Extensions
{
    public static partial class Ext
    {
        public static double F(this double d)
        {
            return d * 1.8 + 32;
        }

        public static double R(this double d)
        {
            return (d + 273.15) * 1.8;
        }

        public static double PSIA(this double d)
        {
            return d * 14.5037738;  //bar to psi
        }

        public static double K(this double d)
        {
            return (d + 273.15);
        }

        public static double KtoC(this double d)
        {
            return (d - 273.15);
        }

        public static double RtoC(this double d)
        {
            return (d * 5 / 9 - 273.15);
        }

        public static double RtoK(this double d)
        {
            return (d * 5 / 9);
        }

        public static double FtoK(this double d)
        {
            return (d - 32) * 5 / 9 + 273.15;
        }

        public static double FtoC(this double d)
        {
            return (d - 32) * 5 / 9;
        }

        public static double CtoK(this double d)
        {
            return (d + 273.15);
        }

        public static double[] KtoC(this double[] d)
        {
            double[] res = new double[d.Length];
            for (int n = 0; n < d.Length; n++)
            {
                res[n] = d[n] - 273.15;
            }
            return res;
        }

        public static double[][] KtoC(this double[][] d)
        {
            double[][] res = new double[d.Length][];

            for (int a = 0; a < d.Length; a++)
            {
                double[] res1 = new double[d[a].Length];

                for (int n = 0; n < d[a].Length; n++)
                {
                    res1[n] = d[a][n] - 273.15;
                }
                res[a] = res1;
            }
            return res;
        }

        public static double[][] CtoK(this double[][] d)
        {
            double[][] res = new double[d.GetUpperBound(0)][];
            for (int a = 0; a < d.Count(); a++)
            {
                double[] res1 = new double[d[a].GetUpperBound(0)];
                for (int n = 0; n < d.Count(); n++)
                {
                    res1[n] = d[a][n] + 273.15;
                }
                res[a] = res1;
            }
            return res;
        }

        public static double[] CtoK(this double[] d)
        {
            double[] res = new double[d.Length];
            for (int n = 0; n < d.Length; n++)
            {
                res[n] = d[n] + 273.15;
            }
            return res;
        }

        public static double BTU_LB_to_KJ_KG(this double d)
        {
            return d * 1.05505585 * 2.20462;
        }
    }
}