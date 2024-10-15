using System;
using System.Windows.Forms;
using System.Drawing;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    class ToolAssayFeed : Units.ToolRectangle
    {
        public ToolAssayFeed()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddnewObject(drawArea, new DrawAssayFeed(e.X, e.Y, 30, 30));
        }

        public override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            MessageBox.Show("DBLCLick");
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
        }
    }
}