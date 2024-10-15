using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    //  Estimate V from table, solve with bounded upper and lower V for Z

    public static partial class LeeKesler
    {
        private static class Corr
        {
            public const double b1 = 0.2026579, b2 = 0.331511, b3 = 0.027655, b4 = 0.203488;
            public const double beta = 1.226;
            public const double c1 = 0.0313385, c2 = 0.0503618, c3 = 0.016901, c4 = 0.041577;
            public const double d1 = 4.8736e-5, d2 = 7.4336e-6;
            public const double gamma = 0.03754;
        }

        private static class Simple
        {
            public const double b1 = 0.1181193, b2 = 0.265728, b3 = 0.154790, b4 = 0.030323;
            public const double beta = 0.65392;
            public const double c1 = 0.0236744, c2 = 0.0186984, c3 = 0, c4 = 0.042724;
            public const double d1 = 1.55488e-5, d2 = 6.23689e-5;
            public const double gamma = 0.060167;
        }

        private static void LK_UpdateZ(Components bcc)
        {
            BaseComp bc;
            double omeg;
            for (int i = 0; i < bcc.ComponentList.Count; i++)
            {
                bc = bcc[i];
                if (bc.Omega > 1) // limit the size of omega or LeeKesler fails (negative Z).
                {
                    omeg = 1;
                }
                else
                {
                    omeg = bc.Omega;
                }

                bc.LK_Z = 0.2905 - 0.085 * omeg;
            }
        }

        private static void LK_UpdateCriticalVolumes(Components bcc)
        {
            BaseComp bc;
            for (int i = 0; i < bcc.ComponentList.Count; i++)
            {
                bc = bcc[i];
                bc.LK_Vc = bc.LK_Z * bc.CritT * ThermodynamicsClass.Rgas / bc.CritP;
            }
        }

        public static double MixVolumes(Components cc, double[] X)
        {
            int NoComp = cc.Count;
            BaseComp J, K;
            double res = 0;

            if (cc.Count == 1)
            {
                return cc[0].LK_Vc;
            }
            else
            {
                for (int j = 0; j < NoComp; j++)
                    for (int k = 0; k < NoComp; k++)
                    {
                        J = cc[j];
                        K = cc[k];
                        res += 1D / 8D * X[j] * X[k] *
                            (J.LK_Vc.Pow(1f / 3f) + K.LK_Vc.Pow(1f / 3f)).Pow(3);
                    }
            }
            return res;
        }

        public static Temperature MixCritT(Components cc, double[] X, double MixedVolumes)
        {
            int NoComp = cc.Count;
            BaseComp J, K;
            Temperature res = new Temperature(0);

            for (int j = 0; j < NoComp; j++)
                for (int k = 0; k < NoComp; k++)
                {
                    J = cc[j];
                    K = cc[k];
                    res += 1 / (8 * MixedVolumes) * X[j] * X[k] *
                        (J.LK_Vc.Pow(1f / 3f) + K.LK_Vc.Pow(1f / 3f)).Pow(3)
                        * (J.CritT * K.CritT).Pow(0.5);
                }

            return res;
        }

        public static double MixOmega(Components o, double[] x)
        {
            int NoComp = o.Count;
            double res = 0;

            for (int j = 0; j < NoComp; j++)
            {
                res += x[j] * o[j].Omega;
            }
            return res;
        }

        public static double MixCritP(double OmegaMix, Temperature TCriMix, double VCritMix)
        {
            Pressure res;
            res = (0.2905 - 0.085 * OmegaMix) * ThermodynamicsClass.Rgas * TCriMix.Kelvin / VCritMix;
            return res;
        }

        /// <summary>
        /// Z is int erpolated unless outside of table area, then rigorous
        /// </summary>
        /// <param name="Tr"></param>
        /// <param name="Pr"></param>
        /// <param name="Omega"></param>
        /// <param name="ss"></param>
        /// <param name="cres"></param>
        /// <return  s></return  s>
        public static double EnthDep0(double Z, double Tr, double Pr)
        {
            double res, E, Vr;

            Vr = Z * Tr / Pr;

            E = Simple.c4 / (2 * Math.Pow(Tr, 3) * Simple.gamma) * (Simple.beta + 1 - (Simple.beta + 1 + Simple.gamma / Vr / Vr)
                * Math.Exp(-Simple.gamma / Vr / Vr));

            res = Tr * (Z - 1 - (Simple.b2 + 2 * Simple.b3 / Tr + 3 * Simple.b4 / Tr / Tr) / Tr / Vr
                - (Simple.c2 - 3 * Simple.c3 / Tr / Tr) / (2 * Tr * Vr * Vr)
                + Simple.d2 / (5 * Tr * Math.Pow(Vr, 5)) + 3 * E);

            return res;
        }

        public static double EnthDep1(double Z, double Tr, double Pr)
        {
            double res, E, Vr;

            Vr = Z * Tr / Pr;

            E = Corr.c4 / (2 * Math.Pow(Tr, 3) * Corr.gamma) * (Corr.beta + 1 - (Corr.beta + 1 + Corr.gamma / Vr / Vr) *
                Math.Exp(-Corr.gamma / Vr / Vr));

            res = Tr * (Z - 1 - (Corr.b2 + 2 * Corr.b3 / Tr + 3 * Corr.b4 / Tr / Tr) / Tr / Vr
                - (Corr.c2 - 3 * Corr.c3 / Tr / Tr) / (2 * Tr * Vr * Vr)
                + Corr.d2 / (5 * Tr * Math.Pow(Vr, 5)) + 3 * E);

            return res;
        }

        public static double SDep0(double Z, double Tr, double Pr)
        {
            double res, E, Vr;

            Vr = Z * Tr / Pr;

            E = Simple.c4 / (2 * Math.Pow(Tr, 3) * Simple.gamma) * (Simple.beta + 1 - (Simple.beta + 1 + Simple.gamma / Vr / Vr) *
                Math.Exp(-Simple.gamma / Vr / Vr));

            res = Math.Log(Z) - (Simple.b1 + Simple.b3 / Tr.Pow(2) + 2 * Simple.b4 / Tr.Pow(3)) / Vr -
                (Simple.c1 - 2 * Simple.c3 / Tr.Pow(3)) / (2 * Vr.Pow(2)) - Simple.d1 / (5 * Vr.Pow(5)) + 2 * E;

            return res;
        }

        public static double SDep1(double Z, double Tr, double Pr)
        {
            double res, E, Vr;

            Vr = Z * Tr / Pr;

            E = Corr.c4 / (2 * Math.Pow(Tr, 3) * Corr.gamma) * (Corr.beta + 1 - (Corr.beta + 1 + Corr.gamma / Vr / Vr) *
                Math.Exp(-Corr.gamma / Vr / Vr));

            res = Math.Log(Z) - (Corr.b1 + Corr.b3 / Tr.Pow(2) + 2 * Corr.b4 / Tr.Pow(3)) / Vr -
                (Corr.c1 - 2 * Corr.c3 / Tr.Pow(3)) / (2 * Vr.Pow(2)) - Corr.d1 / (5 * Vr.Pow(5)) + 2 * E;

            return res;
        }

        private static double ErrorCorr(double Vr, double Tr, double Pr, double B, double C, double D, out double LeftSide)
        {
            double RightSide, Err;

            LeftSide = Pr * Vr / Tr;

            RightSide = 1 + B / Math.Pow(Vr, 1) + C / Math.Pow(Vr, 2) + D / Math.Pow(Vr, 5) + Corr.c4
                / Math.Pow(Tr, 3) / Math.Pow(Vr, 2) * (Corr.beta + Corr.gamma / Math.Pow(Vr, 2)) * Math.Exp(-Corr.gamma / Vr / Vr);

            Err = LeftSide - RightSide;

            return Err;
        }

        private static double ErrorSimple(double Vr, double Tr, double Pr, double B, double C, double D, out double LeftSide)
        {
            double RightSide, Err;

            LeftSide = Pr * Vr / Tr;

            RightSide = 1 + B / Math.Pow(Vr, 1.0) + C / Math.Pow(Vr, 2.0) + D / Math.Pow(Vr, 5.0) + Simple.c4
                / Math.Pow(Tr, 3.0) / Math.Pow(Vr, 2.0) * (Simple.beta + Simple.gamma / Math.Pow(Vr, 2.0)) * Math.Exp(-Simple.gamma / Vr / Vr);

            Err = LeftSide - RightSide;

            return Err;
        }
    }
}