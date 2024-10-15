using MatrixAlgebra;
using ModelEngine.Ports.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class Balanceobject : UnitOperation
    {
        private readonly BalanceModelType type = BalanceModelType.None;

        public Balanceobject(BalanceModelType type = BalanceModelType.None) : base()
        {
            this.type = type;
        }

        public void AddInput(Port_Material port)
        {
            port.flowdirection = FlowDirection.IN;
            Ports.Add(port);
        }

        public void AddOutput(Port_Material port)
        {
            port.flowdirection = FlowDirection.OUT;
            Ports.Add(port);
        }

        public void Balance()
        {
            switch (type)
            {
                case BalanceModelType.Matrix:
                    ModelEngine.Balance.MatrixBalance(this);
                    break;

                case BalanceModelType.Molar:
                    ModelEngine.Balance.FlowBalance(this, ePropID.MOLEF);
                    break;

                case BalanceModelType.Component:
                    ModelEngine.Balance.ComponentBalance(this);
                    break;

                case BalanceModelType.Mass:
                    ModelEngine.Balance.FlowBalance(this, ePropID.MF);
                    break;

                case BalanceModelType.Volume:
                    ModelEngine.Balance.FlowBalance(this, ePropID.VF);
                    break;

                case BalanceModelType.Enthalpy:
                    ModelEngine.Balance.EnergyBalance(this);
                    break;

                default:
                    break;
            }
        }
    }

    public class Balance : UnitOperation
    {
        public static bool Calculate(UnitOperation uo)
        {
            switch (uo)
            {
                case Mixer m:
                    MixerPressureBalance(m);
                    FlowBalanceMoleMassVol(uo);
                    ComponentBalance(uo);
                    EnergyBalance(uo);
                    break;

                case Valve:
                case Heater:
                case Cooler:
                    SimplePressureBalance(uo);
                    FlowBalanceMoleMassVol(uo);
                    ComponentBalance(uo);
                    EnergyBalance(uo);
                    break;

                case Pump:
                    SimplePressureBalance(uo);
                    FlowBalanceMoleMassVol(uo);
                    ComponentBalance(uo);
                    EnergyBalance(uo);
                    break;

                default:
                    SimplePressureBalance(uo);
                    FlowBalanceMoleMassVol(uo);
                    ComponentBalance(uo);
                    EnergyBalance(uo);
                    break;
            }

            return true;
        }

        private static bool FlowBalanceMoleMassVol(UnitOperation uo)
        {
            if (FlowBalance(uo, ePropID.MOLEF))
                return true;
            else if (FlowBalance(uo, ePropID.MF))
                return true;
            else if (FlowBalance(uo, ePropID.VF))
                return true;
            else
                return false;
        }

        public static bool MatrixBalance(UnitOperation uo)
        {
            PortList portsin, portsout, ports, LHPorts = new(), RHPorts = new();
            List<double> RHSflows = new();
            List<int> Sign = new();

            int UnknownOut = 0, UnknownIn = 0;

            portsin = uo.GetPorts(FlowDirection.IN);
            portsout = uo.GetPorts(FlowDirection.OUT);
            ports = uo.GetPorts(FlowDirection.ALL);
            double sumin = 0, sumout = 0;

            if (portsin.Count == 1 && portsout.Count == 1)//Notsuitableformatrixbalance
                return false;

            int noComps = uo.Flowsheet.FlowsheetComponentList.Count;

            foreach (var item in ports)
            {
                if (item.cc.Origin == SourceEnum.Empty)//musthavealltheComponents
                    return false;
            }

            //noofunknownflowsustequalnoofComponents
            List<double[]> LHS = new();//unknownflows
            List<double[]> RHS = new();//Knownflowcomps

            foreach (var item in portsin)
            {
                double MoleF = item.Properties[ePropID.MOLEF];
                if (MoleF.Equals(double.NaN))
                {
                    LHPorts.Add(item);
                    LHS.Add(item.cc.MoleFractions);
                    UnknownIn++;
                    Sign.Add(-1);
                }
                else
                {
                    RHSflows.Add(item.MolarFlow_.Value);
                    RHS.Add(item.MoleFractions);
                    RHPorts.Add(item);
                    sumin += (double)MoleF;
                }
            }

            foreach (var item in portsout)
            {
                double MoleF = item.Properties[ePropID.MOLEF];
                if (double.IsNaN(MoleF))
                {
                    LHS.Add(item.cc.MoleFractions);
                    LHPorts.Add(item);
                    UnknownOut++;
                    Sign.Add(1);
                }
                else
                {
                    sumout += MoleF;
                    RHS.Add(item.MoleFractions);
                    RHPorts.Add(item);
                    RHSflows.Add(-item.MolarFlow_.Value);
                }
            }

            if (UnknownIn + UnknownOut != noComps)
                return false;

            double[][] LHSM = LHS.ToArray();
            double[] RHSUM = new double[noComps];

            List<Port_Material> rhslist = RHPorts.ToList();
            List<Port_Material> LHSPortList = LHPorts.ToList();

            for (int y = 0; y < RHPorts.Count; y++)
            {
                for (int x = 0; x < noComps; x++)
                {
                    RHSUM[y] += RHSflows[x] * rhslist[x].cc[y].MoleFraction;
                }
            }

            Matrix LH = new(LHSM);
            var LHI = LH.Inverse;

            var res = LHI.Transpose().Multiply(new Matrix(RHSUM));

            for (int i = 0; i < LHSPortList.Count; i++)
            {
                LHSPortList[i].SetPortValue(ePropID.MOLEF, res[i, 0] * Sign[i], SourceEnum.UnitOpCalcResult, LHSPortList[i].Guid, uo);
            }

            return true;
        }

        public static bool FlowBalance(UnitOperation uo, ePropID flowID)
        {
            Port EmptyIn = null, EmptyOut = null;
            PortList portsin = new();
            PortList portsout = new();

            if (uo is StreamMaterial sm)
            {
                return true;
            }
            else
            {
                portsin = uo.GetConnectedPorts(FlowDirection.IN);
                portsout = uo.GetConnectedPorts(FlowDirection.OUT);
            }

            if (portsin.Count == 1 && portsout.Count == 1)
            {
                Port_Material In = portsin[0];
                Port_Material Out = portsout[0];

                if (In.Props[flowID].IsFromExternalPort(In))
                    return Out.SetPortValue(flowID, In.Props[flowID], SourceEnum.UnitOpCalcResult,Out.Guid,uo, true);
                else if (Out.Props[flowID].IsFromExternalPort(Out))
                    return In.SetPortValue(flowID, Out.Props[flowID], SourceEnum.UnitOpCalcResult, Out.Guid, uo, true);

                return false;
            }

            int countin = 0, countout = 0;
            double sumin = 0, sumout = 0;

            foreach (var port in portsin)
            {
                double flowIn = port.Props[flowID];
                if (double.IsNaN(flowIn) || port.Props[flowID].IsFromUnitOP)//emptyorsentformUOanyway
                {
                    EmptyIn = port;
                    countin++;
                }
                else
                    sumin += flowIn;
            }

            foreach (var port in portsout)
            {
                double flowOut = port.Props[flowID];
                if (double.IsNaN(flowOut) || port.MolarFlow_.IsFromUnitOP)//emptyorsentformUOanyway
                {
                    EmptyOut = port;
                    countout++;
                }
                else
                    sumout += flowOut;
            }

            if (sumin == sumout)//balanced
                return true;

            if (countin == 1 && countout == 0)
            {
                EmptyIn.SetPortValue(flowID, sumout - sumin, SourceEnum.UnitOpCalcResult, EmptyIn.Guid, uo, false);
                return true;
            }
            else if (countin == 0 && countout == 1)
            {
                EmptyOut.SetPortValue(flowID, sumin - sumout, SourceEnum.UnitOpCalcResult, EmptyOut.Guid, uo, false);
                return true;
            }

            return false;
        }

        public static bool ComponentBalance(UnitOperation uo)//onlyoneflow,andonecomponentlistmustbemissing
        {
            PortList inports = new();
            PortList outports = new();
            int NoComps;

            if (uo is StreamMaterial sm)
            {
                return true;
            }
            else
            {
                inports = uo.GetConnectedPorts(FlowDirection.IN);
                outports = uo.GetConnectedPorts(FlowDirection.OUT);
            }

            PortList UnknownOutPorts = outports.GetPorts_InternalComponents();
            PortList UnknownInPorts = inports.GetPorts_InternalComponents();
            PortList KnownOutPorts = outports.GetPorts_ExternalComponents();
            PortList KnownInPorts = inports.GetPorts_ExternalComponents();
            Port_Material missing = null;

            //if(FlowSheet.SimpleStream&&uoisStreamMaterial)
            //return  true;

            if (UnknownInPorts.Count == 1 && UnknownOutPorts.Count == 0)
                missing = UnknownInPorts.materialList[0];
            else if (UnknownInPorts.Count == 0 && UnknownOutPorts.Count == 1)
                missing = UnknownOutPorts.materialList[0];

            //if (missing != null)
            //    missing.cc.Clear();

            if (inports.Count == 1 && outports.Count == 1)//singlefeed,singleproduct
            {
                TransferComponents(inports[0], outports[0], outports[0].Guid);//trybothways
                TransferComponents(outports[0], inports[0], inports[0].Guid);
                return true;
            }
            else///Multiplefeedsorproducts(componenetsonly)
            {
                Port_Material combinedcompsIn = null, combinedCompsOut = null;

                if (KnownOutPorts.Count != 0)
                {
                    NoComps = outports[0].cc.Count;
                    combinedCompsOut = KnownOutPorts.CombineComponents(NoComps);
                }

                if (KnownInPorts.Count != 0)
                {
                    NoComps = inports[0].cc.Count;
                    combinedCompsOut = KnownInPorts.CombineComponents(NoComps);
                }

                if ((combinedCompsOut is null) && (missing != null) && (combinedcompsIn is not null))
                {
                    //missing.Properties=combinedcompsIn.Properties.Clone();
                    missing.cc = combinedcompsIn.cc.Clone();
                    missing.cc.Origin = SourceEnum.UnitOpCalcResult;
                    missing.SetPortValue(ePropID.MOLEF, combinedcompsIn.MolarFlow_, SourceEnum.UnitOpCalcResult, missing.Guid,uo);
                }
                if ((combinedcompsIn is null) && (missing != null) && (combinedCompsOut is not null))
                {
                    //missing.Properties=combinedCompsOut.Properties.Clone();
                    missing.cc = combinedCompsOut.cc.Clone();
                    missing.cc.Origin = SourceEnum.UnitOpCalcResult;
                    missing.SetPortValue(ePropID.MOLEF, combinedCompsOut.MolarFlow_, SourceEnum.UnitOpCalcResult, missing.Guid, uo);
                }

                if (combinedCompsOut is null || combinedcompsIn is null)
                    return true;

                missing.Deblend(combinedcompsIn, combinedCompsOut);
                return true;
            }
        }

        public static bool EnergyBalance(UnitOperation uo)
        {
            if (uo is StreamEnergy se)
            {
                return true;
            }
            else
            {
                //BalanceResultTypebalancetype=BalanceResultType.None;
                Port_Material EmptyFlowIn = null, EmptyFlowOut = null;
                Port_Energy Eport = null;
                PortList portsin = new(), portsout = new();
                double Sum = double.NaN;
                int countunknown = 0;

                //  portsin.AddRange(uo.GetConnectedPorts(FlowDirection.IN));
                //  portsout.AddRange(uo.GetConnectedPorts(FlowDirection.OUT));

                portsin.AddRange(uo.GetPorts(FlowDirection.IN));
                portsout.AddRange(uo.GetPorts(FlowDirection.OUT));


                List<Port_Energy> Eports = uo.Ports.energyList;

                if (Eports.Count == 1)
                    Eport = Eports[0];

                double duty = double.NaN;

                if (Eport != null && Eport.Value.IsFromExternalPort(Eport))
                    duty = ((EnergyFlow)Eport.Value.UOM).kJ_hr;

                if (portsin.Count == 1 && portsout.Count == 1)//simplecase
                {
                    Port_Material PortIn = portsin[0];
                    Port_Material PortOut = portsout[0];

                    if (Eport != null)//EnergyPortexists
                    {
                        if (!double.IsNaN(PortIn.H_) && !double.IsNaN(PortOut.H_))//Calculateenergystream
                        {
                            if (Eport.flowdirection == FlowDirection.OUT)
                                Eport.SetPortValue(ePropID.EnergyFlow, -(PortOut.EnergyFlow - PortIn.EnergyFlow) / 3600, SourceEnum.UnitOpCalcResult, uo);
                            else
                                Eport.SetPortValue(ePropID.EnergyFlow, (PortOut.EnergyFlow - PortIn.EnergyFlow) / 3600, SourceEnum.UnitOpCalcResult, uo);
                            return true;
                        }
                        if (Eport.Value.IsKnown)
                        {
                            if (!double.IsNaN(PortIn.EnergyFlow))
                            {//kJ/kgmolkJ/hrkWkWtokJ
                                PortOut.SetPortValue(ePropID.H, (PortIn.EnergyFlow - duty) / PortIn.MolarFlow_, SourceEnum.UnitOpCalcResult, uo);
                                return true;
                            }
                            if (!double.IsNaN(PortOut.EnergyFlow))
                            {
                                PortOut.SetPortValue(ePropID.H, (PortOut.EnergyFlow + duty) / PortIn.MolarFlow_, SourceEnum.UnitOpCalcResult, uo);
                                return true;
                            }
                        }
                        return true;//ifithasaheatstreamthenHcannotbetransfeereddirectlybetweeninput/output
                    }
                    else
                    {
                        bool res = false;
                        if (PortIn.H_.IsFromExternalPort(PortIn))
                        {
                            res = PortOut.SetPortValue(ePropID.H, PortIn.H_.BaseValue, SourceEnum.UnitOpCalcResult, uo);
                        }
                        if (!res && PortOut.H_.IsFromExternalPort(PortOut))
                        {
                            res = PortIn.SetPortValue(ePropID.H,PortOut.H_.BaseValue,SourceEnum.UnitOpCalcResult, uo);
                        }
                        return res;
                    }
                }
                else
                {
                    foreach (var item in portsin)
                    {
                        if (double.IsNaN(item.EnergyFlow))
                        {
                            countunknown++;
                            EmptyFlowIn = item;
                        }
                        else
                        {
                            if (double.IsNaN(Sum))
                                Sum = item.EnergyFlow;
                            else
                                Sum += item.EnergyFlow;
                        }
                    }

                    foreach (var item in portsout)
                    {
                        if (double.IsNaN(item.EnergyFlow))
                        {
                            countunknown++;
                            EmptyFlowOut = item;
                        }
                        else
                        {
                            if (double.IsNaN(Sum))
                                Sum = item.EnergyFlow;
                            Sum -= item.EnergyFlow;
                        }
                    }

                    if (countunknown != 1)//canonlybalanceifoneisunknown
                        return false;

                    if (EmptyFlowIn is not null && double.IsNaN(Sum))
                        EmptyFlowIn.SetPortValue(ePropID.H, Sum / EmptyFlowIn.MolarFlow, SourceEnum.UnitOpCalcResult, uo);
                    else if (EmptyFlowOut is not null && !EmptyFlowOut.H_.IsKnown)
                        EmptyFlowOut.SetPortValue(ePropID.H, Sum / EmptyFlowOut.MolarFlow, SourceEnum.UnitOpCalcResult, uo);
                }
            }

            return false;
        }

        public static bool SimplePressureBalance(UnitOperation uo)
        {
            if (uo is StreamEnergy || uo is StreamSignal || uo is StreamMaterial)
            {
                return true;
            }

            StreamProperty dp = new(ePropID.DeltaP, double.NaN);

            if (uo.DP.IsKnown)
                dp = uo.DP.Value;
            else
                dp.Clear();

            PortList PortsIn = uo.GetPorts(FlowDirection.IN); // get the port on the unit op, not the connected port
            PortList PortsOut = uo.GetPorts(FlowDirection.OUT);

            if (PortsOut.Count == 1 && PortsIn.Count == 1)//e.g.pump,valve,heater,cooleretc
            {
                Port_Material In = PortsIn.GetFirst;
                Port_Material Out = PortsOut.GetFirst;

                if (In.P_.IsFromExternalPort(In) && Out.P_.IsFromExternalPort(Out) && !dp.IsInput)
                {
                    if (uo.DP.flowdirection != FlowDirection.Up)
                    {
                        dp.Value = In.P_.Value - Out.P_.Value;
                        uo.DP.SetPortValue(dp.Propid, dp.Value, SourceEnum.UnitOpCalcResult, dp.Guid, uo);
                    }
                    else
                    {
                        dp.Value = Out.P_.Value - In.P_.Value;
                        uo.DP.SetPortValue(dp.Propid, dp.Value, SourceEnum.UnitOpCalcResult, dp.Guid, uo);
                    }
                }
                else if (In.P_.IsFromExternalPort(In) && uo.DP.IsKnown)
                    Out.SetPortValue(ePropID.P, In.P_ - dp, SourceEnum.UnitOpCalcResult, dp.Guid, uo);

                else if (Out.P_.IsFromExternalPort(Out) && uo.DP.IsKnown)
                    In.SetPortValue(ePropID.P, Out.P_ + dp, SourceEnum.UnitOpCalcResult, dp.Guid, uo);

                else if(In.P_.IsFromExternalPort(In) && !uo.DP.IsKnown)
                    Out.SetPortValue(ePropID.P, double.NaN, SourceEnum.Empty, uo);
            }
            return true;   //alldone
        }

        public static bool MixerPressureBalance(UnitOperation uo)
        {
            if (uo is StreamEnergy || uo is StreamSignal)
            {
                return true;
            }

            PortList PortsIn = uo.GetPorts(FlowDirection.IN);
            PortList PortsOut = uo.GetPorts(FlowDirection.OUT);

            if (PortsOut.Count == 0 || PortsIn.Count == 0)
                return false;

            Pressure LowestInP = PortsIn[0].P_.BaseValue;

            for (int i = 0; i < PortsIn.Count; i++)
            {
                if (PortsIn[i].P_.BaseValue < LowestInP)
                    LowestInP = PortsIn[i].P_.BaseValue;
            }

            PortsOut[0].P_.BaseValue = LowestInP;
            PortsOut[0].P_.origin = SourceEnum.UnitOpCalcResult;

            return true;//alldone
        }

        public static void TransferComponents(Port_Material SourcePort, Port_Material DestinationPort, Guid Guid)
        {
            if (SourcePort is null || DestinationPort is null)
                return;

            switch (DestinationPort.cc.Origin)
            {
                case SourceEnum.Empty:
                case SourceEnum.Transferred:
                    if (!SourcePort.cc.IsConsistent(DestinationPort.cc))
                    {
                        TransferComps(SourcePort, DestinationPort);
                        DestinationPort.ClearPortCalcValues();//Componentschangedsoallcalcvaluesinvalid
                        DestinationPort.IsFlashed = false;
                        DestinationPort.cc.OriginPortGuid = Guid;
                        DestinationPort.RaiseCompositionChanged(DestinationPort, new CompositionEventArgs(DestinationPort,DestinationPort.Owner,DestinationPort));
                    }
                    break;

                case SourceEnum.UnitOpCalcResult:
                case SourceEnum.Input:

                    if (!DestinationPort.HasEqualProps(SourcePort))
                    {
                        DestinationPort.cc.Clear();
                        DestinationPort.cc.Add(SourcePort.cc.Clone());
                    }

                    if (!SourcePort.cc.IsConsistent(DestinationPort.cc))
                        for (int i = 0; i < SourcePort.cc.Count; i++)
                            FlowSheet.InconsistencyStack.Add(new InconsistencyObject(SourcePort.cc[i], ePropID.MOLEF, SourcePort, DestinationPort));
                    break;

                case SourceEnum.CalcEstimate:
                default:
                    break;
            }
        }

        private static void TransferComps(Port_Material SourcePort, Port_Material DestinationPort)
        {
            FlowSheet ConnectobjectFlowsheet = DestinationPort.Owner.Flowsheet;

            if (DestinationPort.cc.Count == SourcePort.cc.Count)
                for (int i = 0; i < SourcePort.cc.Count; i++)
                    DestinationPort.cc[i] = SourcePort.cc[i].Clone();//transferanychangedvaluesetc
            else
            {
                DestinationPort.cc.Clear();//Compenentsdon'tmatch
                for (int i = 0; i < SourcePort.cc.Count; i++)
                    DestinationPort.cc.Add(SourcePort.cc[i].Clone());
            }

            DestinationPort.cc.OriginPortGuid = SourcePort.Guid;
            DestinationPort.cc.ThermoLiq = SourcePort.cc.ThermoLiq;
            DestinationPort.cc.ThermoVap = SourcePort.cc.ThermoVap;
            DestinationPort.cc.Origin = SourceEnum.UnitOpCalcResult;
        }
    }
}