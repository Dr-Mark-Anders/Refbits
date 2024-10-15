using ModelEngine;
using Extensions;
using ModelEngine;
using RusselColumn;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    [Serializable]
    public partial class DrawColumn : DrawRectangle, ISerializable
    {
        private ColumnDLG cdlg;
        private COlSubFlowSheet subflowsheet = new();  // Objects added in main solve routine
        private Column column = new();
        internal DrawArea columndrawarea;
        private static int Count;

        /// <summary>
        /// int Guid, ExtGuid
        /// </summary>
        internal Dictionary<Guid, Guid> StreamConnectionsGuidDic = new();

        internal DrawStreamCollection ExtFeedStreams = new();
        internal DrawStreamCollection IntFeedStreams = new();
        internal DrawStreamCollection ExtProductStreams = new();
        internal DrawStreamCollection IntProductStreams = new();
        internal DrawStreamCollection InternalConnectingStreams = new();
        internal DrawStreamCollection IntersectionNetStreams = new();
        internal DrawStreamCollection IntersectionDrawStreams = new();
        internal DrawPACollection PAs = new();
        internal DrawColumnTraySectionCollection DrawTraySections;
        internal DrawColumnCondenserCollection drawColumnCondensers = new();
        internal DrawColumnReboilerCollection drawColumnReboilers = new();
        internal DrawStreamCollection ExportedStreams = new();
        internal DrawStreamCollection PADrawStreams = new();
        internal DrawStreamCollection PAReturnStreams = new();

        internal void RefreshStreams()
        {
            subflowsheet.Name = this.Name;
            ExtFeedStreams = this.DrawArea.GraphicsList.ReturnExternalFeedStreams(this);
            IntFeedStreams = this.graphicslist.ReturnInternalFeedStreams();
            ExtProductStreams = this.DrawArea.GraphicsList.ReturnExternalProductStreams(this);
            IntProductStreams = this.graphicslist.ReturnInternalProductStreams();
            InternalConnectingStreams = this.graphicslist.ReturnInternalConnectingStreams();
            IntersectionNetStreams = this.graphicslist.ReturnIntersectionNetStreams();
            IntersectionDrawStreams = this.graphicslist.ReturnIntersectionDrawStreams();
            drawColumnCondensers = this.graphicslist.ReturnCondensers();
            drawColumnReboilers = this.graphicslist.ReturnReboilers();
            ExportedStreams = this.DrawArea.GraphicsList.ReturnExternalExportedStreams(this);
           /* PADrawStreams = this.PAs.DrawStreams();
            PAReturnStreams = this.PAs.ReturnStreams();

            foreach (PumpAround pa in PAs)
            {
                
            }*/

            for (int i = 0; i < ExtFeedStreams.Count; i++)
            {
                var stream = ExtFeedStreams[i];
                column.ExternalStreams.Add(stream.Stream);
            }


            foreach (DrawMaterialStream drawStream in InternalConnectingStreams) // to handle petyluk configurations
            {
                if (drawStream.endDrawObject is DrawColumnTraySection)
                    drawStream.intersectionstream.isNetBottomConnectedFlow = true;
            }

            foreach (DrawMaterialStream drawStream in IntFeedStreams)
            {
                drawStream.Sidestream.Name = drawStream.Name;
                drawStream.Stream.Name = drawStream.Name;
                foreach (var port in drawStream.Stream.Ports)
                    port.Owner = subflowsheet;
            }

            foreach (DrawMaterialStream drawStream in IntProductStreams)
            {
                drawStream.Sidestream.Name = drawStream.Name;
                drawStream.Stream.Name = drawStream.Name;
                foreach (var port in drawStream.Stream.Ports)
                    port.Owner = subflowsheet;
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

        public override bool UpdateAttachedModel()
        {
            if (!GlobalModel.IsRunning)
            {
                // Sort by size
                subflowsheet.Name = this.Name;
                subflowsheet.IsActive = column.IsActive;
                column.Name = this.Name;

                ClearData();

                UpdateNodePortGuids();

                graphicslist.UpdateDrawObjects();
                RefreshStreams();
                DesignChanged();
                ConnectExtenalFeedsTointernalFeeds();
                ConnectinternalFeedStreamsToFeedPorts();
                ConnectProductPortstointernalProducts();
                ConnectinternalProductsToExternalProducts();
                ConnectinternalConnectingStreamsToStages();
                ConnectReboilerCondensers(); //  connects reboilers and condenser to traysections

                //Connectinternal PAsToExternalPAs();
                ConnectinternalStreamsToExportedStreams();

                ProcessSpecs();
            }

            if (column.Specs.DegreesOfFreedom != 0)
                return false;

            return true;
        }

        private void UpdateNodePortGuids()
        {
            if (this.DrawTraySections is not null)
                foreach (DrawColumnTraySection section in this.DrawTraySections)
                {
                    section.UpdateNodeGuids();

                    foreach (DrawTray tray in section)
                    {
                        tray.UpdateNodeGuids();
                    }
                }
        }


        private void ProcessSpecs()
        {
            DrawMaterialStream drawStream;
            StreamMaterial stream;
            SideStream sideStream;
            ConnectingStream connectstream;
            DrawColumnTraySection drawsection;

            foreach (Specification spec in column.Specs)
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
                        drawsection = this.DrawTraySections[spec.drawObjectGuid];
                        drawStream = this.IntProductStreams[spec.drawStreamGuid];
                        if (drawStream != null)
                        {
                            connectstream = drawStream.intersectionstream;
                            connectstream.EngineDrawSection = drawsection.TraySection;
                            connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            connectstream.InitialFlowEstimate = 0.1;
                        }
                        break;

                    case eSpecType.TrayNetLiqFlow:
                        spec.engineSpecType = eSpecType.TrayNetLiqFlow;
                        drawsection = this.DrawTraySections[spec.drawObjectGuid];
                        drawStream = this.IntProductStreams[spec.drawStreamGuid];
                        if (drawStream != null)
                        {
                            connectstream = drawStream.intersectionstream;
                            connectstream.EngineDrawSection = drawsection.TraySection;
                            connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            connectstream.InitialFlowEstimate = 0.1;
                        }
                        break;

                    case eSpecType.LiquidStream:
                        spec.engineSpecType = eSpecType.LiquidStream;

                        drawStream = this.InternalConnectingStreams[spec.drawObjectGuid];
                        drawsection = this.DrawTraySections[drawStream.startDrawObjectID];

                        if (drawStream != null)
                        {
                            spec.engineStageGuid = drawStream.EndDrawTray.Guid;
                            spec.engineSectionGuid = drawsection.TraySection.Guid;

                            stream = drawStream.Stream;
                            connectstream = drawStream.intersectionstream;
                            spec.connectedStream = connectstream;
                            drawsection = this.DrawTraySections[spec.drawSectionGuid];

                            if (connectstream != null && drawsection != null)
                            {
                                connectstream.EngineDrawSection = drawsection.TraySection;
                                connectstream.engineDrawTray = drawStream.StartDrawTray.Tray;
                            }
                        }
                        break;

                    case eSpecType.VapStream:
                        spec.engineSpecType = eSpecType.VapStream;

                        drawStream = this.InternalConnectingStreams[spec.drawObjectGuid];
                        drawsection = this.DrawTraySections[drawStream.startDrawObjectID];

                        if (drawStream != null)
                        {
                            spec.engineStageGuid = drawStream.EndDrawTray.Guid;
                            spec.engineSectionGuid = drawsection.TraySection.Guid;

                            stream = drawStream.Stream;
                            connectstream = drawStream.intersectionstream;
                            spec.connectedStream = connectstream;
                            drawsection = this.DrawTraySections[spec.drawSectionGuid];

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

                        drawStream = this.IntProductStreams[spec.drawStreamGuid];

                        if (drawStream != null)
                        {
                            stream = drawStream.Stream;

                            switch (drawStream.startDrawObject)
                            {
                                case DrawColumnTraySection dcts:
                                    ss = drawStream.Sidestream;
                                    drawsection = DrawTraySections[drawStream.StartDrawObjectGuid];
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
                                    drawsection = DrawTraySections[drawStream.SectionNumber];
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
                                    drawsection = DrawTraySections[drawStream.startDrawObjectID];
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
                        drawStream = this.IntProductStreams[spec.drawObjectGuid];
                        if (drawStream is null)
                            continue;
                        drawsection = DrawTraySections[drawStream.StartDrawObjectGuid];

                        spec.engineSpecType = eSpecType.VapProductDraw;
                        spec.engineStageGuid = drawStream.StartDrawTray.Tray.Guid;
                        //spec.engineSectionGuid = drawStream.StartDrawObject Section.Guid;

                        if (drawStream != null)
                        {
                            stream = drawStream.Stream;
                            sideStream = column.VapourSideStreams[spec.engineObjectguid];
                            sideStream.EngineDrawTray = column[drawStream.SectionNumber][drawStream.EngineDrawTrayGuid];
                        }
                        break;

                    case eSpecType.PAFlow:
                        spec.engineSpecType = eSpecType.PAFlow;
                        DrawPA drawpa = this.PAs[spec.drawObjectGuid];

                        if (drawpa != null)
                        {
                            PumpAround pa = column.PumpArounds[drawpa.Name];
                            if (pa != null)
                            {
                                DrawTray dt = this.DrawTraySections[drawpa.DrawSectionNo][drawpa.DrawTrayDrawGuid];
                                pa.drawTray = dt.Tray;

                                drawsection = this.DrawTraySections[drawpa.ReturnSectionNo];

                                if (drawsection != null)
                                    dt = drawsection[drawpa.ReturnTrayDrawGuid];

                                pa.returnSection = column[drawpa.ReturnSectionNo];
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
                            PumpAround p = column.PumpArounds[drawpa.Name];
                            DrawTray dt = this.DrawTraySections[drawpa.DrawSectionNo][drawpa.DrawTrayDrawGuid];
                            p.drawTray = dt.Tray;

                            dt = this.DrawTraySections[drawpa.ReturnSectionNo][drawpa.ReturnTrayDrawGuid];

                            p.returnSection = column[drawpa.ReturnSectionNo];
                            spec.engineSectionGuid = p.returnSection.Guid;

                            spec.PAguid = p.Guid;
                        }
                        break;

                    case eSpecType.Energy:
                        spec.engineSpecType = eSpecType.Energy;
                        break;

                    case eSpecType.PADeltaT:
                        drawpa = this.PAs[spec.drawObjectGuid];
                        if (drawpa != null)
                        {
                            PumpAround p = column.PumpArounds[drawpa.Name];
                            DrawTray dt = this.DrawTraySections[drawpa.DrawSectionNo][drawpa.DrawTrayDrawGuid];
                            p.drawTray = dt.Tray;

                            dt = this.DrawTraySections[drawpa.ReturnSectionNo][drawpa.ReturnTrayDrawGuid];

                            p.returnSection = column[drawpa.ReturnSectionNo];
                            spec.engineSectionGuid = p.returnSection.Guid;

                            spec.PAguid = p.Guid;
                        }
                        spec.engineSpecType = eSpecType.PADeltaT;
                        break;

                    case eSpecType.PADuty:
                        drawpa = this.PAs[spec.drawObjectGuid];
                        if (drawpa != null)
                        {
                            PumpAround p = column.PumpArounds[drawpa.Name];
                            DrawTray dt = this.DrawTraySections[drawpa.DrawSectionNo][drawpa.DrawTrayDrawGuid];
                            p.drawTray = dt.Tray;

                            dt = this.DrawTraySections[drawpa.ReturnSectionNo][drawpa.ReturnTrayDrawGuid];

                            p.returnSection = column[drawpa.ReturnSectionNo];
                            spec.engineSectionGuid = p.returnSection.Guid;

                            spec.PAguid = p.Guid;
                        }
                        spec.engineSpecType = eSpecType.PADuty;
                        break;

                    case eSpecType.DistSpec:
                        spec.engineSpecType = eSpecType.DistSpec;
                        drawStream = this.IntProductStreams[spec.drawStreamGuid];
                        if (drawStream != null)
                        {
                            stream = drawStream.Stream;

                            switch (drawStream.startDrawObject)
                            {
                                case DrawColumnTraySection dcts:
                                    ss = drawStream.Sidestream;
                                    drawsection = DrawTraySections[drawStream.StartDrawObjectGuid];
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
                                    drawsection = DrawTraySections[drawStream.SectionNumber];
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
                                    drawsection = DrawTraySections[drawStream.startDrawObjectID];
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

            DrawTraySections = graphicslist.ReturnTraySections();
            DrawTraySections.Sort();
            int count;

            if (DrawTraySections is null || DrawTraySections.Count == 0)
                return;

            foreach (DrawColumnTraySection dts in this.DrawTraySections)
            {
                count = 1;
                foreach (DrawTray tray in dts)
                {
                    tray.Name = "Tray: " + count;
                    count++;
                }
            }

            column.TraySections = new(column, false);

            foreach (DrawColumnTraySection dcts in DrawTraySections)
            {
                TraySection trayCollection = dcts.TraySection;
                trayCollection.Name = dcts.Name;
                column.TraySections.Add(trayCollection);
            }

            column.LiquidSideStreams.Clear();
            column.VapourSideStreams.Clear();

            foreach (DrawMaterialStream stream in IntProductStreams)
            {
                stream.UpdateSideDraws(DrawTraySections);

                if (stream.Startnode(graphicslist) != null)
                {
                    switch (stream.Startnode(graphicslist).NodeType)
                    {
                        case HotSpotType.LiquidDraw:
                            if (stream.StartNode.Owner is not DrawColumnReboiler)
                                column.LiquidSideStreams.Add(stream.Sidestream);
                            break;

                        case HotSpotType.VapourDraw:
                            if (stream.StartNode.Owner is not DrawColumnCondenser)
                                column.VapourSideStreams.Add(stream.Sidestream);
                            break;
                    }
                }
            }

            column.ConnectingDraws.Clear();
            column.ConnectingNetFlows.Clear();

            foreach (DrawMaterialStream drawstream in IntersectionNetStreams)
            {
                drawstream.UpdateEngineInterConnectStream(DrawTraySections);

                if (drawstream.intersectionstream.InitialFlowEstimate <= 0)
                    drawstream.intersectionstream.InitialFlowEstimate = 0.1;

                if (double.IsNaN(drawstream.intersectionstream.InitialFlowEstimate))
                    drawstream.intersectionstream.InitialFlowEstimate = 0.1;

                if (drawstream.StartDrawObject != null && drawstream.StartNode != null)
                    switch (drawstream.StartNode.NodeType)
                    {
                        case HotSpotType.TrayNetLiquid:
                            drawstream.intersectionstream.InitialFlowEstimate = 0.1;
                            drawstream.IntersectionStream.isliquid = true;
                            column.ConnectingNetFlows.Add(drawstream.IntersectionStream);
                            break;

                        case HotSpotType.TrayNetVapour:
                            drawstream.intersectionstream.InitialFlowEstimate = 0.1;
                            drawstream.IntersectionStream.isliquid = false;
                            column.ConnectingNetFlows.Add(drawstream.IntersectionStream);
                            break;
                    }
            }

            foreach (DrawMaterialStream stream in IntersectionDrawStreams)
            {
                stream.UpdateEngineInterConnectStream(DrawTraySections);

                if (stream.StartDrawObject != null && stream.StartNode != null)
                    switch (stream.StartNode.NodeType)
                    {
                        case HotSpotType.LiquidDraw:
                            stream.IntersectionStream.isliquid = true;
                            stream.FlowEstimate = 0.1;
                            column.ConnectingDraws.Add(stream.intersectionstream);
                            break;

                        case HotSpotType.VapourDraw:
                            stream.FlowEstimate = 0.1;
                            stream.IntersectionStream.isliquid = false;
                            column.ConnectingDraws.Add(stream.intersectionstream);
                            break;
                    }
            }

            column.PumpArounds.Clear();
            PumpAround pumparound;
            DrawTray drawtray, returntray;
            DrawColumnTraySection dcts1, rcts1;

            foreach (DrawPA pa in PAs)
            {
                dcts1 = DrawTraySections[pa.DrawSectionNo];
                rcts1 = DrawTraySections[pa.ReturnSectionNo];

                pumparound = column.PumpArounds.Add(pa.PumpAround);

                if (pa.feed is not null && pa.feed.StartNode is not null)
                {
                    drawtray = (DrawTray)pa.feed.StartNode.Owner;
                    pa.DrawTrayDrawGuid = drawtray.Guid;
                    if (drawtray is not null)
                        pumparound.drawTray = drawtray.Tray;

                    pumparound.StreamIn = pa.feed.Stream;
                }

                if (pa.effluent is not null)
                {
                    returntray = (DrawTray)pa.effluent.EndNode.Owner;
                    pa.ReturnTrayDrawGuid = returntray.Guid;
                    if (returntray != null)
                        pumparound.returnTray = returntray.Tray;

                    pumparound.StreamOut = pa.effluent.Stream;
                }

                if (pumparound is not null)
                {
                    pumparound.drawSection = column.TraySections[pa.DrawSectionNo];
                    pumparound.returnSection = column.TraySections[pa.ReturnSectionNo];
                    pumparound.Name = pa.Name;
                }
            }

            ConnectReboilerCondensers(); //  connects reboierlers and condensre to traysections

            foreach (DrawMaterialStream item in PADrawStreams)
            {
                column.PADrawStreams.Add(item.Stream);
            }


            foreach (DrawMaterialStream item in PAReturnStreams)
            {
                column.PAReturnStreams.Add(item.Stream);
            }


            foreach (Specification spec in column.Specs)
            {
                switch (spec.engineSpecType)
                {
                    case eSpecType.TrayNetLiqFlow:
                    case eSpecType.TrayNetVapFlow:
                    case eSpecType.RefluxRatio:
                    case eSpecType.Temperature:
                    case eSpecType.Energy:
                        spec.engineObjectguid = column.Guid;
                        break;
                }
            }
        }

        public DrawColumn() : this(0, 0, 50, 300)
        {
           // cdlg = new ColumnDLG(this);
        }

        public DrawColumn(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            columndrawarea = new();

            try
            {
                graphicslist = (GraphicsList)info.GetValue("graphicslist", typeof(GraphicsList));
                StreamConnectionsGuidDic = (Dictionary<Guid, Guid>)info.GetValue("StreamConnections", typeof(Dictionary<Guid, Guid>));
                column = (Column)info.GetValue("column", typeof(Column));
                //cd.CalcHandler += new  EventHandler(Calculate);
            }
            catch
            {
            }
            column.Name = this.Name;
            columndrawarea.Name = "Column Draw Area" + this.Name;
            subflowsheet.IsActive = column.IsActive;

          
        }

        public DrawColumn(int x, int y, int width, int height) : base()
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

            Hotspots.Add(new Node(this, 1f, 0.05f, "Internal1", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.15f, "Internal2", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.25f, "Internal3", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.35f, "Internal4", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
            Hotspots.Add(new Node(this, 1f, 0.45f, "Internal5", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));

            Initialize();

            this.Name = "C" + Count.ToString();
            columndrawarea = new("Column Draw Area" + this.Name);

            DrawColumnTraySection dcts = new(this.columndrawarea);
            dcts.Rectangle = new(200, 200, 50, 200);
            graphicslist.Add(dcts);

            column.UpdateIteration1 += new IterationEventHAndler(IterationCompleted1);
            column.UpdateIteration2 += new IterationEventHAndler(IterationCompleted2);

            Count++;
        }

        private void ClearData()
        {
            if (cdlg != null)
            {
                cdlg.Error1.Clear();
                cdlg.Error2.Clear();
            }
            column.ErrHistory1 = "";
            column.ErrHistory2 = "";
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
            DrawColumn drawRectangle = new();
            drawRectangle.rectangle = this.rectangle;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }

     
        public void Calculate(object sender, EventArgs e)
        {
            if (column.Specs.DegreesOfFreedom != 0)
            {
                MessageBox.Show("Degress of Freedom Must be 0");
                return;
            }

            column.Err1 = "";
            column.Err2 = "";
            column.ErrHistory1 = "";
            column.ErrHistory2 = "";

            if (!GlobalModel.IsRunning)
                UpdateAttachedModel();

            column.IsDirty = true;
            subflowsheet.IsDirty = true;
            subflowsheet.ModelStack.SetDirty();
            DrawArea.SetUpModelStack(GlobalModel.Flowsheet);
            DrawArea.SolveAsync();

            if (column.IsSolved)
            {
                PenColor = Color.Black;
                cdlg.FillPressureData();
                cdlg.FillProfileData(this.column);

                TransferTraysToProductStreams();
                TransferTraysToPumpAroundStreams();

                cdlg.FillProfileData(column);
                cdlg.FillDiagnosticData(column);
                cdlg.UpdateSpecs(this);
                cdlg.Refresh();
                return;
            }
            else
            {
                PenColor = Color.Red;
                cdlg.FillDiagnosticData(column);
                cdlg.ClearOldResults();
                column.SolutionConverged = false;
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

                    drawstream.Port.Clear();
                    drawstream.Port.H_ = new (ePropID.H,pa.ReturnEnthalpy,SourceEnum.Input);
                    drawstream.Port.P_ = new (ePropID.P,pa.returnTray.P, SourceEnum.Input);
                    drawstream.Port.MolarFlow_ = pa.MoleFlow;
                    drawstream.Port.cc.SetMolFractions(pa.drawTray.LiqComposition);
                    drawstream.Port.cc.Origin = SourceEnum.Input;
                    drawstream.Port.MolarFlow_.Source = SourceEnum.Input;
                    drawstream.Port.Flash();
                }
            }
            if (cdlg != null)
                cdlg.loaddata();
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
                Color OutlineColor = Color.Black;

                if (!IsSolved)
                    OutlineColor = Color.Red;

                Pen pen = new(OutlineColor, 2);

                int rhoff = Convert.ToInt32(0.43 * this.rectangle.Width);
                int r = this.rectangle.Width - rhoff;
                int right = R.TopRight().X;
                int l = 0;
                int w = r - l;
                int ArcHeight = Convert.ToInt32(w / 2);
                int t = this.rectangle.Top;
                int b = this.rectangle.Bottom;
                int h = b - t;

                LinearGradientBrush BodyBrush = new(new PointF(this.RotatedRectangle.Left, 0),
                        new PointF(RotatedRectangle.Right, 0), col2, col1);

                BodyBrush.SetBlendTriangularShape(1f);

                Rectangle ColumnBody = new(this.rectangle.Left, t + ArcHeight / 2, this.rectangle.Width, h - ArcHeight);
                gp.AddRectangle(ColumnBody);
                gp.CloseFigure();

                gp.AddArc(this.rectangle.Left, t, this.rectangle.Width, ArcHeight, 180, 180);
                gp.CloseFigure();

                gp.AddArc(this.rectangle.Left, b - ArcHeight - 1, this.rectangle.Width, ArcHeight, 0, 180);
                if (column.IsActive)
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
           // cdlg = new(this);
            cdlg.ShowDialog();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("graphicslist", graphicslist);
            info.AddValue("StreamConnections", StreamConnectionsGuidDic);
            info.AddValue("column", column);
            base.GetObjectData(info, context);
        }

        [OnDeserialized()]
        public new void OnDeserializedMethod(StreamingContext context)
        {
            DesignChanged();
            cdlg = new(this);

            if (column is null)
            {
                column = new Column();
            }

            column.OnRequestParent += new Func<object>(delegate { return this; });

            if (Hotspots.Count < 22)
            {
                Hotspots.Add(new Node(this, 1f, 0.05f, "Internal1", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
                Hotspots.Add(new Node(this, 1f, 0.15f, "Internal2", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
                Hotspots.Add(new Node(this, 1f, 0.25f, "Internal3", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
                Hotspots.Add(new Node(this, 1f, 0.35f, "Internal4", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
                Hotspots.Add(new Node(this, 1f, 0.45f, "Internal5", NodeDirections.Right, HotSpotType.InternalExport, HotSpotOwnerType.DrawRectangle));
            }

            cdlg = new ColumnDLG(this);
        }
    }
}