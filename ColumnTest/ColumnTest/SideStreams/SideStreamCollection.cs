using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngineTest
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class SideStreamCollectionTest : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private List<SideStreamTest> sideStreams = new List<SideStreamTest>();

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
        }

        public SideStreamCollectionTest()
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

        public SideStreamCollectionTest(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                sideStreams = (List<SideStreamTest>)si.GetValue("PAList", typeof(List<SideStreamTest>));
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

        public SideStreamTest GetPA(Guid p)
        {
            foreach (SideStreamTest t in sideStreams)
            {
                if (t.Guid == p)
                    return t;
            }
            return null;
        }

        public SideStreamTest this[Guid index]
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

        public SideStreamTest this[string index]
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

        public SideStreamTest this[int index]
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

        public List<SideStreamTest> SideStreamList
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

        public SideStreamTest Add(SideStreamTest t)
        {
            sideStreams.Add(t);
            return t;
        }

        public void AddRange(List<SideStreamTest> t)
        {
            sideStreams.AddRange(t);
        }

        public void AddRange(SideStreamCollectionTest t)
        {
            if (t != null)
                sideStreams.AddRange(t.sideStreams);
        }

        public void Insert(int index, SideStreamTest t)
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

        public int IndexOf(SideStreamTest t)
        {
            return sideStreams.IndexOf(t);
        }

        public void Remove(SideStreamTest t)
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

        internal SideStreamCollectionTest Clone()
        {
            SideStreamCollectionTest copy = new();

            foreach (SideStreamTest t in sideStreams)
            {
                copy.Add(t.Clone());
            }
            return copy;
        }

        internal SideStreamCollectionTest CloneDeep(COMColumn col)
        {
            SideStreamCollectionTest copy = new();

            foreach (SideStreamTest t in sideStreams)
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

        public SideStreamTest TopPA
        {
            get
            {
                if (SideStreamList.Count != 0)
                    return (SideStreamTest)SideStreamList[0];
                return null;
            }
        }

        public SideStreamTest BottomPA
        {
            get
            {
                if (SideStreamList.Count != 0)
                    return (SideStreamTest)sideStreams[sideStreams.Count - 1];
                return null;
            }
        }

        public List<SideStreamTest> List
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