using ModelEngine;
using Extensions;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Units.UOM;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>

    [Serializable]
    internal class DrawAssayCutter : DrawRectangle, ISerializable
    {
        private AssayCutter cutter = new();

        public DrawAssayCutter() : this(0, 0, 1, 1)
        {
        }

        private static int Count;
        public Components[] StreamArr = new Components[0];

        public List<Tuple<Temperature, Temperature>> CutPoints
        {
            get { return cutter.CutPoints; }
            set { cutter.CutPoints = value; }
        }

        public DrawAssayCutter(int x, int y, int width, int height) : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Hotspots.Add(new Node(this, 0f, 0.5f, "FEED", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0.5f, 0.0f, "PRODUCT1", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.1f, "PRODUCT2", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.2f, "PRODUCT3", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.3f, "PRODUCT4", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.4f, "PRODUCT5", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT6", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.6f, "PRODUCT7", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.7f, "PRODUCT8", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.8f, "PRODUCT9", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0.5f, 0.99f, "PRODUCT10", NodeDirections.Down, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Initialize();

            this.Name = "AssayCutter" + Count.ToString();
            Count++;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawAssayCutter drawRectangle = new DrawAssayCutter();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public override bool UpdateAttachedModel()
        {
            Hotspots["FEED"].PortGuid = cutter.PortIn.Guid;
            Hotspots["PRODUCT1"].PortGuid = cutter.PortOut1.Guid;
            Hotspots["PRODUCT2"].PortGuid = cutter.PortOut2.Guid;
            Hotspots["PRODUCT3"].PortGuid = cutter.PortOut3.Guid;
            Hotspots["PRODUCT4"].PortGuid = cutter.PortOut4.Guid;
            Hotspots["PRODUCT5"].PortGuid = cutter.PortOut5.Guid;
            Hotspots["PRODUCT6"].PortGuid = cutter.PortOut6.Guid;
            Hotspots["PRODUCT7"].PortGuid = cutter.PortOut7.Guid;
            Hotspots["PRODUCT8"].PortGuid = cutter.PortOut8.Guid;
            Hotspots["PRODUCT9"].PortGuid = cutter.PortOut9.Guid;
            Hotspots["PRODUCT10"].PortGuid = cutter.PortOut10.Guid;

            cutter.Name = this.Name;

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
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality ;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color col2 = Color.White;
                Color col1 = Color.Gray;
                Rectangle R = this.rectangle;

                Pen pen = new Pen(Color.Black, 2);

                int rhoff = Convert.ToInt32(0.43 * R.Width);
                int r = R.Width - rhoff;
                int right = R.TopRight().X;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);
                int t = R.Top;
                int b = R.Bottom;
                int h = b - t;

                g.DrawArc(pen, R.TopLeft().X, R.TopLeft().Y, R.Width, ArcHeight, 180, 180);
                g.DrawLine(pen, R.TopLeft().X, R.TopLeft().Y + ArcHeight / 2, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight / 2);
                g.DrawLine(pen, R.TopRight().X, R.TopRight().Y + ArcHeight / 2, R.BottomRight().X, R.BottomRight().Y - ArcHeight / 2);
                g.DrawArc(pen, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight, this.rectangle.Width, ArcHeight, 0, 180);

                Pen p = new Pen(Color.Black, 1);
                LinearGradientBrush BodyBrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(new PointF(this.rectangle.Left, 0),
                        new PointF(right, 0), col2, col1);
                BodyBrush.SetBlendTriangularShape(1f);
                g.SmoothingMode = SmoothingMode.HighSpeed;

                GraphicsPath Body = new GraphicsPath();
                RectangleF ColumnBody = new RectangleF(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);
                Body.AddRectangle(ColumnBody);
                g.DrawPath(p, Body);
                g.FillPath(BodyBrush, Body);

                GraphicsPath Top = new GraphicsPath();
                Top.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                PathGradientBrush TopBrush = new System.Drawing.Drawing2D.PathGradientBrush(Top);
                TopBrush.CenterPoint = new PointF(this.rectangle.Left, t + ArcHeight / 2);
                TopBrush.CenterColor = col2;
                TopBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Top);
                g.FillPath(TopBrush, Top);

                GraphicsPath Bottom = new GraphicsPath();
                Bottom.AddArc(this.rectangle.Left, b - ArcHeight, this.rectangle.Width, ArcHeight, 0, 180);
                Bottom.CloseAllFigures();

                PathGradientBrush BotBrush = new System.Drawing.Drawing2D.PathGradientBrush(Bottom);
                //BotBrush.SetSigmaBellShape(1f,1f);
                BotBrush.CenterPoint = new PointF(this.rectangle.Left, b - ArcHeight);
                BotBrush.CenterColor = col2;
                BotBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Bottom);
                g.FillPath(BotBrush, Bottom);

                /*GraphicsPath Drum = new  GraphicsPath();
                RectangleF D = new  RectangleF(r + w / 1.5f, t - h / 4f, w / 3f, h / 4.5f);
                Drum.AddRectangle(D);
                g.DrawPath(p, Drum);
                g.FillPath(BodyBrush, Drum);*/

                // draw lines
                for (int i = 1; i < 10; i++)
                {
                    int Height = rectangle.Height * i / 10;
                    g.DrawLine(pen, R.TopLeft().X, R.TopLeft().Y + Height, R.TopRight().X, R.TopRight().Y + Height);
                }
            }
            catch { }

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

        public AssayCutter Cutter { get => cutter; set => cutter = value; }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawAssayCutter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                cutter = (AssayCutter)info.GetValue("Cutter", typeof(AssayCutter));
            }
            catch { }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Cutter", cutter);
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            AssayCutterDLG dlg = new AssayCutterDLG(this, this);
            dlg.ShowDialog();
        }
    }
}