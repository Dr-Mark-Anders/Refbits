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
    ///
    [Serializable]
    internal class DrawIOExchanger : DrawRectangle, ISerializable
    {
        //SaveableData data = new  SaveableData();
        private SaveableData data = new SaveableData();

        public Units.DrawIOExchanger.SaveableData Data
        {
            get { return data; }
            set { data = value; }
        }

        private UOMProperty duty = new UOMProperty(ePropID.EnergyFlow);

        [CategoryAttribute("Properties"),
         Description("kJ/hr")]
        public UOMProperty Duty
        {
            get { return duty; }
            set { duty.BaseValue = value; }
        }

        public UOMProperty UA
        {
            get { return Data.UA; }
            set { Data.UA = value; }
        }

        public Node TSIN;
        public Node TSOUT;
        public Node SSIN;
        public Node SSOUT;

        //public  HotSpot CSIN;
        //public  HotSpot CSOUT;
        private Node hSOUT;

        private Node hSIN;
        private static int Count;

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

        public UOMProperty ShellSideDT
        {
            get
            {
                return Data.shellSideDT;
            }
            set
            {
                Data.shellSideDT = value;
                //IsSolved = false;
            }
        }

        public UOMProperty TubeSideDT
        {
            get
            {
                return Data.TubeSideDT;
            }
            set
            {
                Data.TubeSideDT = value;
                //IsSolved = false;
            }
        }

        public UOMProperty ShellSideDP
        {
            get
            {
                return Data.ShellSideDP;
            }
            set
            {
                Data.ShellSideDP = value;
                //IsSolved = false;
            }
        }

        public UOMProperty TubeSideDP
        {
            get
            {
                return Data.TubeSideDP;
            }
            set
            {
                Data.TubeSideDP = value;
                //IsSolved = false;
            }
        }

        public DrawIOExchanger()
            : this(0, 0, 1, 1)
        {
        }

        public DrawIOExchanger(int x, int y, int width, int height)
            : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 0f, 0.5f, "TSIN", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Hotspots.Add(new Node(this, 1f, 0.5f, "TSOUT", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Hotspots.Add(new Node(this, 0.5f, 0f, "SSIN", NodeDirections.Up, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));  //feed
            Hotspots.Add(new Node(this, 0.5f, 1f, "SSOUT", NodeDirections.Down, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));  //product
            Initialize();

            this.Name = "IOExchanger" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawHeater drawRectangle = new DrawHeater();
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Pen pen = new Pen(StreamColor, PenWidth);
            pen.Color = Color.White;

            g.FillEllipse(pen.Brush, DrawRectangle.GetNormalizedRectangle(rectangle));
            pen.Color = Color.Black;

            Color col2 = Color.White;
            Color col1 = Color.Gray;

            LinearGradientBrush BodyBrush = new System.Drawing.Drawing2D.LinearGradientBrush
                (new PointF(this.rectangle.Left, 0), new PointF(this.rectangle.Right, 0), col2, col1);

            g.FillEllipse(BodyBrush, DrawRectangle.GetNormalizedRectangle(rectangle));
            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(rectangle));

            g.DrawLine(pen, rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(pen, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            g.DrawLine(pen, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            g.DrawLine(pen, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(pen, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);
            pen.Dispose();
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

        public Node HSIN
        {
            get
            {
                return hSIN;
            }

            set
            {
                hSIN = value;
            }
        }

        public Node HSOUT
        {
            get
            {
                return hSOUT;
            }

            set
            {
                hSOUT = value;
            }
        }

        public void UpdateFlowType(NodeCollection Feeds, NodeCollection Products)
        {
        }

        public void UpdateCompositionsType(NodeCollection Feeds, NodeCollection Products)
        {
        }

        public void UpdatePressType(NodeCollection Feeds, NodeCollection Product)
        {
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            TSIN = this.Hotspots.Search("TSIN");
            TSOUT = this.Hotspots.Search("TSOUT");
            SSIN = this.Hotspots.Search("SSIN");
            SSOUT = this.Hotspots.Search("SSOUT");

            DrawingObjectDialogs.HeatExIODialog dlg = new DrawingObjectDialogs.HeatExIODialog(this);
            dlg.ShowDialog();
            //exc.IsSolved = false;
            RaiseValueChangedEvent(new EventArgs());
        }

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawIOExchanger(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            info.AddValue("data", Data);
            info.AddValue("duty", duty);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                Data = (SaveableData)info.GetValue("data", typeof(SaveableData));
                duty = (UOMProperty)info.GetValue("duty", typeof(UOMProperty));
            }
            catch
            {
            }
            base.GetObjectData(info, context);
        }

        [Serializable]
        public class SaveableData
        {
            public SaveableData()
            {
            }

            public UOMProperty TubeSideDT = new UOMProperty(ePropID.DeltaT, 10);
            public UOMProperty shellSideDT = new UOMProperty(ePropID.DeltaT, 10);
            public UOMProperty TubeSideDP = new UOMProperty(ePropID.DeltaP, 0.5);
            public UOMProperty ShellSideDP = new UOMProperty(ePropID.DeltaP, 0.5);
            public UOMProperty UA = new UOMProperty(ePropID.UA, 1e4);
        }
    }
}