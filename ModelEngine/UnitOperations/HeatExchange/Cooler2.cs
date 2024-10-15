using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Cooler : UnitOperation, ISerializable
    {
        public int NoSides = 1;
        public int NoSegments = 1;
        public bool CounterCurrent = true;
        public bool IsCooler = false;

        public Port_Material PortIn = new("CoolerIn", FlowDirection.IN);
        public Port_Material PortOut = new("CoolerOut", FlowDirection.OUT);
        public Port_Signal DT = new("DeltaT", ePropID.DeltaT, FlowDirection.IN);

        //public Port_SignalDP=new Port_Signal("DP",ePropID.DeltaP,FlowDirection.ALL);
        public Port_Energy Q = new("Duty", FlowDirection.OUT);

        public void Initialise()
        {
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(DP);

            Q = new Port_Energy("Q", FlowDirection.OUT);
            this.Name = "Cooler";

            Add(Q);
        }

        public Cooler() : base()
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

        public bool IsConnected
        {
            get
            {
                if(PortIn.IsConnected && PortOut.IsConnected)
                    return true;
                else return false;
            }
        }

        public Cooler(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                PortIn = (Port_Material)info.GetValue("CoolerIn", typeof(Port_Material));
                PortOut = (Port_Material)info.GetValue("CoolerOut", typeof(Port_Material));
                DT = (Port_Signal)info.GetValue("DeltaT", typeof(Port_Signal));
                DP = (Port_Signal)info.GetValue("DeltaP", typeof(Port_Signal));
                Q = (Port_Energy)info.GetValue("Q", typeof(Port_Energy));
                Q.Value.DisplayName = "Duty";
            }
            catch
            {
            }
            Add(PortIn);
            Add(PortOut);
            Add(DT);
            Add(DP);
            Add(Q);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("CoolerIn", PortIn);
            info.AddValue("CoolerOut", PortOut);
            info.AddValue("DeltaT", DT);
            info.AddValue("DeltaP", DP);
            info.AddValue("Q", Q, typeof(Port_Energy));
        }

        public Dictionary<string, List<double>> storedProfiles;

        internal static ThermoDynamicOptions GetThermo()
        {
            return new ThermoDynamicOptions();
        }

        public override bool Solve()
        {
            this.storedProfiles = new Dictionary<string, List<double>> { };

            //this.EraseCalcValues(SourceEnum.PortCalcResult);
            this.EraseValuesCalcFromThisUO();
            this.CalcDT();

            Balance.Calculate(this);
            while (FlashAllPorts() > 0)
                Balance.Calculate(this);

            //Handledinbalanceblock
            /*Energyduty=new Energy();
            duty.kJ_hr=(PortOut.EnergyFlow-PortIn.EnergyFlow);//kJ/hr
            Q.SetPropValue(ePropID.ENERGY,SourceEnum.CalcResult,duty);*/
            this.Q.IsDirty = false;
            
            //base.IsSolved = true;
            base.IsDirty = false;
            SetAllUOPortsClean();

            //PortOut.ConnectedPort.RaiseStreamValueChangedEvent(PortOut.ConnectedPort, null);

            return true;
        }

        internal void ClearIfNotExternallyFlashable()
        {
            PortIn.ClearIfNotExternallyFlashable();
            PortOut.ClearIfNotExternallyFlashable();
        }

        public virtual bool CalcDP()
        {
            //#shouldhaveproperPressure dropcalculation
            PortOut.SetPortValue(ePropID.P, PortIn.P_ - DP.Value, SourceEnum.UnitOpCalcResult);
            PortIn.SetPortValue(ePropID.P, PortOut.P_ + DP.Value, SourceEnum.UnitOpCalcResult);
            DP.SetPortValue(ePropID.P, PortIn.P_ - PortOut.P_, SourceEnum.UnitOpCalcResult);
            return true;
        }

        public virtual bool CalcDT()
        {
            //#shouldhaveproperPressure dropcalculation
            var TIn = PortIn.T_;
            var tOut = PortOut.T_;
            double dt = DT.Value;

            if (IsCooler)
                dt = -dt;

            if (TIn.IsFromExternalPort(PortIn) && tOut.IsFromExternalPort(PortOut))
            {
                DT.SetPortValue(ePropID.DeltaT, tOut - TIn, SourceEnum.CalculatedSpec, this);
            }
            else if (DT.Value.IsFromExternalPort(DT) && TIn.IsFromExternalPort(PortIn))
            {
                if (TIn.IsKnown)
                    PortOut.SetPortValue(ePropID.T, TIn.Value + dt, SourceEnum.UnitOpCalcResult, this);
                else
                    return false;
            }
            else if (DT.Value.IsFromExternalPort(DT) && tOut.IsFromExternalPort(PortOut))
            {
                if (tOut.IsKnown)
                    PortIn.SetPortValue(ePropID.T, tOut.Value - dt, SourceEnum.UnitOpCalcResult, this);
                else
                    return false;
            }
            return false;
        }

        public override bool IsSolved
        {
            get
            {
                return Ports.IsSolved();
            }
        }

        /*public override bool IsDirty
        {
        get
        {
        return  Ports.IsDirty();
        }
        }*/

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