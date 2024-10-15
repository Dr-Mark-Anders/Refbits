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
    internal class DrawMixer : DrawRectangle, ISerializable
    {
        public Mixer mixer = new Mixer();

        public DrawMixer() : this(0, 0, 1, 1)
        {
            mixer = new Mixer();
        }

        public DrawMixer(int x, int y, int width, int height)
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
            Feed1 = Hotspots.Add(new Node(this, 0f, 0f, "FEED1", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Feed2 = Hotspots.Add(new Node(this, 0f, 0.25f, "FEED2", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Feed3 = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED3", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Feed4 = Hotspots.Add(new Node(this, 0f, 0.75f, "FEED4", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Feed5 = Hotspots.Add(new Node(this, 0f, 1f, "FEED5", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Initialize();

            this.Name = "MX" + Count.ToString();
            Count++;
        }

        public override bool UpdateAttachedModel()
        {
            Node Inlet1, Inlet2, Inlet3, Inlet4, Inlet5, Outlet;

            Inlet1 = Hotspots["FEED1"];
            Inlet2 = Hotspots["FEED2"];
            Inlet3 = Hotspots["FEED3"];
            Inlet4 = Hotspots["FEED4"];
            Inlet5 = Hotspots["FEED5"];
            Outlet = Hotspots["PRODUCT"];

            Inlet1.PortGuid = mixer.PortIn1.Guid;
            Inlet2.PortGuid = mixer.PortIn2.Guid;
            Inlet3.PortGuid = mixer.PortIn3.Guid;
            Inlet4.PortGuid = mixer.PortIn4.Guid;
            Inlet5.PortGuid = mixer.PortIn5.Guid;
            Outlet.PortGuid = mixer.PortOut.Guid;

            mixer.Name = this.Name;
            return true;
        }

        public PortList PortListIn
        {
            get
            {
                return mixer.GetPorts(FlowDirection.IN);
            }
        }

        public Port PortOut
        {
            get
            {
                return mixer.Ports["Out1"];
            }
        }

        private readonly UOMProperty deltaPressure = new(ePropID.P);
        private readonly UOMProperty exitPressure = new(ePropID.P, 1);
        private readonly double efficiency = 70;
        private MixerDialog dlg;
        public Node Feed1;
        public Node Feed2;
        public Node Feed3;
        public Node Feed4;
        public Node Feed5;
        public Node Product;

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawMixer drawRectangle = new DrawMixer();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public UOMProperty OutletPressure
        {
            get
            {
                return exitPressure;
            }
            set
            {
                exitPressure.BaseValue = value;
                IsSolved = false;
                RaiseValueChangedEvent(new EventArgs());
            }
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return mixer.IsDirty;
            }
            set
            {
                mixer.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return mixer.IsSolved;
            }
            set
            {
                //mixer.IsSolved = value;
            }
        }

        public UOMProperty DeltaPressure
        {
            get
            {
                return deltaPressure;
            }
            set
            {
                DeltaPressure.BaseValue = value;
            }
        }

        public double Efficiency
        {
            get
            {
                return efficiency;
            }
            set
            {
                Efficiency = value;
            }
        }

        private enum FlowFlagType
        {
            Feed1, Feed2, Product, Overdefined,
            Feed1_Product,
            Feed2_Product,
            Feed1_Feed2,
            None
        }

        //FlowFlagType flowtype = FlowFlagType.Overdefined;

        private static int Count;

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                GraphicsPath gp = new GraphicsPath();
                Pen pen = new Pen(StreamColor, PenWidth);

                Color col2 = Color.White;
                Color col1 = Color.Gray;

                Point[] p = new Point[4];
                p[0].X = R.TopLeft().X;
                p[0].Y = R.TopLeft().Y;
                p[1].X = R.TopRight().X;
                p[1].Y = (R.TopLeft().Y + R.BottomLeft().Y) / 2;
                p[2].X = R.BottomLeft().X;
                p[2].Y = R.BottomLeft().Y;
                p[3].X = R.TopLeft().X;
                p[3].Y = R.TopLeft().Y;

                LinearGradientBrush BodyBrush = new LinearGradientBrush(new PointF(this.RotatedRectangle.Left, 0),
                     new PointF(this.RotatedRectangle.Right, 0), col2, col1);

                gp.AddLines(p);
                gp.CloseAllFigures();

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

                BodyBrush.SetBlendTriangularShape(1f);

                g.FillPath(BodyBrush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new MixerDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //mixer.IsSolved = false;
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawMixer(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed1 = this.Hotspots.Search("FEED1");
            Feed2 = this.Hotspots.Search("FEED2");
            Feed3 = this.Hotspots.Search("FEED3");
            Feed4 = this.Hotspots.Search("FEED4");
            Feed5 = this.Hotspots.Search("FEED5");
            Product = this.Hotspots.Search("PRODUCT");
        }
    }
}