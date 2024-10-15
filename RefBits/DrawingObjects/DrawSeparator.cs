using Extensions;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class DrawSeparator : DrawRectangle, ISerializable
    {
        public SimpleFlash simpleFlash = new();

        public Node Feed, VapProduct, LiqProduct;

        public DrawSeparator() : this(0, 0, 1, 1)
        {
        }

        public DrawSeparator(int x, int y, int width, int height)
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
            VapProduct = Hotspots.Add(new Node(this, 0.5f, 0f, "VapProduct", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            LiqProduct = Hotspots.Add(new Node(this, 0.5f, 1f, "LiqProduct", NodeDirections.Down, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Initialize();

            this.Name = "Separator" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawSeparator(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Pressure = info.GetDouble("Pressure");
            Temperature = info.GetDouble("Temperature");
            try
            {
                simpleFlash = (SimpleFlash)info.GetValue("simpleFlash", typeof(SimpleFlash));
            }
            catch
            {
            }
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return simpleFlash.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Vapour")]
        public Port Liquid
        {
            get
            {
                return simpleFlash.Ports["Out1"];
            }
        }

        [Category("Ports"), Description("Liquid")]
        public Port Vapour
        {
            get
            {
                return simpleFlash.Ports["Out2"];
            }
        }

        public override void CreateFlowsheetUOModel()
        {
            this.Flowsheet.Add(simpleFlash);

            Feed = Hotspots["FEED"];
            VapProduct = Hotspots["VapProduct"];
            LiqProduct = Hotspots["LiqProduct"];

            Feed.PortGuid = simpleFlash.Ports["In"].Guid;
            VapProduct.PortGuid = simpleFlash.Ports["OutV"].Guid;
            LiqProduct.PortGuid = simpleFlash.Ports["OutL"].Guid;

            simpleFlash.Name = this.Name;
        }

        private double pressure = 1;

        public double Pressure
        {
            get { return pressure; }
            set { pressure = value; }
        }

        private double temperature = 1;
        private static int Count;

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public SeparatorDialog dlg { get; private set; }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawSeparator drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = simpleFlash.PortIn.Guid;
            Hotspots["VapProduct"].PortGuid = simpleFlash.PortOutV.Guid;
            Hotspots["LiqProduct"].PortGuid = simpleFlash.PortOutL.Guid;

            simpleFlash.Name = this.Name;
            return true;
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
                Pen pen = new(StreamColor, PenWidth);
                Color col2 = Color.White;
                Color col1 = Color.Gray;

                int rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                int r = this.rectangle.Width - rhoff;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);
                int t = this.rectangle.Top;
                int b = this.rectangle.Bottom;
                int h = b - t;

                GraphicsPath gp = new();
                RectangleF ColumnBody = new(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);

                gp.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                gp.AddRectangle(ColumnBody);
                gp.AddArc(this.rectangle.Left, b - ArcHeight - 1, this.rectangle.Width, ArcHeight, 0, 180);

                gp.CloseAllFigures();

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

                LinearGradientBrush Brush = new(new PointF(RotatedRectangle.Left, 0), new PointF(RotatedRectangle.Right, 0), col2, col1);

                g.FillPath(Brush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
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
            Feeds.Add(Feed);
            Products.Add(VapProduct);
            Products.Add(LiqProduct);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new SeparatorDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //simpleFlash.IsSolved = false;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Pressure", Pressure);
            info.AddValue("Temperature", Temperature);
            info.AddValue("simpleFlash", simpleFlash, typeof(SimpleFlash));
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            VapProduct = this.Hotspots.Search("VapProduct");
            LiqProduct = this.Hotspots.Search("LiqProduct");
        }
    }
}