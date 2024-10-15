using Extensions;
using System;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public static class PengRobinson
    {
        public const double Rgas = Global.Rgas;
        public const double Sqrt2 = 1.41421356;

        public static Pressure PSat(Components cc, Temperature T, enumPRVariation PR)
        {
            if (double.IsNaN(T))
                return double.NaN;

            int NoComps = cc.ComponentList.Count;
            double amix, bmix;
            double ZMin = 0, ZMax = 0;
            double[] kappa, alpha, ac, a, b;
            double[,] kijm;
            Pressure P = 0.5;
            Pressure res = 0;
            double fugl;
            double fugV;
            double Pnew;

            kijm = InteractionParameters.Kij(cc,T);
            //lijm = new  double [NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];
            double[] Ai, Bi;

            CommonRoutines.KaysMixing(cc, cc.MoleFractions, out _, out Pressure PCritMix);

            for (int i = 1; i <= 50; i++)
            {
                switch (PR)
                {
                    case enumPRVariation.PR76:
                        KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                        break;

                    case enumPRVariation.PR78:
                        KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                        break;

                    case enumPRVariation.PRSV:
                        KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                        break;
                }

                amix = CalcAmix(NoComps, cc.MoleFractions, a, kijm, out _, out _);
                bmix = CalcBmix(NoComps, cc.MoleFractions, b);

                var Amix = amix * P.MPa / Math.Pow(Rgas * T, 2);
                var Bmix = P.MPa * bmix / (Rgas * T);

                CalcCompressibility(Amix, Bmix, ref ZMin, ref ZMax);

                fugl = P * Math.Exp(LnPhiTotal(ZMin, Amix, Bmix));
                fugV = P * Math.Exp(LnPhiTotal(ZMax, Amix, Bmix));

                if (ZMin == ZMax)
                {
                    if (ZMin < 0.2)
                        P /= 2;
                    else
                        P += 1;

                    if (P > PCritMix)
                    {
                        res = PCritMix;
                        break;
                    }
                }
                else
                {
                    Pnew = P * ((ZMax - ZMin) - Math.Log(fugV / fugl)) / (ZMax - ZMin);  // secant

                    if (Math.Abs((Pnew - P) / P) < 0.00001)
                    {
                        res = Pnew / 10;
                        break;
                    }
                    else
                    {
                        if (Pnew < 0)
                            P /= 2;
                        else
                            P = Pnew;
                    }
                }
            }
            return res;
        }

        public static Temperature TSat(Components cc, Pressure P, Temperature T, enumPRVariation PR)
        {
            if (double.IsNaN(P))
                return double.NaN;

            int NoComps = cc.ComponentList.Count;
            double amix, bmix;
            double ZMin = 0, ZMax = 0;
            double[] kappa, alpha, ac, a, b;
            double[,] kijm;
            T = cc.TCritMix() * 0.7;
            Temperature res = 0;
            double fugl;
            double fugV;
            double Tnew;
            double[] Ai, Bi;

            kijm = InteractionParameters.Kij(cc, T);
            //lijm = new  double [NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];

            CommonRoutines.KaysMixing(cc, cc.MoleFractions, out Temperature TCritMix, out Pressure PCritMix);

            for (int i = 1; i <= 50; i++)
            {
                switch (PR)
                {
                    case enumPRVariation.PR76:
                        KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                        break;

                    case enumPRVariation.PR78:
                        KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                        break;

                    case enumPRVariation.PRSV:
                        KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                        break;
                }

                amix = CalcAmix(NoComps, cc.MoleFractions, a, kijm, out _, out _);
                bmix = CalcBmix(NoComps, cc.MoleFractions, b);

                var A = amix * P.MPa / Math.Pow(Rgas * T, 2);
                var B = P.MPa * bmix / (Rgas * T);

                CalcCompressibility(A, B, ref ZMin, ref ZMax);

                fugl = P * Math.Exp(LnPhiTotal(ZMin, A, B));
                fugV = P * Math.Exp(LnPhiTotal(ZMax, A, B));

                if (ZMin == ZMax)
                {
                    if (ZMin < 0.2)
                        T /= 2;
                    else
                        T += 1;

                    if (T > TCritMix)
                    {
                        res = TCritMix;
                        break;
                    }
                }
                else
                {
                    Tnew = T * 1 / ((ZMax - ZMin) - Math.Log(fugV / fugl)) / (ZMax - ZMin);  // secant

                    if (Math.Abs((Tnew - T) / T) < 0.00001)
                    {
                        res = Tnew;
                        break;
                    }
                    else
                    {
                        if (Tnew < 0)
                            T /= 2;
                        else
                            T = Tnew;
                    }
                }
            }
            return res;
        }

        // do enthlapies in Pressure   and Rgas ~ 8.314
        public static ThermoProps ThermoBulk(Components cc, double[] X, Pressure P, Temperature T, enumFluidRegion state,
            double IdealEnthalpy, double IdealEntropy, enumPRVariation PR, ThermoDynamicOptions thermo)
        {
            int NoComps = cc.ComponentList.Count;
            double amix, bmix, A, B;
            double ZMin = 0, ZMax = 0;
            double Z, f, H, S, G, Ae, U, V, dadt;
            double H_Higm, S_Sigm, G_Gigm;
            double[] kappa, alpha, ac, a, b;

            double Tk = T.BaseValue;
            double[] Ai, Bi;

            double[,] kijm;

            if (thermo.UseBIPs)
            {
                kijm = InteractionParameters.Kij(cc,T);
            }
            else
                kijm = new double[NoComps, NoComps];

            //lijm = new  double [NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, Tk, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, Tk, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, Tk, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            amix = CalcAmix(NoComps, X, a, kijm, out double[,] aij, out _);
            bmix = CalcBmix(NoComps, X, b);

            A = amix * P / Math.Pow(Rgas * Tk, 2);
            B = P * bmix / (Rgas * Tk);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            dadt = CalcDADT(cc, X, Tk, kappa, alpha, aij);
            //dadt = CalcDADTApprox(T, cc.TCritMix(), amix, kappa.SumProduct(X), alpha.SumProduct(X));

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            H_Higm = (Rgas * Tk * (Z - 1) + (Tk * dadt - amix) / (2 * Sqrt2 * bmix) * Math.Log((Z + (Sqrt2 + 1) * B) / (Z - (Sqrt2 - 1) * B)));
            S_Sigm = Rgas * Math.Log(Z - B) + dadt / (2 * Sqrt2 * bmix) * Math.Log((Z + (Sqrt2 + 1) * B) / (Z - (Sqrt2 - 1) * B));
            G_Gigm = Math.Log(Z - B) - Math.Log((Z + (1 + Sqrt2) * B) / Z + (1 - Sqrt2) * B) * A / B / Math.Sqrt(8); //LN(Z-B)-LN((Z+(1+2^0.5)*B)/Z+(1-2^0.5)*B)*A/B/8^0.5

            if (double.IsNaN(H_Higm))
            {
                H_Higm = 0;
                S_Sigm = 0;
            }

            f = P * Math.Exp(G_Gigm / (Rgas * Tk));
            H = IdealEnthalpy + H_Higm;
            S = IdealEntropy + S_Sigm;
            G = H - Tk * S;
            V = Z * Rgas * Tk / P;
            U = H - Z * Rgas * Tk;
            Ae = U - Tk * S;

            ThermoProps props = new(P, T, cc.MW(), Z, f, H, S, G, Ae, U, V);
            props.H_ig = IdealEnthalpy;
            props.S_ig = IdealEntropy;
            props.H_higm = H_Higm;
            props.S_sigm = S_Sigm;

            return props;
        }



        // do enthlapies in Pressure   and Rgas ~ 8.314
        public static double H_Hig(Components cc, double[] X, Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            int NoComps = cc.ComponentList.Count;
            double amix, bmix, A, B;
            double ZMin = 0, ZMax = 0;
            double Z, dadt;
            double H_Higm;
            double[] kappa, alpha, ac, a, b;

            double Tk = T.BaseValue;
            double[] Ai, Bi;

            double[,] kijm;

            if (thermo.UseBIPs)
            {
                kijm = InteractionParameters.Kij(cc,T);
            }
            else
                kijm = new double[NoComps, NoComps];

            //lijm = new  double [NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];

            switch (thermo.PRVariation)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, Tk, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, Tk, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, Tk, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            amix = CalcAmix(NoComps, X, a, kijm, out double[,] aij, out _);
            bmix = CalcBmix(NoComps, X, b);

            A = amix * P / Math.Pow(Rgas * Tk, 2);
            B = P * bmix / (Rgas * Tk);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            dadt = CalcDADT(cc, X, Tk, kappa, alpha, aij);
            //dadt = CalcDADTApprox(T, cc.TCritMix(), amix, kappa.SumProduct(X), alpha.SumProduct(X));

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            H_Higm = (Rgas * Tk * (Z - 1) + (Tk * dadt - amix) / (2 * Sqrt2 * bmix) * Math.Log((Z + (Sqrt2 + 1) * B) / (Z - (Sqrt2 - 1) * B)));

            return H_Higm;
        }

        public static double[] LnKmix(Components cc, Pressure P, Temperature T, double[] X, double[] Y, out enumFluidRegion state,
            enumPRVariation PR, ThermoDynamicOptions thermo)
        {
            state = enumFluidRegion.Undefined;
            int n = cc.ComponentList.Count;
            double[,] kijm;

            if (thermo.UseBIPs)
                kijm = InteractionParameters.Kij(cc, T);
            else
                kijm = new double[n, n];

            //double [,] lijm = new  double [NoComps, NoComps];
            double[] results;
            double[] lnPhiL1inf = new double[n];
            double[] lnPhiV1inf = new double[n];
            double[] kappa, alpha, ac, a, b, Ai = new double[n], Bi = new double[n];
            double ZMin = 0, ZMax = 0, Zliq, Zvap;
            enumFluidRegion stateliq, statevap;

            CommonRoutines.KaysMixing(cc, X, out Temperature TCritMixLiq, out Pressure PCritMixLiq);
            CommonRoutines.KaysMixing(cc, Y, out Temperature TCritMixVap, out Pressure PCritMixVap);

            kappa = new double[n];
            alpha = new double[n];
            a = new double[n];
            b = new double[n];
            ac = new double[n];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            //double  amixLiq = CalcAmixnew (cc, X, a, kijm, out double [] sumAi);
            //double A = CalcAmixOld(n, X, Ai, kijm, out _, out double[] sumAi);// * P / (Rgas * T.Kelvin).Pow(2);
            double A = CalcAmix(n, X, Ai, kijm, out _, out double[] sumAi);// * P / (Rgas * T.Kelvin).Pow(2);
            double B = CalcBmix(n, X, Bi);// * P / Rgas / T;

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            stateliq = CommonRoutines.CheckState(ZMin, ZMax, T, TCritMixLiq, P, PCritMixLiq);

            Zliq = ZMin;
            lnPhiL1inf = LnPhi(n, Zliq, A, B, Bi, sumAi);

            double AVap = CalcAmix(n, Y, Ai, kijm, out _, out sumAi);         //  Liquid
            double BVap = CalcBmix(n, Y, Bi);                                 //  Liquid

            CalcCompressibility(AVap, BVap, ref ZMin, ref ZMax);

            statevap = CommonRoutines.CheckState(ZMin, ZMax, T, TCritMixVap, P, PCritMixVap);

            Zvap = ZMax;
            lnPhiV1inf = LnPhi(n, Zvap, AVap, BVap, Bi, sumAi);

            var fugl = P * Math.Exp(LnPhiTotal(Zliq, A, B));
            var fugV = P * Math.Exp(LnPhiTotal(Zvap, A, B));

            results = CommonRoutines.CalculateK(P, stateliq, statevap, ref state, lnPhiL1inf, lnPhiV1inf, X);

            return results;
        }

        public static enumFluidRegion GetFluidState(Components cc, double[] X, Temperature T, Pressure P, enumPRVariation PR, ThermoDynamicOptions thermo)
        {
            enumFluidRegion state;
            int NoComps = cc.ComponentList.Count;
            double[,] kijm = InteractionParameters.Kij(cc, T);
            //double [,] lijm = new  double [NoComps, NoComps];
            double[] kappa, alpha, ac, a, b, Ai, Bi;
            double ZMin = 0, ZMax = 0;
            CommonRoutines.KaysMixing(cc, X, out Temperature TCritMix, out Pressure PCritMix);

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            double Amix = CalcAmix(NoComps, X, a, kijm, out _, out _);
            double Bmix = CalcBmix(NoComps, X, b);

            var A = Amix * P.MPa / Math.Pow(Rgas * T, 2);
            var B = P.MPa * Bmix / (Rgas * T);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            state = CommonRoutines.CheckState(ZMin, ZMax, T, TCritMix, P, PCritMix);

            return state;
        }

        public static double[] LnPhiMix(Components cc, Pressure P, Temperature T, enumFluidRegion state, enumPRVariation PR, ThermoDynamicOptions thermo)
        {
            int n = cc.ComponentList.Count;
            double[,] Kij = InteractionParameters.Kij(cc, T);

            //double [,] lijm = new  double [NoComps, NoComps];
            double[] lnPhiMix = new double[n];
            double[] kappa, alpha, a, ac, b, Ai, Bi;
            double ZMin = 0, ZMax = 0;

            kappa = new double[n];
            alpha = new double[n];
            b = new double[n];
            ac = new double[n];
            a = new double[n];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            double amix = CalcAmix(n, cc.MoleFractions, a, Kij, out _, out double[] sumAi);
            double bmix = CalcBmix(n, cc.MoleFractions, b);

            var A = amix * P / Math.Pow(Rgas * T, 2);
            var B = P * bmix / (Rgas * T);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            if (state == enumFluidRegion.Liquid)
                lnPhiMix = LnPhi(n, ZMin, A, B, b, sumAi);
            else
                lnPhiMix = LnPhi(n, ZMax, A, B, b, sumAi);

            return lnPhiMix;
        }

        public static double[] GibbsMix(Components cc, Pressure Pbar, Temperature T, Enthalpy[] IdealEnthalpy,
            Entropy[] IdealEntropy, enumFluidRegion state, enumPRVariation PR, ThermoDynamicOptions thermo)
        {
            int NoComps = cc.ComponentList.Count;
            double[,] Kij = InteractionParameters.Kij(cc, T);
            //double [,] lijm = new  double [NoComps, NoComps];
            double[] Gibbs_i = new double[NoComps];
            double[] kappa, alpha, a, ac, b, Ai, Bi;
            double ZMin = 0, ZMax = 0;
            double P = Pbar.MPa;

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            double amix = CalcAmix(NoComps, cc.MoleFractions, a, Kij, out double[,] ai_d, out _);
            double bmix = CalcBmix(NoComps, cc.MoleFractions, b);

            var A = amix * P / Math.Pow(Rgas * T, 2);
            var B = P * bmix / (Rgas * T);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);
            double dadt = CalcDADT(cc, cc.MoleFractions, T, kappa, alpha, ai_d);

            if (state == enumFluidRegion.Liquid)
                ComponentGibbs(cc, T, ZMin, B, amix, bmix, b, dadt, IdealEnthalpy, IdealEntropy, Gibbs_i);
            else
                ComponentGibbs(cc, T, ZMax, B, amix, bmix, b, dadt, IdealEnthalpy, IdealEntropy, Gibbs_i);

            return Gibbs_i;
        }

        public static double[] HMix(Components cc, Pressure P, Temperature T, enumFluidRegion state, enumPRVariation PR)
        {
            int NoComps = cc.ComponentList.Count;
            double[,] kijm = InteractionParameters.Kij(cc, T);
            //double [,] lijm = new  double [NoComps, NoComps];
            double[] Gibbs_i = new double[NoComps];
            double[] kappa, alpha, a, ac, b, Ai, Bi;
            double ZMin = 0, ZMax = 0;

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            double amix = CalcAmix(NoComps, cc.MoleFractions, a, kijm, out double[,] ai_d, out _);
            double bmix = CalcBmix(NoComps, cc.MoleFractions, b);

            var A = amix * P / Math.Pow(Rgas * T, 2);
            var B = P * bmix / (Rgas * T);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);
            double dadt = CalcDADT(cc, cc.MoleFractions, T, kappa, a, ai_d);

            if (state == enumFluidRegion.Liquid)
                ComponentEnthalpies(cc, T, ZMin, B, amix, bmix, b, dadt, Gibbs_i);
            else
                ComponentEnthalpies(cc, T, ZMax, B, amix, bmix, b, dadt, Gibbs_i);

            return Gibbs_i;
        }

        public static double[] SMix(Components cc, Pressure Pp, Temperature T, enumFluidRegion state, enumPRVariation PR)
        {
            int NoComps = cc.ComponentList.Count;
            double[,] kijm = InteractionParameters.Kij(cc, T);
            //double [,] lijm = new  double [NoComps, NoComps];
            double[] Gibbs_i = new double[NoComps];
            double[] kappa, alpha, a, ac, b, Ai, Bi;
            double ZMin = 0, ZMax = 0;

            double P = Pp.MPa;

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            double amix = CalcAmix(NoComps, cc.MoleFractions, a, kijm, out double[,] ai_d, out _);
            double bmix = CalcBmix(NoComps, cc.MoleFractions, b);

            var A = amix * P / Math.Pow(Rgas * T, 2);
            var B = P * bmix / (Rgas * T);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);
            double dadt = CalcDADT(cc, cc.MoleFractions, T, kappa, a, ai_d);

            if (state == enumFluidRegion.Liquid)
                ComponentEntropies(cc, ZMin, B, bmix, b, dadt, Gibbs_i);
            else
                ComponentEntropies(cc, ZMax, B, bmix, b, dadt, Gibbs_i);

            return Gibbs_i;
        }

        public static double LnPhiTotal(double Z, double AA, double BB)
        {
            double S2 = Math.Pow(2, 0.5);
            double lnPhi = (Z - 1) - Math.Log(Z - BB) - AA / (2 * S2 * BB) * Math.Log((Z + (1 + S2) * BB) / (Z + (1 - S2) * BB));
            return lnPhi;
        }

        public static double KmixInfDil(Components cc1, Temperature T, Pressure P, int I_Inf, enumPRVariation PR)
        {
            int n = cc1.Count;

            Components cc = cc1.Clone();
            cc1[I_Inf].MoleFraction = 0;
            cc1.NormaliseFractions();

            double[,] kijm = InteractionParameters.Kij(cc, T);
            //double [,] lijm = new  double [NoComps, NoComps];
            double[] lnPhiL1inf = new double[n];
            double[] kappa, alpha, ac, a, b, Ai = null, Bi = null;
            double ZMin = 0, ZMax = 0;

            kappa = new double[n];
            alpha = new double[n];
            b = new double[n];
            a = new double[n];
            ac = new double[n];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            double A = CalcAmix(n, cc.MoleFractions, Ai, kijm, out _, out double[] sumYA);
            double B = CalcBmix(n, cc.MoleFractions, Bi);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            lnPhiL1inf = LnPhi(n, ZMin, A, B, b, sumYA);

            return lnPhiL1inf[I_Inf];
        }

        public static void KappaAlphaPR76(Components cc, Pressure P, Temperature T, ref double[] kappa, ref double[] alpha,
            ref double[] ac, ref double[] a, ref double[] b, out double[] A, out double[] B)
        {
            int NoComps;
            NoComps = cc.ComponentList.Count;
            BaseComp bc;

            A = new double[NoComps];
            B = new double[NoComps];

            double Tk = T._Kelvin;

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];

            for (int I = 0; I < NoComps; I++)
            {
                bc = cc[I];
                kappa[I] = 0.37464 + 1.54226 * bc.Omega - 0.26992 * bc.Omega * bc.Omega;
                alpha[I] = (1 + kappa[I] * (1 - Math.Sqrt(Tk / bc.CritT.Kelvin))).Sqr();
                ac[I] = 0.4572355289 * (Rgas * bc.CritT.Kelvin).Sqr() / bc.CritP.MPa;
                a[I] = ac[I] * alpha[I];
                b[I] = 0.077796074 * Rgas * bc.CritT.Kelvin / bc.CritP.MPa;
                A[I] = a[I] * P / Math.Pow(Rgas, 2) / Math.Pow(T, 2);
                B[I] = b[I] * P / Rgas / T;
            }
        }

        public static void KappaAlphaPR78(Components cc, Pressure P, Temperature T, ref double[] kappa, ref double[] alpha,
            ref double[] ac, ref double[] a, ref double[] b, out double[] A, out double[] B)
        {
            int NoComps;
            BaseComp bc;

            double Tk = T._Kelvin;
            NoComps = cc.ComponentList.Count;
            double T2 = T.Kelvin * T.Kelvin;
            double RGas2 = Rgas * Rgas;

            A = new double[NoComps];
            B = new double[NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];

            double P_RGas2_T2 = P / RGas2 / T2;
            double P_RGas_T = P / Rgas / T;

            for (int I = 0; I < NoComps; I++)
            {
                bc = cc.ComponentList[I];
                if (bc.Omega <= 0.491)
                    kappa[I] = 0.37464 + 1.54226 * bc.Omega - 0.26992 * bc.Omega.Pow(2);
                else
                    kappa[I] = 0.379642 + 1.48503 * bc.Omega - 0.164423 * bc.Omega.Pow(2) + 0.016666 * bc.Omega.Pow(3);

                alpha[I] = (1 + kappa[I] * (1 - Math.Sqrt(Tk / bc.CritT.Kelvin))).Pow(2);
                ac[I] = 0.4572355289 * (Rgas * bc.CritT._Kelvin).Pow(2) / bc.CritP;
                a[I] = ac[I] * alpha[I];
                b[I] = 0.077796074 * Rgas * bc.CritT._Kelvin / bc.CritP;
                A[I] = a[I] * P_RGas2_T2;
                B[I] = b[I] * P_RGas_T;
            }
        }

        public static void KappaAlphaPRSV(Components cc, Pressure P, Temperature T, ref double[] kappa, ref double[] alpha,
            ref double[] ac, ref double[] a, ref double[] b, out double[] A, out double[] B)
        {
            int NoComps;
            BaseComp bc;

            double Tk = T._Kelvin;
            NoComps = cc.ComponentList.Count;

            A = new double[NoComps];
            B = new double[NoComps];

            kappa = new double[NoComps];
            double[] kappa0 = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            double Tr;

            for (int I = 0; I < NoComps; I++)
            {
                bc = cc[I];
                Tr = Tk / bc.CritT.Kelvin;
                var K1 = 1;
                kappa0[I] = 0.378893 + 1.4897153 * bc.Omega - 0.17131848 * bc.Omega.Pow(2) + 0.0196554 * bc.Omega.Pow(3);
                kappa[I] = kappa0[I] + K1 * (1 + Math.Sqrt(Tr)) * (0.7 - Tr);

                //kappa[I] = 0.37464 + 1.54226 * bc.Omega - 0.26992 * bc.Omega * bc.Omega; // PR76

                alpha[I] = (1 + kappa[I] * (1 - Math.Sqrt(Tk / bc.CritT))).Pow(2);
                ac[I] = 0.4572355289 * (Rgas * bc.CritT.Kelvin).Pow(2) / bc.CritP.MPa;
                a[I] = ac[I] * alpha[I];
                b[I] = 0.077796074 * Rgas * bc.CritT.Kelvin / bc.CritP.MPa;
                A[I] = a[I] * P / Math.Pow(Rgas, 2) / Math.Pow(T, 2);
                B[I] = b[I] * P / Rgas / T;
            }
        }

        public static void CalcCompressibility(double Am, double Bm, ref double Zmin, ref double Zmax)
        {
            double a1, a2, a0, q, p, R, Theta, P, Q, m, qpm;
            double Z1, Z2, Z3;

            // Solution to Cubic  Z3 + a2.Z2 + a1.Z + a0 =0

            a2 = -1 + Bm;
            a1 = Am - 3 * Bm.Pow(2) - 2 * Bm;  //=A-3*B^2-2*B
            a0 = -Am * Bm + Bm.Pow(2) + Bm.Pow(3);

            p = (3 * a1 - a2.Pow(2)) / 3;
            q = (2 * Math.Pow(a2, 3) - 9 * a2 * a1 + 27 * a0) / 27;

            R = q.Pow(2) / 4 + p.Pow(3) / 27;

            if (R > 0)
            {
                P = (-q / 2 + Math.Sqrt(R)).CubeRoot();
                Q = (-q / 2 - Math.Sqrt(R)).CubeRoot();

                Zmin = P + Q - a2 / 3;
                Zmax = Zmin;
            }
            else
            {
                m = 2 * Math.Sqrt(-p / 3);
                qpm = 3 * q / p / m;
                Theta = Math.Acos(qpm) / 3;

                Z1 = m * Math.Cos(Theta) - a2 / 3;
                Z2 = m * Math.Cos(Theta + 4 * Math.PI / 3) - a2 / 3;
                Z3 = m * Math.Cos(Theta + 2 * Math.PI / 3) - a2 / 3;

                Zmin = CommonRoutines.minval(Z1, Z2, Z3);   // Smallest Positive Root
                Zmax = CommonRoutines.maxval(Z1, Z2, Z3);

                if (Zmax > 1) // supercitical
                    Zmin = Zmax;
            }
        }

        public static double CalcAmixOld(int n, double[] Z, double[] ai, double[,] kijm, out double[,] Aij, out double[] sumAi)
        {
            // Debug.Print (": CalcAmix");
            //
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa
            //
            double amix = 0;
            Aij = new double[n, n];
            sumAi = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        Aij[i, j] = ai[i];
                    else if (i > j)
                        Aij[i, j] = Aij[j, i];
                    else
                        Aij[i, j] = Math.Sqrt(ai[i] * ai[j]) * (1 - kijm[i, j]);
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    sumAi[i] += Z[j] * Aij[i, j];

                amix += sumAi[i] * Z[i];
            }

            return amix;
        }

        /// <summary>
        /// 3a
        /// </summary>
        /// <param name="n"></param>
        /// <param name="XY"></param>
        /// <param name="ai"></param>
        /// <param name="kijm"></param>
        /// <param name="Aij"></param>
        /// <param name="sumAi"></param>
        /// <return  s></return  s>
        public static unsafe double CalcAmix(int n, double[] XY, double[] Ai, double[,] kijm, out double[,] Aij, out double[] sumAi)
        {
            double amix = 0;
            sumAi = new double[n];
            Aij = new double[n, n];

            fixed (double* ptr = Aij, aiPtr = Ai, kijmPtr = kijm, XYPtr = XY)
            {
                double value;
                double* elementIJ = ptr;
                double* kijelement = kijmPtr;
                double* aielement = aiPtr;
                double* JI = ptr;
                double* XYelement = XYPtr;

                for (int i = 0; i < n; i++)
                {
                    var Aii = Ai[i];
                    double sum = XY[i] * Aii;

                    for (int j = 0; j < n; j++)
                    {
                        if (i > j)
                        {
                            value = Math.Sqrt(Aii * Ai[j]) * (1 - *kijelement);
                            *(JI + i * n + j) = value;
                        }
                        else
                        {
                            value = Math.Sqrt(Aii * *aielement) * (1 - *kijelement);
                            *elementIJ = value;
                        }

                        sum += *XYelement * value;

                        aielement++;
                        kijelement++;
                        elementIJ++;
                        XYelement++;
                    }

                    XYelement = XYPtr;
                    aielement = aiPtr;

                    sum -= XY[i] * Aij[i, i];
                    Aij[i, i] = Aii;

                    sumAi[i] = sum;
                    amix += sum * XY[i];
                }
            }

            return amix;
        }

        public static double CalcBmix(int n, double[] XY, double[] B)
        {
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa

            double bmix = 0;
            for (int i = 0; i < n; i++)
                bmix += XY[i] * B[i];

            return bmix;
        }

        public static double CalcDADT(Components cc, double[] XY, double Tk, double[] kappa, double[] alpha, double[,] aij)
        {
            // da/dt =-kappa*a/((1+kappa*(1-Tr^0.5))*(T*Tc)^0.5)

            double dadt = 0;
            double[] dadt_i = new double[aij.Length];
            double bcI_TK, bcJ_Tk;
            double SQRT_t = Math.Sqrt(Tk);
            double AlphaI, KappaI;

            for (int i = 0; i < kappa.Length; i++)
            {
                bcI_TK = cc.ComponentList[i].CritT._Kelvin;
                for (int J = 0; J < kappa.Length; J++)
                {
                    AlphaI = alpha[i];
                    KappaI = kappa[i];
                    bcJ_Tk = cc.ComponentList[J].CritT._Kelvin;
                    dadt_i[i] += -aij[i, J] / (2 * SQRT_t) * (kappa[J] / Math.Sqrt(alpha[J] * bcJ_Tk)
                        + KappaI / Math.Sqrt(AlphaI * bcI_TK)) * XY[J];
                }
                dadt += dadt_i[i] * XY[i];
            }
            return dadt;
        }

        public static double CalcDADTApprox(Temperature T, double TcritKMix, double amix, double kappamix, double alphamix)
        {
            double dadt; // da/dt =-kappa*a/((1+kappa*(1-Tr^0.5))*(T*Tc)^0.5)

            dadt = -amix * T.Pow(-0.5) * kappamix * TcritKMix.Pow(-0.5) * alphamix.Pow(-0.5);

            return dadt;
        }

        public static double[] LnPhi(int n, double Z, double A, double B, double[] bi, double[] Amatrix)
        {
            double Bi_Bm;
            double[] lnPhi_i = new double[n];

            //=EXP(Bi_1/B*(Zl-1)-LN(Zl-B)-A/B/2.8284*LN((Zl+2.4142*B)/(Zl-0.4142*B))*(2*Q28/A-Bi_1/B))

            for (int i = 0; i < n; i++)  //=P*EXP(bi/bm*(Zm-1)-LN(Zm-B)-A/(2.8284*B)*(2*V31/am-bi/bm)*LN((Zm+2.4142*B)/(Zm-0.4142*B)))
            {
                Bi_Bm = bi[i] / B;
                lnPhi_i[i] = (Bi_Bm * (Z - 1) - Math.Log(Z - B) - A / (2.8284 * B) * (2 * Amatrix[i] / A - Bi_Bm) * Math.Log((Z + 2.4142 * B) / (Z - 0.4142 * B)));
            }
            return lnPhi_i;
        }

        public static void ComponentEnthalpies(Components cc, Temperature T, double Z, double B, double Am, double Bm, double[] Bi,
          double dadt, double[] Enthlp)
        {
            int N = cc.ComponentList.Count;

            for (int i = 0; i < N; i++)
            {
                double Bi_Bm = Bi[i] / Bm;
                Enthlp[i] = (Rgas * T * Bi_Bm / Bm * (Z - 1) + (T * dadt - Am) / (2.8284 * Bm) * Math.Log((Z + 2.4142 * B) / (Z - 0.4142 * B)));
            }
        }

        public static void ComponentEntropies(Components cc, double Z, double B, double Bm, double[] Bi, double dadt, double[] Entrop)
        {
            int N = cc.ComponentList.Count;

            for (int i = 0; i < N; i++)
            {
                double Bi_Bm = Bi[i] / Bm;
                Entrop[i] = Bi_Bm / Bm * (Rgas * Math.Log(Z - B) + dadt / (2.8284 * Bm) * Math.Log((Z + 2.4142 * B) / (Z - 0.4142 * B)));
            }
        }

        public static void ComponentGibbs(Components cc, Temperature T, double Z, double B, double Am, double Bm, double[] Bi,
             double dadt, Enthalpy[] enthalpies, Entropy[] entropies, double[] Gibbs_i)
        {
            int N = cc.ComponentList.Count;
            double EnthDep, EntDep;

            for (int i = 0; i < N; i++)
            {
                double Bi_Bm = Bi[i] / Bm;
                EnthDep = (Rgas * T * Bi_Bm / Bm * (Z - 1) + (T * dadt - Am) / (2.8284 * Bm) * Math.Log((Z + 2.4142 * B) / (Z - 0.4142 * B)));
                EntDep = Bi_Bm / Bm * (Rgas * Math.Log(Z - B) + dadt / (2.8284 * Bm) * Math.Log((Z + 2.4142 * B) / (Z - 0.4142 * B)));
                Gibbs_i[i] = enthalpies[i] + EnthDep - T * (entropies[i] + EntDep);
            }
        }

        public static double Damixdni_i(Components cc, Temperature T, int i_index, double[] a)
        {
            double sumyaij = 0D;
            int n = cc.ComponentList.Count;
            double[,] aij = new double[n, n];
            double R = 8.314;
            BaseComp bc;

            double[,] kijm = InteractionParameters.Kij(cc, T);

            for (int i = 0; i < n; i++)
            {
                bc = cc[i];
                aij[i, i] = 0.45724 * R.Pow(2) * bc.CritT.Pow(2) / bc.CritP * a[i];
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i != j)
                    {
                        aij[i, j] = (1D - kijm[i, j]) * Math.Sqrt(aij[i, i] * aij[j, j]);
                        aij[j, i] = aij[i, j];
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                bc = cc[j];
                sumyaij += 2D * bc.MoleFraction * aij[i_index, j];
            }

            return sumyaij;
        }

        public static double Dbmixdni(Components cc, int i_index)
        {
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa

            double bmix = 0D;
            int n = cc.ComponentList.Count;
            double[,] b = new double[n, n];
            double R = 8.314;
            BaseComp bc, bci, bcj;

            double[,] lijm = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                bc = cc[i];
                b[i, i] = 0.0778 * R * bc.CritT / bc.CritP;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i != j)
                    {
                        b[i, j] = (1D - lijm[i, j]) * (b[i, i] + b[j, j]) * 0.5;
                        b[j, i] = b[i, j];
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                bci = cc[i];
                for (int j = 0; j < n; j++)
                {
                    bcj = cc[j];
                    bmix += bci.MoleFraction * bcj.MoleFraction * b[i, j];
                }
            }

            double sumybij = 0;

            for (int j = 0; j < n; j++)
            {
                bcj = cc[j];
                sumybij += 2D * bcj.MoleFraction * b[i_index, j];
            }

            double res = -bmix + sumybij;
            return res;
        }

        public static double CalcDADTnew(Components cc, double[] XY, Temperature T, double[] kappa, double[] a, out double[] ai)
        {
            int Count = cc.ComponentList.Count;
            double dadt = 0;
            double[] dadt_i = new double[Count];
            double[,] dadt_ij = new double[Count, Count];
            ai = new double[Count];
            double CritK;

            for (int I = 0; I < Count; I++)
            {
                CritK = cc.ComponentList[I].CritT._Kelvin;
                ai[I] = -kappa[I] * a[I] / (1 + kappa[I] * (1 - (T / CritK).Pow(0.5))) / (T * CritK).Pow(0.5);
            }

            for (int I = 0; I < Count; I++)
            {
                for (int J = 0; J < kappa.Length; J++)
                {
                    dadt_ij[I, J] = XY[I] * XY[J] * (1 - 0) * ((a[I] / a[J]).Pow(0.5) * ai[J] + (a[J] / a[I]).Pow(0.5) * ai[I]);
                    dadt_i[I] += dadt_ij[I, J];
                }
                dadt += dadt_i[I] / 2;
            }
            return dadt;
        }

        public static ThermoDifferentialPropsCollection DifferentialProps(Components cc, Pressure P, Temperature T,
            enumFluidRegion state, double IdealCp, enumPRVariation PR, ThermoDynamicOptions thermo)
        {
            int NoComps = cc.ComponentList.Count;
            double amix, bmix;
            double ZMin = 0, ZMax = 0;
            double Z, dadt, da2dt2;

            double[] MoleFracs;

            switch (state)
            {
                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    MoleFracs = cc.LiqPhaseMolFractions;
                    break;
                case enumFluidRegion.Vapour:
                case enumFluidRegion.Gaseous:
                case enumFluidRegion.SuperCritical:
                    MoleFracs = cc.VapPhaseMolFractions;
                    break;
                default:
                    MoleFracs = cc.MoleFractions;
                    break;
            }
           

            double[] kappa, alpha, ac, a, b, Ai, Bi;

            double dp_dv_t;
            double dp_dt_v;
            double dt_dp_v;
            double da_dt_p;
            double db_dt_p;
            double dz_dt_p;
            double dv_dt_p;

            double[,] kijm;

            kijm = InteractionParameters.Kij(cc, T);
            //lijm = new  double [NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];

            switch (PR)
            {
                case enumPRVariation.PR76:
                    KappaAlphaPR76(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PR78:
                    KappaAlphaPR78(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;

                case enumPRVariation.PRSV:
                    KappaAlphaPRSV(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, out Ai, out Bi);
                    break;
            }

            amix = CalcAmix(NoComps, MoleFracs, a, kijm, out _, out _);
            bmix = CalcBmix(NoComps, MoleFracs, b);

            var A = amix * P.MPa / Math.Pow(Rgas * T, 2);
            var B = P.MPa * bmix / (Rgas * T);

            CalcCompressibility(A, B, ref ZMin, ref ZMax);

            dadt = CalcDADTnew(cc, MoleFracs, T, kappa, a, out double[] ai_d);
            da2dt2 = CalcDA2DT2(cc, MoleFracs, T, kappa, ac, a, ai_d);

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            dp_dv_t = -Rgas * T / ((double)(Z * Rgas * T / P) - bmix).Pow(2) + 2 * amix * ((double)(Z * Rgas * T / P) + bmix) / ((double)(Z * Rgas * T / P) * ((double)(Z * Rgas * T / P) + bmix) + bmix * ((double)(Z * Rgas * T / P) - bmix)).Pow(2);
            dp_dt_v = Rgas / ((double)(Z * Rgas * T / P) - bmix) - dadt / ((double)(Z * Rgas * T / P) * ((double)(Z * Rgas * T / P) + bmix) + B * ((double)(Z * Rgas * T / P) - B));
            dt_dp_v = 1 / dp_dt_v;
            da_dt_p = P / Rgas.Pow(2) / T.Pow(2) * (dadt - 2 * amix / T);
            db_dt_p = -bmix * P / (Rgas * T * T);
            dz_dt_p = (da_dt_p * (B - Z) + db_dt_p * (6 * B * Z + 2 * Z - 3 * B * B - 2 * B + A - (Z * Z))) / (3 * Z * Z + 2 * (B - 1) * Z + (A - 2 * B - 3 * B * B));
            dv_dt_p = Rgas / P * (T * (dz_dt_p) + Z);

            ThermoDifferentialPropsCollection props = new(dp_dv_t, dp_dt_v, dt_dp_v, da_dt_p, db_dt_p, dz_dt_p, dv_dt_p);

            double Cvid = IdealCp - Rgas;
            double Crv = T * da2dt2 / (bmix * Math.Sqrt(8)) * Math.Log((Z + B * (1 + Math.Sqrt(2))) / (Z + B * (1 - Math.Sqrt(2)))) / 10;
            double Cv = Cvid + Crv;

            double Crp = T * dp_dt_v * dv_dt_p - Rgas + Crv;  // check units rgas etc
            double Cp = IdealCp + Crp;

            double JouleThompson = 1 / Cp * (T * dv_dt_p - (double)(Z * Rgas * T / P));

            double SonicVelocity = (double)(Z * Rgas * T / P) * (-Cp / Cv * dp_dv_t).Pow(0.5); // Note dp_dv_t is in Pressure   not bar so * 10
            SonicVelocity = SonicVelocity * SonicVelocity * 1000 / cc.MW();
            SonicVelocity = SonicVelocity.Pow(0.5);

            props.Cp = Cp;
            props.Cv = Cv;
            props.Cp_Cv = Cp / Cv;
            props.SonicVelocity = SonicVelocity;
            props.JouleThompson = JouleThompson;

            return props;
        }

        public static double CalcDA2DT2(Components cc, double[] XY, Temperature T, double[] kappa, double[] aci, double[] ai, double[] ai_d)
        {
            // da/dt =-kappa*a/((1+kappa*(1-Tr^0.5))*(T*Tc)^0.5)

            double da2dt2 = 0;
            double[] ai_dd = new double[cc.ComponentList.Count];
            double[,] aij_dd = new double[cc.ComponentList.Count, cc.ComponentList.Count];
            BaseComp bcI;

            for (int I = 0; I < cc.ComponentList.Count; I++)
            {
                bcI = cc.ComponentList[I];
                ai_dd[I] = kappa[I] * aci[I] * Math.Sqrt(bcI.CritT._Kelvin / T._Kelvin) * (1 + kappa[I]) / (2 * T._Kelvin) / bcI.CritT._Kelvin;
            }

            for (int I = 0; I < kappa.Length; I++)
            {
                for (int J = 0; J < kappa.Length; J++)
                {
                    aij_dd[I, J] = XY[I] * XY[J] * (1) * (ai_dd[I] * ai_dd[J] / Math.Sqrt(ai[I] * ai_dd[J])
                        + ai_dd[I] * Math.Sqrt(ai[J]) / Math.Sqrt(ai[I])
                        + ai_dd[J] * Math.Sqrt(ai[I]) / Math.Sqrt(ai[J])
                        - 1 / 2 * (ai_d[I].Pow(2) * ai[J].Sqr() / ai_dd[I].Pow(1.5) + ai_d[J].Pow(2) * Math.Sqrt(ai[I]) / ai_dd[J].Pow(1.5)));
                }
            }

            for (int I = 0; I < kappa.Length; I++)
            {
                for (int J = 0; J < kappa.Length; J++)
                {
                    da2dt2 += aij_dd[I, J];
                }
            }

            return da2dt2 / 2;
        }
    }
}