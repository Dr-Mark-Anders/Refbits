using Extensions;
using System;
using Units.UOM;

namespace ModelEngine
{
    public class OriginalMethods
    {
        private readonly Components cc;
        private readonly double ErrorValue = 1e-6;

        public OriginalMethods(Components cc)
        {
            this.cc = cc;
        }

        public OriginalMethods(Port_Material port)
        {
            this.cc = port.cc;
        }

        public bool Solve(Port_Material port, ThermoDynamicOptions thermo)
        {
            enumCalcResult flashres = enumCalcResult.Failed;
            Temperature T = port.T;
            Pressure P = port.P;
            Enthalpy H = port.H;
            Entropy S = port.S;
            Components cc = port.cc;

            //cc.UpdateProps(port);

            if (cc.ComponentList.Count > 0)
            {
                switch (FlashTypes.FlashType(port))
                {
                    case enumFlashType.PT:
                        FlashT_P(port, P, T, ref flashres, thermo);
                        break;

                    case enumFlashType.PH: // calc T and S and Q
                    case enumFlashType.PTQ:
                        T = FlashP_H(port, P, H, ref flashres, thermo);
                        if (!double.IsNaN(T))
                            port.T = T;
                        else
                        {
                            cc.ThermoLiq.Clear();
                            cc.ThermoVap.Clear();
                        }
                        break;

                    case enumFlashType.PQ:
                        T = FlashP_Q(port, port.Q, thermo, ref flashres);
                        port.T = T;
                        break;

                    case enumFlashType.PS:
                        T = FlashP_S(port, P, S, ref flashres, thermo); //  needs finishing
                        port.T = T;
                        break;

                    case enumFlashType.TS:
                        P = FlashT_S(port, P, T, S, ref flashres, thermo); ;
                        port.P = P;
                        break;

                    case enumFlashType.TQ:
                        P = FlashT_Q(cc, P, T, port.Q, thermo, ref flashres);
                        port.P = P;
                        break;

                    case enumFlashType.TH:
                        P = FlashT_H(port, cc, P, T, cc.StreamEnthalpy(port.Q), ref flashres, thermo);
                        port.P = P;
                        break;

                    case enumFlashType.HQ:
                        break;

                    case enumFlashType.None:
                        cc.ClearThermoValues();
                        break;
                }
            }
            if (flashres == enumCalcResult.Failed)
            {
                cc.ClearThermoValues();
                return false;
            }
            else
            {
                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                //Thermodynamics.UpdateThermoDerivativeProperties(cc, thermo, cc.State);
                return true;
            }
        }

        /// <summary>
        /// Returns H after flashing to fixed T and P spec, Bisection Method (slow)
        /// </summary>
        /// <param name="Tspec"></param>
        /// <param name="Pspec"></param>
        /// <returns></returns>
        public static void FlashT_P(Port_Material port, Pressure P, Temperature T, ref enumCalcResult cresc, ThermoDynamicOptions thermo)  // t & P specified
        {
            //Debug.Print("Flash");
            Components cc = port.cc;
            int CompCount = cc.ComponentList.Count;
            double fv = 0.5, delta;
            double[] X = new double[CompCount];
            double[] Y = new double[CompCount];
            double C = 0;
            double Cold = 0;
            double[] Kn;
            double[] KnOld = new double[CompCount];
            double error;
            double Frac;
            //double ATot = 0, BTot = 0;
            int IterCount = 0;

            if (!cc.MolFracSum().AlmostEquals(1, 0.0000001))
                return;

            //Tspec = o.TCritMix().K() * 0.7 - 273.15;
            ThermoDynamicOptions thermowilson = new();
            thermowilson.KMethod = enumEquiKMethod.PR78;
            Kn = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            if (Kn is null)
            {
                cresc = enumCalcResult.Failed;
                return;
            }

            double[] Zn = new double[CompCount];
            for (int n = 0; n < CompCount; n++)
            {
                Zn[n] = cc.ComponentList[n].MoleFraction;
            }

            enumFluidRegion state;
            do // converge on K values
            {
                delta = 0.1;
                for (int count = 0; count < 1000; count++)
                {
                    C = 0;

                    for (int n = 0; n < CompCount; n++) // reset X and Y, only needed if fv reset
                    {
                        if ((Kn[n] - 1.0) == -1) // otherwise fails when Kn[n] ~ 0
                            X[n] = Zn[n];
                        else
                            X[n] = Zn[n] / (1 + fv * (Kn[n] - 1.0));        // Liquid
                        Y[n] = Kn[n] * X[n];

                        C += X[n] - Y[n];
                    }

                    if ((C > 0 && Cold < 0) || (C < 0 && Cold > 0)) // solution bracketed
                        delta /= 2;

                    Cold = C;

                    if (Math.Abs(C) < FlashClass.C_Tolerance)
                        break;

                    if (C > 0)  //make fc smaller
                        fv += -delta;
                    else if (C < 0)
                        fv += delta;

                    if (fv > 1 || fv < 0)
                        break;
                }

                error = 0;

                for (int i = 0; i < Kn.Length; i++)
                    error += (Kn[i] - KnOld[i]).Pow(2);

                KnOld = Kn;

                double sumX = 0, SumY = 0;

                for (int n = 0; n < X.Length; n++) // normalise required if fv = 1 or 0
                {
                    sumX += X[n];
                    SumY += Y[n];
                }

                for (int n = 0; n < X.Length; n++)
                {
                    X[n] = X[n] / sumX;
                    Y[n] = Y[n] / SumY;
                }

                IterCount++;

                Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo);

                if (fv >= 1)
                    fv = 1;
                else if (fv < 0)
                    fv = 0;
            } while ((error > FlashClass.K_Tolerance && IterCount < 500) && !(0 >= 10 || fv == 0 || fv == 1));

            if (double.IsNaN(C))
                state = enumFluidRegion.Undefined;

            if (state == enumFluidRegion.TwoPhase)
            {
                if (fv == 1)
                    state = enumFluidRegion.Vapour;
                else if (fv == 0)
                    state = enumFluidRegion.Liquid;
                else
                    state = enumFluidRegion.TwoPhase;
            }

            switch (state)
            {
                case enumFluidRegion.Undefined:
                    cc.SetFractVapour(double.NaN);
                    port.Q = double.NaN;
                    break;

                case enumFluidRegion.Gaseous:
                case enumFluidRegion.Vapour:
                    cc.SetFractVapour(1);
                    port.Q = 1;
                    break;

                case enumFluidRegion.Liquid:
                    cc.SetFractVapour(0);
                    port.Q = 0;
                    break;

                default:
                    for (int n = 0; n < CompCount; n++)
                    {
                        Frac = (Y[n] * fv) / (cc.ComponentList[n].MoleFraction);  // no of feed moles vaporised

                        if (double.IsInfinity(Frac) || double.IsNaN(Frac))
                            cc.ComponentList[n].MoleFracVap = 0;
                        else
                            cc.ComponentList[n].MoleFracVap = Frac;

                        port.Q = fv;  // MoleFraction
                    }
                    break;
            }
            cresc = enumCalcResult.Converged;
            cc.State = state;
            return;
        }

        public static Temperature FlashP_Q(Port_Material port, Quality Q, ThermoDynamicOptions thermo, ref enumCalcResult cres)  // Q & P specified
        {
            Components cc = port.cc;
            double fv = Q;
            double[] A = new double[cc.Count];
            double[] B = new double[cc.Count];
            double C;
            double Cold = 0;
            int count = 0;
            bool finished = false;
            Temperature TCritMix = port.TCritMix();

            Pressure P = port.P;
            Temperature T = port.T;

            if (!cc.MolFracSum().AlmostEquals(1, 0.00000001))
                return new Temperature(0);

            enumFluidRegion state;

            if (fv == 0)
                fv = 0.000000001;

            Temperature Tspec = TCritMix * 0.7;
            port.T = Tspec;

            double[] Kn;

            state = ThermodynamicsClass.CheckState(cc, cc.MoleFractions, Tspec, port.P, thermo);

            do
            {
                switch (state)
                {
                    case enumFluidRegion.CompressibleLiquid:
                        Tspec = new Temperature(-999);
                        finished = true;
                        break;

                    case enumFluidRegion.Liquid:
                        Tspec += 10;
                        break;

                    case enumFluidRegion.Gaseous:
                    case enumFluidRegion.Vapour:
                        Tspec = new Temperature(Tspec - (TCritMix - Tspec) / 2);
                        break;

                    case enumFluidRegion.TwoPhase:
                        {
                            double delta = Math.Min(10, Math.Abs(TCritMix - Tspec) / 10);
                            do
                            {
                                count++;
                                C = 0;

                                port.T = Tspec;
                                Kn = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);

                                for (int n = 0; n < cc.Count; n++)
                                {
                                    double Zn;
                                    Zn = cc[n].MoleFraction;
                                    A[n] = Zn / (fv * (Kn[n] - 1) + 1);    // Liquid
                                    B[n] = Kn[n] * A[n];
                                    C += A[n] - B[n];
                                }

                                if (C > 0)  //make fc smaller
                                    Tspec += delta;
                                else if (C < 0)
                                    Tspec += -delta;

                                if ((C > 0 && Cold < 0) || (C < 0 && Cold > 0)) // solution bracketed

                                    delta /= 2;

                                Cold = C;
                            } while (Math.Abs(C) > FlashClass.ErrorValue && count < 1000);

                            if (Tspec < 0 || count >= 1000)
                            {
                                cres = enumCalcResult.Failed;
                                return new Temperature(double.NaN);
                            }

                            double Frac;

                            for (int n = 0; n < cc.Count; n++)
                            {
                                if (fv == 1)
                                    cc[n].MoleFracVap = 1;
                                else if (fv == 0)
                                    cc[n].MoleFracVap = 0;
                                else
                                {
                                    if (cc[n].MoleFraction == 0)
                                        cc[n].MoleFracVap = 0;
                                    else
                                    {
                                        Frac = (B[n] * fv) / cc[n].MoleFraction;  // no of feed moles vaporised
                                        cc[n].MoleFracVap = Frac;
                                    }
                                }
                            }
                        }
                        finished = true;
                        break;
                }
                state = ThermodynamicsClass.CheckState(cc, cc.MoleFractions, Tspec, P, thermo);
            } while (!finished);

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, cc.Thermo);

            cres = enumCalcResult.Converged;
            return Tspec;
        }

        public Temperature FlashP_H(Port_Material port, Pressure P, Enthalpy H, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            // Debug.Print("Flash P_H");
            double d = 0, dold = 0;
            double delta = 5;
            Components cc = port.cc;
            Temperature T = cc.TCritMix() * 0.7;
            Enthalpy Aenthalpy;
            int count = 0;

            if (T < 0)
                T = 100;

            do
            {
                if (d > 0)
                    T += delta;
                else if (d < 0)
                    T -= delta;

                FlashT_P(port, P, T, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);

                Aenthalpy = cc.H(port.P, port.T, port.Q);

                d = (H - Aenthalpy) / Math.Abs(H); // positive = Enthalp > Aenthalpy

                // if (d == dold)
                //     return T;

                if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                    delta /= 2;

                dold = d;
                count++;
            } while (Math.Abs((H - Aenthalpy) / H) > FlashClass.ErrorValue && count < 100);

            cres = enumCalcResult.Converged;

            return T;
        }

        public Temperature FlashP_S(Port_Material port, Pressure P, Entropy S, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double dold = 0;
            double delta = 5;
            Components cc = port.cc;
            Temperature T = cc.TCritMix() * 0.7;
            double Aentropy;
            double d;
            do
            {
                FlashT_P(port, P, T, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);

                Aentropy = port.StreamEntropy();

                d = Aentropy - S;

                if (d > 0)
                    T -= delta;
                else
                    T += delta;

                if (d == dold)
                    return new Temperature(T);

                if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                    delta /= 2;

                dold = d;

                //this.T.Value = t;
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            if (Math.Abs(d) < FlashClass.ErrorValue)
            {
                cres = enumCalcResult.Converged;
                return new Temperature(T);
            }

            return double.NaN;
        }

        public static Pressure FlashT_Q(Components cc, Pressure P, Temperature T, Quality Q, ThermoDynamicOptions thermo, ref enumCalcResult cres)  // t & Q specified
        {
            double fv = Q, delta = 0.1;
            double[] A = new double[cc.Count];
            double[] B = new double[cc.Count];
            double Cold = 0;
            Pressure Pspec = 1;
            int count = 0;
            enumFluidRegion state;
            bool finished = false;

            if (cc.MolFracSum() != 1)
                return double.NaN;

            if (fv == 0)
                fv = 0.00000000001;

            if (T.Equals(double.NaN))
                return double.NaN;

            double Zn;

            double[] Kn;

            state = ThermodynamicsClass.CheckState(cc, cc.MoleFractions, T, Pspec, thermo);

            do
            {
                switch (state)
                {
                    case enumFluidRegion.SuperCritical:
                        Pspec = -999;
                        finished = true;
                        break;

                    case enumFluidRegion.Liquid:
                        finished = true;
                        break;

                    case enumFluidRegion.Gaseous:
                    case enumFluidRegion.Vapour:
                        Pspec += 0.1;
                        break;

                    case enumFluidRegion.TwoPhase:
                        double C;
                        do
                        {
                            count++;
                            C = 0;

                            P = Pspec;
                            Kn = ThermodynamicsClass.KMixArray(cc, P, T, cc.MoleFractions, cc.MoleFractions, out _, thermo);

                            for (int n = 0; n < cc.Count; n++)
                            {
                                Zn = cc[n].MoleFraction;
                                A[n] = Zn / (fv * (Kn[n] - 1) + 1);           // Liquid
                                                                              //B[n] = Kn * Zn / (fv * (Kn - 1) + 1);    // Vapour
                                B[n] = Kn[n] * A[n];
                                C += A[n] - B[n];
                            }

                            if (C > 0)  //make fc smaller
                                Pspec += -delta;
                            else if (C < 0)
                                Pspec += delta;

                            if ((C > 0 && Cold < 0) || (C < 0 && Cold > 0)) // solution bracketed
                                delta /= 2;

                            Cold = C;
                        } while (Math.Abs(C) > FlashClass.ErrorValue && Pspec > 0 && count < 500);

                        if (count >= 500)
                        {
                            cres = enumCalcResult.Failed;
                            return double.NaN;
                        }

                        for (int n = 0; n < cc.Count; n++)
                        {
                            if (fv == 1 || fv == 0)
                                cc[n].MoleFracVap = fv;
                            else
                            {
                                if (B[n] > 1)
                                    B[n] = 1;
                                else if (B[n] < 0)
                                    B[n] = 0;
                                cc[n].MoleFracVap = B[n] / cc[n].MoleFraction;
                            }
                        }
                        finished = true;
                        break;
                }

                state = ThermodynamicsClass.CheckState(cc, cc.MoleFractions, T, Pspec, thermo);
            } while (!finished);

            cres = enumCalcResult.Converged;

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);

            return Pspec;
        }

        public static Pressure FlashT_H(Port_Material port, Components cc, Pressure P, Temperature T, Enthalpy H, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 5;
            double p = P;
            double Aenthalpy;

            if (p < 0)
                p = 1;

            do
            {
                FlashT_P(port, P, T, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);

                Aenthalpy = cc.StreamEnthalpy(port.Q);

                d = Aenthalpy - H;

                if (d > 0)
                {
                    p -= delta;
                }
                else
                {
                    p += delta;
                }
                if (d == dold)
                {
                    return p;
                }
                if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                {
                    delta /= 2;
                }
                dold = d;
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);

            return p;
        }

        public static Pressure FlashT_S(Port_Material port, Pressure P, Temperature T, Entropy S, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double dold = 0;
            double delta = 5;
            Components cc = port.cc;
            double p = cc.PCritMix() * 0.1;

            if (p < 0)
                p = 1;
            double d;
            do
            {
                FlashT_P(port, P, T, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                Entropy AEntropy = cc.S(P, T, port.Q);

                d = AEntropy - S;

                if (d > 0)
                {
                    p -= delta;
                }
                else
                {
                    p += delta;
                }
                if (d == dold)
                {
                    return p;
                }
                if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                {
                    delta /= 2;
                }
                dold = d;
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);

            return p;
        }

        /// <summary>
        /// Single Phase Only
        /// </summary>
        /// <param name="H"></param>
        /// <param name="TGuess"></param>
        /// <returns></returns>

        public static Temperature VaryTempToSetMolarSpecificEntropy(Port_Material port, Pressure P, Entropy S, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 5;
            Components cc = port.cc;
            Temperature t = cc.TCritMix() * 0.7;
            double Aentropy;
            do
            {
                FlashT_P(port, P, t, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, t, thermo);
                Aentropy = cc.S(P, t, port.Q);

                d = Aentropy - S.BaseValue;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d > 0)
                        t -= delta;
                    else
                        t += delta;

                    if (d == dold)
                        return t;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            //o.H.SetCalcValue(H);

            return t;
        }

        public Temperature VaryTempToSetMolarSpecificEnthalpy(Port_Material port, Enthalpy Enthalpy, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            Components cc = port.cc;
            double d, dold = 0;
            double delta = 5;
            Temperature t = port.T;
            double Aenthalpy;
            double count = 0;

            do
            {
                FlashT_P(port, port.P, t, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, port.P, t, thermo);
                Aenthalpy = cc.H(port.P, t, port.Q);

                d = Aenthalpy - Enthalpy;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d > 0)
                        t -= delta;
                    else
                        t += delta;

                    if (d == dold)
                        return t;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }

                port.T = t;

                count++;
                if (count > 1000)
                {
                    cres = enumCalcResult.Failed;
                    return double.NaN;
                }
            } while (Math.Abs(d) > ErrorValue && t > -273.15 && t < 5000);

            if (double.IsNaN(d) || t < -273.15 || t > 5000)
            {
                cres = enumCalcResult.Failed;
            }

            return t;
        }

        public static Pressure VaryPressToSetMolarSpecificEnthalpy(Port_Material port, Pressure P, Temperature T, Enthalpy Enthalpy, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            double d, dold = 0;
            double delta = 0.1;
            double Aenthalpy;
            Components cc = port.cc;
            do
            {
                FlashT_P(port, P, T, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                Aenthalpy = cc.H(port.P, port.T, port.Q);

                d = Aenthalpy - Enthalpy;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d > 0)
                        P -= delta;
                    else
                        P += delta;

                    if (d == dold)
                        return P;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue);

            if (double.IsNaN(d))
                cres = enumCalcResult.Failed;

            return P;
        }

        public static Pressure VaryPressToSetMolarSpecificEntropy(Port_Material port, Pressure P, Temperature T, Entropy Entropy, Pressure PGuess, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            Components cc = port.cc;
            double d, dold = 0;
            double delta = 0.1;
            double p = PGuess;
            do
            {
                FlashT_P(port, P, T, ref cres, thermo);

                ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
                Entropy Aentropy = cc.S(P, T, port.Q);

                d = Aentropy - Entropy;
                if (Math.Abs(d) > 0.000000001)
                {
                    if (d < 0)
                        p -= delta;
                    else
                        p += delta;

                    if (d == dold)
                        return p;

                    if ((d > 0 && dold < 0) || (d < 0 && dold > 0)) // solution bracketed
                        delta /= 2;

                    dold = d;
                }
            } while (Math.Abs(d) > FlashClass.ErrorValue && p < 500);

            //o.H.SetCalcValue(H);

            return p;
        }

        public Temperature SingleFlashP_H(Port_Material port, Enthalpy enthalpy, ref enumCalcResult cres, ThermoDynamicOptions thermo)
        {
            Temperature BP = ThermodynamicsClass.BubblePoint(port.cc, port.P, thermo, out _);
            Enthalpy liqenthalpy;
            Enthalpy vapenthalpy;
            Temperature Tres;
            Pressure P = port.P;
            Temperature T = port.T;

            port.T = BP;

            ThermodynamicsClass.UpdateThermoProperties(port.cc, P, T, cc.Thermo);

            liqenthalpy = cc.ThermoLiq.H;
            vapenthalpy = cc.ThermoVap.H;

            if (enthalpy <= liqenthalpy)      // all liquid
            {
                Tres = VaryTempToSetMolarSpecificEnthalpy(port, enthalpy, ref cres, thermo);
                port.Q = 0;
            }
            else if (enthalpy >= vapenthalpy) // all vapour
            {
                Tres = VaryTempToSetMolarSpecificEnthalpy(port, enthalpy, ref cres, thermo);
                port.Q = 1;
            }
            else                              // partially vaporised
            {
                Tres = BP;
                port.Q = (enthalpy.kJ_kgmole - liqenthalpy.kJ_kgmole) / (vapenthalpy.kJ_kgmole - liqenthalpy.kJ_kgmole);
            }
            return Tres;
        }
    }
}