using ModelEngine;
using ModelEngine.Ports.Events;
using RusselColumn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace ModelEngine
{
    [Serializable]
    public partial class Column : UnitOperation, ISerializable, IColumn
    {
        public event IterationEventHAndler UpdateIteration1;

        public event IterationEventHAndler UpdateIteration2;

        public RusselSolver Russel = new();
        private SpecificationCollection specs = new();
        public ColumSolverOptions solverOptions = new();
        private Components components = new();

        public string Err1, Err2;
        private double waterdraw = 0;
        private bool solutionConverged = false;
        private bool totalreflux = false;
        private bool thomasAlgorithm = true;
        private bool splitMainFeed = false;
        private bool resetInitialTemps = true, lineariseEnthalpies = true;
        private double changeIndicator = 0;
        private double subcoolDT = 0, subcoolduty = 0;
        public double feedRatio, MaxAlphaLoopIterations = 5, maxInnerIterations = 50, maxOuterIterations = 50, innertolerance = 0.001;
        private double outertolerance = 0.01; //DegC/ tray

        private SideStreamCollection liquidSideDraws = new();
        private SideStreamCollection vapourSideDraws = new();
        private ConnectingStreamCollection connectingDraws = new();
        private ConnectingStreamCollection connectingNetFlows = new(); // e.g.sectiontop/bottomflow

        public ConnectingStreamCollection LiquidStreams;
        public ConnectingStreamCollection VapourStreams;
        public ConnectingStreamCollection ConnectingNetLiquidStreams;
        public ConnectingStreamCollection ConnectingNetVapourStreams;

        private ThermoDynamicOptions thermoOptions = new();
        public List<StreamMaterial> ExternalStreams = new();
        private PumpAroundCollection pumpArounds = new();

        public List<StreamMaterial> PADrawStreams = new();
        public List<StreamMaterial> PAReturnStreams = new();

        public ThermoDynamicOptions Thermo
        {
            get
            {
                return thermoOptions;
            }
            set
            {
                thermoOptions = value;
            }
        }

        public override bool IsSolved
        {
            get
            {
                if (MainTraySection is not null && MainTraySection.TopTray.TrayVapour.IsSolved && MainTraySection.BottomTray.TrayLiquid.IsSolved)
                    return true;
                else return false;
            }
        }

        public ConnectingStreamCollection ConnectingDraws { get => connectingDraws; set => connectingDraws = value; }
        public ConnectingStreamCollection ConnectingNetFlows { get => connectingNetFlows; set => connectingNetFlows = value; }

        public ConnectingStreamCollection ConnectingStreamsAll
        {
            get
            {
                ConnectingStreamCollection csc = new();
                csc.AddRange(ConnectingNetFlows);
                csc.AddRange(connectingDraws);
                return csc;
            }
        }

        private TraySectionCollection traySections;

        public Column(string Name = "", bool reset = true) : base()
        {
            this.Name = Name;
            this.IsReset = reset;
            traySections = new(this, false);
            IsDirty = true;
        }

        public double ConvergnceError
        {
            get { return Russel.ConvergenceErr; }
        }

        public new Func<object> OnRequestParent;

        protected object RequestParent()
        {
            if (OnRequestParent == null)
                return null;

            return OnRequestParent();
        }

        public Column(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Specs = (SpecificationCollection)info.GetValue("Specs", typeof(SpecificationCollection));
            traySections = (TraySectionCollection)info.GetValue("TraySectionCollection", typeof(TraySectionCollection));
            try
            {
                ErrHistory1 = info.GetString("Err1");
                ErrHistory2 = info.GetString("Err2");
                feedRatio = info.GetDouble("feedRatio");
                issolved = info.GetBoolean("issolved");
            }
            catch
            {
            }
        }

        public void TriggerPort_MainPortValueChanged(object sender, PropertyEventArgs e)
        {
            switch (this.Owner)
            {
                case COlSubFlowSheet csf:
                    if (csf.Owner is FlowSheet fs && !fs.solveStack.Contains(this))
                        fs.solveStack.Push(this);
                    break;

                default: break;
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            for (int i = 0; i < TraySections.Count; i++)
            {
                TraySections[i].SectionChanged += Column_SectionChanged;
            }
        }

        private void Column_SectionChanged(object sender, Ports.Events.PropertyEventArgs e)
        {
            //RaiseUOChangedEvent();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Specs", Specs);
            info.AddValue("TraySectionCollection", traySections);
            info.AddValue("Err1", ErrHistory1);
            info.AddValue("Err2", ErrHistory2);
            info.AddValue("feedRatio", feedRatio);
            info.AddValue("issolved", issolved);
        }

        public TraySection this[int index]
        {
            get { return traySections[index]; }
            set { traySections[index] = value; }
        }

        public TraySection this[Guid guid]
        {
            get
            {
                foreach (TraySection item in traySections)
                    if (item.Guid == guid)
                        return item;

                return null;
            }
        }

        public TraySection this[string name]
        {
            get
            {
                foreach (TraySection traySection in traySections)
                    if (traySection.Name == name)
                        return traySection;

                return null;
            }
        }

        public TraySection MainTraySection
        { get { return traySections[0]; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TraySectionCollection TraySections { get => traySections; set => traySections = value; }

        public SpecificationCollection Specs
        {
            get { return specs; }
            set { specs = value; }
        }

        public Components Components { get => components; set => components = value; }
        public PumpAroundCollection PumpArounds { get => pumpArounds; set => pumpArounds = value; }
        public ColumSolverOptions SolverOptions { get => solverOptions; set => solverOptions = value; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Solve()
        {
            Err1 = "";
            Err2 = "";

            while (FlashAllPorts() > 0)
            {
                FlashAllPorts();
            }

            bool allfeedsflashed = true;

            if (MainSectionStages.Trays.Count == 11)
                MainTraySection.Trays[10].feed.Flash(true);

            foreach (TraySection traysection in traySections)
            {
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    Tray tray = traysection.Trays[i];
                    Port_Material pm = tray.feed.ConnectedPortNext;

                    if (pm != null)
                    {
                        if (pm.Owner is StreamMaterial stream)
                        {
                            if (tray.feed.IsConnected
                                && !tray.feed.IsFullyDefined
                                && !stream.IsInternal) // must be extenal stream
                            {
                                tray.FlashFeedPort();

                                if (!tray.feed.IsFullyDefined)
                                {
                                    allfeedsflashed = false;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            if (allfeedsflashed)
                return Russel.Solve(this);
            else
                return false;
        }

        private bool issolved = false;

        public override int FlashAllPorts()
        {
            int No = 0;
            foreach (TraySection traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    Tray tray = traysection.Trays[i];
                    if (tray.feed.IsConnected && tray.feed.Flash(true))
                        No++;
                    //tray.feed.Flash();
                }

            return 0;
        }

        public void FlashAllOutPorts()
        {
            foreach (TraySection traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    Tray tray = traysection.Trays[i];
                    Port_Material pm = tray.liquidDrawRight;
                    pm.cc.Clear();
                    pm.cc.Add(Components);
                    pm.cc.ClearMoleFractions();
                    pm.cc.Origin = SourceEnum.UnitOpCalcResult;
                    pm.SetMoleFractions(tray.LiqComposition);
                    pm.SetPortValue(Units.ePropID.P, tray.P, SourceEnum.Input, this);
                    pm.SetPortValue(Units.ePropID.T, tray.T, SourceEnum.Input, this);
                    pm.Flash();
                }
        }

        public override void EraseNonFixedValues()
        {
            foreach (TraySection traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    Tray tray = traysection.Trays[i];
                    foreach (var port in tray.Ports)
                        port.ClearNonInputs();
                }
        }
    }
}