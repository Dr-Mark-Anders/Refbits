using Main.Images;
using ModelEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    internal class DrawPump : DrawRectangle, ISerializable
    {
        private PumpDialog dlg;
        private Pump pump = new();

        private static int Count;

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = pump.PortIn.Guid;
            Hotspots["PRODUCT"].PortGuid = pump.PortOut.Guid;
            Hotspots["ENERGY"].PortGuid = pump.Q.Guid;

            pump.Name = this.Name;
            //pump.Eff.Value = pump.Eff.Guid;
            return true;
        }

        public override void PostSolve()
        {
        }

        [Category("Ports"), Description("In Port")]
        public Port PortIn
        {
            get
            {
                return pump.PortIn;
            }
        }

        [Category("Ports"), Description("Out Port")]
        public Port PortOut
        {
            get
            {
                return pump.PortOut;
            }
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return pump.IsDirty;
            }
            set
            {
                pump.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return pump.IsSolved;
            }
            set
            {
                //pump.IsSolved = value;
            }
        }

        public Pump Pump { get => pump; set => pump = value; }

        public DrawPump() : this(0, 0, 1, 1)
        {
        }

        public DrawPump(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 0.0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Hotspots.Add(new Node(this, 1f, 0f, "PRODUCT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Hotspots.Add(new Node(this, 0.5f, 1.0f, "ENERGY", NodeDirections.Down, HotSpotType.EnergyIn, HotSpotOwnerType.DrawRectangle));  //EnergyStream
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

        public DrawPump(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                pump = (Pump)info.GetValue("pump", typeof(Pump));
            }
            catch
            {
            }

            pump.Name = this.Name;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("pump", pump);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            /* GraphicsPath gp = new  GraphicsPath();
             gp.FillMode = FillMode.Winding;
             g.SmoothingMode = SmoothingMode.HighQuality ;
             Pen pen = new  Pen(StreamColor, PenWidth);

             Color col2 = Color.White;
             Color col1 = Color.Gray;

             Point [] point s = new  Point [3];
             point s[0] = R.BottomLeft();
             point s[1] = R.Centre();
             point s[2] = R.BottomRight();

             LinearGradientBrush BodyBrush = new  LinearGradientBrush(new  PointF(RotatedRectangle.Left, 0),
                 new  PointF(this.RotatedRectangle.Right, 0), col2, col1);

             gp.AddLines(point s);
             gp.AddEllipse(R);
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

             pen.Color = StreamColor;

             g.DrawPath(pen, gp);
             g.FillPath(BodyBrush, gp);
             g.DrawEllipse(pen, R);

             pen.Dispose();

             base.Draw(g);*/

            //Bitmap img = Images.Pump();

            g.DrawImage(Images.Pump(), this.rectangle);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            dlg = new PumpDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.ShowDialog();
            //pump.IsSolved = false;
        }
    }
}