using System.Windows.Forms;
using Units;

namespace ModelEngine
{
    // Your class  should look like this:
    public class DataGridViewPortSignalCell : DataGridViewTextBoxCell
    {
        public Port_Signal port;
        private eDisplayState CurrentState;
        private string displayText = "";

        public eDisplayState state
        {
            get
            {
                return CurrentState;
            }
            set
            {
                CurrentState = value;
            }
        }

        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
                base.Value = value;
            }
        }

        public void PortValueUpdate(SourceEnum origin)
        {
            if (Value != null && double.TryParse(Value.ToString(), out double res))
            {
                port.Value = new StreamProperty(ePropID.NullUnits, res, origin);
            }
        }

        public DataGridViewPortSignalCell()
        {
            port = null;
        }

        public DataGridViewPortSignalCell(Port_Signal comp)
        {
            this.port = comp;
            displayText = this.port.Value.ToString();
            base.Value = displayText;
        }

        public void ValueUpdate()
        {
            displayText = this.port.Value.ToString();
            base.Value = displayText;
        }

        public void Erase()
        {
            Value = double.NaN;
            port.Value.Clear();
        }
    }
}