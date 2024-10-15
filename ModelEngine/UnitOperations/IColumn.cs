using ModelEngine;
using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    public interface IColumn
    {
        ColumSolverOptions SolverOptions { get; set; }
        PumpAroundCollection PumpArounds { get; set; }
        Components Components { get; set; }
        TraySection this[Guid guid] { get; }
        TraySection this[int index] { get; set; }
        TraySection this[string name] { get; }

        double ChangeIndicator { get; set; }
        ConnectingStreamCollection ConnectingDraws { get; set; }
        ConnectingStreamCollection ConnectingNetFlows { get; set; }
        ConnectingStreamCollection ConnectingStreamsAll { get; }
        double FeedRatio { get; set; }
        bool IsReset { get; set; }
        int JacobianSize { get; }
        bool LineariseEnthalpies { get; set; }
        SideStreamCollection LiquidSideStreams { get; set; }
        TraySection MainSectionStages { get; set; }
        TraySection MainTraySection { get; }
        double MaxOuterIterations { get; set; }
        double MaxTemperatureLoopCount { get; set; }

        int NoComps { get; }
        int NonJacobianSpecsCount { get; }
        int NoSections { get; }
        int RequiredSpecsCount { get; }
        bool ResetInitialTemps { get; set; }
        bool SolutionConverged { get; set; }
        double SpecificationTolerance { get; set; }
        SpecificationCollection Specs { get; set; }
        bool SplitMainFeed { get; set; }
        double SubcoolDT { get; set; }
        double Subcoolduty { get; set; }
        ThermoDynamicOptions Thermo { get; set; }
        bool ThomasAlgorithm { get; set; }
        bool Totalreflux { get; set; }
        int TotNoStages { get; }
        TraySectionCollection TraySections { get; set; }
        double TrayTempTolerance { get; set; }
        SideStreamCollection VapourSideStreams { get; set; }
        double Waterdraw { get; set; }

        void EraseNonFixedValues();

        void FlashAllOutPorts();

        int FlashAllPorts();

        void GetObjectData(SerializationInfo info, StreamingContext context);

        PumpAroundCollection GetPumpArounds();

        bool InterpolatePressureAllSection();

        PumpAroundCollection PumpAroundsActive();

        SideStreamCollection SideStreamsAll();

        bool Solve();

        string ToString();

        //  void   TransferPortData(bool  override Inputs = false);
        bool ValidateDesign();
    }
}