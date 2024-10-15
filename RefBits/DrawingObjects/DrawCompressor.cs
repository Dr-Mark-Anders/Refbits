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
    internal class DrawCompressor : Units.DrawRectangle, ISerializable
    {
        private UOMProperty deltaPressure = new UOMProperty(ePropID.P);
        private UOMProperty deltaTemp = new UOMProperty(ePropID.T);
        private UOMProperty duty = new UOMProperty(ePropID.EnergyFlow);
        private Boolean isAdiabatic = true;
        public Compressor compressor = new Compressor();

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = compressor.PortIn.Guid;
            Hotspots["PRODUCT"].PortGuid = compressor.PortOut.Guid;
            Hotspots["ENERGY"].PortGuid = compressor.Q.Guid;

            compressor.Name = this.Name;

            return true;
        }

        public Node FEED;
        public Node PRODUCT;
        private Node Feed;
        private Node Product;
        private static int Count;

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return compressor.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return compressor.Ports["Out1"];
            }
        }

        public DrawCompressor() : this(0, 0, 20, 50)
        {
        }

        public DrawCompressor(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0.2f, 0.0f, "FEED", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Product = Hotspots.Add(new Node(this, 0.99f, 0.3f, "PRODUCT", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Hotspots.Add(new Node(this, 0f, 0.5f, "ENERGY", NodeDirections.Right, HotSpotType.EnergyIn, HotSpotOwnerType.DrawRectangle));  //EnergyStream
            Initialize();

            this.Name = "K" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawCompressor drawRectangle = new DrawCompressor();
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
            GraphicsPath gp = new GraphicsPath(FillMode.Winding);

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            Pen pen = new Pen(StreamColor, PenWidth);
            LinearGradientBrush BodyBrush = new LinearGradientBrush
                (new PointF(this.RotatedRectangle.Left, 0), new PointF(this.RotatedRectangle.Right, 0), col2, col1);

            Point[] points = new Point[4];

            points[0] = R.TopLeft();
            points[0].X = R.TopLeft().X + (int)(rectangle.Width * 0.25);

            points[1] = R.TopRight();
            points[1].Offset(0, R.Height().X / 4);

            points[2] = R.BottomRight();
            points[2].Offset(0, -R.Height().X / 4);

            points[3].X = R.BottomLeft().X + (int)(rectangle.Width * 0.25);
            points[3].Y = R.BottomLeft().Y;

            gp.AddRectangle(new RectangleF(R.BottomLeft().X, R.Centre().Y - 2, (int)(Rectangle.Width * 0.25), 5));
            gp.AddPolygon(points);
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

            if (!compressor.IsActive)
                pen.Color = Color.LightSalmon;

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);
        }

        public double Adiabaticeff
        {
            get
            {
                return compressor.AEff.Value;
            }

            set
            {
                compressor.AEff.Value.BaseValue = value;
            }
        }

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

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            FEED = this.Hotspots.Search("FEED");
            PRODUCT = this.Hotspots.Search("PRODUCT");

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            CompressorDialog dlg = new CompressorDialog(this);
            dlg.DLGValueChangedEvent += new CompressorDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();

            this.IsSolved = false;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return compressor.IsDirty;
            }
            set
            {
                compressor.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return compressor.IsSolved;
            }
            set
            {
                //compressor.IsSolved = value;
            }
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawCompressor(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                compressor = (Compressor)info.GetValue("compressor", typeof(Compressor));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("compressor", compressor);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product = this.Hotspots.Search("PRODUCT");
        }
    }

    [Serializable]
    public class SaveableCompData
    {
        public SaveableCompData()
        {
        }

        public bool FixPressureDrop = false;
        public bool FixExitPressure = true;
        public double ExitPressure = 1;
        public double Efficiency = 70;
        public double DeltaPress = 0;
    }
}