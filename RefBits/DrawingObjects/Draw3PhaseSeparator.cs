using Main.Images;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class Draw3PhaseSeparator : DrawRectangle, ISerializable
    {
        public SimpleFlash simpleFlash = new();

        private enum CompFlagType
        {
            Feed, VapProduct, LiqProduct, Overdefined,
            Product1_Product2,
            Feed_Product1,
            Feed_Product2,
            None,
            Water
        }

        private enum FlowFlagType
        {
            Feed, Product1, Product2, Overdefined,
            Feed_Product1,
            Feed_Product2,
            Product1_Product2,
            DivRatio,
            None,
            Water
        }

        private enum EnthalpyFlagType
        {
            Feed, VapProduct, LiqProduct, Overdefined,
            Feed_Product1,
            Feed_Product2,
            Product1_Product2,
            None
        }

        private enum PressureFlagType
        {
            Feed, Product1, Product2, Overdefined,
            Feed_Product1,
            Feed_Product2,
            Product1_Product2
        }

        public Node Feed, VapProduct, LiqProduct, Water;

        public Draw3PhaseSeparator() : this(0, 0, 1, 1)
        {
        }

        public Draw3PhaseSeparator(int x, int y, int width, int height) : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0.3f, 0f, "FEED", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            VapProduct = Hotspots.Add(new Node(this, 0.8f, 0f, "VapProduct", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            LiqProduct = Hotspots.Add(new Node(this, 1f, 0.25f, "LiqProduct", NodeDirections.Down, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Water = Hotspots.Add(new Node(this, 0.62f, 0.91f, "Water", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));

            Initialize();

            this.Name = "Separator" + Count.ToString();
            Count++;
        }

        private double pressure = 1;

        public double Pressure
        {
            get { return Pressure; }
            set { Pressure = value; }
        }

        private double temperature = 1;
        private static int Count;

        public double Temperature
        {
            get { return Temperature; }
            set { Temperature = value; }
        }

        public Separator3PhaseDialog dlg { get; private set; }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            Draw3PhaseSeparator drawRectangle = new();
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
                /* g.SmoothingMode = SmoothingMode.HighQuality ;
                 Pen pen = new  Pen(StreamColor, PenWidth);
                 Color col2 = Color.White;
                 Color col1 = Color.Gray;

                 int  rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                 int  r = this.rectangle.Width - rhoff;
                 int  ArcWidth = Convert.ToInt32(rectangle.Height / 4);

                 GraphicsPath gp = new  GraphicsPath();
                 RectangleF ColumnBody = new  RectangleF(this.rectangle.Left + ArcWidth / 2, rectangle.Top, this.rectangle.Width - ArcWidth, rectangle.Height / 2);

                 gp.AddArc(this.rectangle.Left, rectangle.Top, ArcWidth, this.rectangle.Height / 2, 90, 180);
                 gp.AddRectangle(ColumnBody);
                 gp.AddArc(this.rectangle.Right - ArcWidth, rectangle.Top, ArcWidth, this.rectangle.Height / 2, 270, 180);
                 gp.AddRectangle(new  RectangleF(rectangle.Left + rectangle.Width * 2 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Width / 4, rectangle.Height / 3));
                 gp.AddArc(rectangle.Left + rectangle.Width * 2 / 4, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Width / 4, rectangle.Height * 1 / 6, 0, 180);

                 gp.CloseAllFigures();

                 if (Angle != enumRotationAngle.a0)
                     Rotate(gp, rectangle.Center());

                 if (FlipHorizontal)
                 {
                     Matrix m = new  Matrix();
                     m.Scale(-1, 1);
                     m.Translate(RotatedRectangle.Width + 2 * RotatedRectangle.Left, 0, MatrixOrder.Append);
                     gp.Transform(m);
                 }

                 if (FlipVertical)
                 {
                     Matrix m = new  Matrix();
                     m.Scale(1, -1);
                     m.Translate(0, RotatedRectangle.Height + 2 * RotatedRectangle.Top, MatrixOrder.Append);
                     gp.Transform(m);
                 }

                 LinearGradientBrush Brush = new  LinearGradientBrush(new  PointF(RotatedRectangle.Left, 0), new  PointF(RotatedRectangle.Right, 0), col2, col1);

                 g.FillPath(Brush, gp);
                 g.DrawPath(pen, gp);*/
            }
            catch { }

            g.DrawImage(Images.ThreePhaseSep(), this.rectangle);

            base.Draw(g);
        }

        public override void Dump()
        {
            base.Dump();

            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
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
            //Node Feed = this.Hotspots.Search("FEED");
            //Node VapProduct = this.Hotspots.Search("VAPPRODUCT");
            //Node LiqProduct = this.Hotspots.Search("LIQPRODUCT");

            Feeds.Add(Feed);
            Products.Add(VapProduct);
            Products.Add(LiqProduct);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new Separator3PhaseDialog(this);
            dlg.DLGValueChangedEvent += new PumpDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();

            //this.IsSolved = false;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return simpleFlash.IsDirty;
            }
            set
            {
                simpleFlash.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return simpleFlash.IsSolved;
            }
           /* set
            {
                simpleFlash.IsSolved = value;
            }*/
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public Draw3PhaseSeparator(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Pressure = info.GetDouble("Pressure");
            Temperature = info.GetDouble("Temperature");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Pressure", Pressure);
            info.AddValue("Temperature", Temperature);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            VapProduct = this.Hotspots.Search("VapProduct");
            LiqProduct = this.Hotspots.Search("LiqProduct");
            Water = this.Hotspots.Search("Water");
        }
    }
}