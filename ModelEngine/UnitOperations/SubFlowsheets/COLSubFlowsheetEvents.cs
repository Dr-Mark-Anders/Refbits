using ModelEngine.Ports.Events;

namespace ModelEngine
{
    public partial class COlSubFlowSheet : FlowSheet
    {
        public PortList TriggerPorts = new PortList();

        public event SFSChangedEventHandler SFSChangedEvent;

        public delegate void SFSChangedEventHandler(object sender, PropertyEventArgs e);

        private void RaiseSFSChangedEvent(object sender, PropertyEventArgs e)
        {
            SFSChangedEvent?.Invoke(this, e);
        }
    }
}