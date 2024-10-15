using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class SideStreamCollection : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private List<SideStream> sideStreams = new List<SideStream>();

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
        }

        public SideStreamCollection()
        {
        }

        public void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                si.AddValue("PAList", sideStreams);
            }
            catch
            {
            }
        }

        public SideStreamCollection(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                sideStreams = (List<SideStream>)si.GetValue("PAList", typeof(List<SideStream>));
            }
            catch
            {
            }
        }

        private int top;

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return sideStreams.GetEnumerator();
        }

        public SideStream GetPA(Guid p)
        {
            foreach (SideStream t in sideStreams)
            {
                if (t.Guid == p)
                    return t;
            }
            return null;
        }

        public SideStream this[Guid index]
        {
            get
            {
                foreach (var item in sideStreams)
                {
                    if (item.Guid == index)
                        return item;
                }
                return null;
            }
        }

        public SideStream this[string index]
        {
            get
            {
                foreach (var item in sideStreams)
                {
                    if (item.Name == index)
                        return item;
                }
                return null;
            }
        }

        public SideStream this[int index]
        {
            get
            {
                if (index < this.Count && index >= 0)
                {
                    return sideStreams[index];
                }
                else
                    return null;
            }
            set
            {
                sideStreams[index] = value;
            }
        }

        public List<SideStream> SideStreamList
        {
            get { return sideStreams; }
            set { sideStreams = value; }
        }

        public void Clear()
        {
            sideStreams.Clear();
        }

        public object GetList()
        {
            return sideStreams;
        }

        public SideStream Add(SideStream t)
        {
            sideStreams.Add(t);
            return t;
        }

        public void AddRange(List<SideStream> t)
        {
            sideStreams.AddRange(t);
        }

        public void AddRange(SideStreamCollection t)
        {
            if (t != null)
                sideStreams.AddRange(t.sideStreams);
        }

        public void Insert(int index, SideStream t)
        {
            if (index >= 0)
            {
                sideStreams.Insert(index, t);
            }
        }

        public int Count
        {
            get
            {
                return sideStreams.Count;
            }
        }

        public int IndexOf(SideStream t)
        {
            return sideStreams.IndexOf(t);
        }

        public void Remove(SideStream t)
        {
            if (sideStreams.Count > 1)
            {
                sideStreams.Remove(t);
            }
        }

        public void RemoveAt(int index)
        {
            if (sideStreams.Count > 1)
            {
                sideStreams.RemoveAt(index);
            }
        }

        public void RemoveAt(int index, int NoPAs)
        {
            if (sideStreams.Count > 1 && sideStreams.Count > NoPAs)
            {
                for (int n = 0; n < NoPAs; n++)
                    sideStreams.RemoveAt(sideStreams.Count - 1);
            }
        }

        internal SideStreamCollection Clone()
        {
            SideStreamCollection copy = new();

            foreach (SideStream t in sideStreams)
            {
                copy.Add(t.Clone());
            }
            return copy;
        }

        internal SideStreamCollection CloneDeep(Column col)
        {
            SideStreamCollection copy = new();

            foreach (SideStream t in sideStreams)
            {
                copy.Add(t.CloneDeep(col));
            }
            return copy;
        }

        private bool isspecified = false;

        public bool IsSpecified
        {
            get { return isspecified; }
            set { isspecified = value; }
        }

        public SideStream TopPA
        {
            get
            {
                if (SideStreamList.Count != 0)
                    return (SideStream)SideStreamList[0];
                return null;
            }
        }

        public SideStream BottomPA
        {
            get
            {
                if (SideStreamList.Count != 0)
                    return (SideStream)sideStreams[sideStreams.Count - 1];
                return null;
            }
        }

        public List<SideStream> List
        {
            get
            {
                return sideStreams;
            }
            set
            {
                sideStreams = value;
            }
        }
    }
}