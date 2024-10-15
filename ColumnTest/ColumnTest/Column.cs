using ModelEngine;
using ModelEngine.Ports.Events;
using RusselColumnTest;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngineTest
{
    [Serializable]
    public partial class COMColumn : UnitOperation, ISerializable
    {
        public event IterationEventHAndlerTest UpdateIteration1;

        public event IterationEventHAndlerTest UpdateIteration2;

        public RusselSolverTest Russel = new();
        private SpecificationCollectionTest specs = new();
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

        private SideStreamCollectionTest liquidSideDraws = new();
        private SideStreamCollectionTest vapourSideDraws = new();
        private ConnectingStreamCollectionTest connectingDraws = new();
        private ConnectingStreamCollectionTest connectingNetFlows = new(); // e.g.sectiontop/bottomflow

        public ConnectingStreamCollectionTest LiquidStreams;
        public ConnectingStreamCollectionTest VapourStreams;
        public ConnectingStreamCollectionTest ConnectingNetLiquidStreams;
        public ConnectingStreamCollectionTest ConnectingNetVapourStreams;

        private ThermoDynamicOptions thermoOptions = new();
        private List<StreamMaterial> ExternalStreams = new();
        private PumpAroundCollectionTest pumpArounds = new();

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

        public ConnectingStreamCollectionTest ConnectingDraws { get => connectingDraws; set => connectingDraws = value; }
        public ConnectingStreamCollectionTest ConnectingNetFlows { get => connectingNetFlows; set => connectingNetFlows = value; }

        public ConnectingStreamCollectionTest ConnectingStreamsAll
        {
            get
            {
                ConnectingStreamCollectionTest csc = new();
                csc.AddRange(ConnectingNetFlows);
                csc.AddRange(connectingDraws);
                return csc;
            }
        }

        private TraySectionCollection traySections;// = new (this)

        public COMColumn(RusselSolverTest solver, string Name = "", bool reset = true) : base()
        {
            Russel = solver;
            this.Name = Name;
            this.IsReset = reset;
            traySections = new(this, false);
            IsDirty = true;
        }

        public COMColumn(string Name = "", bool reset = true) : base()
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

        public COMColumn(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Specs = (SpecificationCollectionTest)info.GetValue("Specs", typeof(SpecificationCollectionTest));
            traySections = (TraySectionCollection)info.GetValue("TraySectionCollection", typeof(TraySectionCollection));
            try
            {
                ErrHistory1 = info.GetString("Err1");
                ErrHistory2 = info.GetString("Err2");
                feedRatio = info.GetDouble("feedRatio");
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

        private void Column_SectionChanged(object sender, ModelEngine.Ports.Events.PropertyEventArgs e)
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
        }

        public TraySectionTest this[int index]
        {
            get { return traySections[index]; }
            set { traySections[index] = value; }
        }

        public TraySectionTest this[Guid guid]
        {
            get
            {
                foreach (TraySectionTest item in traySections)
                    if (item.Guid == guid)
                        return item;

                return null;
            }
        }

        public TraySectionTest this[string name]
        {
            get
            {
                foreach (TraySectionTest traySection in traySections)
                    if (traySection.Name == name)
                        return traySection;

                return null;
            }
        }

        public TraySectionTest MainTraySection
        { get { return traySections[0]; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TraySectionCollection TraySections { get => traySections; set => traySections = value; }

        public SpecificationCollectionTest Specs
        {
            get { return specs; }
            set { specs = value; }
        }

        public Components Components { get => components; set => components = value; }
        public PumpAroundCollectionTest PumpArounds { get => pumpArounds; set => pumpArounds = value; }
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

            foreach (TraySectionTest traysection in traySections)
            {
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    TrayTest tray = traysection.Trays[i];
                    if (tray.feed.IsConnected && !tray.feed.IsFlashed)
                        tray.FlashFeedPorts();
                    if (tray.feed.IsConnected && !tray.feed.IsFlashed)
                    {
                        allfeedsflashed = false;
                        return false;
                    }
                }
            }

            if (allfeedsflashed)
            {
                IsSolved = Russel.Solve(this);
                // IsSolved = InitialiseFlowsETC();

                if (IsSolved)
                {
                    pumpArounds.UpdateExternalStreams();
                    return true;
                }
                else
                {
                    pumpArounds.EraseExternalStreams();
                    return false;
                }
            }
            else
                return false;
        }

        public bool Init()
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

            foreach (TraySectionTest traysection in traySections)
            {
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    TrayTest tray = traysection.Trays[i];
                    if (tray.feed.IsConnected && !tray.feed.IsFlashed)
                        tray.FlashFeedPorts();
                    if (tray.feed.IsConnected && !tray.feed.IsFlashed)
                    {
                        allfeedsflashed = false;
                        return false;
                    }
                }
            }

            if (allfeedsflashed)
            {
                IsSolved = Russel.Init(this);
                // IsSolved = InitialiseFlowsETC();

                if (IsSolved)
                {
                    pumpArounds.UpdateExternalStreams();
                    return true;
                }
                else
                {
                    pumpArounds.EraseExternalStreams();
                    return false;
                }
            }
            else
                return false;
        }

        public override int FlashAllPorts()
        {
            int No = 0;
            foreach (TraySectionTest traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                    if (traysection.Trays[i].feed.IsConnected && !traysection.Trays[i].feed.IsFlashed && traysection.Trays[i].feed.Flash())
                        No++;

            return 0;
        }

        public void FlashAllOutPorts()
        {
            foreach (TraySectionTest traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    TrayTest tray = traysection.Trays[i];
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
            foreach (TraySectionTest traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    TrayTest tray = traysection.Trays[i];
                    foreach (var port in tray.Ports)
                        port.ClearNonInputs();
                }
        }

        /*public override void  TransferPortData(bool override Inputs=false)
        {
        if(this.IsSolved)
        {
        //Debug.WriteLine("TransferDataforColumn"+this.Name);

        foreach(TraySectiontraysectionint raySections)
        foreach(Traytrayint raysection)
        foreach(varpint ray.Ports)//onlytransfertrayoutletstostreams
        if(p.ConnectedPorts!=null)
        foreach(varconnectePortinp.ConnectedPorts)
        {
        if(!p.IsFlowIn)
        {
        UnitOperationuo=connectePort.Owner;
        if(uo!=null)
        uo.EraseCalcValues(SourceEnum.PortCalcResult);

        //Debug.WriteLine(this.Name+"Port:"+p.Name+":"+p.Components.Origin.ToString()+":"+p.H.Source.ToString());

        CompTransfer.TransferPortsComponentData(p);
        PropTransfer.TransferPortValue(p);
        }
        }
        }
        }*/

        bool issolved;
        public new bool IsSolved
        {
            get
            {
                return issolved;
                //return issolved;
            }
            set
            {
                issolved = value;
            }
        }
    }
}