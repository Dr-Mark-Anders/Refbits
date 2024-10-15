using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Heater : UnitOperation, ISerializable
    {
        public int NoSides = 1;
        public int NoSegments = 1;
        public bool CounterCurrent = true;
        public bool IsCooler = false;

        public Port_Material PortIn = new("HeaterIn", FlowDirection.IN);
        public Port_Material PortOut = new("HeaterOut", FlowDirection.OUT);
        public Port_Signal DT = new("DeltaT", ePropID.DeltaT, FlowDirection.IN);
        public Port_Energy Q = new("Duty", FlowDirection.OUT);

        public void Initialise()
        {
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(DP);

            Q = new Port_Energy("Q", FlowDirection.IN);
            this.Name = "Heater";

            Add(Q);
        }

        public bool IsConnected
        {
            get
            {
                if (PortIn.IsConnected && PortOut.IsConnected)
                    return true;
                else return false;
            }
        }

        public Heater() : base()
        {
            Initialise();
            this.storedProfiles = new Dictionary<string, List<double>> { };
        }

        public override Port_Material GetPortIn
        {
            get
            {
                return PortIn;
            }
        }

        public override Port_Material GetPortOut
        {
            get
            {
                return PortOut;
            }
        }

        public Heater(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                PortIn = (Port_Material)info.GetValue("HeaterIn", typeof(Port_Material));
                PortOut = (Port_Material)info.GetValue("HeaterOut", typeof(Port_Material));
                DT = (Port_Signal)info.GetValue("DeltaT", typeof(Port_Signal));
                DP = (Port_Signal)info.GetValue("DeltaP", typeof(Port_Signal));
                Q = (Port_Energy)info.GetValue("Q", typeof(Port_Energy));
                Q.Value.DisplayName = "Duty";
            }
            catch { }

            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(DP);
            Add(Q);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("HeaterIn", PortIn);
            info.AddValue("HeaterOut", PortOut);
            info.AddValue("DeltaT", DT);
            info.AddValue("DeltaP", DP);
            info.AddValue("Q", Q, typeof(Port_Energy));
        }

        public Dictionary<string, List<double>> storedProfiles;

        public override bool Solve()
        {
            this.storedProfiles = new Dictionary<string, List<double>> { };

            //this.EraseCalcValues(SourceEnum.PortCalcResult);
            this.EraseValuesCalcFromThisUO();

            this.CalcDT();

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            this.Q.IsDirty = false;
            
            //base.IsSolved = true;
            base.IsDirty = false;
            SetAllUOPortsClean();
            return true;
        }

        public virtual bool CalcDP()
        {
            //# should have proper Pressure  drop calculation
            PortOut.SetPortValue(ePropID.P, PortIn.P_ - DP.Value, SourceEnum.UnitOpCalcResult);
            PortIn.SetPortValue(ePropID.P, PortOut.P_ + DP.Value, SourceEnum.UnitOpCalcResult);
            DP.SetPortValue(ePropID.P, PortIn.P_ - PortOut.P_, SourceEnum.UnitOpCalcResult);
            return true;
        }

        public virtual bool CalcDT()
        {
            //# should have proper Pressure  drop calculation
            var TIn = PortIn.T_;
            var TOut = PortOut.T_;
            double dt = DT.Value;

            if (IsCooler)
                dt = -dt;

            if (TIn.IsFromExternalPort(PortIn) && TOut.IsFromExternalPort(PortOut))
            {
                DT.SetPortValue(ePropID.DeltaT, TOut - TIn, SourceEnum.CalculatedSpec, this);
            }
            else if (DT.IsKnown && TIn.IsFromExternalPort(PortIn))
            {
                if (TIn.IsKnown)
                    PortOut.SetPortValue(ePropID.T, TIn.Value + dt, SourceEnum.UnitOpCalcResult, this);
                else
                    return false;
            }
            else if (DT.IsKnown && TOut.IsFromExternalPort(PortOut))
            {
                if (TOut.IsKnown)
                    PortIn.SetPortValue(ePropID.T, TOut.Value - dt, SourceEnum.UnitOpCalcResult, this);
                else
                    return false;
            }
            return false;
        }

        internal void ClearIfNotExternallyFlashable()
        {
            PortIn.ClearIfNotExternallyFlashable();
            PortOut.ClearIfNotExternallyFlashable();
        }

        public override bool IsSolved
        {
            get
            {
                return Ports.IsSolved();
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (PortIn.IsDirty)
                    return true;
                if (PortOut.IsDirty)
                    return true;
                return false;
            }
        }
    }
}