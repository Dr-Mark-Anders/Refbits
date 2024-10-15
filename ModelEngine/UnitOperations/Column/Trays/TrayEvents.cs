using ModelEngine.Ports.Events;

namespace ModelEngine
{
    public partial class Tray
    {
        public event TrayValueChangedEventHandler TrayChanged;

        public delegate void TrayValueChangedEventHandler(object sender, PropertyEventArgs e);

        private void Feed_MainPortValueChanged(object sender, PropertyEventArgs e)
        {
            TrayChanged?.Invoke(sender, e);
        }
    }
}