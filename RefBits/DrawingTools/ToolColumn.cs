using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolColumn : Units.ToolRectangle
    {
        public ToolColumn()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawObject dobj = new DrawColumn(e.X, e.Y, 50, 300);
            AddnewObject(drawArea, dobj);
            dobj.CreateFlowsheetUOModel();
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            MessageBox.Show("DBLCLick");
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }

        public override void OnMouseClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}