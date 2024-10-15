using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    ///<summary>
    ///Rectangletool
    ///</summary>
    internal class ColumnToolTrays : ToolRectangle
    {
        public ColumnToolTrays()
        {
            //Cursor=new Cursor(GetType(),"Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawColumnTraySection dc = new(e.X, e.Y, 1, 1, 20);
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
                Point point = new(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 5);
                drawArea.Refresh();
            }
        }
    }
}