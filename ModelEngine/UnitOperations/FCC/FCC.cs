using System;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine.FCC
{
    [Serializable]
    public class FCC : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("HeaterIn", FlowDirection.IN);
        public Port_Material PortOut = new("HeaterOut", FlowDirection.OUT);
        public Port_Signal DT = new("DeltaT", ePropID.DeltaT, FlowDirection.IN);
        public Port_Energy Q = new("Duty", FlowDirection.OUT);

        public FCC() : base()
        {
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(DP);
        }

        public FCC(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public override bool Solve()
        {
            Port InPort = this.Ports["In1"];
            Port OutPort = this.Ports["Out1"];

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            return true;
        }
    }
}