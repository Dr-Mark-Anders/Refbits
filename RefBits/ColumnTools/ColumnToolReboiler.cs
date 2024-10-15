using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    ///<summary>
    ///Rectangletool
    ///</summary>
    internal class ColumnToolReboiler : Units.ToolRectangle
    {
        public ColumnToolReboiler()
        {
            //Cursor=new Cursor(GetType(),"Rectangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            AddnewObject(drawArea, new DrawColumnReboiler(e.X, e.Y, 30, 30));
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            MessageBox.Show("DBCCLick");
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