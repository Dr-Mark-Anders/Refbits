using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    internal enum SignalType
    { None, StreamMainProp, StreamGeneralProps, UnitOProp }

    [Serializable]
    public class StreamSignal : BaseStream, ISerializable
    {
        public Port_Signal Port = new Port_Signal("MainSignalPort");
        public Port_Material MaterialPort = new Port_Material("MainStreamPort");

        public StreamSignal() : base()
        {
            Add(Port);
            Add(MaterialPort);
        }

        protected StreamSignal(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
            try
            {
                Port = (Port_Signal)info.GetValue("SPort", typeof(Port_Signal));
                MaterialPort = (Port_Material)info.GetValue("MPort", typeof(Port_Material));
            }
            catch
            {
                Port = new Port_Signal("MainSignalPort");
                MaterialPort = new Port_Material("MainStreamPort");
            }
        }

        [OnDeserialized]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            Add(Port);
            Add(MaterialPort);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SPort", Port);
            info.AddValue("MPort", MaterialPort);
        }

        public override bool Solve()
        {
            return true;
        }

        public bool Equals(StreamSignal other)
        {
            if (other is null)
                return false;
            if (this.Port.Value == other.Port.Value)
                return true;
            return false;
        }

        public static bool operator ==(StreamSignal point1, StreamSignal point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(StreamSignal point1, StreamSignal point2)
        {
            return !point1.Equals(point2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}