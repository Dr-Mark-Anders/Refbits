using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Pipe : UnitOperation, ISerializable
    {
        public Port Feed, Product, Energy;

        public Port_Material PortIn = new Port_Material("In1", FlowDirection.ALL);
        public Port_Material PortOut = new Port_Material("Out1", FlowDirection.ALL);
        public Port_Signal DT = new Port_Signal("DeltaT", ePropID.DeltaT, FlowDirection.IN);
        public Port_Energy Q = new Port_Energy("Q", FlowDirection.OUT);
        public Port_Signal Eff = new Port_Signal("Eff", ePropID.NullUnits, FlowDirection.OUT);

        public PipeFittings pipefittingsin = new PipeFittings();
        public PipeFittings pipefittingsout = new PipeFittings();
        public Dictionary<int, List<double>> FlowHeadEff = new Dictionary<int, List<double>>();

        public Pipe() : base()
        {
            Add(Eff);
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(Q);
        }

        public Pipe(SerializationInfo info, StreamingContext context) : base(info, context)
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