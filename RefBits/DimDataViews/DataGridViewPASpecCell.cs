using ModelEngine;

// Add these to your file:
using System.Windows.Forms;

namespace Units
{
    // Your class  should look like this:
    public class DataGridViewPASpecCell : DataGridViewComboBoxCell
    {
        public DataGridViewPASpecCell()
        {
        }

        public new ePASpecTypes Value
        {
            get
            {
                return (ePASpecTypes)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }
}