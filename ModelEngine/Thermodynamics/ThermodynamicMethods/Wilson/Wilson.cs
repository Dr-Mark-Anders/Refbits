using System;
using Units.UOM;

namespace ModelEngine
{
    public static class Wilson
    {
        public static double K(Temperature Tcrit, Pressure Pcrit, double w, Temperature T, Pressure P)
        {
            return (Pcrit / P) + 5.373 * (1D + w) * (1D - Tcrit / T);
        }

        public static double lnK(Temperature Tcrit, Pressure Pcrit, double w, Temperature T, Pressure P)
        {
            return Math.Log(Pcrit / P) + 5.373 * (1D + w) * (1D - Tcrit / T);
        }

        public static double[] K(Components cc, Pressure P, Temperature T)
        {
            double[] res = new double[cc.ComponentList.Count];
            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                res[i] = cc[i].CritP / P * Math.Exp(5.373 * (1D + cc[i].Omega) * (1D - cc[i].CritT / T));
            }
            return res;
        }

        public static double[] LnK(Components cc, Pressure P, Temperature T)
        {
            double[] res = new double[cc.ComponentList.Count];
            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                res[i] = Math.Log(cc[i].CritP / P) + 5.373 * (1D + cc[i].Omega) * (1D - cc[i].CritT / T);
            }
            return res;
        }
    }
}