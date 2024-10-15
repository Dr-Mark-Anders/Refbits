using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Divider : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new("In1", FlowDirection.IN);
        public Port_Material PortOut1 = new("Out1", FlowDirection.OUT);
        public Port_Material PortOut2 = new("Out2", FlowDirection.OUT);
        public Port_Material PortOut3 = new("Out3", FlowDirection.OUT);
        public Port_Material PortOut4 = new("Out4", FlowDirection.OUT);
        public Port_Material PortOut5 = new("Out5", FlowDirection.OUT);

        public List<Port_Signal> splits = new();

        public Divider() : base()
        {
            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
            Add(PortOut3);
            Add(PortOut4);
            Add(PortOut5);

            for (int i = 0; i < 5; i++)
                splits.Add(new Port_Signal("Split", ePropID.NullUnits, FlowDirection.ALL));
        }

        public Divider(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortOut1 = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            PortOut2 = (Port_Material)info.GetValue("Out2", typeof(Port_Material));
            PortOut3 = (Port_Material)info.GetValue("Out3", typeof(Port_Material));
            PortOut4 = (Port_Material)info.GetValue("Out4", typeof(Port_Material));
            PortOut5 = (Port_Material)info.GetValue("Out5", typeof(Port_Material));

            Add(PortIn);
            Add(PortOut1);
            Add(PortOut2);
            Add(PortOut3);
            Add(PortOut4);
            Add(PortOut5);

            splits = (List<Port_Signal>)info.GetValue("splits", typeof(List<Port_Signal>));
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn, typeof(Port_Material));
            info.AddValue("Out1", PortOut1, typeof(Port_Material));
            info.AddValue("Out2", PortOut2, typeof(Port_Material));
            info.AddValue("Out3", PortOut3, typeof(Port_Material));
            info.AddValue("Out4", PortOut4, typeof(Port_Material));
            info.AddValue("Out5", PortOut5, typeof(Port_Material));

            info.AddValue("splits", splits);
        }

        public override bool Solve()
        {
            PortList products = this.GetPorts(FlowDirection.OUT);
            bool SplitsDefined = true;

            if (PortIn.H_.IsKnown)
                foreach (var item in products)
                    item.SetPortValue(ePropID.H, PortIn.H_.Value, SourceEnum.UnitOpCalcResult);

            if (PortIn.cc.IsDefined)
                foreach (var item in products)
                    item.cc.SetMolFractions(PortIn.cc.MoleFractions, SourceEnum.UnitOpCalcResult);

            if (PortIn.cc.Origin == SourceEnum.Transferred)
            {
                foreach (var p in products)
                {
                    if (p.IsConnected)
                    {
                        p.cc.Clear();
                        for (int j = 0; j < PortIn.cc.Count; j++)
                            p.cc.Add(PortIn.cc[j].Clone());

                        p.cc.Origin = SourceEnum.UnitOpCalcResult;

                        if (PortIn.T_.IsKnown && !p.T_.IsKnown)
                            p.T_ = new StreamProperty(ePropID.T, PortIn.T_, SourceEnum.UnitOpCalcResult);
                    }
                }
            }

            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].IsConnected && !splits[i].IsKnown)
                    SplitsDefined = false;
            }

            if (SplitsDefined)
            {
                if (PortIn.MolarFlow_.IsKnown)
                    for (int i = 0; i < products.Count; i++)

                        products[i].SetPortValue(ePropID.MOLEF, PortIn.MolarFlow_.BaseValue * splits[i].Value, SourceEnum.UnitOpCalcResult);
            }

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
            {
                Balance.Calculate(this);
            }

            return true;
        }
    }
}