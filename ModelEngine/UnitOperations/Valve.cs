using System;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Valve : UnitOperation, ISerializable
    {
        public Port_Material PortIn = new Port_Material("In1", FlowDirection.IN);
        public Port_Material PortOut = new Port_Material("Out1", FlowDirection.OUT);
        public Port_Signal DT = new Port_Signal("DeltaT", ePropID.DeltaT, FlowDirection.IN);

        public Valve(string Name = "") : base()
        {
            this.Name = Name;
            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);
        }

        public Valve(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PortIn = (Port_Material)info.GetValue("In1", typeof(Port_Material));
            PortOut = (Port_Material)info.GetValue("Out1", typeof(Port_Material));
            DT = (Port_Signal)info.GetValue("DeltaT", typeof(Port_Signal));
            DP = (Port_Signal)info.GetValue("DeltaP", typeof(Port_Signal));

            Add(DP);
            Add(PortIn);
            Add(PortOut);
            Add(DT);

            PortIn.flowdirection = FlowDirection.IN;
            PortOut.flowdirection = FlowDirection.OUT;
        }

        private void PortIn_ValueChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", PortIn);
            info.AddValue("Out1", PortOut);
            info.AddValue("DeltaT", DT);
            info.AddValue("DeltaP", DP);
        }

        public override bool Solve()
        {
            Port_Material PortIn = this.Ports["In1"];
            Port_Material PortOut = this.Ports["Out1"];

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
                Balance.Calculate(this);
            //();

            //PortOut.SetPortValue(ePropID.H, PortIn.H_, SourceEnum.UnitOpCalcResult, this);
            //PortIn.SetPortValue(ePropID.H, PortOut.H_, SourceEnum.UnitOpCalcResult, this);

            DT.SetPortValue(ePropID.DeltaT, this.PortOut.T_ - this.PortIn.T_, SourceEnum.UnitOpCalcResult, this);

            //CalcDP();

            //DP.SetPortValue(ePropID.DeltaT, this.PortOut.P_ - this.PortIn.P_, SourceEnum.UnitOpCalcResult, this.Guid);

            if (PortIn.IsSolved && PortOut.IsSolved)
            {
                //IsSolved = true;
                return true;
            }
            else
            {
               // IsSolved = false;
                return false;
            }
        }

        public virtual bool CalcDP()
        {
            //# should have proper Pressure  drop calculation
            if(DP.IsKnown) 
            { }
            PortOut.SetPortValue(ePropID.P, PortIn.P_ - DP.Value, SourceEnum.UnitOpCalcResult);
            PortIn.SetPortValue(ePropID.P, PortOut.P_ + DP.Value, SourceEnum.UnitOpCalcResult);
            DP.SetPortValue(ePropID.P, PortIn.P_ - PortOut.P_, SourceEnum.UnitOpCalcResult);
            return true;
        }

        public override bool IsSolved
        {
            get
            {
                if (PortIn.IsSolved && PortOut.IsSolved)
                {
                    return true;
                }
                return false;
            }
        }

    }
}