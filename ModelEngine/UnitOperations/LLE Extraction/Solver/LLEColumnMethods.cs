using DotNetMatrix;
using Extensions;
using MathNet.Numerics.LinearAlgebra;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace LLE_Solver
{
    internal enum FeedSplitType
    { SimpleFeedModel, AssumeAllLiquid };

    public partial class LLESolver
    {
        private Port_Material MainFeed;
        private readonly FeedSplitType feedsplittype = FeedSplitType.AssumeAllLiquid;

        public void Finishoff()
        {
            if (column.MainTraySection.CondenserType == CondType.Subcooled)
                column.MainTraySection.Trays[0].T -= column.SubcoolDT;

            column.Waterdraw = WaterDraw;
        }

        internal void ProcessActiveSpecs(out double[] errors, bool isbase)
        {
            List<double> errorList = new();
            SpecificationCollection ActiveSpecs = column.Specs.GetActiveSpecs();

            foreach (Specification spec in ActiveSpecs)
            {
                if (isbase)
                {
                    switch (spec.engineSpecType)
                    {
                        case eSpecType.LLE_KSpec:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.TrayNetVapFlow:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.Temperature:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.TrayDuty:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.RefluxRatio:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.LiquidProductDraw:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.VapProductDraw:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.TrayNetLiqFlow:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.PAFlow:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.Energy:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.LiquidStream:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.VapStream:
                            errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.PARetT:
                            //errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.PADeltaT:
                            //errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.PADuty:
                            //errorList.Add(spec.baseerror);
                            break;

                        case eSpecType.DistSpec:
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
                        case eSpecType.LLE_KSpec:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.TrayNetVapFlow:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.Temperature:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.TrayDuty:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.RefluxRatio:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.LiquidProductDraw:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.VapProductDraw:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.TrayNetLiqFlow:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.PAFlow:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.Energy:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.LiquidStream:
                        case eSpecType.VapStream:
                            errorList.Add(spec.error);
                            break;

                        case eSpecType.PARetT:
                            //errorList.Add(spec.error);
                            break;

                        case eSpecType.PADeltaT:
                            // errorList.Add(spec.error);
                            break;

                        case eSpecType.PADuty:
                            //errorList.Add(spec.error);
                            break;

                        case eSpecType.DistSpec:
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
                TraySection section = column.TraySections[sectionNo];

                for (int n = 0; n < section.Trays.Count; n++)
                {
                    Tray tray = section.Trays[n];
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

        public void SetInitialTrayTemps(LLESEP cd)
        {
            Port_Material feed = cd.TraySections.GetLargestFeedStream();
            if (feed != null && column != null)
            {
                double t = feed.T_;

                for (int sectionNo = 0; sectionNo < cd.TraySections.Count; sectionNo++)
                    for (int trayNo = 0; trayNo < column.TraySections[sectionNo].Trays.Count; trayNo++)
                        column.TraySections[sectionNo].Trays[trayNo].T = new Temperature(t);
            }
        }

        public void InitialiseMatrices(LLESEP column)
        {
            JacobianSize = column.JacobianSize;

            A = new double[NoSections][];
            B = new double[NoSections][];

            KBase = new double[NoSections][];
            Errors = new double[JacobianSize];
            deltas = new double[JacobianSize];
            PAreturnEnthalpy = new double[NoSections][];
            StripperLiqFeedEnthalpies = new double[NoSections][];
            StripperVapreturnTotEnthalpies = new double[NoSections][];
            // Stripperreturn  s = new  double [NoSections][];
            TempTrayTempK = new double[NoSections][];

            TrDMatrix = new double[NoComps][][];
            for (int n = 0; n < NoComps; n++)
            {
                TrDMatrix[n] = new double[TotNoTrays][];
                for (int m = 0; m < TotNoTrays; m++)
                    TrDMatrix[n][m] = new double[TotNoTrays];
            }

            TrDInverse = new double[NoComps][][];
            for (int n = 0; n < NoComps; n++)
            {
                TrDInverse[n] = new double[TotNoTrays][];
                for (int m = 0; m < TotNoTrays; m++)
                    TrDInverse[n][m] = new double[TotNoTrays];
            }

            CompLiqMolarFlows = new double[TotNoTrays][];
            CompVapMolarFlows = new double[TotNoTrays][];

            interSectionLiquidDraws = new double[NoSections][];
            interSectionLiquidFeeds = new double[NoSections][];
            interSectionVapourFeeds = new double[NoSections][];
            interSectionVapourDraws = new double[NoSections][];

            if (this.column != null)
            {
                for (int i = 0; i < NoSections; i++)
                {
                    interSectionLiquidDraws[i] = new double[this.column.TraySections[i].Trays.Count];
                    interSectionLiquidFeeds[i] = new double[this.column.TraySections[i].Trays.Count];
                    interSectionVapourDraws[i] = new double[this.column.TraySections[i].Trays.Count];
                    interSectionVapourFeeds[i] = new double[this.column.TraySections[i].Trays.Count];
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
                    TraySection section = this.column.TraySections[sect];

                    int notrays = column.TraySections[sect].Trays.Count;
                    PAreturnEnthalpy[sect] = new double[notrays];
                    StripperLiqFeedEnthalpies[sect] = new double[notrays];
                    StripperVapreturnTotEnthalpies[sect] = new double[notrays];

                    //Stripperreturn  s[sect] = new  double [notrays];

                    B[sect] = new double[notrays];
                    A[sect] = new double[notrays];

                    for (int n = 0; n < section.Trays.Count; n++)
                    {
                        Tray tray = section.Trays[n];
                        //section[n].TrayVapComposition = new  double [NoComps];
                        //section[n].TrayCompositionVap = new  double [NoComps];
                        tray.LiqComposition = new double[NoComps];
                        tray.VapComposition = new double[NoComps];
                        tray.LiqCompositionPred = new double[NoComps];
                        tray.VapCompositionPred = new double[NoComps];
                        tray.LiqCompositionInitial = new double[NoComps];
                        tray.VapCompositionInitial = new double[NoComps];
                        //section[n].lnKAvg

                        //lnAlpha[sect][n] = new  double [NoComps];
                    }
                }
            }

            Jacobian = new double[JacobianSize][];
            for (int n = 0; n < JacobianSize; n++)
                Jacobian[n] = new double[JacobianSize];
        }

        public bool interpolatePressures()
        {
            if (this.column != null)
                foreach (TraySection section in column.TraySections)
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
                            section.Trays[i].P.BaseValue = topp + deltaP;
                    }
                }

            return true;
        }

        internal bool InitialiseFeeds(LLESEP column)
        {
            TotalFeeds = 0;
            PortList feedstreams = new();

            foreach (TraySection section in column.TraySections)
            {
                for (int TrayNo = 0; TrayNo < section.Trays.Count; TrayNo++) // do main column first
                {
                    Tray tray = section.Trays[TrayNo];
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

            MainFeed = feedstreams.GetAVGComposition();// feedstreams.GetLargestStream();

            if (MainFeed is null)
                return false;

            column.FeedRatio = 1;// / MainFeed.MolarFlow;

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
                    TraySection section = column.TraySections[sectionNo];
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

        public void InitialiseTrayCompositions(LLESEP column)
        {
            if (MainFeed == null)
                return;

            if (column.SolutionConverged)
                for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
                {
                    TraySection section = column.TraySections[sectionNo];
                    for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                    {
                        section.Trays[trayNo].LiqCompositionInitial = (double[])section.Trays[trayNo].LiqComposition.Clone();
                        section.Trays[trayNo].VapCompositionInitial = (double[])section.Trays[trayNo].VapComposition.Clone();
                    }
                }
            else
                for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
                {
                    TraySection section = column.TraySections[sectionNo];

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

        public static void InitialisePumpArounds(LLESEP column)
        {
            foreach (PumpAround pa in column.PumpArounds)
            {
                foreach (Specification s in column.Specs.GetSpecs(pa))
                {
                    TraySection section = column.TraySections[s.engineSectionGuid];

                    switch (s.engineSpecType)
                    {
                        case eSpecType.PAFlow:
                            pa.MoleFlow.BaseValue = s.SpecValue;
                            break;

                        case eSpecType.PADeltaT:
                            pa.ReturnTemp = section.Trays[pa.DrawTrayIndex].T - s.Value;
                            break;

                        case eSpecType.PARetT:
                            pa.ReturnTemp = s.Value;
                            break;

                        case eSpecType.PADuty:
                            pa.ReturnTemp = section.Trays[pa.DrawTrayIndex].T;
                            break;
                    }
                }
            }
        }

        public static void InitialiseStrippers(LLESEP column)
        {
            ConnectingStreamCollection streams = column.ConnectingDraws.LiquidStreams;

            foreach (ConnectingStream ds in streams) // do liquid int ersection streams
            {
                TraySection st = ds.EngineDrawSection;
                if (st != null)
                {
                    Specification s = column.Specs.GetActiveSpecs().GetTrayLquidSpec(st.Guid, st.Trays.Last().Guid);
                    if (s != null)
                        ds.FlowEstimate = (MoleFlow)s.Value;
                    else
                        ds.FlowEstimate = 0.1;
                }
            }
        }

        public void Update_FlowFactors(LLESEP column)
        {
            Update_LSS_LFactor(column);
            Update_VSS_VFactor(column);
            UpdatePAdraws(column);
            UpdateConnectedSideDraws(column);
            UpdateSideNetreturns(column);
        }

        public static void UpdatePAdraws(LLESEP column)
        {
            for (int section = 0; section < column.TraySections.Count; section++)
                for (int n = 0; n < column.PumpArounds.Count; n++)
                {
                    PumpAround p = column.PumpArounds[n];
                    if (p.drawTray != null)
                        p.MoleFlow.BaseValue = p.drawTray.L * p.DrawFactor;
                }
        }

        public static void UpdateConnectedSideDraws(LLESEP column)
        {
            ConnectingStreamCollection LiquidStreams = column.ConnectingDraws.LiquidStreams;  // need to know draw tray and return   tray
            ConnectingStreamCollection VapourStreams = column.ConnectingDraws.VapourStreams;  // need to know draw tray and return   tray

            foreach (ConnectingStream cs in LiquidStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                if (cs.DrawTrayIndex >= 0)
                    cs.FlowEstimate = column.MainTraySection.Trays[cs.DrawTrayIndex].L * cs.DrawFactor;

            foreach (ConnectingStream cs in VapourStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                if (cs.DrawTrayIndex >= 0)
                    cs.FlowEstimate = column.MainTraySection.Trays[cs.DrawTrayIndex].V * cs.DrawFactor;
        }

        public static void UpdateSideNetreturns(LLESEP column)  // e.g. for mole balance
        {
            ConnectingStreamCollection Liquidreturns = column.ConnectingNetFlows.LiquidStreams;
            ConnectingStreamCollection Vapourreturns = column.ConnectingNetFlows.VapourStreams;

            foreach (ConnectingStream st in Vapourreturns)
            {
                //TraySection section = st.EngineDrawSection;
                Tray tray = st.engineDrawTray;
                st.FlowEstimate = tray.V;
            }

            foreach (ConnectingStream st in Liquidreturns)
            {
                st.FlowEstimate = Math.Exp(st.engineDrawTray.StripFact);
                Tray tray = st.engineDrawTray;
                st.FlowEstimate = tray.L;
            }
        }

        public void ResetFlowValues()
        {
            if (this.column != null)
                foreach (TraySection traysection in column.TraySections)
                    foreach (Tray tray in traysection)
                    {
                        tray.L = 0;
                        tray.V = 0;
                        tray.lss_spec = 0;
                        tray.vss_spec = 0;
                    }
        }

        public void InitialiseFlows(LLESEP column)
        {
            double TotalTopAndSideDraws;
            double TotalFeedsAndreturns;
            ResetFlowValues();
            //Array.Clear(int erSectionLiquidDraws);

            foreach (SideStream stream in column.LiquidSideStreams)
            {
                if (stream.EngineDrawSection != null && stream.EngineDrawTray != null)
                {
                    SpecificationCollection sc = column.Specs.GetSpecs(stream);
                    Specification s = sc.GetLiquidDrawSpec(stream.EngineDrawSection.Guid, stream.EngineDrawTray.Guid);
                    if (s != null)
                        stream.EngineDrawTray.lss_spec = (MoleFlow)s.SpecValue * column.FeedRatio; // Do Side Draws
                    else
                        stream.EngineDrawTray.lss_spec = 0.5; // if draw exists, set flow estimate to non-zero value
                }
            }

            foreach (SideStream stream in column.VapourSideStreams)
            {
                if (stream.EngineDrawSection != null)
                {
                    Specification s = column.Specs.GetSpecs(stream).GetVapourDrawSpec(stream.EngineDrawSection.Guid, stream.EngineDrawTray.Guid);
                    if (s != null)
                        stream.EngineDrawTray.vss_spec = (MoleFlow)s.SpecValue * column.FeedRatio; // Do Side Draws
                    else
                        stream.EngineDrawTray.vss_spec = 0.1; // if draw exists, set flow estimate to non-zero value
                }
            }

            foreach (ConnectingStream d in column.ConnectingDraws)
            {
                int DrawSectionIndex = column.TraySections.IndexOf(d.EngineDrawSection);
                int RetSectionIndex = column.TraySections.IndexOf(d.EngineReturnSection);
                //Tray drawtray = d.EngineDrawTray;
                //drawtray.lss_spec = d.Flow;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && d.isliquid)
                {
                    interSectionLiquidDraws[DrawSectionIndex][d.DrawTrayIndex] += d.FlowEstimate;
                    interSectionLiquidFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;
                }

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && !d.isliquid)
                {
                    interSectionVapourDraws[DrawSectionIndex][d.DrawTrayIndex] += d.FlowEstimate;
                    interSectionVapourFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;
                }
            }

            foreach (ConnectingStream d in column.ConnectingNetFlows)
            {
                int DrawSectionIndex = column.TraySections.IndexOf(d.EngineDrawSection);
                int RetSectionIndex = column.TraySections.IndexOf(d.EngineReturnSection);

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && d.isliquid)
                    interSectionLiquidFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;

                if (d.DrawTrayIndex >= 0 && d.ReturnTrayIndex >= 0 && DrawSectionIndex >= 0 && RetSectionIndex >= 0 && !d.isliquid)
                    interSectionVapourFeeds[RetSectionIndex][d.ReturnTrayIndex] += d.FlowEstimate;
            }

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;
                Specification vaprate;

                switch (section.CondenserType)
                {
                    case CondType.Partial:
                        vaprate = column.Specs.GetActiveSpecs().GetCondVapourRateSpec(section.Guid);

                        if (vaprate != null)
                            section.Trays[0].V = (MoleFlow)vaprate.SpecValue / column.FeedRatio;     // Vapour Spec
                        else
                            section.Trays[0].V = 0.05;
                        break;

                    case CondType.Subcooled:
                        section.Trays[0].V = 0;
                        break;

                    case CondType.TotalReflux:
                        vaprate = column.Specs.GetActiveSpecs().GetCondVapourRateSpec(section.Guid);

                        if (vaprate != null)
                            section.Trays[0].V = (MoleFlow)vaprate.SpecValue / column.FeedRatio;     // Vapour Spec
                        else
                            section.Trays[0].V = 0.05;
                        break;

                    case CondType.None:
                        section.Trays[0].V = 0.01; // StripperDraws[section][0];
                        break;
                }

                TotalTopAndSideDraws = 0;
                if (MainFeed == null)
                    return;
                TotalFeedsAndreturns = MainFeed.MolarFlow_;
                TotalTopAndSideDraws += section.Trays[0].V;

                for (int ii = 0; ii < column.TraySections[sectionNo].Trays.Count; ii++)
                {
                    TotalTopAndSideDraws += section.Trays[ii].lss_spec;
                    //+ int erSectionLiquidDraws[sectionNo][ii] - int erSectionLiquidFeeds[sectionNo][ii]
                    //+ int erSectionVapourDraws[sectionNo][ii] - int erSectionVapourFeeds[sectionNo][ii];
                    TotalFeedsAndreturns += interSectionVapourFeeds[sectionNo][ii];
                }

                switch (section.CondenserType)
                {
                    case CondType.Partial:
                    case CondType.Subcooled:
                    case CondType.TotalReflux:
                        Specification s = column.Specs.GetActiveSpecs().GetRefluxRatioSpec(section.Guid); // REFLUX Rate is Known

                        foreach (PumpAround pa in column.PumpArounds)
                        {
                            if (pa.drawTray != null && pa.returnTray != null)
                            {
                                pa.drawTray.L -= pa.MoleFlow;
                                pa.returnTray.L += pa.MoleFlow;
                            }
                        }

                        if (s != null)
                            section.Trays[0].L += (section.Trays[0].V + section.Trays[0].lss_spec) * s.SpecValue + interSectionLiquidFeeds[sectionNo][0];
                        else
                            section.Trays[0].L += (section.Trays[0].V + section.Trays[0].lss_spec) * DefaultRefluxRatio + interSectionLiquidFeeds[sectionNo][0]; // assume reflux ratio of 1

                        for (int trayNo = 1; trayNo < TrayCount - 1; trayNo++) // initialise liquid rates;
                            section.Trays[trayNo].L += section.Trays[trayNo - 1].L +
                                section.Trays[trayNo].feed.RatioedLiquidFeed -
                                section.Trays[trayNo].lss_spec +
                                 interSectionLiquidFeeds[sectionNo][trayNo] -
                                 interSectionLiquidDraws[sectionNo][trayNo];

                        section.Trays[TrayCount - 1].L = TotalFeeds - TotalTopAndSideDraws;

                        section.Trays[TrayCount - 1].V = section.Trays[TrayCount - 2].L - section.Trays[TrayCount - 1].L + section.Trays[TrayCount - 1].feed.RatioedVapourFeed;

                        for (int tray = TrayCount - 2; tray > 0; tray--)
                            section.Trays[tray].V = section.Trays[tray + 1].V
                                + interSectionVapourFeeds[sectionNo][tray]
                                + section.BottomTray.feed.RatioedVapourFeed;
                        break;

                    case CondType.None:  // e.g. stripper, simplified
                        section.Trays[0].L += section.Trays[0].feed.RatioedLiquidFeed
                            + interSectionLiquidFeeds[sectionNo][0]
                            - interSectionLiquidDraws[sectionNo][0];

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
                                + interSectionVapourFeeds[sectionNo][tray]
                                - interSectionVapourDraws[sectionNo][tray];
                        break;
                }
            }
        }

        internal void AddTrayHeatBalancesToSpecs()
        {
            int count = 0;
            List<Specification> remove = new();

            foreach (Specification item in column.Specs)
                if (item.engineSpecType == eSpecType.TrayDuty)
                    remove.Add(item);

            foreach (Specification item in remove)
            {
                int loc = column.Specs.IndexOf(item);
                column.Specs.RemoveAt(loc);
            }

            foreach (TraySection ts in column.TraySections)
            {
                foreach (Tray tray in ts)
                {
                    count++;
                    if (tray.IsHeatBalanced)
                    {
                        Specification spec = new("tray duty", tray.Duty, ePropID.EnergyFlow, eSpecType.TrayDuty, ts, tray, tray, true);
                        spec.StageNo = count;
                        column.Specs.Add(spec);
                    }
                }
            }
        }

        internal void UpdatePorts(bool FlashAllPorts = false)
        {
            int count = 0;
            foreach (TraySection traySection in column.TraySections)
            {
                foreach (Tray tray in traySection)
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

            foreach (ConnectingStream cs in column.ConnectingDraws)
            {
                Tray T = cs.engineDrawTray;
                T.liquidDrawRight.MolarFlow_.BaseValue = cs.FlowEstimate / column.feedRatio;
            }

            foreach (SideStream ss in column.LiquidSideStreams)
            {
                Tray tray = ss.EngineDrawTray;
                tray.liquidDrawRight.MolarFlow_.BaseValue = tray.lss_estimate / column.feedRatio;
                tray.liquidDrawRight.MolarFlow_.origin = SourceEnum.UnitOpCalcResult;

                // Debug.Print (tray.Name);
                // Debug.Print (tray.liquidDrawRight.Components.MW().ToString());
                // Debug.Print (tray.liquidDrawRight.Components.SG().ToString());
            }

            foreach (PumpAround pa in column.PumpArounds)//Only need to update return   streams
            {
                int returnsectionindex = column.TraySections.IndexOf(pa.returnSection);
                int returntrayindex = pa.ReturnTrayIndex;
               /* pa.ReturnPort.cc = column.Components.Clone();
                pa.ReturnPort.cc.Origin = SourceEnum.UnitOpCalcResult;
                pa.ReturnPort.H_.BaseValue = PAreturnEnthalpy[returnsectionindex][returntrayindex];
                pa.ReturnPort.P_.BaseValue = pa.returnTray.P;
                pa.ReturnPort.MolarFlow_.BaseValue = pa.MoleFlow / column.feedRatio;
                pa.ReturnPort.MolarFlow_.origin = SourceEnum.UnitOpCalcResult;
                pa.ReturnPort.cc.SetMolFractions(pa.drawTray.LiqComposition);

                pa.DrawPort.cc = column.Components.Clone();
                pa.DrawPort.cc.Origin = SourceEnum.UnitOpCalcResult;
                pa.DrawPort.SetPortValue(ePropID.T, pa.drawTray.T, SourceEnum.UnitOpCalcResult);
                pa.DrawPort.SetPortValue(ePropID.P, pa.drawTray.P, SourceEnum.UnitOpCalcResult);
                pa.DrawPort.SetPortValue(ePropID.MOLEF, pa.MoleFlow / column.feedRatio, SourceEnum.UnitOpCalcResult);
                pa.DrawPort.cc.SetMolFractions(pa.drawTray.LiqComposition);
                pa.DrawPort.Flash();*/
            }
        }

        internal void UpdateVapEnthalpyParameters()
        {
            switch (column.SolverOptions.VapEnthalpyMethod)
            {
                case ColumnEnthalpyMethod.BostonBrittHdep:
                    foreach (TraySection section in column.TraySections)
                        foreach (Tray tray in section)
                            tray.UpdateVapourDepEnthalpies(column.Components, column.Thermo);

                    break;

                case ColumnEnthalpyMethod.SimpleLinear:
                    foreach (TraySection section in column.TraySections)
                        foreach (Tray tray in section)
                            tray.UpdateVapourLinearEnthalpies(column.Components, column.Thermo);

                    break;

                default:
                    break;
            }
        }

        internal void UpdateLiqEnthalpyParameters()
        {
            switch (column.SolverOptions.LiqEnthalpyMethod)
            {
                case ColumnEnthalpyMethod.BostonBrittHdep:
                    foreach (TraySection section in column.TraySections)
                        foreach (Tray tray in section)
                            tray.UpdateLiquidDepEnthalpies(column.Components, column.Thermo);

                    break;

                case ColumnEnthalpyMethod.SimpleLinear:
                    foreach (TraySection section in column.TraySections)
                        foreach (Tray tray in section)
                            tray.UpdateLiquidLinearEnthalpies(column.Components, column.Thermo);

                    break;

                default:
                    break;
            }
        }

        internal void UpdateInitialCompositions(double dampfactor)
        {
            foreach (TraySection section in column.TraySections)
                foreach (Tray tray in section)
                {
                    for (int i = 0; i < column.Components.Count; i++)
                    {
                        tray.LiqCompositionInitial[i] += (tray.LiqComposition[i] - tray.LiqCompositionInitial[i]) * dampfactor;
                        tray.VapCompositionInitial[i] += (tray.VapComposition[i] - tray.VapCompositionInitial[i]) * dampfactor;
                    }
                }
        }

        internal void UpdateActualCompositions()
        {
            foreach (TraySection section in column.TraySections)
                foreach (Tray tray in section)
                {
                    tray.LiqComposition = (double[])tray.LiqCompositionPred.Clone();
                    tray.VapComposition = (double[])tray.VapCompositionPred.Clone();
                }
        }

        internal void SetBasicValues(LLESEP column)
        {
            foreach (TraySection traySection in column.TraySections)
            {
                if (traySection.HasCondenser)
                    traySection.Trays[0].IsHeatBalanced = false;
                if (traySection.HasReboiler)
                    traySection.BottomTray.IsHeatBalanced = false;
            }

            TotNoTrays = column.TotNoStages;
            NoSections = column.NoSections;

            column.Components.Clear();

            foreach (TraySection traysection in column.TraySections)
            {
                foreach (Tray tray in traysection)
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
            //MathNet.Numerics.Control.UseNativeOpenBLAS();  // very slow
            MathNet.Numerics.Control.UseManaged();
            //MathNet.Numerics.Control.UseMultiThreading();  // seems to have no impact
        }

        public bool CreateInitialLLEAlphas(ThermoDynamicOptions thermo)
        {
            double[] X, Y;
            if (column is null)
                return false;

            Components HeavyFeed = column[0].TopTray.feed.cc.Clone();
            Components LightFeed = column[0].BottomTray.feed.cc.Clone();

            int HeavyIndex = HeavyFeed.GetHeaviestCompIndex();
            int LightIndex = HeavyFeed.GetLightestCompIndex();

            TraySection section = column.MainTraySection;

            int noComps = column.Components.Count;
            X = new double[noComps];
            Y = new double[noComps];

            X[HeavyIndex] = 1;
            Y[LightIndex] = 1;

            double[] Act = ThermodynamicsClass.KMixArray(column.Components.Clone(), section.Trays[0].P.BaseValue, 273.15 + 25, X, Y, out _, thermo, true);

            for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
            {
                Tray tray = section.Trays[trayNo];

                Temperature TrayTempK = section.Trays[trayNo].T;
                double TrayP = section.Trays[trayNo].P;
                tray.LLE_K = Act;
                if (tray.LLE_K is null)
                    return false;
            }
            return true;
        }

        public bool UpdateInitialActivies(ThermoDynamicOptions thermo)
        {
            double[] X, Y;
            if (column is null)
                return false;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    Tray tray = section.Trays[trayNo];
                    Pressure P = tray.P.BaseValue;

                    if (tray.LnKCompOld == null)
                        tray.LnKCompOld = new double[column.Components.Count];
                    else
                        tray.LnKCompOld = tray.LLinear.LnKComp;

                    Temperature TrayTempK = section.Trays[trayNo].T;
                    double TrayP = section.Trays[trayNo].P;

                    X = tray.LiqCompositionInitial;
                    Y = tray.VapCompositionInitial; // no vapour estimates at this point , maybe just use wilson??

                    if (tray.TrayEff != 100)
                    {
                        tray.LLE_K = ThermodynamicsClass.LnKMixWithEfficiency(column.Components, P, TrayTempK, X, Y, out _, thermo, tray.TrayEff);
                        if (tray.LLE_K is null)
                            return false;
                        tray.ActivityY = ThermodynamicsClass.LnKMixWithEfficiency(column.Components, P, TrayTempK + KDelta, X, Y, out _, thermo, tray.TrayEff);
                        if (tray.ActivityY is null)
                            return false;
                    }
                    else
                    {
                        tray.LLE_K = ThermodynamicsClass.KMixArray(column.Components, P, TrayTempK, X, Y, out _, thermo, true);
                        if (tray.LLE_K is null)
                            return false;
                    }
                }
            }
            return true;
        }

        public bool UpdateAlphas(ThermoDynamicOptions thermo)
        {
            double[] X, Y;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    Tray tray = section.Trays[trayNo];
                    Pressure P = tray.P.BaseValue;

                    if (tray.LnKCompOld == null)
                        tray.LnKCompOld = new double[column.Components.Count];
                    else
                        tray.LnKCompOld = tray.LLinear.LnKComp;

                    Temperature TrayTempK = section.Trays[trayNo].T;
                    double TrayP = section.Trays[trayNo].P;

                    X = tray.LiqComposition;
                    Y = tray.VapComposition; // no vapour estimates at this point , maybe just use wilson??

                    if (tray.TrayEff != 100)
                    {
                        tray.LLE_K = ThermodynamicsClass.LnKMixWithEfficiency(column.Components, P, TrayTempK, X, Y, out _, thermo, tray.TrayEff, true);
                        if (tray.LLE_K is null)
                            return false;
                        tray.ActivityY = ThermodynamicsClass.LnKMixWithEfficiency(column.Components, P, TrayTempK + KDelta, X, Y, out _, thermo, tray.TrayEff, true);
                        if (tray.ActivityY is null)
                            return false;
                    }
                    else
                    {
                        tray.LLE_K = ThermodynamicsClass.KMixArray(column.Components, P, TrayTempK, X, Y, out _, thermo, true);
                        if (tray.LLE_K is null)
                            return false;
                        tray.ActivityY = ThermodynamicsClass.KMixArray(column.Components, P, TrayTempK + KDelta, X, Y, out _, thermo, true);
                        if (tray.ActivityY is null)
                            return false;
                    }
                }
            }
            return true;
        }

        public void intialiseStripFactorsEtc()
        {
            if (column is null)
                return;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];

                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    Tray tray = section.Trays[trayNo];
                    tray.StripFact = tray.V / tray.L;
                }
            }
        }

        public bool CalcInitialTs(LLESEP column)
        {
            SetBasicValues(column);
            column.Components = new Components();
            foreach (TraySection traysection in column.TraySections)
            {
                foreach (Tray tray in traysection)
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
            interpolatePressures();
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
                UpdateInitialActivies(thermotemp);
                intialiseStripFactorsEtc();
            }

            SolveComponentBalance(column);  // get initial estimate of componenents

            return true;
        }

        public void UpdateColumnBalance(ref double ErrorSum, enumCalcResult cres)
        {
            SolveComponentBalance(column);
            UpdateInitialCompositions(1);
            UpdateActualCompositions();
            //Update_FlowFactors(column);
            EnthalpiesUpdate(ref cres);    // future enthalpies
            UpdateTrayEnthalpyBalances();  // future balance
            CalcErrors(true);              // Future Errors
            ProcessActiveSpecs(out Errors, true);
            ErrorSum = SumArraySquared(Errors);
        }

        public bool UpdateFactors(double[] grads, double gradientfactor = double.NaN)
        {
            double MIND = 1e-10;
            double MAXD = 20;

            if (grads is null)
                return false;

            double[] gradmod = (double[])grads.Clone();

            int deltacount = 0;
            double MaxGrad = gradmod.AbsMax();

            double MINSF = Math.Log(MIND);
            double MAXSF = Math.Log(MAXD);

            // do tray stripping factors
            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];

                for (int trayno = 0; trayno < section.Trays.Count; trayno++)  // update strip factors
                {
                    Tray tray = section.Trays[trayno];
                    tray.StripFact = tray.StripFact - gradmod[deltacount] * gradientfactor;  // Russel LOG form
                    deltacount++;
                }

                //do tray temps
                for (int trayno = 0; trayno < section.Trays.Count; trayno++)  // update strip factors
                {
                    Tray tray = section.Trays[trayno];
                    tray.T -= gradmod[deltacount] * gradientfactor;
                    deltacount++;
                }
            }
            return true;
        }

        public void Update_LSS_LFactor(LLESEP cd)
        {
            TraySection section;
            Tray tray;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                section = cd.TraySections[sectionNo];

                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
                {
                    tray = section.Trays[trayNo];
                    tray.lss_estimate = (tray.LiqDrawFactor) * tray.L;
                    if (tray.LiqCompositionPred != null && WaterLoc > -1)
                        tray.WaterEstimate = (tray.WaterDrawFactor) * (tray.LiqCompositionPred[WaterLoc] * tray.L);
                }
            }
        }

        public void Update_VSS_VFactor(LLESEP cd)
        {
            TraySection section;
            Tray tray;

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                section = cd.TraySections[sectionNo];

                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++) // initialise liquid rates, calculate from assumed vapour rates;
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

        public void SolveComponentBalance(LLESEP column)
        {
            int countstages, CurrentStage;
            double TrayTemp, TrayTempPlusOne = 0;
            double waterfact = 0;
            countstages = 0; // reset

            for (int i = 0; i < TrDMatrix.Length; i++)
                for (int y = 0; y < TrDMatrix[i].Length; y++)
                    Array.Clear(TrDMatrix[i][y], 0, TrDMatrix[i][y].Length);

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                int TrayCount = section.Trays.Count;

                for (int trayNo = 0; trayNo < TrayCount; trayNo++)
                {
                    double k1 = 0;
                    Tray tray = section.Trays[trayNo];
                    Tray trayp1 = null;

                    CurrentStage = countstages + trayNo;
                    TrayTemp = tray.T;

                    if (trayNo < section.Trays.Count - 1)
                    {
                        TrayTempPlusOne = section.Trays[trayNo + 1].T;
                        trayp1 = section.Trays[trayNo + 1];
                    }

                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        double[][] TrdComp = TrDMatrix[comp];
                        double[] Stage = TrdComp[CurrentStage];

                        if (trayNo < section.Trays.Count - 1)
                            k1 = trayp1.K_TestFast(comp, TrayTempPlusOne, column.SolverOptions.KMethod);

                        double k = tray.K_TestFast(comp, TrayTemp, column.SolverOptions.KMethod);

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

            if (this.column is null)
                return;
            // Do PumpArounds
            foreach (PumpAround p in this.column.PumpArounds)
            {
                for (int comp = 0; comp < NoComps; comp++)
                {
                    double[][] TRD = TrDMatrix[comp];
                    if (p.ReturnTrayIndex >= 0 && p.DrawTrayIndex >= 0)
                    {
                        TRD[p.ReturnTrayIndex][p.DrawTrayIndex] -= p.DrawFactor;
                        TRD[p.DrawTrayIndex][p.DrawTrayIndex] += p.DrawFactor;
                    }
                }
            }

            // Do int erconecting Liquid streams
            for (int comp = 0; comp < NoComps; comp++)
            {
                if (column.ConnectingDraws.Count > 0)
                {
                    // Do int erconecting Liquid Draws
                    ConnectingStreamCollection LiquidStreams = column.ConnectingDraws.LiquidStreams;  // need to know draw tray and return   tray

                    foreach (ConnectingStream stream in LiquidStreams)  // do liquid draws // TrDMatrix[Comp][Down][Across]
                    {
                        TraySection dcts_Draw = stream.EngineDrawSection;
                        TraySection dcts_return = stream.EngineReturnSection;

                        if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                            return;

                        int drawsectionindex = column.TraySections.IndexOf(dcts_Draw);
                        int drawtrayindex = dcts_Draw.Trays.IndexOf(stream.engineDrawTray);
                        int returntrayindex = dcts_return.Trays.IndexOf(stream.engineReturnTray);
                        int returnsectionindex = column.TraySections.IndexOf(stream.EngineReturnSection);

                        int returnTraysSum = NoTraysToThisSection(returnsectionindex);

                        if (drawtrayindex >= 0 && returntrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] += stream.DrawFactor;
                            TrDMatrix[comp][returntrayindex + returnTraysSum][drawtrayindex] -= stream.DrawFactor;
                        }
                    }

                    // Do int erconecting Vapour Draws
                    ConnectingStreamCollection VapourStreams = column.ConnectingDraws.VapourStreams;  // need to know draw tray and return   tray

                    foreach (ConnectingStream stream in VapourStreams)  // do vapour draws
                    {
                        TraySection Draw_Section = stream.EngineDrawSection;
                        TraySection return_Section = stream.EngineReturnSection;
                        Tray DrawSectionTray;

                        int drawsectionindex = column.TraySections.IndexOf(Draw_Section);
                        int returnsectionindex = column.TraySections.IndexOf(return_Section);
                        int drawtrayindex = Draw_Section.Trays.IndexOf(stream.engineDrawTray);
                        int returntrayindex = return_Section.Trays.IndexOf(stream.engineReturnTray);
                        int DrawTraysSum = NoTraysToThisSection(drawsectionindex);
                        int returnTraysSum = NoTraysToThisSection(returnsectionindex);

                        //Specification spec = column.Specs.GetActiveSpecs().GetVapourDrawSpec(column, Draw_Section.Guid, drawtrayindex);
                        double VapDrawFact = stream.DrawFactor;

                        DrawSectionTray = Draw_Section.Trays[drawtrayindex];
                        double k = DrawSectionTray.K_TestFast(comp, stream.engineDrawTray.T, column.SolverOptions.KMethod);

                        //correct vapour draw
                        TrDMatrix[comp][drawtrayindex + DrawTraysSum - 1][drawtrayindex + DrawTraysSum]
                           = -DrawSectionTray.StripFact * k * (1 - VapDrawFact);

                        // Vapour return
                        TrDMatrix[comp][returntrayindex + returnTraysSum][drawtrayindex + DrawTraysSum]
                                               = -DrawSectionTray.StripFact * k * VapDrawFact;
                        //TrDMatrix[comp][][] = 0;
                    }
                }

                if (column.ConnectingNetFlows.Count > 0)
                {
                    // do liquid net flows
                    foreach (ConnectingStream stream in column.ConnectingNetFlows.LiquidStreams)   // TrDMatrix[Comp][Down][Across]
                    {
                        TraySection dcts_Draw = stream.EngineDrawSection;
                        TraySection dcts_return = stream.EngineReturnSection;

                        int drawsectionindex = column.TraySections.IndexOf(dcts_Draw);
                        int drawtrayindex = dcts_Draw.Trays.IndexOf(stream.engineDrawTray);
                        int returntrayindex = dcts_return.Trays.IndexOf(stream.engineReturnTray);
                        int returnsectionindex = column.TraySections.IndexOf(stream.EngineReturnSection);

                        int returnTraysSum = NoTraysToThisSection(returnsectionindex);
                        int DrawTraysSum = NoTraysToThisSection(drawsectionindex);

                        if (drawtrayindex == dcts_Draw.NoTrays - 1)
                        {
                            TrDMatrix[comp][drawtrayindex][DrawTraysSum + drawtrayindex] = -1;
                        }

                        /*if (drawtrayindex >= 0 && returnTrayindex >= 0)
                        {
                            TrDMatrix[comp][drawtrayindex][drawtrayindex] += stream.DrawFactor;
                            TrDMatrix[comp][returnTrayindex + returnTraysSum][drawtrayindex] -= stream.DrawFactor;
                        }*/
                    }

                    // Do int erconecting Vapour net flows
                    foreach (ConnectingStream stream in column.ConnectingNetFlows.VapourStreams)  // do vapour draws
                    {
                        TraySection Draw_Section = stream.EngineDrawSection;
                        TraySection return_Section = stream.EngineReturnSection;
                        Tray drawtray = stream.engineDrawTray;

                        int drawsectionindex = column.TraySections.IndexOf(Draw_Section);
                        int drawtrayindex = Draw_Section.Trays.IndexOf(stream.engineDrawTray);
                        int returntrayindex = return_Section.Trays.IndexOf(stream.engineReturnTray);
                        int DrawTraysSum = NoTraysToThisSection(drawsectionindex);

                        if (drawsectionindex >= 0 && drawtrayindex >= 0 && returntrayindex >= 0)
                            // Vapour return
                            TrDMatrix[comp][returntrayindex][drawtrayindex + DrawTraysSum] =
                                -drawtray.StripFact * drawtray.K_TestFast(comp, stream.engineDrawTray.T, column.SolverOptions.KMethod);
                        //TrDMatrix[comp][][] = 0;
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
                TraySection section = column.TraySections[sectionNo];

                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                {
                    Tray tray = section.Trays[trayNo];
                    tray.L = 0;
                    tray.V = 0;

                    for (int comp = 0; comp < NoComps; comp++)
                    {
                        //TotalLiq[section][tray] += CompLiqMolarFlows[tray + SumTrayCount][comp];
                        CompVapMolarFlows[SumTrayCount][comp] = CompLiqMolarFlows[SumTrayCount][comp]
                            * tray.K_TestFast(comp, tray.T, column.SolverOptions.KMethod) * tray.StripFact;

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
                }
            }
            if (WaterCompNo != null)
                WaterDraw = CompLiqMolarFlows[0][(int)WaterCompNo] * WaterDrawFactor;

            //UpdateActualCompositions();
        }

        public void UpdateTrayEnthalpyBalances()  // Inside Loop // Should be non-rigourous inside loop.
        {
            double[][] EnthAdjustments = new double[NoSections][];

            for (int sectionNo = 0; sectionNo < NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                EnthAdjustments[sectionNo] = new double[section.Trays.Count];

                int StartTray = 0;
                int EndTray = section.Trays.Count;

                double LiquidFeedEnthalpy;
                double VapourFeedEnthalpy;

                for (int trayNo = StartTray; trayNo < EndTray; trayNo++)
                {
                    Tray tray = section.Trays[trayNo];

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
                        if (section.CondenserType == CondType.Subcooled)
                            tray.liqenth -= column.Subcoolduty;

                        tray.enthalpyBalance =
                            -tray.liqenth * tray.L                                  // Liquid Leaving Tray
                            + section.Trays[trayNo + 1].vapenth * section.Trays[trayNo + 1].V                         // Vapour Entering Tray
                            - tray.vapenth * tray.V;                                 // Vapour Leaving Tray
                    }
                    else if (trayNo < EndTray - 1) // Middle Trays
                    {
                        tray.enthalpyBalance =
                            section.Trays[trayNo - 1].liqenth * section.Trays[trayNo - 1].L        // Liquid entering tray
                            - tray.liqenth * tray.L              // Liquid Leaving Tray
                            + section.Trays[trayNo + 1].vapenth * section.Trays[trayNo + 1].V      // Vapour Entering Tray
                            - tray.vapenth * tray.V;            // Vapour Leaving Tray
                    }
                    else // Bottom Tray
                    {
                        tray.enthalpyBalance =
                              section.Trays[trayNo - 1].liqenth * section.Trays[trayNo - 1].L       // Liquid entering tray
                            - tray.liqenth * tray.L               // Liquid Leaving Tray
                            - tray.vapenth * tray.V;               // Vapour Entering Tray
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

            foreach (PumpAround pa in column.PumpArounds)
            {
                int drawtray = pa.DrawTrayIndex;
                int returntray = pa.ReturnTrayIndex;
                int pasection = column.TraySections.IndexOf(pa.drawSection);

                if (pasection >= 0 && drawtray >= 0 && pasection < column.TraySections.Count)
                {
                    EnthAdjustments[pasection][drawtray] -= pa.drawTray.liqenth * pa.MoleFlow;

                    Specification s = column.Specs.GetActiveSpecs().GetPAEnergySpec(pa.Guid);
                    if (s is null)
                        System.Windows.Forms.MessageBox.Show("No PA Energy Spec set");
                    else
                        EnthAdjustments[pasection][returntray] += pa.drawTray.PA_liqenth(column, s) * pa.MoleFlow;
                }
            }

            foreach (ConnectingStream stream in column.ConnectingDraws.LiquidStreams)
            {
                TraySection dcts_draw = stream.EngineDrawSection;
                TraySection dcts_ret = stream.EngineReturnSection;

                int drawsection = column.TraySections.IndexOf(dcts_draw);
                int returnsection = column.TraySections.IndexOf(dcts_ret);

                if (stream.engineDrawTray is null || stream.engineReturnTray is null)
                    return;

                int drawtray = dcts_draw.Trays.IndexOf(stream.engineDrawTray);
                int returntray = dcts_ret.Trays.IndexOf(stream.engineReturnTray);

                if (drawtray >= 0 && returntray >= 0)
                {
                    EnthAdjustments[drawsection][drawtray] -= dcts_draw.Trays[drawtray].liqenth * stream.FlowEstimate;
                    EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].liqenth * stream.FlowEstimate;
                }
            }

            foreach (ConnectingStream st in column.ConnectingDraws.VapourStreams)
            {
                TraySection dcts_draw = st.EngineDrawSection;
                TraySection dcts_ret = st.EngineReturnSection;

                int drawsection = column.TraySections.IndexOf(dcts_draw);
                int returnsection = column.TraySections.IndexOf(dcts_ret);

                int drawtray = dcts_draw.Trays.IndexOf(st.engineDrawTray);
                int returntray = dcts_ret.Trays.IndexOf(st.engineReturnTray);

                EnthAdjustments[drawsection][drawtray] -= dcts_draw.Trays[drawtray].vapenth * st.FlowEstimate;
                EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].vapenth * st.FlowEstimate;
            }

            foreach (ConnectingStream st in column.ConnectingNetFlows.LiquidStreams)
            {
                TraySection dcts_draw = st.EngineDrawSection;
                TraySection dcts_ret = st.EngineReturnSection;

                int returnsection = column.TraySections.IndexOf(dcts_ret);

                int drawtray = dcts_draw.Trays.IndexOf(st.engineDrawTray);
                int returntray = dcts_ret.Trays.IndexOf(st.engineReturnTray);

                // Net flows allready in heat balance

                if (returnsection >= 0 && returntray >= 0)
                    EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].liqenth * st.engineDrawTray.L;
            }

            foreach (ConnectingStream st in column.ConnectingNetFlows.VapourStreams)
            {
                TraySection dcts_draw = st.EngineDrawSection;
                TraySection dcts_ret = st.EngineReturnSection;

                int returnsection = column.TraySections.IndexOf(dcts_ret);

                int drawtray = dcts_draw.Trays.IndexOf(st.engineDrawTray);
                int returntray = dcts_ret.Trays.IndexOf(st.engineReturnTray);

                if (returntray >= 0)
                    // Net flows allready in heat balance
                    // EnthAdjustments[drawsection][drawtray] -= dcts_draw[drawtray].vapenth * st.drawTray.V;
                    EnthAdjustments[returnsection][returntray] += dcts_draw.Trays[drawtray].vapenth * st.engineDrawTray.V;
            }

            for (int i = 0; i < NoSections; i++)
            {
                TraySection section = column.TraySections[i];
                for (int trayNo = 0; trayNo < section.Trays.Count; trayNo++)
                    section.Trays[trayNo].enthalpyBalance += EnthAdjustments[i][trayNo];
            }

            return;
        }

        public void CreateJacobianSF(LLESEP column, ref enumCalcResult cres) // strip factor
        {
            column.TraySections.BackupFactors(column);
            int ColumCount = 0;
            int startcount = 0;
            //ViewArray va = new ();

            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                for (int trayNo = startcount; trayNo < section.Trays.Count; trayNo++)  // Do strip factors for main tray section + Stripper sections
                {
                    section.Trays[trayNo].StripFact = Math.Exp(Math.Log(section.Trays[trayNo].StripFact) + JDelta);
                    SolveComponentBalance(column);    //  TRD Matrix changed
                                                      //UpdateActualCompositions();
                                                      //va.View(TrDMatrix[0]);
                    Update_FlowFactors(column);
                    EnthalpiesUpdate(ref cres);
                    UpdateTrayEnthalpyBalances();
                    CalcErrors(false);
                    ProcessActiveSpecs(out Errors, false);
                    UpdateJacobian(ColumCount, Errors);
                    column.TraySections.UnBackupFactor(column);
                    ColumCount++;
                    if (cres == enumCalcResult.Failed)
                        return;
                }
            }

            // Liquid Sidestreams
            foreach (SideStream stream in column.LiquidSideStreams)
            {
                if (stream.EngineDrawTray != null)
                {
                    TraySection section = stream.EngineDrawSection;
                    stream.EngineDrawTray.LiqDrawFactor = Math.Exp(Math.Log(stream.EngineDrawTray.LiqDrawFactor) + JDelta);
                    SolveComponentBalance(column);
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

            // Vapour Sidestreams
            foreach (SideStream ss in column.VapourSideStreams)
            {
                TraySection section = ss.EngineDrawSection;
                ss.EngineDrawTray.VapDrawFactor = Math.Exp(Math.Log(ss.EngineDrawTray.VapDrawFactor) + JDelta);
                SolveComponentBalance(column);
                Update_FlowFactors(column);
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
            foreach (ConnectingStream st in column.ConnectingDraws.LiquidStreams)
            {
                st.DrawFactor = Math.Exp(Math.Log(st.DrawFactor) + JDelta);
                SolveComponentBalance(column);
                Update_FlowFactors(column);
                EnthalpiesUpdate(ref cres);
                UpdateTrayEnthalpyBalances();
                CalcErrors(false);
                ProcessActiveSpecs(out Errors, false);
                UpdateJacobian(ColumCount, Errors);
                column.TraySections.UnBackupFactor(column);
                ColumCount++;
            }

            foreach (ConnectingStream st in column.ConnectingDraws.VapourStreams)
            {
                st.DrawFactor = Math.Exp(Math.Log(st.DrawFactor) + JDelta);
                SolveComponentBalance(column);
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
                PumpAround p = column.PumpArounds[i];
                // SpecificationCollection PAspecs = column.Specs.GetActivePASpecs(p.Guid);

                if (p != null)
                {
                    p.DrawFactor = Math.Exp(Math.Log(p.DrawFactor) + JDelta);
                    SolveComponentBalance(column);
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
        }

        public void CreateJacobianLLE(LLESEP column, ref enumCalcResult cres) // strip factor
        {
            column.TraySections.BackupFactors(column);
            int ColumCount = 0;
            int startcount = 0;
            //ViewArray va = new();

            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                for (int trayNo = startcount; trayNo < section.Trays.Count; trayNo++)  // Do strip factors for main tray section + Stripper sections
                {
                    //section.Trays[trayNo].StripFact = Math.Exp(Math.Log(section.Trays[trayNo].StripFact) + JDelta);
                    section.Trays[trayNo].StripFact = section.Trays[trayNo].StripFact + JDelta;
                    SolveComponentBalance(column);    //  TRD Matrix changed
                    UpdateActualCompositions();
                    //va.View(TrDMatrix[0]);
                    Update_FlowFactors(column);
                    EnthalpiesUpdate(ref cres);
                    UpdateTrayEnthalpyBalances();
                    CalcErrors(false);
                    ProcessActiveSpecs(out Errors, false);
                    UpdateJacobian(ColumCount, Errors);
                    column.TraySections.UnBackupFactor(column);
                    ColumCount++;
                    if (cres == enumCalcResult.Failed)
                        return;
                }
            }

            for (int sectionNo = 0; sectionNo < column.NoSections; sectionNo++)
            {
                TraySection section = column.TraySections[sectionNo];
                for (int trayNo = startcount; trayNo < section.Trays.Count; trayNo++)  // Do strip factors for main tray section + Stripper sections
                {
                    SolveComponentBalance(column);    //  TRD Matrix changed
                    UpdateActualCompositions();
                    //va.View(TrDMatrix[0]);
                    Update_FlowFactors(column);

                    section.Trays[trayNo].T += 0.1;
                    EnthalpiesUpdate(ref cres);
                    UpdateTrayEnthalpyBalances();
                    CalcErrors(false);
                    ProcessActiveSpecs(out Errors, false);
                    UpdateJacobian(ColumCount, Errors);
                    section.Trays[trayNo].T -= 0.1;
                    column.TraySections.UnBackupFactor(column);
                    ColumCount++;
                    if (cres == enumCalcResult.Failed)
                        return;
                }
            }
        }

        public bool UpdateJacobian(int column, double[] err)
        {
            int counter = 0;

            for (int row = 0; row < err.Length; row++)
            {
                if (counter >= Jacobian.Length * 2 || column >= Jacobian[counter].Length * 2)
                    return false;
                else
                {
                    Jacobian[counter][column] = err[row];
                    counter++;
                }
            }
            return true;
        }

        public static bool SolveJacobian(double[][] Jacobian, double[] errors, ref double[] grads)
        {
            enumInverseMethod method = enumInverseMethod.MathNet;

            if (errors != null)
            {
                switch (method)
                {
                    case enumInverseMethod.GeneralMatrix:
                        GeneralMatrix A = new(Jacobian);
                        GeneralMatrix B = new(errors, errors.Length);
                        A = A.Inverse();

                        if (A != null)
                            A = A.Multiply(B);
                        else
                            return false;

                        grads = A.Transpose().Array[0];
                        return true;

                    case enumInverseMethod.Crouts:
                        //double [][] A1 = MatrixInverse.MatrixInverseProgram.MatrixInverse(Jacobian);
                        double[][] B1 = new double[1][];
                        B1[0] = errors;

                        double[][] res = MatrixInverse.MatrixInverseProgram.MatrixProduct(Jacobian, B1);

                        grads = MatrixInverse.MatrixInverseProgram.Transpose(res)[0];

                        return true;

                    case enumInverseMethod.Alglib:
                        /*alglib.densesolverlsreport rep;
                        int  info;
                        double [][] A = alglib.rmatrixsolvem(Jacobian, 1, Errors, 2, true, out info, out rep, out grads);
                        double [][] B = new  double [1][];
                        B[0] = errors;

                        double [][] res = MatrixInverse.MatrixInverseProgram.MatrixProduct(Jacobian, B);

                        grads = MatrixInverse.MatrixInverseProgram.Transpose(res)[0];

                        return   true;*/
                        break;

                    case enumInverseMethod.MathNet:
                        var A2 = Matrix<double>.Build.DenseOfRowArrays(Jacobian);
                        var B2 = Vector<double>.Build.DenseOfArray(errors);
                        try
                        {
                            grads = A2.Solve(B2).ToArray();
                        }
                        catch { return false; }
                        return true;
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

        public void AddLLESpecs()
        {
            foreach (TraySection section in column.TraySections)
            {
                foreach (Tray tray in section.Trays)
                {
                    Specification s = new Specification(tray.Name, 0, ePropID.NullUnits, eSpecType.LLE_KSpec, section, tray);
                    column.Specs.Add(s);
                }
            }
        }
    }
}