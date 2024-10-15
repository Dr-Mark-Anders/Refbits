using ModelEngine;
using Extensions;
using ModelEngine.UnitOperations.RK4New;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;
using Units.UOM;
using static alglib;

public enum SolverMethod{ Euler, RK4 }

public enum RateBasis { time, catalyst };

namespace ModelEngine
{
    [Serializable]
    public class PlugFlowRx : UnitOperation, ISerializable
    {
        public Port Feed, Product, Energy;

        ReactionPackage reactionPackage;

        public Port_Material PortIn = new Port_Material("In1", FlowDirection.ALL);
        public Port_Material PortOut = new Port_Material("Out1", FlowDirection.ALL);
        public Port_Signal DT = new Port_Signal("DeltaT", ePropID.DeltaT, FlowDirection.IN);
        public Port_Energy Q = new Port_Energy("Q", FlowDirection.OUT);
        public Port_Signal Eff = new Port_Signal("Eff", ePropID.NullUnits, FlowDirection.OUT);

        public PipeFittings pipefittingsin = new();
        public PipeFittings pipefittingsout = new();
        public Dictionary<int, List<double>> FlowHeadEff = new();

        private Reactions rxns; private Density CatalystDensity;
        private Mass CatalystMass;
        private MixedComponents mixed;
        private double StepSize;

        public List<double[]> ComProfile = new();

        RateBasis ratebasis =RateBasis.time;

        double Area;
        double Volume;
        double kg_catalyst;
        double StepWt;
        double ResidenceTimeS;

        private Length Diameter = 0.5; //m
        private Length Length = 0.5;   //m

        private double Segments = 25;

        public PlugFlowRx() : base()
        {
            Add(Eff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
        }

        public PlugFlowRx(Reactions rxns, Length Diameter, Length length, Density CatalystDensity, Mass CatalystMass, RateBasis rateBasis = RateBasis.time) : base()
        {
            this.rxns = rxns;
            this.Diameter = Diameter;
            this.Length = length;
            this.CatalystDensity = CatalystDensity;
            this.CatalystMass = CatalystMass;
            this.ratebasis = rateBasis;

            Area = Math.Pow(Diameter / 2.0, 2) * Math.PI;
            Volume = Area * length;
            kg_catalyst = Volume * CatalystDensity._kg_m3;
            StepWt = kg_catalyst / Segments;
            ResidenceTimeS = Volume / PortIn.VF_ / 3600;
            StepSize = ResidenceTimeS / Segments;

            Add(Eff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
        }

        public PlugFlowRx(ReactionPackage pkg, Length Diameter, Length length, Density CatalystDensity, Mass CatalystMass, RateBasis rateBasis = RateBasis.time) : base()
        {
            reactionPackage = pkg;
            this.rxns = pkg.Reactions;
            this.Diameter = Diameter;
            this.Length = length;
            this.CatalystDensity = CatalystDensity;
            this.CatalystMass = CatalystMass;
            this.ratebasis = rateBasis;

            Area = Math.Pow(Diameter / 2.0, 2) * Math.PI;
            Volume = Area * length;
            kg_catalyst = Volume * CatalystDensity._kg_m3;
            StepWt = kg_catalyst / Segments;
            ResidenceTimeS = Volume / PortIn.VF_ / 3600;
            StepSize = ResidenceTimeS / Segments;

            Add(Eff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
        }

        public bool Solve(SolverMethod method = SolverMethod.RK4)
        {
            PortIn.Flash();
            Temperature T = PortIn.T_.BaseValue;
            Pressure P = PortIn.P_.BaseValue-DP.Value/2;
            Components cc = PortIn.cc.Clone();
            MoleFlow flow = PortIn.MolarFlow_.Value;

            rxns.factors.Solve(P);

            //kg_catalyst = Volume * CatalystDensity._kg_m3;

            switch (ratebasis)
            {
                case RateBasis.catalyst:
                    StepSize = CatalystMass / Segments;
                    Segments = CatalystMass;
                    break;
                case RateBasis.time:
                    ResidenceTimeS = Volume / PortIn.VF_ * 3600;
                    StepSize = ResidenceTimeS / Segments;
                    break;
                default:
                    break;
            }           

            RK4New rk = new(ComProfile);

            switch (method)
            {
                case SolverMethod.RK4:
                    rk.SolveRK4(rxns, P, T, PortIn, StepSize, flow);
                    break;

                case SolverMethod.Euler:
                    rk.SolveEuler(PortIn, cc, P, T, PortIn.H_.BaseValue, flow, rxns, 25, 1,true);
                    break;
            }

            //PortOut.T = new StreamProperty(ePropID.T, PortOut.T,SourceEnum.UnitOpCalcResult);
            PortOut.P_ = new StreamProperty(ePropID.P, PortIn.P_-DP.Value, SourceEnum.UnitOpCalcResult);
            PortOut.H_ = new StreamProperty(ePropID.H, PortIn.H_, SourceEnum.UnitOpCalcResult);
            PortOut.MF_ = new StreamProperty(ePropID.MF, PortIn.MF_, SourceEnum.UnitOpCalcResult);
            PortOut.cc = PortIn.cc.Clone();
            PortOut.cc.SetMolFractions(rk.FinalMoleFracs);

            return true;
        }

        public double r(double t)
        {
            Reaction r = rxns[0];
            return 0;
        }

        public PlugFlowRx(SerializationInfo info, StreamingContext context) : base(info, context)
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
            }
            catch { }
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
        }
    }
}