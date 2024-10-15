using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngine
{
    /// <summary>
    ///
    /// </summary>
    [TypeConverter(typeof(ComponentConverter)), Serializable]
    public class StreamComponent : PureComp, IComp, ISerializable
    {
        public StreamComponent()
        {
        }

        public StreamComponent(bool ispure)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
        }

        protected StreamComponent(SerializationInfo info, StreamingContext context)
        {
            try
            {
                Name = (string)info.GetValue("Name", typeof(string));
            }
            catch
            {
                //calctype = enumCalcSeq.BackProp;
            }
        }

        public StreamComponent(int i, bool ispure)
        {
            Props = new double[i];
            this.IsPure = ispure;
        }

        // Meaningful text representation
        public override string ToString()
        {
            return this.Name;
        }

        public class CompChangedEventArgs
        {
            public CompChangedEventArgs(string s)
            { Text = s; }

            public string Text { get; private set; } // readonly
        }

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseCompEvent()
        {
            // Raise the event by using the () operator.
            if (CompEvent != null)
                CompEvent(this, new CompChangedEventArgs("Hello"));
        }

        public delegate void CompEventHandler(object sender, CompChangedEventArgs e);

        public static event CompEventHandler CompEvent;
    }
}