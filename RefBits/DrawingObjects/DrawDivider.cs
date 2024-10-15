using ModelEngine;
using Extensions;
using ModelEngine;
using System;
using System.Collections.Generic;
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
    internal class DrawDivider : DrawRectangle, ISerializable
    {
        private DividerDialog dlg;
        public Divider divider = new();

        public Node Feed;
        public Node Product1 { get; private set; }
        public Node Product2 { get; private set; }
        public Node Product3 { get; private set; }
        public Node Product4 { get; private set; }
        public Node Product5 { get; private set; }

        public UOMProperty divratio = new UOMProperty(ePropID.NullUnits, 0.5);
        private static int Count;

        public List<Port_Material> PortListOut
        {
            get
            {
                return divider.GetPorts(FlowDirection.OUT).materialList;
            }
        }

        public DrawDivider() : this(0, 0, 1, 1)
        {
        }

        public DrawDivider(int x, int y, int width, int height)
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

            this.Name = "Divider" + Count.ToString();
            Count++;
        }

        public override bool UpdateAttachedModel()
        {
            Node Inlet, Outlet1, Outlet2, Outlet3, Outlet4, Outlet5;

            Inlet = Hotspots["FEED"];
            Outlet1 = Hotspots["PRODUCT1"];
            Outlet2 = Hotspots["PRODUCT2"];
            Outlet3 = Hotspots["PRODUCT3"];
            Outlet4 = Hotspots["PRODUCT4"];
            Outlet5 = Hotspots["PRODUCT5"];

            Inlet.PortGuid = divider.PortIn.Guid;
            Outlet1.PortGuid = divider.PortOut1.Guid;
            Outlet2.PortGuid = divider.PortOut2.Guid;
            Outlet3.PortGuid = divider.PortOut3.Guid;
            Outlet4.PortGuid = divider.PortOut4.Guid;
            Outlet5.PortGuid = divider.PortOut5.Guid;

            return true;
        }

        public UOMProperty Divratio
        {
            get { return divratio; }
            set
            {
                divratio.BaseValue = value;
                IsSolved = false;
                RaiseValueChangedEvent(new EventArgs());
            }
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return divider.IsDirty;
            }
            set
            {
                divider.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return divider.IsSolved;
            }
            set
            {
                //divider.IsSolved = value;
            }
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawDivider drawRectangle = new DrawDivider();
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
                GraphicsPath gp = new GraphicsPath(FillMode.Winding);

                Pen pen = new Pen(StreamColor, PenWidth);

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
                p[0].Y = (R.TopLeft().Y + R.BottomLeft().Y) / 2;
                p[1].X = R.TopRight().X;
                p[1].Y = R.TopRight().Y;
                p[2].X = R.BottomRight().X;
                p[2].Y = R.BottomRight().Y;
                p[3].X = R.TopLeft().X;
                p[3].Y = (R.TopLeft().Y + R.BottomLeft().Y) / 2;

                gp.AddLines(p);
                gp.CloseAllFigures();

                Color col2 = Color.White;
                Color col1 = Color.Gray;

                LinearGradientBrush BodyBrush = new LinearGradientBrush(new PointF(this.RotatedRectangle.Left, 0),
                    new PointF(this.RotatedRectangle.Right, 0), col2, col1);

                BodyBrush.SetBlendTriangularShape(1f);

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
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Node Feed = this.Hotspots.Search("FEED");
            Node Product = this.Hotspots.Search("PRODUCT");

            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new DividerDialog(this);
            dlg.DLGValueChangedEvent += new PumpDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();

            //divider.IsSolved = false;
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawDivider(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            divider = (Divider)info.GetValue("divider", typeof(Divider));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("divider", divider);
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