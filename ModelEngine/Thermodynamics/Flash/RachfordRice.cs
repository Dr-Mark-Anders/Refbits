using Extensions;
using System;
using Units.UOM;
using static ModelEngine.FlashIO;

/// Notes:  Z components = cc with the dolids removed and normalised, for flash calcs
/// CC is used directly for enthalpy calcs
/// solid componenets have high Tc and Pc so that fugacity is presnet but extremely low.
/// Also Vc =1 so that binaries can be calculated

namespace ModelEngine
{
    public class RachfordRice
    {
        private double H, P, Q, Frac, MoleFractionSolids = 0;
        private double[] X, Y, Kn;
        private readonly double[] Z;
        private readonly int count;
        private enumFluidRegion state;
        private enumCalcResult flashres = enumCalcResult.Failed;
        public Temperature TResult;
        public Pressure PResult;
        private ThermoDynamicOptions thermo;
        public Port_Material port;
        private readonly Components cc;

        public RachfordRice(Port_Material port, ThermoDynamicOptions thermo)
        {
            this.P = port.P;
            this.port = port;
            this.thermo = thermo;
            this.cc = port.cc;

            count = cc.Count;
            X = new double[count];
            Y = new double[count];

            cc.NormaliseFractions();
            if (Components.ContainsTypes(cc.CompList, typeof(SolidComponent)))
            {
                MoleFractionSolids = cc.Solids().MolFracSum();
                Z = cc.MolFractionsZeroOutSolids.Normalise();
            }
            else
                Z = cc.MoleFractions;
        }

        public bool Solve(enumFlashType flashtype, double sigma = 1E-5)
        {
            Temperature T = port.T;
            Pressure P = port.P;
            Quality Q = port.Q;
            Enthalpy H = port.H;
            Entropy S = port.S;

            if (cc.Count > 0)
            {
                switch (flashtype)
                {
                    case enumFlashType.PT:
                        if (SolveTP(T, out double Qd))
                        {
                            flashres = enumCalcResult.Converged;
                        }
                        break;

                    case enumFlashType.PH: // calc T and S and Q
                        if (SolvePH(P, H, sigma))
                        {
                            T = port.T;
                            flashres = enumCalcResult.Converged;
                        }
                        else
                            flashres = enumCalcResult.Failed;
                        break;

                    case enumFlashType.PQ:
                        T = SolvePQ(Q);
                        port.T = T;
                        if (T.IsKnown)
                            flashres = enumCalcResult.Converged;
                        break;

                    case enumFlashType.PS:
                        SolvePS(S);
                        T = TResult;
                        break;

                    case enumFlashType.HQ:
                        SolveQH();
                        T = TResult;
                        break;

                    case enumFlashType.TS:
                    case enumFlashType.TQ:
                        if (SolveTQ(Q))
                        {
                            port.P = PResult;
                            flashres = enumCalcResult.Converged;
                        }
                        break;

                    case enumFlashType.TH:
                    case enumFlashType.PTQ:
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
                return true;
            }
        }

        private Temperature SolveQH()
        {
            Temperature Test = cc.TCritMix() * 0.5;
            ThermoDynamicOptions thermowilson = new();
            thermowilson.KMethod = enumEquiKMethod.PR78;
            X = Z;
            Y = Z;
            double res = new Temperature(0);

            Kn = ThermodynamicsClass.KMixArray(cc, P, Test, X, Y, out _, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)

            /*Temperature bubblepoint = ThermodynamicsClass.CalcBubblePointT(cc);
            double HBubble = ThermodynamicsClass.BulkStreamThermo(cc, bubblepoint, X, enumMassOrMolar.Molar, enumFluidRegion.Liquid).H;

            Temperature dewpoint = ThermodynamicsClass.CalcDewPointT(cc, cc.P, X, Y);
            double HDew = ThermodynamicsClass.BulkStreamThermo(cc, dewpoint, X, enumMassOrMolar.Molar, enumFluidRegion.Vapour).H;
            Test = bubblepoint;*/

            //double Psat = ThermodynamicsClass.CalcBubblePointP(cc, Test, port.Thermo.KMethod);
            ThermodynamicsClass.UpdateThermoProperties(cc, P, Test, thermo);

            /* if (cc.NoneZeroComps == 1)
             {
                 if (HDew > H && HBubble < H)
                 {
                     port.T = bubblepoint;
                     port.Q = (H.BaseValue - HBubble) / (HDew - HBubble);
                     return true;
                 }
             }
             if (HDew > H && HBubble < H) //  partly vaporised
             {
                 Tres = BrentSolver.Solve(dewpoint, bubblepoint, H, Error);
                 Herror = Error(Tres, H);
             }
             else if (H < HBubble) // Liquid
             {
                 Tres = BrentSolver.Solve(bubblepoint, -273, H, Error);
                 Herror = Error(Tres, H);
             }
             else
             {
                 do
                 {
                     Q1 = SolveTP(Test);
                     cc.CalcLiquidSH(Test);
                     cc.CalcVapourSH(Test);
                     //Thermodynamics.UpdateThermoProperties(cc, Test, port.Thermo);
                     H1 = cc.H();

                     Q2 = SolveTP(Test + delta);
                     cc.CalcLiquidSH(Test + delta);
                     cc.CalcVapourSH(Test + delta);
                     //Thermodynamics.UpdateThermoProperties(cc, Test + delta, port.Thermo);
                     H2 = cc.H();

                     //Q = SolveTP(273.15+25);
                     //Q = SolveTP(Test);

                     var grad = ((H - H1) - (H - H2)) / delta;
                     Herror = H1 - H;

                     Tres = Test;
                     Tdelta = -Herror / grad;
                     //if (Math.Abs(Tdelta) > 50)
                     //    Tdelta = Math.Sign(Tdelta) * 50;

                     if (Math.Abs(Tdelta) > 10)
                         Tdelta = 10 * Math.Sign(Tdelta);

                     if (H1 < H && H2 > H) // solution bracketed
                         delta = delta / 10;

                     if ((Q1 == 1 && Q2 == 0) || (Q1 == 0 && Q2 == 1)) // liquid to vapour etc
                         delta = delta / 10;

                     if ((H1 > H && H2 > H && H1Old < H && H2Old < H) || (H1 < H && H2 < H && H1Old > H && H2Old > H))
                         Tdelta /= 10;

                     Test += Tdelta;

                     IterCount++;
                     H1Old = H1; H2Old = H2;
                 } while (Math.Abs(Herror / H) > FlashClass.PHErrorValue && IterCount < 200);
             }

             if (Math.Abs(Herror / H) <= FlashClass.PHErrorValue)
             {
                 SolveTP(Tres);
                 TResult = Tres;
                 port.T = Tres;
                 port.Q.BaseValue = Q2;
                 flashres = enumCalcResult.Converged;
                 return true;
             }
             else*/
            return res;
        }

        public bool SolveTP(Temperature T, out double fv)
        {
            double c = double.NaN;
            fv = double.NaN;

            if (double.IsNaN(T))
                return false;
            Y = new double[Z.Length]; // Z.Clone();
            X = new double[Z.Length]; // Z.Clone(); //new  double [Z.Length];

           // int Loc = cc.MaxNonZeroBoiling;
           // X[Loc] = 1;
           // int Loc2 = cc.MinNonZeroBoiling;
          //  Y[Loc2] = 1;

            double fv2 = 0.5;
            enumFluidRegion FlashFluidState = enumFluidRegion.Undefined;

            //port.Thermo.KMethod = enumEquiKMethod.PR78;
            thermo.KMethod = enumEquiKMethod.Wilson;
            Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            thermo.KMethod = enumEquiKMethod.PR78;

            int IterCount = 0;
            //state = enumFluidRegion.Undefined;

            if (cc.TCritMix() < T)
            {
                fv = 1;
                fv2 = double.NaN;
                FlashFluidState = enumFluidRegion.Vapour;
            }
            else
            {
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

                        if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance || fv > 10000 || fv < -10000)
                        {
                            break;
                        }
                    }

                    double[] knold = Kn;

                    if (fv >= 1 || state == enumFluidRegion.Vapour) // must run through at least once with rigorous K
                    {
                        fv2 = fv;
                        fv = 1;
                        FlashFluidState = enumFluidRegion.Vapour;
                    }
                    else if (fv < 0 || state == enumFluidRegion.Liquid)// must run through at least once with rigorous K
                    {
                        fv2 = fv;
                        fv = 0;
                        FlashFluidState = enumFluidRegion.Liquid;
                    }
                    else if (double.IsNaN(fv))
                        fv = 0.5;

                    UpdateXY(fv);

                    Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo);

                    error = 0;
                    for (int i = 0; i < Kn.Length; i++)
                        error += (Kn[i] - knold[i]).Pow(2);

                    IterCount++;
                    FlashFluidState = enumFluidRegion.TwoPhase;
                } while ((error > FlashClass.K_Tolerance && IterCount < 500 || IterCount < 2));

                if (FlashFluidState == enumFluidRegion.TwoPhase)
                {
                    if (fv >= 1)
                    {
                        state = enumFluidRegion.Vapour;
                        UpdateXY(fv);
                    }
                    else if (fv <= 0)
                    {
                        state = enumFluidRegion.Liquid;
                        UpdateXY(fv);
                    }
                    else
                    {
                        state = enumFluidRegion.TwoPhase;
                        UpdateXY(fv); // reset x and Ys
                    }
                }
                else if (FlashFluidState == enumFluidRegion.Undefined)
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
            }

            switch (FlashFluidState)
            {
                case enumFluidRegion.Undefined:
                    cc.SetFractVapour(double.NaN);
                    port.Q = double.NaN;
                    break;

                case enumFluidRegion.Gaseous:
                case enumFluidRegion.Vapour:
                    Y = (double[])Z.Clone();
                    cc.SetFractVapour(1);
                    port.Q = 1;
                    fv = 1;
                    break;

                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    X = Z;
                    cc.SetFractVapour(0);
                    port.Q = 0;
                    fv = 0;
                    break;

                case enumFluidRegion.SuperCritical:
                    Y = Z;
                    cc.SetFractVapour(1);
                    port.Q = 1;
                    fv = 1;
                    break;

                case enumFluidRegion.TwoPhase:
                    if (fv == 1)
                        cc.SetFractVapour(1);
                    else if (fv == 0)
                        cc.SetFractVapour(0);
                    break;
            }

            if (fv > 0 && fv < 1)
            {
                for (int n = 0; n < count; n++)
                {
                    Frac = (Y[n] * fv) / (Z[n]);  // no of feed moles vaporised

                    if (Z[n] == 0)
                        cc.ComponentList[n].MoleFracVap = 0;
                    else if (double.IsInfinity(Frac) || double.IsNaN(Frac))
                        cc.ComponentList[n].MoleFracVap = double.NaN;
                    else
                        cc.ComponentList[n].MoleFracVap = Frac;
                }
            }

            fv = (1 - MoleFractionSolids) * fv;
            port.Q = fv;  // MoleFraction
            flashres = enumCalcResult.Converged;
            cc.State = FlashFluidState;
            return flashres == enumCalcResult.Converged;
        }

        public double SolveTP_IO(Temperature T, out double FV2)
        {
            double c = double.NaN;
            FV2 = double.NaN;

            if (double.IsNaN(T))
                return double.NaN;
            Y = new double[Z.Length]; // Z.Clone();
            X = new double[Z.Length]; // Z.Clone(); //new  double [Z.Length];

            int Loc = cc.MaxNonZeroBoiling;
            X[Loc] = 1;
            int Loc2 = cc.MinNonZeroBoiling;
            Y[Loc2] = 1;

            double fv2 = 0.5;
            enumFluidRegion FlashFluidState = enumFluidRegion.Undefined;

            //port.Thermo.KMethod = enumEquiKMethod.PR78;
            thermo.KMethod = enumEquiKMethod.PR78;

            LogLinearMethod_IO LLinear = new(1);
            LLinear.Update(cc, P, T, X, Y, thermo);
            //Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            thermo.KMethod = enumEquiKMethod.PR78;
            double fv;
            int IterCount = 0;
            //state = enumFluidRegion.Undefined;

            if (port.TCritMix() < T)
            {
                fv = 1;
                fv2 = double.NaN;
                FlashFluidState = enumFluidRegion.Vapour;
            }
            else
            {
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

                        if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance || fv > 10000 || fv < -10000)
                        {
                            break;
                        }
                    }

                    double[] knold = Kn;

                    if (fv >= 1 || state == enumFluidRegion.Vapour) // must run through at least once with rigorous K
                    {
                        fv2 = fv;
                        fv = 1;
                        FlashFluidState = enumFluidRegion.Vapour;
                    }
                    else if (fv < 0 || state == enumFluidRegion.Liquid)// must run through at least once with rigorous K
                    {
                        fv2 = fv;
                        fv = 0;
                        FlashFluidState = enumFluidRegion.Liquid;
                    }
                    else if (double.IsNaN(fv))
                        fv = 0.5;

                    UpdateXY(fv);

                    Kn = LLinear.KArray(T, X);

                    error = 0;
                    for (int i = 0; i < Kn.Length; i++)
                        error += (Kn[i] - knold[i]).Pow(2);

                    IterCount++;
                    FlashFluidState = enumFluidRegion.TwoPhase;
                } while ((error > FlashClass.K_Tolerance && IterCount < 500 || IterCount < 2));

                if (FlashFluidState == enumFluidRegion.TwoPhase)
                {
                    if (fv >= 1)
                    {
                        state = enumFluidRegion.Vapour;
                        UpdateXY(fv);
                    }
                    else if (fv <= 0)
                    {
                        state = enumFluidRegion.Liquid;
                        UpdateXY(fv);
                    }
                    else
                    {
                        state = enumFluidRegion.TwoPhase;
                        UpdateXY(fv); // reset x and Ys
                    }
                }
                else if (FlashFluidState == enumFluidRegion.Undefined)
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
            }

            switch (FlashFluidState)
            {
                case enumFluidRegion.Undefined:
                    cc.SetFractVapour(double.NaN);
                    port.Q = double.NaN;
                    break;

                case enumFluidRegion.Gaseous:
                case enumFluidRegion.Vapour:
                    Y = (double[])Z.Clone();
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
            }

            for (int n = 0; n < count; n++)
            {
                Frac = (Y[n] * fv) / (Z[n]);  // no of feed moles vaporised

                if (Z[n] == 0)
                    cc[n].MoleFracVap = 0;
                if (double.IsInfinity(Frac) || double.IsNaN(Frac))
                    cc[n].MoleFracVap = double.NaN;
                else
                    cc[n].MoleFracVap = Frac;

                port.Q = fv;  // MoleFraction
            }

            flashres = enumCalcResult.Converged;
            cc.State = FlashFluidState;
            return fv;
        }

        public Temperature SolvePQ(double Q)
        {
            if (Q == 1)
            {
                foreach (BaseComp bc in cc)
                {
                    bc.MoleFracVap = 1;
                }
                return ThermodynamicsClass.DewPoint(cc, P, thermo, out _);
            }
            else if (Q == 0)
            {
                foreach (BaseComp bc in cc)
                {
                    bc.MoleFracVap = 0;
                }
                return ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
            }

            if (cc.NoneZeroCompsCount == 1)
            {
                foreach (BaseComp bc in cc)
                {
                    bc.MoleFracVap = Q;
                }
                return ThermodynamicsClass.BubblePoint(cc, P, thermo, out _);
            }

            double grad;
            double delta;
            Temperature T = port.TCritMix() * 0.7;
            double c2;
            double c = double.NaN;
            double[] Kn2;
            X = Z;
            Y = Z;

            double fv = Q;

            Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo);
            double KnSum = Kn.Mult(X).Sum();
            double KnDimSum = Kn.Divide(X).Sum();

            Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            Kn2 = ThermodynamicsClass.KMixArray(cc, P, T + 1, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)

            int OScount = 0;

            for (int i = 0; i < 100; i++)
            {
                c2 = C(fv, Kn2);
                c = C(fv, Kn);

                grad = (c2 - c) / 1;
                delta = c / grad;

                if (delta > 10)
                {
                    delta = 10;
                    OScount++;
                }
                else if (delta < -10)
                {
                    delta = -10;
                    OScount++;
                }
                else
                    OScount = 0;

                if (OScount > 5)
                {
                    delta /= 2;
                    OScount = 0;
                }

                T -= delta;

                if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance)
                    break;

                Kn = ThermodynamicsClass.KMixArray(cc, P, T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
                Kn2 = ThermodynamicsClass.KMixArray(cc, P, T + 1, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            }

            if (Math.Abs(c) < 0.000000001)
            {
                port.T = T;
                return T;
            }
            else
                return double.NaN;
        }

        public bool SolveTQ(double Q)
        {
            double grad;
            double delta;
            Pressure P = port.P;
            double c2;
            double c = double.NaN;
            double[] Kn2;
            X = Z;
            Y = Z;

            double fv = Q;

            if (count == 1)
            {
                PResult = ThermodynamicsClass.CalcBubblePointP(cc, port.T, thermo);
                if (PResult.IsKnown)
                    return true;
                else
                    return false;
            }

            Kn = ThermodynamicsClass.KMixArray(cc, port.P, port.T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            Kn2 = ThermodynamicsClass.KMixArray(cc, port.P, port.T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)

            for (int i = 0; i < 100; i++)
            {
                c2 = C(fv, Kn2);
                c = C(fv, Kn);

                grad = (c2 - c) / 1;
                delta = c / grad;

                P -= delta;

                if (Math.Abs(c) < FlashClass.C_Tolerance || Math.Abs(delta) < FlashClass.C_Tolerance)
                    break;

                Kn = ThermodynamicsClass.KMixArray(cc, port.P, port.T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
                Kn2 = ThermodynamicsClass.KMixArray(cc, port.P, port.T, X, Y, out state, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)
            }

            PResult = P;
            if (Math.Abs(c) < 0.000000001)
                return true;
            else
                return false;
        }

        public double Error(double T, double HTarget)
        {
            SolveTP(T, out _);
            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
            double H = cc.H(P, T, Q);
            return HTarget - H;
        }

        public double HError(double T)
        {
            SolveTP(T, out _);
            ThermodynamicsClass.UpdateThermoProperties(cc, P, T, thermo);
            double H = cc.H(P, T, Q);
            return this.H - H;
        }

        public bool SolvePH(Pressure P, Enthalpy H, double sigma)
        {
            if(double.IsNaN(H))
                return false;
            int IterCount = 0;
            double TCrit = cc.RemoveSolids(true).TCritMix();
            double PCrit = cc.RemoveSolids(true).PCritMix();
            enumFluidRegion state;
            Temperature Test = TCrit * 0.7;
            ThermoDynamicOptions thermowilson = new();
            thermowilson.KMethod = enumEquiKMethod.PR78;
            double Q = 0, H1Old = H, H2Old = H, Tdelta = 50;
            this.H = H;
            Temperature LowerT = double.NaN, UpperT = double.NaN;
            bool LowSet = false, HighSet = false;
            bool DoComplex = false;
            Enthalpy H1 = new();

            this.H = H;

            if (double.IsNaN(PCrit) || double.IsNaN(TCrit))
                return false;

            if (cc.Count == 1)
            {
                Temperature bubpoint = ThermodynamicsClass.BubblePoint(cc, port.P, thermo, out state);
                Temperature dewpoint = ThermodynamicsClass.DewPoint(cc, port.P, thermo, out state);

                if (!double.IsNaN(bubpoint))
                    Test = bubpoint;
                else if (!double.IsNaN(dewpoint))
                    Test = dewpoint;

                SolveTP(Test, out Q);
                ThermodynamicsClass.UpdateThermoProperties(cc, P, Test, cc.Thermo);

                double Hliq = cc.ThermoLiq.H;
                double Hvap = cc.ThermoVap.H;

                if (H >= Hliq && H <= Hvap)
                {
                    Q = (H - Hliq) / (Hvap - Hliq);
                    DoComplex = false;
                    LowerT = Test;
                    UpperT = Test;
                    H1 = H;
                }
           /*     else if (H > Hliq)
                {
                    var Tres = BrentSolver.Solve(-273, bubpoint, H, HError, 0.01);
                    var Herror = Error(Tres, H);
                    DoComplex = false;
                    LowerT = Test;
                    UpperT = Test;
                    H1 = H;

                }
                else if (H < Hvap)
                {
                    var Tres = BrentSolver.Solve(dewpoint, 1000, H, HError, 0.01);
                    var Herror = Error(Tres, H);
                    DoComplex = false;
                    LowerT = Test;
                    UpperT = Test;
                    H1 = H;
                }*/
                else
                {
                    DoComplex = true;
                }


            }
            if (cc.Count != 1 || DoComplex) 
            {
                do
                {
                    do
                    {
                        SolveTP(Test, out Q); // doest not update thermoprops

                        ThermodynamicsClass.UpdateThermoProperties(cc, P, Test, cc.Thermo);

                        H1 = cc.H(P, Test, Q, MoleFractionSolids);

                        if (H1 < H)
                        {
                            LowerT = Test;
                            LowSet = true;
                            Test += Tdelta;
                        }
                        else if (H1 > H)
                        {
                            UpperT = Test;
                            HighSet = true;
                            Test -= Tdelta;
                            if (Test < 0)
                            {
                                Test = 0.1;
                                Tdelta /= 2;
                                if (H1 > H)
                                    return false; // enthlpy below -273.15
                            }
                            
                        }
                        else if (H1.BaseValue.AlmostEquals(H))
                        {
                            Tdelta = 0;
                            break;
                        }

                        if (HighSet && LowSet)
                            break;

                        IterCount++;
                    } while (IterCount < 500);

                    Tdelta /= 2;
                } while (IterCount < 500 && Tdelta > 0.000005);
            }

            if (Q.AlmostEquals(1))
                foreach (BaseComp basec in cc.ComponentList)
                    basec.MoleFracVap = 1;
            else if (Q.AlmostEquals(0))
                foreach (BaseComp basec in cc.ComponentList)
                    basec.MoleFracVap = 0;

            if (Math.Abs((H1 - H) / H) <= 1e-3)
            {
                TResult = (LowerT + UpperT) / 2;
                port.T = TResult;
                port.Q = Q;
                flashres = enumCalcResult.Converged;
                return true;
            }
            else
            {
                flashres = enumCalcResult.Failed;
                return false;
            }
        }

        public bool SolvePS(Entropy S)
        {
            int IterCount = 0;
            double Test = port.TCritMix() * 0.7, Tres;
            double delta = 0.1;
            X = Z;
            Y = Z;

            Kn = ThermodynamicsClass.KMixArray(cc, P, Test, X, Y, out _, thermo); //  initialise with wilson or bad fugacities for initial guess (or could estimate heavy liquid first)

            double Serror;
            do
            {
                SolveTP(Test, out _);
                ThermodynamicsClass.UpdateThermoProperties(cc, P, Test, thermo);
                var S1 = port.StreamEntropy();

                SolveTP(Test + delta, out _);
                ThermodynamicsClass.UpdateThermoProperties(cc, P, Test + delta, thermo);
                var S2 = port.StreamEntropy();

                var grad = ((S - S1) - (S - S2)) / delta;
                Serror = S1 - S;
                Tres = Test;
                Test -= Serror / grad;

                IterCount++;
            } while (Math.Abs(Serror) > FlashClass.ErrorValue && IterCount < 500);

            if (Math.Abs(Serror) <= FlashClass.ErrorValue)
            {
                SolveTP(Tres, out _);
                TResult = Tres;
                port.T = Tres;
                flashres = enumCalcResult.Converged;
                return true;
            }
            else
                return false;
        }

        public double C(double fv, double[] Kn)
        {
            double C = 0;

            if (Kn is null)
                return double.NaN;

            double[] X = new double[count];
            double[] Y = new double[count];

            for (int n = 0; n < count; n++) // reset X and Y, only needed if fv reset
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

        public void UpdateXY(double fv)
        {
            if (Kn is null)
                return;
            else if (fv < 0)
                fv = 0;
            else if (fv > 1)
                fv = 1;

            for (int n = 0; n < count; n++) // reset X and Y, only needed if fv reset
            {
                if (fv == 1 && (Kn[n] - 1.0) == -1) // otherwise fails when Kn[n] ~ 0
                    X[n] = 0;
                else
                    X[n] = Z[n] / (1 + fv * (Kn[n] - 1.0));        // Liquid

                Y[n] = Kn[n] * X[n];
            }

            X = X.Normalise();
            Y = Y.Normalise();

            if (Math.Abs(Y.Sum() - 1) > 1e-8)
            {
                Y[cc.MaxNonZeroBoiling] = 1;
            }
            if (Math.Abs(X.Sum() - 1) > 1e-8)
            {
                X[cc.MinNonZeroBoiling] = 1;
            }
        }

        public static double CalcTotalEnthalpy(Components cc, EnthalpyDepartureLinearisation LinearHValues, double Test, double[] XFractions, double[] YFractions, double X, double Y)
        {
            double IdealHGas = IdealGas.StreamIdealGasMolarEnthalpy(cc, Test, YFractions);
            double IdealHLiq = IdealGas.StreamIdealGasMolarEnthalpy(cc, Test, XFractions);

            var LiqEnthalpyDep = LinearHValues.LiqDepEstimate(cc, XFractions, Test);
            var vapEnthalpyDep = LinearHValues.VapDepEstimate(cc, YFractions, Test);

            var LiqEnthalpy = IdealHLiq - LiqEnthalpyDep + LinearHValues.EnthForm25Liquid;
            var VapEnthalpy = IdealHGas - vapEnthalpyDep + LinearHValues.EnthForm25Vapour;

            //var LiqEnthalpyRig = Thermodynamics.BulkStreamThermo(cc, Test, XFractions, thermo, enumMassOrMolar.Molar, enumFluidRegion.Liquid).H;
            //var VapEnthalpyRig = Thermodynamics.BulkStreamThermo(cc, Test, YFractions, thermo, enumMassOrMolar.Molar, enumFluidRegion.Vapour).H;

            double TotalEnthalpy = 0;

            if (!double.IsNaN(LiqEnthalpy))
                TotalEnthalpy += X * LiqEnthalpy;
            if (!double.IsNaN(VapEnthalpy))
                TotalEnthalpy += Y * VapEnthalpy;

            return TotalEnthalpy;
        }
    }
}