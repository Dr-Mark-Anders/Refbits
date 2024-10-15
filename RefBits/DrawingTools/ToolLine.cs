namespace Units
{
    /// <summary>
    /// Line tool
    /// </summary>
    internal class ToolLine : Units.ToolObject
    {
        public ToolLine()
        {
            //  Cursor = new  Cursor(GetType(), "Line.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            AddnewObject(drawArea, new DrawLine(e.X, e.Y, e.X + 1, e.Y + 1));
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }
    }
}