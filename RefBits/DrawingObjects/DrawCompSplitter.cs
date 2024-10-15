using ModelEngine;
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
    internal class DrawCompSplitter : DrawRectangle, ISerializable
    {
        private CompSplitterDialog dlg;
        public CompSplitter compsplitter = new();

        public Node Feed;
        public Node Product1 { get; private set; }
        public Node Product2 { get; private set; }
        public Node Product3 { get; private set; }
        public Node Product4 { get; private set; }
        public Node Product5 { get; private set; }

        public UOMProperty divratio = new(ePropID.NullUnits, 0.5);
        private static int Count;

        public Port PortIn
        {
            get
            {
                return compsplitter.PortIn;
            }
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = compsplitter.PortIn.Guid;
            Hotspots["PRODUCT1"].PortGuid = compsplitter.PortOut1.Guid;
            Hotspots["PRODUCT2"].PortGuid = compsplitter.PortOut2.Guid;
            Hotspots["PRODUCT3"].PortGuid = compsplitter.PortOut3.Guid;
            Hotspots["PRODUCT4"].PortGuid = compsplitter.PortOut4.Guid;
            Hotspots["PRODUCT5"].PortGuid = compsplitter.PortOut5.Guid;

            compsplitter.Name = this.Name;
            return true;
        }

        public PortList PortListOut
        {
            get
            {
                return compsplitter.GetPorts(FlowDirection.OUT);
            }
        }

        public DrawCompSplitter() : this(0, 0, 1, 1)
        {
        }

        public DrawCompSplitter(int x, int y, int width, int height)
            : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Product1 = Hotspots.Add(new Node(this, 1f, 0f, "PRODUCT1", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Product2 = Hotspots.Add(new Node(this, 1f, 1f, "PRODUCT2", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Product3 = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT3", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Product4 = Hotspots.Add(new Node(this, 1f, 0.25f, "PRODUCT4", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Product5 = Hotspots.Add(new Node(this, 1f, 0.75f, "PRODUCT5", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));

            Initialize();

            this.Name = "Component Splitter" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawCompSplitter drawRectangle = new();
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
                using GraphicsPath gp = new(FillMode.Winding);
                using Pen pen = new(StreamColor, PenWidth);
                int rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                int r = this.rectangle.Width - rhoff;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);
                int AH2 = ArcHeight / 2;

                //gp.AddLine(R.TopLeft().X, (R.TopLeft().Y + R.BottomLeft().Y) / 2, R.BottomRight().X, (R.TopLeft().Y + R.BottomLeft().Y) / 2);
                //gp.AddLine(R.BottomRight().X, R.BottomRight().Y, R.TopRight().X, R.TopRight().Y);

                Point[] p = new Point[4];
                p[0].X = R.TopLeft().X;
                p[0].Y = R.TopLeft().Y;
                p[1].X = R.TopRight().X;
                p[1].Y = R.TopRight().Y;
                p[2].X = R.BottomRight().X;
                p[2].Y = R.BottomRight().Y;
                p[3].X = R.BottomLeft().X;
                p[3].Y = R.BottomLeft().Y;

                gp.AddLines(p);
                gp.CloseAllFigures();

                Color col2 = Color.White;
                Color col1 = Color.Gray;

                LinearGradientBrush BodyBrush = new(new PointF(this.RotatedRectangle.Left, 0),
                    new PointF(this.RotatedRectangle.Right, 0), col2, col1);

                BodyBrush.SetBlendTriangularShape(1f);

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

                g.FillPath(BodyBrush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Node Feed = this.Hotspots.Search("FEED");
            Node Product = this.Hotspots.Search("PRODUCT");

            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new CompSplitterDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();

            this.IsSolved = false;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return compsplitter.IsDirty;
            }
            set
            {
                compsplitter.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return compsplitter.IsSolved;
            }
            set
            {
                //compsplitter.IsSolved = value;
            }
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawCompSplitter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            compsplitter = (CompSplitter)info.GetValue("compsplitter", typeof(CompSplitter));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("compsplitter", compsplitter);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product1 = this.Hotspots.Search("PRODUCT1");
            Product2 = this.Hotspots.Search("PRODUCT2");
            Product3 = this.Hotspots.Search("PRODUCT3");
            Product4 = this.Hotspots.Search("PRODUCT4");
            Product5 = this.Hotspots.Search("PRODUCT5");
        }
    }
}