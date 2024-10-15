using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Units
{
    public class DrawPACollection : IEnumerable<DrawPA>
    {
        private List<DrawPA> list = new List<DrawPA>();

        public DrawPACollection()
        {
            list = new List<DrawPA>();
        }

        public class PAComparer : IComparer<DrawPA>
        {
            public int Compare(DrawPA x, DrawPA y)
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
                        int retval = x.DrawTrayDrawGuid.CompareTo(y.DrawTrayDrawGuid);

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
                            return x.DrawTrayDrawGuid.CompareTo(y.DrawTrayDrawGuid);
                        }
                    }
                }
            }
        }

        public void SortByDrawTray()
        {
            PAComparer dc = new PAComparer();
            list.Sort(dc);
        }

        public DrawPA this[int section, int pa]    // Indexer declaration
        {
            get
            {
                DrawPACollection pac = this.GetPAs(section);
                if (pa >= 0 && pa < pac.Count)
                    return pac[pa];
                return null;
            }
        }

        public DrawPACollection GetPAs(int section)
        {
            DrawPACollection dpc = new DrawPACollection();

            foreach (DrawPA dpa in list)
            {
                if (dpa.DrawSectionNo == section)
                    dpc.Add(dpa);
            }
            return dpc;
        }

        /// <summary>
        /// empty
        /// </summary>
        /// <param name="section"></param>
        public DrawPA this[int index]
        {
            get
            {
                if (list.Count > 0 && index >= 0 && index < list.Count)
                {
                    return list[index];
                }
                else
                    return
                        null;
            }
            set
            {
                list[index] = value;
            }
        }

        public DrawPA this[Guid index]
        {
            get
            {
                foreach (DrawPA pa in list)
                {
                    if (pa.Guid == index)
                        return pa;
                }
                return null;
            }
        }

        public DrawPA this[string index]
        {
            get
            {
                foreach (DrawPA pa in list)
                {
                    if (pa.Name == index)
                        return pa;
                }
                return null;
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public void Remove(string Name)
        {
            for (int n = 0; n < list.Count; n++)
            {
                if (list[n].Name == Name)
                {
                    list.Remove(list[n]);
                }
            }
        }

        public void Add(DrawPA pa)
        {
            list.Add(pa);
        }

        public void Clear()
        {
            list.Clear();
        }

        public IEnumerator<DrawPA> GetEnumerator()
        {
            return ((IEnumerable<DrawPA>)list).GetEnumerator();
        }

        internal void Add(PumpAroundCollection getAllPumpArounds)
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        internal DrawStreamCollection DrawStreams()
        {
            DrawStreamCollection res = new();
            foreach (DrawPA pa in list)
            {
                res.Add(pa.feed);
            }

            return res;
        }

        internal DrawStreamCollection ReturnStreams()
        {
            DrawStreamCollection res = new();
            foreach (DrawPA pa in list)
            {
                res.Add(pa.effluent);
            }

            return res;
        }
    }
}