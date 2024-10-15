using Extensions;
using System;
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
    internal class DrawIsom : Units.DrawRectangle, ISerializable
    {
        public DrawIsom() : this(0, 0, 1, 1)
        {
        }

        public DrawIsom(int x, int y, int width, int height)
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
            Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0f, "VAPPRODUCT", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.1f, "TOPPRODUCT", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 1f, "BOTPRODUCT", NodeDirections.Down, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Initialize();

            this.Name = "Isom" + Count.ToString();
            Count++;
        }

        public enum CompList
        {
            H2,
            C1,
            C2,
            C3,
            IC4,
            NC4,
            IC5,
            NC5,
            IC6,
            NC6,
            CH,
            MCP,
            BEN,
            IC7,
            NC7,
            MCH,
            C7CP,
            TOL,
            C8P,
            ECH,
            DMCH,
            PCP,
            C8CP,
            EB,
            PX,
            MX,
            OX,
            C9P,
            C9CH,
            C9CP,
            C9A,
        }

        public enum DBCompList
        {
            HYDROGEN,
            METHANE,
            ETHANE,
            PROPANE,
            ISOBUTANE,
            NBUTANE,
            ISOPENTANE,
            NPENTANE,
            IC6,
            NC6,
            CH,
            MCP,
            BEN,
            IC7,
            NC7,
            MCH,
            C7CP,
            TOL,
            C8P,
            ECH,
            DMCH,
            PCP,
            C8CP,
            EB,
            PX,
            MX,
            OX,
            C9P,
            C9CH,
            C9CP,
            C9A,
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawIsom drawRectangle = new DrawIsom();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public enum Target
        {
            Octane,
            RxTemp
        }

        private Target target = Target.Octane;

        public Target Target1
        {
            get { return target; }
            set { target = value; }
        }

        private double sepPressure;

        public double SepPressure
        {
            get
            {
                return sepPressure;
            }
            set
            {
                sepPressure = value;
            }
        }

        private double sepTemperature;

        public double SepTemperature
        {
            get
            {
                return sepTemperature;
            }
            set
            {
                sepTemperature = value;
            }
        }

        private double lhsv;

        public double LHSV
        {
            get
            {
                return lhsv;
            }
            set
            {
                lhsv = value;
            }
        }

        private double p, n, a;

        public double P
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
            }
        }

        public double N
        {
            get
            {
                return n;
            }
            set
            {
                n = value;
            }
        }

        public double A
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
            }
        }

        private double sg;

        public double SG
        {
            get
            {
                return sg;
            }
            set
            {
                sg = value;
            }
        }

        private double h2_hcratio;
        private static int Count;

        public double H2_HCRatio
        {
            get
            {
                return h2_hcratio;
            }
            set
            {
                h2_hcratio = value;
            }
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality ;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Pen pen = new Pen(StreamColor, PenWidth);

                int rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                int r = this.rectangle.Width - rhoff;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);

                g.DrawArc(pen, R.TopLeft().X, R.TopLeft().Y, this.rectangle.Width, ArcHeight, 180, 180);
                g.DrawLine(pen, R.TopLeft().X, R.TopLeft().Y + ArcHeight / 2, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight / 2);
                g.DrawLine(pen, R.TopRight().X, R.TopRight().Y + ArcHeight / 2, R.BottomRight().X, R.BottomRight().Y - ArcHeight / 2);
                g.DrawArc(pen, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight, this.rectangle.Width, ArcHeight, 0, 180);
            }
            catch { }
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawIsom(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            MessageBox.Show("DBLCLick Isom");
        }
    }
}