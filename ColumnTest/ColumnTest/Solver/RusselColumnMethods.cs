using ModelEngine;
using Extensions;
using ModelEngine;
using ModelEngineTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Units;

namespace RusselColumnTest
{
    internal enum FeedSplitType
    { SimpleFeedModel, AssumeAllLiquid };

    public partial class RusselSolverTest
    {
        private Port_Material MainFeed;
        private readonly FeedSplitType feedsplittype = FeedSplitType.AssumeAllLiquid;
        private readonly int delaycount = 10;

        public void InitialiseSpecs(COMColumn column)
        {
            foreach (SpecificationTest spec in column.Specs)
            {
                spec.traysection = column.TraySections[spec.engineSectionGuid];
                spec.tray = spec.traysection[spec.engineStageGuid];
            };
        }

        private void UpdateTrayKValues()
        {
            for (int i = 0; i < column.TraySections.Count; i++)
            {
                for (int y = 0; y < column.TraySections[i].Trays.Count; y++)
                {
                    column.TraySections[i].Trays[y].UpdateTrayK(column.solverOptions.KMethod);
                }
            }
        }

        public void Finishoff()
        {
            EstimateTs(column, column.solverOptions.KMethod);

            if (column.MainTraySection.CondenserType == COMCondType.Subcooled)
                column.MainTraySection.Trays[0].T -= column.SubcoolDT;

            column.Waterdraw = WaterDraw;
        }

        public void DoUpdateIteration1(string update)
        {
            UpdateIteration1?.Invoke(this, new IterationEventArgsTest(update));
        }

        public void DoUpdateIteration2(string update)
        {
            UpdateIteration2?.Invoke(this, new IterationEventArgsTest(update));
        }

        public void ProcessActiveSpecs(out double[] errors, bool isbase)
        {
            List<double> errorList = new();
            SpecificationCollectionTest ActiveSpecs = column.Specs.GetActiveSpecs();

            foreach (SpecificationTest spec in ActiveSpecs)
            {
                if (isbase)
                {
                    switch (spec.engineSpecType)
                    {
                        case COMeSpecType.TrayNetVapFlow:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.Temperature:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.TrayDuty:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.RefluxRatio:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.LiquidProductDraw:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.VapProductDraw:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.TrayNetLiqFlow:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.PAFlow:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.Energy:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.LiquidStream:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.VapStream:
                            errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.PARetT:
                            //errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.PADeltaT:
                            //errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.PADuty:
                            //errorList.Add(spec.baseerror);
                            break;

                        case COMeSpecType.DistSpec:
                            errorList.Add(spec.baseerror);
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (spec.engineSpecType)
                    {
                        case COMeSpecType.TrayNetVapFlow:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.Temperature:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.TrayDuty:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.RefluxRatio:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.LiquidProductDraw:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.VapProductDraw:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.TrayNetLiqFlow:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.PAFlow:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.Energy:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.LiquidStream:
                        case COMeSpecType.VapStream:
                            errorList.Add(spec.error);
                            break;

                        case COMeSpecType.PARetT:
                            //errorList.Add(spec.error);
                            break;

                        case COMeSpecType.PADeltaT:
                            // errorList.Add(spec.error);
                            break;

                        case COMeSpecType.PADuty:
                            //errorList.Add(spec.error);
                            break;

                        case COMeSpecType.DistSpec:
                            errorList.Add(spec.delta);
                            break;

                        default:
                            break;
                    }
                }
            }

            errors = errorList.ToArray();
        }

        public double UpdateTrayTemps(double dampf)
        {
            double DiffSum = 0;
            double delta, moddelta;

            for (int sectionNo = 0; sectionNo < column.TraySections.Count; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];

                for (int n = 0; n < section.Trays.Count; n++)
                {
                    TrayTest tray = section.Trays[n];
                    delta = tray.TPredicted - tray.T;

                    if (Math.Abs(delta) > MaxDeltaT)
                        moddelta = MaxDeltaT;
                    else
                        moddelta = Math.Abs(delta);

                    if (delta < 0)
                        tray.T -= moddelta * dampf;
                    else
                        tray.T += moddelta * dampf;

                    DiffSum += Math.Abs(delta);
                }
            }
            return DiffSum;
        }

        public void SetInitialTrayTemps(COMColumn cd)
        {
            Port_Material feed = cd.TraySections.GetLargestFeedStream();
            if (feed != null && column != null)
            {
                double t = feed.T_;

                for (int sectionNo = 0; sectionNo < cd.TraySections.Count; sectionNo++)
                    for (int trayNo = 0; trayNo < column.TraySections[sectionNo].Trays.Count; trayNo++)
                    {
                        TrayTest tray = cd.TraySections[sectionNo].Trays[trayNo];
                        tray.T = t;
                    }
            }
        }

        public void InitialiseMatrices(COMColumn column)
        {
            JacobianSize = column.JacobianSize;

            A = new double[NoSections][];
            B = new double[NoSections][];

            KBase = new double[NoSections][];
            Errors = new double[JacobianSize];
            deltas = new double[JacobianSize];
            StripperLiqFeedEnthalpies = new double[NoSections][];
            StripperVapReturnTotEnthalpies = new double[NoSections][];
            // Stripperreturns = new double [NoSections][];
            TempTrayTempK = new double[NoSections][];

            TrDMatrix = new double[NoComps][][];
            for (int n = 0; n < NoComps; n++)
            {
                TrDMatrix[n] = new double[TotNoTrays][];
                for (int m = 0; m < TotNoTrays; m++)
                    TrDMatrix[n][m] = new double[TotNoTrays];
            }

            CompLiqMolarFlows = new double[TotNoTrays][];
            CompVapMolarFlows = new double[TotNoTrays][];

            InterSectionLiquidDraws = new double[NoSections][];
            InterSectionLiquidFeeds = new double[NoSections][];
            InterSectionVapourFeeds = new double[NoSections][];
            InterSectionVapourDraws = new double[NoSections][];

            if (this.column != null)
            {
                for (int i = 0; i < NoSections; i++)
                {
                    InterSectionLiquidDraws[i] = new double[this.column.TraySections[i].Trays.Count];
                    InterSectionLiquidFeeds[i] = new double[this.column.TraySections[i].Trays.Count];
                    InterSectionVapourDraws[i] = new double[this.column.TraySections[i].Trays.Count];
                    InterSectionVapourFeeds[i] = new double[this.column.TraySections[i].Trays.Count];
                }
            }

            for (int n = 0; n < TotNoTrays; n++)
            {
                CompLiqMolarFlows[n] = new double[NoComps];
                CompVapMolarFlows[n] = new double[NoComps];
            }

            if (this.column != null)
            {
                for (int sect = 0; sect < NoSections; sect++)
                {
                    TraySectionTest section = this.column.TraySections[sect];

                    int notrays = column.TraySections[sect].Trays.Count;
                    StripperLiqFeedEnthalpies[sect] = new double[notrays];
                    StripperVapReturnTotEnthalpies[sect] = new double[notrays];
                    section.SectionIndex = sect;

                    //Stripperreturns[sect] = new  double [notrays];

                    B[sect] = new double[notrays];
                    A[sect] = new double[notrays];

                    for (int n = 0; n < section.Trays.Count; n++)
                    {
                        TrayTest tray = section.Trays[n];
                        //section[n].TrayVapComposition = new  double [NoComps];
                        //section[n].TrayCompositionVap = new  double [NoComps];
                        tray.LiqComposition = new double[NoComps];
                        tray.VapComposition = new double[NoComps];
                        tray.LiqCompositionPred = new double[NoComps];
                        tray.VapCompositionPred = new double[NoComps];
                        tray.LiqCompositionInitial = new double[NoComps];
                        tray.VapCompositionInitial = new double[NoComps];
                        tray.KTray = new double[NoComps];
                        tray.TrayIndex = n;
                        //section[n].lnKAvg

                        //lnAlpha[sect][n] = new  double [NoComps];
                    }
                }
            }

            foreach (ConnectingStreamTest stream in column.ConnectingStreamsAll)
            {
                stream.engineDrawSectionIndex = column.TraySections.IndexOf(stream.EngineDrawSection);
                stream.engineReturnSectionIndex = column.TraySections.IndexOf(stream.EngineReturnSection);
                stream.engineDrawTrayIndex = stream.EngineDrawSection.Trays.IndexOf(stream.engineDrawTray);
                stream.engineReturnTrayIndex = stream.EngineReturnSection.Trays.IndexOf(stream.engineReturnTray);
            }

            Jacobian = new double[JacobianSize, JacobianSize];
            //  for (int  n = 0; n < JacobianSize; n++)
            //     Jacobian[n] = new  double [JacobianSize];
        }

        public bool InterpolatePressures()
        {
            if (this.column != null)
                foreach (TraySectionTest section in column.TraySections)
                {
                    double topp = section.Trays[0].P, botp = section.BottomTray.P;
                    if (double.IsNaN(topp))
                    {
                        MessageBox.Show("Pressures Are not Set in Column");
                        return false;
                    }

                    double deltaP = (botp - topp) / (section.Trays.Count - 1);
                    if (double.IsNaN(deltaP))
                        deltaP = 0;

                    for (int i = 1; i < section.Trays.Count - 1; i++)
                    {
                        if (double.IsNaN(section.Trays[i].P))
                            section.Trays[i].P.BaseValue = topp + deltaP*i;
                    }
                }

            return true;
        }

        public bool InitialiseFeeds(COMColumn column)
        {
            TotalFeeds = 0;
            PortList feedstreams = new();

            foreach (TraySectionTest section in column.TraySections)
            {
                for (int TrayNo = 0; TrayNo < section.Trays.Count; TrayNo++) // do main column first
                {
                    TrayTest tray = section.Trays[TrayNo];
                    if (tray.feed != null && (tray.feed.IsConnected || tray.feed.IsSolved))
                    {
                        if (tray.feed.IsSolved)
                        {
                            feedstreams.Add(tray.feed);
                            TotalFeeds += tray.feed.MolarFlow_;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            if (!feedstreams.AllSolved() || feedstreams.Count == 0)
                return false;

            MainFeed = feedstreams.GetLargestStream();
            if (MainFeed is null)
                return false;

            column.FeedRatio = 1 / MainFeed.MolarFlow_;

            if (feedstreams.Count > 0)
                foreach (var feedport in feedstreams)
                {
                    switch (feedsplittype)
                    {
                        case FeedSplitType.SimpleFeedModel:
                            if (feedport.Q_ < 0.99999) // put 100% vapours on vapour array, e.g steam stripping
                                feedport.RatioedLiquidFeed = feedport.LiquidMoleFlow * column.FeedRatio;
                            else
                                feedport.RatioedVapourFeed = feedport.VapourMoleFlow * column.FeedRatio;
                            break;

                        case FeedSplitType.AssumeAllLiquid: // unless all vapour
                            if (feedport.Q_ == 1) // put 100% vapours on vapour array, e.g steam stripping
                            {
                                feedport.RatioedLiquidFeed = 0;
                                feedport.RatioedVapourFeed = feedport.VapourMoleFlow * column.FeedRatio;
                            }
                            else
                            {
                                feedport.RatioedLiquidFeed = feedport.MolarFlow_ * column.FeedRatio;
                                feedport.RatioedVapourFeed = 0;
                            }
                            break;

                        default:
                            if (column.SplitMainFeed)
                                feedport.SplitFeed = true;
                            else
                                feedport.SplitFeed = false;
                            break;
                    }
                }

            Port_Material feed;

            FeedMolarCompFlowsTotal = new double[column.Components.Count][];
            FeedMolarCompFlowsVapour = new double[column.Components.Count][];
            FeedMolarCompFlowsLiquid = new double[column.Components.Count][];

            for (int CompNo = 0; CompNo < column.Components.Count; CompNo++)
            {
                FeedMolarCompFlowsLiquid[CompNo] = new double[column.TraySections.TotNoTrays()];
                FeedMolarCompFlowsVapour[CompNo] = new double[column.TraySections.TotNoTrays()];
                FeedMolarCompFlowsTotal[CompNo] = new double[column.TraySections.TotNoTrays()];
                int count = 0;
                for (int sectionNo = 0; sectionNo < column.TraySections.Count; sectionNo++)
                {
                    TraySectionTest section = column.TraySections[sectionNo];
                    for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                    {
                        feed = section.Trays[trayNo].feed;
                        if (feed.IsFlashed && (feed.IsConnected || feed.MolarFlow_ != 0)
                            && feed.cc != null
                            && feed.cc.Count == column.Components.Count)
                        {
                            FeedMolarCompFlowsLiquid[CompNo][count] = feed.cc.ComponentList[CompNo].MoleFraction * feed.RatioedLiquidFeed;
                            FeedMolarCompFlowsVapour[CompNo][count] = feed.cc.ComponentList[CompNo].MoleFraction * feed.RatioedVapourFeed;
                            FeedMolarCompFlowsTotal[CompNo][count] = FeedMolarCompFlowsLiquid[CompNo][count] + FeedMolarCompFlowsVapour[CompNo][count];
                        }
                        count += 1;
                    }
                }
            }

            //ViewArray va = new  ViewArray();
            //va.View(FeedMolarCompFlowsLiquid);
            //va.View(FeedMolarCompFlowsVapour);
            //va.View(FeedMolarCompFlowsTotal);
            //Debug.Print (FeedMolarCompFlowsTotal.Sum().ToString());
            return true;
        }

        public void InitialiseTrayCompositions(COMColumn column)
        {
            if (MainFeed == null)
                return;

            if (column.SolutionConverged)
                for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
                {
                    TraySectionTest section = column.TraySections[sectionNo];
                    for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                    {
                        section.Trays[trayNo].LiqCompositionInitial = (double[])section.Trays[trayNo].LiqComposition.Clone();
                        section.Trays[trayNo].VapCompositionInitial = (double[])section.Trays[trayNo].VapComposition.Clone();
                    }
                }
            else
                for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
                {
                    TraySectionTest section = column.TraySections[sectionNo];

                    for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise comps;
                    {
                        section.Trays[trayNo].LiqCompositionInitial = new double[NoComps];
                        section.Trays[trayNo].VapCompositionInitial = new double[NoComps];

                        for (int c = 0; c < NoComps; c++)
                        {
                            if (c < MainFeed.cc.Count)
                            {
                                section.Trays[trayNo].LiqCompositionInitial[c] = MainFeed.cc.ComponentList[c].MoleFraction;
                                section.Trays[trayNo].VapCompositionInitial[c] = MainFeed.cc.ComponentList[c].MoleFraction;
                                section.Trays[trayNo].LiqComposition[c] = MainFeed.cc.ComponentList[c].MoleFraction;
                                section.Trays[trayNo].VapComposition[c] = MainFeed.cc.ComponentList[c].MoleFraction;
                            }
                        }
                    }
                }
        }

        public static void InitialisePumpArounds(COMColumn column)
        {
            foreach (PumpAroundTest pa in column.PumpArounds)
            {
                pa.SetDrawTrayNos();

                foreach (SpecificationTest s in column.Specs.GetSpecs(pa))
                {
                    TraySectionTest section = column.TraySections[s.engineSectionGuid];

                    switch (s.engineSpecType)
                    {
                        case COMeSpecType.PAFlow:
                            pa.MoleFlow.BaseValue = s.SpecValue;
                            break;

                        case COMeSpecType.PADeltaT:
                            pa.ReturnTemp = pa.drawTray.T - s.SpecValue;
                            break;

                        case COMeSpecType.PARetT:
                            pa.ReturnTemp = s.SpecValue;
                            break;

                        case COMeSpecType.PADuty:
                            pa.ReturnTemp = pa.drawTray.T;
                            break;
                    }
                }
            }
        }

        public static void InitialiseStrippers(COMColumn column)
        {
            ConnectingStreamCollectionTest LiquidStreams = column.LiquidStreams;

            foreach (ConnectingStreamTest ds in LiquidStreams) // do liquid int ersection streams
            {
                TraySectionTest st = ds.EngineDrawSection;
                if (st != null)
                {
                    SpecificationTest s = column.Specs.GetActiveSpecs().GetTrayLquidSpec(st.Guid, st.Trays.Last().Guid);
                    if (s != null)
                        ds.FlowEstimate = (MoleFlow)s.Value;
                    else
                        ds.FlowEstimate = 0.1;
                }
            }
        }

        public void Update_FlowFactors(COMColumn column)
        {
            Update_LSS_LFactor(column);
            Update_VSS_VFactor(column);
            UpdatePAdraws(column);
            UpdateConnectedSideDraws(column);
            UpdateSideNetReturns(column);
        }

        public static void UpdatePAdraws(COMColumn column)
        {
            for (int section = 0; section < column.TraySections.Count; section++)
                for (int n = 0; n < column.PumpArounds.Count; n++)
                {
                    PumpAroundTest p = column.PumpArounds[n];
                    if (p.drawTray != null)
                        p.MoleFlow.BaseValue = p.drawTray.L * p.DrawFactor;
                }
        }

        public static void UpdateConnectedSideDraws(COMColumn column)
        {
            //ConnectingStreamCollection LiquidStreams = column.LiquidStreams;  // need to know draw tray and return tray
            //ConnectingStreamCollection VapourStreams = column.VapourStreams;  // need to know draw tray and return tray

            foreach (ConnectingStreamTest cs in column.LiquidStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                if (cs.DrawTrayIndex >= 0)
                    cs.FlowEstimate = column.MainTraySection.Trays[cs.DrawTrayIndex].L * cs.DrawFactor;

            foreach (ConnectingStreamTest cs in column.VapourStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                if (cs.DrawTrayIndex >= 0)
                    cs.FlowEstimate = column.MainTraySection.Trays[cs.DrawTrayIndex].V * cs.DrawFactor;
        }

        public static void UpdateSideNetReturns(COMColumn column)  // e.g. for mole balance
        {
            // ConnectingStreamCollection LiquidStreams = column.ConnectingNetFlows.LiquidStreams;
            // ConnectingStreamCollection VapourStreams = column.ConnectingNetFlows.VapourStreams;

            foreach (ConnectingStreamTest st in column.ConnectingNetVapourStreams)
            {
                //TraySection section = st.EngineDrawSection;
                TrayTest tray = st.engineDrawTray;
                st.FlowEstimate = tray.V;
            }

            foreach (ConnectingStreamTest st in column.ConnectingNetLiquidStreams)
            {
                st.FlowEstimate = Math.Exp(st.engineDrawTray.StripFact);
                TrayTest tray = st.engineDrawTray;
                st.FlowEstimate = tray.L;
            }
        }

        public void ResetFlowValues()
        {
            if (this.column != null)
                foreach (TraySectionTest traysection in column.TraySections)
                    foreach (TrayTest tray in traysection)
                    {
                        tray.L = 0;
                        tray.V = 0;
                        tray.lss_spec = 0;
                        tray.vss_spec = 0;
                    }
        }

        public void InitialiseFlowsSimplified(COMColumn column)
        {
            ResetFlowValues();

            foreach (SideStreamTest ss in column.LiquidSideStreams)
            {
                if (ss.EngineDrawSection != null)
                {
                    SpecificationTest s = column.Specs.GetSpecs(ss).GetLiquidDrawSpec(ss.EngineDrawSection.Guid, ss.EngineDrawTray.Guid);
                    if (s != null)
                        ss.EngineDrawTray.lss_spec = (MoleFlow)s.SpecValue / column.FeedRatio; // Do Side Draws
                    else
                        ss.EngineDrawTray.lss_spec = 0;
                }
            }

            foreach (SideStreamTest ss in column.VapourSideStreams)
            {
                if (ss.EngineDrawSection != null)
                {
                    SpecificationTest s = column.Specs.GetSpecs(ss).GetVapourDrawSpec(ss.EngineDrawSection.Guid, ss.EngineDrawTray.Guid);
                    if (s != null)
                        ss.EngineDrawTray.vss_spec = (MoleFlow)s.SpecValue / column.FeedRatio; // Do Side Draws
                    else
                        ss.EngineDrawTray.vss_spec = 0;
                }
            }

            foreach (ConnectingStreamTest d in column.ConnectingDraws)
            {
                int DrawSectionIndex = d.EngineDrawSection.SectionIndex;
                int RetSectionIndex = d.EngineReturnSection.SectionIndex;
                //Tray drawtray = d.EngineDrawTray;
                //drawtray.lss_spec = d.Flow;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && d.isliquid)
                {
                    InterSectionLiquidDraws[DrawSectionIndex][d.DrawTrayIndex] = d.FlowEstimate;
                    InterSectionLiquidFeeds[RetSectionIndex][d.ReturnTrayIndex] = d.FlowEstimate;
                }
            }

            foreach (ConnectingStreamTest d in column.ConnectingNetFlows)
            {
                int DrawSectionIndex = d.EngineDrawSection.SectionIndex;
                int RetSectionIndex = d.EngineReturnSection.SectionIndex;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0)
                {
                    //int erSectionVapourDraws[DrawSectionIndex][d.DrawTrayIndex] = d.FlowEstimate;
                    InterSectionVapourFeeds[RetSectionIndex][d.ReturnTrayIndex] = 0.01; // d.FlowEstimate;
                }
            }

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;
                section.Trays[0].V = 0.05;

                foreach (PumpAroundTest pa in column.PumpArounds)
                {
                    if (pa.drawTray != null && pa.returnTray != null)
                    {
                        pa.drawTray.L -= pa.MoleFlow;
                        pa.returnTray.L += pa.MoleFlow;
                    }
                }

                section.Trays[0].L = 1;

                for (int trayNo = 1; trayNo < TrayCount; trayNo++) // initialise liquid rates;
                {
                    section.Trays[trayNo].L = 1;
                    section.Trays[trayNo].V = 1;
                }
            }
        }

        public void InitialiseFlows(COMColumn column)
        {
            double TotalTopAndSideDraws;
            double TotalFeedsAndReturns;
            ResetFlowValues();
            //Array.Clear(int erSectionLiquidDraws);

            foreach (SideStreamTest stream in column.LiquidSideStreams)
            {
                if (stream.EngineDrawSection != null && stream.EngineDrawTray != null)
                {
                    SpecificationCollectionTest sc = column.Specs.GetSpecs(stream);
                    SpecificationTest s = sc.GetLiquidDrawSpec(stream.EngineDrawSection.Guid, stream.EngineDrawTray.Guid);
                    if (s != null)
                        stream.EngineDrawTray.lss_spec = (MoleFlow)s.SpecValue * column.FeedRatio; // Do Side Draws
                    else
                        stream.EngineDrawTray.lss_spec = 0.5; // if draw exists, set flow estimate to non-zero value
                }
            }

            foreach (SideStreamTest stream in column.VapourSideStreams)
            {
                if (stream.EngineDrawSection != null)
                {
                    SpecificationTest s = column.Specs.GetSpecs(stream).GetVapourDrawSpec(stream.EngineDrawSection.Guid, stream.EngineDrawTray.Guid);
                    if (s != null)
                        stream.EngineDrawTray.vss_spec = (MoleFlow)s.SpecValue * column.FeedRatio; // Do Side Draws
                    else
                        stream.EngineDrawTray.vss_spec = 0.1; // if draw exists, set flow estimate to non-zero value
                }
            }

            foreach (ConnectingStreamTest d in column.ConnectingDraws)
            {
                int DrawSectionIndex = d.EngineDrawSection.SectionIndex;
                int RetSectionIndex = d.EngineReturnSection.SectionIndex;
                //Tray drawtray = d.EngineDrawTray;
                //drawtray.lss_spec = d.Flow;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && d.isliquid)
                {
                    InterSectionLiquidDraws[DrawSectionIndex][d.DrawTrayIndex] += d.FlowEstimate;
                    InterSectionLiquidFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;
                }

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && !d.isliquid)
                {
                    InterSectionVapourDraws[DrawSectionIndex][d.DrawTrayIndex] += d.FlowEstimate;
                    InterSectionVapourFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;
                }
            }

            foreach (ConnectingStreamTest d in column.ConnectingNetFlows)
            {
                int DrawSectionIndex = d.EngineDrawSection.SectionIndex;
                int RetSectionIndex = d.EngineReturnSection.SectionIndex;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && d.isliquid)
                    InterSectionLiquidFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && !d.isliquid)
                    InterSectionVapourFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;
            }

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;
                SpecificationTest vaprate;

                switch (section.CondenserType)
                {
                    case COMCondType.Partial:
                        vaprate = column.Specs.GetActiveSpecs().GetCondVapourRateSpec(section.Guid);

                        if (vaprate != null)
                            section.Trays[0].V = (MoleFlow)vaprate.SpecValue / column.FeedRatio;     // Vapour Spec
                        else
                            section.Trays[0].V = 0.05;
                        break;

                    case COMCondType.Subcooled:
                        section.Trays[0].V = 0;
                        break;

                    case COMCondType.TotalReflux:
                        vaprate = column.Specs.GetActiveSpecs().GetCondVapourRateSpec(section.Guid);

                        if (vaprate != null)
                            section.Trays[0].V = (MoleFlow)vaprate.SpecValue / column.FeedRatio;     // Vapour Spec
                        else
                            section.Trays[0].V = 0.05;
                        break;

                    case COMCondType.None:
                        section.Trays[0].V = 0.01; // StripperDraws[section][0];
                        break;
                }

                TotalTopAndSideDraws = 0;
                if (MainFeed == null)
                    return;
                TotalFeedsAndReturns = MainFeed.MolarFlow_;
                TotalTopAndSideDraws += section.Trays[0].V;

                for (int ii = 0; ii < column.TraySections[sectionNo].Trays.Count; ii++)
                {
                    TotalTopAndSideDraws += section.Trays[ii].lss_spec;
                    //+ InterSectionLiquidDraws[sectionNo][ii] - InterSectionLiquidFeeds[sectionNo][ii]
                    //+ InterSectionVapourDraws[sectionNo][ii] - InterSectionVapourFeeds[sectionNo][ii];
                    TotalFeedsAndReturns += InterSectionVapourFeeds[sectionNo][ii];
                }

                switch (section.CondenserType)
                {
                    case COMCondType.Partial:
                    case COMCondType.Subcooled:
                    case COMCondType.TotalReflux:
                        SpecificationTest s = column.Specs.GetActiveSpecs().GetRefluxRatioSpec(section.Guid); // REFLUX Rate is Known

                        foreach (PumpAroundTest pa in column.PumpArounds)
                        {
                            if (pa.drawTray != null && pa.returnTray != null)
                            {
                                pa.drawTray.L -= pa.MoleFlow;
                                pa.returnTray.L += pa.MoleFlow;
                            }
                        }

                        if (s != null)
                            section.Trays[0].L += (section.Trays[0].V + section.Trays[0].lss_spec) * s.SpecValue + InterSectionLiquidFeeds[sectionNo][0];
                        else
                            section.Trays[0].L += (section.Trays[0].V + section.Trays[0].lss_spec) * DefaultRefluxRatio + InterSectionLiquidFeeds[sectionNo][0]; // assume reflux ratio of 1

                        for (int trayNo = 1; trayNo < TrayCount - 1; trayNo++) // initialise liquid rates;
                            section.Trays[trayNo].L += section.Trays[trayNo - 1].L +
                                section.Trays[trayNo].feed.RatioedLiquidFeed -
                                section.Trays[trayNo].lss_spec +
                                InterSectionLiquidFeeds[sectionNo][trayNo] -
                                InterSectionLiquidDraws[sectionNo][trayNo];

                        section.Trays[TrayCount - 1].L = TotalFeeds - TotalTopAndSideDraws;

                        section.Trays[TrayCount - 1].V = section.Trays[TrayCount - 2].L - section.Trays[TrayCount - 1].L + section.Trays[TrayCount - 1].feed.RatioedVapourFeed;

                        for (int tray = TrayCount - 2; tray > 0; tray--)
                            section.Trays[tray].V = section.Trays[tray + 1].V
                                + InterSectionVapourFeeds[sectionNo][tray]
                                + section.BottomTray.feed.RatioedVapourFeed;
                        break;

                    case COMCondType.None:  // e.g. stripper, simplified
                        section.Trays[0].L += section.Trays[0].feed.RatioedLiquidFeed
                            + InterSectionLiquidFeeds[sectionNo][0]
                            - InterSectionLiquidDraws[sectionNo][0];

                        if (section.Trays[0].L == 0)
                            section.Trays[0].L = 1;

                        for (int tray = 1; tray < TrayCount; tray++) // initialise liquid rates; // add vapaour to liquid rate to match excel version
                        {
                            section.Trays[tray].L = section.Trays[tray - 1].L + section.Trays[tray - 1].feed.RatioedLiquidFeed - section.Trays[tray].lss_spec;
                            if (section.Trays[tray].L <= 0)
                                section.Trays[tray].L = 0.1;
                        }

                        section.Trays[TrayCount - 1].V = section.Trays[TrayCount - 1].feed.RatioedVapourFeed;

                        if (section.Trays[TrayCount - 1].V == 0)
                            section.Trays[TrayCount - 1].V = 0.01;

                        for (int tray = TrayCount - 2; tray >= 0; tray--)
                            section.Trays[tray].V = section.Trays[tray].feed.RatioedVapourFeed + section.Trays[tray].feed.RatioedVapourFeed
                                + section.Trays[tray + 1].V
                                + InterSectionVapourFeeds[sectionNo][tray]
                                - InterSectionVapourDraws[sectionNo][tray];
                        break;
                }
            }
        }

        public void InitialiseFlows2(COMColumn column)
        {
            ResetFlowValues();

            foreach (TraySectionTest section in column.TraySections)
            {
                section.TotalTopAndSideProducts = 0;
                section.TotalFeeds = 0;
                foreach (TrayTest tray in section)
                {
                    tray.Ltemp = 0;
                    tray.Vtemp = 0;
                }
            }

            double MW = column.Components.MW();
            double SG = column.Components.SG();

            foreach (SideStreamTest stream in column.SideStreamsAll())
            {
                if (stream.EngineDrawSection != null && stream.EngineDrawTray != null)
                {
                    SpecificationCollectionTest sc = column.Specs.GetActiveSpecs().GetSpecs(stream);

                    foreach (SpecificationTest s in sc)
                    {
                        switch (s.engineSpecType)
                        {
                            case COMeSpecType.LiquidProductDraw:
                                stream.EngineDrawTray.lss_spec = s.SpecValueMoles(MW, SG) * column.FeedRatio;
                                break;

                            case COMeSpecType.VapProductDraw:
                                stream.EngineDrawTray.vss_spec = s.SpecValueMoles(MW, SG) * column.FeedRatio;
                                break;

                            case COMeSpecType.DistSpec:
                                stream.EngineDrawTray.lss_spec = 0.5;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            //
            foreach (TraySectionTest section in column.TraySections)
            {
                foreach (TrayTest tray in section)
                {
                    if (tray.feed.MolarFlow_.IsKnown) // Do all feeds
                    {
                        section.TotalFeeds += tray.feed.MolarFlow_ * column.feedRatio;
                        tray.Ltemp = tray.feed.RatioedLiquidFeed;
                        tray.Vtemp = tray.feed.RatioedVapourFeed;
                    }

                    tray.Ltemp -= tray.lss_spec;
                    tray.Vtemp -= tray.vss_spec;
                    section.TotalTopAndSideProducts += tray.lss_spec; // do side draw products
                    section.TotalTopAndSideProducts += tray.vss_spec;
                }
            }

            foreach (ConnectingStreamTest d in column.ConnectingDraws)
            {
                if (d.isliquid)
                {
                    d.engineDrawTray.Ltemp -= d.FlowEstimate;
                    d.engineReturnTray.Ltemp += d.FlowEstimate;

                    if (d.engineDrawTray != d.EngineDrawSection.BottomTray)
                        d.EngineDrawSection.TotalTopAndSideProducts += d.FlowEstimate;

                    d.EngineReturnSection.TotalFeeds += d.FlowEstimate;
                }
                else
                {
                    d.engineDrawTray.Vtemp -= d.FlowEstimate;
                    d.engineReturnTray.Vtemp += d.FlowEstimate;

                    d.EngineDrawSection.TotalTopAndSideProducts += d.FlowEstimate;
                    d.EngineReturnSection.TotalFeeds += d.FlowEstimate;
                }
            }

            foreach (ConnectingStreamTest d in column.ConnectingNetFlows)
            {
                if (d.engineDrawTray != d.EngineDrawSection.BottomTray)
                    d.EngineDrawSection.TotalTopAndSideProducts += d.FlowEstimate;
                d.EngineReturnSection.TotalFeeds += d.FlowEstimate;

                if (d.isliquid)
                {
                    //d.EngineDrawTray.Ltemp -= d.FlowEstimate;
                    d.engineReturnTray.Ltemp += d.FlowEstimate;
                }
                else
                {
                    //d.EngineDrawTray.Vtemp -= d.FlowEstimate;
                    d.engineReturnTray.Vtemp += d.FlowEstimate;
                }
            }

            foreach (PumpAroundTest pa in column.PumpArounds)
            {
                if (pa.drawTray != null && pa.returnTray != null)
                {
                    pa.drawTray.Ltemp -= pa.MoleFlow * column.feedRatio;
                    pa.returnTray.Ltemp += (pa.MoleFlow * column.feedRatio);
                }
            }

            // top vapours
            foreach (TraySectionTest section in column.TraySections)
            {
                SpecificationTest vaprate;
                switch (section.CondenserType)
                {
                    case COMCondType.Partial: // vapour can be condensed
                    case COMCondType.TotalReflux:
                        vaprate = column.Specs.GetActiveSpecs().GetCondVapourRateSpec(section.Guid);

                        if (vaprate != null)
                        {
                            section.TopTray.Vtemp = (MoleFlow)vaprate.SpecValue * column.FeedRatio;     // Vapour Spec
                            section.VapSpec = vaprate.SpecValue * column.feedRatio;
                        }
                        else
                        {
                            section.TopTray.Vtemp = 0.05;  // set to some non-zero value
                            section.VapSpec = 0.05;
                        }

                        section.TotalTopAndSideProducts += section.TopTray.Vtemp;
                        break;

                    case COMCondType.Subcooled:
                        section.TopTray.Vtemp = 0;
                        break;

                    case COMCondType.None: // vapour is what it is.
                        if (section == column.MainTraySection)
                        {
                            SpecificationTest s = column.Specs.GetRefluxRatioSpec(section.Guid);
                            double rr;
                            if (s is not null)
                            {
                                rr = s.specvalue;
                                section.TopTray.Ltemp = rr;
                            }

                            section.TopTray.Vtemp = 0.01; // StripperDraws[section][0];
                            section.TotalTopAndSideProducts += 0.01;
                            if(section.TopTray.Ltemp ==0)
                                section.TopTray.Ltemp = 1;
                        }
                        break;
                }
            }

            // do reflux ratio
            foreach (TraySectionTest section in column.TraySections)
            {
                if (section.HasCondenser)
                {
                    SpecificationTest refluxratiospec = null;
                    switch (section.CondenserType)
                    {
                        case COMCondType.Partial:
                        case COMCondType.Subcooled:
                        case COMCondType.TotalReflux:
                            refluxratiospec = column.Specs.GetActiveSpecs().GetRefluxRatioSpec(section.Guid); // REFLUX Rate is Known
                            break;
                    }

                    if (refluxratiospec is null)
                        section.RefluxRatioSpec = 3;
                    else
                        section.RefluxRatioSpec = refluxratiospec.SpecValue;

                    section.Trays[0].Ltemp = (section.Trays[0].lss_spec + section.Trays[0].Vtemp) * section.RefluxRatioSpec;
                }
                else if (section.HasReboiler)
                {
                }
                else // stripper/absorber
                {
                    foreach (PumpAroundTest pa in column.PumpArounds)
                    {
                        if (pa.returnTray == section.TopTray)
                        {
                            section.Trays[0].Ltemp += pa.MoleFlow * 1.1 * column.feedRatio;
                            section.HasReflux = true;
                        }
                    }
                    //section.Trays[0].Ltemp += 0.1;
                    //section.Trays[0].Ltemp += 2; // vac column
                }
            }

            foreach (TraySectionTest section in column.TraySections)
            {
                section.Trays[0].L = section.Trays[0].Ltemp;

                for (int i = 1; i < section.Trays.Count; i++)
                {
                    section.Trays[i].L = section.Trays[i - 1].L + section.Trays[i].Ltemp;
                }

                double BottomStream = section.TotalFeeds - section.TotalTopAndSideProducts;

                section.BottomTray.Vtemp += section.BottomTray.L - BottomStream;
                section.BottomTray.V = section.BottomTray.Vtemp;
                section.BottomTray.L = BottomStream;

                for (int i = section.Trays.Count - 1; i > 0; i--)
                {
                    section.Trays[i - 1].V += section.Trays[i].V + section.Trays[i - 1].Vtemp;
                }

                if (!double.IsNaN(section.VapSpec))
                    section.TopTray.V = section.VapSpec;
            }

            foreach (TraySectionTest section in column.TraySections)
            {
                for (int i = 1; i < section.Trays.Count; i++)
                {
                    if (section.Trays[i].L < 0.0001)
                        section.Trays[i].L = 0.1;
                    if (section.Trays[i].V < 0.0001)
                        section.Trays[i].V = 0.1;
                }
            }
        }

        public void AddTrayHeatBalancesToSpecs()
        {
            int count = 0;
            List<SpecificationTest> remove = new();

            foreach (SpecificationTest item in column.Specs)
                if (item.engineSpecType == COMeSpecType.TrayDuty)
                    remove.Add(item);

            foreach (SpecificationTest item in remove)
            {
                int loc = column.Specs.IndexOf(item);
                column.Specs.RemoveAt(loc);
            }

            foreach (TraySectionTest ts in column.TraySections)
            {
                foreach (TrayTest tray in ts)
                {
                    count++;
                    if (tray.IsHeatBalanced)
                    {
                        SpecificationTest spec = new("tray duty", tray.Duty, ePropID.EnergyFlow, COMeSpecType.TrayDuty, ts, tray, tray, true);
                        spec.StageNo = count;
                        column.Specs.Add(spec);
                    }
                }
            }
        }

        public void UpdatePorts(bool FlashAllPorts = false)
        {
            int count = 0;
            foreach (TraySectionTest traySection in column.TraySections)
            {
                foreach (TrayTest tray in traySection)
                {
                    tray.Name = "Tray" + count.ToString();
                    tray.SetUpPorts(column, FlashAllPorts);

                    foreach (var item in tray.Ports)
                        if (!item.IsFlowIn)
                            item.MolarFlow_.BaseValue /= column.feedRatio;

                    tray.FlashOutPorts(column, FlashAllPorts);

                    count++;
                }
            }

            column.MainSectionStages.BottomTray.TrayLiquid.Flash(forceflash: true);

            foreach (ConnectingStreamTest cs in column.ConnectingDraws)
            {
                TrayTest T = cs.engineDrawTray;
                T.liquidDrawRight.MolarFlow_.BaseValue = cs.FlowEstimate / column.feedRatio;
            }

            foreach (SideStreamTest ss in column.LiquidSideStreams)
            {
                TrayTest tray = ss.EngineDrawTray;
                tray.liquidDrawRight.MolarFlow_.BaseValue = tray.lss_estimate / column.feedRatio;
                tray.liquidDrawRight.MolarFlow_.origin = SourceEnum.UnitOpCalcResult;

                // Debug.Print (tray.Name);
                // Debug.Print (tray.liquidDrawRight.Components.MW().ToString());
                // Debug.Print (tray.liquidDrawRight.Components.SG().ToString());
            }

            foreach (PumpAroundTest pa in column.PumpArounds)//Only need to update return streams
            {
                int returnsectionindex = pa.returnSection.SectionIndex;
                int returntrayindex = pa.ReturnTrayNo;
                pa.ReturnPort.Properties.Clear();
                pa.ReturnPort.cc = column.Components.Clone();
                pa.ReturnPort.cc.Origin = SourceEnum.UnitOpCalcResult;
                //pa.returnPort.SetPortValue(ePropID.H, PAreturnEnthalpy[returnsectionindex][returntrayindex],SourceEnum.UnitOpCalcResult);

                pa.ReturnPort.SetPortValue(ePropID.P, pa.returnTray.P, SourceEnum.UnitOpCalcResult);
                pa.ReturnPort.SetPortValue(ePropID.T, pa.returnTray.T, SourceEnum.UnitOpCalcResult);
                pa.ReturnPort.MolarFlow_.BaseValue = pa.MoleFlow / column.feedRatio;
                pa.ReturnPort.MolarFlow_.origin = SourceEnum.UnitOpCalcResult;
                pa.ReturnPort.cc.SetMolFractions(pa.drawTray.LiqComposition);
                pa.ReturnPort.Flash(true);

                pa.DrawPort.Properties.Clear();
                pa.DrawPort.cc = column.Components.Clone();
                pa.DrawPort.cc.Origin = SourceEnum.UnitOpCalcResult;
                pa.DrawPort.SetPortValue(ePropID.T, pa.drawTray.T, SourceEnum.UnitOpCalcResult);
                pa.DrawPort.SetPortValue(ePropID.P, pa.drawTray.P, SourceEnum.UnitOpCalcResult);
                pa.DrawPort.SetPortValue(ePropID.MOLEF, pa.MoleFlow / column.feedRatio, SourceEnum.UnitOpCalcResult);
                pa.DrawPort.cc.SetMolFractions(pa.drawTray.LiqComposition);
                pa.DrawPort.Flash();
            }
        }

        public void UpdateVapEnthalpyParametersOld()
        {
            switch (column.SolverOptions.VapEnthalpyMethod)
            {
                case COMColumnEnthalpyMethod.BostonBrittHdep:
                    foreach (TraySectionTest section in column.TraySections)
                        foreach (TrayTest tray in section)
                            tray.UpdateVapourDepEnthalpies(column.Components, column.Thermo);

                    break;

                case COMColumnEnthalpyMethod.SimpleLinear:
                    foreach (TraySectionTest section in column.TraySections)
                        foreach (TrayTest tray in section)
                            tray.UpdateVapourLinearEnthalpies(column.Components, column.Thermo);

                    break;

                default:
                    break;
            }
        }

        public void UpdateVapEnthalpyParameters()
        {
            switch (column.SolverOptions.VapEnthalpyMethod)
            {
                case COMColumnEnthalpyMethod.BostonBrittHdep:
                    foreach (TraySectionTest section in column.TraySections)
                        //Parallel.For(0, section.Trays.Count, trayNo =>
                            //{
                             foreach (TrayTest tray in section)
                                    tray.UpdateVapourDepEnthalpies(column.Components, column.Thermo);
                    //section.Trays[trayNo].UpdateVapourDepEnthalpies(column.Components, column.Thermo);
                    //  });

                    break;

                case COMColumnEnthalpyMethod.SimpleLinear:
                    foreach (TraySectionTest section in column.TraySections)
                        foreach (TrayTest tray in section)
                            tray.UpdateVapourLinearEnthalpies(column.Components, column.Thermo);

                    break;

                default:
                    break;
            }
        }

        public void UpdateLiqEnthalpyParameters()
        {
            switch (column.SolverOptions.LiqEnthalpyMethod)
            {
                case COMColumnEnthalpyMethod.BostonBrittHdep:
                    foreach (TraySectionTest section in column.TraySections)
                        //foreach (Tray tray in section)
                        Parallel.For(0, section.Trays.Count, TrayNo =>
                        {
                            section.Trays[TrayNo].UpdateLiquidDepEnthalpies(column.Components, column.Thermo);
                        });

                    foreach (PumpAroundTest pa in column.PumpArounds)
                    {
                        pa.enthalpyDepartureLinearisation.LiqUpdate(column.Components, pa.drawTray.LiqComposition,
                            pa.drawTray.P.BaseValue, pa.ReturnTemp, pa.ReturnTemp + 1, column.Thermo);
                    }

                    break;

                case COMColumnEnthalpyMethod.SimpleLinear:

                    foreach (TraySectionTest section in column.TraySections)
                        foreach (TrayTest tray in section)
                            tray.UpdateLiquidLinearEnthalpies(column.Components, column.Thermo);

                    foreach (PumpAroundTest pa in column.PumpArounds)
                    {
                        pa.enthalpySimpleLinearisation.LiqUpdate(column.Components, pa.drawTray.LiqComposition,
                            pa.returnTray.P.BaseValue, pa.ReturnTemp, pa.ReturnTemp + 1, column.Thermo);
                    }

                    break;

                default:
                    break;
            }
        }

        public void UpdateInitialCompositions(double dampfactor)
        {
            foreach (TraySectionTest section in column.TraySections)
                foreach (TrayTest tray in section)
                {
                    for (int i = 0; i < column.Components.Count; i++)
                    {
                        tray.LiqCompositionInitial[i] += (tray.LiqComposition[i] - tray.LiqCompositionInitial[i]) * dampfactor;
                        tray.VapCompositionInitial[i] += (tray.VapComposition[i] - tray.VapCompositionInitial[i]) * dampfactor;
                    }
                }
        }

        public void UpdateCompositions()
        {
            foreach (TraySectionTest section in column.TraySections)
                foreach (TrayTest tray in section)
                {
                    tray.LiqComposition = (double[])tray.LiqCompositionPred.Clone();
                    tray.VapComposition = (double[])tray.VapCompositionPred.Clone();
                }
        }

        public void SetBasicValues(COMColumn column)
        {
            foreach (TraySectionTest traySection in column.TraySections)
            {
                if (traySection.HasCondenser)
                    traySection.Trays[0].IsHeatBalanced = false;
                if (traySection.HasReboiler)
                    traySection.BottomTray.IsHeatBalanced = false;
            }

            TotNoTrays = column.TotNoStages;
            NoSections = column.NoSections;

            column.Components.Clear();

            foreach (TraySectionTest traysection in column.TraySections)
            {
                foreach (TrayTest tray in traysection)
                {
                    if (tray.feed != null & tray.feed.cc != null)
                        for (int i = 0; i < tray.feed.cc.Count; i++)
                        {
                            if (!column.Components.Contains(tray.feed.cc[i].Name))
                                column.Components.Add(tray.feed.cc[i]);
                        }
                }
            }

            //MathNet.Numerics.Control.UseNativeMKL();       // simialr as USeManaged but delay while loading
            // MathNet.Numerics.Control.UseNativeOpenBLAS();  // very slow
            MathNet.Numerics.Control.UseManaged();
            //  MathNet.Numerics.Control.UseMultiThreading();  // seems to have no impact
            //  MathNet.Numerics.Control.UseNativeMKL();
            //  MathNet.Numerics.Control.UseNativeCUDA();
        }

        public bool UpdateInitialAlphas(ThermoDynamicOptions thermo, bool useWilson = false)
        {
            bool res = true;
            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int No = section.Trays.Count;
                Parallel.For(0, No, i => {
                //for (int i = 0; i < No; i++) // initialise liquid rates, calculate from assumed vapour rates;
                //{
                    TrayTest tray = section.Trays[i];
                    if (useWilson)
                    {
                        ThermoDynamicOptions options = new(thermo);
                        options.KMethod = enumEquiKMethod.Wilson;
                        res = tray.UpdateAlphas(column.Components, tray.LiqCompositionInitial, tray.VapCompositionInitial, column.solverOptions.AlphaMethod, options);
                       // if (!res)
                       //     return res;
                        //tray.UpdateAlphas(column.Components, tray.LiqCompositionInitial, tray.VapCompositionInitial, ColumnAlphaMethod.MA, options);
                    }
                    else
                    {
                        res = tray.UpdateAlphas(column.Components, tray.LiqCompositionInitial, tray.VapCompositionInitial, column.solverOptions.AlphaMethod, thermo);
                       // if (!res)
                        //    return res;
                        //tray.UpdateAlphas(column.Components, tray.LiqCompositionInitial, tray.VapCompositionInitial, ColumnAlphaMethod.MA, thermo);
                    }
                    //  if (!res)
                    //      return false;
                });
            }
            return true;
        }

        public bool UpdateAlphas(ThermoDynamicOptions thermo)
        {
            bool res = false;
            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int No = section.Trays.Count;
                Parallel.For(0, No, i =>
                {
                   // for (int i = 0; i < No; i++) // initialise liquid rates, calculate from assumed vapour rates;
                    {
                        TrayTest tray = section.Trays[i];
                        res = tray.UpdateAlphas(column.Components, tray.LiqComposition, tray.VapComposition, column.solverOptions.AlphaMethod, thermo);
                        //if (!res)
                        //    return res;
                    }
                });
            }
            return true;
        }

        public void EstimateTs(COMColumn column, COMColumnKMethod KMethod)
        {
            if (this.column is null)
                return;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int No = section.Trays.Count;
                for (int trayNo = 0; trayNo < No; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    TrayTest tray = section.Trays[trayNo];
                    //var res2 = tray.EstimateTFast(column.Components, tray.LiqCompositionPred, tray.VapCompositionPred, ColumnKMethod.MA);
                    var res1 = tray.EstimateTFast(column.Components, tray.LiqCompositionPred, tray.VapCompositionPred, KMethod);

                    if (tray.TPredicted < 0)
                        tray.TPredicted = 10;
                    else if (tray.TPredicted > 1000)
                        tray.TPredicted = 1000;

                    //Debug.Print (res1.ToString() + " " + res2.ToString());
                    //if (Math.Abs(res2 - res1) > 5)
                    //    Debugger.Break();
                }
            }
        }

        public void EstimateTs2(COMColumn column, COMColumnKMethod TMethod)
        {
            if (this.column is null)
                return;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    section.Trays[trayNo].EstimateTFast(column.Components, section.Trays[trayNo].LiqCompositionPred, section.Trays[trayNo].VapCompositionPred, TMethod);
                    //var res2 = tray.EstimateT(column.Components, tray.LiqCompositionPred, tray.VapCompositionPred, ColumnTestimateMethod.MA);
                }
            }

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];

                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    TrayTest tray = section.Trays[trayNo];
                    if (tray.TPredicted < 0)
                        tray.TPredicted = 10;
                    if (tray.TPredicted > 1000)
                        tray.TPredicted = 1000;
                }
            }
        }

        public void IntialiseStripFactorsEtc()
        {
            if (column is null)
                return;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int No = section.Trays.Count;
                for (int trayNo = 0; trayNo < No; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    TrayTest tray = section.Trays[trayNo];
                    tray.StripFact = tray.V / tray.L;
                    tray.LiqDrawFactor = tray.lss_spec / tray.L;

                    if (tray.V == 0) // no vapour so draw rate must be 0;
                        tray.VapDrawFactor = 1;
                    else
                        tray.VapDrawFactor = tray.vss_spec / tray.V;
                }
            }

            foreach (ConnectingStreamTest st in column.LiquidStreams)
                if (st.engineDrawTray != null)
                    st.DrawFactor = st.FlowEstimate / st.engineDrawTray.L; // assume column draw flow = stripper product flow

            foreach (ConnectingStreamTest st in column.VapourStreams)
                if (st.engineDrawTray != null)
                    st.DrawFactor = st.FlowEstimate / st.engineDrawTray.V; // assume column draw flow = stripper product flow

            foreach (PumpAroundTest  pa in column.PumpArounds)
                if (pa.drawTray != null)
                    pa.DrawFactor = pa.MoleFlow * column.feedRatio / pa.drawTray.L;
        }

        public void ReEstimateStripFactors()
        {
            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int No = section.Trays.Count;

                for (int trayNo = 0; trayNo < No; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    TrayTest tray = section.Trays[trayNo];
                    tray.StripFact = tray.V / tray.L;
                    tray.LiqDrawFactor = tray.lss_spec / tray.L;

                    if (tray.V == 0) // no vapour so draw rate must be 0;
                        tray.VapDrawFactor = 1;
                    else
                        tray.VapDrawFactor = tray.vss / tray.V;
                }
            }

            int no = column.LiquidStreams.StreamList.Count;
            for (int i = 0; i < no; i++)
            {
                ConnectingStreamTest st = column.LiquidStreams.StreamList[i];
                if (st.engineDrawTray != null)
                    st.DrawFactor = st.FlowEstimate * column.feedRatio / st.engineDrawTray.L; // assume column draw flow = stripper product flow
            }

            no = column.VapourStreams.StreamList.Count;
            for (int i1 = 0; i1 < no; i1++)
            {
                ConnectingStreamTest st = column.VapourStreams.StreamList[i1];
                if (st.engineDrawTray != null)
                    st.DrawFactor = st.FlowEstimate * column.feedRatio / st.engineDrawTray.V; // assume column draw flow = stripper product flow
            }

            no = column.PumpArounds.Pas.Count;
            for (int i2 = 0; i2 < no; i2++)
            {
                PumpAroundTest pa = column.PumpArounds.Pas[i2];
                if (pa.drawTray != null)
                    pa.DrawFactor = pa.MoleFlow * column.feedRatio / pa.drawTray.L;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool CalcInitialTs(COMColumn column)
        {
            SetBasicValues(column);
            column.Components = new Components();
            foreach (TraySectionTest traysection in column.TraySections)
            {
                foreach (TrayTest tray in traysection)
                {
                    if (tray.feed != null & tray.feed.cc != null)
                        column.Components.Add(tray.feed.cc);
                }
            }

            NoComps = column.Components.Count;

            if (NoComps == 0)
                return false;

            WaterCompNo = column.Components.GetWaterCompNo();

            //DateTime start = DateTime.Now;

            InitialiseMatrices(column); // no calcs here
            InterpolatePressures();
            InitialiseFeeds(column);

            ThermoDynamicOptions thermotemp = new();
            if (!column.SolutionConverged)
                thermotemp.KMethod = enumEquiKMethod.Wilson;
            else
                thermotemp.KMethod = column.Thermo.KMethod;

            if (column.SolutionConverged)  // get values from last run
            {
            }
            else
            {
                InitialiseStrippers(column);
                InitialisePumpArounds(column);
                InitialiseFlows(column);
                InitialiseTrayCompositions(column);
                SetInitialTrayTemps(column);
                if (!UpdateInitialAlphas(thermotemp))
                    return false;
                IntialiseStripFactorsEtc();
            }

            SolveComponentBalance(column);  // get initial estimate of componenents
            UpdatePredCompositions();
            UpdatePredCompositions();
            EstimateTs(column, column.solverOptions.KMethod);

            return true;
        }

        public bool UpdateColumnBalance(ref double ErrorSum, enumCalcResult cres)
        {
            if (!SolveComponentBalance(column))
            {
                return false;
            }
            UpdatePredCompositions();
            UpdateLandV();
            Update_FlowFactors(column);
            EstimateTs(column, column.solverOptions.KMethod);
            EnthalpiesUpdate(ref cres);    // future enthalpies
            if (cres == enumCalcResult.Failed)
                return false;
            UpdateTrayEnthalpyBalances();  // future balance
            CalcErrors(true);              // Future Erros
            ProcessActiveSpecs(out Errors, true);
            ErrorSum = SumArraySquared(Errors);
            return true;
        }

        public bool UpdateFactors(double[] grads, double gradientfactor = double.NaN)
        {
            if (grads is null)
                return false;

            double[] gradmod = (double[])grads.Clone();

            int deltacount = 0;
            double MaxGrad = gradmod.AbsMax();

            switch (column.SolverOptions.SFUpdateMethod)
            {
                case COMColumnSFUpdateMethod.Rapid:
                    //for (int i = 0; i < gradmod.Length; i++)
                    //    gradmod[i] *= gradientfactor;
                    break;

                case COMColumnSFUpdateMethod.Excel:
                    for (int i = 0; i < gradmod.Length; i++)
                        gradmod[i] *= gradientfactor;
                    break;

                case COMColumnSFUpdateMethod.Modified:

                    if (double.IsNaN(gradientfactor) || Math.Abs(MaxGrad * JDelta) > 1)
                        for (int i = 0; i < gradmod.Length; i++)
                            gradmod[i] *= 1 / MaxGrad;  // JDelta is largest gradient
                    break;

                case COMColumnSFUpdateMethod.Simple:

                    for (int i = 0; i < gradmod.Length; i++)
                        gradmod[i] *= gradientfactor;
                    break;

                case COMColumnSFUpdateMethod.Delayed:
                    if (delaycount < 10)
                    {
                        gradientfactor = 0.1;
                        deltacount++;
                    }

                    for (int i = 0; i < gradmod.Length; i++)
                        gradmod[i] *= gradientfactor;
                    break;
            }

            double MINSF = Math.Log(MIND);
            double MAXSF = Math.Log(MAXD);

            // do tray stripping factors
            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];

                for (int trayno = 0; trayno < section.Trays.Count; trayno++)  // update strip factors
                {
                    TrayTest tray = section.Trays[trayno];
                    deltas[deltacount] = gradmod[deltacount] * JDelta * gradientfactor;

                    //  if (deltas[deltacount] < -1 || deltas[deltacount] > 1)
                    //      deltas[deltacount] = 0;

                    if (Math.Log(tray.StripFact) - deltas[deltacount] < MINSF)
                        tray.StripFact = Math.Exp(MINSF);
                    else if (Math.Log(tray.StripFact) - deltas[deltacount] > MAXSF)
                        tray.StripFact = Math.Exp(MAXSF);
                    else
                        tray.StripFact = Math.Exp(Math.Log(tray.StripFact) - deltas[deltacount]);  // Russel LOG form
                    {
                        //tray.StripFact = tray.StripFact - deltas[deltacount];  // Russel LOG form
                        // if (tray.StripFact < 0)
                        //     tray.StripFact = 1e-64;
                    }

                    deltacount++;
                }

                /*switch (column.TraySections[sectionNo].CondenserType)
                {
                    case  CondType.Subcooled:
                        section.TopTray.StripFact = 1e-12;
                        break;
                }*/
            }

            // Liquid Side Draws
            foreach (SideStreamTest ss in column.LiquidSideStreams)
            {
                //TraySection section = ss.EngineDrawSection;
                TrayTest tray = ss.EngineDrawTray;

                deltas[deltacount] = gradmod[deltacount] * JDelta * gradientfactor;

                if (tray != null)
                {
                    if (Math.Log(tray.LiqDrawFactor) - deltas[deltacount] < MINSF)
                        tray.LiqDrawFactor = Math.Exp(MINSF);
                    else if (Math.Log(tray.LiqDrawFactor) - deltas[deltacount] > MAXSF)
                        tray.LiqDrawFactor = Math.Exp(MAXSF);
                    else
                        tray.LiqDrawFactor = Math.Exp(Math.Log(tray.LiqDrawFactor) - deltas[deltacount]);
                }

                deltacount++;
            }

            // do connecting streams
            foreach (ConnectingStreamTest stream in column.LiquidStreams)
            {
                deltas[deltacount] = gradmod[deltacount] * JDelta * gradientfactor;

                if (Math.Log(stream.DrawFactor) - deltas[deltacount] < MINSF)
                    stream.DrawFactor = Math.Exp(MINSF);
                else if (Math.Log(stream.DrawFactor) - deltas[deltacount] > MAXSF)
                    stream.DrawFactor = Math.Exp(MAXSF);
                else
                    stream.DrawFactor = Math.Exp(Math.Log(stream.DrawFactor) - deltas[deltacount]);

                deltacount++;
            }

            foreach (ConnectingStreamTest stream in column.VapourStreams)
            {
                deltas[deltacount] = gradmod[deltacount] * JDelta * gradientfactor;

                if (Math.Log(stream.DrawFactor) - deltas[deltacount] < MINSF)
                    stream.DrawFactor = Math.Exp(MINSF);
                else if (Math.Log(stream.DrawFactor) - deltas[deltacount] > MAXSF)
                    stream.DrawFactor = Math.Exp(MAXSF);
                else
                    stream.DrawFactor = Math.Exp(Math.Log(stream.DrawFactor) - deltas[deltacount]);

                deltacount++;
            }

            // PA
            for (int i = 0; i < column.PumpArounds.Count; i++)
            {
                PumpAroundTest p = column.PumpArounds[i];

                deltas[deltacount] = gradmod[deltacount] * JDelta * gradientfactor;

                if (Math.Log(p.DrawFactor) - deltas[deltacount] < MINSF)
                    p.DrawFactor = Math.Exp(MINSF);
                else if (Math.Log(p.DrawFactor) - deltas[deltacount] > MAXSF)
                    p.DrawFactor = Math.Exp(MAXSF);
                else
                    p.DrawFactor = Math.Exp(Math.Log(p.DrawFactor) - deltas[deltacount]);  // Russel LOG form

                deltacount++;
            }

            return true;
        }

        public void Update_LSS_LFactor(COMColumn cd)
        {
            TraySectionTest section;
            TrayTest tray;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                section = cd.TraySections[sectionNo];
                int No = section.Trays.Count;
                for (int trayNo = 0; trayNo < No; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    tray = section.Trays[trayNo];
                    tray.lss_estimate = (tray.LiqDrawFactor) * tray.L;
                    if (tray.LiqCompositionPred != null && WaterLoc > -1)
                        tray.WaterEstimate = (tray.WaterDrawFactor) * (tray.LiqCompositionPred[WaterLoc] * tray.L);
                }
            }
        }

        public void Update_VSS_VFactor(COMColumn cd)
        {
            TraySectionTest section;
            TrayTest tray;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                section = cd.TraySections[sectionNo];
                int No = section.Trays.Count;

                for (int trayNo = 0; trayNo < No; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    tray = section.Trays[trayNo];
                    tray.vss_estimate = (tray.VapDrawFactor - 1) * tray.V;
                }
            }
        }

        public void InitTRDMatrix()
        {
            TrDMatrix = new double[NoComps][][];
            for (int n = 0; n < NoComps; n++)
            {
                TrDMatrix[n] = new double[TotNoTrays][];
                for (int m = 0; m < TotNoTrays; m++)
                    TrDMatrix[n][m] = new double[TotNoTrays];
            }
        }

        public void ClearTRD()
        {
           // int PaNO = column.PumpArounds.Pas.Count;
            for (int i = 0; i < PA_Count; i++)
            {
                PumpAroundTest p = this.column.PumpArounds.Pas[i];
                for (int comp = 0; comp < NoComps; comp++)
                {
                    double[][] TRD = TrDMatrix[comp];
                    if (p.ReturnTrayNo >= 0 && p.DrawTrayNo >= 0)
                    {
                        TRD[p.ReturnTrayNo][p.DrawTrayNo] = 0;
                        TRD[p.DrawTrayNo][p.DrawTrayNo] = 0;
                    }
                }
            }

            for (int comp = 0; comp < NoComps; comp++)
            {
                if (column.ConnectingDraws.Count > 0)
                {
                    // Do int erconecting Liquid Draws
                  //  int StreamCount = column.LiquidStreams.StreamList.Count;
                    for (int i = 0; i < LiquidStreamCount; i++)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                    {
                        ConnectingStreamTest stream = column.LiquidStreams.StreamList[i];
                        TraySectionTest dcts_Draw = stream.EngineDrawSection;
                        TraySectionTest dcts_Return = stream.EngineReturnSection;

                        if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                            return;

                        int drawsectionindex = dcts_Draw.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int returnsectionindex = stream.EngineReturnSection.SectionIndex;

                        int ReturnTraysSum = NoTrays[returnsectionindex];

                        if (drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] = 0;
                            TrDMatrix[comp][returntrayindex + ReturnTraysSum][drawtrayindex] = 0;
                        }
                    }
                }
            }

            /*  for (int  i = 0; i < TrDMatrix.Length; i++)
                  for (int  y = 0; y < TrDMatrix[i].Length; y++)
                      Array.Clear(TrDMatrix[i][y], 0, TrDMatrix[i][y].Length);*/
        }

        public bool SolveComponentBalance(COMColumn column)
        {
            int countstages, CurrentStage;
            double TrayT, TrayPlusOneT = 0;
            double waterfact;
            countstages = 0; // reset

            ClearTRD();

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;

                // Parallel.For(0, TrayCount, trayNo =>
                //{
                for (int trayNo = 0; trayNo < TrayCount; trayNo++)
                {
                    double k1 = 0, k = 0;
                    TrayTest tray = section.Trays[trayNo];
                    TrayTest trayp1 = null;

                    CurrentStage = countstages + trayNo;
                    TrayT = tray.T;

                    if (trayNo < section.Trays.Count - 1)
                    {
                        TrayPlusOneT = section.Trays[trayNo + 1].T;
                        trayp1 = section.Trays[trayNo + 1];
                    }

                    //Parallel.For(0, NoComps, comp =>
                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        double[][] TrdComp = TrDMatrix[comp];
                        double[] Stage = TrdComp[CurrentStage];

                        if (trayNo < section.Trays.Count - 1)
                        {
                            k1 = trayp1.KTray[comp];
                        }

                        k = tray.KTray[comp];
                        //if (k > 1e20 || k1 > 1e20)
                        //     Debugger.Break();

                        if (sectionNo == 0 && WaterCompNo == comp)
                            waterfact = 100000;
                        else
                            waterfact = 0;

                        if (trayNo == 0)
                        {
                            Stage[CurrentStage] = 1 + tray.LiqDrawFactor +
                                     tray.StripFact * k * (1 + tray.VapDrawFactor)
                                    + waterfact;

                            Stage[CurrentStage + 1] = -section.Trays[trayNo + 1].StripFact * k1;
                        }
                        else if (trayNo < section.Trays.Count - 1)
                        {
                            Stage[CurrentStage - 1] = -1;

                            Stage[CurrentStage] = 1 + tray.LiqDrawFactor +
                                 tray.StripFact * k * (1 + tray.VapDrawFactor);

                            Stage[CurrentStage + 1] = -section.Trays[trayNo + 1].StripFact * k1;
                        }
                        else
                        {
                            Stage[countstages + TrayCount - 2] = -1;
                            Stage[countstages + TrayCount - 1] = 1 + tray.LiqDrawFactor
                                    + tray.StripFact * k * (1 + tray.VapDrawFactor);
                        }
                    }
                }
                countstages += TrayCount;
            }

            //if (this.column is null)
            //    return false;
            // Do PumpArounds
            for (int i = 0; i < this.column.PumpArounds.Pas.Count; i++)
            {
                PumpAroundTest p = this.column.PumpArounds.Pas[i];
                for (int comp = 0; comp < NoComps; comp++)
                {
                    double[][] TRD = TrDMatrix[comp];
                    if (p.ReturnTrayNo >= 0 && p.DrawTrayNo >= 0)
                    {
                        TRD[p.ReturnTrayNo][p.DrawTrayNo] -= p.DrawFactor;
                        TRD[p.DrawTrayNo][p.DrawTrayNo] += p.DrawFactor;
                    }
                }
            }

            // Do int erconecting Liquid streams
            for (int comp = 0; comp < NoComps; comp++)
            {
                if (column.ConnectingDraws.Count > 0)
                {
                    // Do int erconecting Liquid Draws

                    foreach (ConnectingStreamTest stream in column.LiquidStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                    {
                        TraySectionTest dcts_Draw = stream.EngineDrawSection;
                        TraySectionTest dcts_Return = stream.EngineReturnSection;

                        if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                            return false;

                        int drawsectionindex = dcts_Draw.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int returnsectionindex = stream.EngineReturnSection.SectionIndex;

                        int ReturnTraysSum = NoTrays[returnsectionindex];

                        if (drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] += stream.DrawFactor;
                            TrDMatrix[comp][returntrayindex + ReturnTraysSum][drawtrayindex] -= stream.DrawFactor;
                        }
                    }

                    // Do interconecting Vapour Draws

                    for (int i = 0; i < column.VapourStreams.StreamList.Count; i++)  // do vapour draws
                    {
                        ConnectingStreamTest stream = column.VapourStreams.StreamList[i];
                        TraySectionTest Draw_Section = stream.EngineDrawSection;
                        TraySectionTest Return_Section = stream.EngineReturnSection;
                        TrayTest DrawSectionTray;

                        int drawsectionindex = Draw_Section.SectionIndex;
                        int returnsectionindex = Return_Section.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int DrawTraysSum = NoTrays[drawsectionindex];
                        int ReturnTraysSum = NoTrays[returnsectionindex];

                        //Specification spec = column.Specs.GetActiveSpecs().GetVapourDrawSpec(column, Draw_Section.Guid, drawtrayindex);
                        double VapDrawFact = stream.DrawFactor;

                        DrawSectionTray = Draw_Section.Trays[drawtrayindex];
                        //double  k = DrawSectionTray.K_TestFast(comp, stream.EngineDrawTray.T, column.solverOptions.KMethod);
                        double k = DrawSectionTray.KTray[comp];

                        //correct vapour draw
                        TrDMatrix[comp][drawtrayindex + DrawTraysSum - 1][drawtrayindex + DrawTraysSum]
                           = -DrawSectionTray.StripFact * k * (1 - VapDrawFact);

                        // Vapour Return
                        TrDMatrix[comp][returntrayindex + ReturnTraysSum][drawtrayindex + DrawTraysSum]
                           = -DrawSectionTray.StripFact * k * VapDrawFact;
                        //TrDMatrix[comp][][] = 0;
                    }
                }

                if (column.ConnectingNetFlows.Count > 0)
                {
                    // do liquid net flows
                    for (int i = 0; i < column.ConnectingNetFlows.LiquidStreams.StreamList.Count; i++)   // TrDMatrix[Comp][Down][Across]
                    {
                        ConnectingStreamTest stream = column.ConnectingNetFlows.LiquidStreams.StreamList[i];
                        TraySectionTest dcts_Draw = stream.EngineDrawSection;
                        TraySectionTest dcts_Return = stream.EngineReturnSection;

                        int drawsectionindex = dcts_Draw.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int returnsectionindex = stream.EngineReturnSection.SectionIndex;

                        int ReturnTraysSum = NoTrays[returnsectionindex];
                        int DrawTraysSum = NoTrays[drawsectionindex];

                        if (drawtrayindex == dcts_Draw.NoTrays - 1)
                        {
                            TrDMatrix[comp][drawtrayindex][DrawTraysSum + drawtrayindex] = -1;
                        }

                        /*if (drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] += stream.DrawFactor;
                            TrDMatrix[comp][returntrayindex + returnTraysSum][drawtrayindex] -= stream.DrawFactor;
                        }*/
                    }

                    // Do interconecting Vapour net flows
                    for (int i = 0; i < column.ConnectingNetVapourStreams.StreamList.Count; i++)  // do vapour draws
                    {
                        ConnectingStreamTest stream = column.ConnectingNetVapourStreams.StreamList[i];
                        TraySectionTest Draw_Section = stream.EngineDrawSection;
                        TraySectionTest Return_Section = stream.EngineReturnSection;
                        TrayTest drawtray = stream.engineDrawTray;

                        int drawsectionindex = Draw_Section.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int DrawTraysSum = NoTrays[drawsectionindex];

                        if (drawsectionindex >= 0 && drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][returntrayindex][drawtrayindex + DrawTraysSum] =
                                -drawtray.StripFact * drawtray.KTray[comp];// drawtray.K_TestFast(comp, stream.EngineDrawTray.T, column.solverOptions.KMethod);// Vapour return
                        }
                    }
                }
            }
            SolveCompMatrix();

            return true;
        }

        public void UpdatePredCompositions()
        {
            // Update Compositions
            int SumTrayCount = 0;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;

                for (int trayNo = 0; trayNo < TrayCount; trayNo++)
                {
                    TrayTest tray = section.Trays[trayNo];
                    double L = 0;
                    double V = 0;

                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        CompVapMolarFlows[SumTrayCount][comp] = CompLiqMolarFlows[SumTrayCount][comp] //* tray.K_TestFast(comp, tray.T, column.solverOptions.KMethod) * tray.StripFact;
                            * tray.KTray[comp] * tray.StripFact;
                    }

                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        V += CompVapMolarFlows[SumTrayCount][comp];
                        L += CompLiqMolarFlows[SumTrayCount][comp];
                    }

                    for (int nc = 0; nc < NoComps; nc++)
                    {
                        tray.LiqCompositionPred[nc] = CompLiqMolarFlows[SumTrayCount][nc] / L;
                        tray.VapCompositionPred[nc] = CompVapMolarFlows[SumTrayCount][nc] / V;
                    }

                    SumTrayCount++;
                }
            }
        }

        public void UpdateLandV()
        {
            // Update Compositions
            int SumTrayCount = 0;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;

                for (int trayNo = 0; trayNo < TrayCount; trayNo++)
                {
                    TrayTest tray = section.Trays[trayNo];
                    tray.L = 0;
                    tray.V = 0;

                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        tray.V += CompVapMolarFlows[SumTrayCount][comp];
                        tray.L += CompLiqMolarFlows[SumTrayCount][comp];
                    }

                    SumTrayCount++;
                }
            }
            if (WaterCompNo != null)
                WaterDraw = CompLiqMolarFlows[0][(int)WaterCompNo] * WaterDrawFactor;
        }


        private bool SolveCompMatrix()
        {
            if (column.ThomasAlgorithm && column.PumpArounds.Count == 0 && column.TraySections.Count == 1)
                Tridiag(column);
            else
                FullMatrixInversion(column);

            return true;
        }

        public bool SolveComponentBalanceNew(COMColumn column)
        {
            int countstages, CurrentStage;
            double TrayT, TrayPlusOneT = 0;
            double waterfact;
            countstages = 0; // reset

            ClearTRD();

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;

                // Parallel.For(0, TrayCount, trayNo =>
                //{
                for (int trayNo = 0; trayNo < TrayCount; trayNo++)
                {
                    double k1 = 0;
                    TrayTest tray = section.Trays[trayNo];
                    TrayTest trayp1 = null;

                    CurrentStage = countstages + trayNo;
                    TrayT = tray.T;

                    if (trayNo < section.Trays.Count - 1)
                    {
                        TrayPlusOneT = section.Trays[trayNo + 1].T;
                        trayp1 = section.Trays[trayNo + 1];
                    }

                    //Parallel.For(0, NoComps, comp =>
                    //{
                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        double[][] TrdComp = TrDMatrix[comp];
                        double[] Stage = TrdComp[CurrentStage];

                        if (trayNo < section.Trays.Count - 1)
                        {
                            //k1 = trayp1.K_TestFast(comp, TrayPlusOneT, column.solverOptions.KMethod);
                            k1 = trayp1.KTray[comp];
                            //if (Math.Abs((k2 - k1)/ k1) > 0.1)
                            //    Debugger.Break();
                            // double  res = trayp1.K_Test(comp, TrayPlusOneT, ColumnAlphaMethod.MA);
                            //  if (Math.Abs((k1 - res)/k1) > 0.1)
                            //    Debugger.Break();
                        }

                        // double  k = tray.K_TestFast(comp, TrayT, column.solverOptions.AlphaMethod);
                        double k = tray.KTray[comp];
                        //  if (Math.Abs((k3 - k) / k) > 0.1)
                        //    Debugger.Break();

                        if (sectionNo == 0 && WaterCompNo == comp)
                            waterfact = 100000;
                        else
                            waterfact = 0;

                        if (trayNo == 0)
                        {
                            Stage[CurrentStage] = 1 + tray.LiqDrawFactor +
                                     tray.StripFact * k * (1 + tray.VapDrawFactor)
                                    + waterfact;

                            Stage[CurrentStage + 1] = -section.Trays[trayNo + 1].StripFact * k1;

                            //var res = tray.K_Test(column.Components, comp, TrayTemp, alphaMethod);
                        }
                        else if (trayNo < section.Trays.Count - 1)
                        {
                            Stage[CurrentStage - 1] = -1;

                            Stage[CurrentStage] = 1 + tray.LiqDrawFactor +
                                 tray.StripFact * k * (1 + tray.VapDrawFactor);

                            Stage[CurrentStage + 1] = -section.Trays[trayNo + 1].StripFact * k1;
                        }
                        else
                        {
                            Stage[countstages + TrayCount - 2] = -1;
                            Stage[countstages + TrayCount - 1] = 1 + tray.LiqDrawFactor
                                    + tray.StripFact * k * (1 + tray.VapDrawFactor);
                        }
                    }
                }
                countstages += TrayCount;
            }

            if (this.column is null)
                return false;
            // Do PumpArounds
            foreach (PumpAroundTest p in this.column.PumpArounds)
            {
                for (int comp = 0; comp < NoComps; comp++)
                {
                    double[][] TRD = TrDMatrix[comp];
                    if (p.ReturnTrayNo >= 0 && p.DrawTrayNo >= 0)
                    {
                        TRD[p.ReturnTrayNo][p.DrawTrayNo] -= p.DrawFactor;
                        TRD[p.DrawTrayNo][p.DrawTrayNo] += p.DrawFactor;
                    }
                }
            }

            // Do int erconecting Liquid streams
            for (int comp = 0; comp < NoComps; comp++)
            {
                if (column.ConnectingDraws.Count > 0)
                {
                    // Do int erconecting Liquid Draws

                    foreach (ConnectingStreamTest stream in column.LiquidStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                    {
                        TraySectionTest dcts_Draw = stream.EngineDrawSection;
                        TraySectionTest dcts_Return = stream.EngineReturnSection;

                        if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                            return false;

                        int drawsectionindex = dcts_Draw.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int returnsectionindex = stream.EngineReturnSection.SectionIndex;

                        int ReturnTraysSum = NoTrays[returnsectionindex];

                        if (drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] += stream.DrawFactor;
                            TrDMatrix[comp][returntrayindex + ReturnTraysSum][drawtrayindex] -= stream.DrawFactor;
                        }
                    }

                    // Do int erconecting Vapour Draws

                    foreach (ConnectingStreamTest stream in column.VapourStreams)  // do vapour draws
                    {
                        TraySectionTest Draw_Section = stream.EngineDrawSection;
                        TraySectionTest Return_Section = stream.EngineReturnSection;
                        TrayTest DrawSectionTray;

                        int drawsectionindex = Draw_Section.SectionIndex;
                        int returnsectionindex = Return_Section.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int DrawTraysSum = NoTrays[drawsectionindex];
                        int ReturnTraysSum = NoTrays[returnsectionindex];

                        //Specification spec = column.Specs.GetActiveSpecs().GetVapourDrawSpec(column, Draw_Section.Guid, drawtrayindex);
                        double VapDrawFact = stream.DrawFactor;

                        DrawSectionTray = Draw_Section.Trays[drawtrayindex];
                        //double  k = DrawSectionTray.K_Test(comp, stream.EngineDrawTray.T, column.solverOptions.AlphaMethod);
                        double k = DrawSectionTray.KTray[comp];

                        //correct vapour draw
                        TrDMatrix[comp][drawtrayindex + DrawTraysSum - 1][drawtrayindex + DrawTraysSum]
                           = -DrawSectionTray.StripFact * k * (1 - VapDrawFact);

                        // Vapour Return
                        TrDMatrix[comp][returntrayindex + ReturnTraysSum][drawtrayindex + DrawTraysSum]
                           = -DrawSectionTray.StripFact * k * VapDrawFact;
                        //TrDMatrix[comp][][] = 0;
                    }
                }

                if (column.ConnectingNetFlows.Count > 0)
                {
                    // do liquid net flows
                    foreach (ConnectingStreamTest stream in column.ConnectingNetFlows.LiquidStreams)   // TrDMatrix[Comp][Down][Across]
                    {
                        TraySectionTest dcts_Draw = stream.EngineDrawSection;
                        TraySectionTest dcts_Return = stream.EngineReturnSection;

                        int drawsectionindex = dcts_Draw.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int returnsectionindex = stream.EngineReturnSection.SectionIndex;

                        int ReturnTraysSum = NoTrays[returnsectionindex];
                        int DrawTraysSum = NoTrays[drawsectionindex];

                        if (drawtrayindex == dcts_Draw.NoTrays - 1)
                        {
                            TrDMatrix[comp][drawtrayindex][DrawTraysSum + drawtrayindex] = -1;
                        }

                        /*if (drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] += stream.DrawFactor;
                            TrDMatrix[comp][returntrayindex + returnTraysSum][drawtrayindex] -= stream.DrawFactor;
                        }*/
                    }

                    // Do int erconecting Vapour net flows
                    foreach (ConnectingStreamTest stream in column.ConnectingNetVapourStreams)  // do vapour draws
                    {
                        TraySectionTest Draw_Section = stream.EngineDrawSection;
                        TraySectionTest Return_Section = stream.EngineReturnSection;
                        TrayTest drawtray = stream.engineDrawTray;

                        int drawsectionindex = Draw_Section.SectionIndex;
                        int drawtrayindex = stream.engineDrawTray.TrayIndex;
                        int returntrayindex = stream.engineReturnTray.TrayIndex;
                        int DrawTraysSum = NoTrays[drawsectionindex];

                        if (drawsectionindex >= 0 && drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            double k = drawtray.KTray[comp];
                            TrDMatrix[comp][returntrayindex][drawtrayindex + DrawTraysSum] = -drawtray.StripFact * k;// Vapour return
                            //TrDMatrix[comp][returntrayindex][drawtrayindex + DrawTraysSum] = -drawtray.StripFact * drawtray.K_Test(comp, stream.EngineDrawTray.T, column.solverOptions.AlphaMethod);// Vapour return
                        }
                    }
                }
            }

            if (column.ThomasAlgorithm && column.PumpArounds.Count == 0 && column.TraySections.Count == 1)
                Tridiag(column);
            else
                FullMatrixInversion(column);

            // Update Compositions
            int SumTrayCount = 0;

            for (int sectionNo = 0; sectionNo < column.TraySections.Count; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];

                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                {
                    TrayTest tray = section.Trays[trayNo];
                    tray.L = 0;
                    tray.V = 0;

                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        double k = tray.KTray[comp];
                        CompVapMolarFlows[SumTrayCount][comp] = CompLiqMolarFlows[SumTrayCount][comp]
                            * k * tray.StripFact;

                        //CompVapMolarFlows[SumTrayCount][comp] = CompLiqMolarFlows[SumTrayCount][comp]
                        //   * tray.K_Test(comp, tray.T, column.solverOptions.AlphaMethod) * tray.StripFact;

                        tray.V += CompVapMolarFlows[SumTrayCount][comp];
                    }

                    for (int comp = 0; comp < NoComps; comp++)
                        tray.L += CompLiqMolarFlows[SumTrayCount][comp];

                    for (int nc = 0; nc < NoComps; nc++)
                    {
                        tray.LiqCompositionPred[nc] = CompLiqMolarFlows[SumTrayCount][nc] / tray.L;
                        tray.VapCompositionPred[nc] = CompVapMolarFlows[SumTrayCount][nc] / tray.V;
                    }

                    SumTrayCount++;

                    if (tray.LiqComposition.Contains(double.NaN))
                        return false;
                }
            }
            if (WaterCompNo != null)
                WaterDraw = CompLiqMolarFlows[0][(int)WaterCompNo] * WaterDrawFactor;

            return true;
        }

        public void DoCondenserCalcs(COMColumn column)
        {
            double CondT;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                TrayTest tray = section.Trays[0];
                CondT = tray.T;

                switch (section.CondenserType)
                {
                    case COMCondType.Partial:
                        break;

                    case COMCondType.Subcooled:
                        tray.StripFact = 0.00000000001;  // cant have ln of zero!
                        tray.VapDrawFactor = 1;
                        column.Subcoolduty = tray.LiqEnthalpy(column, CondT) - tray.LiqEnthalpy(column, CondT - column.SubcoolDT);
                        break;

                    case COMCondType.TotalReflux:
                        tray.StripFact = 0.00000000001;  // cant have ln of zero!
                        tray.VapDrawFactor = 1;
                        break;
                }
            }
        }

        public void UpdateTrayEnthalpyBalances()  // Inside Loop // Should be non-rigourous inside loop.
        {
            double[][] EnthAdjustments = new double[NoSections][];
            int IterCount = 0;
            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                EnthAdjustments[sectionNo] = new double[section.Trays.Count];

                int StartTray = 0;
                int EndTray = section.Trays.Count;

                double LiquidFeedEnthalpy;
                double VapourFeedEnthalpy;

                for (int trayNo = StartTray; trayNo < EndTray; trayNo++)
                {
                    TrayTest tray = section.Trays[trayNo];

                    //tray.feed.Components.CalcVapour(T);
                    // tray.feed.Components.CalcLiquid(T);

                    if (double.IsNaN(tray.FeedEnthalpyLiq))
                        LiquidFeedEnthalpy = 0;
                    else
                        LiquidFeedEnthalpy = tray.FeedEnthalpyLiq;

                    if (double.IsNaN(tray.FeedEnthalpyVap))
                        VapourFeedEnthalpy = 0;
                    else
                        VapourFeedEnthalpy = tray.FeedEnthalpyVap;

                    if (trayNo == StartTray) // TopTray
                    {
                        if (section.CondenserType == COMCondType.Subcooled)
                            tray.liqenth -= column.Subcoolduty;

                        tray.enthalpyBalance =
                            -tray.liqenth * tray.L                                  // Liquid Leaving Tray
                            + section.Trays[trayNo + 1].vapenth * section.Trays[trayNo + 1].V                         // Vapour Entering Tray
                            - tray.vapenth * tray.V                                 // Vapour Leaving Tray
                            - tray.liqenth * tray.lss_estimate;                     // Side Liquid Draw
                    }
                    else if (trayNo < EndTray - 1) // Middle Trays
                    {
                        tray.enthalpyBalance =
                            section.Trays[trayNo - 1].liqenth * section.Trays[trayNo - 1].L        // Liquid entering tray
                            - tray.liqenth * tray.L              // Liquid Leaving Tray
                            + section.Trays[trayNo + 1].vapenth * section.Trays[trayNo + 1].V      // Vapour Entering Tray
                            - tray.vapenth * tray.V              // Vapour Leaving Tray
                            - tray.liqenth * tray.lss_estimate;                // Side Liquid Draw;
                    }
                    else // Bottom Tray
                    {
                        tray.enthalpyBalance =
                              section.Trays[trayNo - 1].liqenth * section.Trays[trayNo - 1].L       // Liquid entering tray
                            - tray.liqenth * tray.L               // Liquid Leaving Tray
                            - tray.vapenth * tray.V               // Vapour Entering Tray
                            - tray.liqenth * tray.lss_estimate;                 // Side Liquid Draw;
                    }

                    if (tray.feed.IsFlashed)
                    {
                        if (!double.IsNaN(tray.feed.LiquidMoleFlow))
                            tray.enthalpyBalance += LiquidFeedEnthalpy * tray.feed.LiquidMoleFlow * column.feedRatio;      // Feed onto tray
                        if (!double.IsNaN(tray.feed.VapourMoleFlow))
                            tray.enthalpyBalance += VapourFeedEnthalpy * tray.feed.VapourMoleFlow * column.feedRatio;      // Feed onto tray
                    }
                }   // Must include stripper feed vapour
            }

            IterCount = column.PumpArounds.Pas.Count;
            for (int i = 0; i < IterCount; i++)
            {
                PumpAroundTest p = column.PumpArounds.Pas[i];
                int drawtray = p.DrawTrayNo;
                int returntray = p.ReturnTrayNo;
                int pasection = column.TraySections.IndexOf(p.drawSection);

                if (pasection >= 0 && drawtray >= 0 && pasection < column.TraySections.Count)
                {
                    EnthAdjustments[pasection][drawtray] -= p.drawTray.liqenth * p.MoleFlow;

                    SpecificationTest s = column.Specs.GetActiveSpecs().GetPAEnergySpec(p.Guid);
                    if (s is null)
                        MessageBox.Show("No PA Energy Spec Set");
                    else
                    {
                        //pa.ReturnEnthalpy = pa.drawTray.PA_liqenth(column, s);
                        //pa.ReturnEnthalpy = pa.ReturnEnthalpyCalc(column, s);
                        EnthAdjustments[pasection][returntray] += p.ReturnEnthalpyCalc(column, s) * p.MoleFlow;
                    }
                }
            }

            IterCount = column.LiquidStreams.StreamList.Count;
            for (int i = 0; i < IterCount; i++)
            {
                ConnectingStreamTest stream = column.LiquidStreams.StreamList[i];
                TraySectionTest dcts_draw = stream.EngineDrawSection;
                TraySectionTest dcts_ret = stream.EngineReturnSection;

                int drawsection = dcts_draw.SectionIndex;
                int returnsection = dcts_ret.SectionIndex;

                if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                    return;

                int drawtray = stream.engineDrawTray.TrayIndex;
                int returntray = stream.engineReturnTray.TrayIndex;

                if (drawtray >= 0 && returntray >= 0)
                {
                    EnthAdjustments[drawsection][drawtray] -= dcts_draw.Trays[drawtray].liqenth * stream.FlowEstimate;
                    EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].liqenth * stream.FlowEstimate;
                }
            }

            IterCount = column.VapourStreams.StreamList.Count;
            for (int i = 0; i < IterCount; i++)
            {
                ConnectingStreamTest st = column.VapourStreams.StreamList[i];
                TraySectionTest dcts_draw = st.EngineDrawSection;
                TraySectionTest  dcts_ret = st.EngineReturnSection;

                int drawsection = dcts_draw.SectionIndex;
                int returnsection = dcts_ret.SectionIndex;

                int drawtray = st.engineDrawTray.TrayIndex;
                int returntray = st.engineReturnTray.TrayIndex;

                EnthAdjustments[drawsection][drawtray] -= dcts_draw.Trays[drawtray].vapenth * st.FlowEstimate;
                EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].vapenth * st.FlowEstimate;
            }

            IterCount = column.ConnectingNetLiquidStreams.StreamList.Count;
            for (int i = 0; i < IterCount; i++)
            {
                ConnectingStreamTest st = column.ConnectingNetLiquidStreams.StreamList[i];
                TraySectionTest  dcts_draw = st.EngineDrawSection;
                TraySectionTest dcts_ret = st.EngineReturnSection;

                int returnsection = dcts_ret.SectionIndex;

                int drawtray = st.engineDrawTray.TrayIndex;
                int returntray = st.engineReturnTray.TrayIndex;

                // Net flows allready in heat balance

                if (returnsection >= 0 && returntray >= 0)
                    EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].liqenth * st.engineDrawTray.L;
            }

            IterCount = column.ConnectingNetVapourStreams.StreamList.Count;
            for (int i = 0; i < IterCount; i++)
            {
                ConnectingStreamTest st = column.ConnectingNetVapourStreams.StreamList[i];
                TraySectionTest dcts_draw = st.EngineDrawSection;
                TraySectionTest dcts_ret = st.EngineReturnSection;

                int returnsection = dcts_ret.SectionIndex;

                int drawtray = st.engineDrawTray.TrayIndex;
                int returntray = st.engineReturnTray.TrayIndex;

                if (returntray >= 0)
                    // Net flows allready in heat balance
                    // EnthAdjustments[drawsection][drawtray] -= dcts_draw[drawtray].vapenth * st.drawTray.V;
                    EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].vapenth * st.engineDrawTray.V;
            }

            for (int i = 0; i < NoSections; i++)
            {
                TraySectionTest section = column.TraySections[i];
                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                    section.Trays[trayNo].enthalpyBalance += EnthAdjustments[i][trayNo];
            }

            return;
        }

        public bool CreateJacobianSF(COMColumn column, ref enumCalcResult cres) // strip factor
        {
            column.TraySections.BackupFactors(column);
            int ColumCount = 0;
            int startcount = 0;
            //ViewArray va = new ();

            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];
                for (int trayNo = startcount; trayNo < section.Trays.Count; trayNo++)  // Do strip factors for main tray section + Stripper sections
                {
                    section.Trays[trayNo].StripFact = Math.Exp(Math.Log(section.Trays[trayNo].StripFact) + JDelta);
                    
                    SolveComponentBalance(column);   //  TRD Matrix changed

                    UpdatePredCompositions();
                    UpdateLandV();

                    //ViewArray va = new ();
                    //va.View(TrDMatrix[0]);
                    Update_FlowFactors(column);
                    EstimateTs(column, column.solverOptions.KMethod);
                    EnthalpiesUpdate(ref cres);
                    UpdateTrayEnthalpyBalances();
                    CalcErrors(false);
                    ProcessActiveSpecs(out Errors, false);
                    // if(Errors.Sum()==0)
                    //   Debugger.Break();
                    UpdateJacobian(ColumCount, Errors);
                    column.TraySections.UnBackupFactor(column);
                    ColumCount++;
                    if (cres == enumCalcResult.Failed)
                        return false;
                }
            }

            // Liquid Sidestreams
            foreach (SideStreamTest stream in column.LiquidSideStreams)
            {
                if (stream.EngineDrawTray != null)
                {
                    TraySectionTest section = stream.EngineDrawSection;
                    stream.EngineDrawTray.LiqDrawFactor = Math.Exp(Math.Log(stream.EngineDrawTray.LiqDrawFactor) + JDelta);

                    SolveComponentBalance(column);

                    UpdatePredCompositions();
                    UpdateLandV();


                    Update_FlowFactors(column);
                    EstimateTs(column, column.solverOptions.KMethod);
                    EnthalpiesUpdate(ref cres);
                    UpdateTrayEnthalpyBalances();
                    CalcErrors(false);
                    ProcessActiveSpecs(out Errors, false);
                    UpdateJacobian(ColumCount, Errors);
                    column.TraySections.UnBackupFactor(column);
                    ColumCount++;
                }
            }

            // Vapour Sidestreams
            foreach (SideStreamTest ss in column.VapourSideStreams)
            {
                TraySectionTest section = ss.EngineDrawSection;
                ss.EngineDrawTray.VapDrawFactor = Math.Exp(Math.Log(ss.EngineDrawTray.VapDrawFactor) + JDelta);

                SolveComponentBalance(column);

                UpdatePredCompositions();
                UpdateLandV();


                Update_FlowFactors(column);
                EstimateTs(column, column.solverOptions.KMethod);
                EnthalpiesUpdate(ref cres);
                UpdateTrayEnthalpyBalances();
                CalcErrors(false);
                ProcessActiveSpecs(out Errors, false);
                UpdateJacobian(ColumCount, Errors);
                column.TraySections.UnBackupFactor(column);
                ColumCount++;
            }

            // do strippers etc
            // Purtub StripDrawFactors
            // stripper is on a product leaving a tray section
            foreach (ConnectingStreamTest st in column.LiquidStreams)
            {
                st.DrawFactor = Math.Exp(Math.Log(st.DrawFactor) + JDelta);

                SolveComponentBalance(column);

                UpdatePredCompositions();
                UpdateLandV();

                EstimateTs(column, column.solverOptions.KMethod);
                Update_FlowFactors(column);
                EnthalpiesUpdate(ref cres);
                UpdateTrayEnthalpyBalances();
                CalcErrors(false);
                ProcessActiveSpecs(out Errors, false);
                UpdateJacobian(ColumCount, Errors);
                column.TraySections.UnBackupFactor(column);
                ColumCount++;
            }

            foreach (ConnectingStreamTest st in column.VapourStreams)
            {
                st.DrawFactor = Math.Exp(Math.Log(st.DrawFactor) + JDelta);

                SolveComponentBalance(column);

                UpdatePredCompositions();
                UpdateLandV();

                EstimateTs(column, column.solverOptions.KMethod);
                Update_FlowFactors(column);
                EnthalpiesUpdate(ref cres);
                UpdateTrayEnthalpyBalances();
                CalcErrors(false);
                ProcessActiveSpecs(out Errors, false);
                UpdateJacobian(ColumCount, Errors);
                column.TraySections.UnBackupFactor(column);
                ColumCount++;
            }

            // Purtub PA Factor 1 // e.g. flowrate

            for (int i = 0; i < column.PumpArounds.Count; i++)
            {
                PumpAroundTest p = column.PumpArounds[i];
                // SpecificationCollectionTest PAspecs = column.Specs.GetActivePASpecs(p.Guid);

                if (p != null)
                {
                    p.DrawFactor = Math.Exp(Math.Log(p.DrawFactor) + JDelta);

                    SolveComponentBalance(column);
 
                    UpdatePredCompositions();
                    UpdateLandV();

                    EstimateTs(column, column.solverOptions.KMethod);
                    Update_FlowFactors(column);
                    EnthalpiesUpdate(ref cres);
                    UpdateTrayEnthalpyBalances();
                    CalcErrors(false);
                    ProcessActiveSpecs(out Errors, false);
                    UpdateJacobian(ColumCount, Errors);
                    column.TraySections.UnBackupFactor(column);
                    ColumCount++;
                }
            }
            return true;
        }

        public bool UpdateJacobian(int column, double[] err)
        {
            int counter = 0;

            for (int row = 0; row < err.Length; row++)
            {
                if (counter >= JacobianSize || column >= JacobianSize)
                    return false;
                else
                {
                    Jacobian[counter, column] = err[row];
                    counter++;
                }
            }
            return true;
        }

        public static bool SolveJacobian(double[,] Jacobian, double[] errors, ref double[] grads, int Size)
        {
            COMenumInverseMethod method = COMenumInverseMethod.Alglib;

            if (errors != null)
            {
                switch (method)
                {
                    /*  case  enumInverseMethod.GeneralMatrix:
                          {
                              GeneralMatrix A = new (Jacobian);
                              GeneralMatrix B = new (errors, errors.Length);
                              //A = A.Inverse();

                              if (A != null)
                                  A = A.Solve(B);
                              else
                                  return false;

                              grads = A.Transpose().Array[0];
                              return true;
                          }*/

                    case COMenumInverseMethod.Alglib:
                        {
                            //ViewArray va = new ();
                            //va.View(Jacobian);
                            alglib.densesolverreport rep;
                            int info;
                            alglib.rmatrixsolve(Jacobian, Size, errors, out info, out rep, out grads);
                            return true;
                        }

                        /*  case  enumInverseMethod.MathNet:
                              var A2 = Matrix<double >.Build.DenseOfRowArrays(Jacobian);
                              var B2 = Vector<double >.Build.DenseOfArray(errors);
                              try
                              {
                                  grads = A2.Solve(B2).ToArray();
                              }
                              catch { return false; }
                              return true;*/
                }
            }
            return false;
        }

        public int NoTraysToThisSection(int sectioNo)
        {
            int sum = 0;
            for (int i = 0; i < sectioNo; i++)
                sum += column.TraySections[i].Trays.Count;

            return sum;
        }

        public double[] GetRHS()
        {
            double ErrorSum = 0;
            enumCalcResult cres = enumCalcResult.Failed;
            UpdateColumnBalance(ref ErrorSum, cres);  //Enth
            return Errors;
        }

        public bool UpdateFactorsOld(double[] gradients, double gradientfactor = 1)
        {
            double MINSF = Math.Log(0.00001);
            double MAXSF = Math.Log(10);

            if (gradients is null)
                return false;

            int deltacount = 0;

            // do tray stripping factors
            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySectionTest section = column.TraySections[sectionNo];

                for (int trayno = 0; trayno < section.Trays.Count; trayno++)  // update strip factors
                {
                    TrayTest tray = section.Trays[trayno];
                    deltas[deltacount] = gradients[deltacount] * JDelta * gradientfactor;

                    if (deltas[deltacount] < MINSF)
                        deltas[deltacount] = MINSF;
                    else if (deltas[deltacount] > MAXSF)
                        deltas[deltacount] = MAXSF;

                    section.Trays[trayno].StripFact = Math.Exp(Math.Log(section.Trays[trayno].StripFact) - deltas[deltacount]);  // Russel LOG form

                    if (tray.StripFact > 30)
                        tray.StripFact = 30;
                    else if (tray.StripFact < 0.00001)
                        tray.StripFact = 0.00001;

                    deltacount++;
                }

                /*switch (column.TraySections[sectionNo].CondenserType)
                {
                    case  CondType.Subcooled:
                        section.TopTray.StripFact = 1e-12;
                        break;
                }*/
            }

            // Liquid Side Draws
            foreach (SideStreamTest ss in column.LiquidSideStreams)
            {
                TraySectionTest section = ss.EngineDrawSection;

                deltas[deltacount] = gradients[deltacount] * JDelta * gradientfactor;

                if (deltas[deltacount] < MINSF)
                    deltas[deltacount] = MINSF;
                else if (deltas[deltacount] > MAXSF)
                    deltas[deltacount] = MAXSF;

                if (ss.EngineDrawTray != null)
                    ss.EngineDrawTray.LiqDrawFactor = Math.Exp(Math.Log(ss.EngineDrawTray.LiqDrawFactor) - deltas[deltacount]);

                deltacount++;
            }

            // do connecting streams
            foreach (ConnectingStreamTest stream in column.LiquidStreams)
            {
                TraySectionTest drawsection = stream.EngineDrawSection;
                int section = drawsection.SectionIndex;

                deltas[deltacount] = gradients[deltacount] * JDelta * gradientfactor;

                if (deltas[deltacount] < MINSF)
                    deltas[deltacount] = MINSF;
                else if (deltas[deltacount] > MAXSF)
                    deltas[deltacount] = MAXSF;

                stream.DrawFactor = Math.Exp(Math.Log(stream.DrawFactor) - deltas[deltacount]);
                deltacount++;
            }

            foreach (ConnectingStreamTest stream in column.VapourStreams)
            {
                TraySectionTest drawsection = stream.EngineDrawSection;
                int section = drawsection.SectionIndex;

                deltas[deltacount] = gradients[deltacount] * JDelta * gradientfactor;

                if (deltas[deltacount] < MINSF)
                    deltas[deltacount] = MINSF;
                else if (deltas[deltacount] > MAXSF)
                    deltas[deltacount] = MAXSF;

                stream.DrawFactor = Math.Exp(Math.Log(stream.DrawFactor) - deltas[deltacount]);
                deltacount++;
            }

            // PA
            for (int i = 0; i < column.PumpArounds.Count; i++)
            {
                PumpAroundTest p = column.PumpArounds[i];

                deltas[deltacount] = gradients[deltacount] * JDelta * gradientfactor;

                if (deltas[deltacount] < MINSF)
                    deltas[deltacount] = MINSF;
                else if (deltas[deltacount] > MAXSF)
                    deltas[deltacount] = MAXSF;

                p.DrawFactor -= deltas[deltacount];
                deltacount++;
            }

            return true;
        }
    }
}