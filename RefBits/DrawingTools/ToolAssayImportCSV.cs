using System;
using System.Windows.Forms;
using System.Drawing;

namespace Units
{
    /// <summary>
    /// Rectangle tool
    /// </summary>
    class ToolAssayImportCSV : Units.ToolRectangle
    {
        public ToolAssayImportCSV()
        {
            // Cursor = new  Cursor(GetType(), "Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddnewObject(drawArea, new DrawAssayImportCSV(e.X, e.Y, 30, 30));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
        }
    }
}