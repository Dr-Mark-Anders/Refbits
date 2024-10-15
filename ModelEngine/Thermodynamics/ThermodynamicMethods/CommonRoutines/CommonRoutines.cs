using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public class CommonRoutines
    {
        public static double fv(double[] Z, double[] Kn)
        {
            double fv = 0.5;
            double perturb = 0.01;
            for (int i = 0; i < 500; i++)
            {
                double c = CommonRoutines.C(fv, Z, Kn);
                double c2 = CommonRoutines.C(fv + perturb, Z, Kn);

                double gradient = (c2 - c) / perturb;
                double delta = c / gradient;

                fv -= delta;

                if (i == 500)
                    return double.NaN;

                if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance)
                    return fv;
            }
            return double.NaN;
        }

        public static double C(double fv, double[] Z, double[] Kn)
        {
            double C = 0;

            if (Kn is null)
                return double.NaN;

            double[] X = new double[Kn.Length];
            double[] Y = new double[Kn.Length];

            for (int n = 0; n < Kn.Length; n++)//resetXandY,onlyneedediffvreset
            {
                if (fv == 1 && (Kn[n] - 1.0) == -1)//otherwisefailswhenKn[n]~0
                    X[n] = 0;
                else
                    X[n] = Z[n] / (1 + fv * (Kn[n] - 1.0));//Liquid

                Y[n] = Kn[n] * X[n];

                C += X[n] - Y[n];
            }

            return C;
        }

        public static void UpdateXY(double fv, double[] Kn, double[] Z, ref double[] X, ref double[] Y)
        {
            X = new double[Kn.Length];
            Y = new double[Kn.Length];

            if (Kn is null)
                return;
            else if (fv < 0)
                fv = 0;
            else if (fv > 1)
                fv = 1;

            for (int n = 0; n < Kn.Length; n++)//resetXandY,onlyneedediffvreset
            {
                if (fv == 1 && (Kn[n] - 1.0) == -1)//otherwisefailswhenKn[n]~0
                    X[n] = 0;
                else
                    X[n] = Z[n] / (1 + fv * (Kn[n] - 1.0));//Liquid

                Y[n] = Kn[n] * X[n];
            }

            X = X.Normalise();
            Y = Y.Normalise();
        }

        public static enumFluidRegion CheckState(double Zmin, double Zmax, Temperature T, Temperature Tcrit, Pressure P, Pressure Pcrit)
        {
            enumFluidRegion state;
            double Tr = T / Tcrit;
            double Pr = P / Pcrit;

            if (Zmin == Zmax)//singlestate
            {
                if (T > Tcrit && P > Pcrit)//supercritical
                    state = enumFluidRegion.SuperCritical;
                else if (Pr > 1 && Tr < 1)
                    state = enumFluidRegion.CompressibleLiquid;
                else if (Pr <= 1 && T > Tcrit)//gaseous
                    state = enumFluidRegion.Gaseous;
                else if (Zmin > 0.3 && Tr > 0.2)//vapourphase
                    state = enumFluidRegion.Vapour;
                else if (P < Pcrit)
                    state = enumFluidRegion.Liquid;
                else
                    state = enumFluidRegion.CompressibleLiquid;
            }
            else//mixedstate
                state = enumFluidRegion.TwoPhase;

            return state;
        }

        public static double[] CalculateK(Pressure p, enumFluidRegion stateliq, enumFluidRegion statevap,
        ref enumFluidRegion state, double[] lnPhiL1inf, double[] lnPhiV1inf, double[] X)
        {
            double[] results = new double[lnPhiL1inf.Length];

            switch (stateliq)
            {
                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    switch (statevap)
                    {
                        case enumFluidRegion.Liquid:
                        case enumFluidRegion.CompressibleLiquid:
                            state = enumFluidRegion.Liquid;
                            break;

                        case enumFluidRegion.Vapour:
                        case enumFluidRegion.Gaseous:
                        case enumFluidRegion.SuperCritical:
                        case enumFluidRegion.TwoPhase:
                            state = enumFluidRegion.TwoPhase;
                            break;
                    }
                    break;

                case enumFluidRegion.Vapour:
                case enumFluidRegion.Gaseous:
                    switch (statevap)
                    {
                        case enumFluidRegion.Vapour:
                        case enumFluidRegion.Gaseous:
                            state = enumFluidRegion.Vapour;
                            break;
                    }
                    break;

                case enumFluidRegion.SuperCritical:
                    switch (statevap)
                    {
                        case enumFluidRegion.SuperCritical:
                            state = enumFluidRegion.SuperCritical;
                            break;
                    }
                    break;

                case enumFluidRegion.TwoPhase:
                    state = enumFluidRegion.TwoPhase;
                    break;
            }

            switch (state)
            {
                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    for (int i = 0; i < lnPhiL1inf.Length; i++)
                        results[i] = lnPhiL1inf[i];//+Math.Log(p.BaseValue);//addln(Pressure )tomuliptlyphi->Fugacity
                    break;

                case enumFluidRegion.Vapour:
                case enumFluidRegion.Gaseous:
                    for (int i = 0; i < lnPhiL1inf.Length; i++)
                        results[i] = lnPhiV1inf[i] + Math.Log(p.BaseValue);
                    break;

                case enumFluidRegion.TwoPhase:
                    if (statevap == enumFluidRegion.Liquid)
                        for (int i = 0; i < lnPhiL1inf.Length; i++)
                        {
                            results[i] = lnPhiL1inf[i];//+Math.Log(p.BaseValue);
                            if (X[i] < 1e-10 && results[i] < 1e-30)
                                results[i] = -1e-30;
                        }
                    else
                        for (int i = 0; i < lnPhiL1inf.Length; i++)
                        {
                            results[i] = lnPhiL1inf[i] - lnPhiV1inf[i];
                            if (X[i] < 1e-10 && results[i] < 1e-30)
                                results[i] = -1e-30;
                        }
                    break;
            }
            return results;
        }

        public static double maxval(double a, double b, double c)
        {
            double max0 = a;
            if (b > max0)
                max0 = b;
            if (c > max0)
                max0 = c;

            return max0;
        }

        public static double minval(double a, double b, double c)
        {
            //Eliminatenegativeandzerovalues

            if (a <= 0)
                a = 9.9999E+99;
            if (b <= 0)
                b = 9.9999E+99;
            if (c <= 0)
                c = 9.9999E+99;

            //Selectthesmallestpositivevalue

            double min0 = a;

            //if(b<min0)//bisneverasolution
            //min0=b;
            if (c < min0)
                min0 = c;

            if (b < min0)
                min0 = b;

            return min0;
        }

        public static void KaysMixing(Components cc, double[] XY, out Temperature TCritMix, out Pressure PCritMix)
        {
            TCritMix = new Temperature(0); PCritMix = 0;

            XY = XY.Normalise();

            if (cc.ComponentList.Count == 1)
            {
                TCritMix = cc.ComponentList[0].CritT;
                PCritMix = cc.ComponentList[0].CritP;
                return;
            }

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                TCritMix.BaseValue += cc.ComponentList[i].CritT.BaseValue * XY[i];
                PCritMix.BaseValue += cc.ComponentList[i].CritP.BaseValue * XY[i];
            }
        }
    }
}