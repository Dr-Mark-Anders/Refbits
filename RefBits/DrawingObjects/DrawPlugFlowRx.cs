using Extensions;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    public class DrawPlugFlowRx : DrawRectangle, ISerializable
    {
        public PlugFlowRx plugflow = new();

        private Node Feed;
        private Node Product;

        public double Efficiency = 70;
        private bool fixExitPressure = false;
        private PlugFlowDialog dlg;

        public DrawPlugFlowRx() : this(0, 0, 1, 1)
        {
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return plugflow.IsDirty;
            }
            set
            {
                plugflow.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return plugflow.IsSolved;
            }
            set
            {
                //plugflow.IsSolved = value;
            }
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = plugflow.Ports["In1"].Guid;
            Hotspots["PRODUCT"].PortGuid = plugflow.Ports["Out1"].Guid;

            plugflow.Name = this.Name;
            return true;
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return plugflow.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return plugflow.Ports["Out1"];
            }
        }

        public bool FixExitPressure
        {
            get
            {
                return fixExitPressure;
            }
            set
            {
                fixExitPressure = value;
            }
        }

        public DrawPlugFlowRx(int x, int y, int width, int height)
            : base()
        {
            if (width < 50)
                width = 50;
            if (height < 11)
                height = 11;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Right, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Left, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Initialize();

            this.Name = "V" + Count.ToString();
            Count++;
        }

        private static int Count = 0;

        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawPlugFlowRx drawRectangle = new DrawPlugFlowRx();
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
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                Color col2 = Color.White;
                Color col1 = Color.Gray;
                Pen pen = new Pen(StreamColor, PenWidth);

                GraphicsPath gp = new GraphicsPath(FillMode.Winding);

                Point[] Polygon = new Point[4];
                Polygon[0] = new Point(R.TopLeft().X, R.TopLeft().Y);
                Polygon[1] = new Point(R.BottomLeft().X, R.BottomLeft().Y);
                Polygon[2] = new Point(R.BottomRight().X, R.BottomRight().Y);
                Polygon[3] = new Point(R.TopRight().X, R.TopRight().Y);

                gp.AddPolygon(Polygon);

                if (Angle != enumRotationAngle.a0)
                    Rotate(gp, rectangle.Center());

                if (FlipHorizontal)
                {
                    Matrix m = new Matrix();
                    m.Scale(-1, 1);
                    m.Translate(RotatedRectangle.Width + 2 * RotatedRectangle.Left, 0, MatrixOrder.Append);
                    gp.Transform(m);
                }

                if (FlipVertical)
                {
                    Matrix m = new Matrix();
                    m.Scale(1, -1);
                    m.Translate(0, RotatedRectangle.Height + 2 * RotatedRectangle.Top, MatrixOrder.Append);
                    gp.Transform(m);
                }

                PathGradientBrush BotBrush = new PathGradientBrush(gp);

                BotBrush.CenterPoint = new PointF((RotatedRectangle.Left + RotatedRectangle.Right) / 2, (RotatedRectangle.Top + RotatedRectangle.Bottom) / 2);
                BotBrush.CenterColor = col2;
                BotBrush.SurroundColors = new Color[] { col1 };

                g.FillPath(BotBrush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Node Feed = this.Hotspots.Search("FEED");
            Node Product = this.Hotspots.Search("PRODUCT");

            Feeds.Add(Feed);
            Products.Add(Product);

            dlg = new(plugflow);
            dlg.DLGValueChangedEvent += new(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            this.IsSolved = false;
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawPlugFlowRx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product = this.Hotspots.Search("PRODUCT");
        }
    }
}