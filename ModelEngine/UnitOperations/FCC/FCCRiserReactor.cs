using ModelEngine;
using Extensions;
using ModelEngine.UnitOperations.RK4New;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;
using Units.UOM;
using static alglib;


namespace ModelEngine
{
    [Serializable]
    public class FCCRiserRx : UnitOperation, ISerializable
    {
        ReactionPackage reactionPackage;
        private Reactions rxns;

        public Port_Material PortIn = new Port_Material("In1", FlowDirection.ALL);
        public Port_Material PortOut = new Port_Material("Out1", FlowDirection.ALL);
        public Port_Signal DT = new Port_Signal("DeltaT", ePropID.DeltaT, FlowDirection.IN);

        private double StepSize;

        public List<double[]> ComProfile = new();

        RateBasis ratebasis =RateBasis.time;

        double StepWt;
        Time ResidenceTimeS;

        private int Segments = 35;

        public FCCRiserRx() : base()
        {
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
        }

        public FCCRiserRx(Reactions rxns, Time ResidenceTime, RateBasis rateBasis = RateBasis.time) : base()
        {
            this.rxns = rxns;
            this.ratebasis = rateBasis;
            ResidenceTimeS = ResidenceTime;
            StepSize = ResidenceTime.S / Segments;

            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
        }

        public bool Solve(SolverMethod method = SolverMethod.RK4)
        {
            PortIn.Flash();
            Temperature T = PortIn.T_.BaseValue;
            Pressure P = PortIn.P_.BaseValue-DP.Value/2;
            Components cc = PortIn.cc.Clone();
            MoleFlow flow = PortIn.MolarFlow_.Value;

            rxns.factors.Solve(P);

            /*switch (ratebasis)
            {
                case RateBasis.catalyst:
                    StepSize = CatalystMass / Segments;
                    Segments = CatalystMass;
                    break;
                case RateBasis.time:
                    ResidenceTimeS = Volume / PortIn.VF * 3600;
                    StepSize = ResidenceTimeS / Segments;
                    break;
                default:
                    break;
            } */          

            RK4New rk = new(ComProfile);

            switch (method)
            {
                case SolverMethod.RK4:
                    rk.SolveRK4(rxns, P, T, PortIn, StepSize, flow);
                    break;

                case SolverMethod.Euler:
                    rk.SolveEuler(PortIn, cc, P, T, PortIn.H_.BaseValue, flow, rxns, Segments, StepSize,false);
                    break;
            }

            PortOut.P_ = new StreamProperty(ePropID.P, PortIn.P_-DP.Value, SourceEnum.UnitOpCalcResult);
            PortOut.H_ = new StreamProperty(ePropID.H, PortIn.H_, SourceEnum.UnitOpCalcResult);
            PortOut.MF_ = new StreamProperty(ePropID.MF, PortIn.MF_, SourceEnum.UnitOpCalcResult);
            PortOut.cc = PortIn.cc.Clone();
            PortOut.cc.SetMolFractionsIncSolids(rk.FinalMoleFracs);

            return true;
        }

        public FCCRiserRx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortOut = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            DT = (Port_Signal)info.GetValue("DeltaT", typeof(Port_Signal));
            DP = (Port_Signal)info.GetValue("DeltaP", typeof(Port_Signal));
         
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn);
            info.AddValue("Out1", PortOut);
            info.AddValue("DeltaT", DT);
            info.AddValue("DeltaP", DP);
        }
    }
}