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
    public enum Coolermode
    { Fixduty, CalcDuty, FixDeltaT }

    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    [Serializable]
    internal class DrawCooler : DrawRectangle, ISerializable
    {
        private Cooler cooler = new Cooler();

        private Node Feed;
        private Node Product;
        private Node Q;

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = cooler.PortIn.Guid;
            Hotspots["PRODUCT"].PortGuid = cooler.PortOut.Guid;
            Hotspots["ENERGY"].PortGuid = cooler.Q.Guid;

            cooler.Name = this.Name;

            return true;
        }

        public bool ValidateConnections(GraphicsList gl)
        {
            DrawStreamCollection fs = gl.ReturnExternalFeedStreams(this);
            DrawStreamCollection ps = gl.ReturnSideProductStreams(this, true);

            Guid startid = fs[0].StartDrawObjectGuid;

            foreach (DrawMaterialStream ds in ps)
            {
                if (ds.EndDrawObjectGuid == startid)
                {
                    return true;
                }
            }

            return false;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return cooler.IsDirty;
            }
            set
            {
                cooler.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return cooler.IsSolved;
            }
           /* set
            {
                cooler.IsSolved = value;
            }*/
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return cooler.PortIn;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return cooler.PortOut;
            }
        }

        public Cooler Cooler { get => cooler; set => cooler = value; }

        public DrawCooler() : this(0, 0, 1, 1)
        {
        }

        public DrawCooler(int x, int y, int width, int height)
            : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Q = Hotspots.Add(new Node(this, 0.5f, 1.0f, "ENERGY", NodeDirections.Down, HotSpotType.EnergyOut, HotSpotOwnerType.DrawRectangle));  //EnergyStream
            Initialize();

            this.Name = "Cooler" + Count.ToString();
            Count++;
        }

        private static int Count;

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawCooler drawRectangle = new();
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

            Pen pen = new Pen(StreamColor, PenWidth);
            pen.Color = Color.White;

            gp.AddEllipse(DrawRectangle.GetNormalizedRectangle(rectangle));

            pen.Color = Color.Black;

            if (!cooler.IsActive)
                pen.Color = Color.DarkOrange;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            gp.AddEllipse(rectangle);
            gp.AddLine(rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            gp.AddLine(rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            gp.AddLine(rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            gp.AddLine(rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            gp.AddLine(rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);

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

            LinearGradientBrush BodyBrush = new
                (new PointF(RotatedRectangle.Left, 0), new PointF(RotatedRectangle.Right, 0), col2, col1);

            g.FillPath(BodyBrush, gp);
            g.DrawPath(pen, gp);

            pen.Dispose();
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawCooler(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                cooler = (Cooler)info.GetValue("cooler", typeof(Cooler));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("cooler", cooler);
        }

        [Serializable]
        public class SaveableData
        {
            public SaveableData()
            {
            }

            public double ExitTemperature = 70;
            public double DeltaTemperature = 0;
            public double DeltaPress = 0.5;
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            CoolerDialog dlg = new CoolerDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //this.IsSolved = false;
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product = this.Hotspots.Search("PRODUCT");
            Q = this.Hotspots.Search("ENERGY");
        }
    }
}