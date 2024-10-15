using Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

public enum enumRotationAngle
{ a90 = 90, a180 = 180, a270 = 270, a0 = 0 };

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>

    [Serializable]
    public class DrawRectangle : DrawObject, IConnectable, ISerializable
    {
        internal UOMDisplayList displayunits = new();

        protected Rectangle rectangle;
        private Rectangle rotatedrectangle;
        protected const string entryRectangle = "Rect";
        public DrawName drawName;
        public NodeCollection Feeds = new();
        public NodeCollection Products = new();
        private enumRotationAngle angle = enumRotationAngle.a0;
        public bool FlipVertical = false, FlipHorizontal = false;
        private int nameoffsetx = 0, nameoffsety = 0;

        public void Dlg_ValueChangedEvent(object sender, EventArgs e)
        {
            RaiseValueChangedEvent(new EventArgs());
        }

        public Rectangle RotatedRectangle
        {
            get
            {
                if (rotatedrectangle.Size.IsEmpty)
                    InitRotatedRectangle();
                return rotatedrectangle;
            }
            set { rotatedrectangle = value; }
        }

        public override Point GetRotatedHandleLocation(int handleNumber)
        {
            int x = 0, y = 0, xCenter, yCenter;

            Rectangle rotrect = rotatedrectangle;
            xCenter = rotrect.Center().X;
            yCenter = rotrect.Center().Y;

            switch (handleNumber)
            {
                case 1:
                    x = rotrect.X;
                    y = rotrect.Y;
                    break;

                case 2:
                    x = xCenter;
                    y = rotrect.Y;
                    break;

                case 3:
                    x = rotrect.Right;
                    y = rotrect.Y;
                    break;

                case 4:
                    x = rotrect.Right;
                    y = yCenter;
                    break;

                case 5:
                    x = rotrect.Right;
                    y = rotrect.Bottom;
                    break;

                case 6:
                    x = xCenter;
                    y = rotrect.Bottom;
                    break;

                case 7:
                    x = rotrect.X;
                    y = rotrect.Bottom;
                    break;

                case 8:
                    x = rotrect.X;
                    y = yCenter;
                    break;
            }
            return new Point(x, y);
        }

        public override int GetRotatedHandleNumber(Point handleNumber) // piont is in normal coordinates, not zoomed
        {
            if (Selected)  // get handle
            {
                for (int i = 0; i <= HandleCount; i++)
                {
                    Rectangle r = GetRotatedHandleRectangle(i);

                    using GraphicsPath gp = new();
                    Matrix m = new();
                    //m.Scale(zoom, zoom);
                    // m.Translate(offsetx, offsety);
                    gp.AddRectangle(r);
                    gp.Transform(m);

                    if (gp.IsVisible(handleNumber))
                        return i;
                }
            }
            return 0;
        }

        public override void MoveRotatedHandleTo(Point point, int handleNumber)
        {
            int left = rotatedrectangle.Left;
            int top = rotatedrectangle.Top;
            int right = rotatedrectangle.Right;
            int bottom = rotatedrectangle.Bottom;

            switch (handleNumber)
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;

                case 2:
                    top = point.Y;
                    break;

                case 3:
                    right = point.X;
                    top = point.Y;
                    break;

                case 4:
                    right = point.X;
                    break;

                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;

                case 6:
                    bottom = point.Y;
                    break;

                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;

                case 8:
                    left = point.X;
                    break;
            }

            rotatedrectangle = new Rectangle(left, top, right - left, bottom - top);

            ResetRectangleToRotated(handleNumber);
        }

        public void InitRotatedRectangle()  // called when angle is set
        {
            rotatedrectangle = rectangle.Rotate((int)angle);
        }

        public Rectangle R
        {
            get
            {
                return rectangle;
            }
        }

        public override Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>

        public override Point GetHandleLocation(int handleNumber)
        {
            int x, y;
            Point Center;
            Rectangle Rectangle = rectangle;

            Center = rectangle.Center();

            x = Rectangle.X;
            y = Rectangle.Y;

            switch (handleNumber)
            {
                case 1:
                    x = Rectangle.X;
                    y = Rectangle.Y;
                    break;

                case 2:
                    x = Center.X;
                    y = Rectangle.Y;
                    break;

                case 3:
                    x = Rectangle.Right;
                    y = Rectangle.Y;
                    break;

                case 4:
                    x = Rectangle.Right;
                    y = Center.Y;
                    break;

                case 5:
                    x = Rectangle.Right;
                    y = Rectangle.Bottom;
                    break;

                case 6:
                    x = Center.X;
                    y = Rectangle.Bottom;
                    break;

                case 7:
                    x = Rectangle.X;
                    y = Rectangle.Bottom;
                    break;

                case 8:
                    x = Rectangle.X;
                    y = Center.Y;
                    break;
            }
            return new Point(x, y);
        }

        public GraphicsPath Rotate(GraphicsPath gp, PointF Center)
        {
            Matrix m = new();
            m.RotateAt((int)Angle, Center, MatrixOrder.Append);
            gp.Transform(m);
            return gp;
        }

        public static GraphicsPath FlipGPHorizontal(GraphicsPath gp)
        {
            PointF[] points = (PointF[])gp.PathPoints.Clone();
            RectangleF rect = gp.GetBounds();
            float dx;
            for (int i = 0; i < points.Length; i++)
            {
                dx = rect.Left + rect.Width - points[i].X;
                points[i].X = rect.Left + dx;
            }
            gp.Reset();
            gp.AddLines(points);

            return gp;
        }

        public static GraphicsPath FlipGPVertical(GraphicsPath gp)
        {
            PointF[] points = (PointF[])gp.PathPoints.Clone();
            RectangleF rect = gp.GetBounds();
            float dy;
            for (int i = 0; i < points.Length; i++)
            {
                dy = rect.Top + rect.Height - points[i].Y;
                points[i].Y = rect.Top + dy;
            }
            gp.Reset();
            gp.AddLines(points);

            return gp;
        }

        public HitType RotatedHitTest(Point location)
        {
            /*if (angle ==  enumRotationAngle.a0)
            {
                if (rectangle.Contains(location))
                    return   HitType.Object;
                else
                    return   HitType.None;
            }*/

            using Matrix matrix = new();
            matrix.RotateAt((int)angle, rectangle.Center());
            //matrix.Scale(zoom, zoom, MatrixOrder.Append);
            //matrix.Translate(offsetx, offsety, MatrixOrder.Append);

            using GraphicsPath path = new();
            path.AddRectangle(rectangle);
            path.Transform(matrix);
            if (path.IsVisible(location.X, location.Y))
                return HitType.Object;
            else
                return HitType.None;
        }

        /// <summary>
        /// Hit test.
        /// return   value: -1 - no hit
        ///                0 - hit anywhere
        ///              > 1 - handle number
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>
        public override HitType HitTest(Point point)
        {
            if (Selected)  // get handle
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    Rectangle r = GetRotatedHandleRectangle(i); //  check location of rotated handles.
                    using GraphicsPath gp = new();
                    Matrix m = new();
                    // m.Scale(zoom,zoom);
                    // m.Translate(offsetx, offsety, MatrixOrder.Append);
                    gp.AddRectangle(r);
                    gp.Transform(m);

                    if (gp.IsVisible(point))
                        return HitType.ObjectHandle;
                }
            }

            Node hs = HitTestHotSpot(point);

            if (hs != null)
                return HitType.StreamConnection;

            return RotatedHitTest(point);
        }

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            if (Hotspots != null)
                Hotspots.SetOwner(this);
        }

        private TempType temptype = TempType.NotDefined;

        public TempType Temptype
        {
            get { return temptype; }
            set { temptype = value; }
        }

        private PressType presstype = PressType.NotDefined;

        public PressType Presstype
        {
            get { return presstype; }
            set { presstype = value; }
        }

        private EnthType enthtype = EnthType.NotDefined;

        public EnthType Enthtype
        {
            get { return enthtype; }
            set { enthtype = value; }
        }

        private FlowType flowtype = FlowType.NotDefined;

        public FlowType Flowtype
        {
            get { return flowtype; }
            set { flowtype = value; }
        }

        private CompositionType compositiontype = CompositionType.NotDefined;

        public CompositionType Compositiontype
        {
            get { return compositiontype; }
            set { compositiontype = value; }
        }

        public Rectangle Location
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }

        public DrawRectangle() : this(0, 0, 20, 20)
        {
        }

        public DrawRectangle(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Initialize();
            if (this is not DrawName)
            {
                drawName = new();
                drawName.Attachedobject = this;
                drawName.Location = new(this.Rectangle.TopRight(), new Size(100, 11));
                //this.Drawarea.GraphicsList.Add(drawname);
            }
        }

        public override Node GetHotSpotFromHandleNumber(int hndl)
        {
            return Hotspots[hndl];
        }

        public override Rectangle GetHotSpotRectangle(Guid NodeID)
        {   // point  is center point !!
            Node hs = Hotspots.Search(NodeID);
            if (hs != null)
            {
                int x = hs.Absolute.X;
                int y = hs.Absolute.Y;
                return new Rectangle(x - 5, y - 5, 9, 9);
            }
            return new Rectangle();
        }

        public override Point GetHotSpotCenter(Guid NodeID)
        {
            int x = (int)(Hotspots.Search(NodeID).X * rectangle.Width) + rectangle.Left;
            int y = (int)(Hotspots.Search(NodeID).Y * rectangle.Height) + rectangle.Top;
            return new Point(x - 5, y - 5);
        }

        public override PointF GetHotSpotRelativeCenter(Guid NodeID)
        {
            return Hotspots.Search(NodeID).RotatedFlipped;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawRectangle drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            if (drawName != null)
                drawName.Draw(g);
        }

        protected void SetRectangle(int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            if (width < 10) // if < 0 causes exception in paint  event
                width = 10;
            if (height < 10)
                height = 10;
            rectangle.Width = width;
            rectangle.Height = height;
        }

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        public enumRotationAngle Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
                InitRotatedRectangle();
            }
        }

        public int NameOffSetX { get => nameoffsetx; set => nameoffsetx = value; }
        public int NameOffSetY { get => nameoffsety; set => nameoffsety = value; }

        public override Node HitTestHotSpot(Point p)
        {
            Node hs = Hotspots.GetHotSpotFromPoint(p);
            return hs;
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    return Cursors.SizeNWSE;

                case 2:
                    return Cursors.SizeNS;

                case 3:
                    return Cursors.SizeNESW;

                case 4:
                    return Cursors.SizeWE;

                case 5:
                    return Cursors.SizeNWSE;

                case 6:
                    return Cursors.SizeNS;

                case 7:
                    return Cursors.SizeNESW;

                case 8:
                    return Cursors.SizeWE;

                default:
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Move handle to new  point  (resizing)
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            switch (handleNumber)
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;

                case 2:
                    top = point.Y;
                    break;

                case 3:
                    right = point.X;
                    top = point.Y;
                    break;

                case 4:
                    right = point.X;
                    break;

                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;

                case 6:
                    bottom = point.Y;
                    break;

                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;

                case 8:
                    left = point.X;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }

        public void RelocateName()
        {
            if (this is not DrawName)
            {
                drawName.Location = new Rectangle(this.rectangle.TopRight(), new Size(100, 11));
                drawName.Location.Offset(nameoffsetx, nameoffsety);
            }
        }

        private void ResetRectangleToRotated(int HandleNumber)
        {
            int Left = rectangle.Left;
            int Top = rectangle.Top;

            rectangle = rotatedrectangle.Rotate(-(int)angle);

            switch (HandleNumber)
            {
                case 1:

                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    rectangle.X = Left;
                    rectangle.Y = Top;
                    break;

                case 5:
                    break;

                case 6:
                    break;

                case 7:
                    break;

                case 8:
                    break;
            }
        }

        /// <summary>
        /// Move handle to new  point  (resizing)
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandle(int dx, int dy, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            switch (handleNumber)
            {
                case 0:
                    break;

                case 1:
                    left = rectangle.Left + dx;
                    top = rectangle.Top + dy;
                    break;

                case 2:
                    top = rectangle.Top + dy;
                    break;

                case 3:
                    right = rectangle.Left + rectangle.Width + dx;
                    top = rectangle.Top + dy;
                    break;

                case 4:
                    right = rectangle.Left + rectangle.Width + dx;
                    break;

                case 5:
                    right = rectangle.Left + rectangle.Width + dx;
                    bottom = rectangle.Top + rectangle.Height + dy;
                    break;

                case 6:
                    bottom = rectangle.Top + rectangle.Height + dy;
                    break;

                case 7:
                    left = rectangle.Left + dx;
                    bottom = rectangle.Top + rectangle.Height + dy;
                    break;

                case 8:
                    left = rectangle.Left + dx;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Move handle to new  point  (resizing)
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public override void MoveRotatedHandle(int dx, int dy, int handleNumber)
        {
            Rectangle r = RotatedRectangle;

            int left = r.Left;
            int top = r.Top;
            int right = r.Right;
            int bottom = r.Bottom;

            switch (handleNumber)
            {
                case 0:
                    break;

                case 1:
                    left = r.Left + dx;
                    top = r.Top + dy;
                    break;

                case 2:
                    top = r.Top + dy;
                    break;

                case 3:
                    right = r.Left + r.Width + dx;
                    top = r.Top + dy;
                    break;

                case 4:
                    right = r.Left + r.Width + dx;
                    break;

                case 5:
                    right = r.Left + r.Width + dx;
                    bottom = r.Top + r.Height + dy;
                    break;

                case 6:
                    bottom = r.Top + r.Height + dy;
                    break;

                case 7:
                    left = r.Left + dx;
                    bottom = r.Top + r.Height + dy;
                    break;

                case 8:
                    left = r.Left + dx;
                    break;
            }

            rotatedrectangle = new Rectangle(left, top, right - left, bottom - top);
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            return Rectangle.IntersectsWith(rectangle);
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void RotatedMove(int deltaX, int deltaY)
        {
            rotatedrectangle.X += deltaX;
            rotatedrectangle.Y += deltaY;
            Move(deltaX, deltaY); //  move rectange also.
        }

        public override void Dump()
        {
            base.Dump();

            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }

        /// <summary>
        /// Save objevt to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("HotSpots", Hotspots);
            info.AddValue("entryRectangle", rectangle);

            info.AddValue("rotation", angle, typeof(enumRotationAngle));
            info.AddValue("FlipVertical", FlipVertical);
            info.AddValue("FlipHorizontal", FlipHorizontal);

            info.AddValue("nameoffsetx", nameoffsetx);
            info.AddValue("nameoffsety", nameoffsety);
            info.AddValue("displayunits", displayunits);

            base.GetObjectData(info, context);
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawRectangle(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                Hotspots = (NodeCollection)info.GetValue("HotSpots", typeof(NodeCollection));
                rectangle = (Rectangle)info.GetValue("entryRectangle", typeof(Rectangle));

                angle = (enumRotationAngle)info.GetValue("rotation", typeof(enumRotationAngle));
                FlipVertical = info.GetBoolean("FlipVertical");
                FlipHorizontal = info.GetBoolean("FlipHorizontal");

                nameoffsetx = info.GetInt32("nameoffsetx");
                nameoffsety = info.GetInt32("nameoffsety");

                displayunits = (UOMDisplayList)info.GetValue("displayunits", typeof(UOMDisplayList));
            }
            catch
            {
            }
        }

        public enum TempType
        {
            FixedOutlet, FixedInlet, FixedInletAndOutlet, NotDefined
        }

        public enum PressType
        {
            FixedOutlet, FixedInlet, FixedInletAndOutlet, NotDefined
        }

        public enum EnthType
        {
            Outlet, Inlet, InletAndOutlet, NotDefined, EnthalpyBalance, Overdefined, FixDT, FixTout, FixUA, FixDuty, CalcDuty
        }

        public enum FlowType
        {
            AllFeedsDefined, AllProductsDefined, OneFeedNotDefined, OneProductNotDefined, NotDefined, Overdefined
        }

        public enum CompositionType
        {
            AllFeedsDefined, AllProductsDefined, OneFeedNotDefined, OneProductNotDefined, NotDefined, Overdefined
        }

        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if (x2 < x1)
            {
                (x1, x2) = (x2, x1);
            }

            if (y2 < y1)
            {
                (y1, y2) = (y2, y1);
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }
    }
}