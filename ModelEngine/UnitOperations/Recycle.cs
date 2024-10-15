using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class Recycle : UnitOperation, ISerializable
    {
        // public  Port estimates = new  Port();
        public int RecCounter = 0;

        public Port_Material PortIn = new Port_Material("PortIn", FlowDirection.IN);
        public Port_Material PortOut = new Port_Material("PortOut", FlowDirection.OUT);

        public Recycle() : base()
        {
            Add(PortIn);
            Add(PortOut);
        }

        protected Recycle(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                PortIn = (Port_Material)info.GetValue("PortIn", typeof(Port_Material));
                PortOut = (Port_Material)info.GetValue("PortOut", typeof(Port_Material));
            }
            catch { }

            Add(PortIn);
            Add(PortOut);
        }

        public override void EraseCalcValues(SourceEnum origin)
        {
            PortIn.Props.Clear();
            PortOut.Props.Clear();
            IsDirty = true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PortIn", PortIn, typeof(Port_Material));
            info.AddValue("PortOut", PortOut, typeof(Port_Material));
        }

        public bool isSolved()
        {
            Port_Material sm = (Port_Material)PortOut;
            if (sm != null)
            {
                if (PortIn.MolarFlow_.CheckRecycleConsistency(sm.MolarFlow_) &&
                PortIn.H_.CheckRecycleConsistency(sm.H_) &&
                PortIn.S_.CheckRecycleConsistency(sm.S_) &&
                PortIn.T_.CheckRecycleConsistency(sm.T_) &&
                PortOut.cc.IsConsistent(PortIn.cc) &&
                PortOut.HasEqualProps(PortIn))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public override bool Solve()
        {
            RecCounter++;

            if (!PortIn.IsSolved)
                return false;

            if (PortOut != null)
            {
                foreach (StreamProperty sp in PortIn.Properties.Props.Values)
                {
                    if (sp.IsKnown)
                    {
                        switch (sp.Property)
                        {
                            case Units.ePropID.MOLEF:
                            case Units.ePropID.H:
                            case Units.ePropID.P:
                                PortOut.Props[sp.Propid].Clear();
                                PortOut.SetPortValue(sp.Propid, sp.BaseValue, SourceEnum.Input);
                                break;

                            default:
                                PortOut.Props[sp.Propid].Clear();
                                PortOut.SetPortValue(sp.Propid, sp.BaseValue, SourceEnum.UnitOpCalcResult);
                                PortOut.SetPortValue(sp.Propid, sp.BaseValue, SourceEnum.UnitOpCalcResult);
                                break;
                        }
                    }
                    else
                    {
                        //OutPort.Props[sp.Propid].Clear();
                        PortOut.Props[sp.Propid].Clear();
                    }
                }

                if (PortIn.cc.IsDefined)
                {
                    PortOut.cc.Clear();
                    PortOut.cc.Add(PortIn.cc.Clone());
                    PortOut.cc.Origin = SourceEnum.Input;
                }
                else
                {
                    PortOut.cc.Clear();
                }
            }

            return true;
        }

        internal void EraseAllValues()
        {
            //PortIn.ClearConnections();
            //PortOut.ClearConnections();
        }
    }
}