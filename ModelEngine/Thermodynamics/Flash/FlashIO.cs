using Extensions;
using Math2;
using ModelEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public class FlashIO
    {
        private double[] Z;
        private Components cc;
        private EnthalpyDepartureLinearisation VapourDepValues = new();
        private EnthalpySimpleLinearisation LiquidSimpleValues = new();

        private enumCalcResult flashres = enumCalcResult.Converged;
        private enumFlashType flashtype = enumFlashType.None;
        private ThermoDynamicOptions thermo;
        private Enthalpy H;
        private Pressure P;
        private int Count;
        private double sigma = 1e-5, Herror;
        private Temperature TResult;
        private LogLinearMethod_IO LLinear = new();
        private double[] Feed;

        private double[] X, Y, XFractions, YFractions, X2, Y2, XFractions2, YFractions2, KBase;

        private double TotalX, TotalY;

        public FlashIO(Components cc, ThermoDynamicOptions thermo)
        {
            this.cc = cc;
            this.Z = cc.MoleFractions;
            this.thermo = thermo;
            Count = Z.Length;
            X = new double[Z.Length];
            Y = new double[Z.Length];
            XFractions = new double[Z.Length];
            YFractions = new double[Z.Length];
            X2 = new double[Z.Length];
            Y2 = new double[Z.Length];
            XFractions2 = new double[Z.Length];
            YFractions2 = new double[Z.Length];
            KBase = new double[Z.Length];
            Feed = (double[])Z.Clone();
        }

        public double HError(double T)
        {
            double H = TotalEnthalpy(cc, P, T, XFractions, YFractions, TotalX, TotalY, thermo);
            return this.H - H;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="props"></param>
        /// <param name="Type"></param>
        /// <param name="duty kJ/hr"></param>
        /// <returns></returns>
        public bool Flash_IO(Port_Material port)
        {
            enumFluidRegion fluidregion;
            double Duty = 0, Q1, Q2 = 0;
            flashtype = enumFlashType.PH;
            Temperature Tres;
            Pressure P = port.P;

            P = port.P_.BaseValue;
            H = port.H_.BaseValue;

            double StripFactor = 0.5, StripFactor2, TotalH, TotalH1, gradient, K1, K2, B, A;
            double TDelta = 0.1, BaseError = 1000, DeltaCaseError, ErrorDelta, delta, TOld;
            int countinner = 0, countouter = 0;
            Temperature TNew1, TNew2, Test = 273.15 + 25, Tcrit = port.TCritMix();

            LLinear.Update(cc, P.BaseValue, Tcrit, X, Y, thermo);

            LLinear.DeltaT = 1;

            KBase = LLinear.LnKComp;

            for (int i = 0; i < Count; i++)
            {
                double K = Math.Exp(KBase[i]);
                X2[i] = Feed[i] / (1 + StripFactor * (K - 1));
                Y2[i] = X2[i] * K;
            }

            XFractions = X2.Normalise();
            YFractions = Y2.Normalise();

            TOld = Tcrit;

            double HBubble = 0, HDew = 0;

            Temperature bubblepoint = ThermodynamicsClass.BubblePoint(port.cc, P, thermo, out enumFluidRegion state);
            if (double.IsNaN(bubblepoint))
                bubblepoint = 1;
            ThermoProps props1 = ThermodynamicsClass.BulkStreamThermo(cc, Feed, P, bubblepoint, enumFluidRegion.Liquid, thermo);
            if (props1 is not null)
                HBubble = props1.H;

            Temperature dewpoint = ThermodynamicsClass.DewPoint(cc, P, thermo, out state);
            if (double.IsNaN(dewpoint))
                dewpoint = 1000;
            ThermoProps props2 = ThermodynamicsClass.BulkStreamThermo(cc, Feed, P, dewpoint, enumFluidRegion.Vapour, thermo);
            if (props2 is not null)
                HDew = props2.H;

            if (double.IsNaN(HDew) || double.IsNaN(HBubble))
                return false;

            if (!double.IsNaN(bubblepoint))
                Test = bubblepoint;
            else if (!double.IsNaN(dewpoint))
                Test = dewpoint;

            if (cc.NoneZeroCompsCount == 1 && HDew > H && HBubble < H)
            {
                port.T = bubblepoint;
                port.Q = (H.BaseValue - HBubble) / (HDew - HBubble);
                return true;
            }

            if (H.BaseValue.AlmostEquals(HBubble, 0.01))
            {
                Tres = bubblepoint;
                Herror = 0;
                Q2 = 0;
            }
            else if (H.BaseValue.AlmostEquals(HDew, 0.01))
            {
                Tres = dewpoint;
                Herror = 0;
                Q2 = 1;
            }
            /*else if (HDew > H && HBubble < H) //  partly vaporised
            {
                Tres = BrentSolver.Solve(bubblepoint, dewpoint, H, HError, sigma);
                Herror = HError(Tres);
                Q2 = SolveTP(Tres);
            }*/
            else if (H < HBubble) // Liquid
            {
                Temperature T = 1;
                Tres = BrentSolver.Solve(T, bubblepoint, H, HError, sigma);
                Herror = HError(Tres);
                Q2 = SolveTP(port,Tres);
                //res = ThermodynamicsClass.BulkStreamThermo(cc, cc.P, bubblepoint, X, enumFluidRegion.Liquid, thermo).H;
            }
            else if (H > HDew) // Vapour
            {
                Temperature T = 1000;
                Tres = BrentSolver.Solve(bubblepoint, T, H, HError, sigma);
                Herror = HError(Tres);
                Q2 = SolveTP(port,Tres);
                //var res = ThermodynamicsClass.BulkStreamThermo(cc, cc.P, new Temperature(25 + 273.15), cc.MolFractions, enumFluidRegion.Vapour, thermo).H;
            }
            else
            {
                do
                {
                    LLinear.Update(cc, P.BaseValue, Test, XFractions, YFractions, thermo);

                    LiquidSimpleValues.LiqUpdate(cc, XFractions, P, Test, Test + 1, thermo);
                    VapourDepValues.VapUpdate(cc, YFractions, P, Test, Test + 1, thermo);

                    do
                    {
                        Strip(Test, ref X2, ref Y2, StripFactor);

                        TotalX = X2.Sum();
                        TotalY = Y2.Sum();

                        XFractions = X2.Normalise();
                        YFractions = Y2.Normalise();

                        StripFactor2 = Math.Exp(Math.Log(StripFactor) + 0.1);

                        Strip(Test, ref X2, ref Y2, StripFactor);

                        XFractions2 = X2.Normalise();
                        YFractions2 = Y2.Normalise();

                        K1 = Math.Log(LLinear.K(Test - TDelta, XFractions));
                        K2 = Math.Log(LLinear.K(Test + TDelta, XFractions));

                        B = (K2 - K1) / (2 * TDelta);
                        A = K1 - B * Test;
                        TNew1 = -A / B;

                        K1 = Math.Log(LLinear.K(Test - TDelta, XFractions2));
                        K2 = Math.Log(LLinear.K(Test + TDelta, XFractions2));

                        B = (K2 - K1) / (2 * TDelta);
                        A = K1 - B * Test;
                        TNew2 = -A / B;

                        TotalH = TotalEnthalpy(cc, P, TNew1, XFractions, YFractions, TotalX, TotalY, thermo);
                        TotalH1 = TotalEnthalpy(cc, P, TNew2, XFractions2, YFractions2, TotalX, TotalY, thermo);

                        switch (flashtype)
                        {
                            case enumFlashType.PT:
                                BaseError = H - TotalH - Duty;
                                DeltaCaseError = H - TotalH1;
                                ErrorDelta = DeltaCaseError - BaseError;
                                gradient = ErrorDelta / 0.1;
                                delta = -BaseError / gradient;
                                StripFactor = Math.Exp(Math.Log(StripFactor) + delta);
                                break;

                            case enumFlashType.PQ:
                                break;

                            case enumFlashType.TQ:
                                break;

                            case enumFlashType.PH:
                                BaseError = H - TotalH - Duty;
                                DeltaCaseError = H - TotalH1 - Duty;
                                ErrorDelta = DeltaCaseError - BaseError;
                                gradient = ErrorDelta / 0.1;
                                delta = -BaseError / gradient;
                                StripFactor = Math.Exp(Math.Log(StripFactor) + delta);
                                break;

                            case enumFlashType.TH:
                                break;

                            case enumFlashType.PS:
                                break;

                            case enumFlashType.TS:
                                break;

                            case enumFlashType.PTQ:
                                break;

                            case enumFlashType.HQ:
                                break;

                            case enumFlashType.None:
                                break;

                            default:
                                break;
                        }

                        if (StripFactor < 0.00001)
                        {
                            fluidregion = enumFluidRegion.Liquid;
                            break;
                        }
                        else if (StripFactor > 1e8)
                        {
                            StripFactor = 1e6;
                            fluidregion = enumFluidRegion.Vapour;
                            break;
                        }
                        else
                            fluidregion = enumFluidRegion.TwoPhase;

                        if (Math.Abs(Test - TNew1) > 20)
                            Test += Math.Sign(TNew1) * 20;
                        else
                            Test = TNew1;

                        if (countinner == 0)
                        {
                            ////LinearKValues.LineariseK(cc, Test, props.P, XFractions, YFractions, thermo);
                            //LinearHValues.Linearise(cc, Test, Test + 1, props.P, XFractions, YFractions, thermo);
                        }

                        countinner++;
                    } while (Math.Abs(BaseError) > 0.0000001 && countinner < 100);

                    if (countouter > 0 && fluidregion != enumFluidRegion.TwoPhase)
                        break;

                    countouter++;
                } while (Math.Abs(TOld - Test) > 0.000001 && countouter < 100);
            }

            if (Q2.AlmostEquals(1))
                foreach (BaseComp basec in cc.ComponentList)
                    basec.MoleFracVap = 1;
            else if (Q2.AlmostEquals(0))
                foreach (BaseComp basec in cc.ComponentList)
                    basec.MoleFracVap = 0;

            if (Math.Abs(Herror / H) <= sigma)
            {
                TResult = Test;
                port.T = Test;
                port.Q = Q2;
                flashres = enumCalcResult.Converged;
                return true;
            }
            else
            {
                flashres = enumCalcResult.Failed;
                return false;
            }
        }

        public double SolveTP(Port_Material port, Temperature T)
        {
            double c = double.NaN;
            Pressure P = port.P;

            if (double.IsNaN(T))
                return double.NaN;
            X = Z;
            Y = Z;

            //port.Thermo.KMethod = enumEquiKMethod.PR78;

            double[] Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out enumFluidRegion state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)

            double fv;
            int IterCount = 0;
            //state = enumFluidRegion.Undefined;

            double error;
            do
            {
                fv = 0.5;
                for (int i = 0; i < 500; i++)
                {
                    double c2 = C(fv + 0.01, Kn);
                    c = C(fv, Kn);

                    var gradient = (c2 - c) / 0.01;
                    var delta = c / gradient;

                    fv -= delta;

                    if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance || fv > 1000)
                        break;
                }

                if (fv >= 1)
                {
                    fv = 1;
                    break;
                }
                else if (fv < 0)
                {
                    fv = 0;
                    break;
                }
                else
                {
                    double[] knold = Kn;
                    Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo);

                    error = 0;
                    for (int i = 0; i < Kn.Length; i++)
                        error += (Kn[i] - knold[i]).Pow(2);

                    IterCount++;
                }
            } while ((error > FlashClass.K_Tolerance && IterCount < 500));

            if (state == enumFluidRegion.TwoPhase)
            {
                if (fv >= 1)
                {
                    state = enumFluidRegion.Vapour;
                    c = C(fv, Kn);
                }
                else if (fv <= 0)
                {
                    state = enumFluidRegion.Liquid;
                    c = C(fv, Kn);
                }
                else
                {
                    state = enumFluidRegion.TwoPhase;
                    c = C(fv, Kn); // reset x and Ys
                }
            }

            if (double.IsNaN(c))
                state = enumFluidRegion.Undefined;

            switch (state)
            {
                case enumFluidRegion.Undefined:
                    cc.SetFractVapour(double.NaN);
                    port.Q = double.NaN;
                    break;

                case enumFluidRegion.Gaseous:
                case enumFluidRegion.Vapour:
                    Y = Z;
                    cc.SetFractVapour(1);
                    port.Q = 1;
                    break;

                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    X = Z;
                    cc.SetFractVapour(0);
                    port.Q = 0;
                    break;

                case enumFluidRegion.SuperCritical:
                    Y = Z;
                    cc.SetFractVapour(1);
                    port.Q = 1;
                    break;

                default:
                    for (int n = 0; n < Count; n++)
                    {
                        double Frac = (Y[n] * fv) / (Z[n]);  // no of feed moles vaporised

                        if (double.IsInfinity(Frac) || double.IsNaN(Frac))
                            cc.ComponentList[n].MoleFracVap = 0;
                        else
                            cc.ComponentList[n].MoleFracVap = Frac;

                        port.Q = fv;  // MoleFraction
                    }
                    break;
            }
            flashres = enumCalcResult.Converged;
            cc.State = state;
            return fv;
        }

        public double C(double fv, double[] Kn)
        {
            double C = 0;

            if (Kn is null)
                return double.NaN;

            X = new double[Count];
            Y = new double[Count];

            for (int n = 0; n < Count; n++) // reset X and Y, only needed if fv reset
            {
                if (fv == 1 && (Kn[n] - 1.0) == -1) // otherwise fails when Kn[n] ~ 0
                    X[n] = 0;
                else
                    X[n] = Z[n] / (1 + fv * (Kn[n] - 1.0));        // Liquid

                Y[n] = Kn[n] * X[n];

                C += X[n] - Y[n];
            }

            return C;
        }

        public double TotalEnthalpy(Components cc, Pressure P, Temperature Test, double[] XFractions, double[] YFractions, double XFLow, double YFlow, ThermoDynamicOptions thermo)
        {
            var LiqEnthalpyDep = LiquidSimpleValues.LiqEstimate(Test, cc.MW());
            var vapEnthalpyDep = VapourDepValues.VapDepEstimate(cc, YFractions, Test);

            var LiqEnthalpy = LiquidSimpleValues.LiqEstimate(Test, cc.MW(XFractions));
            var VapEnthalpy = VapourDepValues.VapEnthalpy(cc, YFractions, P, Test);

            double TotalEnthalpy = 0;

            if (!double.IsNaN(LiqEnthalpy))
                TotalEnthalpy += XFLow * LiqEnthalpy;
            if (!double.IsNaN(VapEnthalpy))
                TotalEnthalpy += YFlow * VapEnthalpy;

            return TotalEnthalpy;
        }

        public void Strip(Temperature T, ref double[] X2, ref double[] Y2, double StripFactor)  //Z[n] / (1 + fv * (Kn[n] - 1.0))
        {
            for (int i = 0; i < Count; i++)
            {
                double K = LLinear.K(i, T);
                X2[i] = Feed[i] / (1 + StripFactor * K);
                Y2[i] = StripFactor * X2[i] * K;
            }
        }

        public struct LogLinearMethod_IO
        {
            public double DeltaT;
            public Temperature BaseT;
            public double[] LnKComp;
            public double[] LnKComp2;
            public double[] lnKCompGrad;

            public LogLinearMethod_IO(double deltaT) : this()
            {
                DeltaT = deltaT;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double K(int comp, double Tk)
            {
                return Math.Exp(LnKComp[comp] + lnKCompGrad[comp] * (Tk - BaseT));
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

            public double[] KArray(double Tk, double[] X)
            {
                double[] res = new double[X.Length];
                for (int i = 0; i < X.Length; i++)
                {
                    res[i] = K(i, Tk) * X[i];
                }
                return res;
            }

            public void Update(Components cc, Pressure P, Temperature T, double[] X, double[] Y, ThermoDynamicOptions thermo)
            {
                BaseT = T;
                LnKComp = ThermodynamicsClass.LnKMixArray(cc, P.BaseValue, T, X, Y, out _, thermo);
                LnKComp2 = ThermodynamicsClass.LnKMixArray(cc, P.BaseValue, T + DeltaT, X, Y, out _, thermo);
                UpdateGrad();
            }

            public void UpdateGrad()
            {
                lnKCompGrad = new double[LnKComp.Length];

                for (int CompNo = 0; CompNo < LnKComp.Length; CompNo++)
                {
                    lnKCompGrad[CompNo] = (LnKComp2[CompNo] - LnKComp[CompNo]) / DeltaT;
                }
            }
        }
    }
}