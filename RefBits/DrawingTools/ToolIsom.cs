using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolIsom : Units.ToolRectangle
    {
        public ToolIsom()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawObject dobj = new DrawIsom(e.X, e.Y, 30, 30);
            AddnewObject(drawArea, dobj);
            dobj.CreateFlowsheetUOModel();
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            MessageBox.Show("DBLCLick Isom");
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}