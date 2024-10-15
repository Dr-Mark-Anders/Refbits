using ModelEngine.Ports.Events;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    public partial class PortList : IEnumerable<Port_Material>, ISerializable
    {
        public event PortChangedEventHandler PortListChangedEvent;

        public delegate void PortChangedEventHandler(object sender, PropertyEventArgs e);

        private void PortListChanged(object sender, PropertyEventArgs e)
        {
            PortListChangedEvent?.Invoke(sender, e);
        }
    }
}