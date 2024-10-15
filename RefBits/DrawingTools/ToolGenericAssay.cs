namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolGenericAssay : Units.ToolRectangle
    {
        public ToolGenericAssay()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawObject dobj = new DrawGenericAssay(e.X, e.Y, 40, 60);
            AddnewObject(drawArea, dobj);
            dobj.CreateFlowsheetUOModel();
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}