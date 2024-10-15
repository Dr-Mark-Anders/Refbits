using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Port_Energy : Port
    {
        private StreamProperty _value = new StreamProperty(ePropID.EnergyFlow);

        [Browsable(false)]
        public StreamProperty Value
        {
            get
            {
                if (base.StreamPort is Port_Energy pe)
                    return pe._value;
                return _value;
            }
            set
            {
                if (base.StreamPort is Port_Energy pe)
                    pe._value = value;
                _value = value;
            }
        }

        public Port_Energy(string Name, FlowDirection fd) : base(Name, fd)
        {
            this.Name = Name;
            this.Value.DisplayName = Name;
            flowdirection = fd;
        }

        public Port_Energy(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Value = (StreamProperty)info.GetValue("value", typeof(StreamProperty));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("value", Value, typeof(StreamProperty));
        }

        public Port_Energy()
        {
        }

        public override bool IsFixed
        {
            get
            {
                if (Value is null)
                    return false;
                else
                    switch (Value.origin)
                    {
                        case SourceEnum.Input:
                            //case SourceEnum.FixedEstimate:
                            return true;
                    }
                return false;
            }
        }

        public bool IsKnown
        {
            get
            {
                return Value.IsKnown;
            }
        }

        public bool IsSolved
        {
            get
            {
                if (Value.IsKnown)
                    return true;
                else 
                    return false;
            }
        }

        public void SetValue(ePropID energy, double val, SourceEnum origin)
        {
            Value = new StreamProperty(energy, val, origin);
        }

        public override string ToString()
        {
            return Name;
        }

        internal void LinkPorts(Port_Energy Q)
        {
            this.StreamPort = Q;
            this.StreamPort.PropertyChangedHandler += PropertyChanged;
        }
    }
}