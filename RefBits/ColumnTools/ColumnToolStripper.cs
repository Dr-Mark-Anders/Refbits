using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    ///<summary>
    ///Rectangletool
    ///</summary>
    internal class ColumnToolStripper : Units.ToolRectangle
    {
        public ColumnToolStripper()
        {
            //Cursor=new Cursor(GetType(),"Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawColumnTraySection dc = new DrawColumnTraySection(e.X, e.Y, 1, 1, 3);
            AddnewObject(drawArea, dc);
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            MessageBox.Show("DBLCLick");
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if (e.Button == MouseButtons.Left)
            {
                Point point = new Point(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
    }
}