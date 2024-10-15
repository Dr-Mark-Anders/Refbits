using System.Windows.Forms;

namespace FormControls
{
    public class DataGridViewUOMColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewUOMColumn()
        {
            this.CellTemplate = new DataGridViewUOMCell();
        }
    }
}