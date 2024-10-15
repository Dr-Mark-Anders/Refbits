using ModelEngine.Ports.Events;

namespace ModelEngine
{
    public partial class Column
    {
        public event ColumnChangedEventHandler ColumnChanged;

        public delegate void ColumnChangedEventHandler(object sender, PropertyEventArgs e);

        private void TraySection_TrayChanged(object sender, PropertyEventArgs e)
        {
            ColumnChanged?.Invoke(sender, e);
        }
    }
}