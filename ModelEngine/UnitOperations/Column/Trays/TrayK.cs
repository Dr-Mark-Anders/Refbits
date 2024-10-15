using Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Units.UOM;

namespace ModelEngine
{
    public partial class Tray
    {
        public MABPMethod KB_MA = new MABPMethod(1);
        public LogLinearMethod LLinear = new(1);
        public LinearMethod Linear = new(1);
        public TraySection Owner;

        public double[] K
        {
            get
            {
                if (LiqComposition != null)
                {
                    double[] res = new double[LiqComposition.Length];
                    for (int i = 0; i < res.Length; i++)
                        res[i] = K_TestFast(i, T, column.SolverOptions.KMethod);

                    return res;
                }
                return null;
            }
        }

        public void UpdateKAvgt()
        {
            KB_MA.KAvg = Math.Exp(KB_MA.lnKA - KB_MA.lnKB / T);
        }

        public bool UpdateAlphas(Components cc, double[] X, double[] Y, ColumnAlphaMethod method, ThermoDynamicOptions thermo)
        {
            int NoComps = LiqComposition.Length;

            if (LnKCompOld == null)
                LnKCompOld = new double[LiqComposition.Length];
            else
                LnKCompOld = LLinear.LnKComp;

            switch (method)
            {
                case ColumnAlphaMethod.Linear:
                    Linear.KCompGrad = new double[LiqComposition.Length];
                    Linear.KCompA = ThermodynamicsClass.KMixArray(cc, P.BaseValue, T, X, Y, out _, thermo);
                    Linear.KCompA1 = ThermodynamicsClass.KMixArray(cc, P.BaseValue, T + Linear.DeltaT, X, Y, out _, thermo);
                    for (int i = 0; i < Linear.KCompA.Length; i++)
                    {
                        Linear.KCompGrad[i] = (Linear.KCompA1[i] - Linear.KCompA[i]) / Linear.DeltaT;
                    }
                    Linear.BaseT = T;
                    KTray = Linear.KCompA;
                    break;

                case ColumnAlphaMethod.LogLinear:
                    LLinear.BaseT = T;

                    if (TrayEff != 100)
                    {
                        LLinear.LnKComp = ThermodynamicsClass.LnKMixWithEfficiency(cc, P.BaseValue, T, X, Y, out _, thermo, TrayEff);
                        if (LLinear.LnKComp is null)
                        {
                            Debugger.Break();
                            return false;
                        }
                        LLinear.LnKComp2 = ThermodynamicsClass.LnKMixWithEfficiency(cc, P.BaseValue, T + LLinear.DeltaT, X, Y, out _, thermo, TrayEff);
                        if (LLinear.LnKComp2 is null)
                            return false;
                    }
                    else
                    {
                        LLinear.LnKComp = ThermodynamicsClass.LnKMixArray(cc, P.BaseValue, T, X, Y, out _, thermo);
                        if (LLinear.LnKComp is null)
                            return false;
                        LLinear.LnKComp2 = ThermodynamicsClass.LnKMixArray(cc, P.BaseValue, T + LLinear.DeltaT, X, Y, out _, thermo);
                        if (LLinear.LnKComp2 is null)
                            return false;
                    }

                    LLinear.lnKCompGrad = new double[cc.Count];

                    for (int CompNo = 0; CompNo < NoComps; CompNo++)
                    {
                        LLinear.lnKCompGrad[CompNo] = (LLinear.LnKComp2[CompNo] - LLinear.LnKComp[CompNo]) / LLinear.DeltaT;
                        KTray[CompNo] = Ext.exp0(LLinear.LnKComp[CompNo]);
                        if (KTray[CompNo] > 1e15)
                            KTray[CompNo] = 1e15;
                    }

                    break;

                case ColumnAlphaMethod.BostonMethod:
                    break;

                case ColumnAlphaMethod.Rigorous:
                    break;

                case ColumnAlphaMethod.MA:
                    KB_MA.BaseT = T;

                    double[] K1 = ThermodynamicsClass.KMixArray(cc, P.BaseValue, T, X, Y, out _, thermo);
                    double[] K2 = ThermodynamicsClass.KMixArray(cc, P.BaseValue, T + KB_MA.DeltaT, X, Y, out _, thermo);
                    double K1AVG = K1.Mult(X).Sum();
                    double K2AVG = K2.Mult(X).Sum();
                    double lnKBase = Math.Log(K1AVG);
                    double lnK2 = Math.Log(K2AVG);
                    double[] k1Ratio = new double[X.Length];
                    double[] k2Ratio = new double[X.Length];

                    KB_MA.KAvg = K1AVG;
                    KB_MA.lnKB = (lnKBase - lnK2) / (1 / (T + KB_MA.DeltaT) - 1 / T);
                    KB_MA.lnKA = lnKBase + KB_MA.lnKB / T;

                    KB_MA.KCompGrad = new double[X.Length];
                    KB_MA.KCompA = new double[X.Length];

                    for (int i = 0; i < X.Length; i++)
                    {
                        k1Ratio[i] = K1[i] / K1AVG;
                        k2Ratio[i] = K2[i] / K2AVG;
                        KB_MA.KCompGrad[i] = (k2Ratio[i] - k1Ratio[i]) / (T + KB_MA.DeltaT - T);
                        KB_MA.KCompA[i] = k1Ratio[i] - KB_MA.KCompGrad[i] * T;
                    }

                    KB_MA.CompRatio = k1Ratio;

                    KTray = K1;
                    return true;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double EstimateT(Components cc, double[] X, double[] Y, ColumnTestimateMethod method)
        {
            double res = 0, oldres;
            switch (method)
            {
                case ColumnTestimateMethod.MA:
                    double k = 0;
                    double KavgT = Math.Exp(KB_MA.lnKA - KB_MA.lnKB / (T + KB_MA.DeltaT));

                    for (int i = 0; i < X.Length; i++)
                    {
                        k += X[i] * KB_MA.K_SimpleRatio(i, T + KB_MA.DeltaT, KavgT);
                    }

                    double LogK = Math.Log(KB_MA.KAvg);
                    double LogKt = Math.Log(k);

                    double Grad = (LogKt - LogK) / KB_MA.DeltaT;
                    double A = -LogK / Grad;

                    res = T + A;
                    if (res < 0)
                        TPredicted = 10;
                    else
                        TPredicted = res;
                    break;

                case ColumnTestimateMethod.LinearEstimate2ValuesLooped:
                    {
                        Pressure P = this.P.BaseValue;
                        var Base = 0D;
                        var Base1 = 0D;
                        int Count = 0;

                        do
                        {
                            for (int i = 0; i < X.Length; i++)
                            {
                                //Base += K_TestFast(i, T, column.SolverOptions.KMethod) * X[i];
                                Base += KTray[i] * X[i];
                                Base1 += K_TestFast(i, T - LLinear.DeltaT, column.SolverOptions.KMethod) * X[i];
                            }

                            var PlnKBase = Math.Log(Base);
                            var PlnKBase1 = Math.Log(Base1);

                            var pb = (PlnKBase - PlnKBase1) / LLinear.DeltaT;
                            var pa = PlnKBase - pb * T;

                            oldres = res;
                            res = -pa / pb;
                            T = res;
                            Count++;
                        } while (res - oldres > 0.01 || Count > 10 || res < 0);

                        if (res < 0)
                            TPredicted = 10;
                        else
                            TPredicted = res;
                    }
                    break;

                case ColumnTestimateMethod.LinearEstimate2Values:
                    {
                        //Parallel.For(0, section.Trays.Count, trayNo => // initialise liquid rates, calculate from assumed vapour rates;
                        Pressure P = this.P.BaseValue;
                        var Base = 0D;
                        var Base1 = 0D;

                        for (int i = 0; i < X.Length; i++)
                        {
                            //Base += K_TestFast(i, T, ColumnKMethod.LogLinear) * X[i];
                            Base += KTray[i] * X[i];
                            Base1 += K_TestFast(i, T - LLinear.DeltaT, ColumnKMethod.LogLinear) * X[i];
                        }

                        var PlnKBase = Math.Log(Base);
                        var PlnKBase1 = Math.Log(Base1);

                        var pb = (PlnKBase - PlnKBase1) / LLinear.DeltaT;
                        var pa = PlnKBase - pb * T;

                        res = -pa / pb;
                        if (res < 0)
                            TPredicted = 10;
                        else
                            TPredicted = res;
                    }
                    break;

                case ColumnTestimateMethod.Rigorous:
                    {
                        Tray tray = this;
                        Pressure P = tray.P.BaseValue;
                        var Tk = tray.T;

                        var PKBase = 0D;
                        var PKBase1 = 0D;
                        var lnKBase2 = 0D;

                        double[] lnKmixBase = ThermodynamicsClass.LnKMixArray(cc, P, Tk, tray.LiqCompositionPred, tray.VapCompositionPred, out enumFluidRegion state, column.Thermo);
                        double[] lnKmixLower = ThermodynamicsClass.LnKMixArray(cc, P, Tk - RigDeltaT, tray.LiqCompositionPred, tray.VapCompositionPred, out state, column.Thermo);
                        double[] lnKmixUpper = ThermodynamicsClass.LnKMixArray(cc, P, Tk + RigDeltaT, tray.LiqCompositionPred, tray.VapCompositionPred, out state, column.Thermo);

                        for (int i = 0; i < tray.LiqComposition.Length; i++)
                        {
                            PKBase += Math.Exp(lnKmixBase[i]) * tray.LiqCompositionPred[i];
                            PKBase1 += Math.Exp(lnKmixLower[i]) * tray.LiqCompositionPred[i];
                            lnKBase2 += Math.Exp(lnKmixUpper[i]) * tray.LiqCompositionPred[i];
                        }

                        var b = (Math.Log(lnKBase2) - Math.Log(PKBase1)) / (2 * LLinear.DeltaT);
                        var a = Math.Log(PKBase) - b * Tk;
                        tray.TPredicted = -a / b;
                    }
                    break;

                case ColumnTestimateMethod.Rigorous2:
                    {
                        Tray tray = this;
                        Pressure P = tray.P.BaseValue;
                        var Tk = tray.T;

                        var PKBase = 0D;
                        var PKBase1 = 0D;

                        double[] lnKmixBase = ThermodynamicsClass.LnKMixArray(cc, P, Tk, tray.LiqCompositionPred, tray.VapCompositionPred, out enumFluidRegion state, column.Thermo);
                        double[] lnKmixLower = ThermodynamicsClass.LnKMixArray(cc, P, Tk - RigDeltaT, tray.LiqCompositionPred, tray.VapCompositionPred, out state, column.Thermo);

                        for (int i = 0; i < LiqComposition.Length; i++)
                        {
                            PKBase += Math.Exp(lnKmixBase[i]) * tray.LiqCompositionPred[i];
                            PKBase1 += Math.Exp(lnKmixLower[i]) * tray.LiqCompositionPred[i];
                        }

                        var b = (Math.Log(PKBase) - Math.Log(PKBase1)) / RigDeltaT;
                        var a = Math.Log(PKBase) - b * Tk;
                        tray.TPredicted = -a / b;
                    }
                    break;

                case ColumnTestimateMethod.BostonMethod:
                    break;
            }

            return TPredicted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double EstimateTFast(Components cc, double[] X, double[] Y, ColumnKMethod method)
        {
            double res = 0;
            double Base = 0D;
            double Base1 = 0D;

            for (int i = 0; i < X.Length; i++)
            {
                //Base += KTray[i] * X[i];
                Base += K_TestFast(i, T, method) * X[i];
                Base1 += K_TestFast(i, T + LLinear.DeltaT, method) * X[i];
            }

            double PlnKBase = Math.Log(Base);
            double PlnKBase1 = Math.Log(Base1);

            double pb = (PlnKBase1 - PlnKBase) / LLinear.DeltaT;
            double pa = PlnKBase - pb * T;

            res = -pa / pb;
            TPredicted = res;

            return res;
        }

        /// <summary>
        /// Should Calculate bubble point  exactly
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="method"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EstimateTFast2(Components cc, double[] X, double[] Y, ColumnTestimateMethod method)
        {
            //Parallel.For(0, section.Trays.Count, trayNo => // initialise liquid rates, calculate from assumed vapour rates;
            Pressure P = this.P.BaseValue;
            var Base = 0D;
            var Base1 = 0D;
            double Test = T;

            do
            {
                Base = ThermodynamicsClass.KMix(cc, P, Test, X, Y, out _, cc.Thermo);
                Base1 = ThermodynamicsClass.KMix(cc, P, Test + 1, X, Y, out _, cc.Thermo);

                var PlnKBase = Math.Log(Base);
                var PlnKBase1 = Math.Log(Base1);

                var pb = -(PlnKBase - PlnKBase1) / LLinear.DeltaT;
                var pa = PlnKBase - pb * Test;

                Test = -pa / pb;

                if (Test < 10)
                    Test = 10;
            } while (Math.Abs(Base - 1) > 0.01);

            TPredicted = Test;
        }

        public double[] KBModel_Boston(double[] Y, double[] lnK1, double[] lnK1Delta, double[] lnK2, double[] lnK2Delta, double T1, double T1Delta, double T2, double T2Delta)
        {
            double[] dk_1_dt = new double[lnK1.Length];
            double[] ti = new double[lnK1.Length];
            double[] wi = new double[lnK1.Length];
            double[] wi_lnknB = new double[lnK1.Length];

            double _1_Ti = 1 / T1;

            for (int i = 0; i < lnK1.Length; i++)
            {
                dk_1_dt[i] = (lnK1Delta[i] - lnK1[i]) / _1_Ti;
                ti[i] = dk_1_dt[i] * Y[i];
            }

            double SumTi = ti.Sum();

            for (int i = 0; i < lnK1.Length; i++)
            {
                wi[i] = ti[i] / SumTi;
                wi_lnknB[i] = wi[i] * lnK1[i];
            }

            double[] dk_1_dt_2 = new double[lnK1.Length];
            double[] ti_2 = new double[lnK1.Length];
            double[] wi_2 = new double[lnK1.Length];
            double[] wi_lnknB_2 = new double[lnK1.Length];

            double _1_Ti_2 = 1 / T2;

            for (int i = 0; i < lnK1.Length; i++)
            {
                dk_1_dt_2[i] = (lnK2Delta[i] - lnK2[i]) / _1_Ti_2;
                ti_2[i] = dk_1_dt_2[i] * Y[i];
            }

            double SumTi_2 = ti_2.Sum();

            for (int i = 0; i < lnK1.Length; i++)
            {
                wi_2[i] = ti_2[i] / SumTi_2;
                wi_lnknB_2[i] = wi_2[i] * lnK2[i];
            }

            double Kb = wi_lnknB.Sum();
            double Kb2 = wi_lnknB_2.Sum();

            double B = (Kb2 - Kb) / (1 / T2 - 1 / T1);
            double A = B / T1;

            double[] Ai = new double[lnK1.Length];

            for (int i = 0; i < lnK1.Length; i++)
                Ai[i] = Math.Exp(lnK1[i]) / Math.Exp(Kb);

            return Ai;
        }
    }

    public struct MABPMethod
    {
        public double lnKB;
        public double lnKA;
        public double KAvg;
        public double[] KCompGrad;
        public double[] KCompA;
        public double DeltaT;
        internal double BaseT;
        internal double[] CompRatio;

        public MABPMethod(double deltaT) : this()
        {
            DeltaT = deltaT;
        }

        public double K_SimpleRatio(int i, double T)
        {
            double Ratio = CompRatio[i];
            double Kt = Math.Exp(lnKA - lnKB / T);
            return Ratio * Kt;
        }

        public double K_SimpleRatio(int i, double T, double Kt)
        {
            double Ratio = CompRatio[i];
            return Ratio * Kt;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double K(int CompNo, double T)
        {
            double LinestRatio = KCompA[CompNo] + KCompGrad[CompNo] * (T);
            double res = LinestRatio * KAvg;
            if (res < 0)
                return 1e-10;
            return res;
        }
    }

    public struct LinearMethod
    {
        public double[] KCompGrad;
        public double[] KCompA;
        public double[] KCompA1;
        public double BaseT;
        public double DeltaT = 1;

        public LinearMethod(double deltaT) : this()
        {
            DeltaT = deltaT;
        }

        public double K(int comp, double Tk)
        {
            return KCompA[comp] + KCompGrad[comp] * (Tk - BaseT);
        }

        public double K(double Tk, double[] X)
        {
            double res = 0;
            for (int i = 0; i < X.Length; i++)
            {
                res += K(i, Tk) * X[i];
            }
            return res;
        }
    }

    public struct LogLinearMethod
    {
        public double DeltaT;
        public double BaseT;
        public double[] LnKComp;
        public double[] LnKComp2;
        public double[] lnKCompGrad;

        public LogLinearMethod(double deltaT) : this()
        {
            DeltaT = deltaT;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double K(int comp, double Tk)
        {
            //return   Math.Exp(LnKComp[comp] + lnKCompGrad[comp] * (Tk - BaseT));
            return Ext.exp0(LnKComp[comp] + lnKCompGrad[comp] * (Tk - BaseT));
        }

        public double K(double Tk, double[] X)
        {
            double res = 0;
            for (int i = 0; i < X.Length; i++)
            {
                res += K(i, Tk) * X[i];
            }
            return res;
        }
    }
}