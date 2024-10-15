using Extensions;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Units
{
    public enum eStripperType
    { Stripped, Reboiled, None }

    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    public class DrawColumnTraySection : DrawRectangle, ISerializable, IComparable, IEnumerable
    {
        public Column column;
        private List<DrawTray> drawTrays = new();
        private readonly TraySection traysection = new();
        private readonly DrawTray CondenserStage = new();
        private readonly DrawTray ReboilerStage = new();
        public DrawColumn drawColumn;
        private bool hideUnconnctedTrays = false;

        //private  CondType condType = CondType.None;
        //private  ReboilerType reboilerType = ReboilerType.None;

        private Node vapProduct, botProduct;
        private int sectionNumber = 0;
        private Guid stripFeedGuid, stripVapourGuid;
        private DrawMaterialStream stripFeed, stripVapour;  // handle differently to normal tray section;
        private eStripperType striptype = eStripperType.None;
        private double stripperDrawFactor = 0;

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            foreach (DrawTray tray in drawTrays)
                tray.Owner = this;
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        public DrawTray this[Guid index]
        {
            get
            {
                foreach (var item in drawTrays)
                    if (item.Guid == index)
                        return item;
                return null;
            }
            set
            {
            }
        }

        public override string ToString()
        {
            return "DrawTraySection: " + this.Name;
        }

        public DrawMaterialStream StripperVapourProduct
        {
            get
            {
                return stripVapour;
            }
            set
            {
                stripVapour = value;
            }
        }

        public DrawMaterialStream StripperTopFeed
        {
            get
            {
                return stripFeed;
            }
            set
            {
                stripFeed = value;
            }
        }

        public Guid StripperReturnTray
        {
            get
            {
                if (stripVapour != null)
                    return stripVapour.EnginereturnTrayGuid;
                else
                    return Guid.Empty;
            }

            set
            {
                stripVapour.EnginereturnTrayGuid = value;
            }
        }

        public Guid StripperDrawTray
        {
            get
            {
                if (stripFeed != null)
                    return stripFeed.EngineDrawTrayGuid;
                else
                    return Guid.Empty;
            }
            set
            {
                if (stripFeed != null)
                    stripFeed.EngineDrawTrayGuid = value;
            }
        }

        public List<DrawTray> DrawTrays
        {
            get { return drawTrays; }
            set { drawTrays = value; }
        }

        /// <summary>
        /// Trays and condenser/reboiler
        /// </summary>
        public TraySection TraySection
        {
            get
            {
                traysection.Clear();

                if (CondenserType != CondType.None)
                    traysection.Add(CondenserStage.Tray);

                foreach (DrawTray tray in drawTrays)
                    traysection.Add(tray.Tray);

                if (Reboilertype != ReboilerType.None)
                    traysection.Add(ReboilerStage.Tray);

                traysection.CondenserType = CondenserType;
                traysection.ReboilerType = this.Reboilertype;

                return traysection;
            }
        }

        public List<DrawTray> AllDrawColumnStages
        {
            get
            {
                List<DrawTray> stages = new();

                if (CondenserType != CondType.None)
                    stages.Add(CondenserStage);

                stages.AddRange(drawTrays);

                if (Reboilertype != ReboilerType.None)
                    stages.Add(ReboilerStage);

                return stages;
            }
        }

        public DrawColumnTraySection(DrawArea columndrawarea = null) : this(0, 0, 1, 1)
        {
            DrawArea = columndrawarea;
        }

        public DrawColumnTraySection(int x, int y, int width, int height, int NoTrays = 10) : base()
        {
            if (width < 50)
                width = 50;
            if (height < 300)
                height = 300;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            Initialize();

            vapProduct = new Node(this, 0.5f, 0f, "VAPPRODUCT", NodeDirections.Up, HotSpotType.TrayNetVapour, HotSpotOwnerType.DrawRectangle);
            botProduct = new Node(this, 0.5f, 1f, "BOTPRODUCT", NodeDirections.Down, HotSpotType.TrayNetLiquid, HotSpotOwnerType.DrawRectangle);

            Hotspots.Add(vapProduct);
            Hotspots.Add(botProduct);

            Active = true;

            for (int i = 0; i < NoTrays; i++)
                drawTrays.Add(new DrawTray(this));

            Name = "TraySection";
        }

        private double topPressure = 6;
        private double bottomPressure = 6;

        internal int Count
        {
            get { return drawTrays.Count; }
        }

        public double TopPressure
        {
            get { return topPressure; }
            set { topPressure = value; }
        }

        public double BottomPressure
        {
            get { return bottomPressure; }
            set { bottomPressure = value; }
        }

        public int NoTrays
        {
            get
            {
                if (TraySection == null)
                    return 0;
                else
                    return TraySection.Trays.Count;
            }
        }

        public void AddAllHotSpots()
        {
            for (int n = 0; n < drawTrays.Count; n++)
            {
                Hotspots.Add(drawTrays[n].FeedLeft);
                Hotspots.Add(drawTrays[n].FeedRight);
                Hotspots.Add(drawTrays[n].LiquidDrawLeft);
                Hotspots.Add(drawTrays[n].LiquidDrawRight);
                Hotspots.Add(drawTrays[n].VapourDraw);
            }
        }

        public void DrawNormalHotspots(Graphics g)
        {
            Pen pen = new(Hotspotcolor);

            if (Hotspots != null)
            {
                for (int i = 0; i < Hotspots.Count; i++)
                {
                    Node hs = Hotspots[i];
                    if (hs != null && !hs.IsConnected)
                    {
                        Rectangle rect = hs.Absolute;
                        if (hs != null)
                        {
                            if (hs.IsStripper)
                            {
                                pen.Color = hs.Color;
                                g.DrawEllipse(pen, rect.Left, rect.Top, 10, 10);
                            }
                            else
                            {
                                Brush b = Brushes.White;
                                g.FillRectangle(b, rect.Centered());
                                pen.Color = Color.Black;
                                g.DrawRectangle(pen, rect.Centered());
                            }
                        }
                    }
                }
            }
            pen.Dispose();
        }

        public void DrawTrayHotspots(Graphics g)
        {
            HotSpotDisplayType disptype = HotSpotDisplayType.All;

            if (DrawArea is not null)
            {
                if (DrawArea.ActiveTool == DrawArea.DrawToolType.Pointer)
                    disptype = HotSpotDisplayType.Inputs;

                /*  if (DrawArea.ControlPressed)
                      disptype = HotSpotDisplayType.Outputs;
                  else
                      disptype = HotSpotDisplayType.Inputs;
                */

                foreach (DrawTray tray in drawTrays)
                {
                    tray.hotspotdisplaytype = disptype;
                    tray.DrawHotSpot(g);
                }
            }
        }

        public override void DrawHotSpot(Graphics g)
        {
            DrawNormalHotspots(g);
            DrawTrayHotspots(g);
        }

        public override Node HitTestHotSpot(Point p)
        {
            if (Hotspots != null)
            {
                Node hs = Hotspots.GetHotSpotFromPoint(p);

                if (hs != null)
                    return hs;

                foreach (DrawTray t in drawTrays)
                {
                    if (t.HotSpots != null)
                    {
                        Node h = t.HotSpots.GetHotSpotFromPoint(p);
                        if (h != null)
                            return h;
                    }
                }
            }
            return null;
        }

        internal int GetTrayNo(Guid guid)
        {
            return drawTrays.IndexOf(GetTray(guid));
        }

        internal override Node GetNode(Guid NodeID)
        {
            if (Hotspots != null)
            {
                Node hs = Hotspots.Search(NodeID);

                if (hs != null)
                    return hs;

                foreach (DrawTray t in drawTrays)
                {
                    if (t.HotSpots != null)
                    {
                        Node h = t.HotSpots.Search(NodeID);

                        if (h != null)
                            return h;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawColumnTraySection drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public void SetNoTrays(int NoTrays)
        {
            if (DrawTrays.Count < NoTrays)
                InsertRange(DrawTrays.Count - 1, NoTrays - drawTrays.Count);
            else if (NoTrays < DrawTrays.Count)
                RemoveAt(DrawTrays.Count - 1, DrawTrays.Count - NoTrays);
        }

        internal DrawTray this[int index]
        {
            get
            {
                if (AllDrawColumnStages.Count > 0 && index >= 0 && index < AllDrawColumnStages.Count)
                    return AllDrawColumnStages[index];

                return null;
            }
            set
            {
                drawTrays[index] = value;
            }
        }

        private int ArcHeight
        {
            get
            {
                return Convert.ToInt32(w / 2);
            }
        }

        private int rhoff
        {
            get { return Convert.ToInt32(0.43 * this.rectangle.Width); }
        }

        private int r
        {
            get { return (this.rectangle.Width - rhoff); }
        }

        private int w
        {
            get { return (r); }
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            int right = R.TopRight().X;
            int t = R.Top;
            int b = R.Bottom;
            int h = b - t;
            //int MinTrayHeight = 10; // pixels

            UpdateTrayLocations();

            try
            {
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality ;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color col2 = Color.White;
                Color col1 = Color.Gray;

                Pen pen = new(StreamColor, PenWidth);

                if (striptype == eStripperType.None || Active)  // normal column sectios allways visisble
                    pen.Color = this.StreamColor;
                else
                    pen = new Pen(Color.Gray, 2);

                g.DrawArc(pen, R.TopLeft().X, R.TopLeft().Y, R.Width, ArcHeight, 180, 180);
                g.DrawLine(pen, R.TopLeft().X, R.TopLeft().Y + ArcHeight / 2, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight / 2);
                g.DrawLine(pen, R.TopRight().X, R.TopRight().Y + ArcHeight / 2, R.BottomRight().X, R.BottomRight().Y - ArcHeight / 2);
                g.DrawArc(pen, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight, R.Width, ArcHeight, 0, 180);

                Pen p;

                if (striptype == eStripperType.None || Active)
                    p = new Pen(Color.Black, 1);
                else
                    p = new Pen(Color.Gray, 1);

                LinearGradientBrush BodyBrush =
                    new(new PointF(this.rectangle.Left, 0), new PointF(right, 0), col2, col1);

                BodyBrush.SetBlendTriangularShape(1f);

                g.SmoothingMode = SmoothingMode.HighSpeed;

                GraphicsPath Body = new();
                RectangleF ColumnBody = new(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);
                Body.AddRectangle(ColumnBody);
                g.DrawPath(p, Body);

                if (striptype == eStripperType.None || Active)
                    g.FillPath(BodyBrush, Body);

                GraphicsPath Top = new();
                Top.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                PathGradientBrush TopBrush = new(Top);
                TopBrush.CenterPoint = new PointF(this.rectangle.Left, t + ArcHeight / 2);
                TopBrush.CenterColor = col2;
                TopBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Top);

                if (striptype == eStripperType.None || Active)
                    g.FillPath(TopBrush, Top);

                GraphicsPath Bottom = new();
                Bottom.AddArc(this.rectangle.Left, b - ArcHeight, this.rectangle.Width, ArcHeight, 0, 180);
                Bottom.CloseAllFigures();

                PathGradientBrush BotBrush = new(Bottom);

                BotBrush.CenterPoint = new PointF(this.rectangle.Left, b - ArcHeight);
                BotBrush.CenterColor = col2;
                BotBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Bottom);

                if (striptype == eStripperType.None || Active)
                    g.FillPath(BotBrush, Bottom);

                // draw lines

                // Set up all the string parameters.
                string stringText = "Sample Text";
                FontFamily family = new("Arial");
                int fontStyle = (int)FontStyle.Regular;
                int emSize = 10;

                StringFormat format = StringFormat.GenericDefault;
                GraphicsPath myPath = new();
                DrawTray tray;

                /*int step=1;
                if((this.rectangle.Height - ArcHeight * 2 )/ DrawTrays.Count < MinTrayHeight)
                {
                    step = (int)this.rectangle.Height / MinTrayHeight;
                }*/

                for (int i = 0; i < drawTrays.Count; i++)
                {
                    tray = drawTrays[i];
                    if (tray.MakeVisible)
                    {
                        // Add the string to the path.
                        Point origin = new(tray.Rectangle.Left + 5, tray.Rectangle.Bottom - 12);
                        stringText = (i + 1).ToString();
                        myPath.AddString(stringText, family, fontStyle, emSize,
                            origin, format);

                        g.DrawLine(pen, tray.Location.X, tray.Location.Bottom, tray.Location.X + rectangle.Width, tray.Location.Bottom);

                        // Debug.Print(drawTrays[i].Location.Bottom.ToString());

                        GraphicsPath traypath = new();
                        traypath.AddString(i.ToString(), FontFamily.GenericSansSerif, 1, 8, new PointF(0, 0), new StringFormat(StringFormatFlags.NoWrap));

                        if (tray.Colour == Color.Green)
                        {
                            traypath.AddString(i.ToString(), FontFamily.GenericSansSerif, 1, 8, new PointF(0, 0), new StringFormat(StringFormatFlags.NoWrap));
                            traypath.AddRectangle(tray.Location);
                            PathGradientBrush TrayBrush = new(traypath);
                            TrayBrush.CenterPoint = new PointF(tray.Location.Left, tray.Location.Top);
                            TrayBrush.CenterColor = Color.Green;
                            BotBrush.SurroundColors = new Color[] { tray.Colour };
                            Pen pp = new(Color.Green, 1);
                            g.DrawPath(pp, traypath);
                            g.FillPath(TrayBrush, traypath);
                        }
                    }
                }

                //Draw the path to the screen.
                g.FillPath(Brushes.Black, myPath);
            }
            catch { }
        }

        public int HideNonAttachedTrays()
        {
            int no = 0;
            for (int i = 0; i < drawTrays.Count; i++)
            {
                DrawTray t = drawTrays[i];

                if (t.IsConnected())
                {
                    t.MakeVisible = true;
                    no++;
                }
                else
                {
                    t.MakeVisible = false;
                    DrawTray t0 = drawTrays[i-1];
                    t.Location = new (t0.Location.X, t0.Location.Y,t0.Location.Width,t0.Location.Height);
                }
            }
            return no;
        }

        public void UpdateTrayLocations()
        {
            DrawTray t;
            int Left, Top, Width;
            Top = this.Location.Y;
            Left = this.Location.X;
            Width = this.rectangle.Width;
            double MinTrayHeight = 20;
            int step = 1;
            int VisibleTrays = drawTrays.Count;

            if (HideUnconnctedTrays)
            {
                VisibleTrays = HideNonAttachedTrays();
            }
            else
                for (int i = 0; i < drawTrays.Count; i += step)
                {
                    drawTrays[i].MakeVisible = true;
                }

            this.rectangle.Height = (int)MinTrayHeight * VisibleTrays + ArcHeight * 2;
            double Height = (this.rectangle.Height - ArcHeight * 2);
            double DisplayedTraysTot = Height / MinTrayHeight + 1;
            double AvgTrayHeight = Height / drawTrays.Count;

            if (DisplayedTraysTot > drawTrays.Count)
                step = 1;
            else
                step = (int)((double)drawTrays.Count / (double)DisplayedTraysTot);

            if (AvgTrayHeight > MinTrayHeight)
            {
                MinTrayHeight = AvgTrayHeight;
            }

            for (int i = 0; i < drawTrays.Count; i++)
            {
                if (drawTrays[i].MakeVisible)
                    drawTrays[i].Location = new Rectangle(Left, Top + (int)(MinTrayHeight * i), Width, (int)MinTrayHeight);
            }

            drawTrays.First().MakeVisible = true;
            drawTrays.Last().MakeVisible = true;
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

        public CondType CondenserType
        {
            get
            {
                return traysection.CondenserType;
            }
            set
            {
                traysection.CondenserType = value;
            }
        }

        public int SectionNo
        {
            get
            {
                return sectionNumber;
            }

            set
            {
                sectionNumber = value;
            }
        }

        public Node VapProduct
        {
            get
            {
                return vapProduct;
            }
            set
            {
                vapProduct = value;
            }
        }

        public Node BotProduct
        {
            get
            {
                return botProduct;
            }
            set
            {
                botProduct = value;
            }
        }

        public ReboilerType Reboilertype
        {
            get
            {
                return traysection.ReboilerType;
            }
            set
            {
                traysection.ReboilerType = value;
            }
        }

        public eStripperType Striptype
        {
            get
            {
                return striptype;
            }

            set
            {
                striptype = value;
            }
        }

        public Guid StripVapourGuid
        {
            get
            {
                return stripVapourGuid;
            }

            set
            {
                stripVapourGuid = value;
            }
        }

        public Guid StripFeedGuid
        {
            get
            {
                return stripFeedGuid;
            }

            set
            {
                stripFeedGuid = value;
            }
        }

        public double StripperDrawFactor
        {
            get
            {
                return stripperDrawFactor;
            }

            set
            {
                stripperDrawFactor = value;
            }
        }

        public DrawMaterialStream StripFeed
        {
            get
            {
                return stripFeed;
            }
            set
            {
                stripFeed = value;
            }
        }

        /// <summary>
        /// Move handle to new  point  (resizing)
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            switch (handleNumber)
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;

                case 2:
                    top = point.Y;
                    break;

                case 3:
                    right = point.X;
                    top = point.Y;
                    break;

                case 4:
                    right = point.X;
                    break;

                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;

                case 6:
                    bottom = point.Y;
                    break;

                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;

                case 8:
                    left = point.X;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            return Rectangle.IntersectsWith(rectangle);
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
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

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public DrawColumnTraySection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                drawTrays = (List<DrawTray>)info.GetValue("Trays", typeof(List<DrawTray>));
                botProduct = (Node)info.GetValue("BotProduct", typeof(Node));
                vapProduct = (Node)info.GetValue("VapProduct", typeof(Node));

                striptype = (eStripperType)info.GetValue("striptype", typeof(eStripperType));
                stripFeedGuid = (Guid)info.GetValue("stripFeedGuid", typeof(Guid));
                stripVapourGuid = (Guid)info.GetValue("stripVapourGuid", typeof(Guid));

                CondenserStage = (DrawTray)info.GetValue("CondenserStage", typeof(DrawTray));
                ReboilerStage = (DrawTray)info.GetValue("ReboilerStage", typeof(DrawTray)); ;

                traysection = (TraySection)info.GetValue("section", typeof(TraySection));
            }
            catch
            {
            }
        }

        internal DrawTray GetTray(Point p)
        {
            foreach (DrawTray tray in drawTrays)
            {
                if (tray.Rectangle.Contains(p))
                    return tray;
            }
            return null; ;
        }

        internal DrawTray GetTray(Guid guid)
        {
            foreach (DrawTray tray in drawTrays)
            {
                if (tray.Guid == guid)
                    return tray;
            }
            return null;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Trays", drawTrays, typeof(List<DrawTray>));
            info.AddValue("BotProduct", botProduct, typeof(Node));
            info.AddValue("VapProduct", vapProduct, typeof(Node));
            info.AddValue("striptype", striptype);
            info.AddValue("stripFeedGuid", stripFeedGuid);
            info.AddValue("stripVapourGuid", stripVapourGuid);

            info.AddValue("CondenserStage", CondenserStage);
            info.AddValue("ReboilerStage", ReboilerStage);

            info.AddValue("section", traysection); // mostly to just save the guid

            base.GetObjectData(info, context);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            DrawColumnTraySection otherTraySection = obj as DrawColumnTraySection;

            if (otherTraySection.NoTrays > NoTrays)
                return 1;
            if (otherTraySection.NoTrays < NoTrays)
                return -1;

            return 0;
        }

        public IEnumerator GetEnumerator()
        {
            return drawTrays.GetEnumerator();
        }

        public DrawTray TopTray
        {
            get { return AllDrawColumnStages[0]; }
        }

        public DrawTray BottomTray
        {
            get { return AllDrawColumnStages.Last(); }
        }

        public bool Active { get; internal set; }
        public bool HideUnconnctedTrays { get => hideUnconnctedTrays; set => hideUnconnctedTrays = value; }

        internal void Remove(DrawTray Name)
        {
            drawTrays.Remove(Name);
        }

        internal void Insert(int v1, DrawTray v2)
        {
            drawTrays.Insert(v1, v2);
        }

        internal void RemoveAt(int v1, int v2)
        {
            throw new NotImplementedException();
        }

        internal int IndexOf(DrawTray t)
        {
            return drawTrays.IndexOf(t);
        }

        internal void InsertRange(int loc, int number)
        {
            List<DrawTray> trays = new(number);
            this.drawTrays.InsertRange(loc, trays);
        }

        internal void EraseAllPortCnnections()
        {
            foreach (DrawTray dtray in drawTrays)
            {
                dtray.HotSpots.ClearConnected();
            }
        }

        internal void UpdateNodeGuids()
        {
            vapProduct.PortGuid = traysection.TopTray.TrayVapour.Guid;
            botProduct.PortGuid = traysection.BottomTray.TrayLiquid.Guid;
        }

        internal string[] TrayNames()
        {
            string[] Names = new string[drawTrays.Count];
            for (int i = 0; i < drawTrays.Count; i++)
            {
                Names[i] = drawTrays[i].Name;
            }
            return Names;
        }
    }
}