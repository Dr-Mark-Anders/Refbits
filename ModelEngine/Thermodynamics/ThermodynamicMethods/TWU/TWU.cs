using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public class TWU
    {
        public static double Tc, Pc, Omega, Zc, MW;
        private static double Tco, Pco, Vco, SGo, a, MWGuess, beta, beta2;
        private static double DSGt, Ft, DSGv, Fv, DSGp, Fp, LnM, Fm, DSGm, x;
        private static double TError, Terror2;
        private static double Vc;

        public static double VcOut { get => Vc / 100; }

        public static void Calc(Temperature Tboil, double SG)
        {
            double Tbguess, TbGuess2;
            int count = 0;
            double Gradiant;
            double Tbr, Kw;
            Temperature Tb = new Temperature();

            if (Tboil.Celsius > 825)
                Tb.Celsius = 825;
            else
                Tb.Kelvin = Tboil.Kelvin;

            Tco = Tb * (0.533272 + 0.34383 * 0.001 * Tb + 2.52617 * 0.0000001 * Math.Pow(Tb, 2) - 1.658481 * 0.0000000001 * Tb.Pow(3D) + 4.60773 * (1E+24) * Tb.Pow(-13D)).Pow(-1);
            a = 1 - Tb / Tco;
            Pco = (1.0661 + 0.31412 * Math.Sqrt(a) + 9.16106 * a + 9.5041 * a.Pow(2) + 27.35886 * a.Pow(4)).Pow(2);
            Vco = (0.34602 + 0.30171 * a + 0.93307 * a.Pow(3) + 5655.414 * a.Pow(14)).Pow(-8);
            SGo = 0.843592 - 0.128624 * a - 3.36159 * a.Pow(3) - 13749.5 * a.Pow(12);
            MWGuess = Tb / (5.8 - 0.0052 * Tb);
            if (MWGuess > 1500)
                MWGuess = 1200;

            do
            {
                beta = Math.Log(MWGuess);
                Tbguess = Math.Exp(5.1264 + 2.71579 * beta - 0.28659 * beta.Pow(2) - 39.8544 / beta - 0.122488 / beta.Pow(2)) - 13.7512 * beta + 19.6197 * beta.Pow(2);
                TError = Tbguess - Tb.Kelvin;
                beta2 = Math.Log(MWGuess + 1);
                TbGuess2 = Math.Exp(5.1264 + 2.71579 * beta2 - 0.28659 * beta2.Pow(2) - 39.8544 / beta2 - 0.122488 / beta2.Pow(2)) - 13.7512 * beta2 + 19.6197 * beta2.Pow(2);
                Terror2 = TbGuess2 - Tb.Kelvin;
                Gradiant = Terror2 - TError;

                MWGuess = MWGuess - TError / Gradiant;
                count++;
            } while (Math.Abs(TError) > 0.01 && count < 100);

            DSGt = Math.Exp(5 * (SGo - SG)) - 1;
            Ft = DSGt * (-0.27016 / Math.Sqrt(Tb) + (0.0398285 - 0.706691 / Math.Sqrt(Tb)) * DSGt);
            Tc = Tco * ((1 + 2 * Ft) / (1 - 2 * Ft)).Pow(2);
            DSGv = Math.Exp(4 * (SGo.Pow(2) - SG.Pow(2))) - 1;
            Fv = DSGv * (0.347776 / Math.Sqrt(Tb) + (-0.182421 + 2.248896 / Math.Sqrt(Tb)) * DSGv);
            Vc = Vco * ((1 + 2 * Fv) / (1 - 2 * Fv)).Pow(2);
            DSGp = Math.Exp(0.5 * (SGo - SG)) - 1;
            Fp = DSGp * ((2.53262 - 34.4321 / Math.Sqrt(Tb.Kelvin) - 2.30193 * Tb.Kelvin / 1000) + (-11.4277 + 187.934 / Math.Sqrt(Tb.Kelvin) + 4.11963 * Tb.Kelvin / 1000) * DSGp);
            Pc = Pco * (Tc / Tco) * Vco / Vc * ((1 + 2 * Fp) / (1 - 2 * Fp)).Pow(2);
            DSGm = Math.Exp(5 * (SGo - SG)) - 1;
            x = (0.012342 - 0.244541 / Math.Sqrt(Tb));
            Fm = DSGm * (x + (-0.0175691 + 0.143979 / Math.Sqrt(Tb)) * DSGm);
            LnM = Math.Log(MWGuess) * ((1 + 2 * Fm) / (1 - 2 * Fm)).Pow(2);
            MW = Math.Exp(LnM);

            Tbr = Tb / Tc;

            if (Tbr <= 0.8)
            {
                Kw = Tb.Rankine.Pow(1f / 3f) / SG;
                Omega = -7.904 + 0.1352 * Kw - 0.007465 * Kw.Pow(2) + 8.359 * Tbr + (1.408 - 0.01063 * Kw) / Tbr;
            }
            else
            {
                Omega = (-Math.Log(Pc / 1.01325) - 5.92714 + 6.09648 / Tbr + 1.28862 * Math.Log(Tbr) - 0.16934 * Tbr.Pow(6))
                    / (15.2518 - 15.6875 / Tbr - 13.4721 * Math.Log(Tbr) + 0.43577 * Tbr.Pow(6));

                if (Omega > 1.5)
                    Omega = 1.5;
            }

            Zc = 0.2905 - 0.085 * Omega;
        }
    }
}