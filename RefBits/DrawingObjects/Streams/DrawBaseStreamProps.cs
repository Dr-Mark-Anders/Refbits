using Extensions;
using Main.DrawingObjects.Streams;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;

//using PointList = System.Collections.Generic.List<Main.DrawingObjects.Streams.FixablePoint>;

namespace Units
{
    public partial class DrawBaseStream : DrawLine, IDrawStream, ISerializable
    {
        [Browsable(true), CategoryAttribute("GUIDS")]
        public Guid StartDrawTrayGuid { get => startDrawTrayID; set => startDrawTrayID = value; }

        [Browsable(true), CategoryAttribute("GUIDS")]
        public Guid EndDrawTrayID { get => endDrawTrayID; set => endDrawTrayID = value; }

        [Browsable(true), CategoryAttribute("GUIDS")]
        public Guid StartDrawObjectGuid { get => startDrawObjectID; set => startDrawObjectID = value; }

        [Browsable(true), CategoryAttribute("GUIDS")]
        public Guid EndDrawObjectGuid { get => endDrawObjectGuid; set => endDrawObjectGuid = value; }

        [Browsable(true), CategoryAttribute("GUIDS")]
        public Guid StartNodeGuid { get => startNodeGuid; set => startNodeGuid = value; }

        [Browsable(true), CategoryAttribute("GUIDS")]
        public Guid EndNodeGuid { get => endNodeGuid; set => endNodeGuid = value; }

        [Browsable(true), CategoryAttribute("OBJECTS")]
        public DrawObject StartDrawObject { get => startDrawObject; set => startDrawObject = value; }

        [Browsable(true), CategoryAttribute("OBJECTS")]
        public DrawObject EndDrawObject { get => endDrawObject; set => endDrawObject = value; }

        [Browsable(true), CategoryAttribute("OBJECTS")]
        public DrawColumnTraySection StartDrawTraySection { get => startDrawTraySection; set => startDrawTraySection = value; }

        [Browsable(true), CategoryAttribute("OBJECTS")]
        public DrawColumnTraySection EndDrawTraySection { get => endDrawTraySection; set => endDrawTraySection = value; }

        [Browsable(true), CategoryAttribute("NODES")]
        public virtual Node StartNode { get => startNode; set => startNode = value; }

        [Browsable(true), CategoryAttribute("NODES")]
        public virtual Node EndNode { get => endNode; set => endNode = value; }

        [Browsable(true), CategoryAttribute("OBJECTS")]
        public DrawTray StartDrawTray { get => startDrawTray; set => startDrawTray = value; }

        [Browsable(true), CategoryAttribute("OBJECTS")]
        public DrawTray EndDrawTray { get => endDrawTray; set => endDrawTray = value; }

        [Browsable(true), CategoryAttribute("EngineGuids")]
        public Guid EngineDrawSectionGuid
        {
            get
            {
                return engineDrawSection;
            }

            set
            {
                engineDrawSection = value;
            }
        }

        [Browsable(true), CategoryAttribute("EngineGuids")]
        public Guid EngineDrawTrayGuid
        {
            get
            {
                return engineDrawTray;
            }

            set
            {
                engineDrawTray = value;
            }
        }

        [Browsable(true), CategoryAttribute("EngineGuids")]
        public Guid EnginereturnSectionGuid

        {
            get
            {
                return enginereturnSection;
            }

            set
            {
                enginereturnSection = value;
            }
        }

        [Browsable(true), CategoryAttribute("EngineGuids")]
        public Guid EnginereturnTrayGuid
        {
            get
            {
                return enginereturnTray;
            }

            set
            {
                enginereturnTray = value;
            }
        }

        [Browsable(true)]
        public bool EndConnected
        {
            get
            {
                if (EndDrawObject is null)
                    return false;
                else
                    return true;
            }
        }

        [Browsable(true)]
        public bool FrontConnected
        {
            get
            {
                if (StartDrawObject is null)
                    return false;
                else
                    return true;
            }
        }

        [Browsable(false)]
        public Point GetLastHandlePosition
        {
            get
            {
                if (pointArray.Count() > 0)
                    return pointArray[^1];
                else
                    return new Point();
            }
        }

        [Browsable(false)]
        public Point GetFirstHandlePosition
        {
            get
            {
                if (pointArray.Count() > 0)
                    return pointArray[0];
                else
                    return new Point();
            }
        }

        [Browsable(false)]
        public int OffsetX { get => offsetX; set => offsetX = value; }

        [Browsable(false)]
        public int OffsetY { get => offsetY; set => offsetY = value; }

        [Browsable(false)]
        public bool Active { get => active; set => active = value; }

        [Browsable(false)]
        public override int HandleCount
        {
            get
            {
                return pointArray.Count();
            }
        }

        [Browsable(false)]
        public override Rectangle Rectangle
        {
            get
            {
                int TopLeftX, TopLeftY, BottomRightX, BottomRightY;
                if (pointArray is null || pointArray.Count() == 0)
                    return Rectangle.Empty;

                TopLeftX = pointArray[0].X;
                TopLeftY = pointArray[0].Y;
                BottomRightX = pointArray[0].X;
                BottomRightY = pointArray[0].Y;

                for (int i = 1; i < pointArray.Count(); i++)
                {
                    Point item = pointArray[i];
                    if (item.X < TopLeftX)
                        TopLeftX = item.X;
                    if (item.Y < TopLeftY)
                        TopLeftY = item.Y;
                    if (item.X > BottomRightX)
                        BottomRightX = item.X;
                    if (item.Y > BottomRightY)
                        BottomRightY = item.Y;
                }

                return new Rectangle(new Point(TopLeftX, TopLeftY), new Size(BottomRightX - TopLeftX, BottomRightY - TopLeftY));
            }
        }

        [Browsable(false)]
        public Point Center
        {
            get
            {
                return Rectangle.Center();
            }
        }

        [Browsable(false)]
        public SmoothingMode SmotthingMode
        {
            get
            {
                return smoothingmode;
            }
            set
            {
                smoothingmode = value;
            }
        }

        [Browsable(false)]
        public EndLineDirection EndDirection
        {
            get { return endDirection; }
            set { endDirection = value; }
        }

        [Browsable(false)]
        public string EndName
        {
            get { return endname; }
            set { endname = value; }
        }

        [Browsable(false)]
        public string StartName
        {
            get { return startname; }
            set { startname = value; }
        }

        [Browsable(false)]
        protected int LastSegmentSize
        {
            get { return lastSegmentSize; }
            set { lastSegmentSize = value; }
        }

        [Browsable(false)]
        public StartLineDirection StartDirection
        {
            get { return startdirection; }
            set { startdirection = value; }
        }

        [Browsable(false)]
        public int FirstSegmentSize
        {
            get { return firstSegmentSize; }
            set { firstSegmentSize = value; }
        }
    }
}