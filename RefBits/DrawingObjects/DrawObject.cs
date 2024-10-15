using Extensions;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    public enum HotSpotDisplayType
    { All, Inputs, Outputs, Energy, Signal }

    /// <summary>
    /// Base class  for all draw objects
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class DrawObject : IDrawObject, IEqualityComparer<DrawObject>, IEquatable<DrawObject>
    {
        private Guid guid = Guid.NewGuid();

        private static Color lastUsedColor = Color.Black;
        private static int lastUsedPenWidth = 1;
        private string name;

        //static int  counter = 0;
        private bool selected;

        internal Color color, hotspotcolor;
        private int penWidth;
        private bool displayhotspots = false;
        private NodeCollection hotspots = new();  // Coordinates stored should be center point s
        private int calcorder = 0;
        public List<DrawObject> OriginObjects = new();
        public List<DrawObject> DestObjects = new();
        private DrawArea drawarea;
        public bool IsLinkedtointerface = false;
        public HotSpotDisplayType hotspotdisplaytype = HotSpotDisplayType.All;

        public event ObjectValueChangedEventHandler ObjectValueChangedEvent;

        public delegate void ObjectValueChangedEventHandler(object sender, EventArgs e);

        public virtual void RaiseValueChangedEvent(EventArgs e)
        {
            ObjectValueChangedEvent?.Invoke(this, e);
        }

        public List<StreamMaterial> GetStreamListFromNodes()
        {
            List<StreamMaterial> streams = new();

            foreach (Node n in hotspots)
            {
                DrawMaterialStream DS = n.GetAttachedStream(drawarea.GraphicsList);
                if (DS != null)
                    streams.Add(n.GetAttachedStream(drawarea.GraphicsList).Stream);
            }

            return streams;
        }

        public List<StreamMaterial> GetProductStreamListFromNodes()
        {
            List<StreamMaterial> streams = new();

            foreach (Node n in hotspots)
            {
                if (!n.IsInput)
                {
                    DrawMaterialStream DS = n.GetAttachedStream(drawarea.GraphicsList);
                    if (DS != null)
                        streams.Add(n.GetAttachedStream(drawarea.GraphicsList).Stream);
                }
            }

            return streams;
        }

        public override string ToString()
        {
            return Name;
        }

        [Browsable(false)]
        public FlowSheet Flowsheet
        {
            get
            {
                return GlobalModel.Flowsheet;
            }
        }

        public virtual void PostSolve()
        {
        }

        [Browsable(false)]
        public UnitOperation AttachedModel
        {
            get
            {
                UnitOperation uo;

                uo = null;
                switch (this)
                {
                    case DrawColumn dc:
                        uo = dc.SubFlowSheet;
                        break;

                    case DrawMaterialStream a:
                        uo = a.Stream;
                        break;

                    case DrawEnergyStream a:
                        uo = a.Stream;
                        break;

                    case DrawSignalStream a:
                        uo = a.Stream;
                        break;

                    case DrawHeater a:
                        uo = a.Heater;
                        break;

                    case DrawCooler a:
                        uo = a.Cooler;
                        break;

                    case DrawPump a:
                        uo = a.Pump;
                        break;

                    case DrawCompressor a:
                        uo = a.compressor;
                        break;

                    case DrawExpander a:
                        uo = a.expander;
                        break;

                    case DrawValve a:
                        uo = a.valve;
                        break;

                    case DrawDivider a:
                        uo = a.divider;
                        break;

                    case DrawMixer a:
                        uo = a.mixer;
                        break;

                    case DrawSeparator a:
                        uo = a.simpleFlash;
                        break;

                    case DrawExchanger a:
                        uo = a.Exchanger;
                        break;

                    case DrawGenericAssay a:
                        uo = a.assay;
                        break;

                    case DrawName _:
                        break;

                    case DrawRecycle a:
                        uo = a.recycle;
                        break;

                    case DrawCompSplitter a:
                        uo = a.compsplitter;
                        break;

                    case DrawGibbs a:
                        uo = a.gibbs;
                        break;

                    case DrawCaseStudy a:
                        uo = a.caseStudy;
                        break;

                    case DrawLLEColumn a:
                        uo = a.SubFlowSheet;
                        break;

                    case DrawSet a:
                        uo = a.set;
                        break;

                    case DrawAdjust a:
                        uo = a.adjust;
                        break;

                    case DrawAssayCutter a:
                        uo = a.Cutter;
                        break;

                    default:
                        break;
                }
                return uo;
            }
        }

        public virtual bool UpdateAttachedModel()
        {
            return true;
        }

        public virtual Dictionary<Guid, string> StreamNames()
        {
            //List<string> names = new  List<string>();
            Dictionary<Guid, string> names = new();

            foreach (Node item in Hotspots)
            {
                if (item.Guid != Guid.Empty && !names.ContainsKey(item.PortGuid))
                    names.Add(item.PortGuid, item.AttachedStreamName);
            }
            return names;
        }

        /// <summary>
        /// Test whether point  is inside of the object
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>
        public virtual Node HitTestHotSpot(Point point)
        {
            return null;
        }

        private Func<object> onRequestParentObject;

        public object GetParent()
        {
            object parent = RequestParentObject();
            return parent;
        }

        protected object RequestParentObject()
        {
            if (OnRequestParentObject == null)
                return null;

            return OnRequestParentObject();
        }

        public virtual void ResetCalculateFlag()
        {
            //Hotspots.Reset();
            //isSolved = true;
        }

        private bool isfeedobject = false;

        internal virtual Node GetNode(Guid startHSID)
        {
            for (int i = 0; i < Hotspots.Count; i++)
            {
                if (Hotspots[i].Guid == startHSID)
                    return Hotspots[i];
            }

            return null;
        }

        public bool IsFeedObject
        {
            get { return isfeedobject; }
            set { isfeedobject = value; }
        }

        public virtual Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        [Browsable(false)]
        public Color Hotspotcolor
        {
            get { return hotspotcolor; }
            set { hotspotcolor = value; }
        }

        [Browsable(false)]
        public int CalcOrder
        {
            get { return calcorder; }
            set { calcorder = value; }
        }

        [Browsable(true), CategoryAttribute("NODES")]
        public virtual NodeCollection Hotspots
        {
            get { return hotspots; }
            set { hotspots = value; }
        }

        [Browsable(false)]
        public bool Displayhotspots
        {
            get { return displayhotspots; }
            set { displayhotspots = value; }
        }

        [CategoryAttribute("Display Properties"), Description("Stream Name")]
        public virtual string Name
        {
            get
            {
                if (name == null)
                    return "free";
                else
                    return name;
            }
            set
            {
                name = value;
                if (AttachedModel != null)
                    AttachedModel.Name = name;
            }
        }

        public virtual Guid ModelGuid
        {
            get
            {
                return Guid.Empty;
            }
        }

        public virtual void CreateFlowsheetUOModel()
        {
        }

        public DrawObject()
        {
            guid = Guid.NewGuid();
        }

        /// <summary>
        /// Selection flag
        /// </summary>
        [Browsable(true)]
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        /// <summary>
        /// Color
        /// </summary>
        ///
        [CategoryAttribute("Display Properties"),
        Description("")]
        public Color StreamColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Pen width
        /// </summary>
        ///
        [CategoryAttribute("Display Properties"), Description("")]
        public int PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }

        /// <summary>
        /// Number of handles
        /// </summary>
        [Browsable(false)]
        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        [Browsable(false)]
        public static Color LastUsedColor
        {
            get
            {
                return lastUsedColor;
            }
            set
            {
                lastUsedColor = value;
            }
        }

        [Browsable(false)]
        public static int LastUsedPenWidth
        {
            get
            {
                return lastUsedPenWidth;
            }
            set
            {
                lastUsedPenWidth = value;
            }
        }

        [Browsable(false)]
        public virtual Rectangle Rectangle
        {
            get;
            set;
        }

        public DrawArea DrawArea { get => drawarea; set => drawarea = value; }

        [Browsable(false)]
        public virtual Node GetHotSpotFromHandleNumber(int handlenumber)
        {
            return new Node();
        }

        [Browsable(false)]
        public virtual Rectangle GetHotSpotRectangle(Guid NodeID)
        {
            return new Rectangle(100, 100, 10, 10);
        }

        [Browsable(false)]
        public virtual Point GetHotSpotCenter(Guid NodeID)
        {
            return new Point(100, 100);
        }

        [Browsable(false)]
        public virtual PointF GetHotSpotRelativeCenter(Guid NodeID)

        {
            return new PointF(1, 1);
        }

        /// <summary>
        /// Clone this instance.
        /// </summary>
        public abstract DrawObject Clone();

        /// <summary>
        /// Draw object
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
        }

        /// <summary>
        /// Draw tracker for selected object
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

            SolidBrush brush = new(Color.Black);

            if (this is DrawRectangle)
                for (int i = 1; i <= HandleCount; i++)
                    g.FillRectangle(brush, GetRotatedHandleRectangle(i));
            else
                for (int i = 1; i <= HandleCount; i++)
                    g.FillRectangle(brush, GetHandleRectangle(i));

            brush.Dispose();
        }

        [Browsable(true), CategoryAttribute("NODES")]
        public List<Port> Ports
        {
            get
            {
                if (AttachedModel != null && AttachedModel.Ports != null)
                    return AttachedModel.Ports.AllPorts;
                else
                    return null;
            }
        }

        [Browsable(true), CategoryAttribute("NODES")]
        public List<Port> ConnectedPorts
        {
            get
            {
                List<Port> connectports = new();
                if (Ports != null)
                    foreach (Port p in Ports)
                    {
                        if (p.IsConnected)
                            connectports.Add(p.StreamPort);
                    }

                return connectports;
            }
        }

        public Func<object> OnRequestParentObject { get => onRequestParentObject; set => onRequestParentObject = value; }

        public class KeyMessageFilter : IMessageFilter
        {
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_KEYUP = 0x0101;
            private bool m_keyPressed = false;

            private Dictionary<Keys, bool> m_keyTable = new();

            public Dictionary<Keys, bool> KeyTable
            {
                get { return m_keyTable; }
                private set { m_keyTable = value; }
            }

            public bool IsKeyPressed()
            {
                return m_keyPressed;
            }

            public bool IsKeyPressed(Keys k)
            {
                if (KeyTable.TryGetValue(k, out bool pressed))
                {
                    return pressed;
                }
                return false;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_KEYDOWN)
                {
                    KeyTable[(Keys)m.WParam] = true;

                    m_keyPressed = true;
                }

                if (m.Msg == WM_KEYUP)
                {
                    KeyTable[(Keys)m.WParam] = false;

                    m_keyPressed = false;
                }

                return false;
            }
        }

        public virtual void DrawHotSpot(Graphics g)
        {
            using Pen pen = new(hotspotcolor);
            Brush b = Brushes.MediumSpringGreen;

            if (this is DrawMaterialStream dms && dms.displayhotspots == true)
            {
                Rectangle DataHSRect = dms.DataNodeLocationUpdate();
                if (!dms.dataNode.IsConnected)
                {
                    g.FillEllipse(Brushes.DarkRed, DataHSRect);
                    g.DrawEllipse(pen, DataHSRect);
                }
                return;
            }

            for (int i = 0; i < hotspots.Count; i++)
            {
                Node hs = hotspots[i];
                hs.HotSpotDisplayType = HotSpotDisplayType.All;

                if (hs.IsConnected)
                    continue;

                Rectangle HSrect = hs.Absolute.Centered();
                if (drawarea is not null)
                    HSrect.Inflate(Convert.ToInt32(1.0 / drawarea.ScaleDraw / drawarea.ScaleDraw), Convert.ToInt32(1.0 / drawarea.ScaleDraw / drawarea.ScaleDraw));
                //HSrect.Width = 20;
                //HSrect.Height = 20;

                if (hs != null && !hs.IsConnected)
                {
                    switch (hs.NodeType)
                    {
                        case HotSpotType.TrayNetVapour:
                        case HotSpotType.TrayNetLiquid:
                        case HotSpotType.BottomTrayLiquid:
                        case HotSpotType.Stream:
                            pen.Color = hs.Color;
                            g.FillRectangle(Brushes.White, HSrect);
                            g.DrawRectangle(pen, HSrect);
                            break;

                        case HotSpotType.InternalExport:
                            pen.Color = hs.Color;
                            g.FillRectangle(Brushes.Yellow, HSrect);
                            g.DrawRectangle(pen, HSrect);
                            break;

                        case HotSpotType.Water:
                            if (hotspotdisplaytype == HotSpotDisplayType.All
                                || hotspotdisplaytype == HotSpotDisplayType.Outputs)
                            {
                                pen.Color = hs.Color;
                                g.FillRectangle(Brushes.Blue, HSrect);
                                g.DrawRectangle(pen, HSrect);
                            }

                            break;

                        case HotSpotType.VapourDraw:
                        case HotSpotType.LiquidDraw:
                        case HotSpotType.LiquidDrawLeft:
                            if (hotspotdisplaytype == HotSpotDisplayType.All
                                || hotspotdisplaytype == HotSpotDisplayType.Outputs)
                            {
                                hs.HotSpotDisplayType = hotspotdisplaytype;
                                pen.Color = hs.Color;
                                g.FillRectangle(Brushes.White, HSrect);
                                g.DrawRectangle(pen, HSrect);
                            }

                            break;

                        case HotSpotType.EnergyIn:
                        case HotSpotType.EnergyOut:
                            if (hotspotdisplaytype == HotSpotDisplayType.All
                                || hotspotdisplaytype == HotSpotDisplayType.Inputs
                                || hotspotdisplaytype == HotSpotDisplayType.Outputs)
                            {
                                pen.Color = hs.Color;
                                g.FillRectangle(Brushes.Red, HSrect);
                                g.DrawRectangle(pen, HSrect);
                            }
                            break;

                        case HotSpotType.SignalIn:
                        case HotSpotType.SignalOut:
                            if (hotspotdisplaytype == HotSpotDisplayType.All
                               || hotspotdisplaytype == HotSpotDisplayType.Inputs
                                || hotspotdisplaytype == HotSpotDisplayType.Outputs)
                            {
                                pen.Color = hs.Color;
                                g.FillRectangle(Brushes.Red, HSrect);
                                g.DrawRectangle(pen, HSrect);
                            }
                            break;

                        case HotSpotType.Signal:
                            if (hotspotdisplaytype == HotSpotDisplayType.All
                                || hotspotdisplaytype == HotSpotDisplayType.Inputs
                                || hotspotdisplaytype == HotSpotDisplayType.Outputs)
                            {
                                pen.Color = hs.Color;
                                g.FillRectangle(Brushes.Red, HSrect);
                                g.DrawRectangle(pen, HSrect);
                            }
                            break;

                        case HotSpotType.Feed:
                        case HotSpotType.FeedRight:
                            if (hotspotdisplaytype == HotSpotDisplayType.All
                                || hotspotdisplaytype == HotSpotDisplayType.Inputs)
                            {
                                pen.Color = hs.Color;
                                g.FillRectangle(b, HSrect);
                                g.DrawRectangle(pen, HSrect);
                            }
                            break;

                        default:
                            break;
                    }
                }
                //hs.colour = Color.Black;
            }
            //b.Dispose();
            pen.Dispose();
            hotspotdisplaytype = HotSpotDisplayType.All;
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual Point GetHandleLocation(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual Point GetRotatedHandleLocation(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual int GetHandleNumber(Point handleNumber)
        {
            return 0;
        }

        public virtual FrontBack GetHandlePosition(Point point)
        {
            return FrontBack.None;
        }

        public virtual void MoveHandleTo(Point point, FrontBack handle)
        {
        }

        /// <summary>
        /// Get handle point  by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual int GetRotatedHandleNumber(Point handleNumber)
        {
            return 0;
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandleLocation(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual Rectangle GetRotatedHandleRectangle(int handleNumber)
        {
            Point point = GetRotatedHandleLocation(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 5, 5);
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <return  s></return  s>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
        /// Test whether object int ersects with rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <return  s></return  s>
        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void Move(int deltaX, int deltaY)
        {
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void RotatedMove(int deltaX, int deltaY)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandle(int dx, int dy, int handleNumber)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveRotatedHandle(int dx, int dy, int handleNumber)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point "></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveRotatedHandleTo(Point point, int handleNumber)
        {
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public virtual void Dump()
        {
            Trace.WriteLine(this.GetType().Name);
            Trace.WriteLine("Selected = " +
                selected.ToString(CultureInfo.InvariantCulture)
                + " ID = " + Guid.ToString());
        }

        /// <summary>
        /// Normalize object.
        /// Call this function in the end of object resizing.
        /// </summary>
        public virtual void Normalize()
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Colour", StreamColor.ToArgb());
            info.AddValue("PenWidth", PenWidth);
            info.AddValue("ObjectName", name);
            info.AddValue("guid", guid);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        ///
        public DrawObject(SerializationInfo info, StreamingContext context)
        {
            try
            {
                int n = info.GetInt32("Colour");
                StreamColor = Color.FromArgb(n);
                PenWidth = info.GetInt32("PenWidth");
                name = info.GetString("ObjectName");
                guid = (Guid)info.GetValue("guid", typeof(Guid));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected void Initialize()
        {
            color = lastUsedColor;
            penWidth = LastUsedPenWidth;
        }

        /// <summary>
        /// Test whether point  is inside of the object
        /// </summary>
        /// <param name="point "></param>
        /// <return  s></return  s>
        public virtual HitType HitTest(Point point)
        {
            return HitType.None;
        }

        /// <summary>
        /// Copy fields from this instance to cloned instance drawObject.
        /// Called from Clone functions of derived class es.
        /// </summary>
        protected void FillDrawObjectFields(DrawObject drawObject)
        {
            drawObject.selected = this.selected;
            drawObject.color = this.color;
            drawObject.penWidth = this.penWidth;
            drawObject.Guid = this.Guid;
        }

        internal virtual void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            MessageBox.Show("DBLCLick Object");
        }

        public bool Equals(DrawObject x, DrawObject y)
        {
            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] DrawObject obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(DrawObject other)
        {
            return this.guid == other.guid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DrawObject);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}