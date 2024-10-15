using ModelEngine;
using Main.DrawingObjects.Streams;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Units.PortForm;
using System.Diagnostics;

namespace Units
{
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using CategoryAttribute = System.ComponentModel.CategoryAttribute;
    using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

    internal enum StreamSubType
    { Vapour, PumpAround, ColumnFeed, Normal, Recycle }

    /// <summary>
    /// Polygon graphic object
    /// </summary>
    ///
    [Serializable]

    public class DrawMaterialStream : DrawBaseStream, IDrawStream, ISerializable
    {
        private StreamMaterial stream = new();
        public SideStream Sidestream = new();
        public ConnectingStream intersectionstream = new();
        private readonly UOMDisplayList displaylist = new();
        private DrawArea drawarea;
        //private  StreamSubType subType = StreamSubType.Normal;

        public DrawMaterialStream(Node hsstart, Node hsend) : base(hsstart, hsend)
        {
            Name = "Stream" + count;
            count++;
            StreamColor = Color.Blue;
            dataNode = new Node(this, 0.5f, 0.5f, "DATA", NodeDirections.Down, HotSpotType.Signal, HotSpotOwnerType.Stream);
            isSolved = false;
            Hotspots.Add(dataNode);
        }

        public DrawMaterialStream() : base()
        {
            Name = "Stream" + count;
            count++;
            Initialize();
            StreamColor = Color.Blue;
            dataNode = new Node(this, 0.5f, 0.5f, "DATA", NodeDirections.Down, HotSpotType.Signal, HotSpotOwnerType.Stream);
            Hotspots.Add(dataNode);
        }

        public DrawMaterialStream(int x1, int y1) : base()
        {
            Name = "Stream" + count.ToString();
            count++;
            
            //for (int i = 0; i < 5; i++)
            //    segArray.Add(new SegmentV(x1, y1, y1));

            Initialize();
            StreamColor = Color.Blue;
            Stream = new StreamMaterial();
            dataNode = new Node(this, 0.5f, 0.5f, "DATA", NodeDirections.Down, HotSpotType.Signal, HotSpotOwnerType.Stream);
        }

        [Browsable(false)]
        public ThermoDynamicOptions Thermo
        {
            get
            {
                return stream.Port.cc.Thermo;
            }
        }

        public override bool UpdateAttachedModel()
        {
            dataNode.PortGuid = stream.Port.Guid;
            return true;
        }

        public DrawMaterialStream(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                EngineDrawTrayGuid = (Guid)info.GetValue("DrawTray", typeof(Guid));
                EnginereturnTrayGuid = (Guid)info.GetValue("ReturnTray", typeof(Guid));
                EngineDrawSectionGuid = (Guid)info.GetValue("DrawSection", typeof(Guid));
                EnginereturnSectionGuid = (Guid)info.GetValue("ReturnSection", typeof(Guid));
                EndDrawTrayID = (Guid)info.GetValue("EndtrayID", typeof(Guid));
                StartDrawTrayGuid = (Guid)info.GetValue("StarttrayID", typeof(Guid));
                stream = (StreamMaterial)info.GetValue("stream", typeof(StreamMaterial));
                Sidestream = (SideStream)info.GetValue("sidestream", typeof(SideStream));
                displaylist = (UOMDisplayList)info.GetValue("displayunits", typeof(UOMDisplayList));
                dataNode = (Node)info.GetValue("DatNode", typeof(Node));
            }
            catch
            {
                if (stream is null)
                    stream = new StreamMaterial();
            }

            drawName = new();
            drawName.Attachedstream = this;
            Sidestream.Name = this.Name;

            if (dataNode is null)
            {
                dataNode = new Node(this, 0.0f, 0.5f, "DATA", NodeDirections.Down, HotSpotType.Signal, HotSpotOwnerType.Stream);
                Point Loc = LineCenter(out bool? ishorizontal);
                Loc.X -= 4;
                Loc.Y -= 4;
                dataNode.SetAbsoluteLocation(Loc.X,Loc.Y);
            }

            dataNode.Owner = this;
            dataNode.NodeType = HotSpotType.Signal;
            Hotspots.Add(dataNode);
        }

        public Func<DrawArea> OnRequestParent;

        [Browsable(false)]
        public int Top
        {
            get
            {
                int Y = 0;
                foreach (Point item in pointArray)
                {
                    if (item.Y < Y)
                        Y = item.Y;
                }
                return Y;
            }
        }

        [Browsable(false)]
        public int Left
        {
            get
            {
                int L = 0;
                foreach (Point item in pointArray)
                {
                    if (item.X < L)
                        L = item.Y;
                }
                return L;
            }
        }

        [Browsable(true)]
        public Node DataNode
        { 
            get
            {
                return dataNode;
            }
        }

        internal void Relocate(int dx, int dy)
        {
            for (int i = 0; i < pointArray.Count(); i++)
            {
                Point item = pointArray[i];
                pointArray[i] = new Point(item.X += dx, item.Y += dy);
            }
        }

        protected DrawArea RequestDrawArea()
        {
            if (OnRequestParent == null)
                return null;

            return OnRequestParent();
        }

        [Browsable(false)]
        public DrawArea Owner
        {
            get
            {
                if (drawarea is null)
                    drawarea = RequestDrawArea();
                return drawarea;
            }
            set
            {
                drawarea = value;
            }
        }

        [Browsable(true), Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return stream.IsSolved;
            }
            /*set
            {
                stream.IsSolved = value;
                base.isSolved = value;
            }*/
        }

        [Browsable(false)]
        public int X0
        {
            get
            {
                return pointArray[0].X;
            }
        }

        [Browsable(false)]
        public int Y0
        {
            get
            {
                return pointArray[0].Y;
            }
        }

        [Browsable(false)]
        public int X1
        {
            get
            {
                return pointArray.Last().X;
            }
        }

        [Browsable(false)]
        public int Y1
        {
            get
            {
                return pointArray.Last().Y;
            }
        }

        public void UpdateEngineInterConnectStream(DrawColumnTraySectionCollection DrawTraySections)
        {
            switch (startDrawObject)
            {
                case DrawColumnTraySection d:
                    intersectionstream.EngineDrawSection = d.TraySection;
                    DrawTray dtray = d[StartDrawTrayGuid];
                    if (dtray != null)
                        intersectionstream.engineDrawTray = dtray.Tray;
                    break;

                case DrawColumnCondenser _:
                    TraySection section = DrawTraySections[0].TraySection;
                    if (section != null)
                    {
                        intersectionstream.EngineDrawSection = section;
                        intersectionstream.engineDrawTray = DrawTraySections[0].TopTray.Tray;
                    }
                    break;

                case DrawColumnReboiler _:
                    section = DrawTraySections[0].TraySection;
                    if (section != null)
                    {
                        intersectionstream.EngineDrawSection = section;
                        intersectionstream.engineDrawTray = DrawTraySections[0].BottomTray.Tray;
                    }
                    break;
            }

            switch (endDrawObject)
            {
                case DrawColumnTraySection d:
                    intersectionstream.EngineReturnSection = d.TraySection;
                    DrawTray dtray = d[endDrawTrayID];
                    if (dtray != null)
                        IntersectionStream.engineReturnTray = dtray.Tray;
                    break;

                case DrawColumnCondenser _:
                    TraySection section = DrawTraySections[0].TraySection;
                    if (section != null)
                    {
                        intersectionstream.EngineDrawSection = section;
                        intersectionstream.engineDrawTray = DrawTraySections[0].TopTray.Tray;
                    }
                    break;

                case DrawColumnReboiler _:
                    section = DrawTraySections[0].TraySection;
                    if (section != null)
                    {
                        intersectionstream.EngineDrawSection = section;
                        intersectionstream.engineDrawTray = DrawTraySections[0].BottomTray.Tray;
                    }
                    break;
            }

            intersectionstream.Name = this.Name;
        }

        public void UpdateSideDraws(DrawColumnTraySectionCollection DrawTraySections)  // side product streams
        {
            switch (startDrawObject)
            {
                case DrawColumnTraySection d:
                    Sidestream.EngineDrawSection = d.TraySection;
                    Sidestream.EngineDrawTray = StartDrawTray.Tray;
                    break;

                case DrawTray d:
                    Sidestream.EngineDrawSection = d.Owner.TraySection;
                    Sidestream.EngineDrawTray = d.Tray;
                    break;

                case DrawColumnCondenser:
                    TraySection section = DrawTraySections[0].TraySection;
                    if (section != null)
                    {
                        Sidestream.EngineDrawSection = section;
                        Sidestream.EngineDrawTray = Sidestream.EngineDrawSection.TopTray;
                    }
                    break;

                case DrawColumnReboiler:
                    section = DrawTraySections[0].TraySection;
                    if (section != null)
                    {
                        Sidestream.EngineDrawSection = section;
                        Sidestream.EngineDrawTray = Sidestream.EngineDrawSection.BottomTray;
                    }
                    break;
            }

            Sidestream.Name = this.Name;
        }

        public override void UpdateDrawObjects(GraphicsList gl)
        {
            StartDrawObject = gl.GetObject(StartDrawObjectGuid);

            if (StartDrawObject != null)
            {
                StartNode = StartDrawObject.GetNode(StartNodeGuid);

                if (StartNode is null)
                    return;

                switch (StartNode.Owner)
                {
                    case DrawColumnTraySection dcts:
                        if (StartNode.NodeType == HotSpotType.TrayNetVapour)
                        {
                            StartDrawTray = dcts.TopTray;
                            StartDrawTrayGuid = StartDrawTray.Guid;
                            EngineDrawTrayGuid = StartDrawTray.Tray.Guid;
                        }
                        else if (StartNode.NodeType == HotSpotType.TrayNetLiquid)
                        {
                            StartDrawTray = dcts.BottomTray;
                            StartDrawTrayGuid = StartDrawTray.Guid;
                            EngineDrawTrayGuid = StartDrawTray.Tray.Guid;
                        }
                        break;

                    case DrawTray drawTray:
                        startDrawObjectID = drawTray.Owner.Guid;
                        EngineDrawSectionGuid = drawTray.Owner.TraySection.Guid;
                        StartDrawTray = drawTray;
                        StartDrawTrayGuid = drawTray.Guid;
                        EngineDrawTrayGuid = drawTray.Tray.Guid;
                        break;

                    case DrawColumnCondenser dcc:
                        if (dcc.section is not null)
                        {
                            EngineDrawSectionGuid = dcc.section.Guid;
                            StartDrawTray = dcc.section.DrawTrays[0];
                            EngineDrawTrayGuid = dcc.section.TraySection.Trays[0].Guid;
                            StartDrawTrayGuid = StartDrawTray.Guid;
                        }
                        break;

                    case DrawColumnReboiler dcr:
                        if (dcr.section is not null)
                        {
                            EngineDrawSectionGuid = dcr.section.Guid;
                            StartDrawTray = dcr.section.DrawTrays.Last();
                            EngineDrawTrayGuid = dcr.section.TraySection.Trays[0].Guid;
                            StartDrawTrayGuid = StartDrawTray.Guid;
                        }
                        break;
                }
            }

            EndDrawObject = gl.GetObject(EndDrawObjectGuid);
            if (EndDrawObject != null)
            {
                EndNode = EndDrawObject.GetNode(EndNodeGuid);
                if (EndNode is null)
                    return;

                switch (EndNode.Owner)
                {
                    case DrawColumnTraySection dcts:
                        if (EndNode.NodeType == HotSpotType.TrayNetVapour)
                        {
                            EndDrawTray = dcts.TopTray;
                            endDrawTrayID = EndDrawTray.Guid;
                            EngineDrawTrayGuid = EndDrawTray.Tray.Guid;
                        }
                        else if (EndNode.NodeType == HotSpotType.TrayNetLiquid)
                        {
                            EndDrawTray = dcts.BottomTray;
                            endDrawTrayID = EndDrawTray.Guid;
                            EngineDrawTrayGuid = EndDrawTray.Tray.Guid;
                        }
                        break;

                    case DrawTray drawTray:
                        if (EndDrawObject is DrawColumnTraySection section)
                        {
                            EngineDrawSectionGuid = section.TraySection.Guid;
                            EndDrawTray = drawTray;
                            EnginereturnTrayGuid = drawTray.Tray.Guid;
                            endDrawTrayID = drawTray.Guid;
                        }
                        break;
                }
            }
        }

        [Browsable(false)]
        public ConnectingStream IntersectionStream { get => intersectionstream; set => intersectionstream = value; }

        /// <summary>
        /// Update connections to draw objects
        /// </summary>
        /// <param name="gl"></param>

        private static int count = 0;

        [Category("Display Properties"), Description("Stream Name")]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                stream.Name = value;
            }
        }

        public override void CreateFlowsheetUOModel()
        {
            if (Flowsheet != null)
                this.Flowsheet.Add(Stream);

            Stream.Name = this.Name;
        }

        [Category("NODES"), Description("Stream Port")]
        public Port_Material Port
        {
            get
            {
                if (stream != null)
                    return stream.Port;
                else
                    return null;
            }
        }

        
        public override void Draw(Graphics g)
        {
            //this.IsSolved = Port.IsFullyDefined;

            //Debug.Print(this.IsSolved.ToString());

            if (stream.IsSolved)
            {
                this.StreamColor = Color.Blue;
            }
            else
            {
                this.StreamColor = Color.Red;
            }

            base.Draw(g);
        }

        public override void ResetCalculateFlag()
        {
            base.ResetCalculateFlag();
        }

        private bool isrecycle = false;

        [Browsable(false)]
        public bool IsRecycle
        {
            get { return isrecycle; }
            set { isrecycle = value; }
        }

        /*[CategoryAttribute("molarenthalpy"),
        DescriptionAttribute("molarenthalpy in kJ/kg")]
        public  double  LiquidEnthalpy
        {
            get
            {
                double  res = oil.LiqEnthalpy(oil.T, oil.P, enumMassOrMolar.Mass);
                liquidenthalpy = res;
                return   Math.Round(liquidenthalpy, 2);
            }
            set { liquidenthalpy = value; }
        }*/

        /*[CategoryAttribute("MolarEntropy"),
         DescriptionAttribute("molarenthalpy in kJ/kg")]
        public  double  LiquidEntropy
        {
            get
            {
                double  res = oil.LiqEntropy(oil.T, oil.P,enumMassOrMolar.Mass);
                liqentropy = res;
                return   Math.Round(liqentropy, 2);
            }
            set { liqentropy = value; }
        }

        [CategoryAttribute("MolarEntropy"),
        DescriptionAttribute("molarenthalpy in kJ/kg")]
        public  double  VapEntropy
        {
            get
            {
                double  res = oil.VapEntropy(oil.T, oil.P,enumMassOrMolar.Mass);
                vapentropy = res;
                return   Math.Round(vapentropy, 2);
            }
            set { vapentropy = value; }
        }*/

        /* [CategoryAttribute("molarenthalpy"),
         DescriptionAttribute("molarenthalpy in kJ/kg")]
         public  double  molarenthalpy
         {
             get
             {
                 double  res = oil.molarenthalpy;
                 return   Math.Round(res, 2);
             }
             set
             {
                 oil.SetEnthalpy(value, 25);
                 spectypes.enthtype = SpecTypes.EnthType.Passed;
             }
         }*/

        /* [CategoryAttribute("Liquid Molar molarenthalpy"),
 DescriptionAttribute("molarenthalpy in kJ/kg.mole")]
         public  double  LiquidMolarEnthalpy
         {
             get
             {
                 double  res = oil.LiqEnthalpy(25, this.press,enumMassOrMolar.Molar);
                 return   res;
             }
         }

         [CategoryAttribute("Vapour Molar molarenthalpy"),
 DescriptionAttribute("molarenthalpy in kJ/kg.mole")]
         public  double  VapourMolarEnthalpy
         {
             get
             {
                 double  res = oil.VapEnthalpy(25, this.press, enumMassOrMolar.Molar);
                 return   Math.Round(res, 2);
             }
         }*/

        /*  [CategoryAttribute("molarenthalpy"),
              DescriptionAttribute("Mass Heat Capacity in kJ/kg/C")]
          public  double  MassHeatCapacity
          {
              get
              {
                  double  res = oil.MassHeatCapacity;
                  return   Math.Round(res, 2);
              }
              set
              {
                  oil.SetEnthalpy(value, 25);
              }
          }

          [CategoryAttribute("MolarEntropy"),
          DescriptionAttribute("molarenthalpy in kJ/kg/C")]
          public  double  MolarEntropy
          {
              get
              {
                  double  res = oil.MolarEntropy;
                  return   Math.Round(res, 2);
              }
              //set { entropy = value; }
          }*/

        public override HitType HitTest(Point point)
        {

            Node hs = TestForDataNode(point);

            if (hs != null)
                return HitType.DataNode;


            if (Selected)  // get handle
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    Rectangle r = GetHandleRectangle(i);

                    using Matrix matrix = new();
                    //matrix.Scale(zoom, zoom, MatrixOrder.Append);
                    //matrix.Translate(offsetx, offsety, MatrixOrder.Append);

                    using GraphicsPath path = new();
                    path.AddRectangle(r);
                    path.Transform(matrix);
                    if (path.IsVisible(point))
                        return HitType.StreamHandle;
                }
            }

            if (PointInObject(point))
                return HitType.Stream;

            return HitType.None;
        }


        public Node TestForDataNode(Point p)
        {

            using GraphicsPath gp = new();
            Matrix m = new();

            Rectangle r = dataNode.Centered();
            r.Inflate(4,4);

            gp.AddRectangle(r);

            if (gp.IsVisible(p))
                return dataNode;

            return null;
        }

        public override int GetHandleNumber(Point point)
        {
            if (Selected)  // get handle
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    Rectangle r = GetHandleRectangle(i);

                    using Matrix matrix = new();
                    //matrix.Scale(zoom, zoom, MatrixOrder.Append);
                    //matrix.Translate(offsetx, offsety, MatrixOrder.Append);

                    using GraphicsPath path = new();
                    path.AddRectangle(r);
                    path.Transform(matrix);
                    if (path.IsVisible(point))
                        return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected override void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);

            if (pointArray.Count() == 0)
                return;
            AreaPath.AddLines(pointArray.ToArray());

            try
            {
                AreaPath.Widen(AreaPen);
            }
            catch
            {
                for (int n = 0; n < pointArray.Count() - 1; n++)
                {
                    Point p = pointArray[n + 1];
                    if (pointArray[n].X == pointArray[n + 1].X)
                    {
                        p.X++;
                        pointArray[n + 1] = p;
                    }
                }
            }

            // Create region from the path
            AreaRegion = new Region(AreaPath);
            AreaPen.Dispose();
            AreaPath.Dispose();
        }

        [Category("Stream Properties"), Description("Atm")]
        public double VapourPressure
        {
            get
            {
                double res;
                if (Port != null)
                    res = ThermodynamicsClass.CalcBubblePointP(Port.cc, Port.T, Port.cc.Thermo);
                else
                    res = double.NaN;
                return Math.Round(res, 2);
            }
        }

        [Category("Stream Properties"), Description("Fraction Vaporised")]
        public double Quality
        {
            get
            {
                if (Port != null)
                    return Port.Q_;
                else return double.NaN;
            }
        }

        [Category("Stream Properties"), Description("Std Specific Gravity")]
        public double SG
        {
            get
            {
                if (Port != null)
                    return Port.cc.SG_Calc();
                else
                    return double.NaN;
            }
        }

        [Category("Stream Properties"), Description("Act Liquid Density")]
        public double ActLiqDensity
        {
            get
            {
                if (Port != null)
                    return Port.ActLiqDensity();
                else
                    return double.NaN;
            }
        }

        [Category("Stream Properties"), Description("Sulfur Wt%")]
        public double Sulfur
        {
            get
            {
                if (Port != null)
                    return Port.Sulfur;
                else
                    return double.NaN;
            }
        }

        [Category("Stream Properties"), Description("API Gravity")]
        public double API
        {
            get
            {
                if (Port != null)
                    return Port.API;
                else return double.NaN;
            }
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawMaterialStream drawStream = new();
            drawStream.segArray = this.segArray.Clone();
            
            FillDrawObjectFields(drawStream);
            return drawStream;
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public override Point GetHandleLocation(int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > pointArray.Count())
                handleNumber = pointArray.Count();

            return ((Point)pointArray[handleNumber - 1]);
        }

        [Browsable(false)]
        public bool IsPumparoud
        {
            get
            {
                return isPumparoud;
            }

            set
            {
                isPumparoud = value;
            }
        }

        [Browsable(false)]
        public bool IsColumnFeed
        {
            get
            {
                return isColumnFeed;
            }

            set
            {
                isColumnFeed = value;
            }
        }

        [Browsable(false)]
        public int SectionNumber { get; set; } = 0;

        [Browsable(false)]
        public bool IsVapourDraw
        {
            get
            {
                return isVapourDraw;
            }
            set
            {
                isVapourDraw = value;
            }
        }

        [Browsable(false)]
        public double FlowEstimate
        {
            get
            {
                return intersectionstream.FlowEstimate;
            }
            set
            {
                intersectionstream.FlowEstimate = value;
            }
        }

        [Category("Calculation"), Description("IsDirty")]
        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return stream.IsDirty;
            }
            set
            {
                stream.IsDirty = value;
            }
        }

        public StreamMaterial Stream { get => stream; set => stream = value; }

        [Browsable(false)]
        public Components Components
        {
            get
            {
                if (Port != null)
                    return Port.cc;
                else
                    return null;
            }
        }

        [Browsable(false)]
        public List<BaseComp> ComponentList
        {
            get
            {
                if (Port != null)
                    return Port.cc.ComponentList;
                else
                    return null;
            }
        }


        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            if (stream != null)
            {
                PortPropertyForm2 SD = new(Stream.Port, displaylist);
                SD.drawMaterialStream = this;
                SD.ValueChanged += SD_ValueChanged;
                SD.Show();
                this.stream.IsDirty = SD.IsDirty;
            }
        }

        private void SD_ValueChanged(object sender, EventArgs e)
        {
            RaiseValueChangedEvent(e);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            //Column Guids
            info.AddValue("DrawTray", engineDrawTray);
            info.AddValue("ReturnTray", enginereturnTray);
            info.AddValue("DrawSection", EngineDrawSectionGuid);
            info.AddValue("ReturnSection", EnginereturnSectionGuid);
            info.AddValue("EndtrayID", EndDrawTrayID);
            info.AddValue("StarttrayID", StartDrawTrayGuid);

            info.AddValue("stream", stream, typeof(ModelEngine.StreamMaterial));
            info.AddValue("sidestream", Sidestream);
            info.AddValue("displayunits", displaylist);
            info.AddValue("DatNode", dataNode);
        }

        internal Rectangle DataNodeLocationUpdateOLd()
        {
            int pointcount = pointArray.Count();

            Point p = this.LineCenter(out bool? IsHoriz);


            switch (IsHoriz)
            {
                case true:
                    dataNode.LineDirection = NodeDirections.Down;
                    break;
                case false:
                    dataNode.LineDirection = NodeDirections.Right;
                    break;
                case null:
                    break;
            }

            dataNode.SetAbsolutePosition = new Point(p.X-4, p.Y-4);

            return dataNode.Absolute;
        }

        internal Rectangle DataNodeLocationUpdate()
        {
            int pointcount = (int)(segArray.Count / 2.0);

            BaseSegment seg = segArray[pointcount];

            Point p=new();

            switch (seg)
            {
                case SegmentV segv:
                    {
                        int X = segv.X;
                        int y = (int)((segv.Y1 + segv.Y2) / 2.0);
                        p = new Point(X, y);
                    }
                    break;
                case SegmentH segh:
                    {
                        int y = segh.Y;
                        int x = (int)((segh.X1 + segh.X2) / 2.0);
                        p = new Point(x, y);
                    }
                    break;
            }

            dataNode.SetAbsolutePosition = p;

            return dataNode.Absolute;
        }
    }
}