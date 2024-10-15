using Extensions;
using ModelEngine.ThermodynamicMethods.Activity_Models;
using ModelEngine.ThermodynamicMethods.UNIFAC;
using SimpleProps;
using Steam97;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public static partial class ThermodynamicsClass
    {
        public const double Rgas = 8.31446261815324;
        private const double HysysPRef = 0.01;
        public const double basep = 1;
        public static readonly double baseTk = 273.15 + 25;

        private static readonly double baset1 = baseTk,
            baset2 = baset1 * baset1,
            baset3 = baset2 * baset1,
            baset4 = baset3 * baset1,
            baset5 = baset4 * baset1;

        private static StmPropIAPWS97 steam = new();

        private static Entropy EntropyDep(Temperature T, Enthalpy enthDep, double FugCoeff)
        {
            return enthDep / T - Rgas * Math.Log(FugCoeff);
        }

        public static double ActLiqDensity(Components cc, Temperature T)
        {
            double density = DensityMethods.Density(cc.Thermo.Density, cc, T);
            return density;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="t"></param>
        /// <param name="p"></param>
        public static ThermoDifferentialPropsCollection UpdateThermoDerivativeProperties(Components cc, Pressure P, Temperature T,
            ThermoDynamicOptions thermo, enumFluidRegion state)
        {
            ThermoDifferentialPropsCollection prop = null;
            switch (state)
            {
                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    prop = ThermoDifferentials(cc, cc.LiqPhaseMolFractions, P, T, state, thermo);
                    break;

                case enumFluidRegion.Vapour:
                case enumFluidRegion.Gaseous:
                    prop = ThermoDifferentials(cc, cc.VapPhaseMolFractions, P, T, state, thermo);
                    break;

                default:
                    prop = ThermoDifferentials(cc, cc.MoleFractions, P, T, state, thermo);
                    break;
            }
            return prop;
        }

        /// <summary>
        /// Update H,S,U, etc, checks for solids
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="t"></param>
        /// <param name="p"></param>
        public static void UpdateThermoProperties(Components cc, Pressure P, Temperature T,
            ThermoDynamicOptions thermo, enumFluidRegion state = enumFluidRegion.TwoPhase)
        {
            Components tempcc=new();
            Components solids=new();

            bool HasSolids = cc.HasSolids;

            if (HasSolids)
            {
                tempcc = cc.RemoveSolids(true);
                solids = cc.Solids();
            }
            else
                tempcc = cc;

            Components cc1 = tempcc.Clone();

            if (double.IsNaN(P) || double.IsNaN(T))
            {
                if (cc is not null)
                {
                    if (cc.ThermoLiq is not null)
                        cc.ThermoLiq.Clear();
                    if (cc.ThermoVap is not null)
                        cc.ThermoVap.Clear();
                    return;
                }
                return;
            }

            switch (state)
            {
                case enumFluidRegion.Liquid:
                case enumFluidRegion.CompressibleLiquid:
                    cc.ThermoLiq = BulkStreamThermo(cc1, cc1.MoleFractions, P, T, enumFluidRegion.Liquid, thermo);
                    cc.ThermoVap = new(P, T);
                    break;

                case enumFluidRegion.Vapour:
                case enumFluidRegion.Gaseous:
                    cc.ThermoVap = BulkStreamThermo(cc1, cc1.MoleFractions, P, T, enumFluidRegion.Vapour, thermo);
                    cc.ThermoLiq = new(P, T);
                    break;

                default:
                    cc.ThermoLiq = BulkStreamThermo(cc1, cc1.LiqPhaseMolFractions, P, T, enumFluidRegion.Liquid, thermo);
                    cc.ThermoVap = BulkStreamThermo(cc1, cc1.VapPhaseMolFractions, P, T, enumFluidRegion.Vapour, thermo);
                    break;
            }

            if (HasSolids)
            {
                double MassFrac = solids.MoleFractions.Sum();
                double res = 0;
                for (int i = 0; i < solids.Count; i++)
                {
                    SolidComponent sc = (SolidComponent)solids[i];
                    res += sc.Enthalpy(T);
                }
                cc.ThermoSolids.H = res;
                cc.ThermoSolids.S = 0;
            }
            else
            {
                cc.ThermoSolids.H = 0;
                cc.ThermoSolids.S = 0;
            }
        }

        public static ThermoProps SolidEnthalpy(Components solids, Temperature T)
        {
            double MassFrac = solids.MoleFractions.Sum();
            double res = 0;
            for (int i = 0; i < solids.Count; i++)
            {
                SolidComponent sc = (SolidComponent)solids[i];
                res += sc.Enthalpy(T);
            }
            solids.ThermoLiq.H += res; // add solid enthalpy
            solids.ThermoSolids.H = res;
            solids.ThermoSolids.S = 0;
            return solids.ThermoSolids;
        }

        public static double CalcPhaseMolarHeatCapacity(Components cc, Pressure P, Temperature t1, Temperature t2, enumFluidRegion ss)
        {
            double enthalpy;
            double enthalpy2;

            UpdateThermoProperties(cc,P,t1,cc.Thermo,ss);

            if(ss==enumFluidRegion.Liquid)
                enthalpy = cc.ThermoLiq.H;
            else
                enthalpy = cc.ThermoVap.H;

            UpdateThermoProperties(cc, P, t2, cc.Thermo, ss);

            if (ss == enumFluidRegion.Liquid)
                enthalpy2 = cc.ThermoLiq.H;
            else
                enthalpy2 = cc.ThermoVap.H;

            return (enthalpy2 - enthalpy) / (t2 - t1);
        }

        public static Enthalpy EnthalpyFormation25(Components cc, double[] X)
        {
            double res = 0;
            int count = cc.ComponentList.Count;
            for (int i = 0; i < count; i++)
                res += cc.ComponentList[i].HForm25 * X[i];
            return res;
        }

        public static ThermoProps BulkStreamThermo(Components cc, double[] XWet, Pressure P, Temperature T,
            enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (cc.Count == 1)
                XWet[0] = 1;

            if (cc is null || cc.Count == 0 || !P.IsKnown || !T.IsKnown)
                return null;

            double[] XCopy = (double[])XWet.Clone();

            if (cc is null || cc.ComponentList is null)
                return null;

            bool HandleWaterSeperately = false;

            Enthalpy IdealH;
            Entropy IdealS;
            ThermoProps thermoProps = null;
            ThermoProps waterProps = null;
            ThermoProps waterPropsVapSat = null;
            ThermoProps waterPropsLiqSat = null;

            int loc = cc.WaterLocation;

            double watermolefrac = 0;

            if (loc >= 0) // water
            {
                watermolefrac = XWet[loc];
                HandleWaterSeperately = true;
            }

            if (HandleWaterSeperately)
            {
                Temperature BoilPoint = steam.Tsat(P);
                Enthalpy H;
                waterProps = StmPropIAPWS97.WaterPropsMolar(P, T, enumSatType.Normal);

                if (Math.Abs(BoilPoint.BaseValue - T.BaseValue) < 0.000001)
                {
                    waterPropsLiqSat = StmPropIAPWS97.WaterPropsMolar(P, BoilPoint, enumSatType.SatLiq);
                    waterPropsVapSat = StmPropIAPWS97.WaterPropsMolar(P, BoilPoint, enumSatType.SatVap);
                    switch (state)
                    {
                        case enumFluidRegion.Liquid:
                            H = waterPropsLiqSat.H;
                            break;

                        case enumFluidRegion.Vapour:
                            H = waterPropsVapSat.H;
                            break;

                        default:
                            H = waterProps.H;
                            break;
                    }
                }
                else
                    H = waterProps.H;

                waterProps.H = H;

                if (T.Celsius <= 0)
                    waterProps.H += 5 * T.Celsius;

                waterProps.G = waterProps.H.BaseValue - T.BaseValue * waterProps.S.BaseValue;
                waterProps.U = double.NaN;
                thermoProps = waterProps;
                XCopy[loc] = 0; // Set Water to Zero
                XCopy = XCopy.Normalise();
            }

            if (cc.Count > 1 || (cc.Count > 0 && !HandleWaterSeperately))
            {
                IdealH = IdealGas.StreamIdealGasMolarEnthalpy(cc, T, XCopy); // without water
                IdealS = IdealGas.StreamIdealGasMolarEntropy(cc, P, T, XCopy);

                Enthalpy EnthFormation = EnthalpyFormation25(cc, XCopy); // inlcude water here for H-Hig calc?

                switch (thermo.Enthalpy)
                {
                    case enumEnthalpy.Ideal:
                        thermoProps = Ideal.ThermoBulk(cc, P, T, XCopy, state, IdealH, IdealS);
                        thermoProps.H_ig = IdealH;
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                            thermoProps.H_ig += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.LeeKesler:
                    case enumEnthalpy.ChaoSeader:
                    case enumEnthalpy.GraysonStreed:
                        thermoProps = LeeKesler.LeeKeslerThermo(cc, XCopy, P, T, state, IdealH, IdealS);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.PR76:
                        thermoProps = PengRobinson.ThermoBulk(cc, XCopy, P, T, state, IdealH, IdealS, enumPRVariation.PR76, thermo);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.PR78:
                        thermoProps = PengRobinson.ThermoBulk(cc, XCopy, P, T, state, IdealH, IdealS, enumPRVariation.PR78, thermo);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.PRSV:
                        thermoProps = PengRobinson.ThermoBulk(cc, XCopy, P, T, state, IdealH, IdealS, enumPRVariation.PRSV, thermo);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.SRK:
                        thermoProps = SRK.ThermoBulk(cc, P, T, XCopy, state, IdealH, IdealS);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.RK:
                        thermoProps = RK.ThermoBulk(cc, P, T, XCopy, state, IdealH, IdealS);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.BWRS:
                        thermoProps = BWRS.ThermoBulk(cc, XCopy, P, T, IdealH, IdealS);
                        if (Global.DeductHForm25)
                        {
                            thermoProps.H += EnthFormation;
                        }
                        break;

                    case enumEnthalpy.SimpleTest:
                        thermoProps = new ThermoProps(P, T);
                        for (int i = 0; i < cc.ComponentList.Count; i++)
                        {
                            BaseComp bc = cc[i];
                            if (state == enumFluidRegion.Liquid)
                                thermoProps.H += TestProps.LiqEnth(bc.Name, T) * XCopy[i];
                            else
                                thermoProps.H += TestProps.VapEnth(bc.Name, T) * XCopy[i];

                            if (Global.DeductHForm25)
                            {
                                thermoProps.H += EnthFormation;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if ((HandleWaterSeperately && cc.Count == 1) || (loc >= 0 && XWet[loc] == 1)) // only water
            {
                thermoProps = waterProps;
            }
            else if (thermoProps != null && waterProps != null)
            {
                thermoProps.H = thermoProps.H * (1 - watermolefrac) + waterProps.H * watermolefrac;
                thermoProps.S = thermoProps.S * (1 - watermolefrac) + waterProps.S * watermolefrac;
                thermoProps.G = thermoProps.H.BaseValue - T.BaseValue * thermoProps.S.BaseValue;
                thermoProps.U = double.NaN;
            }

            return thermoProps;
        }

        public static ThermoDifferentialPropsCollection ThermoDifferentials(Components cc, Pressure P, Temperature T, double[] MoleFracs,
            enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (cc.Count == 0)
                return null;

            return ThermoDifferentials(cc, MoleFracs, P, T, state, thermo);
        }

        public static ThermoDifferentialPropsCollection ThermoDifferentials(Components cc, double[] MoleFracs,
            Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (cc.Count == 0)
                return null;

            double IdealCp = IdealGas.StreamIdealCp(cc, T, MoleFracs, enumMassOrMolar.Molar);

            ThermoDifferentialPropsCollection props = new();

            switch (thermo.Enthalpy)
            {
                case enumEnthalpy.Ideal:
                    props.Cp = IdealCp;
                    break;

                case enumEnthalpy.LeeKesler:
                case enumEnthalpy.ChaoSeader:
                case enumEnthalpy.GraysonStreed:
                case enumEnthalpy.PR76:
                    props = PengRobinson.DifferentialProps(cc, P, T, state, IdealCp, enumPRVariation.PR76, thermo);
                    break;

                case enumEnthalpy.PR78:
                    props = PengRobinson.DifferentialProps(cc, P, T, state, IdealCp, enumPRVariation.PR78, thermo);
                    break;

                case enumEnthalpy.SRK:
                    props = SRK.DifferentialProps(cc, P, T, state, IdealCp);
                    break;

                case enumEnthalpy.RK:
                    props = RK.DifferentialProps(cc, P, T, state, IdealCp);
                    break;

                case enumEnthalpy.SimpleTest:
                default:
                    break;
            }
            return props;
        }

        public static enumFluidRegion CheckState(Components cc, double[] X, Temperature t, Pressure P, ThermoDynamicOptions thermo)
        {
            if (cc.Count == 0)
                return enumFluidRegion.Undefined;

            enumFluidRegion state = enumFluidRegion.TwoPhase;

            switch (thermo.KMethod)
            {
                case enumEquiKMethod.Antoine:
                    break;

                case enumEquiKMethod.LeeKesler:
                case enumEquiKMethod.ChaoSeader:
                case enumEquiKMethod.GraysonStreed:
                    break;

                case enumEquiKMethod.PR76:
                    state = PengRobinson.GetFluidState(cc, X, t, P, enumPRVariation.PR76, thermo);
                    break;

                case enumEquiKMethod.PR78:
                    state = PengRobinson.GetFluidState(cc, X, t, P, enumPRVariation.PR78, thermo);
                    break;

                case enumEquiKMethod.PRSV:
                    state = PengRobinson.GetFluidState(cc, X, t, P, enumPRVariation.PRSV, thermo);
                    break;

                case enumEquiKMethod.RK:
                    break;

                default:
                    break;
            }
            return state;
        }

        public static double[] LnKMix(Components cc, Pressure P, Temperature t, out enumFluidRegion state, ThermoDynamicOptions thermo, bool IsLLE = false)
        {
            double[] X; double[] Y;
            X = cc.LiqPhaseMolFractions;
            Y = cc.VapPhaseMolFractions;
            return LnKMixArray(cc, P, t, X, Y, out state, thermo, IsLLE);
        }

        public static double[] LnKMixWithEfficiency(Components cc, Pressure P, Temperature T, double[] X, double[] Y, out enumFluidRegion state, ThermoDynamicOptions thermo, double Ev, bool IsLLe = false)
        {
            double[] lnk = LnKMixArray(cc, P, T, X, Y, out state, thermo, IsLLe);

            if (Ev != 1 && Ev != 100)
            {
                if (Ev > 1.5) // in percentages
                    Ev /= 100;

                double[] lnknew = new double[lnk.Length];

                for (int i = 0; i < lnk.Length; i++)
                {
                    double Ystar = Math.Exp(lnk[i]) * X[i];
                    double Ynew = Ev * Ystar + (1 - Ev) * Y[i + 1];
                    lnknew[i] = Math.Log(Ynew / X[i]);
                }

                lnknew[lnknew.Length] = lnk[lnk.Length];

                return lnknew;
            }
            return lnk;
        }

        public static bool IsActivityMethod(ThermoDynamicOptions thermo)
        {
            switch (thermo.KMethod)
            {
                case enumEquiKMethod.RK:
                case enumEquiKMethod.SRK:
                case enumEquiKMethod.Antoine:
                case enumEquiKMethod.LeeKesler:
                case enumEquiKMethod.PR76:
                case enumEquiKMethod.PR78:
                case enumEquiKMethod.PRSV:
                    return false;

                case enumEquiKMethod.GraysonStreed:
                case enumEquiKMethod.ChaoSeader:
                case enumEquiKMethod.Wilson:
                case enumEquiKMethod.UnifacVLE:
                case enumEquiKMethod.UnifacLLE:
                    return true;
            }
            return true;
        }

        public static double[] LnKMixArray(Components cc, Pressure P, Temperature T, double[] Xin, double[] Yin,
            out enumFluidRegion state, ThermoDynamicOptions thermo, bool IsLLE = false, enumFluidRegion fixstate = enumFluidRegion.Undefined)
        {
            enumEquiKMethod method = thermo.KMethod;
            bool handlewaterseperately = false;
            Pressure psat = 1;

            /*if (Components.ContainsTypes(cc_in.CompList,typeof(SolidComponent))) // filter out any solids
            {
                cc = cc_in.RemoveSolids();
            }*/

            double[] X = (double[])Xin.Clone(), Y = (double[])Yin.Clone();

            if (IsLLE)
                method = thermo.KMethodLLE;

            if (!cc.Any())
            {
                state = enumFluidRegion.Undefined;
                return null;
            }

            state = enumFluidRegion.Undefined;
            int CompCount = cc.ComponentList.Count;

            if (double.IsNaN(T))
                return null;

            double[] res = new double[CompCount];

            int loc = cc.IndexOf("H2O", X, out double watermolefrac);

            if (!IsActivityMethod(thermo) & !IsLLE && loc >= 0 && T > 273.15) // e.g. steam
            {
                if (X[loc] > 0.99999) //
                {
                    handlewaterseperately = true;
                    if (X[loc] != 1)
                    {
                        X[loc] = 0;
                        Y[loc] = 0;

                        X = X.Normalise();
                        Y = Y.Normalise();
                    }

                    StmPropIAPWS97 steam = new();
                    psat = steam.Psat(T);
                }
            }

            switch (method)
            {
                case enumEquiKMethod.Antoine:
                    res = lnAntK(cc, P, T);
                    break;

                case enumEquiKMethod.LeeKesler:
                    res = LeeKesler.lnK(cc, P, T);
                    break;

                case enumEquiKMethod.PR76:
                    res = PengRobinson.LnKmix(cc, P, T, X, Y, out state, enumPRVariation.PR76, thermo); // must be intialised
                    break;

                case enumEquiKMethod.PR78:
                    res = PengRobinson.LnKmix(cc, P, T, X, Y, out state, enumPRVariation.PR78, thermo); // must be intialised
                    break;

                case enumEquiKMethod.PRSV:
                    res = PengRobinson.LnKmix(cc, P, T, X, Y, out state, enumPRVariation.PRSV, thermo); // must be intialised
                    break;

                case enumEquiKMethod.SRK:
                    res = SRK.LnKmix(cc, P, T, X, Y, out state);
                    break;

                case enumEquiKMethod.RK:
                    res = RK.LnKmix(cc, P, T, X, Y, out state);
                    break;

                case enumEquiKMethod.BWRS:
                    res = BWRS.LnKmix(cc, P, T, X, Y);
                    break;

                case enumEquiKMethod.ChaoSeader:
                    res = ChaoSeader.GetKReal(cc, P, X, T);
                    break;

                case enumEquiKMethod.GraysonStreed:
                    res = GraysonStreed.GetKReal(cc, X, P, T);
                    break;

                case enumEquiKMethod.UnifacLLE:
                    UNIFAC uf = new UNIFAC();
                    LLEUnifacData LLE = new();
                    res = uf.SolveLNActivity(cc, X, T, LLE);
                    break;

                case enumEquiKMethod.UnifacVLE:
                    uf = new UNIFAC();
                    LLEUnifacData VLE = new();
                    res = uf.SolveLNActivity(cc, X, T, VLE);
                    break;

                case enumEquiKMethod.WilsonActivity:
                    break;

                case enumEquiKMethod.UNIQUAC:
                    UNIQUAC UQ = new();

                    double[][] Params = thermo.UniquacParams;
                    double[] R = cc.UniquacR;
                    double[] Q = cc.UniquacQ;

                    UQ.Init(cc, 273.15 + 25, Params, R, Q);

                    //cc.SetMolFractions(X);
                    var activity = UQ.SolveLnGamma();

                    //cc.SetMolFractions(Y);
                    var activity2 = UQ.SolveLnGamma();

                    double[] K = new double[3];

                    for (int i = 0; i < 3; i++)
                    {
                        K[i] = activity[i] - activity2[i];
                    }

                    res = K;
                    break;

                case enumEquiKMethod.SimpleTest:
                    for (int i = 0; i < cc.ComponentList.Count; i++)
                    {
                        BaseComp bc = cc[i].Clone();
                        bc.MoleFraction = X[i];
                        res[i] = Math.Log(TestProps.VP(bc.Name, T) / P);
                    }
                    break;

                case enumEquiKMethod.Wilson:
                    res = Wilson.LnK(cc, P, T);
                    break;

                default:
                    res = Wilson.LnK(cc, P, T);
                    break;
            }

            if (handlewaterseperately)
                res[loc] = Math.Log(psat.BarA / P);

            return res;
        }

        public static double[] KMixArray(Components cc, Pressure P, Temperature T, double[] X, double[] Y,
         out enumFluidRegion state, ThermoDynamicOptions thermo, bool IsLLE = false)
        {
            state = enumFluidRegion.Undefined;

            if (double.IsNaN(T))
                return null;

            double[] res = LnKMixArray(cc, P, T, X, Y, out state, thermo, IsLLE);

            for (int i = 0; i < res.Length; i++)
                //res[i] = Ext.exp0(res[i]);
                res[i] = Math.Exp(res[i]);

            return res;
        }

        public static double KMix(Components cc, Pressure P, Temperature T, double[] X, double[] Y,
         out enumFluidRegion state, ThermoDynamicOptions thermo, bool IsLLE = false)
        {
            state = enumFluidRegion.Undefined;

            if (double.IsNaN(T))
                return double.NaN;

            double k = 0;

            double[] res = LnKMixArray(cc, P, T, X, Y, out state, thermo, IsLLE);

            for (int i = 0; i < res.Length; i++)
                k += Math.Exp(res[i]) * X[i];

            return k;
        }

        public static double[] LnFugMix(Components cc, Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (double.IsNaN(T.BaseValue))
                return null;

            double[] res = new double[cc.ComponentList.Count];

            switch (cc.Thermo.KMethod)
            {
                case enumEquiKMethod.Antoine:
                    for (int i = 0; i < cc.ComponentList.Count; i++)
                        res[i] = Math.Log(Ant(T, cc[i]));
                    break;

                case enumEquiKMethod.LeeKesler:
                    break;

                case enumEquiKMethod.PR76:
                    res = PengRobinson.LnPhiMix(cc, P, T, state, enumPRVariation.PR76, thermo); // must be intialised
                    break;

                case enumEquiKMethod.PR78:
                    res = PengRobinson.LnPhiMix(cc, P, T, state, enumPRVariation.PR78, thermo); // must be intialised
                    break;

                case enumEquiKMethod.PRSV:
                    res = PengRobinson.LnPhiMix(cc, P, T, state, enumPRVariation.PRSV, thermo); // must be intialised
                    break;

                case enumEquiKMethod.RK:
                    res = RK.LnFugMix(cc, P, T, state); // must be intialised
                    break;

                case enumEquiKMethod.SRK:
                    res = RK.LnFugMix(cc, P, T, state); // must be intialised
                    break;

                default:
                    break;
            }
            return res;
        }

        public static double[] GibbsMix(Components cc, Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            double t = T;

            if (double.IsNaN(t))
                return null;

            Enthalpy[] Enthalpy = IdealGas.StreamIdealGasMolarEnthalpies(cc, T);
            Entropy[] Entropy = IdealGas.StreamIdealGasMolarEntropies(cc, P, T);

            double[] res = new double[cc.ComponentList.Count];

            switch (cc.Thermo.KMethod)
            {
                case enumEquiKMethod.Antoine:
                    for (int i = 0; i < cc.ComponentList.Count; i++)
                        res[i] = Math.Log(Ant(t, cc[i]));
                    break;

                case enumEquiKMethod.LeeKesler:
                    break;

                case enumEquiKMethod.PR76:
                    res = PengRobinson.GibbsMix(cc, P, T, Enthalpy, Entropy, state, enumPRVariation.PR76, thermo); // must be intialised
                    break;

                case enumEquiKMethod.PR78:
                    res = PengRobinson.GibbsMix(cc, P, T, Enthalpy, Entropy, state, enumPRVariation.PR78, thermo); // must be intialised
                    break;

                case enumEquiKMethod.PRSV:
                    res = PengRobinson.GibbsMix(cc, P, T, Enthalpy, Entropy, state, enumPRVariation.PRSV, thermo); // must be intialised
                    break;

                case enumEquiKMethod.RK:
                    //res = RK.LnFugMix(cc, state); // must be intialised
                    break;

                case enumEquiKMethod.SRK:
                    // res = RK.LnFugMix(cc, state); // must be intialised
                    break;

                default:
                    break;
            }
            return res;
        }

        public static Enthalpy BulkStreamEnthalpy(Components cc, double[] x, Pressure P, Temperature T, enumFluidRegion state, ThermoDynamicOptions thermo)
        {
            if (cc.Count == 0 || double.IsNaN(T) || cc.Count != x.Length)
                return double.NaN;

            return BulkStreamThermo(cc, x, P, T, state, thermo).H;
        }

        public static double LiqHeatCapacity(Components cc, Pressure P, Temperature T, enumMassOrMolar mm, ThermoDynamicOptions thermo)
        {
            double res = BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo).H;
            T += 1;
            res -= BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo).H;
            T -= 1;

            if (mm == enumMassOrMolar.Mass)
                return res / cc.MW();
            else
                return res;
        }

        public static double VapHeatCapacity(Components cc, Pressure P, Temperature T, enumMassOrMolar mm, ThermoDynamicOptions thermo)
        {
            double res = BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo).H;
            T += 1;
            res -= BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo).H;
            T = -1;

            if (mm == enumMassOrMolar.Mass)
                return res / cc.MW();
            else
                return res;
        }

        public static double EnthalpyVaporisation(Components cc, Pressure P, Temperature T, enumMassOrMolar mm, ThermoDynamicOptions thermo)
        {
            double res = BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Vapour, thermo).H
               - BulkStreamThermo(cc, cc.MoleFractions, P, T, enumFluidRegion.Liquid, thermo).H;

            if (mm == enumMassOrMolar.Mass)
                return res / cc.MW();
            else
                return res;
        }

        public static Pressure CalcBubblePointP(Components cc, Temperature T, ThermoDynamicOptions thermo)
        {
            if (cc.Count == 0)
                return new Pressure(double.NaN);

            Pressure res = double.NaN;

            switch (thermo.KMethod)
            {
                case enumEquiKMethod.PR76:
                    res.MPa = PengRobinson.PSat(cc, T, enumPRVariation.PR76);
                    break;

                case enumEquiKMethod.PR78:
                    res.MPa = PengRobinson.PSat(cc, T, enumPRVariation.PR78);
                    break;

                case enumEquiKMethod.PRSV:
                    res.MPa = PengRobinson.PSat(cc, T, enumPRVariation.PRSV);
                    break;

                case enumEquiKMethod.SRK:
                    res.MPa = SRK.PSat(cc, T);
                    break;

                case enumEquiKMethod.RK:
                    res.MPa = RK.PSat(cc, T);
                    break;

                case enumEquiKMethod.LeeKesler:
                    res = LeeKesler.PSat(cc, T);
                    break;

                case enumEquiKMethod.ChaoSeader:
                    res = ChaoSeader.PSat(cc, T);
                    break;

                case enumEquiKMethod.GraysonStreed:
                    res = GraysonStreed.PSat(cc, T);
                    break;

                case enumEquiKMethod.Antoine: //  antoine for pure, LK for quasi Components
                    res = Ant(T, cc[0]);
                    break;

                case enumEquiKMethod.SimpleTest: //  antoine for pure, LK for quasi Components
                    foreach (BaseComp bc in cc)
                        res += SimpleProps.TestProps.VP(bc.Name, T) * bc.MoleFraction;
                    break;

                default:
                    res = -999;
                    break;
            }
            return res;
        }

        public static Temperature DewPoint(Components cc, Pressure P, ThermoDynamicOptions thermo, out enumFluidRegion state)
        {
            double Delta, perturb = 0.01;
            int Count = 0;
            state = enumFluidRegion.Undefined;
            Temperature T7 = cc.TCritMix() * 0.7;
            Pressure PCrit = cc.PCritMix();

            //double  res = BrentSolver.Solve(LowestTBoil-20, HighestTBoil+10, 1, dew.DewFunc,0.1);

            DewPointClass dew = new(cc, P);
            double KnDivSum = dew.DewFunc(T7, out double[] Kn);
            double KnDivSum2 = dew.DewFunc(T7 + perturb, out double[] Kn2);

            if (Kn is null)
                return double.NaN;

            do
            {
                var Grad = (KnDivSum - KnDivSum2) / perturb;
                var Error = 1 - KnDivSum;
                Delta = -Error / Grad;

                if (Delta > 10)
                    Delta = 10;
                else if (Delta < -9) // use different number to avoid   oscillating
                    Delta = -9;

                T7 += Delta;

                KnDivSum = dew.DewFunc(T7, out Kn);
                KnDivSum2 = dew.DewFunc(T7 + perturb, out Kn);

                if (Kn is null || Kn2 is null)
                    return double.NaN;

                Count++;
            } while (Math.Abs(Delta) > 0.0000001 && Count < 100);

            if (Count < 100)
                return T7;
            else
                return double.NaN;
        }

        public static Temperature DewPointOld(Components cc, Pressure P, ThermoDynamicOptions thermo, out enumFluidRegion state)
        {
            double Delta, perturb = 0.01;
            Temperature LowestTBoil = Components.GetHighestTBoil(cc, out int index);
            double[] X = (double[])cc.MoleFractions.Normalise();
            double[] Y = (double[])cc.MoleFractions.Normalise();
            Temperature TCrit = cc.TCritMix();
            Temperature T7 = TCrit * 0.7;
            Pressure PCrit = cc.PCritMix();

            double[] Kn = ThermodynamicsClass.KMixArray(cc, P, T7, X, Y, out state, thermo);
            double[] Kn2 = ThermodynamicsClass.KMixArray(cc, P, T7 + perturb, X, Y, out state, thermo);

            double KnDimSum = Y.Divide(Kn).Sum();
            double KnDimSum2 = Y.Divide(Kn2).Sum();

            if (Kn is null)
                return double.NaN;

            int Count = 0;

            do
            {
                var Grad = (KnDimSum - KnDimSum2) / perturb;
                var Error = 1 - KnDimSum;
                Delta = -Error / Grad;

                if (Delta > 10)
                    Delta = 10;
                else if (Delta < -9) // use different number to avoid oscillating
                    Delta = -9;

                T7 += Delta;

                Kn = ThermodynamicsClass.KMixArray(cc, P, T7, X, Y, out state, thermo);
                Kn2 = ThermodynamicsClass.KMixArray(cc, P, T7 + perturb, X, Y, out state, thermo);

                if (Kn is null)
                    return double.NaN;

                KnDimSum = Y.Divide(Kn).Sum();
                KnDimSum2 = Y.Divide(Kn2).Sum();

                if (Kn is null || Kn2 is null)
                    return double.NaN;

                for (int i = 0; i < X.Length; i++)
                    X[i] = Y[i] / Kn[i];

                X = X.Normalise();

                Count++;
            } while (Math.Abs(Delta) > 0.0000001 && Count < 100);

            if (Count < 100)
                return T7;
            else
                return double.NaN;
        }

        public static Temperature BubblePoint(Components cc, Pressure P, ThermoDynamicOptions thermo, out enumFluidRegion state)
        {
            state = enumFluidRegion.Undefined;
            Temperature Test = 0.7 * cc.TCritMix();
            double Tdelta = 50;
            int IterCount = 0;
            Temperature TLow = double.NaN, THigh = double.NaN;

            BoilingPointClass BP = new(cc, P);
            bool LowSet = false, HighSet = false;
            double KnSum;
            double[] Kn;
            do
            {
                do
                {
                    KnSum = BP.BoilingPointFunc(Test, out Kn, out state);

                    if (KnSum > 1)
                    {
                        THigh = Test;
                        Test -= Tdelta;
                        HighSet = true;
                        if (Test < 0)
                        {
                            Test = 0.1;
                        }
                    }
                    else if (KnSum < 1)
                    {
                        TLow = Test;
                        Test += Tdelta;
                        LowSet = true;
                        if (Test < 0)
                        {
                            Test = 0.1;
                        }
                    }
                    else if (KnSum.AlmostEquals(1))
                    {
                        Tdelta = 0;
                        break;
                    }

                    if (HighSet && LowSet)
                        break;

                    IterCount++;
                } while (IterCount < 500);

                Tdelta /= 2;
            } while (IterCount < 500 && Tdelta > 0.0005);

            if (IterCount == 500)
            {
                if (!LowSet)
                    return 1;

                return double.NaN;
            }

            return Test;
        }

        public static Temperature BubblePointOld(Components cc, Pressure P, ThermoDynamicOptions thermo, out enumFluidRegion state)
        {
            double Delta, perturb = 0.01;
            Temperature LowestTBoil = Components.GetLowestTBoil(cc, out int index);
            double[] X = (double[])cc.MoleFractions.Normalise();
            double[] Y = new double[cc.Count];
            Temperature TCrit = cc.TCritMix();
            Temperature T7 = TCrit * 0.7;
            Pressure PCrit = cc.PCritMix();
            state = enumFluidRegion.Undefined;

            if (P > PCrit)
                return double.NaN;

            int LowestBP = cc.MinNonZeroBoiling;
            Y[LowestBP] = 1;

            double[] Kn = ThermodynamicsClass.KMixArray(cc, P, T7, X, Y, out state, thermo);
            double[] Kn2 = ThermodynamicsClass.KMixArray(cc, P, T7 + perturb, X, Y, out state, thermo);

            double KnSum = Kn.Mult(X).Sum();
            double KnSum2 = Kn2.Mult(X).Sum();

            int Count = 0;
            do
            {
                var Grad = (KnSum - KnSum2) / perturb;
                var Error = 1 - KnSum;
                Delta = -Error / Grad;

                if (Delta > 20)
                    Delta = 20;
                else if (Delta < -19)
                    Delta = -19;

                T7 += Delta;

                if (double.IsNaN(T7))
                    return double.NaN;

                for (int i = 0; i < X.Length; i++)
                    Y[i] = X[i] * Kn[i];

                Y = Y.Normalise();

                KnSum = ThermodynamicsClass.KMix(cc, P, T7, X, Y, out state, thermo);
                KnSum2 = ThermodynamicsClass.KMix(cc, P, T7 + perturb, X, Y, out state, thermo);

                Count++;
            } while (Math.Abs(Delta) > 0.0000001 && Count < 100);

            if (Count < 100)
                return T7;
            else
                return double.NaN;
        }

        private static double Ant(double t, BaseComp sc)
        {
            if (sc.IsPure)
            {
                double AK;
                AK = Math.Exp(sc.AntK[0] + sc.AntK[1] / (t + sc.AntK[2]) + sc.AntK[3] * Math.Log(t) + sc.AntK[4] * Math.Pow(t, sc.AntK[5]));
                return AK;  // Bar
            }
            else
            {
                double AK;
                AK = LeeKesler.Psat(t, sc.SG_60F, sc.MeABP, sc.CritT, sc.CritP, sc.Omega);
                return AK;
            }
        }

        private static Pressure Ant(Components cc, Temperature T, double[] X)
        {
            BaseComp sc;
            double res = 0;
            double t = T.BaseValue;

            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                sc = cc[i];

                if (sc.IsPure)
                    res += X[i] * Math.Exp(sc.AntK[0] + sc.AntK[1] / (t + sc.AntK[2]) + sc.AntK[3] * Math.Log(t) + sc.AntK[4] * Math.Pow(t, sc.AntK[5])) / 100;
                else
                    res += X[i] * LeeKesler.Psat(t, sc.SG_60F, sc.MeABP, sc.CritT, sc.CritP, sc.Omega);
            }
            return new Pressure(res);
        }

        private static double[] lnAntK(Components cc, Pressure P, Temperature T)
        {
            BaseComp sc;
            double t = T._Kelvin;
            Pressure p = P;

            double[] res = new double[cc.ComponentList.Count];
            for (int i = 0; i < cc.ComponentList.Count; i++)
            {
                sc = cc[i];
                if (sc.IsPure)
                    res[i] = sc.AntK[0] + sc.AntK[1] / (t + sc.AntK[2]) + sc.AntK[3] * Math.Log(t) + sc.AntK[4] * Math.Pow(t, sc.AntK[5]) - Math.Log(p);
                else
                    res[i] = Math.Log(LeeKesler.Psat(t, sc.SG_60F, sc.MeABP, sc.CritT, sc.CritP, sc.Omega) / p);
            }
            return res;
        }
    }
}