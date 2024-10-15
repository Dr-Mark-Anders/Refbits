using ModelEngine.Ports.Events;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngineTest
{
    public partial class TraySectionTest : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        public event SectionChangedEventHandler SectionChanged;

        public delegate void SectionChangedEventHandler(object sender, PropertyEventArgs e);

        private void TraySection_TrayChanged(object sender, PropertyEventArgs e)
        {
            SectionChanged?.Invoke(sender, e);
        }
    }
}