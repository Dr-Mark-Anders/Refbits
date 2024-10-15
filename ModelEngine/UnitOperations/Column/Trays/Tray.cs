using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class Tray : ISerializable
    {
        private Rectangle location = new();
        private Color colour = new();
        private string name;
        private Guid guid;
        public UOMProperty TrayEff = new(ePropID.Quality, 100);
        public int TrayIndex = 0;

        public EnthalpyDepartureLinearisation LinearDepEnthalpies = new();
        public EnthalpySimpleLinearisation LinearSimpleEnthalpies = new();

        public void UpdateLiquidLinearEnthalpies(Components cc, ThermoDynamicOptions thermo)
        {
            LinearSimpleEnthalpies.LiqUpdate(cc, this.LiqComposition, this.P.BaseValue, this.T, this.T + EnthalpyDeltaT, thermo);
        }

        public void UpdateLiquidDepEnthalpies(Components cc, ThermoDynamicOptions thermo)
        {
            LinearDepEnthalpies.LiqUpdate(cc, this.LiqComposition, this.P.BaseValue, this.T, this.T + EnthalpyDeltaT, thermo);
        }

        public void UpdateVapourLinearEnthalpies(Components cc, ThermoDynamicOptions thermo)
        {
            LinearSimpleEnthalpies.VapUpdate(cc, this.VapComposition, this.P.BaseValue, this.T, this.T + EnthalpyDeltaT, thermo);
        }

        public void UpdateVapourDepEnthalpies(Components cc, ThermoDynamicOptions thermo)
        {
            LinearDepEnthalpies.VapUpdate(cc, this.VapComposition, this.P.BaseValue, this.T, this.T + EnthalpyDeltaT, thermo);
        }

        public double[] LiqCompositionInitial;
        public double[] VapCompositionInitial;

        public double[] LiqCompositionPred;
        public double[] VapCompositionPred;

        public double[] LiqComposition;
        public double[] VapComposition;

        public double[] Composition;

        private double EnthalpyDeltaT = 1;

        public double StripFact;
        public double LiqDrawFactor;
        public double VapDrawFactor;

        public double TempLiqDrawFactor;
        public double TempVapDrawFactor;
        public double TempStripFact;
        public double TempT;

        public double[] LiquidEnthalpiesLinearised;
        public double[] VapourEnthalpiesLinearised;
        public double[] LiquidEnthalpiesLinearised2;
        public double[] VapourEnthalpiesLinearised2;
        public double EnthalpyBaseT;

        public double LiqEnthDepLin;
        public double VapEnthDepLin;
        public double LiqEnthDepLin2;
        public double VapEnthDepLin2;

        public double VapCcoeff, VapDcoeff;
        public double LiqCcoeff, LiqDcoeff;
        public double VapTr, LiqTr;
        public double VapTC_PC, LiqTC_PC;
        public double VapDep, LiqDep; /// departures

        private PortList ports = new();

        public double T, TPredicted;
        //public  Pressure  P = double.NaN;

        public UOMProperty P = new(ePropID.P, double.NaN);

        public UOMProperty Tuom = new(ePropID.T, double.NaN);

        public bool SplitFeed = false;

        public Port_Material feed = new("Feed", FlowDirection.IN);
        public Port_Material liquidDrawRight = new("LiquidDrawRight", FlowDirection.OUT, false);
        //public Port_Material liquidDrawLeft = new("LiquidDrawLeft", FlowDirection.OUT, false);
        public Port_Material vapourDraw = new("VapourDraw", FlowDirection.OUT, false);
        public Port_Material TrayVapour = new("TrayVapour", FlowDirection.OUT, false);
        public Port_Material TrayLiquid = new("TrayLiquid", FlowDirection.OUT, false);
        public Port_Material WaterDraw = new("WaterDraw", FlowDirection.OUT, false);
        //public  Port_Material PADraw = new ("PADraw", FlowDirection.OUT, false);

        public double V, L, lss_spec, lss_estimate, balance, vsstemp, liqenth, vapenth, WaterEstimate;
        public double vss_estimate, vss_spec, vss;
        public double Vtemp, Ltemp;

        public double LiqEnthalpy(IColumn col, Temperature T)
        {
            Components cc = col.Components;
            
            double result;
            switch (col.SolverOptions.LiqEnthalpyMethod)
            {
                case ColumnEnthalpyMethod.Rigorous:
                    result = ThermodynamicsClass.BulkStreamEnthalpy(cc, LiqCompositionPred, P.BaseValue, T, enumFluidRegion.Liquid, col.Thermo);
                    return result;

                case ColumnEnthalpyMethod.BostonBrittHdep:
                    result = LinearDepEnthalpies.LiqEnthalpy(cc, LiqCompositionPred, P.BaseValue, T);
                    return result;

                case ColumnEnthalpyMethod.Boston:
                    result = LinearDepEnthalpies.LiqEnthalpy(cc, LiqCompositionPred, P.BaseValue, T);
                    return result;

                case ColumnEnthalpyMethod.SimpleLinear:
                    result = LinearSimpleEnthalpies.LiqEstimate(T, cc.MW(LiqCompositionPred));
                    //result = Thermodynamicsclass .BulkStreamEnthalpy(cc, PredLiqComposition, P.BaseValue, T, enumFluidRegion.Liquid, col.Thermo);
                    return result;

                case ColumnEnthalpyMethod.TestLLE:
                    result = LLEEnthalpyTest(T, VapCompositionPred);
                    return result;

                default:
                    return double.NaN;
            }
        }

        public double SG()
        {
            Components cc = column.Components;
            return cc.SG(LiqCompositionPred);
        }

        public double MW()
        {
            Components cc = column.Components;
            return cc.MW(LiqCompositionPred);
        }

        public double HeavyPhaseEnthalpy(Temperature T)
        {
            Components cc = column.Components;
            return (double)ThermodynamicsClass.BulkStreamEnthalpy(cc.Clone(), LiqCompositionPred, P.BaseValue, T, enumFluidRegion.Liquid, column.Thermo);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public double VapEnthalpy(IColumn col, Temperature T)
        {
            Components cc = col.Components;
            double result;
            switch (col.SolverOptions.VapEnthalpyMethod)
            {
                case ColumnEnthalpyMethod.SimpleLinear:// Doesnt work
                    result = LinearSimpleEnthalpies.VapEstimate(T, cc.MW(VapCompositionPred));
                    return result;

                case ColumnEnthalpyMethod.Rigorous:
                    result = ThermodynamicsClass.BulkStreamEnthalpy(cc, VapCompositionPred, P.BaseValue, T, enumFluidRegion.Vapour, col.Thermo);
                    return result;

                case ColumnEnthalpyMethod.BostonBrittHdep:
                    result = LinearDepEnthalpies.VapEnthalpy(cc, VapCompositionPred, P.BaseValue, T);
                    return result;

                case ColumnEnthalpyMethod.Boston:
                    result = LinearDepEnthalpies.VapEnthalpy(cc, VapCompositionPred, P.BaseValue, T);
                    return result;

                default:
                    return double.NaN;
            }
        }

        public double LightPhaseEnthalpy(Temperature T)
        {
            Components cc = column.Components;
            var result = ThermodynamicsClass.BulkStreamEnthalpy(cc.Clone(), VapCompositionPred, P.BaseValue, T, enumFluidRegion.Liquid, column.Thermo);
            return result;
        }

        public double LLEEnthalpyTest(Temperature T, double[] x)
        {
            return x[0] * 75.6955631003622 * (T.Celsius - 25)
                + x[1] * 153.669998751979 * (T.Celsius - 25)
                + x[2] * 139.662965979802 * (T.Celsius - 25);
        }

        internal void FlashFeedPort()
        {
            feed.Flash();
        }

        internal void FlashFeedPorts(enumEnthalpy enthalpy, enumFlashType flashtype = enumFlashType.None)
        {
            feed.cc.Thermo.Enthalpy = enthalpy;
            feed.Flash(true, flashtype);
        }

        internal void SetUpPorts(IColumn column, bool FlashAll)
        {
            Components cc = column.Components;
            if (FlashAll)
            {
                foreach (var item in ports)
                {
                    item.Clear();
                    item.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    item.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    item.SetPortValue(ePropID.MOLEF, this.lss_estimate, SourceEnum.UnitOpCalcResult);
                    item.cc.Clear();
                    item.cc.Add(cc);
                    item.cc.ClearMoleFractions();
                    item.cc.Origin = SourceEnum.UnitOpCalcResult;
                    item.SetMoleFractions(this.LiqComposition);
                    item.cc.NormaliseFractions();
                }
            }
            else
            {
                if (WaterDraw.IsConnected)
                {
                    WaterDraw.Clear();
                    WaterDraw.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    WaterDraw.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    WaterDraw.SetPortValue(ePropID.MOLEF, this.WaterEstimate, SourceEnum.UnitOpCalcResult);
                    WaterDraw.cc.Clear();
                    WaterDraw.cc.Add(cc);
                    WaterDraw.cc.ClearMoleFractions();
                    WaterDraw.cc.Origin = SourceEnum.UnitOpCalcResult;
                    if (WaterDraw.cc.Contains("H2O"))
                        WaterDraw.cc["H2O"].MoleFraction = 1;
                    WaterDraw.cc.NormaliseFractions();
                }

                if (liquidDrawRight.IsConnected)
                {
                    liquidDrawRight.Clear();
                    liquidDrawRight.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    liquidDrawRight.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    liquidDrawRight.SetPortValue(ePropID.MOLEF, this.lss_estimate, SourceEnum.UnitOpCalcResult);
                    liquidDrawRight.cc.Clear();
                    liquidDrawRight.cc.Add(cc);
                    liquidDrawRight.cc.Origin = SourceEnum.UnitOpCalcResult;
                    liquidDrawRight.SetMoleFractions(this.LiqComposition);
                }

               /* if (liquidDrawLeft.IsConnected && !liquidDrawLeft.IsPADraw)
                {
                    liquidDrawLeft.Clear();
                    liquidDrawLeft.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    liquidDrawLeft.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    liquidDrawLeft.SetPortValue(ePropID.MOLEF, this.lss_estimate, SourceEnum.UnitOpCalcResult);
                    liquidDrawLeft.cc.Clear();
                    liquidDrawLeft.cc.Add(cc);
                    liquidDrawLeft.cc.Origin = SourceEnum.UnitOpCalcResult;
                    liquidDrawLeft.SetMoleFractions(this.LiqComposition);
                }*/

                if (vapourDraw.IsConnected)
                {
                    vapourDraw.Clear();
                    vapourDraw.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    vapourDraw.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    vapourDraw.SetPortValue(ePropID.MOLEF, this.vss_estimate, SourceEnum.UnitOpCalcResult);
                    vapourDraw.cc.Clear();
                    vapourDraw.cc.Add(cc);
                    vapourDraw.cc.Origin = SourceEnum.UnitOpCalcResult;
                    vapourDraw.SetMoleFractions(this.VapComposition);
                }

                if (TrayLiquid.IsConnected)
                {
                    TrayLiquid.Clear();
                    TrayLiquid.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    TrayLiquid.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    TrayLiquid.SetPortValue(ePropID.MOLEF, this.L, SourceEnum.UnitOpCalcResult);
                    TrayLiquid.cc.Clear();
                    TrayLiquid.cc.Add(cc);
                    TrayLiquid.cc.Origin = SourceEnum.UnitOpCalcResult;
                    TrayLiquid.SetMoleFractions(this.LiqComposition);
                }

                if (TrayVapour.IsConnected)
                {
                    TrayVapour.Clear();
                    TrayVapour.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    TrayVapour.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    TrayVapour.SetPortValue(ePropID.MOLEF, this.V, SourceEnum.UnitOpCalcResult);
                    TrayVapour.cc.Clear();
                    TrayVapour.cc.Add(cc);
                    TrayVapour.cc.Origin = SourceEnum.UnitOpCalcResult;
                    TrayVapour.SetMoleFractions(this.VapComposition);
                }
            }
        }

        internal void FlashOutPorts(IColumn column, bool FlashAll)
        {
            Components cc = column.Components;
            if (FlashAll)
            {
                foreach (var item in ports)
                {
                    item.SetPortValue(ePropID.T, this.T, SourceEnum.UnitOpCalcResult);
                    item.SetPortValue(ePropID.P, this.P, SourceEnum.UnitOpCalcResult);
                    item.SetPortValue(ePropID.MOLEF, this.lss_estimate, SourceEnum.UnitOpCalcResult);
                    item.cc.Clear();
                    item.cc.Add(cc);
                    item.cc.ClearMoleFractions();
                    item.cc.Origin = SourceEnum.UnitOpCalcResult;
                    item.SetMoleFractions(this.LiqComposition);
                    item.cc.NormaliseFractions();
                }
            }
            else
            {
                if (WaterDraw.IsConnected)
                    WaterDraw.Flash(thermo: column.Thermo);

                if (liquidDrawRight.IsConnected)
                    liquidDrawRight.Flash(thermo: column.Thermo);

               /* if (liquidDrawLeft.IsConnected && !liquidDrawLeft.IsPADraw)
                    liquidDrawLeft.Flash(thermo: column.Thermo);*/

                if (vapourDraw.IsConnected)
                    vapourDraw.Flash(thermo: column.Thermo);

                if (TrayLiquid.IsConnected)
                    TrayLiquid.Flash(forceflash: true, thermo: column.Thermo);

                if (TrayVapour.IsConnected)
                    TrayVapour.Flash(thermo: column.Thermo);
            }
        }

        internal double LiqFlow(enumflowtype flowType, double MW, double SG)
        {
            double res = double.NaN;
            switch (flowType)
            {
                case enumflowtype.Molar:
                    res = L;
                    break;

                case enumflowtype.Mass:
                    res = L * MW;
                    break;

                case enumflowtype.StdLiqVol:
                    res = L * MW / SG;
                    break;
            }
            return res;
        }

        internal double VapFlow(enumflowtype flowType, double MW, double SG)
        {
            double res = double.NaN;
            switch (flowType)
            {
                case enumflowtype.Molar:
                    res = V;
                    break;

                case enumflowtype.Mass:
                    res = V * MW;
                    break;

                case enumflowtype.StdLiqVol:
                    res = V * MW / SG;
                    break;
            }
            return res;
        }

        internal double VssFlow(enumflowtype flowType, double MW, double SG)
        {
            double res = double.NaN;
            switch (flowType)
            {
                case enumflowtype.Molar:
                    res = vss_estimate;
                    break;

                case enumflowtype.Mass:
                    res = vss_estimate * MW;
                    break;

                case enumflowtype.StdLiqVol:
                    res = vss_estimate * MW / SG;
                    break;
            }
            return res;
        }

        internal double lssFlow(enumflowtype flowType, double MW, double SG)
        {
            double res = double.NaN;
            switch (flowType)
            {
                case enumflowtype.Molar:
                    res = lss_estimate;
                    break;

                case enumflowtype.Mass:
                    res = lss_estimate * MW;
                    break;

                case enumflowtype.StdLiqVol:
                    res = lss_estimate * MW / SG;
                    break;
            }
            return res;
        }

        public double[] KTray;

        public void UpdateTrayK(ColumnKMethod method)
        {
            for (int i = 0; i < LiqComposition.Length; i++)
            {
                KTray[i] = K_TestFast(i, T, method);
                if (KTray[i] > 1e20)
                    KTray[i] = 1e20;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public double K_TestOld(int comp, double Tk, ColumnKMethod method)
        {
            //if (double .IsNaN(Tk))
            //return   double.NaN;

            switch (method)
            {
                case ColumnKMethod.Rigorous:
                    return double.NaN;

                case ColumnKMethod.LogLinear:
                    return LLinear.K(comp, Tk);

                case ColumnKMethod.Linear:
                    return Linear.K(comp, Tk);

                case ColumnKMethod.BostonMethod:
                    return double.NaN;

                case ColumnKMethod.LLE:
                    if (LLE_K is null)
                        return double.NaN;
                    return LLE_K[comp];

                case ColumnKMethod.MA:
                    return KB_MA.K(comp, Tk); // apply corrections for component

                default:
                    return double.NaN;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public double K_TestFast(int comp, double Tk, ColumnKMethod method)
        {
            switch (method)
            {
                case ColumnKMethod.MA:
                    return KB_MA.K_SimpleRatio(comp, Tk); // apply corrections for component
                case ColumnKMethod.Rigorous:
                    return double.NaN;

                case ColumnKMethod.LogLinear:
                    double res = LLinear.K(comp, Tk);
                    if (res > 1e5)
                        return 1e5;
                    else
                        return res;

                case ColumnKMethod.Linear:
                    return Linear.K(comp, Tk);
            }
            return double.NaN;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public double LnK_Test(int comp, double Tk, ColumnAlphaMethod method)
        {
            return LLinear.LnKComp[comp] + LLinear.lnKCompGrad[comp] * (Tk - LLinear.BaseT);
        }

        private Func<UnitOperation> onRequestParentObject;

        internal Temperature Told;
        public double enthalpyBalance = 0;
        public double trayError = 0; // e.g. enthalp balance or spec error
        public double baseTrayError = 0; // e.g. enthalp balance or spec error
        internal IColumn column;
        internal double WaterDrawFactor = 1e5;
        internal double[] LnKCompOld;
        private double RigDeltaT = 1;

        public UnitOperation GetParent()
        {
            if (OnRequestParentObject == null)
                return null;

            return OnRequestParentObject();
        }

        public Rectangle Location
        {
            get { return location; }
            set { location = value; }
        }

        public bool Contains(Point p)
        {
            return location.Contains(p);
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Guid Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        public double FeedEnthalpyVap
        {
            get
            {
                return feed.VapourEnthalpy;
            }
        }

        public double FeedEnthalpyLiq
        {
            get
            {
                return feed.LiquidEnthalpy;
            }
        }

        public PortList Ports { get => ports; set => ports = value; }
        public bool IsHeatBalanced { get; set; } = true;
        public EnergyFlow Duty { get; set; } = 0;
        public Func<UnitOperation> OnRequestParentObject { get => onRequestParentObject; set => onRequestParentObject = value; }
        public double[] LLE_K { get; internal set; }
        public double[] ActivityY { get; internal set; }

        public Enthalpy PA_liqenth(IColumn col, Specification spec)
        {
            Temperature Tt;
            Enthalpy enthalpy;
            switch (spec.engineSpecType)
            {
                case eSpecType.PADeltaT:
                    Tt = this.T - (Temperature)spec.SpecValue;
                    enthalpy = LiqEnthalpy(col, Tt);
                    break;

                case eSpecType.PADuty:
                    enthalpy = liqenth - (Enthalpy)spec.SpecValue;
                    break;

                case eSpecType.PARetT:
                    Tt = new(spec.SpecValue);
                    enthalpy = LiqEnthalpy(col, Tt);
                    break;

                default:
                    enthalpy = new Enthalpy(double.NaN);
                    break;
            }
            return enthalpy;
        }

        internal void UnBackupFactors()
        {
            LiqDrawFactor = TempLiqDrawFactor;
            VapDrawFactor = TempVapDrawFactor;
            StripFact = TempStripFact;
            T = TempT;
        }

        public void BackupFactors()
        {
            TempLiqDrawFactor = LiqDrawFactor;
            TempVapDrawFactor = VapDrawFactor;
            TempStripFact = StripFact;
            TempT = T;
        }

        public Tray(IColumn column)
        {
            this.column = column;
            colour = Color.Black;
            guid = Guid.NewGuid();
            feed.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0);
            liquidDrawRight.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0);
            vapourDraw.MolarFlow_ = new StreamProperty(ePropID.MOLEF, 0);

            Ports.Add(feed);
            Ports.Add(liquidDrawRight);
            Ports.Add(vapourDraw);
            Ports.Add(TrayVapour);
            Ports.Add(TrayLiquid);
            Ports.Add(WaterDraw);

            feed.Owner = (UnitOperation)this.column;
            liquidDrawRight.Owner = (UnitOperation)this.column;
            TrayVapour.Owner = (UnitOperation)this.column;
            TrayLiquid.Owner = (UnitOperation)this.column;
            WaterDraw.Owner = (UnitOperation)this.column;
        }

        public Tray(SerializationInfo info, StreamingContext context)
        {
            try
            {
                guid = (Guid)info.GetValue("guid", typeof(Guid));
                T = info.GetDouble("T");
                L = info.GetDouble("L");
                V = info.GetDouble("V");
                P = (UOMProperty)info.GetValue("P", typeof(UOMProperty));
                TrayEff = (UOMProperty)info.GetValue("Eff", typeof(UOMProperty));
            }
            catch { }

            if (guid == Guid.Empty)
                guid = Guid.NewGuid();

            ports.Clear();

            feed = new Port_Material("Feed", FlowDirection.IN);
            liquidDrawRight = new Port_Material("LiquidDrawRight", FlowDirection.OUT, false);
            //liquidDrawLeft = new Port_Material("LiquidDrawLeft", FlowDirection.OUT, false);
            vapourDraw = new Port_Material("VapourDraw", FlowDirection.OUT, false);
            TrayVapour = new Port_Material("TrayVapour", FlowDirection.OUT, false);
            TrayLiquid = new Port_Material("TrayLiqud", FlowDirection.OUT, false);
            WaterDraw = new Port_Material("WaterDraw", FlowDirection.OUT, false);
            //PADraw = new  Port_Material("PADraw", FlowDirection.OUT, false);

            Ports.Add(feed);
            Ports.Add(liquidDrawRight);
            //Ports.Add(liquidDrawLeft);
            Ports.Add(vapourDraw);
            Ports.Add(TrayLiquid);
            Ports.Add(TrayVapour);
            Ports.Add(WaterDraw);

            feed.Name = "Tray Feed";
            liquidDrawRight.Name = "Liquid Draw";
            vapourDraw.Name = "Vapour Draw";
            feed.PortPropertyChanged += Feed_MainPortValueChanged;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("P", P, typeof(UOMProperty));
            info.AddValue("guid", Guid);
            info.AddValue("T", T);
            info.AddValue("L", L);
            info.AddValue("V", V);
            info.AddValue("Eff", TrayEff, typeof(UOMProperty));
        }

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            if (feed != null)
                feed.OnRequestParent += new Func<UnitOperation>(delegate { return GetParent(); });
            if (liquidDrawRight != null)
                liquidDrawRight.OnRequestParent += new Func<UnitOperation>(delegate { return GetParent(); });
            if (TrayVapour != null)
                TrayVapour.OnRequestParent += new Func<UnitOperation>(delegate { return GetParent(); });
            if (TrayLiquid != null)
                TrayLiquid.OnRequestParent += new Func<UnitOperation>(delegate { return GetParent(); });
            if (WaterDraw != null)
                WaterDraw.OnRequestParent += new Func<UnitOperation>(delegate { return GetParent(); });
        }

        internal Tray CloneDeep(Column col)
        {
            Tray t = new(col);

            return t;
        }

        internal Tray Clone()
        {
            Tray t = new(null);

            return t;
        }
    }
}