using Main.DrawingObjects.Streams;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using static Main.DrawingObjects.Streams.SegmentArray;

//using PointList = System.Collections.Generic.List<System.Drawing.Point>;
//using PointList = Main.DrawingObjects.Streams.FixablePointArray;

namespace Units
{
    /// <summary>
    /// Polygon graphic object
    /// </summary>
    ///
    [Serializable]
    public partial class DrawBaseStream : DrawLine, IDrawStream, ISerializable
    {
        internal Guid startNodeGuid, endNodeGuid;
        internal Guid startDrawObjectID, endDrawObjectGuid;
        internal Guid startDrawTrayID, endDrawTrayID;

        internal Guid engineDrawSection, engineDrawTray, enginereturnSection, enginereturnTray;

        internal DrawObject startDrawObject, endDrawObject;
        internal DrawColumnTraySection startDrawTraySection, endDrawTraySection;
        internal DrawTray endDrawTray;
        internal SmoothingMode smoothingmode;
        internal bool active = true;
        internal DrawTray startDrawTray;
        internal readonly int penWidth = 1;

        private Node startNode, endNode;
        public Node dataNode;

        public bool isSolved;
        public DrawName drawName;

        internal bool isPumparoud = false;
        internal bool isColumnFeed = false;
        internal bool isVapourDraw;

        //public FixablePointArray pointArray;         // list of point s
        public SegmentArray segArray = new();

        // private  static Cursor handleCursor = new  Cursor(typeof(DrawPolygon), "PolyHandle.cur");
        private static readonly Cursor handleCursor = new(new System.IO.MemoryStream(Main.Properties.Resources.PolyHandle));

        public PointF StartRelPosition, EndRelativePosition;
        private string startname = "", endname = "";
        private int firstSegmentSize = 0;

        //readonly  DrawName dsn;
        private int offsetX, offsetY;

        public Point[] pointArray
        { get { return segArray.PointArray; } }

        public DrawBaseStream() : base()
        {
            Initialize();
            StreamColor = Color.Blue;
        }

        public DrawBaseStream(int x1, int y1) : base()
        {
            Initialize();
            StreamColor = Color.Blue;
        }

        [Browsable(false)]
        public int NameOffSetX { get => nameoffsetx; set => nameoffsetx = value; }

        [Browsable(false)]
        public int NameOffSetY { get => nameoffsety; set => nameoffsety = value; }

        public DrawBaseStream(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            //string[] Names = info.nam
            try
            {
                Name = info.GetString("Name");
                StartNodeGuid = (Guid)info.GetValue("startNodeId", typeof(Guid));
                EndNodeGuid = (Guid)info.GetValue("endNodeId", typeof(Guid));
                StartRelPosition = (PointF)info.GetValue("StartRP", typeof(PointF));
                EndRelativePosition = (PointF)info.GetValue("EndRP", typeof(PointF));
                StartName = info.GetString("StartName");
                EndName = info.GetString("EndName");
                firstSegmentSize = info.GetInt32("firstSegmentSize");
                lastSegmentSize = info.GetInt32("lastSegmentSize");
                startdirection = (StartLineDirection)info.GetValue("StartDirection", typeof(StartLineDirection));
                EndDirection = (EndLineDirection)info.GetValue("EndDirection", typeof(EndLineDirection));
                StartDrawObjectGuid = (Guid)info.GetValue("StartID", typeof(Guid));
                EndDrawObjectGuid = (Guid)info.GetValue("EndID", typeof(Guid));

                nameoffsetx = info.GetInt32("nameoffsetx");
                nameoffsety = info.GetInt32("nameoffsety");

                segArray = (SegmentArray)info.GetValue("SegmentArray", typeof(SegmentArray));
            }
            catch
            {
                nameoffsetx = 0;
                nameoffsety = 0;
            }

            if (StartDrawObjectGuid == Guid.Empty && segArray.Count > 0)
            {
                startNode = new(null, segArray.List[0].Point1.X, segArray.List[0].Point1.Y, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
            }
            else
            {
                startNode = new(null, 100, 100, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
            }

            if (EndDrawObjectGuid == Guid.Empty && segArray.Count > 0)
            {
                endNode = new(null, segArray.List.Last().Point2.X, segArray.List.Last().Point2.Y, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
            }
            else
            {
                endNode = new(null, 100, 100, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            //info.AddValue("PointArray", pointArray, typeof(FixablePointArray));
            info.AddValue("Name", Name);

            // DrawObject Guids
            info.AddValue("EndID", EndDrawObjectGuid);
            info.AddValue("StartID", StartDrawObjectGuid);
            info.AddValue("startNodeId", StartNodeGuid);
            info.AddValue("endNodeId", EndNodeGuid);

            info.AddValue("StartRP", StartRelPosition);
            info.AddValue("EndRP", EndRelativePosition);
            info.AddValue("StartName", startname);
            info.AddValue("EndName", endname);
            info.AddValue("firstSegmentSize", firstSegmentSize);
            info.AddValue("lastSegmentSize", lastSegmentSize);
            info.AddValue("StartDirection", startdirection);
            info.AddValue("EndDirection", EndDirection);
            info.AddValue("active", active);
            info.AddValue("nameoffsetx", nameoffsetx);
            info.AddValue("nameoffsety", nameoffsety);
            info.AddValue("SegmentArray", segArray, typeof(SegmentArray));
        }

        public virtual void UpdateDrawObjects(GraphicsList gl)
        {
            StartDrawObject = gl.GetObject(StartDrawObjectGuid);
            EndDrawObject = gl.GetObject(EndDrawObjectGuid);
        }

        /// <summary>
        /// Update connections to draw objects
        /// </summary>
        /// <param name="gl"></param>
        public event ValueErrorEventHandler errorChangedEvent;

        public delegate void ValueErrorEventHandler(object sender, EventArgs e);

        [Category("Display Properties"), Description("Stream Name")]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        public int SegmentLength(int i)
        {
            return segArray.Length();
        }

        public Point LineCenter(out bool? IsHorizontal)
        {
            Point rectcenter = Center;
            IsHorizontal = true;

            int pointcount = (int)(segArray.Count / 2.0);

            BaseSegment seg = segArray[pointcount];

            Point p = new();

            switch (seg)
            {
                case SegmentV segv:
                    {
                        int X = segv.X - 4;
                        int y = (int)((segv.Y1 + segv.Y2) / 2.0) - 4;
                        p = new Point(X, y);
                        IsHorizontal = false;
                    }
                    break;

                case SegmentH segh:
                    {
                        int y = segh.Y - 4;
                        int x = (int)((segh.X1 + segh.X2) / 2.0) - 4;
                        p = new Point(x, y);
                        IsHorizontal = true;
                    }
                    break;
            }

            return p;
        }

        public Node Endnode(GraphicsList g, bool checkfortray = true)
        {
            DrawObject d = g.GetObject(EndDrawObjectGuid, checkfortray);

            if (d != null)
                return d.GetNode(EndNodeGuid);

            return null;
        }

        public Node Startnode(GraphicsList g)
        {
            DrawObject d = g.GetObject(StartDrawObjectGuid);

            if (d != null)
            {
                return d.GetNode(StartNodeGuid);
            }

            return null;
        }

        public override void CreateFlowsheetUOModel()
        {
        }

        public static bool HasChanged()
        {
            return false;
        }

        public static void ResetIsChanged()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        protected virtual void RaiseValueErrorEvent(EventArgs e)
        {
            errorChangedEvent?.Invoke(this, e);
        }

        protected int lastSegmentSize = 0;

        public override void Move(int deltaX, int deltaY)
        {
            segArray.Move(deltaX, deltaY);
            Invalidate();
        }

        public override void ResetCalculateFlag()
        {
            base.ResetCalculateFlag();
        }

        protected StartLineDirection startdirection = StartLineDirection.Left;

        private EndLineDirection endDirection = EndLineDirection.Right;
        private int nameoffsetx;
        private int nameoffsety;

        protected bool PointInObject(Point point)
        {
            if (segArray.Count > 0)
            {
                return segArray.ContainsPoint(point);
            }
            return false;
        }

        public override HitType HitTest(Point point)
        {
            if (Selected)  // get handle
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    Rectangle r = GetHandleRectangle(i);

                    using Matrix matrix = new();
                    using GraphicsPath path = new();
                    path.AddRectangle(r);
                    path.Transform(matrix);
                    if (path.IsVisible(point))
                        return HitType.StreamHandle;
                }
            }

            Node hs = HitTestHotSpot(point);

            if (hs != null)
                return HitType.StreamConnection;

            if (PointInObject(point))
                return HitType.Stream;

            return HitType.None;
        }

        public override int GetHandleNumber(Point point)
        {
            if (Selected)  // get handle
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    Rectangle r = GetHandleRectangle(i);

                    using Matrix matrix = new();
                    using GraphicsPath path = new();

                    path.AddRectangle(r);
                    path.Transform(matrix);
                    if (path.IsVisible(point))
                        return i;
                }
            }
            return 0;
        }

        public override FrontBack GetHandlePosition(Point point)
        {
            int No = this.GetHandleNumber(point);

            if(No==1)
                return FrontBack.Front;

            if (No == HandleCount)
                return FrontBack.Back;

            return FrontBack.None;
        }

        [Browsable(false)]
        public SegmentArray SegArray
        {
            get
            {
                return segArray;
            }
        }

        public void AddEndPoints(NodeDirections HSDirection, EndLineDirection LineDirection)
        {
            Point end, end2;

            /* switch (HSDirection)
             {
                 case NodeDirections.Down:
                     switch (LineDirection)
                     {
                         case EndLineDirection.Left:
                             end = (Point)segArray.End;
                             end2 = pointArray[pointArray.Count - 2];
                             pointArray.Add(new Point(end.X, end.Y));
                             pointArray[pointArray.Count - 2] = new Point(end.X, end.Y - 30);
                             pointArray[pointArray.Count - 3] = new Point(end2.X, end2.Y - 30);
                             EndDirection = EndLineDirection.Down;
                             break;

                         case EndLineDirection.Right:
                             end = pointArray[pointArray.Count - 1];
                             end2 = pointArray[pointArray.Count - 2];
                             pointArray.Add(new Point(end.X, end.Y));
                             pointArray[pointArray.Count - 2] = new Point(end.X, end.Y - 30);
                             pointArray[pointArray.Count - 3] = new Point(end2.X, end2.Y - 30);
                             EndDirection = EndLineDirection.Down;
                             break;
                     }
                     break;
             }*/
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected override void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);

            if (segArray.Count == 0)
                return;

            AreaPath.AddLines(segArray.PointArray);

            try
            {
                AreaPath.Widen(AreaPen);
            }
            catch
            {
            }

            // Create region from the path
            AreaRegion = new Region(AreaPath);
            AreaPen.Dispose();
            AreaPath.Dispose();
        }

        public DrawBaseStream(Node hsstart, Node hsend) : base()
        {
            Initialize();
            StreamColor = Color.Blue;

            Point StartPoint, EndPoint;
            Node startHotSpot, endHotSpot;

            if (hsstart == null || hsend == null)
                return;

            if ((hsstart.IsInput && !hsend.IsInput) ||
                (hsstart.NodeType == HotSpotType.EnergyIn &&
                hsend.NodeType != HotSpotType.EnergyIn) ||
                (hsstart.NodeType == HotSpotType.SignalIn &&
                hsend.NodeType != HotSpotType.SignalOut))
            {
                startHotSpot = hsend;
                endHotSpot = hsstart;
            }
            else
            {
                endHotSpot = hsend;
                startHotSpot = hsstart;
            }

            this.startNode = startHotSpot;
            this.endNode = endHotSpot;

            switch (startHotSpot.Owner)
            {
                case DrawTray t:
                    object Owner = t.GetParent();
                    if (Owner != null)
                    {
                        startDrawObject = Owner as DrawTray;
                        startDrawObjectID = ((DrawObject)Owner).Guid;
                    }
                    break;

                case DrawRectangle dr:
                    startDrawObject = dr;
                    startDrawObjectID = dr.Guid;
                    break;

                case DrawArea da:
                    //EndDrawObject = da;
                    startDrawObjectID = da.Guid;
                    break;
            }

            switch (endHotSpot.Owner)
            {
                case DrawTray t:
                    object Owner = t.GetParent();
                    if (Owner != null)
                    {
                        endDrawObject = Owner as DrawTray;
                        EndDrawObjectGuid = ((DrawObject)Owner).Guid;
                    }
                    break;

                case DrawRectangle dr:
                    EndDrawObject = dr;
                    EndDrawObjectGuid = dr.Guid;
                    break;

                case DrawArea da:
                    //EndDrawObject = da;
                    EndDrawObjectGuid = da.Guid;
                    break;
            }

            startHotSpot.IsConnected = true;
            endHotSpot.IsConnected = true;

            startHotSpot.AttachedStreamGuid = this.Guid;
            endHotSpot.AttachedStreamGuid = this.Guid;

            StartNodeGuid = startHotSpot.Guid;
            EndNodeGuid = endHotSpot.Guid;

            StartPoint = startHotSpot.Absolute.Location;
            EndPoint = endHotSpot.Absolute.Location;

            startname = startHotSpot.Name;
            endname = endHotSpot.Name;

            //pointArray[0] = StartPoint;
            // pointArray[^1] = EndPoint;
            startdirection = StartLineDirection.Right;
            endDirection = EndLineDirection.Right;

            CreateOrthogonals();

            if (dataNode is not null)
            {
                Point loc = this.LineCenter(out bool? ishorizontal);
                switch (ishorizontal)
                {
                    case true:
                        dataNode.LineDirection = NodeDirections.Left;
                        break;

                    case false:
                        dataNode.LineDirection = NodeDirections.Down;
                        break;

                    case null:
                        dataNode.LineDirection = NodeDirections.Left;
                        break;
                }

                dataNode.SetAbsoluteLocation(loc.X - 4, loc.Y - 4);
            }

            StartRelPosition = StartPoint;
            EndRelativePosition = EndPoint;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawBaseStream drawStream = new();
            drawStream.segArray = segArray.Clone();

            FillDrawObjectFields(drawStream);
            return drawStream;
        }

        public enum enumLocation
        { AboveRight, AboveLeft, BelowRight, BelowLeft }

        public enum enumDirection
        {
            BothForward,
            BothBackward,

            UpUp,
            DownDown,

            ForwardBackward,
            BackwardForward,

            UpDown,
            DownUp,

            ForwardUp,
            ForwardDown,
            BackwardDown,
            BackwardUp,

            DownForward,
            DownBackward,

            UpForward,
            UpBackward
        }

        public StreamDirections StreamDirection(Node node)
        {
            if (node.Owner is DrawArea || node.NodeType == HotSpotType.Floating)
            {
                if (StartNode.Absolute.X > endNode.Absolute.X)
                    return StreamDirections.BackwardIn;
                return StreamDirections.ForwardIn;
            }

            if (node.Owner is DrawMaterialStream dms)
            {
                Point p = dms.LineCenter(out bool? isHorizontal);

                switch (isHorizontal)
                {
                    case true:
                        if (p.Y < node.Relative.Y)
                        {
                            return StreamDirections.TopIn;
                        }
                        else
                            return StreamDirections.TopIn;

                    case false:
                        if (p.X < node.Relative.X)
                        {
                            return StreamDirections.ForwardIn;
                        }
                        else
                            return StreamDirections.ForwardOut;

                    default:
                        return StreamDirections.ForwardIn;
                }
            }
            else
            {
                StreamDirections nd = StreamDirections.ForwardOut;

                if (node.RotatedFlipped.X == 0 || node.RotatedFlipped.X == 1) // either left or right, not top or bottom
                {
                    if (node.RotatedFlipped.X < 0.5)  // left
                    {
                        if (node.IsInput)  // left and input
                            nd = StreamDirections.ForwardIn;
                        else
                            nd = StreamDirections.BackwardOut;
                    }
                    else  // right
                    {
                        if (node.IsInput)  // right and output
                            nd = StreamDirections.BackwardIn;
                        else
                            nd = StreamDirections.ForwardOut;
                    }
                }
                else  // top or bottom
                {
                    if (node.RotatedFlipped.Y > 0.5 && node.IsInput)
                        nd = StreamDirections.BotIn;
                    else if (node.RotatedFlipped.Y > 0.5 && !node.IsInput)
                        nd = StreamDirections.BotOut;
                    else if (node.IsInput)
                        nd = StreamDirections.TopIn;
                    else
                        nd = StreamDirections.TopOut;
                }

                //if (node.Owner is DrawRectangle dr)
                //    nd = RotateDirection(nd, dr.Angle, dr.FlipHorizontal, dr.FlipVertical);

                return nd;
            }
        }

        public StreamDirections RotateDirection(StreamDirections nd, enumRotationAngle Angle, bool FlipHorizontal, bool FlipVertical)
        {
            switch (nd)
            {
                case StreamDirections.TopIn:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.ForwardIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BotIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BackwardIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.TopIn;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BackwardIn:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.BotIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.ForwardIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.TopIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BackwardIn;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.ForwardIn:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.TopIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BackwardIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BotIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.ForwardIn;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BotIn:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.ForwardIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.TopIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BackwardIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BotIn;
                            break;

                        default:
                            break;
                    }
                    break;

                case StreamDirections.TopOut:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.ForwardOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BotOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BackwardOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.TopOut;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BackwardOut:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.TopOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.ForwardOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BotOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BackwardOut;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.ForwardOut:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.BotOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BackwardOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.TopOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.ForwardOut;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BotOut:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.BackwardOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.TopOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.ForwardOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BotOut;
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            if (FlipHorizontal && nd == StreamDirections.ForwardIn)
                nd = StreamDirections.BackwardIn;
            else if (FlipVertical && nd == StreamDirections.BackwardIn)
                nd = StreamDirections.ForwardIn;

            if (FlipVertical && nd == StreamDirections.TopIn)
                nd = StreamDirections.BotIn;
            else if (FlipVertical && nd == StreamDirections.BotIn)
                nd = StreamDirections.TopIn;

            if (FlipHorizontal && nd == StreamDirections.ForwardOut)
                nd = StreamDirections.BackwardOut;
            else if (FlipVertical && nd == StreamDirections.BackwardOut)
                nd = StreamDirections.ForwardOut;

            if (FlipVertical && nd == StreamDirections.TopOut)
                nd = StreamDirections.BotOut;
            else if (FlipVertical && nd == StreamDirections.BotOut)
                nd = StreamDirections.TopOut;

            return nd;
        }

        private enumDirection Directions2(Node StartNode, Node EndNode)
        {
            if (startNode is null || endNode is null)
            {
                // Debugger.Break();
                return enumDirection.BothForward;
            }

            enumDirection eDirection = enumDirection.BothForward;

            switch (StreamDirection(startNode))
            {
                case StreamDirections.ForwardIn:
                    switch (StreamDirection(EndNode))
                    {
                        case StreamDirections.BackwardOut:
                            eDirection = enumDirection.ForwardBackward;
                            break;

                        case StreamDirections.BackwardIn:
                            eDirection = enumDirection.ForwardBackward;
                            break;

                        case StreamDirections.TopIn:
                        case StreamDirections.BotOut:
                            eDirection = enumDirection.ForwardDown;
                            break;

                        case StreamDirections.TopOut:
                        case StreamDirections.BotIn:
                            eDirection = enumDirection.ForwardUp;
                            break;
                    }
                    break;

                case StreamDirections.BackwardOut:
                    switch (StreamDirection(EndNode))
                    {
                        case StreamDirections.BackwardIn:
                            eDirection = enumDirection.BothBackward;
                            break;

                        case StreamDirections.ForwardIn:
                            eDirection = enumDirection.BackwardForward;
                            break;

                        case StreamDirections.TopIn:
                        case StreamDirections.BotOut:
                            eDirection = enumDirection.BackwardDown;
                            break;

                        case StreamDirections.TopOut:
                        case StreamDirections.BotIn:
                            eDirection = enumDirection.BackwardUp;
                            break;
                    }
                    break;

                case StreamDirections.BackwardIn:
                    switch (StreamDirection(EndNode))
                    {
                        case StreamDirections.ForwardOut:
                            eDirection = enumDirection.BothForward;
                            break;

                        case StreamDirections.BackwardIn:
                            eDirection = enumDirection.BothBackward;
                            break;

                        case StreamDirections.ForwardIn:
                        case StreamDirections.BackwardOut:
                            eDirection = enumDirection.BothForward;
                            break;

                        case StreamDirections.TopIn:
                        case StreamDirections.BotOut:
                            eDirection = enumDirection.BackwardDown;
                            break;

                        case StreamDirections.TopOut:
                        case StreamDirections.BotIn:
                            eDirection = enumDirection.BackwardUp;
                            break;
                    }
                    break;

                case StreamDirections.ForwardOut:
                    switch (StreamDirection(EndNode))
                    {
                        case StreamDirections.ForwardOut:
                            eDirection = enumDirection.BothForward;
                            break;

                        case StreamDirections.ForwardIn:
                            eDirection = enumDirection.BothForward;
                            break;

                        case StreamDirections.BackwardIn:
                            eDirection = enumDirection.ForwardBackward;
                            break;

                        case StreamDirections.TopIn:
                        case StreamDirections.BotOut:
                            eDirection = enumDirection.ForwardDown;
                            break;

                        case StreamDirections.TopOut:
                        case StreamDirections.BotIn:
                            eDirection = enumDirection.BackwardUp;
                            break;
                    }
                    break;

                case StreamDirections.TopOut:
                case StreamDirections.BotIn:
                    switch (StreamDirection(EndNode))
                    {
                        case StreamDirections.ForwardIn:
                        case StreamDirections.ForwardOut:
                            eDirection = enumDirection.UpForward;
                            break;

                        case StreamDirections.BackwardIn:
                        case StreamDirections.BackwardOut:
                            eDirection = enumDirection.UpBackward;
                            break;

                        case StreamDirections.TopIn:
                        case StreamDirections.BotOut:
                            eDirection = enumDirection.UpDown;
                            break;

                        case StreamDirections.TopOut:
                        case StreamDirections.BotIn:
                            eDirection = enumDirection.UpUp;
                            break;
                    }
                    break;

                case StreamDirections.TopIn:
                case StreamDirections.BotOut:
                    switch (StreamDirection(EndNode))
                    {
                        case StreamDirections.ForwardIn:
                        case StreamDirections.ForwardOut:
                            eDirection = enumDirection.DownForward;
                            break;

                        case StreamDirections.BackwardIn:
                        case StreamDirections.BackwardOut:
                            eDirection = enumDirection.DownBackward;
                            break;

                        case StreamDirections.TopIn:
                        case StreamDirections.BotOut:
                            eDirection = enumDirection.DownDown;
                            break;

                        case StreamDirections.TopOut:
                        case StreamDirections.BotIn:
                            eDirection = enumDirection.UpUp;
                            break;
                    }
                    break;
            }

            //eDirection = ro

            return eDirection;
        }

        private enumLocation Location(int StartX, int EndX, int StartY, int EndY)
        {
            enumLocation eLocation = new enumLocation();

            if (EndX >= StartX + 30)
            {
                if (EndY > StartY)

                    eLocation = enumLocation.BelowRight;

                if (EndY < StartY)

                    eLocation = enumLocation.AboveRight;
            }
            else // below
            {
                if (EndY > StartY) // on the left

                    eLocation = enumLocation.BelowLeft;

                if (EndY < StartY)

                    eLocation = enumLocation.AboveLeft;
            }
            return eLocation;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            //  if (startNode is null && EndNode is null)
            //  return;
            //  CalculateOrthoganals(startNode, endNode, StartDrawObject, EndDrawObject);

            g.SmoothingMode = smoothingmode;

            Pen pen;
            if (!active)
                pen = new Pen(Color.Gray, penWidth);
            else
                pen = new Pen(this.StreamColor, penWidth);

            if (pointArray.Count() <= 1)
                return;

            if (this is DrawSignalStream)
            {
                pen.DashCap = DashCap.Round;
                pen.DashPattern = new float[] { 4.0f, 2.0f, 2.0f, 3.0f };
            }
            else if (this.isPumparoud)
            {
                pen.DashCap = DashCap.Round;
                pen.DashPattern = new float[] { 4.0f, 2.0f, 2.0f, 3.0f };
                pen.Color = Color.LightBlue;
            }

            //pointArray = segArray.Points();

            for (int i = 0; i < segArray.Count; i++)
            {
                switch (segArray[i])
                {
                    case SegmentH segH:
                        // if (segH.X1 == segH.X2)
                        //   Debugger.Break();
                        //if (segH is not null)
                        {
                            //    Debug.Print(segH.Point1.ToString());
                            //    Debug.Print(segH.Point2.ToString());
                            g.DrawLine(pen, segH.Point1, segH.Point2);
                        }
                        break;

                    case SegmentV segV:
                        // if (segV.Y1 == segV.Y2)
                        //     Debugger.Break();

                        //if (segV is not null)
                        {
                            //   Debug.Print(segV.Point1.ToString());
                            //   Debug.Print(segV.Point2.ToString());
                            g.DrawLine(pen, segV.Point1, segV.Point2);
                        }
                        break;
                }
            }

            Point[] arrow = new Point[4];

            if (endDrawObject is null)
            {
                Rectangle r = new Rectangle();

                Point point = segArray.PointArray[^1];
                point.Offset(-3, -3);
                r.Location = point;

                r.Size = new Size(6, 6);

                SolidBrush brush = new(StreamColor);

                pen.DashCap = DashCap.Flat;
                pen.DashStyle = DashStyle.Solid;

                brush.Color = pen.Color;

                g.DrawEllipse(pen, r);
                g.FillEllipse(brush, r);
            }
            else
            {
                switch (EndDirection) // draw arrows
                {
                    case EndLineDirection.Right:
                        arrow[0] = pointArray[^1];
                        arrow[1] = arrow[0];
                        arrow[1].Offset(-10, -5);
                        arrow[2] = arrow[0];
                        arrow[2].Offset(-7, 0);
                        arrow[3] = arrow[0];
                        arrow[3].Offset(-10, +5);
                        break;

                    case EndLineDirection.Left:
                        arrow[0] = pointArray[^1];
                        arrow[1] = arrow[0];
                        arrow[1].Offset(+10, -5);
                        arrow[2] = arrow[0];
                        arrow[2].Offset(+7, 0);
                        arrow[3] = arrow[0];
                        arrow[3].Offset(+10, +5);
                        break;

                    case EndLineDirection.Up:
                        arrow[0] = pointArray[^1];
                        arrow[1] = arrow[0];
                        arrow[1].Offset(+5, +10);
                        arrow[2] = arrow[0];
                        arrow[2].Offset(0, +7);
                        arrow[3] = arrow[0];
                        arrow[3].Offset(-5, +10);
                        break;

                    case EndLineDirection.Down:
                        arrow[0] = pointArray[^1];
                        arrow[1] = arrow[0];
                        arrow[1].Offset(+5, -10);
                        arrow[2] = arrow[0];
                        arrow[2].Offset(0, -7);
                        arrow[3] = arrow[0];
                        arrow[3].Offset(-5, -10);
                        break;
                } // Draw Arrow

                SolidBrush brush = new(StreamColor);

                pen.DashCap = DashCap.Flat;
                pen.DashStyle = DashStyle.Solid;

                brush.Color = pen.Color;

                g.DrawPolygon(pen, arrow);
                g.FillPolygon(brush, arrow);
            }

            pen.Dispose();
            //dsn?.Draw(g);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segment"></param>
        public void DeletePoint(int segment)
        {
            if (segment == 1 || segment == 0)
                return;
            if (segment == pointArray.Count() - 1)
                return;
            if (pointArray.Count() <= 3)
                return;

            segArray.RemoveAt(segment);

            if (IsOdd(segment))  // vertical
            {
                pointArray[segment - 1] = new Point(pointArray[segment - 1].X, pointArray[segment - 2].Y);
            }
            else
            {
                pointArray[segment - 1] = new Point(pointArray[segment - 2].X, pointArray[segment - 1].Y);
            }
            Invalidate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Segment"></param>
        public void InsertPoint(int Segment)
        {
            if (Segment < 1)
                return;
            // Streams should allways start and end vertically vertical height is 0 for horizontal streams
            Point p1 = pointArray[Segment - 1];
            Point p2 = pointArray[Segment];
            Point p3;
            Point p4;

            int x = (p1.X + p2.X) / 2;
            int y = (p1.Y + p2.Y) / 2;

            if (IsVertical(Segment))  // vertical
            {
                p3 = new Point(p1.X, y);
                p4 = new Point(x, y);
                p2.X = x;
                pointArray[Segment] = p2;
            }
            else
            {
                p3 = new Point(x, p1.Y);
                p4 = new Point(x, y);
                p2.Y = y;
                pointArray[Segment] = p2;
            }

            segArray.Insert(Segment, new SegmentH());
            segArray.Insert(Segment + 1, new SegmentV());
            Invalidate();
        }

        public int getmiddlesegment()
        {
            int nopoints = pointArray.Count();
            return nopoints / 2;
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Point GetHandleLocation(int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > pointArray.Count())
                handleNumber = pointArray.Count();

            return pointArray[handleNumber - 1];
        }

        // public  static int ? Count { get => count; set => count = value; }

        //public  enumFlashAlgorithm FlashAlgorithm { get => this.stream.FlashAlgorithm; set => FlashAlgorithm = value; }
        /// <summary>
        ///
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            return handleCursor;
        }

        /// <summary>
        /// return   segment Number
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>
        public int GetSegment(Point point)
        {
            for (int n = 0; n < pointArray.Count() - 1; n++)
            {
                if (LineContains(pointArray[n], pointArray[n + 1], point))
                {
                    return n + 1;
                }
            }
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segment"></param>
        /// <return  s></return  s>
        public bool IsVertical(int segment)
        {
            if (segment > pointArray.Count() - 1 || segment == 0)
                return false;
            if (Math.Abs(pointArray[segment].X - pointArray[segment - 1].X) < 5)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <return  s></return  s>
        public bool LineContains(Point a, Point b, Point x) // Streams Must Be vertical or Horizontal
        {
            if (a.X == b.X && Math.Abs(x.X - a.X) < 7)  // Vertical
            {
                return true;
            }

            if (a.Y == b.Y && Math.Abs(x.Y - a.Y) < 7)  // Horizontal
            {
                return true;
            }

            if ((x.X > a.X && x.X > b.X) || (x.X < a.X && x.X < b.X))
            {
                return false;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="TT"></param>

        private static bool IsOdd(int i)
        {
            Math.DivRem(i, 2, out int res);
            if (res > 0)
            {
                return true;
            }

            return false;
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void CreateOrthogonals()
        {
            int OffsetSize = 25, StartX = 0, StartY = 0, EndX = 0, EndY = 0;

            if (startNode != null)
            {
                StartX = (int)startNode.Absolute.Location.X;
                StartY = (int)startNode.Absolute.Location.Y;
            }

            if (endNode != null)
            {
                EndX = (int)endNode.Absolute.Location.X;
                EndY = (int)endNode.Absolute.Location.Y;
            }

            enumDirection eDirection = Directions2(StartNode, EndNode);
            enumLocation eLocation = Location(StartX, EndX, StartY, EndY);

            //segArray.Clear();
            segArray.Reset = false;
            SetUpPointsSegments(OffsetSize, StartX, StartY, EndX, EndY, eDirection, eLocation, this.startDrawObject, this.endDrawObject, true);
        }

        public void UpdateOrthogonals()
        {
            int OffsetSize = 15, StartX = 0, StartY = 0, EndX = 0, EndY = 0;

            if (startNode != null)
            {
                StartX = (int)startNode.Absolute.Location.X;
                StartY = (int)startNode.Absolute.Location.Y;
            }

            if (endNode != null)
            {
                EndX = (int)endNode.Absolute.Location.X;
                EndY = (int)endNode.Absolute.Location.Y;
            }

            if (SegArray.Reset)
            {
                CreateOrthogonals();
            }
            else
            {
                enumDirection eDirection = Directions2(StartNode, EndNode);
                enumLocation eLocation = Location(StartX, EndX, StartY, EndY);

                //segArray.Clear();

                SetUpPointsSegments(OffsetSize, StartX, StartY, EndX, EndY, eDirection, eLocation, this.startDrawObject, this.endDrawObject, false);
            }
        }

        public void ResetOthoganals()
        {
            //pointArray.UnFreeze();
            CreateOrthogonals();
        }

        public void SetUpPointsSegments(int OffsetSize, int StartX, int StartY, int EndX, int EndY,
         enumDirection eDirection, enumLocation eLocation, DrawObject StartObject, DrawObject EndObject, bool setup)
        {
            FixablePointArray parray = new();
            int MidX = (int)((StartX + EndX) / 2.0);
            int MidY = (int)((StartY + EndY) / 2.0);
            int X1, X2, X3, X4, X5, Y1, Y2, Y3, Y4;

            switch (eDirection)
            {
                case enumDirection.BothForward:
                    switch (eLocation)
                    {
                        case enumLocation.AboveRight:
                        case enumLocation.BelowRight:
                            {
                                int count = segArray.Count;

                                if (startDrawObject is DrawObject d)
                                    MidY = Math.Min(MidY, StartY - OffsetSize - d.Rectangle.Height / 2);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FFARBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegArrayType.FFARBR;

                                break;
                            }
                        case enumLocation.BelowLeft:
                            {
                                MidX = Math.Max(MidX, StartX + OffsetSize);

                                if (startDrawObject is DrawObject d)
                                    MidY = Math.Max(MidY, EndY - OffsetSize - d.Rectangle.Height / 2);

                                if (EndObject is DrawRectangle dr && MidY < dr.Rectangle.Bottom && MidY > dr.Rectangle.Top)
                                    MidY = dr.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawRectangle drs && MidY < drs.Rectangle.Bottom && MidY > drs.Rectangle.Top)
                                    MidY = drs.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FFBL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1)); //0 length
                                    segArray.Add(new SegmentV(X3, Y1, Y2)); // to mid Y
                                    segArray.Add(new SegmentH(X3, X4, Y2)); //offset
                                    segArray.Add(new SegmentV(X4, Y2, Y3)); //offset
                                    segArray.Add(new SegmentH(X4, X5, Y3)); //end
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[1].Update(new SegmentV(X3, Y1, Y2));
                                    segArray[^3].Update(new SegmentH(X3, X4, Y2));
                                    segArray[^2].Update(new SegmentV(X4, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegArrayType.FFBL;

                                break;
                            }
                        case enumLocation.AboveLeft:
                            {
                                MidX = Math.Max(MidX, StartX + OffsetSize);

                                if (startDrawObject is DrawObject d)
                                    MidY = Math.Max(MidY, EndY - OffsetSize - d.Rectangle.Height / 2);

                                if (EndObject is DrawRectangle dr && MidY < dr.Rectangle.Bottom && MidY > dr.Rectangle.Top)
                                    MidY = dr.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawRectangle drs && MidY < drs.Rectangle.Bottom && MidY > drs.Rectangle.Top)
                                    MidY = drs.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FFAL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1)); //0 length
                                    segArray.Add(new SegmentV(X3, Y1, Y2)); // to mid Y
                                    segArray.Add(new SegmentH(X3, X4, Y2)); //offset
                                    segArray.Add(new SegmentV(X4, Y2, Y3)); //offset
                                    segArray.Add(new SegmentH(X4, X5, Y3)); //end
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[1].Update(new SegmentV(X3, Y1, Y2));
                                    segArray[^3].Update(new SegmentH(X3, X4, Y2));
                                    segArray[^2].Update(new SegmentV(X4, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegArrayType.FFAL;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Right;
                    break;

                case enumDirection.ForwardBackward:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                            {
                                if (EndObject is DrawObject eo)
                                {
                                    if (MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                        MidY = eo.Rectangle.Top - OffsetSize;
                                    if (MidX < eo.Rectangle.Right)
                                        MidX = eo.Rectangle.Right + OffsetSize;
                                }

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = StartY + OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FBBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y4));
                                    segArray.Add(new SegmentH(X3, X5, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[1].Update(new SegmentV(X3, Y1, Y4));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FBBR;

                                break;
                            }
                        case enumLocation.BelowLeft:
                            {
                                if (EndObject is DrawObject eo)
                                {
                                    if (MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                        MidY = eo.Rectangle.Top - OffsetSize;
                                    if (MidX < eo.Rectangle.Right)
                                        MidX = eo.Rectangle.Right + OffsetSize;
                                }

                                //if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //    MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, StartX + OffsetSize);
                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                //segArray.Add(new SegmentH(StartX, StartX + OffsetSize, StartY));
                                //segArray.Add(new SegmentV(MidX, StartY, EndY));
                                //segArray.Add(new SegmentH(MidX, EndX, EndY));

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = StartY + OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FBBL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y4));
                                    segArray.Add(new SegmentH(X3, X5, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[1].Update(new SegmentV(X3, Y1, Y4));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FBBL;

                                break;
                            }
                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, StartX + OffsetSize);
                                MidX = Math.Max(MidX, EndX + OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FBARAL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FBARAL;

                                break;
                            }

                        default:
                            break;
                    }

                    endDirection = EndLineDirection.Left;
                    break;

                case enumDirection.ForwardDown:
                    EndDirection = EndLineDirection.Down;
                    switch (eLocation)
                    {
                        case enumLocation.AboveLeft:
                            {
                                // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                //   MidY = eo.Rectangle.Top - OffsetSize;

                                //   if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //        MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;

                                MidX = Math.Max(MidX, X2);

                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY - OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FDAL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray.Add(new SegmentV(X3, Y1, Y2)); // vertical
                                    segArray.Add(new SegmentH(X3, X4, Y2)); // backwards
                                    segArray.Add(new SegmentV(X4, Y2, Y3)); // offset
                                    segArray.Add(new SegmentH(X4, X5, Y3)); // offset
                                    segArray.Add(new SegmentV(X5, Y3, Y4)); // down
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray[1].Update(new SegmentV(X3, Y1, Y2)); // offset
                                    segArray[^4].Update(new SegmentH(X3, X4, Y2)); // vertical
                                    segArray[^3].Update(new SegmentV(X4, Y2, Y3)); // backwards
                                    segArray[^2].Update(new SegmentH(X4, X5, Y3)); // backwards
                                    segArray[^1].Update(new SegmentV(X5, Y3, Y4)); // backwards
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FDAL;

                                break;
                            }
                        case enumLocation.BelowLeft:
                            {
                                // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                //   MidY = eo.Rectangle.Top - OffsetSize;

                                //   if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //        MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;

                                MidX = Math.Max(MidX, X2);

                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY - OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FDBL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray.Add(new SegmentV(X3, Y1, Y2)); // vertical
                                    segArray.Add(new SegmentH(X3, X5, Y2)); // backwards
                                    segArray.Add(new SegmentV(X5, Y2, Y4)); // offs
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray[1].Update(new SegmentV(X3, Y1, Y2)); // offset
                                    segArray[^2].Update(new SegmentH(X3, X5, Y2)); // vertical
                                    segArray[^1].Update(new SegmentV(X5, Y2, Y4)); // backwards
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FDBL;

                                break;
                            }

                        case enumLocation.AboveRight:
                        case enumLocation BelowRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = EndY - OffsetSize;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FDALAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1)); // Too offset
                                    segArray.Add(new SegmentV(X3, Y1, Y2)); // up
                                    segArray.Add(new SegmentH(X3, X5, Y2)); // across
                                    segArray.Add(new SegmentV(X5, Y2, Y3)); // down
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y2));
                                    segArray[^2].Update(new SegmentH(X3, X5, Y2));
                                    segArray[^1].Update(new SegmentV(X5, Y2, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FDALAR;

                                break;
                            }
                    }
                    break;

                case enumDirection.ForwardUp:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:
                            {
                                // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                //   MidY = eo.Rectangle.Top - OffsetSize;

                                //   if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //        MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                //   MidY = eo.Rectangle.Top - OffsetSize;

                                //   if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //        MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;

                                MidX = Math.Max(MidX, X2);

                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY + OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FUBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray.Add(new SegmentV(X3, Y1, Y2)); // vertical
                                    segArray.Add(new SegmentH(X4, X5, Y3)); // offset
                                    segArray.Add(new SegmentV(X5, Y3, Y4)); // down
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray[1].Update(new SegmentV(X3, Y1, Y2)); // offset
                                    segArray[^2].Update(new SegmentH(X4, X5, Y3)); // backwards
                                    segArray[^1].Update(new SegmentV(X5, Y3, Y4)); // backwards
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FUBLBR;
                                EndDirection = EndLineDirection.Up;
                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                //   MidY = eo.Rectangle.Top - OffsetSize;

                                //   if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //        MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;

                                MidX = Math.Max(MidX, X2);

                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY - OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.FDBL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X5, Y1)); // to offset
                                    segArray.Add(new SegmentV(X5, Y1, Y4)); // vertical
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1)); // to offset
                                    segArray[^1].Update(new SegmentV(X3, Y1, Y4)); // offset
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.FUALAR;

                                break;
                            }

                        default:
                            endDirection = EndLineDirection.Down;
                            break;
                    }
                    break;

                case enumDirection.BothBackward:
                    switch (eLocation)
                    {
                        case enumLocation.AboveLeft:
                        case enumLocation.BelowLeft:

                            X1 = StartX;
                            X2 = StartX - OffsetSize;
                            X3 = MidX;
                            X4 = EndX + OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup || segArray.segArrayType != SegArrayType.BBALBL)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X3, Y1));
                                segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^1].Update(new SegmentH(X3, X5, Y3));
                            }
                            segArray.segArrayType = SegmentArray.SegArrayType.BBALBL;
                            break;

                        case enumLocation.AboveRight:
                        case enumLocation.BelowRight:
                            MidX = Math.Min(MidX, StartX - OffsetSize);
                            if (startDrawObject is not null)
                                MidY = Math.Min(MidY, EndY + OffsetSize - startDrawObject.Rectangle.Height / 2);
                            if (endDrawObject is not null)
                                MidY = Math.Min(MidY, EndY - OffsetSize - endDrawObject.Rectangle.Height);

                            X1 = StartX;
                            X2 = StartX - OffsetSize;
                            X3 = MidX;
                            X4 = EndX + OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X4, Y3));
                                segArray.Add(new SegmentH(X4, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X3, Y1));
                                segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                segArray[^1].Update(new SegmentH(X4, X5, Y3));
                            }
                            segArray.segArrayType = SegmentArray.SegArrayType.BBARBR;
                            break;

                        default:
                            break;
                    }

                    endDirection = EndLineDirection.Left;
                    break;

                case enumDirection.BackwardUp:
                    EndDirection = EndLineDirection.Up;
                    switch (eLocation)
                    {
                        case enumLocation.BelowLeft:
                            if (endDrawObject is not null)
                                MidY = Math.Max(MidY, EndY + OffsetSize);

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup || segArray.segArrayType != SegArrayType.BUBL)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y2));
                                segArray.Add(new SegmentH(X3, X5, Y2));
                                segArray.Add(new SegmentV(X5, Y2, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X3, Y1));
                                segArray[1].Update(new SegmentV(X3, Y1, Y2));
                                segArray[^2].Update(new SegmentH(X3, X5, Y2));
                                segArray[^1].Update(new SegmentV(X5, Y2, Y3));
                            }
                            segArray.segArrayType = SegmentArray.SegArrayType.BUBL;
                            break;

                        case enumLocation.AboveLeft:

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup || segArray.segArrayType != SegArrayType.BUAL)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X5, Y1));
                                segArray.Add(new SegmentV(X5, Y1, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X5, Y1));
                                segArray[^1].Update(new SegmentV(X5, Y1, Y3));
                            }
                            segArray.segArrayType = SegmentArray.SegArrayType.BUAL;
                            break;

                        case enumLocation.AboveRight:
                        case enumLocation.BelowRight:
                            MidX = Math.Min(MidX, StartX - OffsetSize);
                            if (startDrawObject is not null)
                                MidY = Math.Min(MidY, EndY + OffsetSize - startDrawObject.Rectangle.Height / 2);

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X3, Y1));
                                segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^1].Update(new SegmentH(X3, X5, Y3));
                            }
                            segArray.segArrayType = SegmentArray.SegArrayType.BUBLBR;
                            break;

                        default:
                            break;
                    }

                    endDirection = EndLineDirection.Up;
                    break;

                case enumDirection.BackwardForward:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:
                            {
                                MidX = Math.Min(MidX, StartX - OffsetSize);
                                MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.BFBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X4, Y3));
                                    segArray.Add(new SegmentH(X4, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.BFBLBR;

                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                MidX = Math.Min(MidX, StartX - OffsetSize);
                                MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X4, Y3));
                                    segArray.Add(new SegmentH(X4, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.BFALAR;

                                break;
                            }

                        default:
                            break;
                    }

                    endDirection = EndLineDirection.Right;
                    break;

                case enumDirection.BackwardDown:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                if (EndObject is DrawObject eo1 && MidY < eo1.Rectangle.Bottom && MidY > eo1.Rectangle.Top)
                                    MidY = eo1.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so1 && MidY < so1.Rectangle.Bottom && MidY > so1.Rectangle.Top)
                                    MidY = so1.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX);

                                X1 = StartX;
                                X2 = StartX - OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY - OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.BDBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X2, Y1));
                                    segArray.Add(new SegmentV(X2, Y1, Y2));
                                    segArray.Add(new SegmentH(X2, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                    segArray.Add(new SegmentV(X5, Y3, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X2, Y1));
                                    segArray[1].Update(new SegmentV(X2, Y1, Y2));
                                    segArray[2].Update(new SegmentH(X2, X3, Y1));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^2].Update(new SegmentH(X5, X5, Y3));
                                    segArray[^1].Update(new SegmentV(X5, Y3, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.BDBR;

                                break;
                            }
                        case enumLocation.BelowLeft:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                if (EndObject is DrawObject eo1 && MidY < eo1.Rectangle.Bottom && MidY > eo1.Rectangle.Top)
                                    MidY = eo1.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so1 && MidY < so1.Rectangle.Bottom && MidY > so1.Rectangle.Top)
                                    MidY = so1.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX + OffsetSize);

                                X1 = StartX;
                                X2 = StartX - OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY - OffsetSize;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.BDBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                    segArray.Add(new SegmentV(X5, Y3, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[1].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^2].Update(new SegmentH(X3, X5, Y3));
                                    segArray[^1].Update(new SegmentV(X5, Y3, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.BDBLBR;

                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                // if (EndObject is DrawObject eo && MidX > eo.Rectangle.Left && MidY < eo.Rectangle.Right)
                                //     MidX = eo.Rectangle.Left - OffsetSize;

                                //  if (StartObject is DrawObject so && MidX > so.Rectangle.Left && MidY < so.Rectangle.Right)
                                //      MidX = so.Rectangle.Left - OffsetSize;

                                //MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX - OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                int Y1a = StartY - OffsetSize * 2;
                                Y2 = EndY - OffsetSize;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.BDALAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X2, Y1));
                                    segArray.Add(new SegmentV(X1, Y1, Y1a));
                                    segArray.Add(new SegmentH(X2, X3, Y1a));
                                    segArray.Add(new SegmentV(X3, Y1a, Y2));
                                    segArray.Add(new SegmentH(X3, X5, Y2));
                                    segArray.Add(new SegmentV(X5, Y2, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[1].Update(new SegmentV(X2, Y1, Y1a));
                                    segArray[2].Update(new SegmentH(X2, X3, Y1a));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y2));
                                    segArray[^2].Update(new SegmentH(X3, X5, Y2));
                                    segArray[^1].Update(new SegmentV(X5, Y2, Y3));
                                }

                                segArray.segArrayType = SegArrayType.BDALAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Down;
                    break;

                case enumDirection.DownDown:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup || segArray.segArrayType != SegArrayType.DDBLBR)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X4, Y3));
                                segArray.Add(new SegmentH(X4, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X3, Y1));
                                segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                segArray[^1].Update(new SegmentH(X4, X5, Y3));
                            }

                            segArray.segArrayType = SegmentArray.SegArrayType.DDBLBR;

                            break;

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            if (StartObject is DrawObject so1 && MidX < so1.Rectangle.Right && MidX > so1.Rectangle.Left)
                                MidX = so1.Rectangle.Left - OffsetSize;

                            if (EndObject is DrawObject so2 && MidX < so2.Rectangle.Right && MidX > so2.Rectangle.Left)
                                MidX = so2.Rectangle.Left - OffsetSize;

                            if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                MidY = eo.Rectangle.Top - OffsetSize;

                            if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                MidY = so.Rectangle.Top - OffsetSize;

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X4, Y3));
                                segArray.Add(new SegmentH(X4, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X3, Y1));
                                segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                segArray[^1].Update(new SegmentH(X4, X5, Y3));
                            }

                            segArray.segArrayType = SegmentArray.SegArrayType.DDALAR;

                            break;

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Down;
                    break;

                case enumDirection.DownUp:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:
                            {
                                if (StartObject is DrawObject so1 && MidX < so1.Rectangle.Right && MidX > so1.Rectangle.Left)
                                    MidX = so1.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject so2 && MidX < so2.Rectangle.Right && MidX > so2.Rectangle.Left)
                                    MidX = so2.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.DUBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X4, Y3));
                                    segArray.Add(new SegmentH(X4, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DUBLBR;

                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                if (StartObject is DrawObject so1 && MidX < so1.Rectangle.Right && MidX > so1.Rectangle.Left)
                                    MidX = so1.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject so2 && MidX < so2.Rectangle.Right && MidX > so2.Rectangle.Left)
                                    MidX = so2.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X4, Y3));
                                    segArray.Add(new SegmentH(X4, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^3].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^2].Update(new SegmentH(X3, X4, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DUALAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Up;
                    break;

                case enumDirection.DownForward:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = StartY + OffsetSize;
                                Y3 = MidY;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.DFBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y2, Y4));
                                    segArray.Add(new SegmentH(X3, X5, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X1, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y2, Y4));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DFBLBR;

                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = StartY + OffsetSize;
                                Y3 = MidY;
                                Y4 = EndY;

                                if (setup || SegArray.segArrayType != SegArrayType.DFAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y2, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X1, X3, Y1));
                                    segArray[^2].Update(new SegmentV(X3, Y1, Y4));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DFAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Right;
                    break;

                case enumDirection.DownBackward:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                            {
                                endDirection = EndLineDirection.Right;
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX + OffsetSize);

                                X1 = StartX;
                                X2 = StartX;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = StartY + OffsetSize;
                                Y3 = MidY;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.DBBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y3, Y4));
                                    segArray.Add(new SegmentH(X4, X5, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X1, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y3, Y4));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DBBR;
                            }
                            break;

                        case enumLocation.BelowLeft:
                            {
                                endDirection = EndLineDirection.Right;
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = StartY + OffsetSize;
                                Y3 = MidY;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.DBBL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X2, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y3, Y4));
                                    segArray.Add(new SegmentH(X4, X5, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X2, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y3, Y4));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DBBL;

                                break;
                            }

                        case enumLocation.AboveLeft:
                            {
                                endDirection = EndLineDirection.Right;
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = StartY + OffsetSize;
                                Y3 = MidY;
                                Y4 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.DBAL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y3, Y4));
                                    segArray.Add(new SegmentH(X4, X5, Y4));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X1, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y3, Y4));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y4));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DBAL;

                                break;
                            }
                        case enumLocation.AboveRight:
                            {
                                endDirection = EndLineDirection.Right;

                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX + OffsetSize);

                                X1 = StartX;
                                X2 = StartX;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = StartY + OffsetSize;
                                Y3 = EndY;

                                Debug.Print(EndY.ToString());

                                if (setup || segArray.segArrayType != SegArrayType.DBAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y2, Y3));
                                    segArray.Add(new SegmentH(X4, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X1, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.DBAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Left;
                    break;

                case enumDirection.UpUp:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup || segArray.segArrayType != SegArrayType.UUBLBR)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X2, Y1));
                                segArray.Add(new SegmentH(X2, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X2, Y1));
                                segArray[1].Update(new SegmentH(X2, X3, Y1));
                                segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^1].Update(new SegmentH(X3, X5, Y3));
                            }

                            segArray.segArrayType = SegmentArray.SegArrayType.UUBLBR;

                            break;

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            if (StartObject is DrawObject so1 && MidX < so1.Rectangle.Right && MidX > so1.Rectangle.Left)
                                MidX = so1.Rectangle.Left - OffsetSize;

                            if (EndObject is DrawObject so2 && MidX < so2.Rectangle.Right && MidX > so2.Rectangle.Left)
                                MidX = so2.Rectangle.Left - OffsetSize;

                            if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                MidY = eo.Rectangle.Top - OffsetSize;

                            if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                MidY = so.Rectangle.Top - OffsetSize;

                            X1 = StartX;
                            X2 = StartX + OffsetSize;
                            X3 = MidX;
                            X4 = EndX - OffsetSize;
                            X5 = EndX;

                            Y1 = StartY;
                            Y2 = MidY;
                            Y3 = EndY;

                            if (setup)
                            {
                                segArray.Clear();
                                segArray.Add(new SegmentH(X1, X2, Y1));
                                segArray.Add(new SegmentH(X2, X3, Y1));
                                segArray.Add(new SegmentV(X3, Y1, Y3));
                                segArray.Add(new SegmentH(X3, X5, Y3));
                            }
                            else
                            {
                                segArray[0].Update(new SegmentH(X1, X2, Y1));
                                segArray[1].Update(new SegmentH(X2, X3, Y1));
                                segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                segArray[^1].Update(new SegmentH(X3, X5, Y3));
                            }
                            segArray.segArrayType = SegmentArray.SegArrayType.UUALAR;

                            break;

                        default:
                            break;
                    }

                    endDirection = EndLineDirection.Up;
                    break;

                case enumDirection.UpDown:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                        case enumLocation.BelowLeft:
                            {
                                if (StartObject is DrawObject so1 && MidX < so1.Rectangle.Right && MidX > so1.Rectangle.Left)
                                    MidX = so1.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject so2 && MidX < so2.Rectangle.Right && MidX > so2.Rectangle.Left)
                                    MidX = so2.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.UDBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X2, Y1));
                                    segArray.Add(new SegmentH(X2, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X2, Y1));
                                    segArray[1].Update(new SegmentH(X2, X3, Y1));
                                    segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UDBLBR;

                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                if (StartObject is DrawObject so1 && MidX < so1.Rectangle.Right && MidX > so1.Rectangle.Left)
                                    MidX = so1.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject so2 && MidX < so2.Rectangle.Right && MidX > so2.Rectangle.Left)
                                    MidX = so2.Rectangle.Left - OffsetSize;

                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > eo.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.UDALAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentH(X1, X2, Y1));
                                    segArray.Add(new SegmentH(X2, X3, Y1));
                                    segArray.Add(new SegmentV(X3, Y1, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentH(X1, X2, Y1));
                                    segArray[1].Update(new SegmentH(X2, X3, Y1));
                                    segArray[^2].Update(new SegmentV(X3, Y1, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UDALAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Down;
                    break;

                case enumDirection.UpForward:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(StartX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                int Y1a = Y1 - OffsetSize;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.UFBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y1a, Y2));
                                    segArray.Add(new SegmentV(X3, Y2, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y1a));
                                    segArray[1].Update(new SegmentH(X1, X3, Y2));
                                    segArray[2].Update(new SegmentV(X3, Y1a, Y2));
                                    segArray[3].Update(new SegmentH(X3, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UFBR;

                                break;
                            }
                        case enumLocation.BelowLeft:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.UFBLBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X2, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y2, Y3));
                                    segArray.Add(new SegmentH(X3, X4, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X2, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UFBLBR;

                                break;
                            }

                        case enumLocation.AboveLeft:
                        case enumLocation.AboveRight:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Min(MidX, EndX - OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX - OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.UFARAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));
                                    segArray.Add(new SegmentH(X1, X3, Y2));
                                    segArray.Add(new SegmentV(X3, Y2, Y3));
                                    segArray.Add(new SegmentH(X3, X5, Y3));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new SegmentH(X1, X3, Y2));
                                    segArray[^2].Update(new SegmentV(X3, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X3, X5, Y3));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UFARAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Right;
                    break;

                case enumDirection.UpBackward:
                    switch (eLocation)
                    {
                        case enumLocation.BelowRight:
                            {
                               // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                               //     MidY = eo.Rectangle.Top - OffsetSize;

                                //if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //    MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                if (setup || segArray.segArrayType != SegArrayType.UBBR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(StartX, StartY, StartY - OffsetSize));
                                    segArray.Add(new SegmentH(StartX, MidX, StartY - OffsetSize));
                                    //segArray.Add(new SegmentV(StartX, MidX, MidY));
                                    //segArray.Add(new SegmentH(MidX, EndX, EndY - OffsetSize));
                                    segArray.Add(new SegmentV(EndX, EndY - OffsetSize, EndY));
                                    segArray.Add(new SegmentH(EndX + OffsetSize, EndX, EndY));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(StartX, StartY, StartY - OffsetSize));
                                    segArray[1].Update(new SegmentH(StartX, MidX, StartY - OffsetSize));
                                    //segArray[2].Update(new SegmentV(MidX, StartY - OffsetSize, MidY));
                                   // segArray[^3].Update(new SegmentH(MidX, EndX+OffsetSize, MidY));
                                    segArray[^2].Update(new SegmentV(EndX + OffsetSize, EndY - OffsetSize, EndY));
                                    segArray[^1].Update(new SegmentH(EndX + OffsetSize, EndX, EndY));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UBBR;

                                break;
                            }
                        case enumLocation.BelowLeft:
                            {
                                //if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                                 //   MidY = eo.Rectangle.Top - OffsetSize;

                               // if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                               //     MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                if (setup || segArray.segArrayType != SegArrayType.UBBL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(StartX, StartY, StartY - OffsetSize));
                                    segArray.Add(new SegmentH(StartX, MidX, StartY - OffsetSize));
                                    segArray.Add(new SegmentV(StartX, StartY - OffsetSize, EndY));
                                    segArray.Add(new SegmentH(MidX, EndX, EndY));
                                   // segArray.Add(new SegmentV(EndX, EndY - OffsetSize, EndY));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(StartX, StartY, StartY - OffsetSize));
                                    segArray[1].Update(new SegmentH(StartX, MidX, StartY - OffsetSize));
                                    segArray[^2].Update(new SegmentV(MidX, StartY - OffsetSize, EndY));
                                    segArray[^1].Update(new SegmentH(MidX, EndX, EndY));
                                   // segArray[^1].Update(new SegmentV(EndX, EndY - OffsetSize, EndY));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UBBL;

                                break;
                            }

                        case enumLocation.AboveLeft:
                            {
                                if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                                    MidY = eo.Rectangle.Top - OffsetSize;

                                if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                    MidY = so.Rectangle.Top - OffsetSize;

                                MidX = Math.Max(MidX, EndX + OffsetSize);

                                if (setup || segArray.segArrayType != SegArrayType.UBAL)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(StartX, StartY, EndY));
                                    segArray.Add(new SegmentH(StartX, EndX, EndY));
                                }
                                else
                                {
                                    segArray[0].Update(new SegmentV(StartX, StartY, EndY));
                                    segArray[^1].Update(new SegmentH(StartX, EndX, EndY));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UBAL;

                                break;
                            }
                        case enumLocation.AboveRight:
                            {
                                // if (EndObject is DrawObject eo && MidY < eo.Rectangle.Bottom && MidY > EndObject.Rectangle.Top)
                                //     MidY = eo.Rectangle.Top - OffsetSize;

                                //if (StartObject is DrawObject so && MidY < so.Rectangle.Bottom && MidY > so.Rectangle.Top)
                                //    MidY = so.Rectangle.Top - OffsetSize;

                                //MidX = Math.Max(MidX, EndX + OffsetSize);

                                X1 = StartX;
                                X2 = StartX + OffsetSize;
                                X3 = MidX;
                                X4 = EndX + OffsetSize;
                                X5 = EndX;

                                Y1 = StartY;
                                Y2 = MidY;
                                Y3 = EndY;

                                if (setup || segArray.segArrayType != SegArrayType.UBAR)
                                {
                                    segArray.Clear();
                                    segArray.Add(new SegmentV(X1, Y1, Y2));                                   
                                    segArray.Add(new SegmentH(X1, X4, Y2));
                                    segArray.Add(new SegmentV(X4, Y2, Y3));
                                    segArray.Add(new SegmentH(X4, X5, Y3));
                                    //segArray.Add(new SegmentV(EndX, EndY - OffsetSize, EndY));
                                }
                                else
                                {
                                    segArray[0].Update(new  SegmentV(X1, Y1, Y2));
                                    segArray[1].Update(new  SegmentH(X1, X4, Y2));
                                    segArray[^2].Update(new SegmentV(X4, Y2, Y3));
                                    segArray[^1].Update(new SegmentH(X4, X5, Y3));
                                    //segArray[^1].Update(new SegmentV(EndX, EndY - OffsetSize, EndY));
                                }

                                segArray.segArrayType = SegmentArray.SegArrayType.UBAR;

                                break;
                            }

                        default:
                            break;
                    }
                    endDirection = EndLineDirection.Left;
                    break;

                default:
                    break;
            }
            SegArray.Recalculate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber == 1)
            {
                if (startNode is not null)
                    startNode.SetAbsolutePosition = point;
            }
            else if (handleNumber == pointArray.Count())
            {
                if (endNode is not null)
                    endNode.SetAbsolutePosition = point;
            }

            UpdateOrthogonals();
            Invalidate();
        }

        public override void MoveHandleTo(Point point, FrontBack handle)
        {
            switch(handle)
                {
                case FrontBack.Front:
                    if (startNode is not null)
                        startNode.SetAbsolutePosition = point;
                    break;
                case FrontBack.Back:
                    if (endNode is not null)
                        endNode.SetAbsolutePosition = point;
                    break;

            }

            UpdateOrthogonals();
            Invalidate();
        }
    }
}