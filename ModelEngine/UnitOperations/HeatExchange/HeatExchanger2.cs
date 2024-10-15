using RefBitsEquationSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public partial class HeatExchanger2 : HXSubFlowSheet, ISerializable, ISolveable, ICloneable
    {
        private SpecType solvetype = SpecType.None;

        public Port_Material ColdIn = new("coldPortIn", FlowDirection.IN);
        public Port_Material ColdOut = new("coldPortOut", FlowDirection.OUT);
        public Port_Material HotIn = new("hotPortIn", FlowDirection.IN);
        public Port_Material HotOut = new("hotPortOut", FlowDirection.OUT);

        public Port_Material tubePortIn = new("tubePortIn", FlowDirection.IN);
        public Port_Material tubePortOut = new("tubePortOut", FlowDirection.OUT);
        public Port_Material shellPortIn = new("shellPortIn", FlowDirection.IN);
        public Port_Material shellPortOut = new("shellPortOut", FlowDirection.OUT);

        public Port_Material tubePortInCopy = new("tubePortInCopy", FlowDirection.IN);
        public Port_Material tubePortOutCopy = new("tubePortOutCopy", FlowDirection.OUT);
        public Port_Material shellPortInCopy = new("shellPortInCopy", FlowDirection.IN);
        public Port_Material shellPortOutCopy = new("shellPortOutCopy", FlowDirection.OUT);

        public SpecClass ExSpecs = new();

        public Heater heater = new();
        public Cooler cooler = new();

        public Port_Signal UA = new("UA", ePropID.UA, FlowDirection.ALL);
        public Port_Energy Q = new("QExch", FlowDirection.ALL);
        public Port_Signal LMTD = new("LMTD", ePropID.DeltaT, FlowDirection.ALL);

        public Port_Signal deltaTShellSide = new("DT Shell Side", ePropID.DeltaT, FlowDirection.ALL);
        public Port_Signal deltaTTubeSide = new("DT Tube Side", ePropID.DeltaT, FlowDirection.ALL);
        public Port_Signal deltaTApproach = new("Approach T", ePropID.DeltaT, FlowDirection.ALL);

        public Port_Signal deltaPShellSide = new("DP Shell Side", ePropID.DeltaP, FlowDirection.ALL);
        public Port_Signal deltaPTubeSide = new("DP Tube Side", ePropID.DeltaP, FlowDirection.ALL);

        private double UATemp = 0, LMTDTemp = 0;

        public bool CounterCurrent { get; set; }

        public double[] XInitial
        {
            get
            {
                List<double> res = new();
                res.Add(0.1);
                return res.ToArray();
            }
        }

        private double[] xFinal;

        public double[] XFinal
        {
            get
            {
                return xFinal;
            }
            set
            {
                xFinal = value;
            }
        }

        public double lastLMTD;
        public double tinydt;
        private bool SimpleIsSolved = false;
        private bool SimpleIsDirty = false;
        private readonly Variables vars = new();
        private bool issimple;
        private bool ishotside;

        public override bool IsSolved
        {
            get
            {
                SimpleIsSolved = false;
                issimple = false;
                ishotside = false;

                if (heater.PortIn.IsConnected && heater.PortOut.IsConnected)
                {
                    if (!cooler.PortIn.IsConnected && !cooler.PortOut.IsConnected)
                    {
                        issimple = true;
                        ishotside = false;
                    }
                }
                else if (cooler.PortIn.IsConnected && cooler.PortOut.IsConnected)
                {
                    if (!heater.PortIn.IsConnected && !heater.PortOut.IsConnected)
                    {
                        issimple = true;
                        ishotside = true;
                    }
                }

                if (issimple && ishotside && cooler.IsSolved)
                {
                    this.SimpleIsSolved = true;
                    this.SimpleIsDirty = false;
                }
                else if (issimple && heater.IsSolved)
                {
                    this.SimpleIsSolved = true;
                    this.SimpleIsDirty = false;
                }

                if (SimpleIsSolved)
                    return true;

                foreach (UnitOperation uo in ModelStack)
                {
                    if (!uo.IsSolved)
                        return false;
                }
                return true;
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (SimpleIsDirty)
                    return true;

                foreach (UnitOperation uo in ModelStack)
                {
                    if (uo.IsDirty)
                        return true;
                }
                return false;
            }
        }

        public void Initialise()
        {
            this.Add(heater);
            this.Add(cooler);

            // export the flow ports
            //this.coldPortIn = this.coldSide.PortIn;
            //this.hotPortIn = this.hotSide.PortIn;

            // this.coldPortOut = this.coldSide.PortOut;
            // this.hotPortOut = this.hotSide.PortOut;

            // connect the energy ports
            heater.Q.LinkPorts(Q);
            cooler.Q.LinkPorts(Q);
            // heater.DT = this.deltaTColdSide;
            // cooler.DT = this.deltaTHotSide;

            // export the delta P ports
            //this.deltaPShellSide = cooler.DP;
            //this.deltaPTubeSide = heater.DP;

            // this.deltaTHotSide = cooler.DT;
            // this.deltaTColdSide = heater.DT;

            // default parameters
            this.CounterCurrent = true;
            this.Add(UA);
            this.Add(LMTD);
            this.Add(deltaTApproach);

            this.Add(deltaTTubeSide);
            this.Add(deltaTShellSide);

            /*  this.Add(ColdIn);
              this.Add(HotIn);
              this.Add(ColdOut);
              this.Add(HotOut);*/

            this.Add(Q);

            this.Add(tubePortIn);
            this.Add(tubePortOut);
            this.Add(shellPortIn);
            this.Add(shellPortOut);

            this.lastLMTD = double.NaN;
            this.tinydt = 1E-05;

            vars.Add(new Variable(heater.Q.Value));
        }

        public HeatExchanger2(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.ModelStack.Clear();
            UA = (Port_Signal)info.GetValue("UA", typeof(Port_Signal));
            LMTD = (Port_Signal)info.GetValue("LMTD", typeof(Port_Signal));

            try
            {
                deltaTApproach = (Port_Signal)info.GetValue("deltaTApproach", typeof(Port_Signal));
                deltaPShellSide = (Port_Signal)info.GetValue("deltaPShellSide", typeof(Port_Signal));
                deltaPTubeSide = (Port_Signal)info.GetValue("deltaPTubeSide", typeof(Port_Signal));
                deltaTTubeSide = (Port_Signal)info.GetValue("deltaTTubeSide", typeof(Port_Signal));
                deltaTShellSide = (Port_Signal)info.GetValue("deltaTShellSide", typeof(Port_Signal));

                tubePortIn = (Port_Material)info.GetValue("tubePortIn", typeof(Port_Material));
                tubePortOut = (Port_Material)info.GetValue("tubePortOut", typeof(Port_Material));
                shellPortIn = (Port_Material)info.GetValue("shellPortIn", typeof(Port_Material));
                shellPortOut = (Port_Material)info.GetValue("shellPortOut", typeof(Port_Material));
                Q = (Port_Energy)info.GetValue("Q", typeof(Port_Energy));

                ColdIn = (Port_Material)info.GetValue("coldPortIn", typeof(Port_Material));
                HotIn = (Port_Material)info.GetValue("hotPortIn", typeof(Port_Material));
                ColdOut = (Port_Material)info.GetValue("coldPortOut", typeof(Port_Material));
                HotOut = (Port_Material)info.GetValue("hotPortOut", typeof(Port_Material));

                heater = (Heater)info.GetValue("heater", typeof(Heater));
                cooler = (Cooler)info.GetValue("cooler", typeof(Cooler));
            }
            catch
            {
            }

            solveStack.Push(heater);
            solveStack.Push(cooler);

            Initialise();

            ExSpecs.Clear();
            ExSpecs.Add(UA, SpecType.UA);
            ExSpecs.Add(LMTD, SpecType.LMTD);

            ExSpecs.Add(deltaTShellSide, SpecType.ShellSideDT);
            ExSpecs.Add(deltaTTubeSide, SpecType.TubeSideDT);

            ExSpecs.Add(deltaTApproach, SpecType.MinApproachT);
            ExSpecs.Add(Q, SpecType.Duty);

            if (UA.Value.Propid != ePropID.UA) { UA.Value = new StreamProperty(ePropID.UA, UA.Value, UA.Value.origin); }
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("UA", UA);
            info.AddValue("LMTD", LMTD);
            info.AddValue("deltaTApproach", deltaTApproach);
            info.AddValue("deltaPShellSide", deltaPShellSide);
            info.AddValue("deltaPTubeSide", deltaPTubeSide);
            info.AddValue("deltaTShellSide", deltaTShellSide);
            info.AddValue("deltaTTubeSide", deltaTTubeSide);

            info.AddValue("coldPortIn", heater.PortIn);
            info.AddValue("hotPortIn", cooler.PortIn);
            info.AddValue("coldPortOut", heater.PortOut);
            info.AddValue("hotPortOut", cooler.PortOut);

            info.AddValue("tubePortIn", tubePortIn);
            info.AddValue("tubePortOut", tubePortOut);
            info.AddValue("shellPortIn", shellPortIn);
            info.AddValue("shellPortOut", shellPortOut);

            info.AddValue("heater", heater);
            info.AddValue("cooler", cooler);

            info.AddValue("Q", Q);

            //info.AddValue("SPECS", ExSpecs);
        }

        public HeatExchanger2()
        {
            ExSpecs.Clear();
            ExSpecs.Add(UA, SpecType.UA);
            ExSpecs.Add(LMTD, SpecType.LMTD);

            ExSpecs.Add(deltaTShellSide, SpecType.ShellSideDT);
            ExSpecs.Add(deltaTTubeSide, SpecType.TubeSideDT);

            ExSpecs.Add(deltaTApproach, SpecType.MinApproachT);

            Initialise();
            this.Name = "Exchanger SFS";
        }

        public SpecType CheckSpecs()
        {
            return ExSpecs.GetSolveType();
        }

        public override void ResetSpecs()
        {
            for (int i = 0; i < ExSpecs.Count; i++)
            {
                ExcSpec ex = ExSpecs[i];
                if (ex != null && double.IsNaN(ex.value.BaseValue))
                {
                    ex.value.Clear();
                }
            }
        }

        public SpecType SolveType
        {
            get
            {
                heater.CalcDT();
                cooler.CalcDT();
                return CheckSpecs();
            }
        }

        public override bool Solve(bool singlestep = false)
        {
            //ClearIfNotExternallyFlashable();
            base.ClearInternals();
            ExSpecs.EraseCalcValues();

            tubePortIn.Flash();
            tubePortOut.Flash();
            shellPortIn.Flash();
            shellPortOut.Flash();

            tubePortInCopy = tubePortIn.Clone();
            tubePortOutCopy = tubePortOut.Clone();
            shellPortInCopy = shellPortIn.Clone();
            shellPortOutCopy = shellPortOut.Clone();

            switch (IdentifyHotSide())
            {
                case HotSide.tube:
                    cooler.PortIn = tubePortIn;
                    cooler.PortOut = tubePortOut;
                    heater.PortIn = shellPortIn;
                    heater.PortOut = shellPortOut;

                    heater.DP.SetPortValue(ePropID.DeltaP, deltaPShellSide.Value, deltaPShellSide.Value.origin);
                    cooler.DP.SetPortValue(ePropID.DeltaP, deltaPTubeSide.Value, deltaPTubeSide.Value.origin);

                    if (!deltaTTubeSide.IsKnown)
                        cooler.DT.SetPortValue(ePropID.DeltaT, deltaTTubeSide.Value, SourceEnum.CalculatedSpec, cooler);

                    if (!deltaTShellSide.IsKnown)
                        heater.DT.SetPortValue(ePropID.DeltaT, deltaTShellSide.Value, SourceEnum.CalculatedSpec, heater);

                    break;

                case HotSide.shell:
                    cooler.PortIn = shellPortIn;
                    cooler.PortOut = shellPortOut;
                    heater.PortIn = tubePortIn;
                    heater.PortOut = tubePortOut;

                    cooler.DP.SetPortValue(ePropID.DeltaP, deltaPShellSide.Value, deltaPShellSide.Value.origin);
                    heater.DP.SetPortValue(ePropID.DeltaP, deltaPTubeSide.Value, deltaPTubeSide.Value.origin);

                    if (deltaTShellSide.IsKnown)
                        cooler.DT.SetPortValue(ePropID.DeltaT, deltaTShellSide.Value, SourceEnum.CalculatedSpec, heater);

                    if (deltaTTubeSide.IsKnown)
                        heater.DT.SetPortValue(ePropID.DeltaT, deltaTTubeSide.Value, SourceEnum.CalculatedSpec, cooler);

                    break;

                case HotSide.TubeIn:
                    cooler.PortIn = tubePortIn;
                    cooler.PortOut = tubePortOut;
                    //heater.PortIn.ConnectPort(shellPortIn);
                    //heater.PortOut.ConnectPort(shellPortOut);

                    cooler.DT = deltaTTubeSide;

                    break;

                case HotSide.TubeOut:
                    heater.PortIn = tubePortIn;
                    heater.PortOut = tubePortOut;
                    //cooler.PortIn.ConnectPort(shellPortIn);
                    //cooler.PortOut.ConnectPort(shellPortOut);

                    heater.DT = deltaTTubeSide;

                    cooler.DP.SetPortValue(ePropID.DeltaP, deltaPShellSide.Value, deltaPShellSide.Value.origin);
                    heater.DP.SetPortValue(ePropID.DeltaP, deltaPTubeSide.Value, deltaPTubeSide.Value.origin);

                    break;

                case HotSide.ShellIn:
                    cooler.PortIn = shellPortIn;
                    cooler.PortOut = shellPortOut;
                    //heater.PortIn.ConnectPort(shellPortIn);
                    //heater.PortOut.ConnectPort(shellPortOut);

                    cooler.DT = deltaTShellSide;
                    break;

                case HotSide.ShellOut:
                    heater.PortIn = shellPortIn;
                    heater.PortOut = shellPortOut;
                    //cooler.PortIn.ConnectPort(shellPortIn);
                    //cooler.PortOut.ConnectPort(shellPortOut);

                    heater.DT = deltaTShellSide;
                    break;

                case HotSide.Unknown:
                    heater.PortIn = tubePortIn;
                    heater.PortOut = tubePortOut;
                    cooler.PortIn = shellPortIn;
                    cooler.PortOut = shellPortOut;

                    break;
            }

            SimpleIsSolved = false;

            if (heater.PortOut.ConnectedPortNext is not null && heater.PortOut.ConnectedPortNext.ConnectedPortNext is not null)
                Debug.Print(heater.PortOut.ConnectedPortNext.ConnectedPortNext.Name);

            //heater.CalcDT();
            //cooler.CalcDT();

            heater.Solve();
            cooler.Solve();

            Balance.ComponentBalance(heater);
            Balance.ComponentBalance(cooler);

            solvetype = CheckSpecs();

            switch (solvetype)
            {
                case SpecType.OverSpecified:  // Try solve anayway and add to inconsistency stack
                    Debugger.Break();
                    MessageBox.Show("Exchanger " + Name + " is Overspecified");
                    return false;

                case SpecType.UnderSpecified:
                    List<string> txt = this.Flowsheet.messages.Lines.ToList();
                    txt.Add("Exchanger " + Name + " is UnderSpecified");
                    this.Flowsheet.messages.Lines=txt.ToArray();
                    return false;

                case SpecType.TubeSideDT:
                    if (!cooler.IsConnected)
                        this.ModelStack.Remove(cooler);
                    heater.DP.SetPortValue(ePropID.DeltaP, deltaPTubeSide.Value, deltaPTubeSide.Value.origin);
                    heater.Solve();

                    tubePortOut.RaiseStreamValueChangedEvent(tubePortOut, null);
                    //shellPortOut.ConnectedPort.RaiseStreamValueChangedEvent(shellPortOut.ConnectedPort, null);
                    break;

                case SpecType.ShellSideDT:
                    if (!heater.IsConnected)
                        this.ModelStack.Remove(heater);
                    cooler.Solve();

                    shellPortOut.ConnectedPortNext.RaiseStreamValueChangedEvent(shellPortOut.ConnectedPortNext, null);
                    break;

                case SpecType.Duty:
                    base.Solve(false); // solve sub-flowsheet
                    LMTD.SetPortValue(ePropID.DeltaT, CalcLMTD(cooler.PortOut.T_, heater.PortIn.T_, cooler.PortIn.T_, heater.PortOut.T_), SourceEnum.UnitOpCalcResult, this);
                    SolveForQandUA(Q.Value, LMTD.Value, UA.Value);
                    SolveOuterParameters();
                    break;

                case SpecType.UA: // UA is more difficult
                    heater.Solve();
                    cooler.Solve();

                    ColdIn = heater.PortIn.CloneShallow();
                    ColdOut = heater.PortOut.CloneShallow();
                    HotIn = cooler.PortIn.CloneShallow();
                    HotOut = cooler.PortOut.CloneShallow();

                    double UASpec = UA.Value;
                    double DTestimate = 1; // to calculate heat capcity and MCP

                    ColdOut.T_ = new(ePropID.T, ColdIn.T_ + DTestimate, SourceEnum.Input);
                    HotOut.T_ = new(ePropID.T, HotIn.T_ - DTestimate, SourceEnum.Input);
                    ColdIn.Flash();
                    HotIn.Flash();
                    ColdOut.Flash(true, enumFlashType.PT);
                    HotOut.Flash(true, enumFlashType.PT);

                    //ColdOut.H = new StreamProperty(ePropID.H, ColdIn.H.BaseValue + Qstart, SourceEnum.Input);
                    //HotOut.H = new StreamProperty(ePropID.H, HotIn.H.BaseValue - Qstart, SourceEnum.Input);
                    //ColdOut.Components.Origin = SourceEnum.Input;
                    //HotOut.Components.Origin = SourceEnum.Input;
                    //ColdOut.Flash();
                    //HotOut.Flash();

                    var res = heater.PortIn.IsDirty;

                    double UACalc;
                    double DTCold = ColdOut.T_ - ColdIn.T_;
                    double DTHot = HotIn.T_ - HotOut.T_;
                    double DTIN = HotIn.T_ - ColdIn.T_;
                    double DTOUT = HotOut.T_ - ColdOut.T_;
                    double DT1, DT2;

                    double MCP1 = (ColdOut.H_ - ColdIn.H_) / DTCold * ColdIn.MolarFlow_;
                    double MCP2 = (HotIn.H_ - HotOut.H_) / DTHot * HotIn.MolarFlow_;

                    double Qest = (Math.Exp(UASpec / MCP2 - UASpec / MCP1) * DTIN - DTIN) 
                        / (Math.Exp(UASpec / MCP2 - UASpec / MCP1) * 1 / MCP2 - 1 / MCP1);

                    if (double.IsNaN(Qest))
                        return false;

                    ColdOut.T_.Clear();
                    HotOut.T_.Clear();

                    for (int i = 0; i < 100; i++)
                    {
                        ColdOut.H_ = new StreamProperty(ePropID.H, ColdIn.H_.BaseValue + Qest / ColdIn.MolarFlow_, SourceEnum.Input);
                        HotOut.H_ = new StreamProperty(ePropID.H, HotIn.H_.BaseValue - Qest / HotIn.MolarFlow_, SourceEnum.Input);
                        ColdOut.Flash(true, enumFlashType.PH);
                        HotOut.Flash(true, enumFlashType.PH);
                        DTCold = ColdOut.T_ - ColdIn.T_;
                        DTHot = HotIn.T_ - HotOut.T_;
                        MCP1 = (ColdOut.H_ - ColdIn.H_) / DTCold * ColdIn.MolarFlow_;
                        MCP2 = (HotIn.H_ - HotOut.H_) / DTHot * HotIn.MolarFlow_;
                        DT1 = HotOut.T_ - ColdIn.T_;
                        DT2 = HotIn.T_ - ColdOut.T_;
                        LMTDTemp = (DT1 - DT2) / Math.Log(DT1 / DT2);
                        Qest = (Math.Exp(UASpec / MCP2 - UASpec / MCP1) * DTIN - DTIN) / (Math.Exp(UASpec / MCP2 - UASpec / MCP1) * 1 / MCP2 - 1 / MCP1);
                        //if(double.IsNaN(Qest))
                        //Debugger.Break();
                        if (double.IsNaN(DT1) || double.IsNaN(DT2) || double.IsNaN(LMTDTemp))
                            break;

                        UACalc = Qest / LMTDTemp;
                        if (Math.Abs((UASpec - UACalc) / UASpec) < 1e-4)
                        {
                            heater.PortOut.ConnectedPortNext.SetPortValue(ePropID.H, ColdOut.H_.BaseValue, SourceEnum.UnitOpCalcResult);
                            cooler.PortOut.ConnectedPortNext.SetPortValue(ePropID.H, HotOut.H_.BaseValue, SourceEnum.UnitOpCalcResult);
                            heater.PortOut.ConnectedPortNext.Flash(true, enumFlashType.PH);
                            cooler.PortOut.ConnectedPortNext.Flash(true, enumFlashType.PH);
                            // Triggers port value chnaged on streamport not on UO port!!

                            Q.SetPortValue(ePropID.EnergyFlow, Qest, SourceEnum.UnitOpCalcResult, this);

                            heater.Solve();
                            //hotSide.Q.SetPortValue(ePropID.EnergyFlow, Q.Value.BaseValue, SourceEnum.Transferred, this.Guid);
                            cooler.Solve();
                            this.solveStack.Push(heater);
                            //base.Solve();
                            LMTD.SetPortValue(ePropID.DeltaT, LMTDTemp, SourceEnum.UnitOpCalcResult, this);

                            heater.IsDirty = false;
                            cooler.IsDirty = false;
                            this.IsDirty = false;
                            break;
                        }
                        else if ((double.IsNaN(UACalc) || UACalc < UASpec) && (DT1 < 0 || DT2 < 0)) // UA Spec is too high
                        {
                            MessageBox.Show(this.Name + " UA Spec is too high", "Exchanger Spec Error");
                            Q.ClearConnections();
                            heater.PortIn.ClearIfNotExternallyFlashable();
                            heater.PortOut.ClearIfNotExternallyFlashable();
                            cooler.PortIn.ClearIfNotExternallyFlashable();
                            cooler.PortOut.ClearIfNotExternallyFlashable();
                            //this.EraseCalcValues(SourceEnum.PortCalcResult);
                            return false;
                        }
                    }
                    break;

                case SpecType.LMTD:
                case SpecType.Flow:
                    heater.Q.SetPortValue(ePropID.EnergyFlow, 0.1, SourceEnum.FixedEstimate, this);
                    {
                        SetEstimateMode();
                        base.Solve(false);
                        if (heater.PortIn.IsSolved && heater.PortOut.IsSolved && cooler.PortIn.IsSolved && cooler.PortOut.IsSolved) // Inner solve must be complete before iterating, estimate DT or Q if UA, or LMTD spec'd etc
                            if (SolveSubFlowsheetOuterLevel())
                            {
                                SetOverwriteEstimatesMode();
                                this.solveStack.Push(heater);
                                base.Solve(false);
                                SolveOuterParameters();
                            }
                            else
                            {
                                return false;
                            }
                        SetNormalMode();
                    }
                    break;
            }

            SolveOuterParameters();
            ExSpecs[SpecType.TubeSideDT].SetProperty((DeltaTemperature)deltaTTubeSide.Value.BaseValue);
            ExSpecs[SpecType.ShellSideDT].SetProperty((DeltaTemperature)deltaTShellSide.Value.BaseValue);
            ExSpecs[SpecType.UA].SetProperty((UA)UA.Value.BaseValue);
            ExSpecs[SpecType.LMTD].SetProperty((DeltaTemperature)LMTD.Value.BaseValue);
            ExSpecs[SpecType.MinApproachT].SetProperty((DeltaTemperature)MinApproach());
            if (ExSpecs.Contains(SpecType.Duty))
                ExSpecs[SpecType.Duty].SetProperty((EnergyFlow)Q.Value.BaseValue);
            return true;
        }

        private void ClearIfNotExternallyFlashable()
        {
            heater.ClearIfNotExternallyFlashable();
            cooler.ClearIfNotExternallyFlashable();
        }

        public override void ClearInternals()  // clear all sub-models
        {
            heater.ClearInternals();
            cooler.ClearInternals();
        }

        // Just need to to delta T stuff since children handle everything else
        public bool SolveSubFlowsheetOuterLevel() // solve sub-flowsheet
        {
            if ((vars.Count == ExSpecs.CountActive())
                && NonLinearSolver.SolveEquations(this, SolverType.NewtonRaphson))

                return true;

            return false;
        }

        public void SolveOuterParameters() // Q Should allways be known if exchanger is solving iteratively
        {
            LMTDTemp = CalcLMTD(cooler.PortOut.T_, heater.PortIn.T_, cooler.PortIn.T_, heater.PortOut.T_);
            UATemp = heater.Q.Value / LMTDTemp;

            switch (solvetype)
            {
                case SpecType.UA: // UA Fixed Input
                    //UA.SetPortValue(ePropID.UA, SourceEnum.CalcResult, UATemp);
                    LMTD.Clear();
                    LMTD.SetPortValue(ePropID.DeltaT, LMTDTemp, SourceEnum.UnitOpCalcResult, this);
                    break;

                case SpecType.LMTD: // LMTD Fixed Input
                    //LMTD.SetPortValue(ePropID.DeltaT, SourceEnum.CalcResult, LMTDTemp);
                    UA.Clear();
                    UA.SetPortValue(ePropID.DeltaT, UATemp, SourceEnum.UnitOpCalcResult, this);
                    break;

                default:
                    LMTD.Clear();
                    LMTD.SetPortValue(ePropID.DeltaT, LMTDTemp, SourceEnum.UnitOpCalcResult, this);
                    UA.Clear();
                    UA.SetPortValue(ePropID.DeltaT, UATemp, SourceEnum.UnitOpCalcResult, this);
                    break;
            }

            calcApproachTs();
        }

        public void EstimateOuterParameters() // Q Should allways be known if exchanger is solving iteratively
        {
            LMTDTemp = CalcLMTD(cooler.PortOut.T_, heater.PortIn.T_, cooler.PortIn.T_, heater.PortOut.T_);
            UATemp = heater.Q.Value / LMTDTemp;

            if (solvetype == SpecType.UA)
                UA.Estimate = new StreamProperty(ePropID.UA, Q.Value / LMTDTemp);
            else if (solvetype == SpecType.LMTD)
                LMTD.Estimate = new StreamProperty(ePropID.DeltaT, LMTDTemp);
        }

        public double[][] CalculateJacobian(double[] x, double[] RHS)
        {
            double[] res = new double[x.Length];
            // vary Q to meet specs
            double[][] jacobian = new double[x.Length][];
            for (int i = 0; i < x.Length; i++)
                jacobian[i] = new double[x.Length];

            for (int i = 0; i < vars.Count; i++)
            {
                vars[i].value = x[i] + 0.1;
                this.solveStack.Push(heater);
                base.Solve();
                EstimateOuterParameters();

                ExcSpec spec = ExSpecs.GetActive()[0];

                res[0] = spec.Error - RHS[0];
                jacobian[i] = res;
            }
            return jacobian;
        }

        public double[] CalculateRHS(double[] x)
        {
            double[] res = new double[x.Length];
            for (int i = 0; i < vars.Count; i++)
                vars[i].value = x[i];

            this.solveStack.Push(heater);
            base.Solve();
            EstimateOuterParameters();

            List<ExcSpec> exspecs = ExSpecs.GetActive();

            for (int i = 0; i < exspecs.Count; i++)
                res[i] = exspecs[i].Error;

            return res;
        }

        public override object Clone()
        {
            HeatExchanger2 hx = new();

            hx.heater.PortIn.SetProperties(heater.PortIn.Properties, this);
            hx.heater.PortOut.SetProperties(heater.PortOut.Properties, this);
            hx.cooler.PortIn.SetProperties(cooler.PortIn.Properties, this);
            hx.cooler.PortOut.SetProperties(cooler.PortOut.Properties, this);

            hx.solveStack.Push(hx.heater);
            hx.solveStack.Push(hx.cooler);

            return hx;
        }
    }
}