using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    //  Estimate V from table, solve with bounded upper and lower V for Z

    public static partial class LeeKesler
    {
        public static Pressure PSat(Components bcc, Temperature T)
        {
            Pressure res = 0;
            res += Psat(T, bcc.SG(), bcc.VolAveBP(), bcc.TCritMix(), bcc.PCritMix(), bcc.OmegaMix());

            return res;
        }

        // Lee Kesler, from original paper
        public static Pressure Psat(Temperature T, double SG, Temperature bp, Temperature CritT, Pressure CritP, double Omega)
        {
            double f0, f1, Tr, PR;
            double Kw = bp.Rankine.Pow(1f / 3f) / SG;

            Tr = T / CritT;
            f0 = 5.92714 - 6.09648 / Tr - 1.28862 * Math.Log(Tr) + 0.169347 * Math.Pow(Tr, 6);
            f1 = 15.2518 - 15.6875 / Tr - 13.4721 * Math.Log(Tr) + 0.43577 * Math.Pow(Tr, 6);
            PR = Math.Exp(f0 + Omega * f1); // bar

            return PR * CritP;
        }

        // Lee Kesler, from original paper
        public static double[] K(Components cc, Pressure P, Temperature T)
        {
            BaseComp bc;

            double[] res = new double[cc.ComponentList.Count];

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                bc = cc[i];
                res[i] = Psat(T, bc.SG_60F, bc.BP, bc.CritT, bc.CritP, bc.Omega) / P;
            }
            return res;
        }

        // Lee Kesler, from original paper
        public static double[] lnK(Components cc,Pressure P, Temperature T)
        {
            double[] res = K(cc,P, T);

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                res[i] = Math.Log(res[i]);
            }

            return res;
        }

        // from API
        /*public  static Pressure   VP2(Temperature  t, Temperature  MeABP, double  Kw) // Lee Kesler (tested ok)
        {
            double  x, VP;

            x = (MeABP.R / t - 0.0002867 * MeABP.R) / (748.1 - 0.2145 * MeABP.R);

            if (x > 0.0022)
            {
                VP = (3000.538 * x - 6.76156) / (43 * x - 0.987672);
            }
            else if ((x <= 0.0022) && (x > 0.0013))
            {
                VP = (2663.129 * x - 5.994296) / (95.76 * x - 0.972546);
            }
            else
            {
                VP = (2770.085 * x - 6.412631) / (36 * x - 0.989679);
            }
            //return   Math.Pow(10,VP); // mmHG
            return   Math.Pow(10, VP) / 750.0615613;  // Bar
        }*/
    }
}