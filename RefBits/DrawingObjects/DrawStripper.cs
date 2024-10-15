using EngineThermo;
using Extensions;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///

    [Serializable]
    class DrawStripper : DrawRectangle, ISerializable
    {
        int notrays = 3;
        bool isreboiled = false;
        UOMProperty duty;
        int drawtray = 7, returnTray = 6;
        int section;
        static int Count = 0;
        double topPressure = 6;

        public double  return  FlowSpecOrDefault()
        {
            foreach (Specification s in Specs)
            {
                if (s.SpecType == eSpecType.LiqFlow)
                {
                    return s.Value;
                }
            }
            return 0.1; // default 1kgmol/hr
        }

        public UOMProperty Duty
        {
            get { return duty; }
            set { duty = value; }
        }

        public int returnTray
        {
            get { return returnTray; }
            set { returnTray = value; }
        }

        public int returnTrayMatrix(int i)
        {
            return returnTray - 1 + i;
        }

        public int DrawTray
        {
            get { return drawtray; }
            set { drawtray = value; }
        }

        public int DrawTrayM(int i)
        {
            return drawtray - 1 + i;
        }

        enumflowtype flowtype = enumflowtype.ActStdLiqVol;

        public new enumflowtype Flowtype
        {
            get { return flowtype; }
            set { flowtype = value; }
        }

        public new bool Active
        {
            get { return base.Active; }
            set { base.Active = value; }
        }

        public bool IsReboiled
        {
            get { return isreboiled; }
            set { isreboiled = value; }
        }

        eStripperType striptype = eStripperType.Stripped;

        public new Func<object> OnRequestParentObject = null;

        protected new object RequestParentObject()
        {
            if (OnRequestParentObject == null)
                throw new Exception("OnRequestParentObject handler is not assigned");

            return OnRequestParentObject();
        }

        public new object GetParent()
        {
            object parent = RequestParentObject();
            return parent;
        }

        public bool ValidateConnections(GraphicsList gl)
        {
            DrawStreamCollection fs = gl.return FeedStreams(this);
            DrawStreamCollection ps = gl.return SideProductStreams(this, true);

            Guid startid = new Guid();
            if (fs.Count > 0)
                startid = fs[0].StartID;

            foreach (DrawStream ds in ps)
            {
                if (ds.EndID == startid)
                {
                    return true;
                }
            }
            return false;
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            //base.OnDeserializedMethod(context);
            foreach (Tray t in trays)
            {
                t.OnRequestParentObject += new Func<object>(delegate { return this; });
            }

            Specs.SetDrawObjectGUID(this.Guid);
        }

        public List<DrawTray> Trays
        {
            get { return trays; }
            set { trays = value; }
        }

        public DrawStripper() : this(0, 0, 1, 1, 3)
        {
        }

        public DrawStripper(int x, int y, int width, int height, int NoTrays) : base()
        {
            if (width < 20)
                width = 20;
            if (height < 50)
                height = 50;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            Initialize();

            trays = new List<DrawTray>(NoTrays);

            UpdateTrayLocations();

            Hotspots.Add(new Node(this, 0.5f, 0f, "VAPPRODUCT", NodeDirections.Up, HotSpotType.Product, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0.5f, 1f, "BOTPRODUCT", NodeDirections.Down, HotSpotType.Product, HotSpotOwnerType.DrawRectangle));
            AddAllHotSpots();

            this.Name = "Stripper" + Count.ToString();
            Count++;
        }

        public void SetNoTrays(int NoTrays)
        {
            if (Trays.Count < NoTrays)
            {
                Trays.InsertRange(Trays.Count - 1, NoTrays - trays.Count);
            }
            else if (NoTrays < Trays.Count)
            {
                Trays.RemoveAt(Trays.Count - 1, Trays.Count - NoTrays);
            }
        }

        public double TopPressure
        {
            get { return topPressure; }
            set { topPressure = value; }
        }

        double bottomPressure = 6;

        public double BottomPressure
        {
            get { return bottomPressure; }
            set { bottomPressure = value; }
        }

        public int NoTrays
        {
            get
            {
                if (Trays == null)
                    return 0;
                else
                    return Trays.Count;
            }
        }

        public void AddAllHotSpots()
        {
            for (int n = 0; n < NoTrays; n++)
            {
                Hotspots.Add(trays[n].FeedLeft);
                Hotspots.Add(trays[n].FeedRight);
                Hotspots.Add(trays[n].LiquidDrawLeft);
                Hotspots.Add(trays[n].LiquidDrawRight);
                Hotspots.Add(trays[n].VapourDrawLeft);
                Hotspots.Add(trays[n].VapourDrawRight);
            }
        }

        public void DrawNormalHotspots(Graphics g)
        {
            Pen pen = new Pen(Hotspotcolor);

            if (Hotspots != null)
            {
                for (int i = 0; i < Hotspots.Count; i++)
                {
                    Node hs = Hotspots[i];
                    if (hs != null)
                    {
                        Rectangle rect = hs.rectangle;
                        if (hs != null)
                        {
                            if (hs.IsStripper)
                            {
                                pen.Color = hs.colour;
                                g.DrawEllipse(pen, rect.Left, rect.Top, 10, 10);
                            }
                            else
                            {
                                Brush b = Brushes.MediumSpringGreen;
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
            Brush b = Brushes.MediumSpringGreen;
            Pen pen = new Pen(Color.Black);

            foreach (DrawTray t in trays)
            {
                if (t.HotSpots != null)
                {
                    foreach (Node hs in t.HotSpots)
                    {
                        if (!hs.IsConnected)
                        {
                            hs.Owner = t;
                            g.FillRectangle(b, hs.rectangle.Centered());
                            g.DrawRectangle(pen, hs.rectangle.Centered());
                        }
                    }
                }
            }

            pen.Dispose();
        }

        public override void DrawHotSpot(Graphics g)
        {
            DrawNormalHotspots(g);
            DrawTrayHotspots(g);
        }

        public override Node HitTestHotSpot(Point p, float zoom, int offsetx, int offsety)
        {
            if (Hotspots != null)
            {
                Node hs = Hotspots.GetHotSpotFromPoint(p, zoom, offsetx, offsety);

                if (hs != null)
                    return hs;

                foreach (DrawTray t in trays)
                {
                    if (t.HotSpots != null)
                    {
                        Node h = t.HotSpots.GetHotSpotFromPoint(p, zoom, offsetx, offsety);

                        if (h != null)
                            return h;
                    }
                }
            }
            return null;
        }

        public override Node GetHotSpotFromID(Guid NodeID)
        {
            if (Hotspots != null)
            {
                Node hs = Hotspots.Search(NodeID);

                if (hs != null)
                    return hs;

                foreach (DrawTray t in trays)
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
            DrawColumnTraySection drawRectangle = new DrawColumnTraySection();
            drawRectangle.Rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        public void Calculate(object sender, EventArgs e)
        {
        }

        public bool UpdateConnections(GraphicsList gl)
        {
            return true;
        }

        int ArcHeight
        {
            get
            {
                return Convert.ToInt32(w / 2);
            }
        }

        int rhoff
        {
            get { return Convert.ToInt32(0.43 * this.rectangle.Width); }
        }

        int r
        {
            get { return (this.rectangle.Width - rhoff); }
        }

        int w
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

            try
            {
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality ;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color col2 = Color.White;
                Color col1 = Color.Gray;

                Pen pen = new Pen(Color, PenWidth);

                if (Active)
                {
                    pen.Color = this.Color;
                }
                else
                    pen.Color = Color.Gray;

                if (!Active)
                    pen = new Pen(Color.Gray, 2);

                g.DrawArc(pen, R.TopLeft().X, R.TopLeft().Y, R.Width, ArcHeight, 180, 180);
                g.DrawLine(pen, R.TopLeft().X, R.TopLeft().Y + ArcHeight / 2, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight / 2);
                g.DrawLine(pen, R.TopRight().X, R.TopRight().Y + ArcHeight / 2, R.BottomRight().X, R.BottomRight().Y - ArcHeight / 2);
                g.DrawArc(pen, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight, this.rectangle.Width, ArcHeight, 0, 180);

                Pen p;

                if (Active)
                    p = new Pen(Color.Black, 1);
                else
                    p = new Pen(Color.Gray, 1);

                LinearGradientBrush BodyBrush =
                    new LinearGradientBrush(new PointF(this.rectangle.Left, 0),
                        new PointF(right, 0), col2, col1);

                BodyBrush.SetBlendTriangularShape(1f);

                g.SmoothingMode = SmoothingMode.HighSpeed;

                GraphicsPath Body = new GraphicsPath();
                RectangleF ColumnBody = new RectangleF(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);
                Body.AddRectangle(ColumnBody);
                g.DrawPath(p, Body);

                if (Active)
                    g.FillPath(BodyBrush, Body);

                GraphicsPath Top = new GraphicsPath();
                Top.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                PathGradientBrush TopBrush = new System.Drawing.Drawing2D.PathGradientBrush(Top);
                TopBrush.CenterPoint = new PointF(this.rectangle.Left, t + ArcHeight / 2);
                TopBrush.CenterColor = col2;
                TopBrush.SurroundColors = new Color[] { col1 };
                g.DrawPath(p, Top);

                if (Active)
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

                if (Active)
                    g.FillPath(BotBrush, Bottom);

                /*GraphicsPath Drum = new  GraphicsPath();
                RectangleF D = new  RectangleF(r + w / 1.5f, t - h / 4f, w / 3f, h / 4.5f);
                Drum.AddRectangle(D);
                g.DrawPath(p, Drum);
                g.FillPath(BodyBrush, Drum);*/

                UpdateTrayLocations();

                // draw lines

                DrawTray tray;
                for (int i = 0; i < trays.Count; i++)
                {
                    tray = trays[i];
                    //int  Height = (rectangle.Height-ArcHeight) * i / notrays + ArcHeight/2;

                    //RelocateHotSpots("FEED" + i, Height);
                    //g.DrawLine(pen, traysTopLeft().X, TopLeft().Y + Height , TopRight().X, TopRight().Y + Height);
                    g.DrawLine(pen, tray.Location.X, tray.Location.Y, tray.Location.X + rectangle.Width, tray.Location.Y);
                    if (tray.Colour == Color.Green)
                    {
                        GraphicsPath traypath = new GraphicsPath();
                        traypath.AddRectangle(tray.Location);
                        PathGradientBrush TrayBrush = new System.Drawing.Drawing2D.PathGradientBrush(traypath);
                        TrayBrush.CenterPoint = new PointF(tray.Location.Left, tray.Location.Top);
                        TrayBrush.CenterColor = Color.Green;
                        BotBrush.SurroundColors = new Color[] { tray.Colour };
                        Pen pp = new Pen(Color.Green, 1);
                        g.DrawPath(pp, traypath);
                        g.FillPath(TrayBrush, traypath);
                    }
                }
            }
            catch { }
        }

        public void UpdateTrayLocations()
        {
            trays.RepositionTrays(new Rectangle(this.rectangle.Left, this.rectangle.Top + ArcHeight / 2, rectangle.Width, rectangle.Height - ArcHeight));
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

        public int Notrays
        {
            get
            {
                return notrays;
            }

            set
            {
                notrays = value;
            }
        }

        public int Section
        {
            get
            {
                return section;
            }

            set
            {
                section = value;
            }
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Point GetHandleLocation(int handleNumber)
        {
            int x, y, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width / 2;
            yCenter = rectangle.Y + rectangle.Height / 2;
            x = rectangle.X;
            y = rectangle.Y;

            switch (handleNumber)
            {
                case 1:
                    x = rectangle.X;
                    y = rectangle.Y;
                    break;

                case 2:
                    x = xCenter;
                    y = rectangle.Y;
                    break;

                case 3:
                    x = rectangle.Right;
                    y = rectangle.Y;
                    break;

                case 4:
                    x = rectangle.Right;
                    y = yCenter;
                    break;

                case 5:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;

                case 6:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;

                case 7:
                    x = rectangle.X;
                    y = rectangle.Bottom;
                    break;

                case 8:
                    x = rectangle.X;
                    y = yCenter;
                    break;
            }
            return new Point(x, y);
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    return Cursors.SizeNWSE;

                case 2:
                    return Cursors.SizeNS;

                case 3:
                    return Cursors.SizeNESW;

                case 4:
                    return Cursors.SizeWE;

                case 5:
                    return Cursors.SizeNWSE;

                case 6:
                    return Cursors.SizeNS;

                case 7:
                    return Cursors.SizeNESW;

                case 8:
                    return Cursors.SizeWE;

                default:
                    return Cursors.Default;
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

        public override bool int ersectsWith(Rectangle rectangle)
        {
            return Rectangle.int ersectsWith(rectangle);
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY, float zoom, int offsetx, int offsety)
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
        public DrawStripper(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                trays = (DrawTrayCollection)info.GetValue("Trays", typeof(DrawTrayCollection));
                isreboiled = info.GetBoolean("isreboiled");
                duty = (UOMProperty)info.GetValue("duty", typeof(UOMProperty));
                drawtray = info.GetInt32("drawtray");
                returnTray = info.GetInt32("returnTray");
            }
            catch
            {
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Trays", Trays, typeof(TrayCollection));
            info.AddValue("isreboiled", isreboiled);
            info.AddValue("duty", duty);
            info.AddValue("drawtray", drawtray);
            info.AddValue("returnTray", returnTray);
            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    internal class DrawStripperCollection : IEnumerable
    {
        List<DrawColumnTraySection> STs = new List<DrawColumnTraySection>();

        public DrawColumnTraySection this[int index]
        {
            get
            {
                return STs[index];
            }

            set
            {
                STs[index] = value;
            }
        }

        public int TotNoStripperTrays()
        {
            int temp = 0;
            for (int n = 0; n < STs.Count; n++)
            {
                temp = +STs[n].NoTrays;
            }
            return temp;
        }

        public void Add(DrawColumnTraySection pa)
        {
            STs.Add(pa);
        }

        public List<DrawColumnTraySection> STS
        {
            get
            {
                return STs;
            }
        }

        public string[] Names()
        {
            string[] res = new string[STs.Count];
            for (int n = 0; n < STs.Count; n++)
            {
                res[n] = STs[n].Name;
            }
            return res;
        }

        public int Count
        {
            get
            {
                return STs.Count;
            }
        }

        public void Clear()
        {
            STs.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return STs.GetEnumerator();
        }

        public int BottomTray(int MainSectionTrays, int Stripper)
        {
            int res = MainSectionTrays;

            for (int n = 0; n <= Stripper; n++)
            {
                res += STs[n].NoTrays;
            }
            return res;
        }

        public int BottomTrayMatrix(int MainSectionTrays, int Stripper)
        {
            int res = MainSectionTrays;

            for (int n = 0; n <= Stripper; n++)
            {
                res += STs[n].NoTrays;
            }
            return res - 1;
        }

        public int TopTray(int MainSectionTrays, int Stripper)
        {
            int res = MainSectionTrays;

            for (int n = 0; n < Stripper - 1; n++)
            {
                res += STs[n].NoTrays;
            }
            res += 1;
            return res;
        }

        public int TopTrayMatrix(int MainSectionTrays, int Stripper)
        {
            return TopTray(MainSectionTrays, Stripper) - 1;
        }

        public class StripperComparer : IComparer<DrawColumnTraySection>
        {
            public int Compare(DrawColumnTraySection x, DrawColumnTraySection y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        // If x is null and y is null, they're
                        // equal.
                        return 0;
                    }
                    else
                    {
                        // If x is null and y is not null, y
                        // is greater.
                        return -1;
                    }
                }
                else
                {
                    // If x is not null...
                    //
                    if (y == null)
                    // ...and y is null, x is greater.
                    {
                        return 1;
                    }
                    else
                    {
                        // ...and y is not null, compare the
                        // lengths of the two strings.
                        //
                        int retval = x.StripperDrawTray.CompareTo(y.StripperDrawTray);

                        if (retval != 0)
                        {
                            // If the strings are not of equal length,
                            // the longer string is greater.
                            //
                            return retval;
                        }
                        else
                        {
                            // If the strings are of equal length,
                            // sort them with ordinary string comparison.
                            //
                            return x.StripperDrawTray.CompareTo(y.StripperDrawTray);
                        }
                    }
                }
            }
        }

        public void SortByDrawTray()
        {
            StripperComparer dc = new StripperComparer();
            STS.Sort(dc);
        }

        internal void Add(TraySectionCollection strippers)
        {
            ;
        }
    }
}