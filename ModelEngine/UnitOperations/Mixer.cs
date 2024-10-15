using System;
using System.Runtime.Serialization;
using Extensions;

namespace ModelEngine
{
    [Serializable]
    public class Mixer : UnitOperation, ISerializable
    {
        public Port_Material PortIn1 = new Port_Material("PortIn1", FlowDirection.IN);
        public Port_Material PortIn2 = new Port_Material("PortIn2", FlowDirection.IN);
        public Port_Material PortIn3 = new Port_Material("PortIn3", FlowDirection.IN);
        public Port_Material PortIn4 = new Port_Material("PortIn4", FlowDirection.IN);
        public Port_Material PortIn5 = new Port_Material("PortIn5", FlowDirection.IN);
        public Port_Material PortOut = new Port_Material("PortOut", FlowDirection.OUT);

        public Mixer() : base()
        {
            Add(PortIn1);
            Add(PortIn2);
            Add(PortIn3);
            Add(PortIn4);
            Add(PortIn5);
            Add(PortOut);
        }

        public Mixer(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn1 = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortIn2 = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            PortIn3 = (Port_Material)info.GetValue("Out2", typeof(Port_Material));
            PortIn4 = (Port_Material)info.GetValue("Out3", typeof(Port_Material));
            PortIn5 = (Port_Material)info.GetValue("Out4", typeof(Port_Material));
            PortOut = (Port_Material)info.GetValue("Out5", typeof(Port_Material));

            Add(PortIn1);
            Add(PortIn2);
            Add(PortIn3);
            Add(PortIn4);
            Add(PortIn5);
            Add(PortOut);
        }

        public override bool Solve()
        {
            Balance.Calculate(this);

            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            PortOut.Flash();

            return true;
        }
    }
}