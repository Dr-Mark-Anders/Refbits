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
    internal class DrawGibbs : Units.DrawRectangle, ISerializable
    {
        public GibbsReactor gibbs = new GibbsReactor();

        public Node Feed, VapProduct, LiqProduct;

        public DrawGibbs() : this(0, 0, 1, 1)
        {
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return gibbs.Ports["In1"];
            }
        }

        [Category("Ports"), Description("Vapour")]
        public Port Liquid
        {
            get
            {
                return gibbs.Ports["Out1"];
            }
        }

        [Category("Ports"), Description("Liquid")]
        public Port Vapour
        {
            get
            {
                return gibbs.Ports["Out2"];
            }
        }

        public override void CreateFlowsheetUOModel()
        {
            this.Flowsheet.Add(gibbs);

            Feed = Hotspots["FEED"];
            VapProduct = Hotspots["VapProduct"];
            LiqProduct = Hotspots["LiqProduct"];

            Feed.PortGuid = gibbs.Ports["In1"].Guid;
            VapProduct.PortGuid = gibbs.Ports["Out1"].Guid;
            LiqProduct.PortGuid = gibbs.Ports["Out2"].Guid;

            gibbs.Name = this.Name;
        }

        public DrawGibbs(int x, int y, int width, int height) : base()
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

        public GibbsDialog dlg { get; private set; }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawGibbs drawRectangle = new DrawGibbs();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = gibbs.Ports["In1"].Guid;
            Hotspots["VapProduct"].PortGuid = gibbs.Ports["Out1"].Guid;
            Hotspots["LiqProduct"].PortGuid = gibbs.Ports["Out2"].Guid;

            gibbs.Name = this.Name;

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
                Pen pen = new Pen(StreamColor, PenWidth);
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

                GraphicsPath gp = new GraphicsPath();
                RectangleF ColumnBody = new RectangleF(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);

                gp.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                gp.AddRectangle(ColumnBody);
                gp.AddArc(this.rectangle.Left, b - ArcHeight - 1, this.rectangle.Width, ArcHeight, 0, 180);

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
                LinearGradientBrush Brush = new LinearGradientBrush(new PointF(RotatedRectangle.Left, 0), new PointF(RotatedRectangle.Right, 0), col2, col1);

                g.FillPath(Brush, gp);
                g.DrawPath(pen, gp);

                Font f = new Font("Arial", 10.0F);
                SizeF size = g.MeasureString("Grx", new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point));
                g.DrawString("Grx", f, Brushes.Black, RotatedRectangle.Left + RotatedRectangle.Width / 2 - size.Width / 2
                    , RotatedRectangle.Top + RotatedRectangle.Height / 2 - size.Height / 2);
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
            //Node Feed = this.Hotspots.Search("FEED");
            //Node VapProduct = this.Hotspots.Search("VAPPRODUCT");
            //Node LiqProduct = this.Hotspots.Search("LIQPRODUCT");

            Feeds.Add(Feed);
            Products.Add(VapProduct);
            Products.Add(LiqProduct);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new GibbsDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
           // gibbs.IsSolved = false;
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawGibbs(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Pressure = info.GetDouble("Pressure");
            Temperature = info.GetDouble("Temperature");
            try
            {
                gibbs = (GibbsReactor)info.GetValue("gibbs", typeof(GibbsReactor));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Pressure", Pressure);
            info.AddValue("Temperature", Temperature);
            info.AddValue("gibbs", gibbs);
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