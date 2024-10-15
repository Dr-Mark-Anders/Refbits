using Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable]
    public class Pump : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("In1", FlowDirection.IN);
        public Port_Material PortOut = new("Out1", FlowDirection.OUT);
        public Port_Signal DT = new("DeltaT", ePropID.DeltaT, FlowDirection.IN);

        //private  Port_Signal DP = new  Port_Signal("DP", ePropID.DeltaP, FlowDirection.ALL);
        public Port_Energy Q = new("Q", FlowDirection.OUT);

        public Port_Signal Eff = new("Eff", ePropID.NullUnits, FlowDirection.OUT);

        public PipeFittings pipefittingsin = new();
        public PipeFittings pipefittingsout = new();
        public Dictionary<int, List<double>> FlowHeadEff = new();
        public Port_Signal DifferentialHead = new("Head", ePropID.Length, FlowDirection.OUT);
        public Port_Signal TotalHydraulicpower = new("Hydraulic Power", ePropID.EnergyFlow, FlowDirection.OUT);
        private Density SpecificHydraulicpower;

        public Pump() : base()
        {
            Add(Eff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
        }

        public Pump(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortOut = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            DT = (Port_Signal)info.GetValue("DeltaT", typeof(Port_Signal));
            DP = (Port_Signal)info.GetValue("DeltaP", typeof(Port_Signal));
            Q = (Port_Energy)info.GetValue("Q", typeof(Port_Energy));
            Eff = (Port_Signal)info.GetValue("Eff", typeof(Port_Signal));

            Add(Eff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);

            try
            {
                pipefittingsin = (PipeFittings)info.GetValue("pipefittingsin", typeof(PipeFittings));
                pipefittingsout = (PipeFittings)info.GetValue("pipefittingsout", typeof(PipeFittings));
                FlowHeadEff = (Dictionary<int, List<double>>)info.GetValue("FlowHeadEff", typeof(Dictionary<int, List<double>>));
                DifferentialHead = (Port_Signal)info.GetValue("DifferentialHead", typeof(Port_Signal));
                TotalHydraulicpower = (Port_Signal)info.GetValue("Hydraulicpower", typeof(Port_Signal));
            }
            catch { }

            Add(DifferentialHead);
            Add(TotalHydraulicpower);

            PortIn.flowdirection = FlowDirection.IN;
            PortOut.flowdirection = FlowDirection.OUT;
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn);
            info.AddValue("Out1", PortOut);
            info.AddValue("DeltaT", DT);
            info.AddValue("DeltaP", DP);
            info.AddValue("Q", Q, typeof(Port_Energy));
            info.AddValue("Eff", Eff, typeof(Port_Signal));

            info.AddValue("pipefittingsin", pipefittingsin);
            info.AddValue("pipefittingsout", pipefittingsout);
            info.AddValue("FlowHeadEff", FlowHeadEff);
            info.AddValue("DifferentialHead", DifferentialHead);
            info.AddValue("Hydraulicpower", TotalHydraulicpower);
        }

        public override bool Solve()
        {
            DP.flowdirection = FlowDirection.Up;  // display psositve but calcualtion negative to match other case s

            if (DP.Value.origin == SourceEnum.Input)
            {
                PortIn.SetPortValue(ePropID.P, PortOut.P_.Value - DP.Value, SourceEnum.UnitOpCalcResult);
                PortOut.SetPortValue(ePropID.P, PortIn.P_.Value + DP.Value, SourceEnum.UnitOpCalcResult);
            }
            else if (DP.Value.origin == SourceEnum.UnitOpCalcResult)
            {
            }

            Balance.Calculate(this);

            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            pipefittingsin.P = PortIn.P_;
            pipefittingsout.P = PortOut.P_;

            if (!PortOut.P_.IsKnown || !PortIn.P_.IsKnown)
                return false;

            double DifferentialDP = (PortOut.P_ - PortIn.P_);

            DifferentialHead.SetPortValue(ePropID.Length, DifferentialDP
                * 100 / (PortIn.Dens_Act / 1000 * 9.81), SourceEnum.UnitOpCalcResult);

            SpecificHydraulicpower = DifferentialHead.Value * (PortIn.Dens_Act / 1000) * 9.81 / 3600 / (Eff.Value / 100);  // kw

            TotalHydraulicpower.SetPortValue(ePropID.EnergyFlow, SpecificHydraulicpower * PortIn.Vol_Flow_Act, SourceEnum.UnitOpCalcResult);  // kw

            double NPSHavailable = (PortIn.P_ - PortIn.VapourPressure()) * 100000 / (PortIn.Dens_Act * 9.81);

            PortOut.SetPortValue(ePropID.H, PortIn.H_ + SpecificHydraulicpower * 3600, SourceEnum.UnitOpCalcResult);
            PortIn.SetPortValue(ePropID.H, PortOut.H_ - SpecificHydraulicpower * 3600, SourceEnum.UnitOpCalcResult);

            FlashAllPorts();

            DP.SetPortValue(ePropID.DeltaP, PortOut.P_ - PortIn.P_, SourceEnum.UnitOpCalcResult);
            DT.SetPortValue(ePropID.DeltaT, PortOut.T_ - PortIn.T_, SourceEnum.UnitOpCalcResult);
            Q.SetPortValue(ePropID.EnergyFlow, TotalHydraulicpower.Value, SourceEnum.UnitOpCalcResult);

            return true;
        }

        public static double PressureDrop(PipeFittings pf, double Flowrate_m3_s = 22.7, double Density_kg_m3 = 1000, double Viscosity_cp = 1)
        {
            double roughness = 25.4 * 0.0018;
            double MiscDP = 0;
            double EquipDP = 0.34474;
            double TotalDP;

            if (Viscosity_cp < 1 || double.IsNaN(Viscosity_cp))
                Viscosity_cp = 1;

            if (pf.NomPipeSize is null || pf.schedule is null)
            {
                return 0;
            }

            int SizeIndex = pf.PipeIndex.IndexOf(pf.NomPipeSize);
            int scheduleindex = pf.Schedule[pf.schedule];
            if (SizeIndex > 0 && scheduleindex > 0)
            {
                double inside_diamter_mm = pf.PipeinternalSizes[SizeIndex, scheduleindex] * 25.4;

                double Area = Math.PI * (inside_diamter_mm / 1000).Sqr() / 4;
                double Velocity = Flowrate_m3_s / (Area * 3600);
                double Re = (inside_diamter_mm / 1000) * Velocity * Density_kg_m3 * 1000 / Viscosity_cp;
                double e_D = roughness / inside_diamter_mm;
                double A = Math.Pow((-2.457 * Math.Log(Math.Pow((7 / Re), 0.9) + 0.27 * e_D)), 16);
                double B = Math.Pow(37530 / Re, 16);
                double f = 8 * Math.Pow(Math.Pow(8 / Re, 12) + 1 / Math.Pow(A + B, 1.5), 1 / 12D);
                double DP1 = Density_kg_m3 * f * 100 * Velocity.Sqr() / ((inside_diamter_mm / 1000) * 2 * 100000);

                double fittigsloss = FittingsLoss(Re, inside_diamter_mm, pf) * Velocity.Sqr() * Density_kg_m3 / (2 * 100000);

                TotalDP = ((Pressure)pf.P.UOM).BarA + pf.StaticHead * (Density_kg_m3 * 9.81 / 100000)
                    - EquipDP - MiscDP - fittigsloss - DP1 * ((Length)pf.PipeLength.UOM).m / 100;
            }
            else
            {
                TotalDP = 0;
            }
            return TotalDP;
        }

        public static double FittingsLoss(double reynolds, double PipeID, PipeFittings pf)
        {
            double total = 0;

            foreach (var item in pf.FittingTypes)
            {
                switch (item.Key)
                {
                    case "Elbows90LR":
                        total += pf.K(reynolds, PipeID, new Elbows90LR()) * item.Value;
                        break;

                    case "Elbows45LR":
                        total += pf.K(reynolds, PipeID, new Elbows45LR()) * item.Value;
                        break;

                    case "TeesThru":
                        total += pf.K(reynolds, PipeID, new TeesThru()) * item.Value;
                        break;

                    case "TeesBranch":
                        total += pf.K(reynolds, PipeID, new TeesBranch()) * item.Value;
                        break;

                    case "BallValves":
                        total += pf.K(reynolds, PipeID, new BallValves()) * item.Value;
                        break;

                    case "ButterflyValves":
                        total += pf.K(reynolds, PipeID, new ButterflyValves()) * item.Value;
                        break;

                    case "GateValves":
                        total += pf.K(reynolds, PipeID, new GateValves()) * item.Value;
                        break;

                    case "GlobeValves":
                        total += pf.K(reynolds, PipeID, new GlobeValves()) * item.Value;
                        break;

                    case "CheckValves":
                        total += pf.K(reynolds, PipeID, new CheckValves()) * item.Value;
                        break;

                    case "PlugValves":
                        total += pf.K(reynolds, PipeID, new PlugValves()) * item.Value;
                        break;

                    case "PipeEntrance":
                        total += pf.K(reynolds, PipeID, new PipeEntrance()) * item.Value;
                        break;

                    default:
                        break;
                }
            }
            return total;
        }
    }
}