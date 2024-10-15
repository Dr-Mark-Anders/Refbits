using System;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class Toplevel : ISerializable
    {
        private FlowSheet fs = new FlowSheet();

        public Toplevel()
        {
            fs.AddComponent("n-Butane");
        }

        public FlowSheet Fs { get => fs; set => fs = value; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FS", Fs, typeof(FlowSheet));
        }

        public Toplevel(SerializationInfo info, StreamingContext context)
        {
            Fs = (FlowSheet)info.GetValue("FS", typeof(FlowSheet));
        }
    }
}