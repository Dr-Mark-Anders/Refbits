using ModelEngine;
using Extensions;

using ModelEngine;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class DrawTray : DrawRectangle, ISerializable
    {
        private Tray tray = new(null);
        public double Efficiency = 70;
        private DrawColumnTraySection owner;
        public bool MakeVisible = true;

        public DrawTray(DrawColumnTraySection parent = null) : this(0, 0, 1, 1)
        {
            this.owner = parent;
        }

        public Node FeedLeft { get; internal set; }

        public Node FeedRight { get; internal set; }
        public Node LiquidDrawRight { get; internal set; }
        public Node LiquidDrawLeft { get; internal set; }
        public Node VapourDraw { get; internal set; }
        public Node WaterDraw { get; internal set; }

        public DrawTray(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            FeedLeft = Hotspots.Add(new Node(this, 0f, 0.5f, "FeedLeft", NodeDirections.Right, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            FeedRight = Hotspots.Add(new Node(this, 1f, 0.5f, "FeedRight", NodeDirections.Left, HotSpotType.FeedRight, HotSpotOwnerType.DrawRectangle));

            LiquidDrawLeft = Hotspots.Add(new Node(this, 0f, 0.25f, "LiquidDrawLeft", NodeDirections.Left, HotSpotType.LiquidDrawLeft, HotSpotOwnerType.DrawRectangle));
            LiquidDrawRight = Hotspots.Add(new Node(this, 1f, 0.25f, "LiquidDrawRight", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));

            VapourDraw = Hotspots.Add(new Node(this, 1f, 0.1f, "VapourDraw", NodeDirections.Right, HotSpotType.VapourDraw, HotSpotOwnerType.DrawRectangle));
            WaterDraw = Hotspots.Add(new Node(this, 1f, 1f, "WaterDraw", NodeDirections.Right, HotSpotType.Water, HotSpotOwnerType.DrawRectangle));

            Initialize();

            this.Name = "V" + Count.ToString();
            Count++;
        }

        private static int Count = 0;

        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawTray drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public override void DrawHotSpot(Graphics g)
        {
            base.DrawHotSpot(g);
        }

        public bool IsConnected()
        {
            if (FeedLeft.HasStream || FeedRight.HasStream || LiquidDrawLeft.HasStream || LiquidDrawRight.HasStream)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        /// 

        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                Color col2 = Color.White;
                Color col1 = Color.Gray;
                Pen pen = new(StreamColor, PenWidth);

                GraphicsPath gp = new(FillMode.Winding);

                Point[] Polygon = new Point[4];
                Polygon[0] = new Point(R.TopLeft().X, R.TopLeft().Y);
                Polygon[1] = new Point(R.BottomLeft().X, R.BottomLeft().Y);
                Polygon[2] = new Point(R.TopRight().X, R.TopRight().Y);
                Polygon[3] = new Point(R.BottomRight().X, R.BottomRight().Y);

                gp.AddPolygon(Polygon);

                if (Angle != enumRotationAngle.a0)
                    Rotate(gp, rectangle.Center());

                if (FlipHorizontal)
                {
                    Matrix m = new();
                    m.Scale(-1, 1);
                    m.Translate(RotatedRectangle.Width + 2 * RotatedRectangle.Left, 0, MatrixOrder.Append);
                    gp.Transform(m);
                }

                if (FlipVertical)
                {
                    Matrix m = new();
                    m.Scale(1, -1);
                    m.Translate(0, RotatedRectangle.Height + 2 * RotatedRectangle.Top, MatrixOrder.Append);
                    gp.Transform(m);
                }

                PathGradientBrush BotBrush = new(gp);

                BotBrush.CenterPoint = new PointF((RotatedRectangle.Left + RotatedRectangle.Right) / 2, (RotatedRectangle.Top + RotatedRectangle.Bottom) / 2);
                BotBrush.CenterColor = col2;
                BotBrush.SurroundColors = new Color[] { col1 };

                g.FillPath(BotBrush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
        }

        public Color Colour { get; internal set; }

        public NodeCollection HotSpots
        { get { return Hotspots; } }

        public UOMProperty P
        {
            get { return new UOMProperty(tray.P); }
            set { tray.P.BaseValue = value; }
        }

        public Node PaDraw { get; internal set; }
        public Node Pareturn { get; internal set; }
        internal DrawColumnTraySection Owner { get => owner; set => owner = value; }
        public Tray Tray { get => tray; set => tray = value; }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawTray(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Efficiency = info.GetDouble("Efficiency");
            try
            {
                tray = (Tray)info.GetValue("tray", typeof(Tray));
            }
            catch { };
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("tray", tray);
            info.AddValue("Efficiency", Efficiency);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            FeedLeft = this.Hotspots.Search("FeedLeft");
            LiquidDrawLeft = this.Hotspots.Search("LiquidDrawLeft");
            FeedRight = this.Hotspots.Search("FeedRight");
            LiquidDrawRight = this.Hotspots.Search("LiquidDrawRight");
            VapourDraw = this.Hotspots.Search("VapourDraw");
            WaterDraw = this.Hotspots.Search("WaterDraw");

            if (FeedLeft is null)
                FeedLeft = Hotspots.Add(new Node(this, 0f, 0.5f, "FeedLeft", NodeDirections.Right, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));

            if (FeedRight is null)
                FeedRight = Hotspots.Add(new Node(this, 1f, 0.5f, "FeedRight", NodeDirections.Left, HotSpotType.FeedRight, HotSpotOwnerType.DrawRectangle));

            if (LiquidDrawLeft is null)
                LiquidDrawLeft = Hotspots.Add(new Node(this, 0f, 0.75f, "LiquidDrawLeft", NodeDirections.Left, HotSpotType.LiquidDrawLeft, HotSpotOwnerType.DrawRectangle));

            if (LiquidDrawRight is null)
                LiquidDrawRight = Hotspots.Add(new Node(this, 1f, 0.75f, "LiquidDrawRight", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));

            if (VapourDraw is null)
                VapourDraw = Hotspots.Add(new Node(this, 1f, 0.1f, "VapourDraw", NodeDirections.Right, HotSpotType.VapourDraw, HotSpotOwnerType.DrawRectangle));

            if (WaterDraw is null)
                WaterDraw = Hotspots.Add(new Node(this, 1f, 1f, "WaterDraw", NodeDirections.Right, HotSpotType.Water, HotSpotOwnerType.DrawRectangle));

            LiquidDrawRight.Relative = new PointF(1, 0.5f);
            LiquidDrawLeft.Relative = new PointF(0, 0.5f);
            FeedRight.Relative = new PointF(1, 0.75f);
            FeedLeft.Relative = new PointF(0, 0.75f);
            VapourDraw.Relative = new PointF(1, 0.25f);
            VapourDraw.NodeType = HotSpotType.VapourDraw;
            WaterDraw.Relative = new PointF(1, 1f);
            LiquidDrawLeft.NodeType = HotSpotType.LiquidDrawLeft;
        }

        internal void UpdateNodeGuids()
        {
            FeedLeft.PortGuid = tray.feed.Guid;
            FeedRight.PortGuid = tray.feed.Guid;
            //LiquidDrawLeft.PortGuid = tray.liquidDrawLeft.Guid;
            LiquidDrawRight.PortGuid = tray.liquidDrawRight.Guid;
            VapourDraw.PortGuid = tray.vapourDraw.Guid;
            WaterDraw.PortGuid = tray.WaterDraw.Guid;
        }
    }
}