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
    internal class DrawExpander : Units.DrawRectangle, ISerializable
    {
        private UOMProperty deltaPressure = new UOMProperty(ePropID.P);
        private UOMProperty deltaTemp = new UOMProperty(ePropID.T);
        private UOMProperty exitPressure = new UOMProperty(ePropID.P, 1);
        private UOMProperty DeltaPress = new UOMProperty(ePropID.DeltaP, 0.5);

        private UOMProperty polyeff = new UOMProperty(ePropID.NullUnits, 70);

        public Expander expander = new Expander();

        private double efficiency = 70;
        private bool fixExitPressure = false;

        public Node FEED;
        public Node PRODUCT;

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = expander.PortIn.Guid;
            Hotspots["PRODUCT"].PortGuid = expander.PortOut.Guid;
            Hotspots["ENERGY"].PortGuid = expander.Q.Guid;

            expander.Name = this.Name;

            return true;
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
                //expander.IsSolved = false;
                RaiseValueChangedEvent(new EventArgs());
            }
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return expander.PortIn;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return expander.PortOut;
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

        public DrawExpander() : this(0, 0, 10, 10)
        {
        }

        public DrawExpander(int x, int y, int width, int height) : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            FEED = Hotspots.Add(new Node(this, 0f, 0.25f, "FEED", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            PRODUCT = Hotspots.Add(new Node(this, 0.75f, 0f, "PRODUCT", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Hotspots.Add(new Node(this, 1f, 0.5f, "ENERGY", NodeDirections.Right, HotSpotType.EnergyOut, HotSpotOwnerType.DrawRectangle));  //EnergyStream
            Initialize();

            this.Name = "Ex" + Count.ToString();
            Count++;
        }

        public override DrawObject Clone()
        {
            DrawExpander drawRectangle = new DrawExpander();
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
            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new GraphicsPath();
            gp.FillMode = FillMode.Winding;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new Pen(StreamColor, PenWidth);
            Point[] points = new Point[5];

            points[0] = R.TopLeft();
            points[0].Offset(0, R.Height().X / 4);

            points[1] = R.TopRight();
            points[1].Offset((int)(-R.Width * 0.25), 0);

            points[2] = R.BottomRight();
            points[2].Offset((int)(-R.Width * 0.25), 0);

            points[3] = R.BottomLeft();
            points[3].Offset(0, -R.Height().X / 4);

            points[4] = R.TopLeft();
            points[4].Offset(0, R.Height().X / 4);

            LinearGradientBrush BodyBrush = new LinearGradientBrush
                (new PointF(this.RotatedRectangle.Left, 0), new PointF(this.RotatedRectangle.Right, 0), col2, col1);

            gp.AddRectangle(new RectangleF(R.BottomRight().X - (int)(Rectangle.Width * 0.25), R.Centre().Y - 2, (float)(Rectangle.Width * 0.25), 5));
            gp.AddPolygon(points);

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

            if (expander.IsActive)
                pen.Color = Color.Black;

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);
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

        private bool isAdiabatic = true;

        public bool IsAdiabatic
        {
            get
            {
                return isAdiabatic;
            }
            set
            {
                isAdiabatic = value;
            }
        }

        private double adiabaticeff = 70;

        private UOMProperty duty = new UOMProperty(ePropID.EnergyFlow, 0);
        private ExpanderDialog dlg;
        private static int Count;

        public double Adiabaticeff
        {
            get
            {
                return adiabaticeff;
            }

            set
            {
                adiabaticeff = value;
            }
        }

        public UOMProperty Duty
        {
            get
            {
                return duty;
            }

            set
            {
                duty = value;
            }
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

            dlg = new ExpanderDialog(this);
            dlg.DLGValueChangedEvent += new PumpDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //expander.IsSolved = false;
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawExpander(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            expander = (Expander)info.GetValue("expander", typeof(Expander));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("expander", expander);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            FEED = this.Hotspots.Search("FEED");
            PRODUCT = this.Hotspots.Search("PRODUCT");
        }
    }

    [Serializable]
    public class SaveableExpanderData
    {
        public SaveableExpanderData()
        {
        }

        public bool FixPressureDrop = false;
        public bool FixExitPressure = true;
        public double ExitPressure = 1;
        public double Efficiency = 70;
        public double DeltaPress = 0;
    }
}