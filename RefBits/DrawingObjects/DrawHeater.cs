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
    internal class DrawHeater : DrawRectangle, ISerializable
    {
        private HeaterDialog dlg;
        private Heater heater = new();

        private Node Feed;
        private Node Product;
        //private  Node Q;

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return heater.IsDirty;
            }
            set
            {
                heater.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return heater.IsSolved;
            }
            /*  set
              {
                  heater.IsSolved = value;
              }*/
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = heater.PortIn.Guid;
            Hotspots["PRODUCT"].PortGuid = heater.PortOut.Guid;
            Hotspots["ENERGY"].PortGuid = heater.Q.Guid;

            heater.Name = this.Name;
            return true;
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return heater.PortIn;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return heater.PortOut;
            }
        }

        public Heater Heater { get => heater; set => heater = value; }

        public DrawHeater() : this(0, 0, 1, 1)
        {
        }

        public DrawHeater(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Product = Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Hotspots.Add(new Node(this, 0.5f, 1.0f, "ENERGY", NodeDirections.Down, HotSpotType.EnergyIn, HotSpotOwnerType.DrawRectangle));  //EnergyStream
            Initialize();

            this.Name = "HTR" + Count.ToString();
            Count++;
        }

        private static int Count;

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawHeater drawRectangle = new();
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
            GraphicsPath gp = new(FillMode.Winding);

            Pen pen = new(StreamColor, PenWidth);
            pen.Color = Color.White;

            gp.AddEllipse(DrawRectangle.GetNormalizedRectangle(rectangle));

            pen.Color = Color.Black;

            if (!heater.IsActive)
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
        public DrawHeater(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                heater = (Heater)info.GetValue("heater", typeof(Heater));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("heater", Heater);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            Feeds.Add(Feed);
            Products.Add(Product);

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new HeaterDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.Show();
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            Feed = this.Hotspots.Search("FEED");
            Product = this.Hotspots.Search("PRODUCT");
            //Q = this.Hotspots.Search("PRODUCT");
        }
    }
}