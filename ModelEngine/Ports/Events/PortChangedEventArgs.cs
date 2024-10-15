using System;

namespace ModelEngine.Ports.Events
{
    public class PropertyEventArgs : EventArgs
    {
        public UnitOperation uo;
        public object ObjectPort;
        public StreamProperty prop;
        public Port StreamPort;

        public PropertyEventArgs(object sender, UnitOperation uo, StreamProperty prop, Port streamport)
        {
            this.ObjectPort = sender;
            this.uo = uo;
            this.prop = prop;
            this.StreamPort = streamport;
        }
    }

    public class CompositionEventArgs : EventArgs
    {
        public UnitOperation uo;
        public object ObjectPort;
        public Port StreamPort;

        public CompositionEventArgs(object sender, UnitOperation uo, Port streamport)
        {
            this.ObjectPort = sender;
            this.uo = uo;
            this.StreamPort = streamport;
        }
    }

    public class UOChangedEventArgs : EventArgs
    {
        public Port port;

        public UOChangedEventArgs(Port port)
        {
            this.port = port;
        }
    }
}