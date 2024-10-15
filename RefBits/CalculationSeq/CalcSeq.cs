using System;
using System.Collections.Generic;

namespace Units.CalculationSeq
{
    public class CalcSeq
    {
        private DrawArea drawarea;
        private int steps;
        private EnumCalcSeq calcseq;

        public int Steps
        {
            get
            {
                return steps;
            }

            set
            {
                steps = value;
            }
        }

        public EnumCalcSeq Calcseq
        {
            get
            {
                return calcseq;
            }

            set
            {
                calcseq = value;
            }
        }

        public CalcSeq(DrawArea drawArea)
        {
            this.drawarea = drawArea;
            List<DrawObject> CalcOrderOne = new List<DrawObject>();
            List<DrawObject> NextCalcOrders = new List<DrawObject>();
            List<DrawObject> CalcOrders = new List<DrawObject>();
            List<DrawObject> FeedObjects = drawarea.GraphicsList.ReturnFeedObjects();
            List<DrawObject> ConnectedStreams = drawarea.GraphicsList.ReturnFrontConnectedStreams();

            CalcOrderOne.Clear();

            foreach (object ds in FeedObjects)
            {
                if (ds.GetType() == typeof(DrawMaterialStream))
                {
                    DrawMaterialStream d = (DrawMaterialStream)ds;
                    if (d.EndDrawObjectGuid != new Guid())
                    {
                        DrawObject dest = drawarea.GraphicsList.GetObject(d.EndDrawObjectGuid);
                        if (!CalcOrderOne.Contains(dest))
                        {
                            if (dest != null)//doesntgoanywhere
                            {
                                dest.CalcOrder = 1;
                            }
                            d.CalcOrder = 1;
                            CalcOrderOne.Add(dest);//level1destinations
                        }
                    }
                }
                else if (ds is DrawRectangle)//assayfeeder
                {
                    DrawRectangle d = (DrawRectangle)ds;
                    d.CalcOrder = -1;
                    DrawStreamCollection ss = drawarea.GraphicsList.ReturnExitingStreams(d.Guid);
                    foreach (DrawMaterialStream s in ss)
                    {
                        if (s != null)
                        {
                            if (s.EndDrawObjectGuid != Guid.Empty)
                            {
                                DrawObject dest = drawarea.GraphicsList.GetObject(s.EndDrawObjectGuid);
                                if (!CalcOrderOne.Contains(dest))
                                {
                                    if (dest != null)//doesntgoanywhere
                                    {
                                        dest.CalcOrder = 1;
                                    }
                                    s.CalcOrder = 0;
                                    CalcOrderOne.Add(dest);//level1destinations
                                }
                            }
                        }
                    }
                }
            }

            CalcOrders = CalcOrderOne;

            //calcorder1defined,nextdootherlevels

            if (CalcOrders.Count > 0)
            {
                for (int no = 1; no < 100; no++)//shouldntbemorethan100levels
                {
                    foreach (DrawObject d in CalcOrders)
                    {
                        if (CalcOrders.Count > 1000)
                            return;

                        if (d != null)
                        {
                            if (d is DrawMaterialStream)
                            {
                                DrawObject dest = drawarea.GraphicsList.GetObject(((DrawMaterialStream)d).EndDrawObjectGuid);
                                if (dest != null && !(dest is DrawRecycle))
                                {
                                    if (dest.OriginObjects.Contains(dest))//recycle
                                    {
                                        ((DrawMaterialStream)d).IsRecycle = true;
                                    }
                                    else
                                    {
                                        if (!NextCalcOrders.Contains(dest))
                                        {
                                            dest.CalcOrder = no + 2;
                                        }
                                    }
                                    NextCalcOrders.Add(dest);
                                    dest.CalcOrder = no + 1;
                                }
                            }
                            else
                            {
                                DrawStreamCollection streams = drawarea.GraphicsList.ReturnExitingStreams(d.Guid);
                                foreach (DrawMaterialStream ds in streams)
                                {
                                    ds.CalcOrder = no + 1;
                                    NextCalcOrders.Add(ds);
                                }
                            }
                        }
                    }
                    CalcOrders.Clear();
                    CalcOrders.AddRange(NextCalcOrders.ToArray());
                    NextCalcOrders.Clear();
                    if (CalcOrders.Count == 0)
                    { break; }
                }
            }
        }
    }
}