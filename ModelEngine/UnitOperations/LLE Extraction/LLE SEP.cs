using ModelEngine;
using LLE_Solver;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public partial class LLESEP : UnitOperation, ISerializable, IColumn
    {
        public LLESolver solver = new();
        private SpecificationCollection specs = new();
        private ColumSolverOptions solverOptions = new();
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
        public double feedRatio, MaxAlphaLoopIterations = 50, maxInnerIterations = 50, maxOuterIterations = 50, innertolerance = 0.001;
        private double outertolerance = 0.01; // Deg C / tray

        private SideStreamCollection liquidSideDraws = new();
        private SideStreamCollection vapourSideDraws = new();
        private ConnectingStreamCollection connectingDraws = new();
        private ConnectingStreamCollection connectingNetFlows = new(); // e.g. section top/bottom flow
        private ThermoDynamicOptions thermoOptions = new();

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

        private PumpAroundCollection pumpArounds = new();

        public LLESEP(string Name = "", bool reset = true) : base()
        {
            this.Name = Name;
            this.IsReset = reset;
            traySections = new(this, false);
            IsDirty = true;
        }

        public LLESEP(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Specs = (SpecificationCollection)info.GetValue("Specs", typeof(SpecificationCollection));
            traySections = (TraySectionCollection)info.GetValue("TraySectionCollection", typeof(TraySectionCollection));
            try
            {
                // ErrHistory1 = info.GetString("Err1");
                // ErrHistory2 = info.GetString("Err2");
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Specs", Specs);
            info.AddValue("TraySectionCollection", traySections);
            //   info.AddValue("Err1", ErrHistory1);
            //   info.AddValue("Err2", ErrHistory2);
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

        public ColumSolverOptions SolverOptions { get => solverOptions; set => solverOptions = value; }
        public Components Components { get => components; set => components = value; }
        public PumpAroundCollection PumpArounds { get => pumpArounds; set => pumpArounds = value; }

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

            foreach (TraySection traysection in traySections)
            {
                for (int i = 0; i < traysection.Trays.Count; i++)
                {
                    Tray tray = traysection.Trays[i];

                    if (tray.feed.IsConnected)
                    {
                        tray.feed.StreamEnthalpy();
                        tray.feed.H_.Clear();
                        tray.FlashFeedPorts(enumEnthalpy.LeeKesler, enumFlashType.PT); // recalc enthalpy, internal  calcs are Lee Kesler
                    }

                    if (tray.feed.IsConnected && !tray.feed.IsFlashed)
                    {
                        allfeedsflashed = false;
                        break;
                    }
                }
            }

            if (allfeedsflashed)
                return solver.Solve(this);
            else
                return false;
        }

        public override int FlashAllPorts()
        {
            int No = 0;
            foreach (TraySection traysection in traySections)
                for (int i = 0; i < traysection.Trays.Count; i++)
                    if (traysection.Trays[i].feed.IsConnected && !traysection.Trays[i].feed.IsFlashed && traysection.Trays[i].feed.Flash())
                        No++;

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
                    pm.SetPortValue(Units.ePropID.P, tray.P, SourceEnum.Input);
                    pm.SetPortValue(Units.ePropID.T, tray.T, SourceEnum.Input);
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
                        port.Clear();
                }
        }

        /*  public  override  void   TransferPortData(bool  override Inputs = false)
          {
              if (this.IsSolved)
              {
                  //Debug.WriteLine("Transfer Data for Column " + this.Name);

                  foreach (TraySection traysection in traySections)
                      foreach (Tray tray in traysection)
                          foreach (var p in tray.Ports)  // only transfer tray outlets to streams
                              if (p.ConnectedPorts != null)
                                  foreach (var pc in p.ConnectedPorts)
                                  {
                                      if (!p.IsFlowIn)
                                      {
                                          UnitOperation uo = pc.Owner;
                                          if (uo != null)
                                              uo.EraseCalcValues(SourceEnum.PortCalcResult);

                                          //Debug.WriteLine(this.Name + " Port: " + p.Name + ": " + p.Components.Origin.ToString() + ": " + p.H.Source.ToString());

                                          CompTransfer.TransferPortsComponentData(p);
                                          PropTransfer.TransferPortValue(p);
                                      }
                                  }
              }
          }*/
    }
}