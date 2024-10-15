using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolCaseStudy : ToolRectangle
    {
        public ToolCaseStudy()
        {
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawCaseStudy dobj = new(e.X, e.Y, 60, 60);
            AddnewObject(drawArea, dobj);
            dobj.CreateFlowsheetUOModel();
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