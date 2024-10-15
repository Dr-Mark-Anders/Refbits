using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable]
    public class Port_Signal : Port
    {
        private StreamProperty value = new StreamProperty(ePropID.NullUnits, double.NaN);
        internal double scaleFactor = 1;
        private DataStore datastore = new DataStore();

        public Port_Signal(string Name, ePropID prop, FlowDirection fd, double defaultvalue = double.NaN) : base(Name, fd)
        {
            value = new(prop);
            value.BaseValue = defaultvalue;
            value.DisplayName = Name;
            value.Name = Name;
            if (!double.IsNaN(defaultvalue))
                value.origin = SourceEnum.Input;
        }

        public Port_Signal(Port_Energy port) : base(port.Name, port.flowdirection)
        {
            value = port.Value;
        }

        public Port_Signal(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            value = (StreamProperty)info.GetValue("value", typeof(StreamProperty));
            try
            {
                datastore = (DataStore)info.GetValue("DataStore", typeof(DataStore));
            }
            catch
            {
            }
        }

        public Port_Signal(string name = "Signal") : base()
        {
            Name = name;
        }

        public Port_Signal(string name, FlowDirection flowdirection) : base(name, flowdirection)
        {
            this.Name = name;
            this.flowdirection = flowdirection;
        }

        public StreamProperty Value
        {
            get
            {
                if (StreamPort  is Port_Signal ps)
                    return ps.Value;
                return value;
            }
            set
            {
                if (StreamPort is Port_Signal ps)
                    ps.Value = value;
                this.value = value;
            }
        }

        public double Estimate
        {
            get
            {
                return value.estimate;
            }
            set
            {
                this.value.estimate = value;
            }
        }

        public double Error
        {
            get
            {
                return value.estimate - value.BaseValue;
            }
        }

        public bool IsKnown
        {
            get
            {
                if (value is null || double.IsNaN(value.BaseValue))
                    return false;
                else
                    return true;
            }
        }

        public override bool IsFixed
        {
            get
            {
                if (value is null || value.origin != SourceEnum.Input)
                    return false;
                else
                    return true;
            }
        }

        [Browsable(true)]
        public DataStore Datastore { get => datastore; set => datastore = value; }
        public bool IsSolved
        {
            get
            {
                if (value.IsKnown)
                    return true;
                return false;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("value", value, typeof(StreamProperty));
            info.AddValue("DataStore", datastore);
        }

        public override string ToString()
        {
            if (Name is not null)
                return Name.ToString();

            return "NA";
        }

        /*   internal void SetPortValue(Port_Signal value)
           {
               this.value=value.value;
               this.value.origin = value.value.Source;
           }

           internal void SetPortValue(Port_Signal value, SourceEnum origin, Guid OriginGuid)
           {
               this.value = value.value;
               this.value.origin = origin;
               this.value.OriginGuid = OriginGuid;
           }*/
    }
}