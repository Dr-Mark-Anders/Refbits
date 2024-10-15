namespace ModelEngine
{
    public class ColumSolverOptions
    {
        public ColumnInitialFlowsMethod InitFlowsMethod = ColumnInitialFlowsMethod.Modified;
        public ColumnInitialiseMethod ColumnInitialiseMethod = ColumnInitialiseMethod.ConvergeOnK;
        public ColumnSFUpdateMethod SFUpdateMethod = ColumnSFUpdateMethod.Modified;
        public ColumnEnthalpyMethod VapEnthalpyMethod = ColumnEnthalpyMethod.BostonBrittHdep;
        public ColumnEnthalpyMethod LiqEnthalpyMethod = ColumnEnthalpyMethod.SimpleLinear;
        public ColumnAlphaMethod AlphaMethod = ColumnAlphaMethod.LLE;
        public ColumnKMethod KMethod = ColumnKMethod.Linear;
        public ColumnTestimateMethod TEstimateMethod = ColumnTestimateMethod.LinearEstimate2Values;
    }

    public enum ColumnKMethod
    {
        Rigorous, LogLinear, BostonMethod, LLE, MA, Linear
    }

    public enum ColumnAlphaMethod
    {
        Rigorous, LogLinear, BostonMethod, LLE, MA, Linear
    }

    public enum ColumnEnthalpyMethod
    {
        Rigorous, BostonBrittHdep, SimpleLinear, TestLLE, Boston
    }

    public enum ColumnTestimateMethod
    {
        QuadraticEstimate, Rigorous, Parallel, LinearEstimate2Values, Rigorous2, BostonMethod, LinearEstimate2ValuesLooped, MA,
        LogLinear,
        Linear
    }

    public enum ColumnSFUpdateMethod
    {
        Simple, Excel, Modified, Delayed,
        Rapid
    }

    public enum ColumnInitialFlowsMethod
    {
        Simple, Excel, Modified, LLE
    }

    public enum ColumnInitialiseMethod
    {
        ConvergeOnK, Excel, StripFactors, None, Test, LLE
    }

    internal enum enumInverseMethod
    {
        GeneralMatrix, Alglib, MathNet,
        Crouts
    }

    public enum CondType
    { None, Partial, Subcooled, TotalReflux, }

    public enum ReboilerType
    { None, Kettle, Thermosiphon, HeatEx }

    public enum ePASpecTypes
    { Flow, Duty, ReturnT, DeltaT }

    public enum enumflowtype
    { Mass, StdLiqVol, Molar, None }

    public enum eSpecType
    {
        LLE_KSpec, TrayDuty, TrayNetVapFlow, Temperature, RefluxRatio, LiquidProductDraw, VapProductDraw, TrayNetLiqFlow, PAFlow, Energy, PARetT,
        PADeltaT, PADuty, LiquidStream, VapStream, DistSpec
    }

    public enum eSectionType
    { Main, stripper, secondary }
}