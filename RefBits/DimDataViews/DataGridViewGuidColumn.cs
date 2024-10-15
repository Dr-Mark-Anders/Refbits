using System.Windows.Forms;

namespace Units
{
    public class DataGridViewGuidColumn : DataGridViewComboBoxColumn
    {
        public DataGridViewGuidColumn()
        {
            this.CellTemplate = new DataGridViewGuidCell();
        }
    }
}