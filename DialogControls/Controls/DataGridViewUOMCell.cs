using ModelEngine;
using ModelEngine;
using System.Windows.Forms;
using Units;

namespace FormControls
{
    // Your class  should look like this:
    public class DataGridViewUOMCell : DataGridViewTextBoxCell
    {
        public UOMProperty uom = new UOMProperty(ePropID.NullUnits);
        private string displayText = "";

        public ePropID Propid
        {
            get
            {
                return uom.Propid;
            }
        }

        public void Update(UOMProperty UOM = null, bool isempty = false, bool valueonly = false)
        {
            if (UOM != null)
                if (valueonly)
                    this.uom.BaseValue = uom.BaseValue;
                else
                    this.uom = UOM;
            if (isempty)
                base.Value = "";
            else
                base.Value = uom.DisplayValueOut().ToString("F4");
        }

        public new double Value
        {
            get
            {
                return uom.DisplayValueOut();
            }
            set
            {
                uom.ValueIn(value);
                //base.Value = uom.DisplayValueOut();
            }
        }

        public DataGridViewUOMCell()
        {
        }

        public DataGridViewUOMCell(UOMProperty q, bool isempty = false)
        {
            string units = GlobalModel.displayunits.UnitsDict[q.Propid];
            q.DisplayUnit = units;
            uom = q;
            if (isempty)
                displayText = "";
            else
                displayText = uom.DisplayValueOut().ToString("F4");
            base.Value = displayText;
        }

        public DataGridViewUOMCell(StreamProperty q, bool isempty = false)
        {
            uom = q;
            if (isempty)
                displayText = "";
            else
                displayText = uom.DisplayValueOut().ToString("F4");
            base.Value = displayText;
        }
    }
}