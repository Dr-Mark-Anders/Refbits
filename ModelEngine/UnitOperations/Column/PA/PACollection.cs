using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class PumpAroundCollection : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private List<PumpAround> pas = new List<PumpAround>();

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            foreach (PumpAround t in pas)
            {
            }
        }

        public PumpAroundCollection()
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

        public PumpAroundCollection(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                pas = (List<PumpAround>)si.GetValue("PAList", typeof(List<PumpAround>));
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

        public PumpAround GetPA(Guid p)
        {
            foreach (PumpAround t in pas)
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
                foreach (PumpAround t in pas)
                    res.Add(t.DrawFactor);

                return res;
            }
        }

        public PumpAround this[string name]
        {
            get
            {
                foreach (PumpAround item in List)
                    if (item.Name == name)
                        return item;

                return null;
            }
        }

        public PumpAround this[Guid name]
        {
            get
            {
                foreach (PumpAround item in List)
                    if (item.Guid == name)
                        return item;

                return null;
            }
        }

        public PumpAround this[int index]
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

        public List<PumpAround> PAList
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

        public PumpAround Add(PumpAround t, bool active = true)
        {
            t.IsActive = active;
            pas.Add(t);
            return t;
        }

        public void AddRange(List<PumpAround> t)
        {
            pas.AddRange(t);
        }

        public void AddRange(PumpAroundCollection t)
        {
            if (t != null)
                pas.AddRange(t.PAList);
        }

        public void Insert(int index, PumpAround t)
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
                    pas.Insert(index, new PumpAround(null));
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

        public int IndexOf(PumpAround t)
        {
            return pas.IndexOf(t);
        }

        public void Remove(PumpAround t)
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

        internal PumpAroundCollection Clone()
        {
            throw new NotImplementedException();
        }

        private bool isspecified = false;

        public bool IsSpecified
        {
            get { return isspecified; }
            set { isspecified = value; }
        }

        public PumpAround TopPA
        {
            get
            {
                if (PAList.Count != 0)
                    return (PumpAround)PAList[0];
                return null;
            }
        }

        public PumpAround BottomPA
        {
            get
            {
                if (PAList.Count != 0)
                    return (PumpAround)pas[pas.Count - 1];
                return null;
            }
        }

        public List<PumpAround> List
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

        public List<PumpAround> Pas { get => pas; set => pas = value; }
    }
}