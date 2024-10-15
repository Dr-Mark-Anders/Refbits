using ModelEngine.Ports.Events;

namespace ModelEngineTest
{
    public partial class TrayTest
    {
        public event TrayValueChangedEventHandler TrayChanged;

        public delegate void TrayValueChangedEventHandler(object sender, PropertyEventArgs e);

        private void Feed_MainPortValueChanged(object sender, PropertyEventArgs e)
        {
            TrayChanged?.Invoke(sender, e);
        }
    }
}