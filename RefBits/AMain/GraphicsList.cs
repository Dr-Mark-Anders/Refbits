using Extensions;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Units
{
    using DrawList = List<DrawObject>;
    using Point = System.Drawing.Point;
    using Rectangle = System.Drawing.Rectangle;

    /// <summary>
    /// List of graphic objects
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), Serializable]
    public class GraphicsList : ISerializable, IEnumerable
    {
        public event ValueErrorEventHandler errorChangedEvent;

        public delegate void ValueErrorEventHandler(object sender, EventArgs e);

        public event GraphicsValueChangedEvent graphicsValueChangedEvent;

        public delegate void GraphicsValueChangedEvent(object sender, EventArgs e);

        protected virtual void RaiseValueChangedEvent(object sender, EventArgs e)
        {
            graphicsValueChangedEvent?.Invoke(sender, e);
        }

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            graphicsList.Insert(0, obj);
            if (obj is DrawMaterialStream stream)
            {
                DrawArea.StaticStreamList.Add(stream);
                stream.errorChangedEvent += GraphicsList_errorChangedEvent;
            }
            else
            {
                obj.OnRequestParentObject += new Func<object>(delegate { return this; });
                obj.ObjectValueChangedEvent += RaiseValueChangedEvent;
            }
        }

        public void ResetEvents()
        {
            foreach (DrawObject dobj in graphicsList)
            {
                dobj.ObjectValueChangedEvent -= RaiseValueChangedEvent;
                dobj.ObjectValueChangedEvent += RaiseValueChangedEvent;
            }
        }

        private ThermoDynamicOptions thermooptions = new();
        private Components components = new();
        private float scale = 1;

        public GraphicsList()
        {
            graphicsList = new DrawList();
        }

        public ThermoDynamicOptions Thermo
        {
            get { return thermooptions; }
            set { thermooptions = value; }
        }

        public DrawObject this[string name]
        {
            get
            {
                foreach (var doObj in graphicsList)
                {
                    if (doObj.Name == name)
                        return doObj;
                }
                return null;
            }
        }

        private int offsetY = 0;
        private int offsetx = 0;
        private double y = 0;
        private double x = 0;

        [Browsable(true), Category("Appearance")]
        public int Offsetx { get => offsetx; set => offsetx = value; }

        [Browsable(true), Category("Appearance")]
        public int Offsety { get => offsetY; set => offsetY = value; }

        [Browsable(true), Category("Appearance")]
        public double X { get => x; set => x = value; }

        [Browsable(true), Category("Appearance")]
        public double Y { get => y; set => y = value; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(entryCount, graphicsList.Count);
            info.AddValue("graphicslist", graphicsList, typeof(DrawList));
            info.AddValue("ThermoOptions", Thermo, typeof(ThermoDynamicOptions));
            info.AddValue("CalcMethod", this.CalcType);
            info.AddValue("Components", this.Components);
            info.AddValue("ScaleDraw", scale);
            info.AddValue("Offsetx", Offsetx);
            info.AddValue("Offsety", Offsety);
            info.AddValue("x", x);
            info.AddValue("y", y);
        }

        public GraphicsList CloneGraphics()
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(stream, this);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                stream.Seek(0, SeekOrigin.Begin);
                return (GraphicsList)formatter.Deserialize(stream);
            }
        }

        protected GraphicsList(SerializationInfo info, StreamingContext context)
        {
            graphicsList = (DrawList)info.GetValue("graphicslist", typeof(DrawList));
            Thermo = (ThermoDynamicOptions)info.GetValue("ThermoOptions", typeof(ThermoDynamicOptions));
            try
            {
                calctype = (EnumCalcSeq)info.GetValue("CalcMethod", typeof(EnumCalcSeq));
                Components = (Components)info.GetValue("Components", typeof(Components));
                scale = info.GetSingle("ScaleDraw");
                Offsetx = info.GetInt32("Offsetx");
                Offsety = info.GetInt32("Offsety");
                x = info.GetDouble("x");
                y = info.GetDouble("y");
            }
            catch
            {
                calctype = EnumCalcSeq.BackProp;
            }
        }

        internal DrawColumnCondenserCollection ReturnCondensers()
        {
            DrawColumnCondenserCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawColumnCondenser dcc)
                    dl.Add(dcc);
            }
            return dl;
        }

        internal DrawColumnReboilerCollection ReturnReboilers()
        {
            DrawColumnReboilerCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawColumnReboiler dcr)
                    dl.Add(dcr);
            }
            return dl;
        }

        protected virtual void RaiseValueErrorEvent(EventArgs e)
        {
            errorChangedEvent?.Invoke(this, e);
        }

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            foreach (DrawObject d in graphicsList)
                d.OnRequestParentObject += new Func<object>(delegate { return this; });

            SetStripperDrawGUIDs();
            SetStripperTopFeedGUIDs();

            // re-attach objects, trays and nodes

            // AddNameObjects();
        }

        public void UpdateGuidConnections()
        {
            foreach (DrawObject ds in this)
                if (ds is DrawRectangle dr)
                    this.UpdateGuids(dr);
        }

        internal void UpdateDrawObjects()
        {
            foreach (DrawObject obj in this.graphicsList)
                if (obj is DrawBaseStream stream)
                    stream.UpdateDrawObjects(this);
        }

        public void AddNameObjects()
        {
            List<DrawRectangle> drs = this.ReturnDrawRectangles();

            foreach (DrawRectangle dr in drs)
            {
                dr.drawName = new();
                dr.drawName.Attachedobject = dr;
                dr.drawName.Location = new(dr.Rectangle.TopRight(), new Size(100, 11));
                this.Add(dr.drawName);
            }

            List<DrawBaseStream> drb = this.ReturnStreams();

            foreach (DrawBaseStream dr in drb)
            {
                dr.drawName = new();
                dr.drawName.Attachedstream = dr;
                Point dnloc = dr.Center;
                dnloc.Offset(dr.NameOffSetX, dr.NameOffSetY);
                dr.drawName.Location = new(dnloc, new Size(100, 11));
                this.Add(dr.drawName);
            }
        }

        public Func<object> OnRequestParentObject;

        public object GetParent()
        {
            object parent = RequestParentObject();
            return parent;
        }

        protected object RequestParentObject()
        {
            if (OnRequestParentObject == null)
                throw new Exception("OnRequestParentObject handler is not assigned");

            return OnRequestParentObject();
        }

        private DrawStreamCollection GetStripperTopFeeds()
        {
            List<DrawBaseStream> streams = ReturnStreams();
            DrawStreamCollection dl = new();

            foreach (DrawBaseStream s in streams)
            {
                if (s is DrawMaterialStream stream && s.EndConnected && s.FrontConnected)
                {
                    DrawObject StartObject = ReturnStartObject(s);
                    DrawObject EndObject = ReturnEndObject(s);

                    if (StartObject is DrawColumnTraySection section && EndObject is DrawColumnTraySection)
                    {
                        DrawColumnTraySection dcts = section;
                        if (dcts.Striptype == eStripperType.None) // cant attach a stripper to a stripper
                            dl.Add(stream);
                    }
                }
            }
            return dl;
        }

        private DrawStreamCollection GetStripperTopVapours()
        {
            List<DrawBaseStream> streams = ReturnStreams();
            DrawStreamCollection dl = new();

            foreach (DrawBaseStream s in streams)
            {
                if (s is DrawMaterialStream stream && s.EndConnected && s.FrontConnected)
                {
                    DrawObject StartObject = ReturnStartObject(s);
                    DrawObject EndObject = ReturnEndObject(s);

                    if (StartObject is DrawColumnTraySection && EndObject is DrawColumnTraySection section)
                    {
                        DrawColumnTraySection dcts = section;
                        if (dcts.Striptype == eStripperType.None) // cant attach a stripper to a stripper
                            dl.Add(stream);
                    }
                }
            }
            return dl;
        }

        public void SetStripperDrawGUIDs()
        {
            DrawStreamCollection streams = GetStripperTopFeeds();
            foreach (DrawMaterialStream s in streams)
            {
                DrawColumnTraySection dcts = (DrawColumnTraySection)ReturnEndObject(s);
                dcts.StripFeedGuid = s.Guid;
                dcts.StripperTopFeed = s;
            }
        }

        public void SetStripperTopFeedGUIDs()
        {
            DrawStreamCollection streams = GetStripperTopVapours();
            foreach (DrawMaterialStream s in streams)
            {
                DrawColumnTraySection dcts = (DrawColumnTraySection)ReturnStartObject(s);
                dcts.StripVapourGuid = s.Guid;
                dcts.StripperVapourProduct = s;
            }
        }

        private readonly DrawList graphicsList;
        private const string entryCount = "Count";

        public Dictionary<Guid, DrawObject> ObjDictionary = new();

        public void UpdateGuidDic()
        {
            ObjDictionary.Clear();
            foreach (DrawObject obj in graphicsList)
                switch (obj)
                {
                    case DrawColumnTraySection dcts:
                        ObjDictionary.Add(dcts.Guid, dcts);
                        foreach (DrawTray t in dcts.DrawTrays)
                            ObjDictionary.Add(t.Guid, t);

                        break;

                    default:
                        ObjDictionary.Add(obj.Guid, obj);
                        break;
                }
        }

        public IEnumerator GetEnumerator()
        {
            return graphicsList.GetEnumerator();
        }

        public void ReconnectObjects()
        {
            foreach (DrawObject dobj in graphicsList)
            {
                switch (dobj)
                {
                    case DrawBaseStream ds:
                        ds.UpdateDrawObjects(this);
                        break;

                    case DrawColumnTraySection dcts:
                        foreach (DrawTray tray in dcts)
                        {
                            tray.Owner = dcts;
                            foreach (Node n in tray.Hotspots)
                                n.Owner = tray;
                        }
                        foreach (Node hs in dcts.Hotspots)
                            hs.Owner = dcts;
                        break;

                    case DrawRectangle ds:
                        foreach (Node hs in ds.Hotspots)
                            hs.Owner = ds;
                        break;
                }
            }
        }

        private EnumCalcActive calcActive = EnumCalcActive.Yes;

        public EnumCalcActive CalcActive
        {
            get { return calcActive; }
            set { calcActive = value; }
        }

        private EnumCalcSeq calctype = EnumCalcSeq.BackProp;

        public EnumCalcSeq CalcType
        {
            get { return calctype; }
            set { calctype = value; }
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public void Dump()
        {
            Trace.WriteLine("");

            foreach (DrawObject o in graphicsList)
                o.Dump();
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (graphicsList.Count > 0);
            graphicsList.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return graphicsList.Count;
            }
        }

        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= graphicsList.Count)
                    return null;

                return graphicsList[index];
            }
        }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (DrawObject o in Selection)
                    n++;

                return n;
            }
        }

        public List<DrawObject> AllObjects
        {
            get
            {
                List<DrawObject> res = new();

                foreach (DrawObject o in graphicsList)
                    res.Add(o);

                return res;
            }
        }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public List<DrawObject> GetSelected
        {
            get
            {
                List<DrawObject> res = new();

                foreach (DrawObject o in Selection)
                    if (o.Selected)
                        res.Add(o);

                return res;
            }
        }

        /// <summary>
        /// Returns INumerable object which may be used for enumeration
        /// of selected objects.
        ///
        /// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
        /// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
        /// To make this program CLS-compliant, replace
        /// IEnumerable<DrawObject> with IEnumerable. This requires
        /// casting to object at runtime.
        /// </summary>
        /// <value></value>
        public List<DrawObject> Selection
        {
            get
            {
                List<DrawObject> list = new();
                foreach (DrawObject o in graphicsList)
                    if (o.Selected)
                        list.Add(o);

                return list;
            }
        }

        public Components Components { get => components; set => components = value; }
        public float Scale { get => scale; set => scale = value; }

        public DrawList Graphics => graphicsList;

        private void GraphicsList_errorChangedEvent(object sender, EventArgs e)
        {
            RaiseValueErrorEvent(e);
        }

        /// <summary>
        /// Insert object to specified place.
        /// Used for Undo.
        /// </summary>
        public void Insert(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.Insert(index, obj);
                if (obj is DrawMaterialStream stream)
                    DrawArea.StaticStreamList.Add(stream);
            }
        }

        internal void Relocate(int dx, int dy)
        {
            foreach (DrawObject item in graphicsList)
            {
                switch (item)
                {
                    case DrawRectangle dr:
                        dr.Location = new Rectangle(dr.Location.X + dx, dr.Location.Y + dy, dr.Location.Width, dr.Location.Height);
                        break;

                    case DrawMaterialStream dl:
                        dl.Relocate(dx, dy);
                        break;
                }
            }
        }

        internal void RelocateTo(int dx, int dy)
        {
            foreach (DrawObject item in graphicsList)
            {
                switch (item)
                {
                    case DrawRectangle dr:
                        dr.Location = new Rectangle(dr.Location.X + dx, dr.Location.Y + dy, dr.Location.Width, dr.Location.Height);
                        break;

                    case DrawMaterialStream dl:
                        dl.Relocate(dx, dy);
                        break;
                }
            }
        }

        internal int GetTopMost()
        {
            int Top = 0;

            foreach (DrawObject draw in graphicsList)
            {
                switch (draw)
                {
                    case DrawRectangle dr:
                        if (dr.Location.Top < Top)
                            Top = dr.Location.Top;
                        break;

                    case DrawMaterialStream dl:
                        if (dl.Top < Top)
                            Top = dl.Top;
                        break;

                    default:
                        break;
                }
            }

            return Top;
        }

        internal int GetLeftMost()
        {
            int Left = 0;

            foreach (DrawObject draw in graphicsList)
            {
                switch (draw)
                {
                    case DrawRectangle dr:
                        if (dr.Location.Left < Left)
                            Left = dr.Location.Left;
                        break;

                    case DrawMaterialStream dl:
                        if (dl.Left < Left)
                            Left = dl.Left;
                        break;

                    default:
                        break;
                }
            }

            return Left;
        }

        /// <summary>
        /// Replace object in specified place.
        /// Used for Undo.
        /// </summary>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.RemoveAt(index);
                graphicsList.Insert(index, obj);
                if (obj is DrawMaterialStream stream)
                    DrawArea.StaticStreamList.Add(stream);
            }
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void RemoveAt(int index)
        {
            graphicsList.RemoveAt(index);
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void Remove(DrawObject obj)
        {
            graphicsList.Remove(obj);
        }

        public bool Contains(DrawObject obj)
        {
            return graphicsList.Contains(obj);
        }

        /// <summary>
        /// Delete last added object from the list
        /// (used for Undo operation).
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if (graphicsList.Count > 0)
            {
                graphicsList.RemoveAt(0);
            }
        }

        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in graphicsList)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }
        }

        public List<DrawColumnTraySection> ReturnDrawColumnTraySections()
        {
            List<DrawColumnTraySection> res = new();

            foreach (DrawObject o in this)  // only used if selected
            {
                if (o is DrawColumnTraySection section)
                {
                    res.Add(section);
                }
            }
            return res;
        }

        public void UnselectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = false;
            }
        }

        public void SelectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = true;
            }
        }

        public List<DrawBaseStream> ReturnStreams()
        {
            List<DrawBaseStream> dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawBaseStream stream)
                    dl.Add(stream);
            }
            return dl;
        }

        public List<DrawMaterialStream> ReturnMaterialStreams()
        {
            List<DrawMaterialStream> dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream)
                    dl.Add(stream);
            }
            return dl;
        }

        public List<DrawRectangle> ReturnDrawRectangles()
        {
            List<DrawRectangle> dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawRectangle rectangle)
                    dl.Add(rectangle);
            }
            return dl;
        }

        public List<DrawName> ReturnDrawNames()
        {
            List<DrawName> dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawName name)
                    dl.Add(name);
            }
            return dl;
        }

        public List<DrawRecycle> ReturnRecycles()
        {
            List<DrawRecycle> dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawRecycle dr)
                    dl.Add(dr);
            }
            return dl;
        }

        public DrawList ReturnFeedObjects()
        {
            DrawList dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms
                    && dms.FrontConnected == false)
                {
                    dl.Add(o);
                }

                if (o.IsFeedObject)
                {
                    dl.Add(o);
                }
            }
            return dl;
        }

        public DrawList ReturnConnectedObjects(DrawObject s)
        {
            DrawList dl = new();

            if (s is DrawBaseStream ds)
            {
                foreach (DrawObject o in graphicsList)
                {
                    if (o is DrawRectangle
                        && (o.Guid == ds.EndDrawObjectGuid
                        || o.Guid == ds.StartDrawObjectGuid))
                    {
                        dl.Add(o);
                    }
                }
            }
            else if (s is DrawRectangle dr)
            {
                foreach (DrawObject o in graphicsList)
                {
                    if (o is DrawBaseStream ids
                        && (dr.Guid == ids.EndDrawObjectGuid
                        || dr.Guid == ids.StartDrawObjectGuid))
                    {
                        dl.Add(ids);
                    }
                }
            }
            return dl;
        }

        public static List<Port> ReturnConnectedPorts(DrawObject s)
        {
            List<Port> ports = new();

            if (s is DrawBaseStream ds)
            {
                ports.AddRange(ds.ConnectedPorts);
            }
            else if (s is DrawRectangle dr)
            {
                ports.AddRange(dr.ConnectedPorts);
            }
            return ports;
        }

        public DrawObject ReturnEndObject(DrawBaseStream s)
        {
            foreach (DrawObject o in graphicsList)
                if (o.Guid == s.EndDrawObjectGuid)
                    return o;

            return null;
        }

        public DrawObject ReturnStartObject(DrawBaseStream s)
        {
            foreach (DrawObject o in graphicsList)
                if (o is not DrawBaseStream && o.Guid == s.StartDrawObjectGuid)
                    return o;
            return null;
        }

        public DrawObject ReturnStartObject(DrawSignalStream s)
        {
            foreach (DrawObject o in graphicsList)
                if (o.Guid == s.StartDrawObjectGuid)
                    return o;
            return null;
        }

        public void UpdateGuids(DrawRectangle dr)
        {
            Guid newguid = Guid.NewGuid();
            Guid oldguid = dr.Guid;

            foreach (DrawObject o in graphicsList)
                if (o is DrawBaseStream dbs && dbs.startDrawObjectID == oldguid)
                {
                    dr.Guid = newguid;
                    dbs.startDrawObjectID = newguid;
                    break;
                }

            foreach (DrawObject o in graphicsList)
                if (o is DrawBaseStream dbs && dbs.endDrawObjectGuid == oldguid)
                {
                    dr.Guid = newguid;
                    dbs.endDrawObjectGuid = newguid;
                    break;
                }
        }

        public DrawStreamCollection ReturnInternalFeedStreams()
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms && !dms.FrontConnected)
                {
                    dl.Add(dms);
                    dms.Stream.IsInternal = false;
                }
            }
            return dl;
        }

        public DrawMaterialStream ReturnStream(Guid ID)
        {
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream
                    && stream.Guid == ID)
                {
                    return stream;
                }
            }
            return null;
        }

        public DrawMaterialStream ReturnStream(string Name)
        {
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms
                    && dms.Name == Name)
                {
                    return dms;
                }
            }
            return null;
        }

        public DrawStreamCollection ReturnExitingStreams(Guid ObjectID)
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms && dms.StartDrawObjectGuid == ObjectID)
                {
                    dl.Add(dms);
                }
            }
            return dl;
        }

        public DrawList ReturnConnectingStreams(Guid ObjectID)
        {
            DrawList dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms
                    && dms.StartDrawObjectGuid == ObjectID
                    && dms.EndDrawObjectGuid != Guid.Empty)
                {
                    dl.Add(dms);
                    dms.Stream.IsInternal = true;
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnInternalConnectingStreams()
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms
                    && dms.EndConnected
                    && dms.FrontConnected)
                {
                    dms.Stream.IsInternal = true;
                    dl.Add(dms);
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnIntersectionNetStreams()
        {
            DrawStreamCollection dl = new();
            HotSpotType front = HotSpotType.None;

            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream
                    && stream.EndConnected
                    && stream.FrontConnected)
                {
                    if (stream.StartNode is null)
                        continue;
                    if (stream.StartNode != null)
                        front = stream.StartNode.NodeType;
                    if ((front == HotSpotType.TrayNetLiquid
                        || front == HotSpotType.TrayNetVapour) && !(
                            stream.StartDrawObject is DrawColumnCondenser ||
                            stream.StartDrawObject is DrawColumnReboiler ||
                            stream.EndDrawObject is DrawColumnCondenser ||
                            stream.EndDrawObject is DrawColumnReboiler))
                        dl.Add(stream);
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnIntersectionDrawStreams()
        {
            DrawStreamCollection dl = new();
            HotSpotType front;
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream && stream.EndConnected && stream.FrontConnected)
                {
                    if (stream.StartNode is null)
                        continue;

                    front = stream.StartNode.NodeType;
                    //back = stream.endNode.NodeType;
                    if (stream.EndDrawObject is DrawPA || stream.StartDrawObject is DrawPA)
                        continue;

                    if (front == HotSpotType.LiquidDraw || front == HotSpotType.VapourDraw)
                        if (!(
                        stream.StartDrawObject is DrawColumnCondenser ||
                        stream.StartDrawObject is DrawColumnReboiler ||
                        stream.EndDrawObject is DrawColumnCondenser ||
                        stream.EndDrawObject is DrawColumnReboiler))
                            dl.Add(stream);
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnPAStreams()
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawPA pa)
                {
                    foreach (DrawObject ds in graphicsList)
                    {
                        if (ds is DrawMaterialStream dss
                            && (dss.StartDrawObjectGuid == pa.Guid || dss.EndDrawObjectGuid == pa.Guid))
                            dl.Add(dss);
                    }
                }
            }
            return dl;
        }

        public DrawMaterialStream ReturnStream(Guid ObjectGuid, string Name) // not hotspot guid
        {
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream
                    && stream.Guid == ObjectGuid
                    && (stream.StartName == Name || stream.EndName == Name))
                {
                    return stream;
                }
            }
            return null;
        }

        public DrawMaterialStream ReturnStream(Guid ObjectGuid, Guid NodeGuid) // not hotspot guid
        {
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream ds
                    && (ds.StartDrawObjectGuid == ObjectGuid
                    || ds.EndDrawObjectGuid == ObjectGuid)
                    && (ds.StartNodeGuid == NodeGuid
                    || ds.EndNodeGuid == NodeGuid))
                    return ((DrawMaterialStream)o);
            }
            return null;
        }

        public Node ReturnHotSpot(Guid ObjectID, Guid HotspotID)
        {
            DrawObject DO = null;
            Node HS = null;

            foreach (DrawObject d in graphicsList)
            {
                if (d.Guid == ObjectID)
                {
                    DO = d;
                    break;
                }
            }

            if (DO != null)
                HS = DO.Hotspots.Search(HotspotID);

            return HS;
        }

        public DrawObject GetObject(Guid ID, bool checkfortray = true)
        {
            foreach (DrawObject d in graphicsList)
            {
                switch (d)
                {
                    case DrawColumnTraySection dcts:
                        if (dcts.Guid == ID)
                            return dcts;
                        foreach (DrawTray tray in dcts.DrawTrays)
                            if (tray.Guid == ID)
                                if (checkfortray)
                                    return tray;
                                else
                                    return dcts;
                        break;

                    default:
                        if (d.Guid == ID)
                            return d;
                        break;
                }
            }
            return null;
        }

        public DrawObject ReturnObject(Type ID)
        {
            foreach (DrawObject d in graphicsList)
            {
                if (d != null && d.GetType() == ID)
                    return d;
            }
            return null;
        }

        public DrawObject ReturnObject(String Name)
        {
            foreach (DrawObject d in graphicsList)
            {
                if (d.Name == Name)
                    return d;
            }
            return null;
        }

        public List<DrawObject> ReturnUnitOps()
        {
            DrawList objects = new();

            foreach (DrawObject d in graphicsList)
            {
                if (d is DrawRectangle)
                    objects.Add(d);
            }
            return objects;
        }

        public DrawList ReturnObjects(Type ID)
        {
            DrawList objects = new();

            foreach (DrawObject d in graphicsList)
            {
                if (d.GetType() == ID)
                    objects.Add(d);
            }
            return objects;
        }

        public class myReverserClass : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                return new CaseInsensitiveComparer().Compare(y, x);
            }
        }

        public DrawColumnTraySectionCollection ReturnTraySections()
        {
            DrawColumnTraySectionCollection objects = new();

            foreach (DrawObject d in graphicsList)
            {
                if (d.GetType() == typeof(DrawColumnTraySection))
                    objects.Add((DrawColumnTraySection)d);
            }
            return objects;
        }

        public List<DrawColumnTraySection> ReturnTraySectionsAndStrippers()
        {
            List<DrawColumnTraySection> objects = new();

            foreach (DrawObject d in graphicsList)
            {
                if (d.GetType() == typeof(DrawColumnTraySection))
                    objects.Add((DrawColumnTraySection)d);
            }
            return objects;
        }

        public DrawPACollection ReturnAllPumpArounds()
        {
            DrawPACollection objects = new();

            foreach (DrawObject da in graphicsList)
                if (da is DrawPA pa)
                {
                    objects.Add((DrawPA)da);
                    foreach (DrawObject stream in graphicsList)
                    {
                        if (stream is DrawMaterialStream dsm)
                        {
                            if (dsm.endDrawObject == pa)
                                pa.feed = dsm;
                            if (dsm.startDrawObject == pa)
                                pa.effluent = dsm;
                        }
                    }

                    if (pa.effluent is null || pa.feed is null)
                        MessageBox.Show("PA" + pa.Name + "Does not have either draw or return stream");
                }

            return objects;
        }

        public DrawColumnTraySectionCollection ReturnActiveStrippers()
        {
            DrawColumnTraySectionCollection objects = new();
            for (int n = 0; n < graphicsList.Count; n++)
            {
                if (graphicsList[n] is DrawColumnTraySection section)
                {
                    DrawColumnTraySection da = section;

                    if (da.Striptype != eStripperType.None)
                        objects.Add(da);
                }
            }
            return objects;
        }

        public DrawColumnTraySectionCollection ReturnAllStrippers()
        {
            DrawColumnTraySectionCollection objects = new();
            IDrawObject da;

            for (int n = 0; n < graphicsList.Count; n++)
            {
                da = graphicsList[n];
                if (da is DrawColumnTraySection dcts
                    && dcts.Striptype != eStripperType.None)
                    objects.Add(dcts);
            }
            return objects;
        }

        public void ClearPumpArounds()
        {
            List<DrawPA> objects = new();
            foreach (DrawObject d in graphicsList)
            {
                if (d.GetType() == typeof(DrawPA))
                    objects.Add((DrawPA)d);
            }

            for (int n = 0; n < objects.Count; n++)
                graphicsList.Remove(objects[n]);
        }

        public DrawStreamCollection ReturnSideProductStreams(DrawObject d, bool EndIsConnected)
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream ds
                    && ds.StartDrawObjectGuid == d.Guid
                    && ds.EndConnected == EndIsConnected)
                {
                    Node hs = ReturnHotSpot(d.Guid, ds.StartNodeGuid);
                    if (hs != null && hs.Name != "BOTPRODUCT" && hs.Name != "TOPPRODUCT")
                        dl.Add(ds);
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnSideProductStreams(DrawObject doobj)
        {
            DrawStreamCollection dl = new();

            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream d
                    && d.startDrawObject == doobj
                    && ReturnHotSpot(d.Guid, d.StartNodeGuid) is Node hs)
                    dl.Add(d);
            }
            return dl;
        }

        public DrawStreamCollection ReturnProductStreams(Tray d)
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream && stream.StartDrawObjectGuid == d.Guid && stream.EndConnected == false)
                {
                    dl.Add(stream);
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnInternalProductStreams()
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms)
                    if (!dms.EndConnected)
                    {
                        Node node = dms.StartNode;
                        if (node is not null && node.NodeType != HotSpotType.InternalExport)
                        {
                            dl.Add(dms);
                            switch (node.NodeType)
                            {
                                case HotSpotType.TopTrayVapour:
                                case HotSpotType.TrayNetVapour:
                                    dms.IsVapourDraw = true;
                                    break;

                                case HotSpotType.BottomTrayLiquid:
                                case HotSpotType.LiquidDraw:
                                case HotSpotType.TrayNetLiquid:
                                case HotSpotType.LiquidDrawLeft:
                                    dms.IsVapourDraw = false;
                                    break;
                            }
                        }
                    }
            }
            return dl;
        }

        public DrawStreamCollection ReturnExternalExportedStreams(DrawObject d)
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream)
                {
                    if (stream.startDrawObjectID == d.Guid
                    && stream.StartNode is not null
                    && stream.StartNode.NodeType == HotSpotType.InternalExport)
                    {
                        stream.IsPumparoud = true;
                        dl.Add(stream);
                    }
                    else
                        stream.IsPumparoud = false;
                }
            }
            return dl;
        }

        public DrawStreamCollection ReturnExternalProductStreams(DrawObject d)
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream
                    && stream.StartDrawObjectGuid == d.Guid
                    && stream.StartNode is not null
                    && stream.StartNode.NodeType != HotSpotType.InternalExport)
                    dl.Add(stream);
            }
            return dl;
        }

        public DrawStreamCollection ReturnExternalFeedStreams(DrawObject d)
        {
            DrawStreamCollection dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream stream && stream.EndDrawObjectGuid == d.Guid)
                    dl.Add(stream);
            }
            return dl;
        }

        public DrawList ReturnFrontConnectedStreams()
        {
            DrawList dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms
                    && dms.FrontConnected == true)
                {
                    dl.Add(dms);
                }
            }
            return dl;
        }

        public DrawList ReturnStreams(DrawRectangle dr)
        {
            DrawList dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms &&
                    (dr.Guid == dms.StartDrawObjectGuid || dr.Guid == dms.EndDrawObjectGuid))
                    dl.Add(o);
            }
            return dl;
        }

        public List<DrawMaterialStream> ReturnStreams(Node dr)
        {
            List<DrawMaterialStream> dl = new();
            foreach (DrawObject o in graphicsList)
            {
                if (o is DrawMaterialStream dms
                    && (dr.Guid == dms.EndNodeGuid || dr.Guid == dms.StartNodeGuid))
                    dl.Add(dms);
            }
            return dl;
        }

        public DrawMaterialStream ReturnAttachedStream(Node hs)
        {
            foreach (DrawObject o in graphicsList)
            {
                if (hs is not null
                    && o is DrawMaterialStream stream &&
                    (hs.Guid == stream.EndNodeGuid || hs.Guid == stream.StartNodeGuid))
                {
                    return stream;
                }
            }
            return null;
        }

        public void UpdateAllStreams()
        {
            foreach (DrawObject o in graphicsList)
            {
                UpdateStreams(o);
            }
        }

        public static void UpdateStreams(DrawBaseStream ds, int dx, int dy)
        {
            int start, end;

            //if(ds.st)

            if (ds.StartName != "Floating")
                start = 0;
            else
                start = 0;

            if (ds.StartNode is not null && ds.StartNode.IsConnected)
                start = 1;
            else
                start = 0;

            if (ds.EndNode is not null && ds.EndNode.IsConnected)
                end = 1;
            else
                end = 0;

            if (ds.EndName != "Floating")
                end = ds.segArray.Points().Count;
            else
                end = ds.segArray.Points().Count;

            ds.segArray.Move(dx, dy);

            //ds.ResetStreamPoints();
            return;
        }

        public void UpdateStreams(DrawObject dr)
        {
            Node node;

            switch (dr)
            {
                case DrawColumnTraySection DCTS:
                    DrawTray tray;

                    foreach (DrawObject stream in graphicsList)
                    {
                        if (stream is DrawMaterialStream ds)
                        {
                            node = DCTS.GetNode(ds.StartNodeGuid);

                            if (node is not null)
                            {
                                ds.UpdateOrthogonals();
                            }
                            else
                            {
                                tray = DCTS.GetTray(ds.StartDrawObjectGuid);
                                if (tray != null)
                                    node = tray.GetNode(ds.StartNodeGuid);

                                if (node != null)
                                {
                                    //float RelX = node.Absolute.X;
                                    //float RelY = node.Absolute.Y;
                                    //ds.MoveHandleTo(new Point((int)RelX, (int)RelY), 1);
                                    ds.UpdateOrthogonals();
                                }
                            }

                            node = DCTS.GetNode(ds.EndNodeGuid);
                            if (node is not null)
                            {
                                ds.UpdateOrthogonals();
                            }
                            else
                            {
                                tray = DCTS.GetTray(ds.EndDrawObjectGuid);
                                if (tray != null)
                                    node = tray.GetNode(ds.EndNodeGuid);

                                if (node != null)
                                {
                                    float RelX = node.Absolute.X;
                                    float RelY = node.Absolute.Y;
                                    ds.MoveHandleTo(new Point((int)RelX, (int)RelY), ds.HandleCount);
                                }
                            }
                        }
                    }
                    break;

                case DrawRectangle dobj:
                    {
                        foreach (DrawObject stream in ReturnStreams(dobj))
                            if (stream is not null && stream is DrawBaseStream ds)
                                ds.UpdateOrthogonals();
                    }
                    break;

                case DrawMaterialStream movedStream:
                    {
                        Node datanode = movedStream.dataNode;
                        if (datanode is not null && ObjDictionary.ContainsKey(datanode.AttachedStreamGuid))
                        {
                            DrawObject dbs = ObjDictionary[datanode.AttachedStreamGuid];
                            if (dbs is DrawSignalStream dss)
                                if (dr.Guid == dss.StartDrawObjectGuid)
                                {
                                    if (dss.StartNode is Node start)
                                    {
                                        float RelX = start.Absolute.X;
                                        float RelY = start.Absolute.Y;
                                        dss.MoveHandleTo(new Point((int)RelX, (int)RelY), 1);
                                    }
                                }
                                else if (dr.Guid == dss.EndDrawObjectGuid)
                                {
                                    if (dss.EndNode is Node end)
                                    {
                                        float RelX = end.Absolute.X;
                                        float RelY = end.Absolute.Y;
                                        dss.MoveHandleTo(new Point((int)RelX, (int)RelY), dss.HandleCount);
                                    }
                                }
                        }
                    }
                    break;

                case DrawBaseStream dbs:
                    if (dr.Guid == dbs.StartDrawObjectGuid)
                    {
                        if (dbs.StartNode is Node start)
                        {
                            float RelX = start.Absolute.X;
                            float RelY = start.Absolute.Y;
                            dbs.MoveHandleTo(new Point((int)RelX, (int)RelY), 1);
                        }
                    }
                    else if (dr.Guid == dbs.EndDrawObjectGuid)
                    {
                        if (dbs.EndNode is Node end)
                        {
                            float RelX = end.Absolute.X;
                            float RelY = end.Absolute.Y;
                            dbs.MoveHandleTo(new Point((int)RelX, (int)RelY), dbs.HandleCount);
                        }
                    }
                    break;

                    //default:
                    //    break;
            }
            return;
        }

        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                graphicsList.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                graphicsList.Add(tempList[i]);
            }

            return (n > 0);
        }

        private GraphicsProperties GetProperties()
        {
            GraphicsProperties properties = new();

            bool bFirst = true;

            int firstColor = 0;
            int firstPenWidth = 1;

            bool allColorsAreEqual = true;
            bool allWidthAreEqual = true;

            foreach (DrawObject o in Selection)
            {
                if (bFirst)
                {
                    firstColor = o.StreamColor.ToArgb();
                    firstPenWidth = o.PenWidth;
                    bFirst = false;
                }
                else
                {
                    if (o.StreamColor.ToArgb() != firstColor)
                        allColorsAreEqual = false;

                    if (o.PenWidth != firstPenWidth)
                        allWidthAreEqual = false;
                }
            }

            if (allColorsAreEqual)
            {
                properties.Color = Color.FromArgb(firstColor);
            }

            if (allWidthAreEqual)
            {
                properties.PenWidth = firstPenWidth;
            }

            return properties;
        }

        private bool ApplyProperties(GraphicsProperties properties)
        {
            bool changed = false;

            foreach (DrawObject o in graphicsList)
            {
                if (o.Selected)
                {
                    if (properties.Color.HasValue)
                    {
                        if (o.StreamColor != properties.Color.Value)
                        {
                            o.StreamColor = properties.Color.Value;
                            DrawObject.LastUsedColor = properties.Color.Value;
                            changed = true;
                        }
                    }

                    if (properties.PenWidth.HasValue)
                    {
                        if (o.PenWidth != properties.PenWidth.Value)
                        {
                            o.PenWidth = properties.PenWidth.Value;
                            DrawObject.LastUsedPenWidth = properties.PenWidth.Value;
                            changed = true;
                        }
                    }
                }
            }
            return changed;
        }

        public bool ShowPropertiesDialog(DrawArea parent)
        {
            if (SelectionCount < 1)
                return false;

            GraphicsProperties properties = GetProperties();
            PropertiesDialog dlg = new();
            dlg.Properties = properties;

            CommandChangeState c = new(this);

            if (dlg.ShowDialog(parent) != DialogResult.OK)
                return false;

            if (ApplyProperties(properties))
            {
                c.newState(this);
                parent.AddCommandToHistory(c);
            }

            return true;
        }

        internal void Add(GraphicsList gl)
        {
            graphicsList.AddRange(gl.AllObjects);
        }

        public void UpdateAllConnections()
        {
            foreach (DrawObject obj in graphicsList)
            {
                if (obj is DrawColumn column)
                    column.graphicslist.UpdateDrawStreamGuidConnections();
            }
            UpdateDrawStreamGuidConnections();
        }

        private void UpdateDrawStreamGuidConnections()
        {
            EraseAllNodeConnections();

            DrawObject startObj, endObj;
            Node startObjNode, endObjnode;

            UpdateGuidDic();

            List<DrawBaseStream> streams = ReturnStreams();

            foreach (DrawBaseStream stream in streams)
            {
                startObj = null;
                endObj = null;
                startObjNode = null;
                endObjnode = null;

                stream.startDrawObject = null;
                stream.endDrawObject = null;
                if (stream.StartNode is not null && stream.StartNode.NodeType != HotSpotType.Floating) // dont disconnect floating nodes
                    stream.StartNode = null;
                if (stream.EndNode is not null && stream.EndNode.NodeType != HotSpotType.Floating)
                    stream.EndNode = null;
                stream.StartDrawTray = null;
                stream.EndDrawTray = null;
                stream.StartDrawTraySection = null;
                stream.endDrawTraySection = null;

                if (ObjDictionary.ContainsKey(stream.startDrawObjectID))
                {
                    startObj = ObjDictionary[stream.startDrawObjectID];
                    startObjNode = startObj.GetNode(stream.startNodeGuid);
                    if (startObjNode != null)
                    {
                        startObjNode.IsConnected = true;
                        startObjNode.AttachedStreamGuid = stream.Guid;
                    }
                }

                if (ObjDictionary.ContainsKey(stream.endDrawObjectGuid))
                {
                    endObj = ObjDictionary[stream.endDrawObjectGuid];
                    endObjnode = endObj.GetNode(stream.endNodeGuid);
                    if (endObjnode != null)
                    {
                        endObjnode.IsConnected = true;
                        endObjnode.AttachedStreamGuid = stream.Guid;
                    }
                }

                if (startObjNode != null)
                {
                    startObjNode.IsConnected = true;
                    stream.StartNode = startObjNode;
                    stream.startDrawObject = startObj;

                    switch (startObj)
                    {
                        case DrawTray t:
                            stream.StartDrawTray = t;
                            stream.EngineDrawTrayGuid = t.Tray.Guid;
                            stream.EngineDrawSectionGuid = t.Tray.Owner.Guid;
                            break;

                        case DrawColumnTraySection dcts:
                            stream.StartDrawTraySection = dcts;
                            if (stream.StartNode.Owner is DrawTray dt)
                            {
                                stream.StartDrawTray = dt;
                                stream.EngineDrawTrayGuid = dt.Tray.Guid;
                            }
                            stream.EngineDrawSectionGuid = dcts.TraySection.Guid;
                            break;

                        case DrawColumnCondenser dcc:
                            stream.StartDrawObject = dcc;
                            stream.StartDrawObjectGuid = dcc.Guid;
                            stream.StartNode = startObjNode;
                            stream.startNodeGuid = startObjNode.Guid;
                            break;

                        case DrawColumnReboiler dcr:
                            stream.StartDrawObject = dcr;
                            stream.StartDrawObjectGuid = dcr.Guid;
                            stream.StartNode = startObjNode;
                            stream.startNodeGuid = startObjNode.Guid;
                            break;

                        case DrawPA pa:
                            if (endObj is DrawTray tray)
                            {
                                pa.ReturnTrayDrawGuid = tray.Guid;
                                pa.Destinationguid = tray.Owner.Guid;
                            }
                            break;
                    }
                    stream.StartName = startObjNode.Name;
                }

                if (endObjnode != null)
                {
                    endObjnode.IsConnected = true;
                    stream.EndNode = endObjnode;
                    stream.endDrawObject = endObj;

                    switch (endObj)
                    {
                        case DrawTray t:
                            stream.EndDrawTray = t;
                            stream.EngineDrawTrayGuid = t.Tray.Guid;
                            stream.EngineDrawSectionGuid = t.Tray.Owner.Guid;
                            break;

                        case DrawColumnTraySection dcts:
                            stream.endDrawTraySection = dcts;
                            if (stream.EndNode.Owner is DrawTray dt)
                            {
                                stream.EndDrawTray = dt;
                                stream.EngineDrawTrayGuid = dt.Tray.Guid;
                            }
                            stream.EngineDrawSectionGuid = dcts.TraySection.Guid;
                            break;

                        case DrawColumnCondenser dcc:
                            stream.endDrawObject = dcc;
                            stream.endDrawObjectGuid = dcc.Guid;
                            stream.EndNode = endObjnode;
                            stream.endNodeGuid = endObjnode.Guid;
                            break;

                        case DrawColumnReboiler dcr:
                            stream.endDrawObject = dcr;
                            stream.endDrawObjectGuid = dcr.Guid;
                            stream.EndNode = endObjnode;
                            stream.endNodeGuid = endObjnode.Guid;
                            break;

                        case DrawPA pa:
                            if (endObj is DrawTray tray)
                            {
                                pa.ReturnTrayDrawGuid = tray.Guid;
                                pa.Destinationguid = tray.Owner.Guid;
                            }
                            break;
                    }
                    stream.EndName = endObjnode.Name;
                }
            }

            EraseAllPortConnections();
            ReconnectPortConnections(streams);
        }

        private void EraseAllNodeConnections()
        {
            foreach (DrawObject dobj in graphicsList)
            {
                dobj.Hotspots.ClearConnected();

                switch (dobj)
                {
                    case DrawColumn dc:
                        dc.Hotspots.ClearConnected();
                        break;

                    case DrawColumnTraySection dcts:
                        //dcts.EraseAllNodeConnections();
                        break;
                }
            }
        }

        private void EraseAllPortConnections()
        {
            foreach (DrawObject dobj in graphicsList)
            {
                switch (dobj)
                {
                    case DrawColumn dc:
                        dc.graphicslist.EraseAllPortConnections();
                        break;

                    case DrawColumnTraySection dcts:
                        dcts.EraseAllPortCnnections();
                        break;
                }

                UnitOperation uo = dobj.AttachedModel;

                switch (uo)
                {
                    case Column c:
                        foreach (Port p in c.Ports)
                            p.StreamPort.ClearConnections();

                        foreach (TraySection section in c.TraySections)
                            foreach (Tray tray in section)
                                foreach (Port p in tray.Ports)
                                    p.StreamPort.ClearConnections();
                        break;

                    case null:
                        break;

                    default:
                        foreach (Port p in uo.Ports)
                        {
                            p.ClearChangeEventHandlers();
                            p.ClearConnections();
                        }
                        break;
                }
            }
        }

        private void ReconnectPortConnections(List<DrawBaseStream> streams)
        {
            List<DrawObject> AttachedModels = new();

            foreach (DrawBaseStream stream in streams)
            {
                Port StreamPort = null;
                Port StartModelPort = null;
                Port EndModelPort = null;

                DrawObject startDrawObj = null, endDrawObj = null;
                UnitOperation StartUnitOP = null, EndUnitOp = null;

                Node streamStartNode = stream.StartNode;
                Node streamEndNode = stream.EndNode;

                switch (stream)
                {
                    case DrawEnergyStream des:
                        {
                            if (des.Stream != null)
                                des.Stream.Name = stream.Name;

                            startDrawObj = des.startDrawObject;
                            endDrawObj = des.endDrawObject;

                            StreamPort = des.Port;

                            if (startDrawObj is not null)
                            {
                                switch (startDrawObj)
                                {
                                    case DrawColumnTraySection dcts:
                                        if (des.StartDrawTray is not null)
                                            StartModelPort = des.StartDrawTray.Tray.Ports[streamStartNode.PortGuid];
                                        break;

                                    case DrawTray:
                                        break;

                                    case DrawPA p:
                                    case DrawColumnCondenser:
                                    case DrawColumnReboiler:
                                        break;

                                    default:
                                        StartUnitOP = startDrawObj.AttachedModel;
                                        if (streamStartNode is not null)
                                            StartModelPort = StartUnitOP.Ports[streamStartNode.PortGuid];
                                        else
                                            MessageBox.Show("Error in Port Connection" + StartUnitOP.Name + ":" + des.Name);
                                        break;
                                }

                                StartModelPort.ConnectPort(des.Stream);
                            }

                            if (endDrawObj is not null)
                            {
                                switch (endDrawObj)
                                {
                                    case DrawColumnTraySection dcts:
                                        if (des.EndDrawTray is not null)
                                            EndModelPort = des.EndDrawTray.Tray.Ports[streamStartNode.PortGuid];
                                        break;

                                    case DrawTray:
                                        break;

                                    case DrawPA p:
                                    case DrawColumnCondenser:
                                    case DrawColumnReboiler:
                                        break;

                                    default:
                                        EndUnitOp = endDrawObj.AttachedModel;
                                        if (streamEndNode is not null)
                                            EndModelPort = EndUnitOp.Ports[streamEndNode.PortGuid];
                                        else
                                            MessageBox.Show("Error in Port Connection" + EndUnitOp.Name + ":" + des.Name);
                                        break;
                                }

                                EndModelPort.ConnectPort(des.Stream);
                            }
                            break;
                        }

                    case DrawSignalStream dss:
                        {
                            if (dss.Stream != null)
                                dss.Stream.Name = stream.Name;  // update engine.stream name

                            startDrawObj = dss.startDrawObject;
                            endDrawObj = dss.endDrawObject;

                            if (startDrawObj is not null)
                            {
                                StartUnitOP = startDrawObj.AttachedModel;
                                StartModelPort = StartUnitOP.Ports[streamStartNode.PortGuid];
                            }
                            if (endDrawObj is not null)
                            {
                                EndUnitOp = endDrawObj.AttachedModel;
                                EndModelPort = EndUnitOp.Ports[streamEndNode.PortGuid];
                            }

                            if (StartModelPort is not null && EndModelPort is not null)
                            {
                                if (startDrawObj is DrawSet || startDrawObj is DrawAdjust)
                                {
                                    StartModelPort.ConnectPort(EndModelPort);
                                }
                                else
                                {
                                    EndModelPort.ConnectPort(StartModelPort);
                                }
                            }
                            break;
                        }

                    case DrawMaterialStream dms:
                        {
                            if (dms.Stream != null)
                                dms.Stream.Name = stream.Name;  // update engine.stream name

                            startDrawObj = dms.startDrawObject;
                            endDrawObj = dms.endDrawObject;

                            StreamPort = dms.Port;

                            if (startDrawObj is not null)
                            {
                                switch (startDrawObj)
                                {
                                    case DrawColumnTraySection dcts:
                                        if (dms.StartDrawTray is not null)
                                            StartModelPort = dms.StartDrawTray.Tray.Ports[streamStartNode.PortGuid];
                                        break;

                                    case DrawTray t:
                                        if (streamStartNode is not null)
                                            StartModelPort = t.Tray.Ports[streamStartNode.PortGuid];
                                        break;

                                    case DrawPA p:
                                    case DrawColumnCondenser:
                                    case DrawColumnReboiler:
                                        break;

                                    default:
                                        StartUnitOP = startDrawObj.AttachedModel;
                                        if (StartUnitOP is not null && streamStartNode is not null)
                                            StartModelPort = StartUnitOP.Ports[streamStartNode.PortGuid];
                                        //else
                                        //  MessageBox.Show("Error in Port Connection" + StartUnitOP.Name + ":" + dms.Name);
                                        break;
                                }

                                if (StartModelPort is not null)
                                    StartModelPort.ConnectPort(dms.Stream);
                            }

                            if (endDrawObj is not null)
                            {
                                switch (endDrawObj)
                                {
                                    case DrawColumnTraySection dcts:
                                        if (dms.EndDrawTray is not null && streamStartNode is not null)
                                        {
                                            EndModelPort = dms.EndDrawTray.Tray.Ports[streamEndNode.PortGuid];
                                        }
                                        break;

                                    case DrawTray t:
                                        if (streamEndNode is not null)
                                            EndModelPort = dms.EndDrawTray.Tray.Ports[streamEndNode.PortGuid];
                                        break;

                                    case DrawPA p:
                                    case DrawColumnCondenser:
                                    case DrawColumnReboiler:
                                        break;

                                    case DrawColumn dc:
                                        //dms.Port.StreamPortValueChanged -= dc.Column.TriggerPort_MainPortValueChanged;
                                        dms.Port.PropertyChangedHandler += dc.Column.TriggerPort_MainPortValueChanged;
                                        break;

                                    default:
                                        EndUnitOp = endDrawObj.AttachedModel;
                                        if (EndUnitOp is not null && streamEndNode is not null)
                                        {
                                            EndModelPort = EndUnitOp.Ports[streamEndNode.PortGuid];
                                        }
                                        //else
                                        // MessageBox.Show("Error in Port Connection" + EndUnitOp.Name + ":" + streamEndNode.Name);
                                        break;
                                }
                                if (EndModelPort is not null)
                                    EndModelPort.ConnectPort(dms.Stream);
                            }
                            break;
                        }
                }

                if ((startDrawObj != null) && !AttachedModels.Contains(startDrawObj))
                    AttachedModels.Add(startDrawObj);

                if ((endDrawObj != null) && !AttachedModels.Contains(endDrawObj))
                    AttachedModels.Add(endDrawObj);
            }

            foreach (DrawObject model in AttachedModels)
            {
                model.UpdateAttachedModel();
            }
        } // ports only
    }
}