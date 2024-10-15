using ModelEngine;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Units
{
    public class DrawStreamCollection : IEnumerable
    {
        private readonly List<DrawMaterialStream> streamlist = new();

        public Components SumAndNormaliseComponents()
        {
            Components bcc = new();

            foreach (DrawMaterialStream ds in streamlist) //  add all the bc's
            {
                foreach (BaseComp bc in ds.Components)
                    bc.TempMolarFlow = bc.MoleFraction * ds.Port.MolarFlow_;
            }

            foreach (DrawMaterialStream ds in streamlist) //  add all the bc's
            {
                foreach (BaseComp bc in ds.Components)
                    bcc.Add(bc.Clone());
            }

            int waterlocation = 0;
            for (int i = 0; i < bcc.ComponentList.Count; i++)
            {
                if (bcc[i].Name == "Water")
                    waterlocation = i;
            }

            BaseComp water = bcc[waterlocation];
            bcc.Remove(water);
            bcc.Insert(water);

            bcc.NormaliseTempMolFlows();

            return bcc;
        }

        public DrawStreamCollection()
        {
            streamlist = new();
        }

        public DrawStreamCollection GetLiquidStreams()
        {
            DrawStreamCollection dstc = new();
            foreach (DrawMaterialStream ds in streamlist)
            {
                if (!ds.IsVapourDraw && ds.Active)
                    dstc.Add(ds);
            }
            return dstc;
        }

        public DrawStreamCollection GetVapourStreams()
        {
            DrawStreamCollection dstc = new();
            foreach (DrawMaterialStream ds in streamlist)
            {
                if (ds.IsVapourDraw)
                    dstc.Add(ds);
            }
            return dstc;
        }

        public DrawMaterialStream this[int index]
        {
            get
            {
                if (streamlist.Count > 0 && index >= 0 && index < streamlist.Count)
                {
                    return streamlist[index];
                }
                else
                    return
                        null;
            }
            set
            {
                streamlist[index] = value;
            }
        }

        public DrawMaterialStream this[string index]
        {
            get
            {
                for (int i = 0; i < streamlist.Count; i++)
                {
                    if (streamlist[i].Name == index)
                        return streamlist[i];
                }
                return null;
            }
        }

        public DrawMaterialStream this[Guid index]
        {
            get
            {
                for (int i = 0; i < streamlist.Count; i++)
                {
                    if (streamlist[i].Guid == index)
                        return streamlist[i];
                }
                return null;
            }
        }

        public int Count
        {
            get
            {
                return streamlist.Count;
            }
        }

        public string[] Names
        {
            get
            {
                string[] res = new string[streamlist.Count];

                for (int i = 0; i < streamlist.Count; i++)
                    res[i] = streamlist[i].Name;

                return res;
            }
        }

        public void Add(DrawMaterialStream pa)
        {
            streamlist.Add(pa);
        }

        public IEnumerator GetEnumerator()
        {
            return streamlist.GetEnumerator();
        }

        public class PAComparer : IComparer<DrawMaterialStream>
        {
            public int Compare(DrawMaterialStream x, DrawMaterialStream y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        // If x is null and y is null, they're
                        // equal.
                        return 0;
                    }
                    else
                    {
                        // If x is null and y is not null, y
                        // is greater.
                        return -1;
                    }
                }
                else
                {
                    // If x is not null...
                    //
                    if (y == null)
                    // ...and y is null, x is greater.
                    {
                        return 1;
                    }
                    else
                    {
                        // ...and y is not null, compare the
                        // lengths of the two strings.
                        //
                        int retval = x.EngineDrawTrayGuid.CompareTo(y.EngineDrawTrayGuid);

                        if (retval != 0)
                        {
                            // If the strings are not of equal length,
                            // the longer string is greater.
                            //
                            return retval;
                        }
                        else
                        {
                            // If the strings are of equal length,
                            // sort them with ordinary string comparison.
                            //
                            return x.EngineDrawTrayGuid.CompareTo(y.EngineDrawTrayGuid);
                        }
                    }
                }
            }
        }

        public class PAComparer2 : IComparer<DrawMaterialStream>
        {
            public int Compare(DrawMaterialStream x, DrawMaterialStream y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        // If x is null and y is null, they're
                        // equal.
                        return 0;
                    }
                    else
                    {
                        // If x is null and y is not null, y
                        // is greater.
                        return -1;
                    }
                }
                else
                {
                    // If x is not null...
                    //
                    if (y == null)
                    // ...and y is null, x is greater.
                    {
                        return 1;
                    }
                    else
                    {
                        // ...and y is not null, compare the
                        // lengths of the two strings.
                        //
                        int retval = x.EnginereturnTrayGuid.CompareTo(y.EnginereturnTrayGuid);

                        if (retval != 0)
                        {
                            // If the strings are not of equal length,
                            // the longer string is greater.
                            //
                            return retval;
                        }
                        else
                        {
                            // If the strings are of equal length,
                            // sort them with ordinary string comparison.
                            //
                            return x.EnginereturnTrayGuid.CompareTo(y.EnginereturnTrayGuid);
                        }
                    }
                }
            }
        }

        public void SortByDrawTray()
        {
            PAComparer dc = new();
            streamlist.Sort(dc);
        }

        public void SortByreturnTray()
        {
            PAComparer2 dc = new();
            streamlist.Sort(dc);
        }

        public void sort()
        {
            throw new NotImplementedException();
        }

        internal List<DrawMaterialStream> TheList()
        {
            return streamlist;
        }

        internal List<StreamMaterial> Streams()
        {
            List<StreamMaterial> res = new();
            for (int i = 0; i < streamlist.Count; i++)
            {
                res.Add(streamlist[i].Stream);
            }
            return res;
        }

        internal bool AllSolved()
        {
            foreach (DrawMaterialStream item in streamlist)
            {
                if (!item.Stream.Port.IsSolved)
                    return false;
            }
            return true;
        }
    }
}