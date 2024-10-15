using ModelEngine;
using System.Windows.Forms;

namespace Units
{
    public class DataGridViewPortSignalColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewPortSignalColumn()
        {
            this.CellTemplate = new DataGridViewPortSignalCell();
        }
    }
}