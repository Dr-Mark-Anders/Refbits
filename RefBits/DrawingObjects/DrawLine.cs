using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Line graphic object
    /// </summary>
    public class DrawLine : DrawObject, ISerializable
    {
        private Point startPoint;
        private Point endPoint;

        /// <summary>
        ///  Graphic objects for hit test
        /// </summary>
        private GraphicsPath areaPath = null;

        private Pen areaPen = null;
        private Region areaRegion = null;

        [Browsable(false)]
        public DrawLine() : this(0, 0, 1, 0)
        {
        }

        [Browsable(false)]
        public DrawLine(int x1, int y1, int x2, int y2) : base()
        {
            startPoint.X = x1;
            startPoint.Y = y1;
            endPoint.X = x2;
            endPoint.Y = y2;

            Initialize();
        }

        [Browsable(false)]
        public DrawLine(Point p1, Point p2) : base()
        {
            startPoint.X = p1.X;
            startPoint.Y = p1.Y;
            endPoint.X = p2.X;
            endPoint.Y = p2.Y;

            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawLine drawLine = new();
            drawLine.startPoint = this.startPoint;
            drawLine.endPoint = this.endPoint;

            FillDrawObjectFields(drawLine);
            return drawLine;
        }

        [Browsable(false)]
        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new(StreamColor, PenWidth);

            g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            pen.Dispose();
        }

        [Browsable(false)]
        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Point GetHandleLocation(int handleNumber)
        {
            if (handleNumber == 1)
                return startPoint;
            else
                return endPoint;
        }

        [Browsable(false)]
        public override bool IntersectsWith(Rectangle rectangle)
        {
            CreateObjects();
            if (areaRegion == null)
                return false;
            return AreaRegion.IsVisible(rectangle);
        }

        [Browsable(false)]
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;

                default:
                    return Cursors.Default;
            }
        }

        [Browsable(false)]
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)
                startPoint = point;
            else
                endPoint = point;

            Invalidate();
        }

        [Browsable(false)]
        public override void Move(int deltaX, int deltaY)
        {
            startPoint.X += deltaX;
            startPoint.Y += deltaY;

            endPoint.X += deltaX;
            endPoint.Y += deltaY;

            Invalidate();
        }

        [Browsable(false)]
        public DrawLine(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            startPoint = (Point)info.GetValue("StartPoint", typeof(Point));
            endPoint = (Point)info.GetValue("EndPoint", typeof(Point));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StartPoint", startPoint);
            info.AddValue("EndPoint", endPoint);

            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        /// [Browsable(false)]
        protected void Invalidate()
        {
            if (AreaPath != null)
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if (AreaPen != null)
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if (AreaRegion != null)
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        [Browsable(false)]
        protected virtual void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);
            AreaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            AreaPath.Widen(AreaPen);

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }

        [Browsable(false)]
        protected GraphicsPath AreaPath
        {
            get
            {
                return areaPath;
            }
            set
            {
                areaPath = value;
            }
        }

        [Browsable(false)]
        protected Pen AreaPen
        {
            get
            {
                return areaPen;
            }
            set
            {
                areaPen = value;
            }
        }

        [Browsable(false)]
        protected Region AreaRegion
        {
            get
            {
                return areaRegion;
            }
            set
            {
                areaRegion = value;
            }
        }
    }
}