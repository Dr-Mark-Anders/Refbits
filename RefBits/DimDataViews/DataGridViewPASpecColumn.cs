using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public class DataGridViewPASpecColumn : DataGridViewComboBoxColumn
    {
        public DataGridViewPASpecColumn()
        {
            this.CellTemplate = new DataGridViewPASpecCell();
            this.ReadOnly = false;
            this.DataSource = Enum.GetNames(typeof(ePASpecTypes));
            this.ValueType = typeof(ePASpecTypes);
        }
    }
}