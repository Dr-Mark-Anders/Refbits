namespace UOMGrid
{
    public class UOMGridColumn : DataGridViewTextBoxColumn
    {
        public UOMGridColumn()
        {
            this.CellTemplate = new UOMGridCell();
        }
    }
}