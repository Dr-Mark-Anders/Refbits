using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    public class CaseStudies : IList<CaseStudy>

    {
        public CaseStudy this[int index] { get => this[index]; set => this[index] = value; }

        public int Count => this.Count;

        public bool Isreadonly => this.Isreadonly;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(CaseStudy item)
        {
            this.Add(item);
        }

        public void Clear()
        {
            this.Clear();
        }

        public bool Contains(CaseStudy item)
        {
            return this.Contains(item);
        }

        public void CopyTo(CaseStudy[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CaseStudy> GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int IndexOf(CaseStudy item)
        {
            return this.IndexOf(item);
        }

        public void Insert(int index, CaseStudy item)
        {
            this.Insert(index, item);
        }

        public bool Remove(CaseStudy item)
        {
            return this.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    [Serializable]
    public class CaseStudy : UnitOperation, ISerializable

    {
        private caseSets caseSets = new();
        private caseOutputVariables caseOutput = new();
        private caseResult caseResult = new();

        protected CaseStudy(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            //thrownew NotImplementedException();
        }

        public CaseStudy()
        {
        }

        public bool IsEmpty()
        {
            if (caseSets == null || caseSets.Count == 0)
                return true;
            else return false;
        }

        public caseResults CaseResults = new();
        public caseSets CaseSets { get => caseSets; set => caseSets = value; }

        public caseOutputVariables CaseOutput
        { get => caseOutput; set => caseOutput = value; }

        public void AddSet(CaseSet set)
        {
            caseSets.AddValue(set);
        }

        public void Runcases(FlowSheet flowsheet)
        {
            for (int i = 0; i < caseSets.Count; i++)
            {
                CaseSet Set = caseSets[i];
                Solve(flowsheet, Set);
                CaseResults.AddValue(caseResult);
            }
        }

        public void AddOutput(StreamProperty prop)
        {
            caseResult.AddValue(prop);
        }

        private void Solve(FlowSheet flowSheet, CaseSet set)
        {
            StreamProperty prop;
            caseResult = new();

            for (int propNo = 0; propNo < set.InData.Count; propNo++)
            {
                prop = set[propNo].Item1;
                double value = set[propNo].Item2;
                if (prop != null)
                    prop.BaseValue = value;
            }

            flowSheet.Solve();

            for (int propNo = 0; propNo < caseOutput.Count; propNo++)
            {
                prop = caseOutput.PropList[propNo];
                double value;

                if (prop != null)
                {
                    value = prop.BaseValue;
                    caseResult.AddValue(value);
                }
            }
        }

        public void Clear()
        {
            caseSets.Clear();
            caseOutput.Clear();
            caseResult.Clear();
            CaseResults.Clear();
        }
    }

    [Serializable]
    public class caseSets

    {
        private List<CaseSet> sets = new();

        public int Count { get => sets.Count; }

        public CaseSet this[int i]
        {
            get => sets[i];
        }

        public void AddValue(CaseSet set)
        {
            sets.Add(set);
        }

        internal void Clear()
        {
            sets.Clear();
        }
    }

    [Serializable]
    public class CaseSet

    {
        public List<Tuple<StreamProperty, double>> InData = new();

        public Tuple<StreamProperty, double> this[int i]
        {
            get { return InData[i]; }
        }

        public int Count { get => InData.Count; }

        public void AddValue(StreamProperty prop, double value)
        {
            InData.Add(Tuple.Create(prop, value));
        }
    }

    [Serializable]
    public class caseResults

    {
        private List<caseResult> results = new();

        public void AddValue(caseResult value)
        {
            results.Add(value);
        }

        public caseResult this[int i]
        {
            get { return results[i]; }
        }

        public int Count
        {
            get
            {
                return results.Count;
            }
        }

        internal void Clear()
        {
            results.Clear();
        }
    }

    [Serializable]
    public class caseResult

    {
        private List<double> results = new List<double>();

        public int Count { get => results.Count; }

        public void AddValue(double value)
        {
            results.Add(value);
        }

        internal void Clear()
        {
            results.Clear();
        }
    }

    [Serializable]
    public class caseOutputVariables

    {
        public List<StreamProperty> PropList = new();

        public int Count
        {
            get => PropList.Count;
        }

        public void Clear()
        {
            PropList.Clear();
        }

        public void AddOutput(StreamProperty prop)
        {
            PropList.Add(prop);
        }
    }
}