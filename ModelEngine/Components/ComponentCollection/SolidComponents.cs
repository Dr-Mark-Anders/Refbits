using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using Units.UOM;

namespace ModelEngine
{
    public class SolidComponents : IList<SolidComponent>
    {
        private List<SolidComponent> Components = new();

        public SolidComponents()
        {
        }

        public SolidComponent this[int index] { get => Components[index]; set => Components[index] = value; }

        public SolidComponent this[string index]
        {
            get
            {
                return Components.Find(X => X.Name == index);
            }
        }

        public int Count => Components.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public MoleFlow MolarFlow { get; internal set; }

        public void Add(SolidComponent item)
        {
            Components.Add(item);
        }

        public void Clear()
        {
            Components.Clear();
        }

        public SolidComponents Clone()
        {
            SolidComponents res = new SolidComponents();
            res.Components = (List<SolidComponent>)Components.Clone();
            return res;
        }

        public bool Contains(SolidComponent item)
        {
            return Components.Contains(item);
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

        public void CopyTo(SolidComponent[] array, int arrayIndex)
        {
            Components.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SolidComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        public int IndexOf(SolidComponent item)
        {
            return Components.IndexOf(item);
        }

        public void Insert(int index, SolidComponent item)
        {
            Components.Insert(index, item);
        }

        public bool Remove(SolidComponent item)
        {
            return Components.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Components.RemoveAt(index);
        }

        public Enthalpy H()
        {
            Enthalpy res = 0;
            for (int i = 0; i < Components.Count; i++)
            {
                res += Components[i].CPMass * Components[i].MassFraction;
            }
            return res;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        internal MoleFlow MW()
        {
            double res = 0;
            for (int i = 0; i < Components.Count; i++)
            {
                res += Components[i].MW;
            };
            return res;
        }
    }
}