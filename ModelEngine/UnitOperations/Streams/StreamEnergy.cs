using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class StreamEnergy : BaseStream, ISerializable
    {
        public Port_Energy Port = new("", FlowDirection.ALL);

        public StreamEnergy() : base()
        {
            Add(Port);
        }

        protected StreamEnergy(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Port = (Port_Energy)info.GetValue("In1", typeof(Port_Energy));
        }

        [OnDeserialized]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            Add(Port);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("In1", Port);
        }

        public override bool Solve()
        {
            Balance.Calculate(this);
            return true;
        }

        public bool Equals(StreamEnergy other)
        {
            if (other is null)
                return false;
            if (Equals(this.Port.Value.BaseValue, other.Port.Value.BaseValue))
                return true;
            return false;
        }

        public static bool operator ==(StreamEnergy point1, StreamEnergy point2)
        {
            return Equals(point1, point2);
        }

        public static bool operator !=(StreamEnergy point1, StreamEnergy point2)
        {
            return !Equals(point1, point2);
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