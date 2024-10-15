using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Polygon tool
    /// </summary>
    internal class ToolStream : ToolObject
    {
        private int lastX;
        private int lastY;
        private DrawMaterialStream newStream;
        private const int minDistance = 500;
        private bool isstart = true;

        public ToolStream()
        {
            //Cursor = new  Cursor(GetType(), "Pencil.cur");
            this.Cursor = new Cursor(new System.IO.MemoryStream(Main.Properties.Resources.Pencil));
        }

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            if (isstart)
            {
                newStream = new(e.X, e.Y);
                lastX = e.X;
                lastY = e.Y;
                newStream.StartNode = new Node(drawArea, e.X, e.Y, "floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
                isstart = false;
            }
            else
            {
                drawArea.Cursor = Cursor;

                if (e.Button != MouseButtons.Left)
                    return;

                if (newStream == null)
                    return;                 // precaution

                Point point = new(e.X, e.Y);
                int distance = (e.X - lastX) * (e.X - lastX) + (e.Y - lastY) * (e.Y - lastY);

                if (distance < minDistance)    // Distance between last two point s is less than minimum - move last point
                    newStream.MoveHandleTo(point, newStream.HandleCount);

                else// Add new  point
                {
                    //int last = newStream.pointArray.Count() - 1;
                    //newStream.pointArray[last] = point;
                    //lastX = e.X;
                    //lastY = e.Y;
                    newStream.EndNode = new Node(drawArea, e.X, e.Y, "floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
                    newStream.CreateOrthogonals();
                }
                

                AddnewObject(drawArea, newStream); // also adds to GlobalFlowsheet

                newStream = null;
                isstart = true;
                base.OnMouseUp(drawArea, e);
                drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
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
            drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
        }
    }
}