using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolCoker : Units.ToolRectangle
    {
        public ToolCoker()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            AddnewObject(drawArea, new DrawCoker(e.X, e.Y, 30, 30));
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            MessageBox.Show("DBLCLick Coker");
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}