using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

public enum Elements
{ C, H2, O2, S, N2, Cl2, Br2 }

public enum Atoms
{ C, H, S, O, N, Cl, Br }

public enum enumPropType
{ Conditions, Properties, All };

public enum enumVapPressure
{ Antoine, LeeKesler, RK, SRK, PR76, PR78, PRSV, ChaoSeader, GraysonStreed, BWRS, SimpleTest = 999 }

public enum enumEquiKMethod
{
    Antoine = 0,
    LeeKesler = 1,
    RK = 2, SRK = 3,
    PR76 = 4,
    PR78 = 5,
    PRSV = 6,
    ChaoSeader = 7,
    GraysonStreed = 8,
    Wilson = 9,
    UnifacLLE = 10,
    UnifacVLE = 11,
    BWRS,
    SimpleTest = 999,
    UNIQUAC = 1000,
    WilsonActivity = 1001
}

public enum enumEnthalpy
{ Ideal, LeeKesler, RK, SRK, PR76, PR78, PRSV, ChaoSeader, GraysonStreed, BWRS, SimpleTest = 999 }

public enum enumDensity
{ Rackett, Costald, BWRS } //, YamadaGunn, RRPS }

public enum enumViscLiqMethod
{ LetsouStiel, ElyHanley, Fixed }

public enum enumViscVapMethod
{ Lucas, YoonThodos, ElyHanley }

public enum enumThermalConductivity
{ API_1981_3_12A3_1, ElyHanley }

public enum enumSurfaceTensionMethod
{ BrockBird, EscobedoMansoori, HugillVanWelseness, SastriRaoAcids, SastriRaoAlcohols }

public enum IdealHeatCapMethod
{ Cavett, LeeKesler, API, RefBits };

public enum enumMassOrMolar
{ Mass, Molar }

public enum enumMassMolarOrVol
{ Mass, Molar, Vol, notdefined }

public enum enumFlowOrFraction
{ Flow, Fraction }

public enum enumShortMediumFull
{ Short, Medium, Full }

public enum enumFluidRegion
{ Solid, Liquid, Vapour, Gaseous, SuperCritical, TwoPhase, Undefined, CompressibleLiquid }

public enum enumFlashAlgorithm
{ RR, IO }

public enum enumPRVariation
{ PR76, PR78, PRSV }

public enum enumPropLocation
{ LVPCT, SG, VISC1, VISC2, VISC3, VISC4, VISC5, AROMATICS, NAPHTHENES, PARRAFINS, OLEFINS, NICKEL, VANADIUM };

public enum enumPCTType
{ LV_Crude, Mass_Crude, Mol_Crude, LV_Stream, Mass_Stream, Mol_Stream, NaN }

public enum enumDistType
{ D86, D1160, D2887, TBP_WT, TBP_VOL, NON }

public enum enumDistPoints
{ D1, D5, D10, D20, D30, D50, D70, D80, D90, D95, D99 }

public enum enumStreamType
{ empty, Pure, Pseudo, Mixed }

public enum enumTemp
{ C, F, K, R, NON }

public enum enumStreamComponentType
{ Pure, Mixed, Pseudo, UNDF }

public enum PseudoMixingMethod
{ Molar }

public enum enumOmegaMethod
{ LeeKesler, Edmister, Cavett };

public enum enumCritTMethod
{ LeeKesler, RiaziDaubert1, RiaziDaubert2, RiaziDaubert3, Vetere, TWU };

public enum enumCritPMethod
{ LeeKesler, RiaziDaubert0, RiaziDaubert1, RiaziDaubert2, Vetere, TWU };

public enum enumCritZMethod
{ LeeKesler };

public enum enumCritVMethod
{ LeeKesler, TWU }

public enum enumMW_Method
{ RiaziDaubert, LeeKesler, TWU };

public enum enumReboil
{ Vetere79, Mehmendoust };

public enum enumAssayType
{ Assay, MultipleStream, SingleStream }

public enum enumBIPPredMethod
{ Nishiumi1988, ValdReyes, Elnabawy, Tsonopoulos, PPR78, Benedeck, ChuePrausnitz }

public enum enumSteamMethod
{ _1967, _1997 }

/// <summary>
/// Type1 is mid BP, type 2 is end BP
/// </summary>
public enum EnumQuasiType
{ Type1, Type2 }

public enum ReformerReactionType
{ CHDehydrogTerm, CPDehydrogTerm, IsomTerm, CyclTerm, PCrackTerm, DealkTerm, NCrackTerm, none }

public enum enumAssayPCProperty // properties assigned to a quasi-componenet
{
    ANILINEPOINT,

    API,
    DENSITY15,
    SG,
    MW,
    RI67,

    BASICNITROGEN,
    TOTALNITROGEN,

    FREEZEPOINT,
    POURPOINT,
    CLOUDPOINT,
    CETANEINDEX,

    CONCARBON,
    MICROCARBONRESIDUE,
    RAMSBOTTOMCARBON,
    C7ASPHALTENES,
    CTOHRATIO,

    SULFUR,
    MERCAPTANSULFUR,

    MONC,
    RONC,
    MONL,
    RONL,

    PARAFFINS,
    ISOPARAFFINS,
    NAPHTHENES,
    AROMATICS,
    NAPHTHALENES,
    OLEFINS,

    NICKEL,
    SODIUM,
    VANADIUM,
    IRON,
    CUFE,

    SATURATEDRINGS,

    TBPCUTPOINT,

    T2C4E,
    TOTALACIDNUMER,

    TVP,

    VIS20,
    VIS40,
    VIS50,
    VIS60,
    VIS100,
    VIS130,
    WAX,
    IBP,
    BP1,
    BP5,
    BP10,
    BP20,
    BP30,
    BP40,
    BP50,
    BP60,
    BP70,
    BP80,
    BP90,
    BP95,
    BP99,
    FBP,

    H2,
    O2,
    CO,
    CO2,
    N2,
    NITROGEN,
    NC4,
    NC5,
    H2S,
    HYDROGEN,
    PENT1,
    IC4,
    IC4E,
    IC5,
    IC5E,
    BUT1,
    BUT13,
    C1,
    C10A,
    C10IP,
    C10N,
    C10NP,
    C10O,
    C10P,
    C11A,
    C11IP,
    C11N,
    C11NP,
    C11O,
    C11P,
    C2,
    C2C4E,
    C2E,
    C3,
    C3E,
    C6A,
    C6IP,
    C6N,
    C6NP,
    C6O,
    C6P,
    C7A,
    C7IP,
    C7N,
    C7NP,
    C7O,
    C7P,
    C8A,
    C8IP,
    C8N,
    C8NP,
    C8O,
    C8P,
    C9A,
    C9IP,
    C9N,
    C9NP,
    C9O,
    C9P,
    CC5,

    Ca1,
    Ca2,
    Ca3,
    Ca4,
    Ca5,
    Ca6,
    Ca7,

    RI20
}

public enum enumCommonPureComps
{
    [Description("Hydrogen")]
    H2,

    [Description("Nitrogen")]
    N2,

    [Description("O2")]
    O2,

    [Description("CO")]
    CO,

    [Description("CO2")]
    CO2,

    [Description("H2S")]
    H2S,

    [Description("Methane")]
    C1,

    [Description("Ethane")]
    C2,

    [Description("Propane")]
    C3,

    [Description("i-Butane")]
    iC4,

    [Description("n-Butane")]
    nC4,

    [Description("i-Pentane")]
    iC5,

    [Description("n-Pentane")]
    nC5
}

public enum enumMassVolMol
{
    [Description("Wt %")]
    Wt_PCT,

    [Description("Liq Vol %")]
    Vol_PCT,

    [Description("Mol %")]
    Mol_PCT,

    UNDF
}

//[Serializable]
public enum SourceEnum
{
    Input = 0,
    UnitOpCalcResult = 1,
    Default = 2,
    Empty = 3,
    Transferred = 4,
    CalcEstimate = 5,
    FixedEstimate = 6,
    NotConnected = 7,
    TransEstimate = 8,
    PortCalcResult = 9,
    SignalTransfer = 10,
    CalculatedSpec = 11
}

public enum enumFlashType
{ PT, PQ, TQ, PH, TH, PS, TS, PTQ, HQ, None, PHOld, PHIO, solid }

public enum FlowPropagateFlagEngine
{ Mass, Vol, MOL, Undefined };

public enum FlowFlagEngine
{ Mass, LiqVol, Molar, All, Undefined, EraseAll, Unknown };  // which flow is speced, All means defined elashere not on this stream

public enum enumCalcResult
{ Converged, Failed }

public enum FlowPropagateFlag
{ Mass, Vol, MOL, Undefined };

public enum FlowFlag
{ Mass, LiqVol, Molar, All, Undefined, EraseAll, Unknown };  // which flow is speced, All means defined elashere not on this stream

[TypeConverterAttribute(typeof(ThermoDynamicOptions)), Description("Expand to see value and units")]
[Serializable]
public class ThermoExpander : ExpandableObjectConverter //, ISerializable
{
    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
    {
        return base.ConvertTo(context, culture, value, destType);
    }
}

[TypeConverterAttribute(typeof(ThermoDynamicOptions)), Description("Expand to see options")]
[Serializable]
public class ThermoDynamicOptions : ThermoExpander, ISerializable
{
    public double[][] UniquacParams;

    private bool useBIPs = true;
    private bool useSteamTables = false;

    // ThermoMethods
    private enumSteamMethod steamMethod = enumSteamMethod._1997;

    private enumVapPressure pSat = enumVapPressure.PR78;
    private enumEnthalpy EnthalpyMethod = enumEnthalpy.PR78;
    private enumDensity DensityMethod = enumDensity.Rackett;
    private enumViscLiqMethod viscliqMethod = enumViscLiqMethod.ElyHanley;
    private enumViscVapMethod viscvapMethod = enumViscVapMethod.ElyHanley;
    private enumThermalConductivity thermcondMethod = enumThermalConductivity.ElyHanley;
    private enumSurfaceTensionMethod surftenMethod = enumSurfaceTensionMethod.BrockBird;
    private enumEquiKMethod EquilKMethodVLE = enumEquiKMethod.PR78;
    private enumEquiKMethod EquilKMethodLLE = enumEquiKMethod.UNIQUAC;
    private enumPRVariation pRVariation = enumPRVariation.PR78;

    // Characterisation methods
    private enumOmegaMethod omegaMethod = enumOmegaMethod.LeeKesler;

    private enumCritTMethod critTMethod = enumCritTMethod.LeeKesler;
    private enumCritPMethod critPMEthod = enumCritPMethod.LeeKesler;
    private enumCritZMethod critZMethod = enumCritZMethod.LeeKesler;
    private enumCritVMethod critVMethod = enumCritVMethod.LeeKesler;
    private enumBIPPredMethod bIPMethod = enumBIPPredMethod.ChuePrausnitz;

    private enumMW_Method mw_Method = enumMW_Method.LeeKesler;

    private enumFlashAlgorithm flashMethod = enumFlashAlgorithm.RR;

    [Browsable(true)]
    [Category("Vapour Pressure"), Description("P Sat")]
    [DisplayName("P Sat")]
    public enumVapPressure PSat
    {
        get { return pSat; }
        set { pSat = value; }
    }

    [Browsable(true)]
    [Category("Enthalpy"), Description("Enthalpy")]
    [DisplayName("Enthalpy")]
    public enumEnthalpy Enthalpy
    {
        get { return EnthalpyMethod; }
        set { EnthalpyMethod = value; }
    }

    public enumDensity Density
    {
        get { return DensityMethod; }
        set { DensityMethod = value; }
    }

    public enumEquiKMethod KMethod
    {
        get
        {
            return EquilKMethodVLE;
        }
        set
        {
            EquilKMethodVLE = value;
        }
    }

    public ThermoDynamicOptions Clone()
    {
        ThermoDynamicOptions clone = new ThermoDynamicOptions();

        clone.UniquacParams = UniquacParams;

        clone.useBIPs = useBIPs;
        // ThermoMethods
        clone.pSat = PSat;
        clone.EnthalpyMethod = EnthalpyMethod;
        clone.DensityMethod = DensityMethod;
        clone.viscliqMethod = viscliqMethod;
        clone.viscvapMethod = viscvapMethod;
        clone.thermcondMethod = thermcondMethod;
        clone.surftenMethod = surftenMethod;
        clone.EquilKMethodVLE = EquilKMethodVLE;
        clone.EquilKMethodLLE = EquilKMethodLLE;

        // Characterisation methods
        clone.omegaMethod = omegaMethod;
        clone.critTMethod = critTMethod;
        clone.critPMEthod = critPMEthod;
        clone.critZMethod = CritZMethod;
        clone.critVMethod = CritVMethod;
        clone.bIPMethod = bIPMethod;
        clone.mw_Method = MW_Method;

        return clone;
    }

    public enumSurfaceTensionMethod SurfaceTensionMethod
    {
        get { return surftenMethod; }
        set { surftenMethod = value; }
    }

    public enumThermalConductivity ThermcondMethod
    {
        get { return thermcondMethod; }
        set { thermcondMethod = value; }
    }

    public enumViscLiqMethod ViscLiqMethod
    {
        get { return viscliqMethod; }
        set { viscliqMethod = value; }
    }

    public enumViscVapMethod ViscVapMethod
    {
        get { return viscvapMethod; }
        set { viscvapMethod = value; }
    }

    public enumOmegaMethod OmegaMethod
    {
        get
        {
            return omegaMethod;
        }

        set
        {
            omegaMethod = value;
        }
    }

    public enumCritTMethod CritTMethod
    {
        get
        {
            return critTMethod;
        }

        set
        {
            critTMethod = value;
        }
    }

    public enumCritPMethod CritPMethod
    {
        get
        {
            return critPMEthod;
        }

        set
        {
            critPMEthod = value;
        }
    }

    public enumMW_Method MW_Method
    {
        get
        {
            return mw_Method;
        }

        set
        {
            mw_Method = value;
        }
    }

    public enumCritZMethod CritZMethod { get => critZMethod; set => critZMethod = value; }
    public enumCritVMethod CritVMethod { get => critVMethod; set => critVMethod = value; }
    public enumBIPPredMethod BIPMethod { get => bIPMethod; set => bIPMethod = value; }
    public enumEnthalpy EnthalpyMethod1 { get => EnthalpyMethod; set => EnthalpyMethod = value; }
    public bool UseBIPs { get => useBIPs; set => useBIPs = value; }
    public enumEquiKMethod KMethodLLE { get => EquilKMethodLLE; set => EquilKMethodLLE = value; }
    public bool UseSteamTables { get => useSteamTables; set => useSteamTables = value; }
    public enumSteamMethod SteamMethod { get => steamMethod; set => steamMethod = value; }
    public enumFlashAlgorithm FlashMethod { get => flashMethod; set => flashMethod = value; }
    public enumPRVariation PRVariation { get => pRVariation; set => pRVariation = value; }

    public ThermoDynamicOptions()
    { }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("VPMethod", PSat);
        info.AddValue("EnthalpyMethod", EnthalpyMethod);
        info.AddValue("DensityMethod", DensityMethod);
        info.AddValue("viscliqMethod", viscliqMethod);
        info.AddValue("viscvapMethod", viscvapMethod);
        info.AddValue("thermcondMethod", thermcondMethod);
        info.AddValue("surftenMethod", surftenMethod);
        info.AddValue("EquilKMethod", EquilKMethodVLE);

        info.AddValue("omegaMethod", omegaMethod);
        info.AddValue("critTMethod", critTMethod);
        info.AddValue("critPMEthod", critPMEthod);

        info.AddValue("EquilKMethodLLE", EquilKMethodLLE);
    }

    protected ThermoDynamicOptions(SerializationInfo info, StreamingContext context)
    {
        try
        {
            PSat = (enumVapPressure)info.GetValue("VPMethod", typeof(enumVapPressure));
            EnthalpyMethod = (enumEnthalpy)info.GetValue("EnthalpyMethod", typeof(enumEnthalpy));
            DensityMethod = (enumDensity)info.GetValue("DensityMethod", typeof(enumDensity));

            viscliqMethod = (enumViscLiqMethod)info.GetValue("viscliqMethod", typeof(enumViscLiqMethod));
            viscvapMethod = (enumViscVapMethod)info.GetValue("viscvapMethod", typeof(enumViscVapMethod));
            thermcondMethod = (enumThermalConductivity)info.GetValue("thermcondMethod", typeof(enumThermalConductivity));
            surftenMethod = (enumSurfaceTensionMethod)info.GetValue("surftenMethod", typeof(enumSurfaceTensionMethod));
            EquilKMethodVLE = (enumEquiKMethod)info.GetValue("EquilKMethod", typeof(enumEquiKMethod));

            omegaMethod = (enumOmegaMethod)info.GetValue("omegaMethod", typeof(enumOmegaMethod));
            critTMethod = (enumCritTMethod)info.GetValue("critTMethod", typeof(enumCritTMethod));
            critPMEthod = (enumCritPMethod)info.GetValue("critPMEthod", typeof(enumCritPMethod));

            EquilKMethodLLE = (enumEquiKMethod)info.GetValue("EquilKMethodLLE", typeof(enumEquiKMethod));
        }
        catch
        {
        }
    }

    public ThermoDynamicOptions(ThermoDynamicOptions thermo)
    {
        EnthalpyMethod = thermo.EnthalpyMethod;
        DensityMethod = thermo.DensityMethod;

        viscliqMethod = thermo.ViscLiqMethod;
        viscvapMethod = thermo.viscvapMethod;
        thermcondMethod = thermo.thermcondMethod;
        surftenMethod = thermo.surftenMethod;
        EquilKMethodVLE = thermo.EquilKMethodLLE;

        omegaMethod = thermo.omegaMethod;
        critTMethod = thermo.critTMethod;

        critPMEthod = thermo.critPMEthod;

        EquilKMethodLLE = thermo.EquilKMethodLLE;
    }
}

public class CharacterisationBasis
{
    private static enumOmegaMethod omegaMethod = enumOmegaMethod.LeeKesler;
    private static enumCritTMethod critTMethod = enumCritTMethod.LeeKesler;
    private static enumCritPMethod critPMethod = enumCritPMethod.RiaziDaubert1;

    public static enumOmegaMethod OmegaMethod { get => omegaMethod; set => omegaMethod = value; }
    public static enumCritTMethod CritTMethod { get => critTMethod; set => critTMethod = value; }
    public static enumCritPMethod CritPMethod { get => critPMethod; set => critPMethod = value; }
}

public class Global
{
    private static int[] lvpct_standard = new int[] { 1, 5, 10, 20, 30, 50, 70, 80, 90, 95, 99 };
    private static double[] lv1 = new double[] { 0.999, 5, 10, 20, 30, 50, 70, 80, 90, 95, 98.999 };  // to avoid extrapolation in curve fit
    private static bool deductHForm25 = true;
    public const double Rgas = 8.31446261815324;

    private static double[] bRangeL = new double[]
    {
      -273,-252.8,-195.6,-191.5,-182.8,-161.5,-103.7,-88.6,-87.9,-60.3,
      -47.7,-42.1,-11.7,-6.9,-6.3,-4.4,-0.5,0.9,3.7,27.8,29.9,31.2,36,40,50,
      60,70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,
      260,270,280,290,300,310,320,330,340,350,360,370,380,390,400,410,420,
      430,440,450,460,480,500,520,540,560,580,600,650,700
    };

    private static double[] bRangeU = new double[]
    {
        -252.8,-195.6,-191.5,-182.8,-161.5,-103.7,-88.6,-87.9,-60.3,-47.7,-42.1,
        -11.7,-6.9,-6.3,-4.4,-0.5,0.9,3.7,27.8,29.9,31.2,36,
        40,50,60,70,80,90,100,110,120,130,140,150,160,170,
        180,190,200,210,220,230,240,250,260,270,280,290,300,
        310,320,330,340,350,360,370,380,390,400,410,420,430,
        440,450,460,480,500,520,540,560,580,600,650,700,850
    };

    public static int[] Lvpct_standard { get => lvpct_standard; set => lvpct_standard = value; }
    public static double[] Lv1 { get => lv1; set => lv1 = value; }
    public static bool DeductHForm25 { get => deductHForm25; set => deductHForm25 = value; }
    public static double[] BRangeL { get => bRangeL; set => bRangeL = value; }
    public static double[] BRangeU { get => bRangeU; set => bRangeU = value; }

    public static double[] BRangeM()
    {
        double[] res = new double[BRangeL.Length];
        for (int n = 0; n < BRangeL.Length; n++)
        {
            res[n] = (BRangeL[n] + BRangeU[n]) / 2;
        }
        return res;
    }

    public static double MidBPt(int PC)
    {
        return (BRangeL[PC] + BRangeU[PC]) / 2;
    }

    public static int PCIndex(double Temperature)  //  return PC number
    {
        for (int n = 0; n < BRangeU.Length; n++)
        {
            if (Temperature <= BRangeU[n] && Temperature >= BRangeL[n])
            {
                return n;
            }
        }
        return 0;
    }

    public static double fract(int pc, double Temperature) //boiling range
    {
        return (Temperature - BRangeL[pc]) / (BRangeU[pc] - BRangeL[pc]);
    }
}