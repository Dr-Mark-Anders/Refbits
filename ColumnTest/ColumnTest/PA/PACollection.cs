using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngineTest
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class PumpAroundCollectionTest : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private List<PumpAroundTest> pas = new List<PumpAroundTest>();

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            foreach (PumpAroundTest t in pas)
            {
            }
        }

        public PumpAroundCollectionTest()
        {
        }

        public void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                si.AddValue("PAList", pas);
            }
            catch
            {
            }
        }

        public PumpAroundCollectionTest(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                pas = (List<PumpAroundTest>)si.GetValue("PAList", typeof(List<PumpAroundTest>));
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
            return pas.GetEnumerator();
        }

        public PumpAroundTest GetPA(Guid p)
        {
            foreach (PumpAroundTest t in pas)
            {
                if (t.Guid == p)
                    return t;
            }
            return null;
        }

        public List<double> PAFactors
        {
            get
            {
                List<double> res = new List<double>();
                foreach (PumpAroundTest t in pas)
                    res.Add(t.DrawFactor);

                return res;
            }
        }

        public PumpAroundTest this[string name]
        {
            get
            {
                foreach (PumpAroundTest item in List)
                    if (item.Name == name)
                        return item;

                return null;
            }
        }

        public PumpAroundTest this[Guid name]
        {
            get
            {
                foreach (PumpAroundTest item in List)
                    if (item.Guid == name)
                        return item;

                return null;
            }
        }

        public PumpAroundTest this[int index]
        {
            get
            {
                if (index < this.Count && index >= 0)
                {
                    return pas[index];
                }
                else
                    return null;
            }
            set
            {
                pas[index] = value;
            }
        }

        public List<PumpAroundTest> PAList
        {
            get { return pas; }
            set { pas = value; }
        }

        public void Clear()
        {
            pas.Clear();
        }

        public object GetList()
        {
            return pas;
        }

        public PumpAroundTest Add(PumpAroundTest t, bool active = true)
        {
            t.IsActive = active;
            pas.Add(t);
            return t;
        }

        public void AddRange(List<PumpAroundTest> t)
        {
            pas.AddRange(t);
        }

        public void AddRange(PumpAroundCollectionTest t)
        {
            if (t != null)
                pas.AddRange(t.PAList);
        }

        public void Insert(int index, PumpAroundTest t)
        {
            if (index >= 0)
            {
                pas.Insert(index, t);
            }
        }

        public void Insert(int index, int t)
        {
            if (index >= 0)
            {
                for (int n = 0; n < t; n++)
                {
                    pas.Insert(index, new PumpAroundTest(null));
                }
            }
        }

        public void UpdateExternalStreams()
        {
            for (int n = 0; n < pas.Count; n++)
            {
                pas[n].UpdateStreams();
            }
        }

        public void Sort()
        {
            pas.Sort();
        }

        public int Count
        {
            get
            {
                return pas.Count;
            }
        }

        public int IndexOf(PumpAroundTest t)
        {
            return pas.IndexOf(t);
        }

        public void Remove(PumpAroundTest t)
        {
            if (pas.Count > 1)
            {
                pas.Remove(t);
            }
        }

        public void RemoveAt(int index)
        {
            if (pas.Count > 1)
            {
                pas.RemoveAt(index);
            }
        }

        public void RemoveAt(int index, int NoPAs)
        {
            if (pas.Count > 1 && pas.Count > NoPAs)
            {
                for (int n = 0; n < NoPAs; n++)
                    pas.RemoveAt(pas.Count - 1);
            }
        }

        internal void EraseExternalStreams()
        {
            for (int n = 0; n < pas.Count; n++)
            {
                pas[n].EraseStreams();
            }
        }

        internal PumpAroundCollectionTest Clone()
        {
            throw new NotImplementedException();
        }

        private bool isspecified = false;

        public bool IsSpecified
        {
            get { return isspecified; }
            set { isspecified = value; }
        }

        public PumpAroundTest TopPA
        {
            get
            {
                if (PAList.Count != 0)
                    return (PumpAroundTest)PAList[0];
                return null;
            }
        }

        public PumpAroundTest BottomPA
        {
            get
            {
                if (PAList.Count != 0)
                    return (PumpAroundTest)pas[pas.Count - 1];
                return null;
            }
        }

        public List<PumpAroundTest> List
        {
            get
            {
                return pas;
            }
            set
            {
                pas = value;
            }
        }

        public List<PumpAroundTest> Pas { get => pas; set => pas = value; }
    }
}