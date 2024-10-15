using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    ///<summary>
    ///Polygontool
    ///</summary>
    internal class ColumnToolStream : Units.ToolObject
    {
        private int lastX;
        private int lastY;
        private DrawMaterialStream newStream;
        private const int minDistance = 500;
        private bool isstart = true;

        public ColumnToolStream()
        {
            Cursor = new Cursor(GetType(), "Pencil.cur");
        }

        ///<summary>
        ///Leftnousebuttonispressed
        ///</summary>
        ///<paramname="drawArea"></param>
        ///<paramname="e"></param>
        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            if (isstart)
            {
                //Createnew polygon,addittothelist
                //andkeepreferencetoit
                newStream = new DrawMaterialStream(e.X, e.Y);
                AddnewObject(drawArea, newStream);
                lastX = e.X;
                lastY = e.Y;
                isstart = false;
            }
            else
            {
                drawArea.Cursor = Cursor;

                if (e.Button != MouseButtons.Left)
                    return;

                if (newStream == null)
                    return;//precaution

                Point point = new Point(e.X, e.Y);

                int distance = (e.X - lastX) * (e.X - lastX) + (e.Y - lastY) * (e.Y - lastY);

                if (distance < minDistance)
                {
                    //Distancebetweenlasttwopoint sislessthanminimum-
                    //movelastpoint
                    newStream.MoveHandleTo(point, newStream.HandleCount);
                }
                else
                {
                    //Addnew point
                  //  newStream.segArray.AddPoint(point);
                    lastX = e.X;
                    lastY = e.Y;
                }
            }
            drawArea.Refresh();
        }

        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }

        public override void OnMouseUp(DrawArea drawArea, DrawMouseEventArgs e)
        {
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            newStream = null;
            isstart = true;
            base.OnMouseUp(drawArea, e);
        }
    }
}