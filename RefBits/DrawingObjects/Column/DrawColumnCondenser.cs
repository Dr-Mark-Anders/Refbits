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
    internal class DrawColumnCondenser : DrawRectangle, ISerializable
    {
        //public DrawColumn drawColumn;

        private Node feed, vaproduct, reflux, liqproduct, water;
        internal CondType CondenserType;
        internal DrawColumnTraySection section;

        public DrawColumnCondenser() : this(0, 0, 1, 1)
        {
        }

        public DrawColumnCondenser(int x, int y, int width, int height) : base()
        {
            if (width < 50)
                width = 50;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            feed = new Node(this, 0.0f, 0.5f, "FEED", NodeDirections.Right, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle);
            vaproduct = new Node(this, 0.5f, 0.0f, "VapProduct", NodeDirections.Up, HotSpotType.TrayNetVapour, HotSpotOwnerType.DrawRectangle);
            reflux = new Node(this, 0.4f, 1.0f, "Reflux", NodeDirections.Down, HotSpotType.Stream, HotSpotOwnerType.DrawRectangle);
            liqproduct = new Node(this, 1f, 0.5f, "LiqProduct", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle);
            water = new Node(this, 0.6f, 1f, "Water", NodeDirections.Right, HotSpotType.Water, HotSpotOwnerType.DrawRectangle);

            Hotspots.Add(feed);
            Hotspots.Add(vaproduct);
            Hotspots.Add(reflux);
            Hotspots.Add(liqproduct);
            Hotspots.Add(water);

            Initialize();
        }

        [OnDeserialized()]
        public new static void OnDeserializedMethod(StreamingContext context)
        {
        }

        public bool ValidateConnections(GraphicsList gl)
        {
            DrawStreamCollection fs = gl.ReturnExternalFeedStreams(this);
            DrawStreamCollection ps = gl.ReturnSideProductStreams(this, true);

            Guid startid = Guid.NewGuid();

            if (fs.Count > 0)
                startid = fs[0].StartDrawObjectGuid;

            foreach (DrawMaterialStream ds in ps)
            {
                if (ds.EndDrawObjectGuid == startid)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawColumnCondenser drawRectangle = new();
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
            Pen pen = new(StreamColor, PenWidth);
            pen.Color = Color.White;

            g.FillEllipse(pen.Brush, GetNormalizedRectangle(rectangle));
            pen.Color = Color.Black;

            Color col2;
            Color col1;

            if (section is not null && section.CondenserType != ModelEngine.CondType.None)
            {
                col2 = Color.White;
                col1 = Color.Gray;
            }
            else
            {
                col2 = Color.White;
                col1 = Color.Red;
            }

            LinearGradientBrush BodyBrush = new(new PointF(this.rectangle.Left, 0), new PointF(this.rectangle.Right, 0), col2, col1);

            g.FillEllipse(BodyBrush, GetNormalizedRectangle(rectangle));
            g.DrawEllipse(pen, GetNormalizedRectangle(rectangle));

            g.DrawLine(pen, rectangle.Left, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(pen, rectangle.Left + rectangle.Width / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4);
            g.DrawLine(pen, rectangle.Left + rectangle.Width / 3, rectangle.Top + rectangle.Height / 4, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4);
            g.DrawLine(pen, rectangle.Left + rectangle.Width * 2 / 3, rectangle.Top + rectangle.Height * 3 / 4, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2);
            g.DrawLine(pen, rectangle.Left + rectangle.Width * 3 / 4, rectangle.Top + rectangle.Height / 2, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height / 2);
            pen.Dispose();
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            ColumnDrawCondenserDLG dg = new(this, section);
            dg.ShowDialog();
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

        public Node Feed
        {
            get
            {
                return feed;
            }

            set
            {
                feed = value;
            }
        }

        public Node Vaproduct
        {
            get
            {
                return vaproduct;
            }

            set
            {
                vaproduct = value;
            }
        }

        public Node Reflux
        {
            get
            {
                return reflux;
            }

            set
            {
                reflux = value;
            }
        }

        public Node Liqproduct
        {
            get
            {
                return liqproduct;
            }

            set
            {
                liqproduct = value;
            }
        }

        public Node Water
        {
            get
            {
                return water;
            }

            set
            {
                water = value;
            }
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawColumnCondenser(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                feed = (Node)info.GetValue("feed", typeof(Node));
                vaproduct = (Node)info.GetValue("vaproduct", typeof(Node));
                reflux = (Node)info.GetValue("reflux", typeof(Node));
                liqproduct = (Node)info.GetValue("liqproduct", typeof(Node));
                water = (Node)info.GetValue("water", typeof(Node));
                CondenserType = (CondType)info.GetValue("CondType", typeof(CondType));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("feed", feed);
            info.AddValue("vaproduct", vaproduct);
            info.AddValue("reflux", reflux);
            info.AddValue("liqproduct", liqproduct);
            info.AddValue("water", water);
            info.AddValue("CondType", CondenserType);

            base.GetObjectData(info, context);
        }
    }
}