using ModelEngine;

// Add these to your file:
using System.Windows.Forms;

namespace Units
{
    // Your class  should look like this:
    public class DataGridViewSpecTypeCell : DataGridViewComboBoxCell
    {
        public DataGridViewSpecTypeCell()
        {
        }

        public new eSpecType Value
        {
            get
            {
                return (eSpecType)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }
}