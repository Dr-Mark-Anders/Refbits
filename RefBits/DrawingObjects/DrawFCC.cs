using ModelEngine;
using Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;
using ModelEngine.FCC;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    internal class DrawFCC : DrawRectangle, ISerializable
    {
        private UOMProperty deltaPressure = new UOMProperty(ePropID.P);
        private UOMProperty deltaTemp = new UOMProperty(ePropID.T);
        private UOMProperty exitPressure = new UOMProperty(ePropID.P, 1);
        private UOMProperty DeltaPress = new UOMProperty(ePropID.DeltaP, 0.5);
        private UOMProperty Duty = new UOMProperty(ePropID.EnergyFlow, 0);

        //PumpDialog dlg;
        private FCC fcc = new FCC();

        private double efficiency = 70;
        private bool fixExitPressure = false;
        private static int Count;

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return fcc.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return fcc.Ports["Out1"];
            }
        }

        public bool OutletPressureFixed
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

        public UOMProperty OutletPressure
        {
            get
            {
                return exitPressure;
            }
            set
            {
                exitPressure.BaseValue = value;
                //fcc.IsSolved = false;
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
                DeltaPress.BaseValue = value;
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
                efficiency = value;
            }
        }

        public FCC Pump { get => fcc; set => fcc = value; }

        public DrawFCC() : this(0, 0, 1, 1)
        {
        }

        public DrawFCC(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 0.5f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Hotspots.Add(new Node(this, 0.5f, 0f, "PRODUCT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Hotspots.Add(new Node(this, 0.5f, 1.0f, "ENERGY", NodeDirections.Right, HotSpotType.EnergyIn, HotSpotOwnerType.DrawRectangle));  //EnergyStream
            Initialize();

            this.Name = "P" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = GetNormalizedRectangle(rectangle);
        }

        public DrawFCC(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            deltaPressure = (UOMProperty)info.GetValue("deltaPressure ", typeof(UOMProperty));
            deltaTemp = (UOMProperty)info.GetValue("deltaTemp", typeof(UOMProperty));
            exitPressure = (UOMProperty)info.GetValue("exitPressure ", typeof(UOMProperty));
            DeltaPress = (UOMProperty)info.GetValue("DeltaPress", typeof(UOMProperty));
            Efficiency = info.GetDouble("Efficiency");
            fixExitPressure = info.GetBoolean("fixExitPressure ");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("deltaPressure ", deltaPressure, typeof(UOMProperty));
            info.AddValue("deltaTemp", deltaTemp, typeof(UOMProperty));
            info.AddValue("exitPressure ", exitPressure, typeof(UOMProperty));
            info.AddValue("DeltaPress", DeltaPress, typeof(UOMProperty));
            info.AddValue("Efficiency", Efficiency);
            info.AddValue("fixExitPressure ", fixExitPressure);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.FillMode = FillMode.Winding;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new Pen(StreamColor, PenWidth);

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Point[] points = new Point[3];
            points[0] = R.BottomLeft();
            points[1] = R.Centre();
            points[2] = R.BottomRight();

            LinearGradientBrush BodyBrush = new LinearGradientBrush(new PointF(RotatedRectangle.Left, 0),
                new PointF(this.RotatedRectangle.Right, 0), col2, col1);

            gp.AddLines(points);
            gp.AddEllipse(R);
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

            pen.Color = StreamColor;

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            pen.Dispose();

            base.Draw(g);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Node Feed = this.Hotspots.Search("FEED");
            Node Product = this.Hotspots.Search("PRODUCT");

            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            //dlg = new  PumpDialog(this);
            //dlg.DLGValueChangedEvent += new  BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            //dlg.ShowDialog();
            //fcc.IsSolved = false;
        }
    }
}