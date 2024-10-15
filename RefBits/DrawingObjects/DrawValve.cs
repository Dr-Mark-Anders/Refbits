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
    internal class DrawValve : DrawRectangle, ISerializable
    {
        public Valve valve = new Valve();
        private Node Feed;
        private Node Product;
        private ValveDialog dlg;

        public DrawValve() : this(0, 0, 1, 1)
        {
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return valve.IsDirty;
            }
            set
            {
                valve.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return valve.IsSolved;
            }
           /* set
            {
                valve.IsSolved = value;
            }*/
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = valve.Ports["In1"].Guid;
            Hotspots["PRODUCT"].PortGuid = valve.Ports["Out1"].Guid;

            valve.Name = this.Name;
            return true;
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return valve.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return valve.Ports["Out1"];
            }
        }

        public Valve Valve
        {
            get
            {
                return valve;
            }
        }

        public DrawValve(int x, int y, int width, int height)
            : base()
        {
            //if (width > 20)
            width = 50;
            //if (height > 50)
            height = 25;
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
            DrawValve drawRectangle = new DrawValve();
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
                Polygon[2] = new Point(R.TopRight().X, R.TopRight().Y);
                Polygon[3] = new Point(R.BottomRight().X, R.BottomRight().Y);

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

            dlg = new ValveDialog(this);
            dlg.DLGValueChangedEvent += new ValveDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //this.IsSolved = false;
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawValve(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                valve = (Valve)info.GetValue("valve", typeof(Valve));
            }
            catch { }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("valve", valve);
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