using ModelEngine;
using Extensions;
using ModelEngine;
using RusselColumn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    public class DrawLLEColumn : DrawRectangle, ISerializable
    {
        private LLEDLG cdlg;
        private HXSubFlowSheet subflowsheet = new();  // Objects added in main solve routine
        private LLESEP llesep = new();
        internal DrawArea columndrawarea;

        /// <summary>
        /// int Guid, ExtGuid
        /// </summary>
        internal Dictionary<Guid, Guid> StreamConnectionsGuidDic = new();

        internal DrawStreamCollection ExtFeedStreams = new();
        internal DrawStreamCollection intFeedStreams = new();
        internal DrawStreamCollection ExtProductStreams = new();
        internal DrawStreamCollection intProductStreams = new();
        internal DrawStreamCollection internalConnectingStreams = new();
        internal DrawStreamCollection intersectionNetStreams = new();
        internal DrawStreamCollection intersectionDrawStreams = new();
        internal DrawPACollection PAs = new();
        internal DrawColumnTraySectionCollection DrawColumnTraySections;
        internal DrawColumnCondenserCollection drawColumnCondensers = new();
        internal DrawColumnReboilerCollection drawColumnReboilers = new();

        internal void RefreshStreams()
        {
            subflowsheet.Name = this.Name;
            ExtFeedStreams = this.DrawArea.GraphicsList.ReturnExternalFeedStreams(this);
            intFeedStreams = this.graphicslist.ReturnInternalFeedStreams();
            ExtProductStreams = this.DrawArea.GraphicsList.ReturnExternalProductStreams(this);
            intProductStreams = this.graphicslist.ReturnInternalProductStreams();
            internalConnectingStreams = this.graphicslist.ReturnInternalConnectingStreams();
            intersectionNetStreams = this.graphicslist.ReturnIntersectionNetStreams();
            intersectionDrawStreams = this.graphicslist.ReturnIntersectionDrawStreams();
            drawColumnCondensers = this.graphicslist.ReturnCondensers();
            drawColumnReboilers = this.graphicslist.ReturnReboilers();

            foreach (DrawMaterialStream drawStream in internalConnectingStreams) // to handle petyluk configurations
            {
                if (drawStream.endDrawObject is DrawColumnTraySection)
                    drawStream.intersectionstream.isNetBottomConnectedFlow = true;
            }

            foreach (DrawMaterialStream drawStream in intFeedStreams)
            {
                drawStream.Sidestream.Name = drawStream.Name;
                drawStream.Stream.Name = drawStream.Name;
                // foreach (var port in drawStream.Stream.Ports)
                //   port.Owner = subflowsheet;
            }

            foreach (DrawMaterialStream drawStream in intProductStreams)
            {
                drawStream.Sidestream.Name = drawStream.Name;
                drawStream.Stream.Name = drawStream.Name;
                //foreach (var port in drawStream.Stream.Ports)
                //  port.Owner = subflowsheet;
            }

            PAs = this.graphicslist.ReturnAllPumpArounds();
        }

        public void ResetDrawArea()
        {
            columndrawarea.ResetDrawArea();
            List<DrawName> n = graphicslist.ReturnDrawNames();

            foreach (DrawName dn in n) // remove drawnames
                graphicslist.Remove(dn);

            graphicslist.AddNameObjects();
        }

        public bool Active
        {
            get
            {
                return llesep.IsActive;
            }
            set
            {
                llesep.IsActive = value;
                subflowsheet.IsActive = llesep.IsActive;
            }
        }

        public override bool UpdateAttachedModel()
        {
            // Sort by size
            subflowsheet.Name = this.Name;
            subflowsheet.IsActive = llesep.IsActive;
            llesep.Name = this.Name;
            ClearData();
            graphicslist.UpdateDrawObjects();
            RefreshStreams();
            DesignChanged();
            ConnectExtenalFeedsTointernalFeeds();
            ConnectinternalFeedsToFeedPorts();
            ConnectProductPortstointernalProducts();
            ConnectinternalProductsToExternalProducts();
            ConnectinternalConnectingStreamsToStages();

            ProcessSpecs();

            if (llesep.Specs.DegreesOfFreedom != 0)
                return false;

            return true;
        }

        internal DrawColumnTraySection GetColumnSectionFromCondenser()
        {
            foreach (DrawMaterialStream item in internalConnectingStreams)
            {
                DrawObject start = item.StartDrawObject;
                DrawObject end = item.EndDrawObject;

                if (start is DrawColumnTraySection dcts)
                    return dcts;

                if (end is DrawColumnTraySection dcts1)
                    return dcts1;
            }

            return null;
        }

        private void ProcessSpecs()
        {
            DrawMaterialStream drawStream;
            StreamMaterial stream;
            SideStream sideStream;
            ConnectingStream connectstream;
            DrawColumnTraySection drawsection;

            foreach (Specification spec in llesep.Specs)
            {
                switch (spec.graphicSpecType)
                {
                    case eSpecType.TrayDuty:
                        spec.engineSpecType = eSpecType.TrayDuty;
                        break;

                    case eSpecType.Temperature:
                        spec.engineSpecType = eSpecType.Temperature;
                        break;

                    case eSpecType.RefluxRatio:
                        spec.engineSpecType = eSpecType.RefluxRatio;
                        break;

                    case eSpecType.TrayNetVapFlow:
                        spec.engineSpecType = eSpecType.TrayNetVapFlow;
                        drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[spec.drawObjectGuid];
                        drawStream = this.intProductStreams[spec.drawStreamGuid];
                        if (drawStream != null)
                        {
                            connectstream = drawStream.intersectionstream;
                            connectstream.EngineDrawSection = drawsection.TraySection;
                            connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            connectstream.FlowEstimate = 0.01;
                            connectstream.InitialFlowEstimate = 0.01;
                        }
                        break;

                    case eSpecType.TrayNetLiqFlow:
                        spec.engineSpecType = eSpecType.TrayNetLiqFlow;
                        drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[spec.drawObjectGuid];
                        drawStream = this.intProductStreams[spec.drawStreamGuid];
                        if (drawStream != null)
                        {
                            connectstream = drawStream.intersectionstream;
                            connectstream.EngineDrawSection = drawsection.TraySection;
                            connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            connectstream.FlowEstimate = 0.1;
                            connectstream.InitialFlowEstimate = 0.1;
                        }
                        break;

                    case eSpecType.LiquidStream:
                        spec.engineSpecType = eSpecType.LiquidStream;

                        drawStream = this.internalConnectingStreams[spec.drawObjectGuid];

                        drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[drawStream.startDrawObjectID];

                        if (drawStream != null)

                        {
                            spec.engineStageGuid = drawStream.EndDrawTray.Guid;
                            spec.engineSectionGuid = drawsection.TraySection.Guid;

                            stream = drawStream.Stream;
                            connectstream = drawStream.intersectionstream;
                            spec.connectedStream = connectstream;

                            drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[spec.drawSectionGuid];

                            if (connectstream != null && drawsection != null)

                            {
                                connectstream.EngineDrawSection = drawsection.TraySection;
                                connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            }
                        }

                        break;

                    case eSpecType.VapStream:
                        spec.engineSpecType = eSpecType.VapStream;

                        drawStream = this.internalConnectingStreams[spec.drawObjectGuid];
                        drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[drawStream.startDrawObjectID];

                        if (drawStream != null)
                        {
                            spec.engineStageGuid = drawStream.EndDrawTray.Guid;
                            spec.engineSectionGuid = drawsection.TraySection.Guid;

                            stream = drawStream.Stream;
                            connectstream = drawStream.intersectionstream;
                            spec.connectedStream = connectstream;
                            drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[spec.drawSectionGuid];

                            if (connectstream != null && drawsection != null)
                            {
                                connectstream.EngineDrawSection = drawsection.TraySection;
                                connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            }
                        }
                        break;

                    case eSpecType.LiquidProductDraw:
                        TraySection ts;
                        SideStream ss;
                        spec.engineSpecType = eSpecType.LiquidProductDraw;

                        drawStream = this.intProductStreams[spec.drawStreamGuid];

                        if (drawStream != null)
                        {
                            stream = drawStream.Stream;

                            switch (drawStream.startDrawObject)
                            {
                                case DrawColumnTraySection dcts:
                                    ss = drawStream.Sidestream;
                                    drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.StartDrawObjectGuid];
                                    spec.engineStageGuid = drawsection[drawStream.StartDrawTrayGuid].Tray.Guid;
                                    spec.engineSectionGuid = drawsection.TraySection.Guid;
                                    spec.engineObjectguid = drawsection.TraySection.Guid;
                                    spec.sideStreamGuid = ss.Guid;
                                    if (drawStream.StartNode.NodeType == HotSpotType.TrayNetLiquid)
                                        spec.engineSpecType = eSpecType.TrayNetLiqFlow;
                                    else
                                        spec.engineSpecType = eSpecType.LiquidProductDraw;
                                    break;

                                case DrawColumnCondenser _:
                                    drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.SectionNumber];
                                    ts = drawsection.TraySection;

                                    if (ts != null)
                                        spec.engineSectionGuid = ts.Guid;

                                    ss = drawStream.Sidestream;
                                    spec.sideStreamGuid = ss.Guid;
                                    spec.engineSectionGuid = ts.Guid;
                                    spec.engineObjectguid = ts.Guid;

                                    if (ss != null && ss.EngineDrawTray != null)
                                        spec.engineStageGuid = ss.EngineDrawTray.Guid;
                                    break;

                                case DrawTray _:
                                    drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.startDrawObjectID];
                                    ts = drawsection.TraySection;

                                    if (ts != null)
                                        spec.engineSectionGuid = ts.Guid;

                                    ss = drawStream.Sidestream;
                                    spec.sideStreamGuid = ss.Guid;
                                    spec.engineSectionGuid = ts.Guid;
                                    spec.engineObjectguid = ts.Guid;

                                    if (ss != null && ss.EngineDrawTray != null)
                                        spec.engineStageGuid = ss.EngineDrawTray.Guid;
                                    break;
                            }
                        }
                        break;

                    case eSpecType.VapProductDraw:
                        drawStream = this.intProductStreams[spec.drawObjectGuid];
                        if (drawStream is null)
                            continue;
                        drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.StartDrawObjectGuid];

                        spec.engineSpecType = eSpecType.VapProductDraw;
                        spec.engineStageGuid = drawStream.StartDrawTray.Tray.Guid;
                        //spec.engineSectionGuid = drawStream.StartDrawObject Section.Guid;

                        if (drawStream != null)
                        {
                            stream = drawStream.Stream;
                            sideStream = llesep.VapourSideStreams[spec.engineObjectguid];
                            sideStream.EngineDrawTray = llesep[drawStream.SectionNumber][drawStream.EngineDrawTrayGuid];
                        }
                        break;

                    case eSpecType.PAFlow:
                        spec.engineSpecType = eSpecType.PAFlow;
                        DrawPA drawpa = this.PAs[spec.drawObjectGuid];

                        if (drawpa != null)
                        {
                            PumpAround pa = llesep.PumpArounds[drawpa.Name];
                            if (pa != null)
                            {
                                DrawTray dt = (DrawTray)this.DrawColumnTraySections[drawpa.DrawSectionNo][drawpa.DrawTrayDrawGuid];
                                pa.drawTray = dt.Tray;

                                drawsection = (DrawColumnTraySection)this.DrawColumnTraySections[drawpa.ReturnSectionNo];

                                if (drawsection != null)
                                    dt = (DrawTray)drawsection[drawpa.ReturnTrayDrawGuid];

                                pa.returnSection = llesep[drawpa.ReturnSectionNo];
                                spec.engineSectionGuid = pa.returnSection.Guid;

                                spec.PAguid = pa.Guid;
                            }
                        }
                        break;

                    case eSpecType.PARetT:
                        spec.engineSpecType = eSpecType.PARetT;

                        drawpa = this.PAs[spec.drawObjectGuid];
                        if (drawpa != null)
                        {
                            PumpAround p = llesep.PumpArounds[drawpa.Name];
                            DrawTray dt = (DrawTray)this.DrawColumnTraySections[drawpa.DrawSectionNo][drawpa.DrawTrayDrawGuid];
                            p.drawTray = dt.Tray;

                            dt = this.DrawColumnTraySections[drawpa.ReturnSectionNo][drawpa.ReturnTrayDrawGuid];

                            p.returnSection = llesep[drawpa.ReturnSectionNo];
                            spec.engineSectionGuid = p.returnSection.Guid;

                            spec.PAguid = p.Guid;
                        }
                        break;

                    case eSpecType.Energy:
                        spec.engineSpecType = eSpecType.Energy;
                        break;

                    case eSpecType.PADeltaT:
                        spec.engineSpecType = eSpecType.PADeltaT;
                        break;

                    case eSpecType.PADuty:
                        spec.engineSpecType = eSpecType.PADuty;
                        break;

                    case eSpecType.DistSpec:
                        spec.engineSpecType = eSpecType.DistSpec;
                        drawStream = this.intProductStreams[spec.drawStreamGuid];
                        if (drawStream != null)
                        {
                            stream = drawStream.Stream;

                            switch (drawStream.startDrawObject)
                            {
                                case DrawColumnTraySection dcts:
                                    ss = drawStream.Sidestream;
                                    drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.StartDrawObjectGuid];
                                    spec.engineStageGuid = drawsection[drawStream.StartDrawTrayGuid].Tray.Guid;
                                    spec.engineSectionGuid = drawsection.TraySection.Guid;
                                    spec.engineObjectguid = drawsection.TraySection.Guid;
                                    spec.sideStreamGuid = ss.Guid;
                                    if (drawStream.StartNode.NodeType == HotSpotType.TrayNetLiquid)
                                        spec.engineSpecType = eSpecType.TrayNetLiqFlow;
                                    else
                                        spec.engineSpecType = eSpecType.LiquidProductDraw;
                                    break;

                                case DrawColumnCondenser _:
                                    drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.SectionNumber];
                                    ts = drawsection.TraySection;

                                    if (ts != null)
                                        spec.engineSectionGuid = ts.Guid;

                                    ss = drawStream.Sidestream;
                                    spec.sideStreamGuid = ss.Guid;
                                    spec.engineSectionGuid = ts.Guid;
                                    spec.engineObjectguid = ts.Guid;

                                    if (ss != null)
                                    {
                                        if (ss.EngineDrawTray != null)
                                            spec.engineStageGuid = ss.EngineDrawTray.Guid;
                                    }
                                    break;

                                case DrawTray _:
                                    drawsection = (DrawColumnTraySection)DrawColumnTraySections[drawStream.startDrawObjectID];
                                    ts = drawsection.TraySection;

                                    if (ts != null)
                                        spec.engineSectionGuid = ts.Guid;

                                    ss = drawStream.Sidestream;
                                    spec.sideStreamGuid = ss.Guid;
                                    spec.engineSectionGuid = ts.Guid;
                                    spec.engineObjectguid = ts.Guid;

                                    if (ss != null)
                                    {
                                        if (ss.EngineDrawTray != null)
                                            spec.engineStageGuid = ss.EngineDrawTray.Guid;
                                    }
                                    break;
                            }
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        public void DesignChanged()
        {
            graphicslist.UpdateDrawObjects();

            DrawColumnTraySections = (DrawColumnTraySectionCollection)graphicslist.ReturnTraySections();
            DrawColumnTraySections.Sort();
            int count;

            foreach (DrawColumnTraySection dts in this.DrawColumnTraySections)
            {
                count = 1;
                foreach (DrawTray tray in dts)
                {
                    tray.Name = "Tray: " + count;
                    count++;
                }
            }

            TraySection trayCollection;

            if (DrawColumnTraySections.Count == 0)
                return;

            llesep.TraySections = new(llesep, false);

            foreach (DrawColumnTraySection dcts in DrawColumnTraySections)
            {
                trayCollection = (TraySection)dcts.TraySection;
                trayCollection.Name = dcts.Name;
                llesep.TraySections.Add(trayCollection);
            }

            llesep.LiquidSideStreams.Clear();
            llesep.VapourSideStreams.Clear();

            foreach (DrawMaterialStream stream in intProductStreams)
            {
                stream.UpdateSideDraws(DrawColumnTraySections);

                if (stream.Startnode(graphicslist) != null)
                {
                    switch (stream.Startnode(graphicslist).NodeType)
                    {
                        case HotSpotType.LiquidDraw:
                            if (stream.StartNode.Owner is not DrawColumnReboiler)
                                llesep.LiquidSideStreams.Add(stream.Sidestream);
                            break;

                        case HotSpotType.VapourDraw:
                            if (stream.StartNode.Owner is not DrawColumnCondenser)
                                llesep.VapourSideStreams.Add(stream.Sidestream);
                            break;
                    }
                }
            }

            llesep.ConnectingDraws.Clear();
            llesep.ConnectingNetFlows.Clear();

            foreach (DrawMaterialStream drawstream in intersectionNetStreams)
            {
                drawstream.UpdateEngineInterConnectStream(DrawColumnTraySections);

                if (drawstream.intersectionstream.FlowEstimate <= 0)
                    drawstream.intersectionstream.FlowEstimate = 0.01;

                if (double.IsNaN(drawstream.intersectionstream.FlowEstimate))
                    drawstream.intersectionstream.FlowEstimate = 0.01;

                if (drawstream.StartDrawObject != null && drawstream.StartNode != null)
                    switch (drawstream.StartNode.NodeType)
                    {
                        case HotSpotType.TrayNetLiquid:
                            drawstream.intersectionstream.FlowEstimate = 0.1;
                            drawstream.intersectionstream.isliquid = true;
                            llesep.ConnectingNetFlows.Add(drawstream.intersectionstream);
                            break;

                        case HotSpotType.TrayNetVapour:
                            drawstream.intersectionstream.FlowEstimate = 0.01;
                            drawstream.intersectionstream.isliquid = false;
                            llesep.ConnectingNetFlows.Add(drawstream.intersectionstream);
                            break;
                    }
            }

            foreach (DrawMaterialStream stream in intersectionDrawStreams)
            {
                stream.UpdateEngineInterConnectStream(DrawColumnTraySections);

                if (stream.StartDrawObject != null && stream.StartNode != null)
                    switch (stream.StartNode.NodeType)
                    {
                        case HotSpotType.LiquidDraw:
                            stream.intersectionstream.isliquid = true;
                            stream.FlowEstimate = 0.1;
                            llesep.ConnectingDraws.Add(stream.intersectionstream);
                            break;

                        case HotSpotType.VapourDraw:
                            stream.FlowEstimate = 0.01;
                            stream.intersectionstream.isliquid = false;
                            llesep.ConnectingDraws.Add(stream.intersectionstream);
                            break;
                    }
            }

            llesep.PumpArounds.Clear();
            PumpAround pumparound;
            DrawTray drawtray, returntray;
            DrawColumnTraySection dcts1, rcts1;

            foreach (DrawPA pa in PAs)
            {
                dcts1 = (DrawColumnTraySection)DrawColumnTraySections[pa.DrawSectionNo];
                rcts1 = (DrawColumnTraySection)DrawColumnTraySections[pa.ReturnSectionNo];

                pumparound = llesep.PumpArounds.Add(pa.PumpAround);

                if (pa.feed is not null)
                {
                    drawtray = (DrawTray)pa.feed.StartNode.Owner;
                    pa.DrawTrayDrawGuid = drawtray.Guid;
                    if (drawtray is not null)
                        pumparound.drawTray = drawtray.Tray;
                }

                if (pa.effluent is not null)
                {
                    returntray = (DrawTray)pa.effluent.EndNode.Owner;
                    pa.ReturnTrayDrawGuid = returntray.Guid;
                    if (returntray != null)
                        pumparound.returnTray = returntray.Tray;
                }

                if (pumparound is not null)
                {
                    pumparound.drawSection = llesep.TraySections[pa.DrawSectionNo];
                    pumparound.returnSection = llesep.TraySections[pa.ReturnSectionNo];
                    pumparound.Name = pa.Name;
                }
            }

            foreach (Specification spec in llesep.Specs)
            {
                switch (spec.engineSpecType)
                {
                    case eSpecType.TrayNetLiqFlow:
                    case eSpecType.TrayNetVapFlow:
                    case eSpecType.RefluxRatio:
                    case eSpecType.Temperature:
                    case eSpecType.Energy:
                        spec.engineObjectguid = llesep.Guid;
                        break;
                }
            }
        }

        private static int Count;

        public DrawLLEColumn() : this(0, 0, 50, 300)
        {
            /* RC = new  Russel();
             RC.UpdateIteration1 += new  IterationEventHAndler(IterationCompleted1);
             RC.UpdateIteration2 += new  IterationEventHAndler(IterationCompleted2);*/
        }

        public DrawLLEColumn(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            columndrawarea = new();

            try
            {
                graphicslist = (GraphicsList)info.GetValue("graphicslist", typeof(GraphicsList));
                StreamConnectionsGuidDic = (Dictionary<Guid, Guid>)info.GetValue("StreamConnections", typeof(Dictionary<Guid, Guid>));
                llesep = (LLESEP)info.GetValue("column", typeof(LLESEP));
                //cd.CalcHandler += new  EventHandler(Calculate);
            }
            catch
            {
            }
            llesep.Name = this.Name;
            columndrawarea.Name = "Column Draw Area" + this.Name;
            subflowsheet.IsActive = llesep.IsActive;
        }

        public DrawLLEColumn(int x, int y, int width, int height) : base()
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            Hotspots.Add(new Node(this, 0f, 0.1f, "FEED1", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.2f, "FEED2", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.3f, "FEED3", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.4f, "FEED4", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.5f, "FEED5", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.6f, "FEED6", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.7f, "FEED7", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.8f, "FEED8", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0f, 0.9f, "FEED9", NodeDirections.Left, HotSpotType.Feed, HotSpotOwnerType.DrawRectangle));

            Hotspots.Add(new Node(this, 0.5f, 0f, "VAPPRODUCT", NodeDirections.Up, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.1f, "PRODUCT1", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.2f, "PRODUCT2", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.3f, "PRODUCT3", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.4f, "PRODUCT4", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.5f, "PRODUCT5", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.6f, "PRODUCT6", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.7f, "PRODUCT7", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.8f, "PRODUCT8", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.9f, "PRODUCT9", NodeDirections.Right, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 0.5f, 1f, "BOTPRODUCT", NodeDirections.Down, HotSpotType.LiquidDraw, HotSpotOwnerType.DrawRectangle));

            Initialize();

            this.Name = "C" + Count.ToString();
            columndrawarea = new("Column Draw Area" + this.Name);

            DrawColumnTraySection dcts = new(this.columndrawarea);
            dcts.Rectangle = new(200, 200, 50, 200);
            graphicslist.Add(dcts);

            Count++;
        }

        public FlowSheet ColumnFlowSheet
        {
            get
            {
                return this.subflowsheet;
            }
        }

        private void ClearData()
        {
            if (cdlg != null)
            {
                cdlg.Error1.Clear();
                cdlg.Error2.Clear();
            }
            llesep.ErrHistory1 = "";
            llesep.ErrHistory2 = "";
        }

        internal void ConnectExtenalFeedsTointernalFeeds()
        {
            RefreshStreams();

            foreach (DrawMaterialStream extFeed in ExtFeedStreams)
            {
                // extFeed.PortOut.ConnectedPorts.Clear();
                if (StreamConnectionsGuidDic.TryGetValue(extFeed.Guid, out Guid intGuid))
                {
                    DrawMaterialStream intFeed = intFeedStreams[intGuid];
                    if (intFeed != null && intFeed.Port.ConnectedPortNext is not null)
                    {
                        intFeed.Port.ConnectedPortNext.ClearConnections();
                        //  int Feed.Port.ConnectPorts(extFeed.PortOut);
                    }
                }
            }
        }

        internal void ConnectinternalProductsToExternalProducts()
        {
            RefreshStreams();

            foreach (DrawMaterialStream exProd in ExtProductStreams)
            {
                if (exProd.Port.ConnectedPortNext is not null)
                {
                    exProd.Port.ConnectedPortNext.ClearConnections();
                    if (StreamConnectionsGuidDic.TryGetValue(exProd.Guid, out Guid intGuid))
                    {
                        DrawMaterialStream intProduct = intProductStreams[intGuid];
                        if (intProduct != null)
                        {
                            //  int Product.PortOut.ConnectedPorts.Clear();
                            //  int Product.PortOut.ConnectPorts(exProd.Port);
                        }
                    }
                }
            }
        }

        internal void ConnectinternalFeedsToFeedPorts()
        {
            RefreshStreams();

            foreach (DrawMaterialStream intFeed in intFeedStreams)
            {
                DrawObject doobj;

                if (intFeed != null)
                {
                    intFeed.ConnectedPorts.Clear();
                    doobj = this.graphicslist.GetObject(intFeed.EndDrawObjectGuid, false);

                    switch (doobj)
                    {
                        case DrawColumnTraySection dcts:
                            DrawTray drawtray = dcts[intFeed.EndDrawTrayID];

                            if (drawtray != null && intFeed.Port.ConnectedPortNext is not null)
                            {
                                drawtray.Tray.feed.ConnectedPortNext.ClearConnections();
                                //  drawtray.Tray.feed.ConnectPorts(int Feed.PortOut);
                                drawtray.Tray.feed.Owner = llesep;
                            }
                            break;
                    }
                }
            }
        }

        internal void ConnectProductPortstointernalProducts()
        {
            RefreshStreams();
            DrawObject obj = null;

            foreach (DrawMaterialStream intProduct in intProductStreams)
            {
                if (intProduct != null)
                {
                    obj = (DrawObject)this.graphicslist.GetObject(intProduct.StartDrawObjectGuid);
                }

                switch (obj)
                {
                    case DrawColumnTraySection a:
                        Node n = a.GetNode(intProduct.StartNodeGuid);
                        if (n.Owner is DrawTray)
                        {
                            DrawTray drawtray = a.GetTray(intProduct.StartDrawTrayGuid);

                            switch (n.Name)
                            {
                                case "BOTPRODUCT":
                                    a.DrawTrays.Last().Tray.TrayLiquid.ConnectPort(intProduct.Stream);
                                    intProduct.Port.Owner = this.llesep;
                                    break;

                                case "VAPPRODUCT":
                                    a.DrawTrays[0].Tray.TrayVapour.ConnectPort(intProduct.Stream);
                                    intProduct.Port.Owner = this.llesep;
                                    break;

                                default:
                                    drawtray.Tray.liquidDrawRight.ConnectPort(intProduct.Stream);
                                    intProduct.Port.Owner = this.llesep;
                                    break;
                            }
                        }
                        else
                        {
                            switch (n.Name)
                            {
                                case "BOTPRODUCT":
                                    a.DrawTrays.Last().Tray.TrayLiquid.ConnectPort(intProduct.Stream);
                                    break;

                                case "VAPPRODUCT":
                                    a.DrawTrays[0].Tray.TrayVapour.ConnectPort(intProduct.Stream);
                                    break;
                            }
                        }
                        break;

                    case DrawColumnReboiler r:

                        switch (r.GetNode(intProduct.StartNodeGuid).Name)
                        {
                            case "FEED":
                                llesep.MainTraySection.BottomTray.feed.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.llesep;
                                break;

                            case "Reboil Vap":
                                llesep.MainTraySection.BottomTray.vapourDraw.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.llesep;
                                break;

                            case "LiqProduct":
                                llesep.MainTraySection.BottomTray.liquidDrawRight.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.llesep;
                                break;
                        }
                        llesep.MainTraySection.BottomTray.TrayLiquid.ConnectPort(intProduct.Stream);
                        intProduct.Port.Owner = this.llesep;
                        break;

                    case DrawColumnCondenser c:
                        switch (c.GetNode(intProduct.StartNodeGuid).Name)
                        {
                            case "VapProduct":
                                llesep.MainTraySection.TopTray.TrayVapour.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.llesep;
                                break;

                            case "LiqProduct":
                                llesep.MainTraySection.TopTray.liquidDrawRight.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.llesep;
                                break;

                            case "Water":
                                llesep.MainTraySection.TopTray.WaterDraw.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.llesep;
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// also does pumparounds
        /// </summary>
        internal void ConnectinternalConnectingStreamsToStages()
        {
            RefreshStreams();
            DrawObject obj = null;

            foreach (DrawMaterialStream intStream in internalConnectingStreams)
            {
                if (intStream != null)
                    obj = this.graphicslist.GetObject(intStream.StartDrawObjectGuid);

                if (intStream.EndDrawObject is DrawPA drawPA)
                {
                    PumpAround pa = drawPA.PumpAround;
                    if (intStream.StartNode.NodeType is HotSpotType.LiquidDrawLeft)
                    {
                        //pa.DrawPort = intStream.StartDrawTray.Tray.liquidDrawLeft;
                        pa.drawTray = intStream.StartDrawTray.Tray;
                       // intStream.StartDrawTray.Tray.liquidDrawLeft.IsPADraw = true;
                        intStream.StartDrawTray.Tray.liquidDrawRight.IsPADraw = false;
                    }
                    else
                    {
                        //pa.DrawPort = intStream.StartDrawTray.Tray.liquidDrawRight;
                        pa.drawTray = intStream.StartDrawTray.Tray;
                        intStream.StartDrawTray.Tray.liquidDrawRight.IsPADraw = true;
                       // intStream.StartDrawTray.Tray.liquidDrawRight.IsPADraw = false;
                    }
                }

                switch (obj)
                {
                    case DrawColumnTraySection a:
                        DrawTray drawtray = a.GetTray(intStream.StartDrawTrayGuid);
                        if (drawtray is null)
                        {
                            Node n = a.GetNode(intStream.StartNodeGuid);

                            switch (n.Name)
                            {
                                case "BOTPRODUCT":
                                    a.DrawTrays.Last().Tray.TrayLiquid.ConnectPort(intStream.Stream);
                                    break;

                                case "VAPPRODUCT":
                                    a.DrawTrays[0].Tray.TrayVapour.ConnectPort(intStream.Stream);
                                    break;
                            }
                        }
                        else
                        {
                            switch (intStream.StartNode.NodeType)
                            {
                                case HotSpotType.LiquidDrawLeft:
                                    drawtray.Tray.liquidDrawRight.ConnectPort(intStream.Stream);
                                    break;

                                case HotSpotType.LiquidDraw:
                                    drawtray.Tray.liquidDrawRight.ConnectPort(intStream.Stream);
                                    break;
                            }
                        }
                        break;

                    case DrawColumnReboiler r:
                        switch (r.GetNode(intStream.StartNodeGuid).Name)
                        {
                            case "VapProduct":
                                llesep.MainTraySection.BottomTray.TrayVapour.ConnectPort(intStream.Stream);
                                break;
                        }
                        break;

                    case DrawColumnCondenser c:
                        switch (c.GetNode(intStream.StartNodeGuid).Name)
                        {
                            case "Reflux":
                                llesep.MainTraySection.TopTray.TrayLiquid.ConnectPort(intStream.Stream);
                                break;
                        }
                        break;

                    case DrawPA:
                        {
                            //intStream.PortIn.H=pa.PumpAround.return  Port;
                        }
                        break;
                }
            }
        }

        public DrawColumnTraySection MainSection
        {
            get
            {
                DrawColumnTraySectionCollection DrawColumnTraySections = (DrawColumnTraySectionCollection)graphicslist.ReturnTraySections();
                if (DrawColumnTraySections.Count > 0)
                {
                    DrawColumnTraySection res = (DrawColumnTraySection)DrawColumnTraySections[0];
                    foreach (DrawColumnTraySection item in DrawColumnTraySections)
                    {
                        if (item.NoTrays > res.NoTrays)
                            res = item;
                    }

                    return res;
                }
                return null;
            }
        }

        private void IterationCompleted1(object sender, IterationEventArgs e)
        {
            if (cdlg != null && !cdlg.IsDisposed)
                cdlg.UpdateIteration1(e.data);
        }

        private void IterationCompleted2(object sender, IterationEventArgs e)
        {
            if (cdlg != null && !cdlg.IsDisposed)
                cdlg.UpdateIteration2(e.data);
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawLLEColumn drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

        [Category("Calculation"), Description("IsDirty")]
        public bool IsDirty
        {
            get
            {
                return llesep.IsDirty;
            }
            set
            {
                llesep.IsDirty = value;
            }
        }

        [Category("Calculation"), Description("IsSolved")]
        public bool IsSolved
        {
            get
            {
                return llesep.IsSolved;
            }
            set
            {
                //llesep.IsSolved = value;
            }
        }

        public ThermoDynamicOptions ThermoOptions
        {
            get
            {
                if (llesep != null)
                    return llesep.Thermo;
                else
                    return null;
            }
            set
            {
                if (llesep != null)
                    llesep.Thermo = value;
            }
        }

        public GraphicsList ParentGraphicsList
        {
            get
            {
                return DrawArea.GraphicsList;
            }
        }

        public Color PenColor { get; private set; }
        public bool Isdirty { get; internal set; }

        public GraphicsList graphicslist
        {
            get
            {
                return columndrawarea.GraphicsList;
            }
            set
            {
                columndrawarea.GraphicsList = value;
            }
        }

        public LLESEP Column { get => llesep; set => llesep = value; }
        public HXSubFlowSheet SubFlowSheet { get => subflowsheet; set => subflowsheet = value; }

        public void Calculate(object sender, EventArgs e)
        {
            if (llesep.Specs.DegreesOfFreedom != 0)
            {
                MessageBox.Show("Degress of Freedom Must be 0");
                return;
            }

            llesep.Err1 = "";
            llesep.Err2 = "";
            llesep.ErrHistory1 = "";
            llesep.ErrHistory2 = "";

            UpdateAttachedModel();

            llesep.IsDirty = true;
            subflowsheet.IsDirty = true;
            subflowsheet.ModelStack.SetDirty();
            DrawArea.SetUpModelStack(GlobalModel.Flowsheet);
            DrawArea.SolveAsync();

            if (llesep.IsSolved)
            {
                PenColor = Color.Black;
                cdlg.FillPressureData();
                cdlg.FillProfileData(this.llesep);

                TransferTraysToProductStreams();
                TransferTraysToPumpAroundStreams();

                cdlg.FillProfileData(llesep);
                cdlg.FillDiagnosticData(llesep);
                cdlg.UpdateSpecs(this);
                cdlg.Refresh();
                return;
            }
            else
            {
                PenColor = Color.Red;
                cdlg.FillDiagnosticData(llesep);
                cdlg.ClearOldResults();
                llesep.SolutionConverged = false;
                IsSolved = false;
            }

            PostSolve();
        }

        public override void PostSolve()
        {
            foreach (DrawObject stream in graphicslist)
            {
                if (stream is DrawMaterialStream drawstream && drawstream.StartDrawObject is DrawPA pA)
                {
                    DrawPA drawPA = pA;
                    PumpAround pa = drawPA.PumpAround;
                    StreamMaterial sm = pa.StreamOut;

                    drawstream.Port.H_ = sm.Port.H_;
                    drawstream.Port.P_ = sm.Port.P_;
                    drawstream.Port.MolarFlow_ = sm.Port.MolarFlow_;
                    drawstream.Port.ComponentList = (List<BaseComp>)sm.Port.ComponentList.Clone();
                    drawstream.Port.cc.Origin = SourceEnum.UnitOpCalcResult;
                    drawstream.Port.H_.Source = SourceEnum.UnitOpCalcResult;
                    drawstream.Port.P_.Source = SourceEnum.UnitOpCalcResult;
                    drawstream.Port.MolarFlow_.Source = SourceEnum.UnitOpCalcResult;

                    drawstream.Port.Flash();
                }
            }
            if (cdlg != null)
                cdlg.loaddata();
        }

        public void TransferTraysToProductStreams()
        {
            DrawColumnTraySectionCollection DrawColumnTraySections = (DrawColumnTraySectionCollection)graphicslist.ReturnTraySections();
            DrawColumnTraySection DrawMainSection = (DrawColumnTraySection)DrawColumnTraySections[0];
            DrawMaterialStream extstream;

            foreach (DrawMaterialStream stream in intProductStreams)
            {
                if (StreamConnectionsGuidDic.TryGetValue(stream.Guid, out Guid ExtStreamGuid))
                {
                    extstream = ExtProductStreams[ExtStreamGuid];
                    DrawTray tray = (DrawTray)DrawMainSection.GetTray(stream.Guid);
                }
            }
        }

        public static void TransferTraysToPumpAroundStreams()
        {
            /*  Guid ExtStreamGuid;
              DrawTray tray;
              DrawColumnTraySectionCollection DrawColumnTraySections = graphicslist.returnTraySections();
              DrawColumnTraySection DrawMainSection = DrawColumnTraySections[0];
              DrawStream extstream;

              foreach (DrawPA pa in PAs)
              {
                  extstream.Stream = pa.
                  if (StreamConnectionsGuidDic.TryGetValue(stream.Guid, out ExtStreamGuid))
                  {
                      extstream = ExtProductStreams[ExtStreamGuid];
                      tray = DrawMainSection.GetTray(stream.Guid);
                  }
              }*/
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            try
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                GraphicsPath gp = new(FillMode.Winding);

                Color col2 = Color.White;
                Color col1 = Color.Gray;

                Pen pen = new(Color.Black, 2);

                int rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                int r = this.rectangle.Width - rhoff;
                int right = R.TopRight().X;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);
                int t = this.rectangle.Top;
                int b = this.rectangle.Bottom;
                int h = b - t;

                Pen p = new(Color.Black, 1);

                LinearGradientBrush BodyBrush = new(new PointF(this.RotatedRectangle.Left, 0),
                        new PointF(RotatedRectangle.Right, 0), col2, col1);

                BodyBrush.SetBlendTriangularShape(1f);

                Rectangle ColumnBody = new(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);
                gp.AddRectangle(ColumnBody);
                gp.CloseFigure();

                gp.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                gp.CloseFigure();

                gp.AddArc(this.rectangle.Left, b - ArcHeight - 1, this.rectangle.Width, ArcHeight, 0, 180);
                if (llesep.IsActive)
                    gp.CloseFigure();

                gp.AddArc(R.TopLeft().X, R.TopLeft().Y, this.rectangle.Width, ArcHeight, 180, 180);
                gp.CloseFigure();
                gp.AddLine(R.TopLeft().X, R.TopLeft().Y + ArcHeight / 2, R.BottomLeft().X, R.BottomLeft().Y - ArcHeight / 2);
                gp.CloseFigure();
                gp.AddLine(R.TopRight().X, R.TopRight().Y + ArcHeight / 2, R.BottomRight().X, R.BottomRight().Y - ArcHeight / 2);
                gp.CloseFigure();
                gp.AddArc(R.BottomLeft().X, R.BottomLeft().Y - ArcHeight, this.rectangle.Width, ArcHeight, 0, 180);
                gp.CloseFigure();

                // draw lines
                for (int i = 1; i < 10; i++)
                {
                    int Height = ColumnBody.Height * i / 10;
                    gp.AddLine(ColumnBody.TopLeft().X, ColumnBody.TopLeft().Y + Height, ColumnBody.TopRight().X, ColumnBody.TopRight().Y + Height);
                    gp.CloseFigure();
                }

                if (Angle != enumRotationAngle.a0)
                    Rotate(gp, rectangle.Center());

                if (FlipHorizontal)
                {
                    Matrix m = new();
                    m.Scale(-1, 1);
                    m.Translate(RotatedRectangle.Width + 2 * RotatedRectangle.Left, 0, MatrixOrder.Append);
                    gp.Transform(m);
                }

                if (FlipVertical)
                {
                    Matrix m = new();
                    m.Scale(1, -1);
                    m.Translate(0, RotatedRectangle.Height + 2 * RotatedRectangle.Top, MatrixOrder.Append);
                    gp.Transform(m);
                }

                g.FillPath(BodyBrush, gp);
                g.DrawPath(pen, gp);
            }
            catch { }
        }

        internal override void OnDoubleClick(DrawArea drawArea, MouseEventArgs e)
        {
            cdlg = new(this);
            cdlg.ShowDialog();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("graphicslist", graphicslist);
            info.AddValue("StreamConnections", StreamConnectionsGuidDic);
            info.AddValue("column", llesep);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            DesignChanged();
            cdlg = new(this);

            if (llesep is null)
            {
                llesep = new();
                //GlobalModel.flowsheet.Add(column);
            }
        }
    }
}