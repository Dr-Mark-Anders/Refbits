using ModelEngine;
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
    [Serializable]
    internal class DrawColumnReboiler : Units.DrawRectangle, ISerializable
    {
        public DrawColumnReboiler() : this(0, 0, 1, 1)
        {
        }

        //public DrawColumn drawColumn;

        private Node feed, ReboilVap, liqproduct;
        public ReboilerType reboilertype = ReboilerType.Kettle;
        internal DrawColumnTraySection section;

        public DrawColumnReboiler(int x, int y, int width, int height)
            : base()
        {
            if (width < 50)
                width = 50;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            feed = Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Right, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            ReboilVap = Hotspots.Add(new Node(this, 0.5f, 0f, "Reboil Vap", NodeDirections.Up, HotSpotType.TrayNetVapour, HotSpotOwnerType.DrawRectangle));
            liqproduct = Hotspots.Add(new Node(this, 1f, 0.5f, "LiqProduct", NodeDirections.Left, HotSpotType.BottomTrayLiquid, HotSpotOwnerType.DrawRectangle));
            Initialize();
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
        }

        public bool ValidateConnections(GraphicsList gl)
        {
            DrawStreamCollection fs = gl.ReturnExternalFeedStreams(this);
            DrawStreamCollection ps = gl.ReturnSideProductStreams(this, true);

            if (fs.Count > 0)
            {
                Guid startid = fs[0].StartDrawObjectGuid;

                foreach (DrawMaterialStream ds in ps)
                {
                    if (ds.EndDrawObjectGuid == startid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private double pressure = 1;

        public double Pressure
        {
            get { return pressure; }
            set { Pressure = value; }
        }

        private double temperature = 1;

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public bool Calculate(DrawArea da)
        {
            GraphicsList gl = da.GraphicsList;

            try
            {
                DrawMaterialStream FeedStream = gl.ReturnStream(Guid, "FEED");
                DrawMaterialStream VapProd = gl.ReturnStream(Guid, "VapProduct");
                DrawMaterialStream LiqProd = gl.ReturnStream(Guid, "LiqProduct");
            }
            catch
            {
            }
            return true;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawColumnReboiler drawRectangle = new DrawColumnReboiler();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            ColumnDrawreboilerDLG dg = new(this, section);
            dg.ShowDialog();
        }
        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Pen pen = new Pen(StreamColor, PenWidth);
                pen.Color = Color.White;

                g.FillEllipse(pen.Brush, DrawRectangle.GetNormalizedRectangle(rectangle));
                pen.Color = Color.Black;

                Color col2;
                Color col1;

                if (section is not null && section.Reboilertype!=ModelEngine.ReboilerType.None)
                {
                    col2 = Color.White;
                    col1 = Color.Gray;
                }
                else
                {
                    col2 = Color.White;
                    col1 = Color.Red;
                }

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
            catch { }
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

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawColumnReboiler(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                ReboilVap = (Node)info.GetValue("ReboilVap", typeof(Node));
                liqproduct = (Node)info.GetValue("liqproduct", typeof(Node));
                feed = (Node)info.GetValue("feed", typeof(Node));
                reboilertype = (ReboilerType)info.GetValue("reboilertype", typeof(ReboilerType));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ReboilVap", ReboilVap);
            info.AddValue("liqproduct", liqproduct);
            info.AddValue("feed", feed);
            info.AddValue("reboilertype", reboilertype);
            base.GetObjectData(info, context);
        }

        [OnDeserializing]
        private void SetCountryRegionDefault(StreamingContext sc)
        {
            // reboilerType = ReboilerType.Kettle;
        }
    }
}