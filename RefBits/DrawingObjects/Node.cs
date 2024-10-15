using Extensions;
using Main.DrawingObjects.Streams;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Units
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class Node : ISerializable
    {
        private bool isstripper = false;

        private Guid GUID = Guid.NewGuid();

        private Guid portGuid;
        private HotSpotOwnerType OwnerType;
        private object owner = null;
        private Guid attachedstreamguid = Guid.Empty;
        private bool isinput = false;
        private NodeDirections linedirection = NodeDirections.Right;
        private Color colour = Color.Black;
        private HotSpotType type;
        private DataStore datastore = new DataStore();  // private int floatY;

        public bool IsFistChanged
        {
            get { return false; }
        }

        internal DrawMaterialStream GetAttachedStream(GraphicsList gl)
        {
            return gl.ReturnStream(AttachedStreamGuid);
        }

        public void UpdateVariable(UOMProperty var)
        {
            switch (var.Property)
            {
                case ePropID.T: { break; }
                case ePropID.P: { break; }
            }
        }

        public Guid AttachedStreamGuid
        {
            get { return attachedstreamguid; }
            set { attachedstreamguid = value; }
        }

        public Guid Guid
        {
            get { return GUID; }
            set { GUID = value; }
        }

        private static int counter;

        public static int Counter
        {
            get { return Node.counter; }
            set { Node.counter = value; }
        }

        private bool isconnected = false;

        public bool IsConnected
        {
            get { return isconnected; }
            set { isconnected = value; }
        }

        public bool HasStream
        {
            get
            {
                if (attachedstreamguid == Guid.Empty)
                {
                    return false;
                }
                else return true;
            }
        }

        internal object Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public bool IsStripper
        {
            get { return isstripper; }
            set { isstripper = value; }
        }

        public Node Clone()
        {
            return new Node();
        }

        public bool Contains(Point p)
        {
            Rectangle test = Absolute;
            test.Inflate(1, 1);
            return test.Contains(p);
        }

        public NodeDirections LineDirection
        {
            get { return linedirection; }
            set { linedirection = value; }
        }

        public StreamDirections StreamDirection
        {
            get
            {
                StreamDirections nd = StreamDirections.ForwardOut;

                if (NodeType == HotSpotType.Floating)
                    return nd;

                if (relObjectPosition.X == 0 || relObjectPosition.X == 1) // either left or right, not top or bottom
                {
                    if (this.relObjectPosition.X < 0.5)  // left
                    {
                        if (isinput)  // left and input
                            nd = StreamDirections.ForwardIn;
                        else
                            nd = StreamDirections.BackwardOut;
                    }
                    else  // right
                    {
                        if (isinput)  // right and output
                            nd = StreamDirections.BackwardIn;
                        else
                            nd = StreamDirections.ForwardOut;
                    }
                }
                else  // top or bottom
                {
                    if (relObjectPosition.Y > 0.5 && isinput)
                        nd = StreamDirections.BotIn;
                    else if (relObjectPosition.Y > 0.5 && !isinput)
                        nd = StreamDirections.BotOut;
                    else if (isinput)
                        nd = StreamDirections.TopIn;
                    else
                        nd = StreamDirections.TopOut;
                }

                if (owner is DrawRectangle dr)
                    nd = RotateDirection(nd, dr.Angle, dr.FlipHorizontal, dr.FlipVertical);

                return nd;
            }
        }

        public StreamDirections RotateDirection(StreamDirections nd, enumRotationAngle Angle, bool FlipHorizontal, bool FlipVertical)
        {
            switch (nd)
            {
                case StreamDirections.TopIn:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.ForwardIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BotIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BackwardIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.TopIn;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BackwardIn:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.BotIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.ForwardIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.TopIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BackwardIn;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.ForwardIn:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.TopIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BackwardIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BotIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.ForwardIn;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BotIn:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.ForwardIn;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.TopIn;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BackwardIn;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BotIn;
                            break;

                        default:
                            break;
                    }
                    break;

                case StreamDirections.TopOut:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.ForwardOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BotOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BackwardOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.TopOut;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BackwardOut:

                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.TopOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.ForwardOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.BotOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BackwardOut;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.ForwardOut:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.BotOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.BackwardOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.TopOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.ForwardOut;
                            break;

                        default:
                            break;
                    }

                    break;

                case StreamDirections.BotOut:
                    switch (Angle)
                    {
                        case enumRotationAngle.a90:
                            nd = StreamDirections.BackwardOut;
                            break;

                        case enumRotationAngle.a180:
                            nd = StreamDirections.TopOut;
                            break;

                        case enumRotationAngle.a270:
                            nd = StreamDirections.ForwardOut;
                            break;

                        case enumRotationAngle.a0:
                            nd = StreamDirections.BotOut;
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            if (FlipHorizontal && nd == StreamDirections.ForwardIn)
                nd = StreamDirections.BackwardIn;
            else if (FlipVertical && nd == StreamDirections.BackwardIn)
                nd = StreamDirections.ForwardIn;

            if (FlipVertical && nd == StreamDirections.TopIn)
                nd = StreamDirections.BotIn;
            else if (FlipVertical && nd == StreamDirections.BotIn)
                nd = StreamDirections.TopIn;

            if (FlipHorizontal && nd == StreamDirections.ForwardOut)
                nd = StreamDirections.BackwardOut;
            else if (FlipVertical && nd == StreamDirections.BackwardOut)
                nd = StreamDirections.ForwardOut;

            if (FlipVertical && nd == StreamDirections.TopOut)
                nd = StreamDirections.BotOut;
            else if (FlipVertical && nd == StreamDirections.BotOut)
                nd = StreamDirections.TopOut;

            return nd;
        }

        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string attachedstreamname = "";
        private FixablePoint point2;

        // private int floatX;

        public string AttachedStreamName
        {
            get { return attachedstreamname; }
            set { attachedstreamname = value; }
        }

        internal void UpdateStreamName(GraphicsList gl)
        {
            DrawMaterialStream ds = gl.ReturnAttachedStream(this);
            if (ds != null)
                attachedstreamname = ds.Name;
        }

        public bool IsInput
        {
            get { return isinput; }
            set { isinput = value; }
        }

        public Node()
        {
            counter++;
            //hotspotID = GetHashCode();
            colour = Color.DarkBlue;
        }

        public Node(float X, float Y, string Name)
        {
            counter++;
            //hotspotID = counter;

            this.Name = Name;
            relObjectPosition.X = X;
            relObjectPosition.Y = Y;
            colour = Color.DarkBlue;
        }

        internal Node(object d, float X, float Y, string Name, NodeDirections LineDirection, HotSpotType NodeType, HotSpotOwnerType hsotype)
        {
            counter++;

            OwnerType = hsotype;

            relObjectPosition.X = X;
            relObjectPosition.Y = Y;
            
            owner = d;

            switch (NodeType)
            {
                case HotSpotType.Feed:
                    this.colour = Color.GreenYellow;
                    this.isinput = true;
                    break;

                case HotSpotType.FeedRight:
                    this.colour = Color.GreenYellow;
                    this.isinput = true;
                    break;

                case HotSpotType.LiquidDraw:
                    colour = Color.Black;
                    isinput = false;
                    break;

                case HotSpotType.Stream:
                    colour = Color.Blue;
                    this.isinput = false;
                    break;

                case HotSpotType.Water:
                    colour = Color.Blue;
                    this.isinput = false;
                    break;

                case HotSpotType.Floating:
                    this.absoluteposition.X = (int)X;
                    this.absoluteposition.Y = (int)Y;
                    colour = Color.Blue;
                    this.isinput = false;
                    break;

                case HotSpotType.SignalIn:
                    colour = Color.Blue;
                    this.isinput = true;
                    break;

                case HotSpotType.SignalOut:
                    colour = Color.Blue;
                    this.isinput = false;
                    break;
            }
            this.Name = Name;
            this.LineDirection = LineDirection;
            this.NodeType = NodeType;
        }

        public Guid PortGuid { get => portGuid; set => portGuid = value; }
        public HotSpotType NodeType { get => type; set => type = value; }
        public DataStore Datastore { get => datastore; set => datastore = value; }
        public HotSpotDisplayType HotSpotDisplayType { get; set; }
        public Color Color { get => colour; set => colour = value; }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            si.AddValue("ID", this.GUID);
            si.AddValue("RelativeObjectPosition", this.Relative);
            si.AddValue("Name", this.Name);
            si.AddValue("LineDirection", this.LineDirection);
            si.AddValue("IsConnected", this.IsConnected);
            si.AddValue("type", this.NodeType);
            si.AddValue("IsInput", this.IsInput);
            si.AddValue("OwnerType", OwnerType);
            si.AddValue("StreamGuid", attachedstreamguid);
            si.AddValue("portguid", portGuid);
            si.AddValue("AbsolutePosition", this.absoluteposition);
        }

        public Node(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                GUID = (Guid)si.GetValue("ID", typeof(Guid));
                Relative = (PointF)si.GetValue("RelativeObjectPosition", typeof(PointF));
                name = (string)si.GetValue("Name", typeof(string));
                linedirection = (NodeDirections)si.GetValue("LineDirection", typeof(NodeDirections));
                isconnected = (bool)si.GetValue("IsConnected", typeof(bool));
                NodeType = (HotSpotType)si.GetValue("type", typeof(HotSpotType));
                IsInput = (bool)si.GetValue("IsInput", typeof(bool));
                this.OwnerType = (HotSpotOwnerType)si.GetValue("OwnerType", typeof(HotSpotOwnerType));
                attachedstreamguid = (Guid)si.GetValue("StreamGuid", typeof(Guid));
                portGuid = (Guid)si.GetValue("portguid", typeof(Guid));
                absoluteposition = (Point)si.GetValue("AbsolutePosition", typeof(Point));
            }
            catch
            {
            }
        }

        public Node(FixablePoint point)
        {
            this.absoluteposition.X = point.X;
            this.absoluteposition.Y = point.Y;
            Guid = Guid.NewGuid();
        }
    }

    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class NodeCollection : IEnumerable
    {
        private List<Node> hotspots = new List<Node>();

        public IEnumerator GetEnumerator()
        {
            return hotspots.GetEnumerator();
        }

        public Rectangle LastHotSpot
        {
            get
            {
                if (hotspots.Count > 0)
                    return (hotspots[hotspots.Count - 1].Absolute);
                else
                    return new Rectangle();
            }
        }

        public Rectangle FirstHotSpot
        {
            get
            {
                if (hotspots.Count > 0)
                    return (hotspots[0].Absolute);
                else
                    return new Rectangle();
            }
        }

        internal void UpdateAttachedStreamNames(GraphicsList gl)
        {
            foreach (Node hs in HotSpotList)
            {
                hs.UpdateStreamName(gl);
            }
        }

        public void RepositionHotSpots(Rectangle columnrectangle)
        {
            float TrayHeight;

            if (hotspots.Count == 0)
                return;

            if (hotspots.Count == 1)
                TrayHeight = columnrectangle.Height;
            else
                TrayHeight = columnrectangle.Height / (hotspots.Count);

            int width = columnrectangle.Width;

            for (int n = 0; n < hotspots.Count; n++)
            {
                Rectangle r = hotspots[n].Absolute;
                r.Y = Convert.ToInt32(columnrectangle.Y + n * TrayHeight);
                r.X = columnrectangle.X;
                r.Height = Convert.ToInt32(TrayHeight);
                r.Width = width;
                //hotspots[n].rectangle = r;
            }
        }

        public bool Contains(Point p)
        {
            foreach (Node t in hotspots)
            {
                if (t.Contains(p))
                    return true;
            }
            return false;
        }

        public NodeCollection GetProductStreams()
        {
            NodeCollection hss = new NodeCollection();

            for (int n = 0; n < hotspots.Count; n++)
            {
                if (!hotspots[n].IsInput)
                {
                    hss.Add(hotspots[n]);
                }
            }
            return hss;
        }

        public NodeCollection GetFeeds()
        {
            NodeCollection hs = new NodeCollection();
            Node hs1;

            for (int n = 0; n < hotspots.Count; n++)
            {
                hs1 = hotspots[n];
                if (hs1.IsInput && hs1.IsConnected)
                {
                    hs.Add(hotspots[n]);
                }
            }
            return hs;
        }

        public NodeCollection GetProducts()
        {
            NodeCollection hs = new NodeCollection();
            Node hs1;

            for (int n = 0; n < hotspots.Count; n++)
            {
                hs1 = hotspots[n];
                if (!hs1.IsInput && hs1.IsConnected)
                {
                    hs.Add(hotspots[n]);
                }
            }
            return hs;
        }

        public Node GetHotSpotFromPoint(Point p)
        {
            foreach (Node t in hotspots)
            {
                if (t != null && !t.IsConnected)
                {
                    using GraphicsPath gp = new();
                    Matrix m = new();
                    //m.Scale(zoom, zoom);
                    //m.Translate(offsetx, offsety, MatrixOrder.Append);

                    gp.AddRectangle(t.Centered());
                    //gp.Transform(m);

                    if (gp.IsVisible(p))
                        return t;
                }
            }
            return null;
        }

        public void SetOwner(object o)
        {
            foreach (Node t in hotspots)
                if (t != null)
                    t.Owner = o;
        }

        public int CountValidTemps()
        {
            int count = 0;
            for (int n = 0; n < hotspots.Count; n++)
            {
                //if (hotspots[n].T.IsValid)
                count++;
            }
            return count;
        }

        public Node this[int index]
        {
            get
            {
                if (index >= HotSpotList.Count)
                    return null;
                else
                    return hotspots[index];
            }

            set
            {
                hotspots[index] = value;
            }
        }

        public Node this[string index]
        {
            get
            {
                foreach (Node hs in hotspots)
                {
                    if (hs.Name == index)
                        return hs;
                }

                return null;
            }
        }

        public bool Contains(Guid guid)
        {
            foreach (Node hs in hotspots)
            {
                if (hs.Guid == guid)
                    return true;
            }

            return false;
        }

        public List<Node> HotSpotList
        {
            get { return hotspots; }
            set { hotspots = value; }
        }

        public void ReNumber()
        {
        }

        public void Clear()
        {
            hotspots.Clear();
        }

        public object GetList()
        {
            return hotspots;
        }

        public Node Add(Node t)
        {
            hotspots.Add(t);
            return t;
        }

        public void AddRange(List<Node> t)
        {
            hotspots.AddRange(t);
        }

        public void AddRange(NodeCollection t)
        {
            hotspots.AddRange(t.hotspots);
        }

        public void Insert(int index, Node t)
        {
            hotspots.Insert(index, t);
            ReNumber();
        }

        public void Sort()
        {
            hotspots.Sort();
        }

        public int Count
        {
            get
            {
                return hotspots.Count;
            }
        }

        public int IndexOf(Node t)
        {
            return hotspots.IndexOf(t);
        }

        public void Remove(Node t)
        {
            if (hotspots.Count > 1)
            {
                hotspots.Remove(t);
                ReNumber();
            }
        }

        public void RemoveAt(int index)
        {
            if (hotspots.Count > 1)
            {
                hotspots.RemoveAt(index);
                ReNumber();
            }
        }

        public Node Search(Guid NodeID)
        {
            Node hs = hotspots.Find(delegate (Node bk)
            {
                if (bk != null)
                {
                    return bk.Guid == NodeID;
                }
                else
                {
                    return false;
                }
            });
            return hs;
        }

        public Node Search(String Name)
        {
            Node hs = hotspots.Find(delegate (Node bk)
            {
                return bk.Name == Name;
            });
            return hs;
        }

        internal void ClearConnected()
        {
            foreach (Node node in hotspots)
            {
                node.IsConnected = false;
            }
        }
    }
}