using System;
using System.Collections;
using System.Collections.Generic;

namespace Units
{
    public class DrawColumnTraySectionCollection : IEnumerable<DrawColumnTraySection>
    {
        private List<DrawColumnTraySection> list = new();
        public Guid guid = Guid.NewGuid();

        public DrawColumnTraySection this[int index]
        {
            get
            {
                if (list.Count > 0 && index >= 0 && index < list.Count)
                    return list[index];
                else
                    return
                        null;
            }
            set
            {
                list[index] = value;
            }
        }

        public DrawColumnTraySection this[string index]
        {
            get
            {
                foreach (DrawColumnTraySection item in list)
                {
                    if (item.Name == index)
                        return item;
                }
                return null;
            }
        }

        public void Sort()
        {
            list.Sort((x, y) => y.Count.CompareTo(x.Count));
        }

        public DrawColumnTraySection this[Guid index]
        {
            get
            {
                foreach (DrawColumnTraySection pa in list)
                {
                    if (pa.Guid == index)
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

        public void Remove(DrawColumnTraySection dcts)
        {
            for (int n = 0; n < list.Count; n++)
                if (list[n].Guid == dcts.Guid)
                    list.Remove(list[n]);
        }

        public void Add(DrawColumnTraySection pa)
        {
            list.Add(pa);
        }

        public void Clear()
        {
            list.Clear();
        }

        public IEnumerator<DrawColumnTraySection> GetEnumerator()
        {
            return ((IEnumerable<DrawColumnTraySection>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DrawColumnTraySection>)list).GetEnumerator();
        }

        public class DSCTComparer : IComparer<DrawColumnTraySection>
        {
            public int Compare(DrawColumnTraySection x, DrawColumnTraySection y)
            {
                DrawMaterialStream xfeed, yfeed;
                yfeed = y.StripFeed;
                xfeed = x.StripFeed;

                if (x == null || xfeed == null)
                {
                    if (y == null || yfeed == null)

                        // If x is null and y is null, they're
                        // equal.
                        return 0;
                    else
                        // If x is null and y is not null, y
                        // is greater.
                        return -1;
                }
                else
                {
                    // If x is not null...
                    //
                    if (y == null || yfeed == null)  // ...and y is null, x is greater.
                        return 1;
                    else
                    {
                        // ...and y is not null, compare the
                        // lengths of the two strings.
                        //
                        int retval = x.StripperDrawTray.CompareTo(y.StripperDrawTray);
                        if (retval != 0)
                            // If the strings are not of equal length,
                            // the longer string is greater.
                            return retval;
                        else
                            // If the strings are of equal length,
                            // sort them with ordinary string comparison.
                            //
                            return x.StripperDrawTray.CompareTo(y.StripperDrawTray);
                    }
                }
            }
        }

        public void SortByDrawTray()
        {
            DSCTComparer dc = new DSCTComparer();
            list.Sort(dc);
        }

        internal void AddRange(DrawColumnTraySectionCollection strippers)
        {
            list.AddRange(strippers);
        }

        internal void AddRange(object strippers)
        {
            throw new NotImplementedException();
        }

        internal DrawColumnTraySectionCollection Strippers()
        {
            DrawColumnTraySectionCollection dcts = new();

            for (int i = 1; i < list.Count; i++)
            {
                dcts.Add(list[i]);
            }
            return dcts;
        }
    }
}