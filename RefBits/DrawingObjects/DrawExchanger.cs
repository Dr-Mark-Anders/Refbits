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
    public enum EnumSpec
    {
        UA, LMTD, DUTY
    }

    public enum EnumHotSide
    {
        Tube, Shell
    }

    [Serializable]
    internal class DrawExchanger : DrawRectangle, ISerializable
    {
        public double specvalue = 0.02;

        private static int Count = 1;
        public HeatExDialog dlg;
        private HeatExchanger2 exchanger = new();

        public EnumSpec Spec
        {
            get { return spec; }
            set { spec = value; }
        }

        public Node TSIN;
        public Node TSOUT;
        public Node SSIN;
        public Node SSOUT;

        public Node CSIN = null;
        public Node CSOUT = null;
        public Node HSIN = null;
        public Node HSOUT = null;
        private EnumSpec spec = EnumSpec.UA;
        private EnumHotSide hotside = EnumHotSide.Shell;

        public Port_Material tsin;
        public Port_Material ssin;
        public Port_Material tsout;
        public Port_Material ssout;

        public Port_Material hsin;
        public Port_Material hsout;
        public Port_Material csin;
        public Port_Material csout;

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return exchanger.IsSolved;
            }
          /*  set
            {
                exchanger.IsSolved = value;
            }*/
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return exchanger.IsDirty;
            }
            set
            {
                exchanger.IsDirty = value;
            }
        }

       /* [Category("Calculation"), Description("SolveType")]
        public SpecType SolveType
        {
            get
            {
                return exchanger.SolveType;
            }
        }*/

        public override bool UpdateAttachedModel()
        {
            Hotspots["TSIN"].PortGuid = exchanger.tubePortIn.Guid;
            Hotspots["TSOUT"].PortGuid = exchanger.tubePortOut.Guid;
            Hotspots["SSIN"].PortGuid = exchanger.shellPortIn.Guid;
            Hotspots["SSOUT"].PortGuid = exchanger.shellPortOut.Guid;
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
                    return true;
            }
            return false;
        }

        public DrawExchanger() : this(0, 0, 5, 5)
        {
        }

        public DrawExchanger(int x, int y, int width, int height, DrawArea DrawArea) : this(x, y, width, height)
        {
            this.DrawArea = DrawArea;
        }

        public DrawArea Parent
        {
            get
            {
                return this.DrawArea;
            }
        }

        public DrawExchanger(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            // create nodes
            TSIN = Hotspots.Add(new Node(this, 0f, 0.5f, "TSIN", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            TSOUT = Hotspots.Add(new Node(this, 1f, 0.5f, "TSOUT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            SSIN = Hotspots.Add(new Node(this, 0.5f, 0f, "SSIN", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            SSOUT = Hotspots.Add(new Node(this, 0.5f, 1f, "SSOUT", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product

            Initialize();

            this.Name = "E" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawHeater drawRectangle = new();
            drawRectangle.Rectangle = this.rectangle;

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
            GraphicsPath gp = new();
            gp.FillMode = FillMode.Winding;

            Pen pen = new(StreamColor, PenWidth);
            pen.Color = Color.White;

            gp.AddEllipse(GetNormalizedRectangle(rectangle));

            pen.Color = Color.Black;

            if (!exchanger.IsActive)
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

            //  Bitmap bitmap = new  Bitmap(Convert.ToInt32(1024), Convert.ToInt32(1024), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //  Graphics g = Graphics.FromImage(bitmap);
            //  e.Graphics.DrawImage(bitmap, 0, 0);

            //Bitmap bmp = new  Bitmap(100, 100, g);

            pen.Dispose();
            base.Draw(g);
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

        public HeatExchanger2 Exchanger { get => exchanger; set => exchanger = value; }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            TSIN = this.Hotspots.Search("TSIN");
            TSOUT = this.Hotspots.Search("TSOUT");
            SSIN = this.Hotspots.Search("SSIN");
            SSOUT = this.Hotspots.Search("SSOUT");

            Hotspots.UpdateAttachedStreamNames(drawArea.GraphicsList);

            dlg = new HeatExDialog(this);
            dlg.DLGValueChangedEvent += new BaseDialog.DialogValueChangedEventHandler(Dlg_ValueChangedEvent);

            dlg.Show();
            //this.IsSolved = false;
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawExchanger(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                exchanger = (HeatExchanger2)info.GetValue("exchanger", typeof(HeatExchanger2));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("exchanger", exchanger, typeof(HeatExchanger2));
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            TSIN = this.Hotspots.Search("TSIN");
            TSOUT = this.Hotspots.Search("TSOUT");
            SSIN = this.Hotspots.Search("SSIN");
            SSOUT = this.Hotspots.Search("SSOUT");
        }
    }
}