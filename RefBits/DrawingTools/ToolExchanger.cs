namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    internal class ToolExchanger : Units.ToolRectangle
    {
        public ToolExchanger()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawObject dobj = new DrawExchanger(e.X, e.Y, 60, 60);
            AddnewObject(drawArea, dobj);
            dobj.CreateFlowsheetUOModel();
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}