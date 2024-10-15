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
    public class DrawPipe : DrawRectangle, ISerializable
    {
        public Pipe pipe = new Pipe();
        private UOMProperty deltaTemp = new UOMProperty(ePropID.T);
        private UOMProperty exitPressure = new UOMProperty(ePropID.P, 1);
        private UOMProperty DeltaPress = new UOMProperty(ePropID.DeltaP, 0.5);

        private Node Feed;
        private Node Product;

        public double Efficiency = 70;
        private bool fixExitPressure = false;
        private PipeDialog dlg;

        public DrawPipe() : this(0, 0, 1, 1)
        {
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return pipe.IsDirty;
            }
            set
            {
                pipe.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return pipe.IsSolved;
            }
            set
            {
                //pipe.IsSolved = value;
            }
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = pipe.Ports["In1"].Guid;
            Hotspots["PRODUCT"].PortGuid = pipe.Ports["Out1"].Guid;

            pipe.Name = this.Name;
            return true;
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return pipe.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return pipe.Ports["Out1"];
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

        public UOMProperty ExitPressure
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

        public UOMProperty DeltaPressure
        {
            get
            {
                return DeltaPress;
            }
            set
            {
                if (DeltaPressure.Source == SourceEnum.Input ||
                    DeltaPress.Source == SourceEnum.Empty)
                {
                    DeltaPress.BaseValue = value;
                    IsSolved = false;
                    //RaiseValueChangedEvent(new  EventArgs());
                }
            }
        }

        public DrawPipe(int x, int y, int width, int height)
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
            DrawPipe drawRectangle = new DrawPipe();
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

            dlg = new PipeDialog(this);
            dlg.DLGValueChangedEvent += new PumpDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            this.IsSolved = false;
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawPipe(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            DeltaPressure = (UOMProperty)info.GetValue("deltaPressure ", typeof(UOMProperty));
            deltaTemp = (UOMProperty)info.GetValue("deltaTemp", typeof(UOMProperty));
            exitPressure = (UOMProperty)info.GetValue("exitPressure ", typeof(UOMProperty));
            DeltaPress = (UOMProperty)info.GetValue("DeltaPress", typeof(UOMProperty));
            Efficiency = info.GetDouble("Efficiency");
            fixExitPressure = info.GetBoolean("fixExitPressure ");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("deltaPressure ", DeltaPressure, typeof(UOMProperty));
            info.AddValue("deltaTemp", deltaTemp, typeof(UOMProperty));
            info.AddValue("exitPressure ", exitPressure, typeof(UOMProperty));
            info.AddValue("DeltaPress", DeltaPress, typeof(UOMProperty));
            info.AddValue("Efficiency", Efficiency);
            info.AddValue("fixExitPressure ", fixExitPressure);
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