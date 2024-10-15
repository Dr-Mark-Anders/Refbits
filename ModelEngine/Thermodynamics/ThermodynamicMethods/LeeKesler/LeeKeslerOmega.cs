using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public partial class LeeKesler
    {
        public static double Omega(double SG, Temperature MeABP, Temperature TCrit, Pressure PCrit)
        {
            double Tbr = MeABP / TCrit;
            double Pbr = 1 / PCrit;
            double res;

            //res = Math.Log()

            res = (Math.Log(Pbr) - 5.92714 + 6.09648 / Tbr + 1.28862 * Math.Log(Tbr) - 0.169347 * Math.Pow(Tbr, 6))
                / (15.2518 - 15.6875 / Tbr - 13.4721 * Math.Log(Tbr) + 0.43577 * Math.Pow(Tbr, 6));

            /*if (Tbr <= 0.8)
            {
                Kw = MeABP.Rankine.Pow(1f / 3f) / SG;
                res = -7.904 + 0.1352 * Kw - 0.007465 * Kw.Pow(2) + 8.359 * Tbr + (1.408 - 0.01063 * Kw) / Tbr;
            }
            else
            {
                res = (-Math.Log(PCrit / 1.01325) - 5.92714 + 6.09648 / Tbr + 1.28862 * Math.Log(Tbr) - 0.16934 * Tbr.Pow(6))
                    / (15.2518 - 15.6875 / Tbr - 13.4721 * Math.Log(Tbr) + 0.43577 * Tbr.Pow(6));

                if (res > 1.5)
                    res = 1.5;
            }*/

            return res;
        }

        public static double Omega_Old(double SG, Temperature MeABP, Temperature TCrit, double PCrit) // gives -ngtv omegas???
        {
            double A1, A2, A3, B1, B2, B3, B4;
            double K;
            double MW, X, OMEGA;
            double PCritTRank, PCriticalPress;
            double PCritTRank70;
            double MeABP_F = MeABP.Fahrenheit;
            double VapPress;

            K = MeABP.Rankine.Pow(1f / 3f) / SG;

            A1 = (-1171.26 + (23.722 + 24.907 * SG) * K + (1149.82 - 46.535 * K) / SG) / 1000;
            A2 = (1 + 0.82463 * K) * (56.086 - 13.817 / SG) / 1000000;
            A3 = -(1 + 0.82463 * K) * (9.6757 - 2.3653 / SG) / 1000000000;

            if (SG > 0.885)
                B4 = 0;
            else
                B4 = Math.Pow(((12.8 / K - 1) * (1 - 10 / K) * (SG - 0.885) * (SG - 0.7) * 10000), 2);

            B1 = (-356.44 + 29.72 * K + B4 * (295.02 - 248.46 / SG)) / 1000;
            B2 = ((-146.24 + (77.62 - 2.772 * K) * K - B4 * (301.42 - 253.87 / SG)) / 1000000);
            B3 = (-56.487 - 2.95 * B4) / 1000000000;

            MW = 20.486 * (Math.Exp(1.165 / 10000 * (MeABP_F + 460) - 7.78712 * SG + 1.1582 / 1000 *
                (MeABP_F + 460) * SG)) * Math.Pow((MeABP_F + 460), 1.26007) * Math.Pow(SG, 4.98308);

            PCritTRank = TCrit.Rankine;
            PCriticalPress = PCrit;
            PCritTRank70 = PCritTRank * 0.7;

            X = (((MeABP_F + 460) / PCritTRank70 - 0.0002867 * (MeABP_F + 460))) / (748.1 - 0.2145 * (MeABP_F + 460));

            if (X > 0.0022)
            {
                VapPress = Math.Pow(10, (3000.538 * X - 6.76156) / (43 * X - 0.987672));
            }
            else if (X > 0.0013)
            {
                VapPress = Math.Pow(10, (2663.129 * X - 5.994296) / (95.76 * X - 0.972546));
            }
            else
            {
                VapPress = Math.Pow(10, (2770.085 * X - 6.412631) / (36 * X - 0.989679));
            }

            OMEGA = -Math.Log10(VapPress * 14.7 / (PCriticalPress * 760)) - 1;

            return OMEGA;
        }
    }
}