using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class BaseStream : UnitOperation, ISerializable
    {
        private Guid connectedStream;

        public Guid ConnectedStreamGuid { get => connectedStream; set => connectedStream = value; }

        protected BaseStream(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BaseStream() : base()
        {
        }

        /*public virtualPortPort
        {
        get
        {
        return  null;
        }
        }*/
    }
}