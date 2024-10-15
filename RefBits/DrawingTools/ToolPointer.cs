using Extensions;
using Main.DrawingObjects.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Units.UOM;


public enum FrontBack { Front, Back, None}

namespace Units
{
    /// <summary>
    /// Point er tool
    /// </summary>
    internal class ToolPointer : Tool
    {
        private enum SelectionMode
        {
            None,
            NetSelection,   // group selection is active
            Move,           // object(s) are moves
            Size,           // object is resized
            Drag,
            StreamSegmentMove,
            Connect,
            ReConnect
        }

        private enum HandleType
        { StreamStart, StreamEnd, StreamNull, Input, Output }

        private SelectionMode selectMode = SelectionMode.None;

        // Object which is currently resized:
        private DrawObject firstObject, previousDrawobject;

        private DrawObject LastOverObject, OverObject;

        private BaseSegment Segment;
        private int Handle;
        private FrontBack HandlePosition;
        private Node FirstNode;
        private HandleType StreamHandleType = HandleType.StreamNull, StreamConnectionType;

        // Keep state about last and current point  (used to move and resize objects)
        private Point lastPoint = new(0, 0);

        private Point startNetPoint = new(0, 0);
        private Point endNetPoint = new(0, 0);
        private Point LastStartPoint;
        private Point LastEndOPoint;

        private CommandChangeState commandChangeState;
        private bool wasMove;
        private bool updateconnections = true;
        private Node OldNode = null;

        public ToolPointer()
        { }

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, DrawMouseEventArgs e)
        {
            updateconnections = false;
            if (e.Button == MouseButtons.Left)
            {
                commandChangeState = null;
                wasMove = false;

                startNetPoint = drawArea.PointToScreen(e.MouseXY);
                LastStartPoint = startNetPoint;
                LastEndOPoint = startNetPoint;

                if (selectMode != SelectionMode.NetSelection)
                    selectMode = SelectionMode.None;

                // Test for resizing (only if control is selected, cursor is on the handle)
                firstObject = drawArea.returnObject(e.ModXY);

                if (firstObject is null) // stream takes precendence if two objects in same location
                {
                    if (firstObject is DrawTray tray)
                        firstObject = tray.Owner;

                    if (drawArea.ColumnDesigncontrol != null
                        && drawArea.ColumnDesigncontrol.PropertyGrid1 != null)
                        drawArea.ColumnDesigncontrol.PropertyGrid1.SelectedObject = drawArea;
                    else if (drawArea.Owner != null
                        && drawArea.Owner.propertiesgrid != null
                        && drawArea.Owner.propertiesgrid.propertyGrid != null)
                        drawArea.Owner.propertiesgrid.propertyGrid.SelectedObject = drawArea;
                }
                else
                {
                    if (drawArea.ColumnDesigncontrol != null
                        && drawArea.ColumnDesigncontrol.PropertyGrid1 != null)
                        drawArea.ColumnDesigncontrol.PropertyGrid1.SelectedObject = firstObject;
                    else if (drawArea.Owner != null
                        && drawArea.Owner.propertiesgrid != null && drawArea.Owner.propertiesgrid.propertyGrid != null)
                        drawArea.Owner.propertiesgrid.propertyGrid.SelectedObject = firstObject;
                }

                if (firstObject is not null)
                {
                    switch (firstObject.HitTest(e.ModXY))
                    {
                        case HitType.DataNode:
                            break;

                        case HitType.ObjectHandle:          // Resize
                            {
                                drawArea.GraphicsList.UnselectAll(); // Since we want to resize only one object, unselect all other objects
                                firstObject.Selected = true;
                                selectMode = SelectionMode.Size;
                                //commandChangeState = new  CommandChangeState(drawArea.GraphicsList);
                                drawArea.Cursor = Cursors.Hand;
                                Handle = firstObject.GetRotatedHandleNumber(e.ModXY); // d we need to run this again ???
                                break;
                            }

                        case HitType.StreamHandle:          // Move Stream Handle
                            {
                                selectMode = SelectionMode.ReConnect;
                                Handle = firstObject.GetHandleNumber(e.ModXY);
                                HandlePosition = firstObject.GetHandlePosition(e.ModXY);
                                drawArea.Cursor = Cursors.Hand;
                                if (firstObject is DrawBaseStream dbs)
                                {
                                    if (Handle == 1)
                                        StreamHandleType = HandleType.StreamStart;
                                    else if (Handle == dbs.HandleCount)
                                        StreamHandleType = HandleType.StreamEnd;
                                }
                                break;
                            }

                        case HitType.Stream:                // Move Stream
                            {
                                if (firstObject is DrawBaseStream dms)
                                {
                                    FirstNode = firstObject.HitTestHotSpot(e.ModXY);
                                    selectMode = SelectionMode.Connect;
                                    drawArea.Cursor = Cursors.Hand;

                                    if (FirstNode is null)
                                    {
                                        drawArea.GraphicsList.UnselectAll();
                                        firstObject.Selected = true;
                                        selectMode = SelectionMode.StreamSegmentMove;

                                        Segment = dms.segArray.GetSegment(e.ModXY);

                                        if (firstObject is DrawMaterialStream stream)
                                        {
                                            if (Segment is SegmentV)
                                                drawArea.Cursor = Cursors.SizeWE;
                                            else
                                                drawArea.Cursor = Cursors.SizeNS;
                                        }
                                    }
                                }
                                break;
                            }

                        case HitType.Object:                // Move
                            {
                                if (!firstObject.Selected)
                                    drawArea.GraphicsList.UnselectAll();

                                firstObject.Selected = true;
                                if (firstObject is DrawRectangle rectangle)
                                    rectangle.InitRotatedRectangle();
                                selectMode = SelectionMode.Move;
                                drawArea.Cursor = Cursors.Hand;
                                break;
                            }

                        case HitType.StreamConnection:      // ConnectStream
                            {
                                selectMode = SelectionMode.Connect;
                                FirstNode = firstObject.HitTestHotSpot(e.ModXY);

                                if (drawArea.Owner != null && drawArea.Owner.propertiesgrid != null)
                                    drawArea.Owner.propertiesgrid.propertyGrid.SelectedObject = FirstNode;
                                else if (drawArea.ColumnDesigncontrol != null && drawArea.ColumnDesigncontrol.PropertyGrid1 != null)
                                    drawArea.ColumnDesigncontrol.PropertyGrid1.SelectedObject = FirstNode;

                                if (FirstNode.IsInput)
                                    StreamConnectionType = HandleType.Input;  // For Node Highlighting
                                else
                                    StreamConnectionType = HandleType.Output;

                                if (FirstNode != null)
                                    drawArea.Cursor = Cursors.Hand;

                                break;
                            }
                    }
                }

                switch (selectMode)
                {
                    case SelectionMode.None:// Test for move (cursor is on the object)
                        {
                            int n1 = drawArea.GraphicsList.Count;
                            IDrawObject o = null;
                            for (int i = 0; i < n1; i++)
                            {
                                if (drawArea.GraphicsList[i].HitTest(e.ModXY) == 0)
                                {
                                    o = drawArea.GraphicsList[i];
                                    if ((Control.ModifierKeys & Keys.Control) == 0 && !o.Selected)
                                        drawArea.GraphicsList.UnselectAll();

                                    o.Selected = true;
                                    break;
                                }
                            }
                            // Net selection
                            // click on background
                            if ((Control.ModifierKeys & Keys.Control) == 0)
                                drawArea.GraphicsList.UnselectAll();

                            selectMode = SelectionMode.NetSelection;

                            break;
                        }

                    case SelectionMode.StreamSegmentMove:
                        {
                            if (firstObject is DrawBaseStream dbs && dbs.HitTestHotSpot(e.MouseXY) != null)  // not dragging
                            {
                                if ((Control.ModifierKeys & Keys.Control) == 0 && !firstObject.Selected)
                                    drawArea.GraphicsList.UnselectAll();
                                // Select clicked object
                                firstObject.Selected = true;
                                Segment = dbs.segArray.GetSegment(e.ModXY);
                            }
                            break;
                        }
                }

                drawArea.Capture = true;
                drawArea.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                firstObject = drawArea.returnObject(e.ModXY);
                switch (firstObject)
                {
                    case DrawColumnTraySection dcts:
                        dcts.hotspotdisplaytype = HotSpotDisplayType.Outputs;
                        drawArea.Refresh();
                        break;
                }
            }
        }

        /// <summary>
        /// Mouse is moved.
        /// None button is pressed, or left button is pressed.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, DrawMouseEventArgs e)
        {
            int dx, dy;

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
            {
                wasMove = true;

                OverObject = drawArea.returnObject(e.ModXY, firstObject); // do not return the same object

                if (OverObject != null)
                {
                    Debug.Print(OverObject.Name);
                    LastOverObject = OverObject;
                }

                if (LastOverObject is not null && LastOverObject != OverObject)
                    LastOverObject.Displayhotspots = false;

                if (OverObject is not null && firstObject is DrawBaseStream)
                    OverObject.Displayhotspots = false;

                lastPoint = endNetPoint;
                endNetPoint = drawArea.PointToScreen(e.ModXY);

                dx = endNetPoint.X - lastPoint.X;
                dy = endNetPoint.Y - lastPoint.Y;

                if (e.Button != MouseButtons.Left)
                    selectMode = SelectionMode.None;

                switch (selectMode)
                {
                    case SelectionMode.Connect:
                        {
                            if (OverObject != null)
                            {
                                OverObject.Displayhotspots = true;
                                HitType htype = OverObject.HitTest(e.ModXY);

                                switch (htype)
                                {
                                    case HitType.StreamConnection:
                                        Node node = OverObject.HitTestHotSpot(e.ModXY);
                                        node.Color = Color.Red;
                                        OverObject.Displayhotspots = true;
                                        break;

                                    default:
                                        break;
                                }
                            }

                            if (firstObject is DrawMaterialStream)
                                firstObject.MoveHandleTo(e.MouseXY, Handle);

                            DrawArea.SetDirty();
                            drawArea.Refresh();
                            break;
                        }

                    case SelectionMode.ReConnect:
                        {
                            if (OverObject != null)
                            {
                                if (firstObject is not DrawMaterialStream)
                                {
                                    OverObject.Displayhotspots = true;
                                }

                                switch (StreamHandleType)
                                {
                                    case HandleType.StreamStart:
                                        OverObject.hotspotdisplaytype = HotSpotDisplayType.Outputs;
                                        break;

                                    case HandleType.StreamEnd:
                                        OverObject.hotspotdisplaytype = HotSpotDisplayType.Inputs;
                                        break;

                                    case HandleType.StreamNull:
                                        OverObject.hotspotdisplaytype = HotSpotDisplayType.All;
                                        break;
                                }

                                switch (StreamConnectionType)
                                {
                                    case HandleType.Input:
                                        OverObject.hotspotdisplaytype = HotSpotDisplayType.All;
                                        break;

                                    case HandleType.Output:
                                        OverObject.hotspotdisplaytype = HotSpotDisplayType.All;
                                        break;

                                    case HandleType.StreamNull:
                                        OverObject.hotspotdisplaytype = HotSpotDisplayType.All;
                                        break;
                                }

                                OverObject.Displayhotspots = true;
                                HitType htype = OverObject.HitTest(e.MouseXY);

                                switch (htype)
                                {
                                    case HitType.StreamConnection:
                                        Node node = OverObject.HitTestHotSpot(e.MouseXY);
                                        node.Color = Color.Red;
                                        OverObject.Displayhotspots = true;
                                        break;

                                    default:
                                        break;
                                }
                            }

                            switch (firstObject)
                            {
                                case DrawBaseStream dbs:
                                    dbs.MoveHandleTo(e.ModXY, HandlePosition);
                                    break;
                            }

                            DrawArea.SetDirty();
                            drawArea.Refresh();

                            break;
                        }

                    case SelectionMode.Size:
                        {
                            if (firstObject != null)
                            {
                                firstObject.MoveRotatedHandleTo(e.ModXY, Handle);
                                if (firstObject is DrawRectangle)
                                    drawArea.GraphicsList.UpdateStreams((DrawObject)firstObject);

                                DrawArea.SetDirty();
                                drawArea.Refresh();
                            }
                            break;
                        }

                    case SelectionMode.StreamSegmentMove:
                        {
                            if (firstObject is DrawBaseStream dbs)
                            {
                                int loc = dbs.segArray.IndexOf(Segment);
                                if (Segment is SegmentV segv && loc > 0 && loc < dbs.segArray.Count - 1)
                                {
                                    segv.X = e.ModXY.X;
                                    dbs.segArray.Update(segv);
                                    dbs.UpdateOrthogonals();
                                    DrawArea.SetDirty();
                                    drawArea.Refresh();
                                    drawArea.Cursor = Cursors.SizeWE;
                                }
                                else if (Segment is SegmentH segh && loc > 0 && loc < dbs.segArray.Count - 1)
                                {
                                    segh.Y = e.ModXY.Y;
                                    dbs.segArray.Update(segh);
                                    dbs.UpdateOrthogonals();
                                    DrawArea.SetDirty();
                                    drawArea.Refresh();
                                    drawArea.Cursor = Cursors.SizeNS;
                                }
                                if (firstObject is DrawMaterialStream dms)
                                {
                                    dms.DataNodeLocationUpdate();
                                }
                            }
                            DrawArea.SetDirty();
                            drawArea.Refresh();

                            break;
                        }

                    case SelectionMode.Move:
                        {
                            List<DrawObject> objects = drawArea.GraphicsList.GetSelected;

                            if (objects.Count == 1 && objects[0] is DrawName nameTag)
                            {
                                nameTag.RotatedMove((int)(dx), (int)(dy));
                                if (nameTag.Attachedobject != null)
                                {
                                    nameTag.Attachedobject.NameOffSetX = nameTag.Location.X - nameTag.Attachedobject.Rectangle.TopRight().X;
                                    nameTag.Attachedobject.NameOffSetY = nameTag.Location.Y - nameTag.Attachedobject.Rectangle.TopRight().Y;
                                }
                                else if (nameTag.Attachedstream != null)
                                {
                                    DrawBaseStream stream = nameTag.Attachedstream;
                                    Point center = stream.Center;
                                    center.Offset(nameTag.NameOffSetX, nameTag.NameOffSetY);

                                    stream.NameOffSetX = nameTag.Location.X - center.X;
                                    stream.NameOffSetY = nameTag.Location.Y - center.Y;
                                }
                            }
                            else
                            {
                                foreach (DrawObject o in drawArea.GraphicsList.Selection)
                                {
                                    //o.RotatedMove((int)(dx / drawArea.ScaleDraw), (int)(dy / drawArea.ScaleDraw));
                                    o.RotatedMove((int)(dx), (int)(dy));
                                    //o.RotatedMove((int )(dx / drawArea.Zoom), (int )(dy / drawArea.Zoom));
                                    switch (o)
                                    {
                                        case null: break;
                                        case DrawName nameTag1:
                                            /*    if (nameTag.Attachedobject != null)
                                                {
                                                    nameTag.Attachedobject.NameOffSetX = nameTag.Location.X - nameTag.Attachedobject.Rectangle.TopRight().X;
                                                    nameTag.Attachedobject.NameOffSetY = nameTag.Location.Y - nameTag.Attachedobject.Rectangle.TopRight().Y;
                                                }
                                                else if (nameTag.Attachedstream != null)
                                                {
                                                    DrawBaseStream stream = nameTag.Attachedstream;
                                                    Point center = stream.Center;
                                                    center.Offset(nameTag.NameOffSetX, nameTag.NameOffSetY);

                                                    stream.NameOffSetX = nameTag.Location.X - center.X;
                                                    stream.NameOffSetY = nameTag.Location.Y - center.Y;
                                                }*/
                                            break;

                                        case DrawRectangle dr:
                                            drawArea.GraphicsList.UpdateStreams(dr);
                                            break;

                                        case DrawBaseStream dbs1:
                                            //GraphicsList.UpdateStreams(dbs1, (int)(dx*drawArea.ScaleDraw), (int)(dy *drawArea.ScaleDraw));
                                            GraphicsList.UpdateStreams(dbs1, (int)(dx), (int)(dy));
                                            break;
                                    }
                                }
                            }
                            DrawArea.SetDirty();
                            drawArea.Refresh();
                            break;
                        }

                    case SelectionMode.NetSelection:
                        {
                            endNetPoint = drawArea.PointToScreen(e.MouseXY);
                            ControlPaint.DrawReversibleFrame(DrawRectangle.GetNormalizedRectangle(LastStartPoint, LastEndOPoint),   // Remove old selection rectangle
                                Color.Black, FrameStyle.Dashed);

                            ControlPaint.DrawReversibleFrame(DrawRectangle.GetNormalizedRectangle(startNetPoint, endNetPoint),   // Draw new  selection rectangle
                                Color.Black, FrameStyle.Dashed);
                            LastStartPoint = startNetPoint;
                            LastEndOPoint = endNetPoint;
                            return;
                        }

                    case SelectionMode.Drag:
                        {
                            if (e.Button != MouseButtons.Left)
                                return;

                            for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                            {
                                drawArea.GraphicsList[i].Displayhotspots = false;
                                HitType ht = drawArea.GraphicsList[i].HitTest(e.ModXY);
                                if (ht == HitType.Object)
                                    drawArea.GraphicsList[i].Displayhotspots = true;
                            }
                            drawArea.Refresh();
                            break;
                        }

                    case SelectionMode.None:
                        {
                            OverObject = drawArea.returnObject(e.ModXY);

                            if (OverObject is null)
                            {
                                if (previousDrawobject is not null)
                                {
                                    previousDrawobject.Displayhotspots = false;
                                    drawArea.Refresh();
                                }
                                break;
                            }
                            else
                            {
                                if (OldNode is not null)
                                    OldNode.Color = Color.Black;

                                previousDrawobject = OverObject;

                                HitType htype = OverObject.HitTest(e.ModXY);
                                switch (htype)
                                {
                                    case HitType.DataNode:
                                        OverObject.Displayhotspots = true;
                                        OverObject.Hotspotcolor = Color.White;
                                        drawArea.Refresh();
                                        break;

                                    case HitType.Object:
                                        OverObject.Displayhotspots = true;
                                        OverObject.Hotspotcolor = Color.DarkBlue;
                                        drawArea.Refresh();
                                        break;

                                    case HitType.StreamConnection:
                                        Node hs = OverObject.HitTestHotSpot(e.ModXY);
                                        hs.Color = Color.Red;
                                        OldNode = hs;
                                        OverObject.Displayhotspots = true;
                                        drawArea.Refresh();
                                        break;

                                    case HitType.Stream:
                                        hs = OverObject.HitTestHotSpot(e.ModXY);
                                        if (hs is not null)
                                        {
                                            hs.Color = Color.Red;
                                            OldNode = hs;
                                        }

                                        OverObject.Displayhotspots = true;
                                        drawArea.Refresh();
                                        break;

                                    default:
                                        break;
                                }

                                if (firstObject != previousDrawobject
                                    && firstObject is DrawBaseStream dbs1)
                                {
                                    switch (dbs1)
                                    {
                                        case DrawMaterialStream dms:
                                            drawArea.toolTip1.Active = true;
                                            drawArea.toolTip1.SetToolTip(drawArea,
                                                "Temp: " + ((Temperature)dms.Port.T_.UOM).Celsius.ToString("##.# ") + "C" + "\n"
                                                + "Press: " + dms.Port.P_.ToString("##.# ") + "BAR" + "\n"
                                                + "MassFlow: " + dms.Port.MF_.ToString("#####.# ") + "kg/hr" + "\n"
                                                + "VolFlow: " + dms.Port.VF_.ToString("#####.# ") + "m3/hr");
                                            break;

                                        case DrawEnergyStream des:
                                            drawArea.toolTip1.Active = true;
                                            drawArea.toolTip1.SetToolTip(drawArea,
                                                "Energy: " + des.EnergyFlow);
                                            break;
                                    }
                                }
                                else if (OverObject != null && OverObject != firstObject)
                                {
                                    //OverObject.Displayhotspots = false;
                                    drawArea.Refresh();
                                }
                            }
                        }
                        break;
                }
            }

            //drawArea.GraphicsList.UpdateAllConnections(); 

            drawArea.toolTip1.SetToolTip(drawArea, "X= " + e.MouseXY.X + " Y= " + e.MouseXY.Y
            + " StartNetX= " + startNetPoint.X + " StartNetY= " + startNetPoint.Y
            + " endNetPoint X= " + endNetPoint.X + " endNetPoint  Y= " + endNetPoint.Y
            + " Offset X= " + drawArea.OffsetX + " Offset Y= " + drawArea.OffsetY
            + " Scale = " + drawArea.ScaleDraw.ToString());
        }

        /// <summary>
        /// Right mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, DrawMouseEventArgs e)
        {
            Node node = null;
            Point ScreenstartNetPoint, ScreenendNetPoint;
            double zoom = drawArea.ScaleDraw;
            int offsetx = drawArea.OffsetX;
            int offsety = drawArea.OffsetY;

            ScreenstartNetPoint = drawArea.PointToClient(new(startNetPoint.X, startNetPoint.Y));
            ScreenendNetPoint = drawArea.PointToClient(new(endNetPoint.X, endNetPoint.Y));

            ScreenstartNetPoint = new((int)((ScreenstartNetPoint.X / zoom - offsetx)), (int)((ScreenstartNetPoint.Y / zoom - offsety)));
            ScreenendNetPoint = new((int)((ScreenendNetPoint.X / zoom - offsetx)), (int)((ScreenendNetPoint.Y / zoom - offsety)));

            DrawObject endObject = drawArea.returnObject(e.ModXY, firstObject);
            if (endObject is not null && firstObject != endObject)
                node = endObject.HitTestHotSpot(e.ModXY);

            if (e.Button == MouseButtons.Left)
            {
                StreamHandleType = HandleType.StreamNull;

                switch (selectMode)
                {
                    case SelectionMode.StreamSegmentMove:
                        {
                            if (firstObject is DrawBaseStream dbs)
                            {
                                int loc = dbs.segArray.IndexOf(Segment);
                                if (Segment is SegmentV segV && loc > 0 && loc < dbs.segArray.Count - 1)  // vertical segment moved
                                {
                                    segV.X = e.ModXY.X;
                                    if (loc > 0 && loc < dbs.segArray.Count - 1)
                                        segV.IsFixed = true;
                                    DrawArea.SetDirty();
                                    drawArea.Refresh();
                                    drawArea.Cursor = Cursors.SizeWE;
                                }
                                else if (Segment is SegmentH segH && loc > 0 && loc < dbs.segArray.Count - 1)// horizontal segment moved
                                {
                                    segH.Y = e.ModXY.Y;
                                    if (loc > 0 && loc < dbs.segArray.Count - 1) // dont fix last segment
                                        segH.IsFixed = true;
                                    DrawArea.SetDirty();
                                    drawArea.Refresh();
                                    drawArea.Cursor = Cursors.SizeNS;
                                }
                            }
                            DrawArea.SetDirty();
                            drawArea.Refresh();
                        }
                        break;

                    case SelectionMode.NetSelection:
                        {
                            // Remove old selection rectangle
                            ControlPaint.DrawReversibleFrame(DrawRectangle.GetNormalizedRectangle(startNetPoint, endNetPoint), Color.Black, FrameStyle.Dashed);

                            // Make group selection
                            drawArea.GraphicsList.SelectInRectangle(DrawRectangle.GetNormalizedRectangle(ScreenstartNetPoint, ScreenendNetPoint));

                            selectMode = SelectionMode.NetSelection;

                            if (drawArea.GraphicsList.Selection.Count == 1 && drawArea.Owner is not null)
                                drawArea.Owner.propertiesgrid.propertyGrid.SelectedObject = drawArea.GraphicsList.Selection[0];

                            //List<DrawObject> list = drawArea.GraphicsList.GetSelected;
                            //if (list.Count == 1 && drawArea.Owner.propertiesgrid!=null)
                            //    drawArea.Owner.propertiesgrid.propertyGrid.SelectedObject = list[0];
                            break;
                        }

                    case SelectionMode.None:
                        {
                            foreach (DrawObject o in drawArea.GraphicsList)
                                if (o.GetType().BaseType == typeof(DrawRectangle))
                                    drawArea.GraphicsList.UpdateStreams(o);
                            break;
                        }

                    case SelectionMode.Connect:
                        {
                            updateconnections = true;
                            firstObject.Displayhotspots = false;

                            HitType ht = HitType.None;

                            if (endObject != null)
                                ht = endObject.HitTest(e.ModXY);

                            switch (ht)
                            {
                                case HitType.StreamConnection: // create new  stream
                                    {
                                        Node FinalNode;
                                        Node StreamStartNode, StreamEndNode;

                                        selectMode = SelectionMode.None; // stop dragging
                                        if (endObject != null)
                                        {
                                            FinalNode = endObject.HitTestHotSpot(e.ModXY);

                                            if (FirstNode.IsInput)
                                            {
                                                StreamStartNode = FinalNode;
                                                StreamEndNode = FirstNode;
                                            }
                                            else
                                            {
                                                StreamStartNode = FirstNode;
                                                StreamEndNode = FinalNode;
                                            }

                                            if (StreamStartNode == StreamEndNode)
                                                break;

                                            if (FinalNode != null && !FinalNode.IsConnected)  // hotspot
                                            {
                                                DrawMaterialStream ds = new(FirstNode, FinalNode);  // Create new  stream
                                                AddnewObject(drawArea, ds);
                                                ds.CreateFlowsheetUOModel();
                                                endObject.Displayhotspots = false;

                                                ds.StartDrawObject = firstObject;
                                                ds.StartDrawObjectGuid = firstObject.Guid;

                                                ds.EndDrawObject = endObject;
                                                ds.EndDrawObjectGuid = endObject.Guid;

                                                ds.StartNodeGuid = StreamStartNode.Guid;
                                                ds.EndNodeGuid = StreamEndNode.Guid;

                                                StreamEndNode.AttachedStreamGuid = ds.Guid;
                                                StreamStartNode.AttachedStreamGuid = ds.Guid;
                                            }
                                        }
                                        break;
                                    }

                                case HitType.None: // e.g. the background draw area
                                    {
                                        Node FinalNode;
                                        Node StreamStartNode, StreamEndNode;
                                        DrawBaseStream ds;

                                        FinalNode = new(drawArea, e.ModifiedMouseXY.X, e.ModifiedMouseXY.Y, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);

                                        if (FirstNode.IsInput) // swap nodes over
                                        {
                                            StreamStartNode = FinalNode;
                                            StreamEndNode = FirstNode;
                                        }
                                        else
                                        {
                                            StreamStartNode = FirstNode;
                                            StreamEndNode = FinalNode;
                                        }

                                        if (Math.Abs(FirstNode.Absolute.X - FinalNode.X) > 10     // dont do if too close to first point
                                            || Math.Abs(FirstNode.Absolute.Y - FinalNode.Y) > 10)
                                        {
                                           

                                            switch (FirstNode.NodeType)
                                            {
                                                case HotSpotType.EnergyIn:
                                                case HotSpotType.EnergyOut:
                                                    ds = new DrawEnergyStream(StreamStartNode, StreamEndNode);
                                                    break;

                                                case HotSpotType.SignalIn:
                                                    ds = new DrawSignalStream(StreamStartNode, StreamEndNode);
                                                    break;

                                                case HotSpotType.SignalOut:
                                                    ds = new DrawSignalStream(StreamStartNode, StreamEndNode);
           
                                                    break;

                                                default:
                                                    ds = new DrawMaterialStream(StreamStartNode, StreamEndNode);

                                                    if(FirstNode.IsInput)
                                                    {
                                                        ds.endDrawObject = firstObject;
                                                        if (firstObject is not null)
                                                            ds.endDrawObjectGuid = firstObject.Guid;
                                                        ds.startDrawObject = endObject;
                                                        if (endObject is not null)
                                                            ds.StartDrawObjectGuid = endObject.Guid;
                                                    }
                                                    else
                                                    {
                                                        ds.endDrawObject = endObject;
                                                        if (endObject is not null)
                                                            ds.endDrawObjectGuid = endObject.Guid;
                                                        ds.startDrawObject = firstObject;
                                                        ds.StartDrawObjectGuid = firstObject.Guid;
                                                    }
                                                    


                                                    ds.UpdateOrthogonals();
                                                    drawArea.Refresh();

                                                    break;
                                            }

                                            AddnewObject(drawArea, ds);
                                            ds.CreateFlowsheetUOModel();
                                        }
                                        break;
                                    }
                            }
                            drawArea.GraphicsList.UnselectAll();
                            break;
                        }

                    case SelectionMode.ReConnect:
                        {
                            updateconnections = true;
                            DrawBaseStream drawStream = (DrawBaseStream)firstObject;
                            drawStream.isSolved = false;

                            firstObject.Displayhotspots = true;
                            drawArea.Refresh();

                            HitType hitType = HitType.None;

                            if (endObject != null && endObject is not DrawName)
                                hitType = endObject.HitTest(e.ModXY);

                            switch (hitType)
                            {
                                case HitType.DataNode:
                                    if (endObject is DrawMaterialStream ms)
                                        node = ms.TestForDataNode(e.ModXY);

                                    if (node != null)  // hotspot
                                    {
                                        if (Handle == 1)  // Start
                                        {
                                            drawStream.StartDrawObjectGuid = endObject.Guid;
                                            drawStream.StartNodeGuid = node.Guid;
                                            drawStream.StartRelPosition = node.Absolute.Location;
                                        }
                                        else if (Handle == drawStream.HandleCount)
                                        {
                                            drawStream.EndDrawObjectGuid = endObject.Guid;
                                            drawStream.EndNodeGuid = node.Guid;
                                            drawStream.EndRelativePosition = node.Absolute.Location;
                                            //  drawStream.AddEndPoints(endNode.LineDirection, drawStream.EndDirection);
                                        }
                                        drawStream.UpdateOrthogonals();
                                        node.AttachedStreamGuid = drawStream.Guid;
                                        endObject.Displayhotspots = false;
                                        drawArea.Refresh();
                                    }
                                    break;

                                case HitType.StreamConnection:
                                    if (node != null)  // hotspot
                                    {
                                        if (Handle == 1)  //Start
                                        {
                                            drawStream.StartDrawObjectGuid = endObject.Guid;
                                            drawStream.StartNodeGuid = node.Guid;
                                            drawStream.StartRelPosition = node.Absolute.Location;
                                        }
                                        else if (Handle == drawStream.HandleCount)
                                        {
                                            drawStream.EndDrawObjectGuid = endObject.Guid;
                                            drawStream.EndNodeGuid = node.Guid;
                                            drawStream.EndRelativePosition = node.Absolute.Location;
                                            // drawStream.AddEndPoints(node.LineDirection, drawStream.EndDirection);
                                        }
                                        node.AttachedStreamGuid = drawStream.Guid;
                                        endObject.Displayhotspots = false;
                                        drawStream.UpdateOrthogonals();
                                        drawArea.Refresh();
                                    }

                                    break;

                                case HitType.None: // e.g. disconnect
                                    //Node OldNodd
                                    node = new(drawArea, e.ModifiedMouseXY.X, e.ModifiedMouseXY.Y, "Floating", NodeDirections.Right, HotSpotType.Floating, HotSpotOwnerType.DrawArea);
                                    Node startnode, endnode;

                                    startnode = drawStream.StartNode; endnode = drawStream.EndNode;

                                    if (Handle == 1 && startnode is not null)
                                    {
                                        startnode.AttachedStreamGuid = Guid.Empty;
                                        startnode.AttachedStreamName = "";
                                        drawStream.StartNode = node;
                                        drawStream.StartNode.Guid = node.Guid;
                                        drawStream.startDrawObject = null;
                                        drawStream.startDrawObjectID = Guid.Empty; // must be empty, do not change
                                    }
                                    else if (Handle == drawStream.HandleCount && endnode is not null)
                                    {
                                        endnode.AttachedStreamGuid = Guid.Empty;
                                        endnode.AttachedStreamName = "";
                                        drawStream.EndNode = node;
                                        drawStream.EndNode.Guid = node.Guid;
                                        drawStream.endDrawObject = null;
                                        drawStream.endDrawObjectGuid = Guid.Empty; // must be empty
                                    }

                                    drawStream.UpdateOrthogonals();

                                    break;

                                case HitType.StreamHandle:
                                    drawArea.Refresh();
                                    break;
                            }
                            drawArea.GraphicsList.UnselectAll();
                            break;
                        }
                }

                drawArea.Capture = false;
                drawArea.Refresh();

                if (commandChangeState != null && wasMove)// Keep state after moving/resizing and add command to history
                {
                    commandChangeState.newState(drawArea.GraphicsList);
                    drawArea.AddCommandToHistory(commandChangeState);
                    commandChangeState = null;
                    foreach (DrawObject o in drawArea.GraphicsList.Selection)
                    {
                        if (o.GetType().BaseType == typeof(DrawRectangle))
                            drawArea.GraphicsList.UpdateStreams(o);
                    }
                }

                if (firstObject is DrawName dn && dn.Attachedobject != null)
                {
                    dn.Attachedobject.NameOffSetX = dn.Location.X - dn.Attachedobject.Rectangle.TopRight().X;
                    dn.Attachedobject.NameOffSetY = dn.Location.Y - dn.Attachedobject.Rectangle.TopRight().Y;
                }

                if (firstObject != null)
                {
                    // after resizing
                    firstObject.Normalize();
                    firstObject = null;
                }

                drawArea.Cursor = Cursors.Arrow;
            }

            selectMode = SelectionMode.None; // stop dragging
            FirstNode = null;

            if (updateconnections) // dont do every time a mouse is clicked
                drawArea.GraphicsList.UpdateAllConnections();

            updateconnections = false;
        }

        public override void OnDoubleClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            foreach (DrawObject o in drawArea.GraphicsList.Selection)  // only used if selected
                o.OnDoubleClick(drawArea, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
        }

        public override void OnMouseClick(DrawArea drawArea, DrawMouseEventArgs e)
        {
            DrawObject CurrentMouseOverObject = drawArea.returnObject(e.ModXY);

            if (CurrentMouseOverObject != null)
            {
                drawArea.lastobject = CurrentMouseOverObject;
                drawArea.LastMousePosition = e.MouseXY;
            }
        }

        protected static void AddnewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            DrawArea.SetDirty();

            o.DrawArea = drawArea;
        }

        public static bool IsOdd(int i)
        {
            Math.DivRem(i, 2, out int res);
            if (res == 0)
                return false;
            else
                return true;
        }
    }
}