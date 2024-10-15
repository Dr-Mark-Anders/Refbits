namespace ModelEngineTest
{
    public class ColumSolverOptions
    {
        public COMColumnInitialFlowsMethod InitFlowsMethod = COMColumnInitialFlowsMethod.Modified;
        public COMColumnInitialiseMethod ColumnInitialiseMethod = COMColumnInitialiseMethod.ConvergeOnK;
        public COMColumnSFUpdateMethod SFUpdateMethod = COMColumnSFUpdateMethod.Modified;
        public COMColumnEnthalpyMethod VapEnthalpyMethod = COMColumnEnthalpyMethod.BostonBrittHdep;
        public COMColumnEnthalpyMethod LiqEnthalpyMethod = COMColumnEnthalpyMethod.SimpleLinear;
        public COMColumnAlphaMethod AlphaMethod = COMColumnAlphaMethod.LLE;
        public COMColumnKMethod KMethod = COMColumnKMethod.Linear;
        public COMColumnTestimateMethod TEstimateMethod = COMColumnTestimateMethod.LinearEstimate2Values;
    }

    public enum COMColumnKMethod
    {
        Rigorous, LogLinear, BostonMethod, LLE, MA, Linear
    }

    public enum COMColumnAlphaMethod
    {
        Rigorous, LogLinear, BostonMethod, LLE, MA, Linear
    }

    public enum COMColumnEnthalpyMethod
    {
        Rigorous, BostonBrittHdep, SimpleLinear, TestLLE, Boston
    }

    public enum COMColumnTestimateMethod
    {
        QuadraticEstimate, Rigorous, Parallel, LinearEstimate2Values, Rigorous2, BostonMethod, LinearEstimate2ValuesLooped, MA,
        LogLinear,
        Linear
    }

    public enum COMColumnSFUpdateMethod
    {
        Simple, Excel, Modified, Delayed,
        Rapid
    }

    public enum COMColumnInitialFlowsMethod
    {
        Simple, Excel, Modified, LLE
    }

    public enum COMColumnInitialiseMethod
    {
        ConvergeOnK, Excel, StripFactors, None, Test, LLE
    }

    internal enum COMenumInverseMethod
    {
        GeneralMatrix, Alglib, MathNet,
        Crouts
    }

    public enum COMCondType
    { None, Partial, Subcooled, TotalReflux, }

    public enum COMReboilerType
    { None, Kettle, Thermosiphon, HeatEx }

    public enum COMePASpecTypes
    { Flow, Duty, ReturnT, DeltaT }

    public enum COMenumflowtype
    { Mass, StdLiqVol, Molar, None }

    public enum COMeSpecType
    {
        LLE_KSpec, TrayDuty, TrayNetVapFlow, Temperature, RefluxRatio, LiquidProductDraw, VapProductDraw, TrayNetLiqFlow, PAFlow, Energy, PARetT,
        PADeltaT, PADuty, LiquidStream, VapStream, DistSpec
    }

    public enum COMeSectionType
    { Main, stripper, secondary }
}