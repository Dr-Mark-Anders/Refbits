using System;
using System.Collections;
using System.Collections.Generic;

namespace ModelEngine.ThermodynamicMethods.UNIFAC
{
    public class PRIMARY_DATA : IComparable<string>, IComparable<int>
    {
        public string name;
        public double Q, R;
        public int Index;
        public int PRIM_LOCATION;
        public int SEC_LOCATION;

        public PRIMARY_DATA(string name, double r, double q, int index, int PRIM_LOCATION, int sec_loc)
        {
            this.name = name;
            R = r;
            Q = q;
            this.PRIM_LOCATION = PRIM_LOCATION;
            this.SEC_LOCATION = sec_loc;
            Index = index;
        }

        public PRIMARY_DATA(string name, Tuple<double, double, int, int, int> data)
        {
            this.name = name;
            R = data.Item1;
            Q = data.Item2;
            Index = data.Item3;
            PRIM_LOCATION = data.Item4;
            SEC_LOCATION = data.Item5;
        }

        public int CompareTo(string other)
        {
            return name.CompareTo(other);
        }

        public int CompareTo(int other)
        {
            return PRIM_LOCATION.CompareTo(other);
        }
    }

    public class PrimaryDataCollection : IList<PRIMARY_DATA>
    {
        private List<PRIMARY_DATA> list = new();

        public PRIMARY_DATA this[int index] { get => list[index]; set => list[index] = value; }

        public PRIMARY_DATA this[string index]
        {
            get
            {
                foreach (PRIMARY_DATA item in list)
                {
                    if (item.name == index)
                        return item;
                }
                return null;
            }
        }

        public void SortByBiLocation()
        {
            list.Sort((a, b) => a.PRIM_LOCATION.CompareTo(b.PRIM_LOCATION));
        }

        public void SortByIndex()
        {
            list.Sort((a, b) => a.Index.CompareTo(b.Index));
        }

        public string[] Names
        {
            get
            {
                List<string> res = new List<string>();

                for (int i = 0; i < list.Count; i++)
                {
                    res.Add(list[i].name);
                }
                return res.ToArray();
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(PRIMARY_DATA item)
        {
            list.Add(item);
        }

        public void Add(string name, Tuple<double, double, int, int, int> data)
        {
            PRIMARY_DATA pd = new PRIMARY_DATA(name, data);
            list.Add(pd);
        }

        public void Add(string name, double R, double Q, int index, int PGroup, int SGroup)
        {
            PRIMARY_DATA pd = new PRIMARY_DATA(name, R, Q, index, PGroup, SGroup);
            list.Add(pd);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(PRIMARY_DATA item)
        {
            return list.Contains(item);
        }

        public bool Contains(string name)
        {
            foreach (PRIMARY_DATA pd in list)
            {
                if (pd.name == name)
                    return true;
            }
            return false;
        }

        public void CopyTo(PRIMARY_DATA[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PRIMARY_DATA> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(PRIMARY_DATA item)
        {
            return list.IndexOf(item);
        }

        public int IndexOf(string name)
        {
            foreach (PRIMARY_DATA item in list)
            {
                if (item.name == name)
                    return list.IndexOf(item);
            }
            return -1;
        }

        public void Insert(int index, PRIMARY_DATA item)
        {
            list.Insert(index, item);
        }

        public bool Remove(PRIMARY_DATA item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}