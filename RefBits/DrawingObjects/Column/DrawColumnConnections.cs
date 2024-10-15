using ModelEngine;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Units
{
    /// <summary>
    /// Rectangle graphic object
    /// </summary>
    ///
    public partial class DrawColumn : DrawRectangle, ISerializable
    {
        internal void ConnectExtenalFeedsTointernalFeeds()
        {
            RefreshStreams();

            foreach (DrawMaterialStream extFeed in ExtFeedStreams)
            {
                // extFeed.PortOut.ConnectedPorts.Clear();
                if (StreamConnectionsGuidDic.TryGetValue(extFeed.Guid, out Guid intGuid))
                {
                    DrawMaterialStream intFeed = IntFeedStreams[intGuid];
                    intFeed.Stream.ConnectedStreamGuid = extFeed.Stream.Guid;
                    extFeed.Stream.ConnectedStreamGuid = intFeed.Guid;
                    intFeed.Port.ConnectPort(extFeed.Stream);
                }
            }
        }

        internal void ConnectinternalProductsToExternalProducts()
        {
            RefreshStreams();

            foreach (DrawMaterialStream exProd in ExtProductStreams)
            {
                if (exProd.Port.ConnectedPortNext is not null)
                    exProd.Port.ConnectedPortNext.ClearConnections();

                if (StreamConnectionsGuidDic.TryGetValue(exProd.Guid, out Guid intGuid))
                {
                    DrawMaterialStream intProduct = IntProductStreams[intGuid];
                    if (intProduct != null)
                    {
                        intProduct.Stream.ConnectedStreamGuid = exProd.Stream.Guid;
                        exProd.Stream.ConnectedStreamGuid = intProduct.Guid;
                        exProd.Port.ConnectPort(intProduct.Stream);
                    }
                }
            }
        }

        internal void ConnectinternalPAsToExternalPAs()
        {
            RefreshStreams();

            foreach (DrawMaterialStream exporteStream in ExportedStreams)
            {
                if (exporteStream.Port.ConnectedPortNext is not null)
                    exporteStream.Port.ConnectedPortNext.ClearConnections();
                if (StreamConnectionsGuidDic.TryGetValue(exporteStream.Guid, out Guid intGuid))
                {
                    DrawMaterialStream intPADraw = PAs.DrawStreams()[intGuid];
                    if (intPADraw != null)
                    {
                        exporteStream.Port.ConnectPort(intPADraw.Stream);
                    }
                }
            }
        }

        internal void ConnectinternalStreamsToExportedStreams()
        {
            RefreshStreams();

            foreach (DrawMaterialStream exporteStream in ExportedStreams)
            {
                if (exporteStream.Port.ConnectedPortNext is not null)
                    exporteStream.Port.ConnectedPortNext.ClearConnections();

                if (StreamConnectionsGuidDic.TryGetValue(exporteStream.Guid, out Guid intGuid))
                {
                    DrawMaterialStream intPADraw = InternalConnectingStreams[intGuid];

                    if (intPADraw != null)
                    {
                        exporteStream.Port.ConnectPort(intPADraw.Stream);
                    }
                }
            }
        }

        internal void ConnectinternalFeedStreamsToFeedPorts()
        {
            RefreshStreams();

            foreach (DrawMaterialStream intFeed in IntFeedStreams)
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

                            if (drawtray != null)
                            {
                                //if (drawtray.Tray.feed.ConnectedPort is not null)
                                //    drawtray.Tray.feed.ConnectedPort.ClearConnections();
                                drawtray.Tray.feed.ConnectPort(intFeed.Stream);
                                drawtray.Tray.feed.Owner = column;
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

            foreach (DrawMaterialStream intProduct in IntProductStreams)
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
                                    //int Product.PortIn.Owner = this.column;
                                    break;

                                case "VAPPRODUCT":
                                    a.DrawTrays[0].Tray.TrayVapour.ConnectPort(intProduct.Stream);
                                    //int Product.PortIn.Owner = this.column;
                                    break;

                                default:
                                    drawtray.Tray.liquidDrawRight.ConnectPort(intProduct.Stream);
                                    //int Product.PortIn.Owner = this.column;
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
                                column.MainTraySection.BottomTray.feed.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.column;
                                break;

                            case "Reboil Vap":
                                column.MainTraySection.BottomTray.vapourDraw.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.column;
                                break;

                            case "LiqProduct":
                                column.MainTraySection.BottomTray.liquidDrawRight.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.column;
                                break;
                        }
                        column.MainTraySection.BottomTray.TrayLiquid.ConnectPort(intProduct.Stream);
                        intProduct.Port.Owner = this.column;
                        break;

                    case DrawColumnCondenser c:
                        switch (c.GetNode(intProduct.StartNodeGuid).Name)
                        {
                            case "VapProduct":
                                column.MainTraySection.TopTray.TrayVapour.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.column;
                                break;

                            case "LiqProduct":
                                column.MainTraySection.TopTray.liquidDrawRight.ConnectPort(intProduct.Stream);
                                intProduct.Port.Owner = this.column;
                                break;

                            case "Water":
                                column.MainTraySection.TopTray.WaterDraw.ConnectPort(intProduct.Stream);
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

            foreach (DrawMaterialStream intStream in InternalConnectingStreams)
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
                        //intStream.StartDrawTray.Tray.liquidDrawLeft.IsPADraw = true;
                        intStream.StartDrawTray.Tray.liquidDrawRight.IsPADraw = false;
                    }
                    else
                    {
                        //pa.DrawPort = intStream.StartDrawTray.Tray.liquidDrawRight;
                        pa.drawTray = intStream.StartDrawTray.Tray;
                        intStream.StartDrawTray.Tray.liquidDrawRight.IsPADraw = true;
                       // intStream.StartDrawTray.Tray.liquidDrawLeft.IsPADraw = false;
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

                                case HotSpotType.TrayNetVapour:
                                    drawtray.Tray.TrayVapour.ConnectPort(intStream.Stream);
                                    break;
                            }
                        }
                        break;

                    case DrawColumnReboiler r:
                        switch (r.GetNode(intStream.StartNodeGuid).Name)
                        {
                            case "Reboil Vap":
                                column.MainTraySection.BottomTray.TrayVapour.ConnectPort(intStream.Stream);
                                break;
                        }
                        break;

                    case DrawColumnCondenser c:
                        switch (c.GetNode(intStream.StartNodeGuid).Name)
                        {
                            case "Reflux":
                                column.MainTraySection.TopTray.TrayLiquid.ConnectPort(intStream.Stream);
                                break;
                        }
                        break;

                    case DrawPA pa:
                        //pa.PumpAround.ReturnPort.ConnectPort(intStream.Stream);

                        break;
                }
            }
        }

        public void TransferTraysToProductStreams()
        {
            DrawColumnTraySectionCollection DrawTraySections = graphicslist.ReturnTraySections();
            DrawColumnTraySection DrawMainSection = DrawTraySections[0];
            DrawMaterialStream extstream;

            foreach (DrawMaterialStream stream in IntProductStreams)
            {
                if (StreamConnectionsGuidDic.TryGetValue(stream.Guid, out Guid ExtStreamGuid))
                {
                    extstream = ExtProductStreams[ExtStreamGuid];
                    DrawTray tray = DrawMainSection.GetTray(stream.Guid);
                }
            }
        }

        public static void TransferTraysToPumpAroundStreams()
        {
            /*  Guid ExtStreamGuid;
              DrawTray tray;
              DrawColumnTraySectionCollection DrawTraySections = graphicslist.returnTraySections();
              DrawColumnTraySection DrawMainSection = DrawTraySections[0];
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

        internal void ConnectReboilerCondensers()
        {
            foreach (DrawMaterialStream dms in InternalConnectingStreams)
            {
                if (dms.endDrawObject is DrawColumnReboiler dcr &&
                    dms.startDrawObject is DrawColumnTraySection dcts)
                {
                    dcr.section = dcts;
                    dcts.Reboilertype = dcr.reboilertype;

                }
                if (dms.endDrawObject is DrawColumnCondenser dcc &&
                    dms.startDrawObject is DrawColumnTraySection dcts2)
                {
                    dcc.section = dcts2;
                    dcts2.CondenserType = dcc.CondenserType;
                }
            }
        }
    }
}