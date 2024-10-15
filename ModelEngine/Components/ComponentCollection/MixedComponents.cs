using System.Collections;
using System.Collections.Generic;

namespace ModelEngine
{
    public class MixedComponents : IList<MixedComponent>
    {
        private List<MixedComponent> Components = new();

        public MixedComponent this[int index] { get => ((IList<MixedComponent>)Components)[index]; set => ((IList<MixedComponent>)Components)[index] = value; }

        public MixedComponent this[string index]
        {
            get
            {
                return Components.Find(X => X.Name == index);
            }
        }

        public int Count => ((ICollection<MixedComponent>)Components).Count;

        public bool IsReadOnly => ((ICollection<MixedComponent>)Components).IsReadOnly;

        public void Add(MixedComponent item)
        {
            ((ICollection<MixedComponent>)Components).Add(item);
        }

        public void Add(MixedComponents mixedcomps)
        {
            Components.AddRange(mixedcomps);
        }

        public void AddCracked(List<string> cracks, Dictionary<string, double[]> crack)
        {
            foreach (KeyValuePair<string, double[]> cracked in crack)
            {
                MixedComponent mc = new(cracked.Key);
                for (int i = 0; i < cracks.Count; i++)
                {
                    BaseComp bc = Thermodata.GetComponent(cracks[i]);
                    mc.Add(bc.Clone(), cracked.Value[i]);
                }
                Components.Add(mc);
            }
        }

        public void Clear()
        {
            ((ICollection<MixedComponent>)Components).Clear();
        }

        public bool Contains(MixedComponent item)
        {
            return ((ICollection<MixedComponent>)Components).Contains(item);
        }

        public bool Contains(string v)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].Name == v)
                    return true;
            };
            return false;
        }

        public void CopyTo(MixedComponent[] array, int arrayIndex)
        {
            ((ICollection<MixedComponent>)Components).CopyTo(array, arrayIndex);
        }

        public IEnumerator<MixedComponent> GetEnumerator()
        {
            return ((IEnumerable<MixedComponent>)Components).GetEnumerator();
        }

        public int IndexOf(MixedComponent item)
        {
            return ((IList<MixedComponent>)Components).IndexOf(item);
        }

        public void Insert(int index, MixedComponent item)
        {
            ((IList<MixedComponent>)Components).Insert(index, item);
        }

        public BaseComp NewBaseComp(string v)
        {
            MixedComponent mc = Components.Find(p => p.Name == v);

            if (mc == null)
                return null;

            BaseComp bc = new();
            bc.CritP = mc.CritP;
            bc.CritT = mc.CritT;
            bc.CritV = mc.CritV;
            bc.CritZ = mc.CritZ;
            bc.Density = mc.Density;
            bc.MW = mc.MW;
            bc.Name = v;
            bc.SG_60F = mc.SG_60F;

            return bc;
        }

        public bool Remove(MixedComponent item)
        {
            return ((ICollection<MixedComponent>)Components).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<MixedComponent>)Components).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Components).GetEnumerator();
        }
    }
}