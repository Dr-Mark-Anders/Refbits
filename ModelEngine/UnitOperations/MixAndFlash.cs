using System;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class MixAndFlash : FlowSheet, ISerializable
    {
        private Mixer mix = new Mixer();
        private SimpleFlash sf = new SimpleFlash();

        public Port_Material PortIn1 = new("HeaterIn", FlowDirection.IN);
        public Port_Material PortIn2 = new("HeaterIn", FlowDirection.IN);
        public Port_Material PortOut1 = new("HeaterOut", FlowDirection.OUT);
        public Port_Material PortOut2 = new("HeaterOut", FlowDirection.OUT);
        public Port_Signal DT = new("DeltaT", ePropID.DeltaT, FlowDirection.IN);
        public Port_Energy Q = new("Duty", FlowDirection.OUT);

        public MixAndFlash()
        {
            Name = "MixAndFlash";

            Add(PortIn1);
            Add(PortIn2);
            Add(PortOut1);
            Add(PortOut2);
            Add(Q);

            Add(mix, "Mix");
            Add(sf, "Flash");
        }

        public override bool Solve()
        {
            return base.Solve();
        }

        public double[] GetCompositionValues(object i)
        {
            throw new NotImplementedException();
        }

        public string GetPropValue(object i, ePropID t)
        {
            throw new NotImplementedException();
        }
    }
}