using System;
using Units.UOM;

namespace ModelEngine
{
    public class GraysonStreed
    {
        public static Pressure PSat(Components cc, Temperature T)
        {
            int NoComps = cc.ComponentList.Count;
            double oldsum = 0;
            double delta = 0.1;
            bool crossedover = false;
            Pressure P = 1;

            KaysMixingRules.KaysMixing(cc, out _, out _);

            for (int i = 0; i <= 1000; i++)
            {
                double[] K = GetKReal(cc, cc.MoleFractions,P, T);

                double SumK = 0;

                for (int ii = 0; ii < NoComps; ii++)
                {
                    SumK += K[ii] * cc[ii].MoleFraction;
                }

                if (Math.Abs(SumK - 1) > 0.0001) // not finished
                {
                    if (SumK > 1)
                        P += delta;
                    else if (SumK < 1)
                        P -= delta;

                    if (P <= 0)
                        return -999;
                }
                else
                    return P; // converged

                if (oldsum > 1 && SumK < 1 || oldsum < 1 && SumK > 1)
                {
                    crossedover = true;
                }

                oldsum = SumK;

                if (crossedover)
                {
                    delta = delta / 2D;
                    crossedover = false;
                }
            }
            return -999;
        }

        public static double[] GetKReal(Components comps, double[] X,Pressure P, Temperature T)
        {
            // Compute real K (vaporization equilibirum ratio, K=y/x) values using   the Chao-Seader
            // technique
            int NumComp = comps.Count;
            BaseComp bc;
            double[] EqK = new double[NumComp];

            /*if(comps.LiquidComponents is null)
            {
                comps.UpdateLiqVapFractionCompositions(comps.Q);
            }*/

            double RT;
            double Pr;                                  // Reduced Pressure  , reduced Pressure   squared
            double Term;                                // int ermediate calculations
            double[] FLLog = new double[NumComp];       // Natural log of liquid fugacity coefficient
            double[] GamLog = new double[NumComp];      // Natural log of gamma (liquid activity coefficient)
            double VolMix;                              // Volume of mixture (molar)
            double SolAvg;                              // Average solubility parameter for solutio

            // Calculate Z (vapor phase compressibility factor) using   Redlich and Kwong EOS
            double[] FVLog = RK.LnFugMix(comps, P, T, enumFluidRegion.Vapour);       // Natural log of vapor fugacity coefficient

            VolMix = 0;
            SolAvg = 0;
            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                Term = X[I] * bc.LiquidMolarVolumeCM3();
                VolMix = VolMix + Term;
                SolAvg = SolAvg + Term * bc.HildebrandSolPar();
            }
            SolAvg = SolAvg / VolMix;

            RT = 8.3145 * T;  // Units?

            for (int I = 0; I < NumComp; I++)
            {
                bc = comps[I];
                Pr = P / bc.CritP.BarA;

                GamLog[I] = (bc.LiquidMolarVolumeCM3() / RT) * Math.Pow((bc.HildebrandSolPar() - SolAvg), 2); // Calc solublity parameters

                Term = GetLiqFug(bc, T, P);

                FLLog[I] = Math.Log(Term);

                EqK[I] = Math.Exp(FLLog[I] + GamLog[I] - FVLog[I]) / Pr;
            }
            return EqK;
        }

        public static double GetLiqFug(BaseComp bc, Temperature T, Pressure P)
        {
            double Tr = T / bc.CritT;
            double Pr = P / bc.CritP;
            double Tr2 = Tr * Tr;
            double Tr3 = Tr2 * Tr;
            double Pr2 = Pr * Pr;
            double Term, res;

            if (bc.MW < 3) // H2
            {
                Term = 1.50709 + 2.74283 / Tr - 0.0211 * Tr + 0.00011 * Tr2 + 0.008585 * Pr;
            }
            else if (bc.MW > 16 && bc.MW < 16.2) //CH4
            {
                Term = 2.4384 - 1.54831 / Tr - 0 * Tr + 0.02889 * Tr2 - 0.01076 * Tr3 + Pr * (0.10486 - 0.02529 * Tr);
            }
            else
            {
                Term = 2.05135 - 2.10899 / Tr - 0.19396 * Tr2 + 0.02282 * Tr3 + Pr * (0.08852 - 0.00872 * Tr2)
                       + Pr2 * (-0.00353 + 0.00203 * Tr);  // - Math.Log(Pr, 10);

                Term += bc.Omega * (-4.23893 + 8.65808 * Tr - 1.2206 / Tr - 3.15224 * Tr2 - 0.025 * (Pr - 0.6));
            }

            res = Term * 2.3025851; // convert base 10 to base e

            res = Math.Exp(res);

            return res;
        }
    }
}