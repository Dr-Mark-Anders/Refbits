namespace Units
{
    /// <summary>
    /// Ellipse tool
    /// </summary>
    internal class ToolEllipse : Units.ToolRectangle
    {
        public ToolEllipse()
        {
            //Cursor = new  Cursor(GetType(), "Ellipse.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            AddnewObject(drawArea, new DrawEllipse(e.X, e.Y, 30, 30));
        }
    }
}