using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolName : ToolRectangle
    {
        public ToolName()
        {
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            //AddnewObject(drawArea, new  DrawValve(e.X, e.Y, 60, 30));
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            MessageBox.Show("DBCCLick");
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}