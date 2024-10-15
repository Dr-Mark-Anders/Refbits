using Extensions;
using System;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public static class SRK
    {
        public const double Rgas = 8.3144621;
        //public  const double  Rgas_cm3 = 83.14459848;

        public static Pressure PSat(Components cc, Temperature T)
        {
            int NoComps = cc.ComponentList.Count;
            double amix, bmix, Am, Bm;
            double ZMin = 0, ZMax = 0;
            double[] kappa, alpha, ac, a, A, b, B;
            double[,] aij = new double[NoComps, NoComps];
            double[] aimix = new double[NoComps];
            double[,] kijm, lijm;
            double P0 = 1;
            Pressure res = 0;
            Pressure PCritMix;
            Temperature TCritMix;

            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            //Debugger.Launch();

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            b = new double[NoComps];

            CommonRoutines.KaysMixing(cc, cc.MoleFractions, out TCritMix, out PCritMix);

            //Debugger.Launch();

            KappaAlphaSRK(cc,P0, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            amix = CalcAmix(cc, cc.MoleFractions, a, ref aimix, kijm, ref aij);
            bmix = CalcBmix(cc, cc.MoleFractions, b, lijm);

            for (int i = 1; i <= 50; i++)
            {
                Am = amix * P0 / Math.Pow(Rgas * T, 2);
                Bm = P0 * bmix / (Rgas * T);

                CalcCompressibility(Am, Bm, ref ZMin, ref ZMax);

                double fugl = P0 * Math.Exp(LnPhiTotal(ZMin, Am, Bm));
                double fugV = P0 * Math.Exp(LnPhiTotal(ZMax, Am, Bm));

                if (ZMin == ZMax)
                {
                    if (ZMin < 0.2)
                    {
                        P0 /= 2;
                    }
                    else
                    {
                        P0 += 1;
                    }

                    if (P0 > PCritMix)
                    {
                        res = PCritMix / 10;
                        break;
                    }
                }
                else
                {
                    double Pnew = P0 * ((ZMax - ZMin) - Math.Log(fugV / fugl)) / (ZMax - ZMin);

                    if (Math.Abs((Pnew - P0) / P0) < 0.00001)
                    {
                        res = Pnew;
                        break;
                    }
                    else
                    {
                        if (Pnew < 0)
                            P0 /= 2;
                        else
                            P0 = Pnew;
                    }
                }
            }
            return res;
        }

        //not finished
        public static double LnPhiTotal(double Z, double AA, double BB)
        {
            double lnPhi = Math.Log(Math.Pow(Z / (Z + BB), AA / BB) / (Z - BB) * (Math.Exp(Z - 1)));
            return lnPhi;
        }

        public static ThermoProps ThermoBulk(Components cc, Pressure P, Temperature T, double[] X, enumFluidRegion state, double IdealEnthalpy, double IdealEntropy)
        {
            int NoComps = cc.ComponentList.Count; ;
            double amix, bmix, Am, Bm;
            double ZMin = 0, ZMax = 0;
            double Z, f, H, S, G, Ae, U, V, dadt;
            double H_Higm, S_Sigm, G_Gigm;
            double[] kappa, alpha, ac, a, A, b, B;
            double[,] aij = new double[NoComps, NoComps];
            double[] aimix = new double[NoComps];
            double[,] kijm, lijm;

            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            b = new double[NoComps];

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            amix = CalcAmix(cc, X, a, ref aimix, kijm, ref aij);
            bmix = CalcBmix(cc, X, b, lijm);

            Am = amix * P / Math.Pow(Rgas * T, 2);
            Bm = P * bmix / (Rgas * T);

            CalcCompressibility(Am, Bm, ref ZMin, ref ZMax);

            dadt = CalcDADT(cc, X, T, kappa, alpha, aij);

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            H_Higm = ((Z - 1) + 1 / (bmix * Rgas * T) * (amix - T * dadt) * Math.Log(Z / (Z + Bm))) * Rgas * T;
            S_Sigm = Rgas * Math.Log(Z - Bm) - dadt / bmix * Math.Log(Z / (Z + Bm));

            G_Gigm = H_Higm - T * S_Sigm;

            f = P * Math.Exp(G_Gigm / (Rgas * T));
            H = IdealEnthalpy + H_Higm;
            S = IdealEntropy + S_Sigm;
            G = H - T * S;
            V = Z * Rgas * T / P;
            U = H - P * V;
            Ae = U - T * S;

            ThermoProps props = new ThermoProps(P, T, cc.MW(), Z, f, H, S, G, Ae, U, V);

            return props;
        }

        public static Enthalpy H_Hig(Components cc, Pressure P, Temperature T, double[] X, enumFluidRegion state)
        {
            int NoComps = cc.ComponentList.Count; ;
            double amix, bmix, Am, Bm;
            double ZMin = 0, ZMax = 0;
            double Z, dadt;
            double H_Higm;
            double[] kappa, alpha, ac, a, A, b, B;
            double[,] aij = new double[NoComps, NoComps];
            double[] aimix = new double[NoComps];
            double[,] kijm, lijm;

            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            b = new double[NoComps];

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            amix = CalcAmix(cc, X, a, ref aimix, kijm, ref aij);
            bmix = CalcBmix(cc, X, b, lijm);

            Am = amix * P / Math.Pow(Rgas * T, 2);
            Bm = P * bmix / (Rgas * T);

            CalcCompressibility(Am, Bm, ref ZMin, ref ZMax);

            dadt = CalcDADT(cc, X, T, kappa, alpha, aij);

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            H_Higm = ((Z - 1) + 1 / (bmix * Rgas * T) * (amix - T * dadt) * Math.Log(Z / (Z + Bm))) * Rgas * T;

            return H_Higm;
        }

        public static ThermoProps[] ThermoMix(Components cc, Temperature T, Pressure P, enumFluidRegion state, double IdealEnthalpy, double IdealEntropy)
        {
            int NoComps = cc.ComponentList.Count;
            double amix, bmix, Am, Bm;
            double ZMin = 0, ZMax = 0;
            double Z, f, H, S, G, Ae, U, V, dadt;
            double H_Higm, S_Sigm, G_Gigm;
            double[] kappa, alpha, ac, a, A, b, B;
            double[,] aij = new double[NoComps, NoComps];
            double[] aimix = new double[NoComps];

            double[,] kijm, lijm;

            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            b = new double[NoComps];

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            amix = CalcAmix(cc, cc.MoleFractions, a, ref aimix, kijm, ref aij);
            bmix = CalcBmix(cc, cc.MoleFractions, b, lijm);

            Am = amix * P / Math.Pow(Rgas * T, 2);
            Bm = P * bmix / (Rgas * T);

            CalcCompressibility(Am, Bm, ref ZMin, ref ZMax);

            dadt = CalcDADT(cc, cc.MoleFractions, T, kappa, alpha, aij);

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            ThermoProps[] props = new ThermoProps[cc.ComponentList.Count];

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                H_Higm = ((Z - 1) + b[i] / (bmix * Rgas * T) * (amix - T * dadt) * Math.Log(Z / (Z + Bm))) * Rgas * T;
                S_Sigm = b[i] / Bm * (Rgas * Math.Log(Z - Bm) - dadt / bmix * Math.Log(Z / (Z + Bm)));

                G_Gigm = H_Higm - T * S_Sigm;

                f = ((Z / (Z + Bm)).Pow(Am / Bm) / (Z - Bm) * (Math.Exp(Z - 1))); // not finished

                H = IdealEnthalpy + H_Higm;
                S = IdealEntropy + S_Sigm;
                G = H - T * S;
                V = Z * Rgas * T / P;
                U = H - P * V;
                Ae = U - T * S;

                props[i] = new ThermoProps(P, T, cc.MW(), Z, f, H, S, G, Ae, U, V);
            }
            return props;
        }

        public static double[] Kmix(Components cc, Pressure P, Temperature T, double[] X, double[] Y, out enumFluidRegion state)
        {
            double[] results = LnKmix(cc, P, T, X, Y, out state);

            for (int i = 0; i < results.Length; i++)
                results[i] = Math.Exp(results[i]);

            return results;
        }

        public static double[] LnKmix(Components cc, Pressure P, Temperature T, double[] X, double[] Y, out enumFluidRegion state)
        {
            state = enumFluidRegion.Undefined;
            int NoComps = cc.ComponentList.Count;
            double[,] kijm = new double[NoComps, NoComps];
            double[,] lijm = new double[NoComps, NoComps];
            double[,] aij = new double[NoComps, NoComps];
            double[] lnPhiL1inf = new double[NoComps];
            double[] lnPhiV1inf = new double[NoComps];
            double[] kappa, alpha, ac, a, A, b, B, AimxLiq, AimxVap;
            double ZMin = 0, ZMax = 0;

            Temperature TCritMixLiq, TCritMixVap;
            Pressure PCritMixLiq, PCritMixVap;
            enumFluidRegion stateliq, statevap;

            CommonRoutines.KaysMixing(cc, X, out TCritMixLiq, out PCritMixLiq);
            CommonRoutines.KaysMixing(cc, Y, out TCritMixVap, out PCritMixVap);

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            AimxLiq = new double[NoComps];
            AimxVap = new double[NoComps];

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            double AmixLiq = CalcAmix(cc, X, A, ref AimxLiq, kijm, ref aij);
            double BmixLiq = CalcBmix(cc, X, B, lijm);

            CalcCompressibility(AmixLiq, BmixLiq, ref ZMin, ref ZMax);

            stateliq = CommonRoutines.CheckState(ZMin, ZMax, T, TCritMixLiq, P, PCritMixLiq);
            double AmixVap = CalcAmix(cc, Y, A, ref AimxVap, kijm, ref aij);
            double BmixVap = CalcBmix(cc, Y, B, lijm);

            CalcCompressibility(AmixLiq, BmixVap, ref ZMin, ref ZMax);

            statevap = CommonRoutines.CheckState(ZMin, ZMax, T, TCritMixVap, P, PCritMixVap);

            lnPhi(cc, ZMin, AmixLiq, BmixLiq, AimxLiq, B, lnPhiL1inf);
            lnPhi(cc, ZMax, AmixVap, BmixVap, AimxVap, B, lnPhiV1inf);

            double[] results = CommonRoutines.CalculateK(P, stateliq, statevap, ref state, lnPhiL1inf, lnPhiV1inf, X);

            return results;
        }

        public static enumFluidRegion GetFluidState(Components cc, Temperature T, Pressure p)
        {
            enumFluidRegion state;
            int NoComps = cc.ComponentList.Count;
            double[,] kijm = new double[NoComps, NoComps];
            double[,] lijm = new double[NoComps, NoComps];
            double[,] aij = new double[NoComps, NoComps];
            double[] kappa, alpha, ac, a, A, b, B, Aimx;
            double ZMin = 0, ZMax = 0;

            Temperature TCritMix;
            Pressure PCritMix;

            CommonRoutines.KaysMixing(cc, cc.MoleFractions, out TCritMix, out PCritMix);

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            a = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            Aimx = new double[NoComps];

            // convert Pressure   from bar to bar
            double P = p;

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            double Amix = CalcAmix(cc, cc.MoleFractions, a, ref Aimx, kijm, ref aij);
            double Bmix = CalcBmix(cc, cc.MoleFractions, b, lijm);

            CalcCompressibility(Amix, Bmix, ref ZMin, ref ZMax);

            if (ZMin == ZMax) //  single state
            {
                if (T > TCritMix && P > PCritMix) // supercritical
                {
                    state = enumFluidRegion.SuperCritical;
                }
                else if (ZMin > 0.3 && T > TCritMix) // gaseous
                {
                    state = enumFluidRegion.Gaseous;
                }
                else if (P < PCritMix)
                {
                    state = enumFluidRegion.Liquid;
                }
                else // P > Pcrit && T< Tcrit
                {
                    state = enumFluidRegion.CompressibleLiquid;
                }
            }
            else // mixed state
            {
                state = enumFluidRegion.TwoPhase;
            }

            return state;
        }

        public static double[] FugMix(Components cc, Temperature T, Pressure Pstream, enumFluidRegion state)
        {
            int NoComps = cc.ComponentList.Count;
            double[,] kijm = new double[NoComps, NoComps];
            double[,] lijm = new double[NoComps, NoComps];
            double[,] aij = new double[NoComps, NoComps];
            double[] results = new double[NoComps];
            double[] lnPhiMix = new double[NoComps];
            double[] kappa, alpha, a, ac, A, b, B, Aimx;
            double ZMin = 0, ZMax = 0;

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            b = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            Aimx = new double[NoComps];

            // convert Pressure   from bar to bar
            double P = Pstream;

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            double Amix = CalcAmix(cc, cc.MoleFractions, a, ref Aimx, kijm, ref aij);
            double Bmix = CalcBmix(cc, cc.MoleFractions, b, lijm);

            CalcCompressibility(Amix, Bmix, ref ZMin, ref ZMax);

            if (state == enumFluidRegion.Liquid)
            {
                lnPhi(cc, ZMin, Amix, Bmix, Aimx, B, lnPhiMix);
            }
            else
            {
                lnPhi(cc, ZMax, Amix, Bmix, Aimx, B, lnPhiMix);
            }

            return lnPhiMix;
        }

        public static double KmixInfDil(Components cc1, Temperature T, Pressure Pstream, int I_Inf)
        {
            int NoComps = cc1.Count;

            Components cc = cc1.Clone();
            cc1[I_Inf].MoleFraction = 0;
            cc1.NormaliseFractions();

            double[,] kijm = new double[NoComps, NoComps];
            double[,] lijm = new double[NoComps, NoComps];
            double[,] aij = new double[NoComps, NoComps];
            double[] results = new double[NoComps];
            double[] lnPhiL1inf = new double[NoComps];
            double[] lnPhiV1inf = new double[NoComps];
            double[] kappa, alpha, ac, A, a, b, B, Aimx;
            double ZMin = 0, ZMax = 0;

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            b = new double[NoComps];
            a = new double[NoComps];
            ac = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            Aimx = new double[NoComps];

            // convert Pressure   from bar to bar
            double P = Pstream;

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            double Amix = CalcAmix(cc, cc.MoleFractions, a, ref Aimx, kijm, ref aij);
            double Bmix = CalcBmix(cc, cc.MoleFractions, b, lijm);

            CalcCompressibility(Amix, Bmix, ref ZMin, ref ZMax);

            lnPhi(cc, ZMin, Amix, Bmix, Aimx, B, lnPhiL1inf);

            return lnPhiL1inf[I_Inf];
        }

        public static void KappaAlphaSRK(Components cc, Pressure P, Temperature T, ref double[] kappa, ref double[] alpha,
            ref double[] ac, ref double[] a, ref double[] b, ref double[] A, ref double[] B)
        {
            int NoComps;
            BaseComp bc;

            double[,] kijm, lijm;

            NoComps = cc.ComponentList.Count;

            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];

            for (int I = 0; I < NoComps; I++)
            {
                bc = cc[I];
                kappa[I] = 0.48508 + 1.5517 * bc.Omega - 0.15613 * bc.Omega.Pow(2);  //SRK
                alpha[I] = (1 + kappa[I] * (1 - Math.Sqrt(T / bc.CritT))).Pow(2);
                ac[I] = 0.42748 * (Rgas * bc.CritT).Pow(2) / bc.CritP.MPa;
                a[I] = ac[I] * alpha[I];
                A[I] = a[I] * P / Rgas.Pow(2) / T.Pow(2);
                b[I] = 0.08664 * Rgas * bc.CritT / bc.CritP.MPa;
                B[I] = b[I] * P / Rgas / T;
            }
        }

        public static void CalcCompressibility(double Am, double Bm, ref double Zmin, ref double Zmax)
        {
            double a2, a1, a3, q, p, R, Theta, P, Q, m, qpm;
            double Z1, Z2, Z3;

            // Solution to Cubic  Z3 + a2.Z2 + a1.Z + a0 =0

            a1 = -1; // -1 + Bm;
            a2 = Am - Bm - Bm.Pow(2);
            a3 = -Am * Bm;

            p = (3 * a2 - a1.Pow(2)) / 3;
            q = (2 * Math.Pow(a1, 3) - 9 * a1 * a2 + 27 * a3) / 27;

            R = q.Pow(2) / 4 + p.Pow(3) / 27;

            if (R > 0)
            {
                P = (-q / 2 + R.Pow(0.5)).CubeRoot();
                Q = (-q / 2 - R.Pow(0.5)).CubeRoot();

                Zmin = P + Q - a1 / 3;
                Zmax = Zmin;
            }
            else
            {
                m = 2 * (-p / 3).Pow(0.5);
                qpm = 3 * q / p / m;
                Theta = Math.Acos(qpm) / 3;

                Z1 = m * Math.Cos(Theta) + 1 / 3D;
                Z2 = m * Math.Cos(Theta + 4 * Math.PI / 3) + 1 / 3D;
                Z3 = m * Math.Cos(Theta + 2 * Math.PI / 3) + 1 / 3D;

                Zmin = CommonRoutines.minval(Z1, Z2, Z3);
                Zmax = CommonRoutines.maxval(Z1, Z2, Z3);
            }
        }

        public static double CalcAmix(Components cc, double[] XY, double[] A, ref double[] Aimix, double[,] kijm, ref double[,] Aij)
        {
            //
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa
            //
            int n = cc.ComponentList.Count;
            double amix = 0;
            double[,] aij = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        aij[i, j] = A[i];
                    }
                    else
                    {
                        aij[i, j] = (A[i] * A[j]).Pow(0.5) * (1 - kijm[i, j]);
                    }
                }
            }

            BaseComp J, I;
            for (int i = 0; i < n; i++)
            {
                I = cc[i];
                for (int j = 0; j < n; j++)
                {
                    J = cc[j];
                    Aimix[i] += XY[j] * aij[i, j];
                }
                amix += Aimix[i] * XY[i];
            }

            Aij = aij;

            return amix;
        }

        public static double CalcBmix(Components cc, double[] XY, double[] B, double[,] lijm)
        {
            // T is in units of Kelvin; Tc in Kelvin; Pc in Pa

            int n = cc.ComponentList.Count;
            BaseComp I;

            double bmix = 0D;
            for (int i = 0; i < n; i++)
            {
                I = cc[i];
                bmix = bmix + XY[i] * B[i];
            }

            return bmix;
        }

        public static double CalcDADT(Components cc, double[] XY, Temperature T, double[] kappa, double[] alpha, double[,] aij)
        {
            // da/dt =-kappa*a/((1+kappa*(1-Tr^0.5))*(T*Tc)^0.5)

            double dadt = 0;
            double[] dadt_i = new double[aij.Length];
            BaseComp bcI, bcJ;

            for (int I = 0; I < kappa.Length; I++)
            {
                bcI = cc[I];
                for (int J = 0; J < kappa.Length; J++)
                {
                    bcJ = cc[J];
                    dadt_i[I] += -aij[I, J] / (2 * Math.Sqrt(T))
                        * ((kappa[J] / Math.Sqrt(alpha[J] * bcJ.CritT) + kappa[I]
                        / Math.Sqrt(alpha[I] * bcI.CritT))) * XY[J];
                }
                dadt += dadt_i[I] * XY[I];
            }
            return dadt;
        }

        public static double CalcDADTAprrox(Components cc, Temperature T, Temperature TcritKMix, double amix, double kappamix, double alphamix)
        {
            double dadt = 0;

            dadt = -amix * T.Pow(-0.5) * kappamix * TcritKMix.Pow(-0.5) * alphamix.Pow(-0.5);

            return dadt;
        }

        public static void lnPhi(Components cc, double Z, double A, double B, double[] Ai, double[] Bi, double[] lnPhi_i)
        {
            int N = cc.ComponentList.Count;
            double Ai_Am, Bi_Bm;

            for (int i = 0; i < N; i++)
            {
                Ai_Am = Ai[i] / A;
                Bi_Bm = Bi[i] / B;

                //lnPhi_i[i] = Bi_Bm * (Z - 1) - Math.Log(Z - Bmx) - Amx / (2.8284 * Bmx) * (2 * Ai_Am - Bi_Bm) * Math.Log((Z + 2.4142 * Bmx) / (Z - 0.4142 * Bmx)); // PR
                lnPhi_i[i] = (Bi_Bm * (Z - 1) - Math.Log(Z - B) + A / B * (2 * Ai_Am - Bi_Bm) * Math.Log(Z / (Z + B))); // srk
            }
        }

        public static double damixdni_i(Components cc, int i_index, double[] a, double TinK)
        {
            double sumyaij = 0D;
            int n = cc.ComponentList.Count;
            double[,] aij = new double[n, n];
            double R = 8.314;
            BaseComp bc;

            double[,] kijm = new double[cc.ComponentList.Count, cc.ComponentList.Count];

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
                sumyaij = sumyaij + 2D * bc.MoleFraction * aij[i_index, j];
            }

            return sumyaij;
        }

        public static double dbmixdni(Components cc, int i_index)
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
                    bmix = bmix + bci.MoleFraction * bcj.MoleFraction * b[i, j];
                }
            }

            double sumybij = 0;

            for (int j = 0; j < n; j++)
            {
                bcj = cc[j];
                sumybij = sumybij + 2D * bcj.MoleFraction * b[i_index, j];
            }

            double res = -bmix + sumybij;
            return res;
        }

        public static double CalcDADTnew(Components cc, double[] XY, Temperature T, double[] kappa, double[] a, out double[] ai)
        {
            // da/dt =-kappa*a/((1+kappa*(1-Tr^0.5))*(T*Tc)^0.5)
            int Count = cc.ComponentList.Count;
            double dadt = 0;
            double[] dadt_i = new double[Count];
            double[,] dadt_ij = new double[Count, Count];
            ai = new double[Count];
            double CritK;

            BaseComp bcI, bcJ;
            double SQRT_t = Math.Sqrt(T);

            for (int I = 0; I < Count; I++)
            {
                CritK = cc[I].CritT._Kelvin;
                ai[I] = -kappa[I] * a[I] / (1 + kappa[I] * (1 - (T / CritK).Pow(0.5))) / (T * CritK).Pow(0.5);
            }

            for (int I = 0; I < Count; I++)
            {
                bcI = cc[I];
                for (int J = 0; J < kappa.Length; J++)
                {
                    bcJ = cc[J];
                    dadt_ij[I, J] = XY[I] * XY[J] * (1 - 0) * ((a[I] / a[J]).Pow(0.5) * ai[J] + (a[J] / a[I]).Pow(0.5) * ai[I]);
                    dadt_i[I] += dadt_ij[I, J];
                }
                dadt += dadt_i[I] / 2;
            }
            return dadt;
        }

        public static ThermoDifferentialPropsCollection DifferentialProps(Components cc, Pressure P, Temperature T, enumFluidRegion state, double IdealCp)
        {
            int NoComps = cc.ComponentList.Count; ;
            double amix, bmix, Am, Bm;
            double ZMin = 0, ZMax = 0;
            double Z, dadt;
            double[] kappa, alpha, ac, a, A, b, B;
            double[,] aij = new double[NoComps, NoComps];
            double[] aimix = new double[NoComps];
            double[,] kijm, lijm;
            double dp_dv_t = 0;
            double dp_dt_v = 0;
            double dt_dp_v = 0;
            double da_dt_p = 0;
            double db_dt_p = 0;
            double dz_dt_p = 0;
            double dv_dt_p = 0;

            kijm = new double[NoComps, NoComps];
            lijm = new double[NoComps, NoComps];

            kappa = new double[NoComps];
            alpha = new double[NoComps];
            ac = new double[NoComps];
            a = new double[NoComps];
            A = new double[NoComps];
            B = new double[NoComps];
            b = new double[NoComps];

            KappaAlphaSRK(cc, P, T, ref kappa, ref alpha, ref ac, ref a, ref b, ref A, ref B);

            amix = CalcAmix(cc, cc.MoleFractions, a, ref aimix, kijm, ref aij);
            bmix = CalcBmix(cc, cc.MoleFractions, b, lijm);

            Am = amix * P / Math.Pow(Rgas * T, 2);
            Bm = P * bmix / (Rgas * T);

            CalcCompressibility(Am, Bm, ref ZMin, ref ZMax);

            dadt = CalcDADT(cc, cc.MoleFractions, T, kappa, alpha, aij);

            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            double[] ai_d;

            dadt = CalcDADTnew(cc, cc.MoleFractions, T, kappa, a, out ai_d);
            var da2dt2 = CalcDA2DT2(cc, cc.MoleFractions, T, kappa, ac, a, ai_d);
            if (state == enumFluidRegion.Liquid)
                Z = ZMin;
            else
                Z = ZMax;

            double v = Z * Rgas * T / P;

            dp_dv_t = -Rgas * T / (v - bmix).Pow(2) + 2 * amix * (v + bmix) / (v * (v + bmix) + bmix * (v - bmix)).Pow(2);
            dp_dt_v = Rgas / (v - bmix) - dadt / (v * (v + bmix) + Bm * (v - Bm));
            dt_dp_v = 1 / dp_dt_v;
            da_dt_p = P / Rgas.Pow(2) / T.Pow(2) * (dadt - 2 * amix / T);
            db_dt_p = -bmix * P / (Rgas * T * T);
            dz_dt_p = (da_dt_p * (Bm - Z) + db_dt_p * (6 * Bm * Z + 2 * Z - 3 * Bm * Bm - 2 * Bm + Am - (Z * Z))) / (3 * Z * Z + 2 * (Bm - 1) * Z + (Am - 2 * Bm - 3 * Bm * Bm));
            dv_dt_p = Rgas / P * (T * (dz_dt_p) + Z);

            ThermoDifferentialPropsCollection props = new ThermoDifferentialPropsCollection(dp_dv_t, dp_dt_v, dt_dp_v, da_dt_p, db_dt_p, dz_dt_p, dv_dt_p);

            double Cvid = IdealCp - Rgas;
            double Crv = T * da2dt2 / (bmix * Math.Sqrt(8)) * Math.Log((Z + Bm * (1 + Math.Sqrt(2))) / (Z + Bm * (1 - Math.Sqrt(2)))) / 10;
            double Cv = Cvid + Crv;

            double Crp = T * dp_dt_v * dv_dt_p - Rgas + Crv * 10;  // check units rgas etc
            double Cp = IdealCp + Crp / 10;

            double JouleThompson = 1 / Cp * (T * dv_dt_p - v) / 10;

            double SonicVelocity = v * (-Cp / Cv * dp_dv_t).Pow(0.5);
            SonicVelocity = SonicVelocity * SonicVelocity * 1000000 / cc.MW();
            SonicVelocity = SonicVelocity.Pow(0.5) / 100;

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
            BaseComp bcI, bcJ;
            double SQRT_t = Math.Sqrt(T);

            for (int I = 0; I < cc.ComponentList.Count; I++)
            {
                bcI = cc[I];
                ai_dd[I] = kappa[I] * aci[I] * Math.Sqrt(bcI.CritT._Kelvin / T) * (1 + kappa[I]) / 2 / T / bcI.CritT._Kelvin;
            }

            for (int I = 0; I < kappa.Length; I++)
            {
                bcI = cc[I];
                for (int J = 0; J < kappa.Length; J++)
                {
                    bcJ = cc[J];
                    aij_dd[I, J] = XY[I] * XY[J] * (1) * (ai_dd[I] * ai_dd[J] / (ai[I] * ai_dd[J]).Pow(0.5)
                        + ai_dd[I] * ai[J].Pow(0.5) / ai[I].Pow(0.5)
                        + ai_dd[J] * ai[I].Pow(0.5) / ai[J].Pow(0.5)
                        - 1 / 2 * (ai_d[I].Pow(2) * ai[J].Sqr() / ai_dd[I].Pow(3 / 2) + ai_d[J].Pow(2) * ai[I].Pow(0.5) / ai_dd[J].Pow((3 / 2))));
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