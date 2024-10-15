using System;
using System.Collections;
using System.Collections.Generic;
using Units;

namespace ModelEngine
{
    public class UOMPropertyList : IList<Tuple<UOMProperty, string>>
    {
        private List<Tuple<UOMProperty, string>> TheList = new List<Tuple<UOMProperty, string>>();

        public UOMPropertyList()
        {
        }

        public UOMPropertyList(List<Tuple<UOMProperty, string>> theList)
        {
            TheList = theList;
        }

        public UOMPropertyList(UOMPropertyList uOMPropertyList)
        {
            foreach (var item in TheList)
            {
                TheList.Add(Tuple.Create(item.Item1, item.Item2));
            }
        }

        public void Add(UOMProperty prop)
        {
            TheList.Add(Tuple.Create(prop, ""));
        }

        public void Add(UOMProperty prop, string str)
        {
            TheList.Add(Tuple.Create(prop, str));
        }

        public void Add(ePropID prop, SourceEnum origin, double value, string str)
        {
            TheList.Add(Tuple.Create(new UOMProperty(prop, origin, value, str), str));
        }

        public int IndexOf(Tuple<UOMProperty, string> item)
        {
            return ((IList<Tuple<UOMProperty, string>>)TheList).IndexOf(item);
        }

        public void Insert(int index, Tuple<UOMProperty, string> item)
        {
            ((IList<Tuple<UOMProperty, string>>)TheList).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Tuple<UOMProperty, string>>)TheList).RemoveAt(index);
        }

        public void Add(Tuple<UOMProperty, string> item)
        {
            ((ICollection<Tuple<UOMProperty, string>>)TheList).Add(item);
        }

        public void Clear()
        {
            ((ICollection<Tuple<UOMProperty, string>>)TheList).Clear();
        }

        public bool Contains(Tuple<UOMProperty, string> item)
        {
            return ((ICollection<Tuple<UOMProperty, string>>)TheList).Contains(item);
        }

        public void CopyTo(Tuple<UOMProperty, string>[] array, int arrayIndex)
        {
            ((ICollection<Tuple<UOMProperty, string>>)TheList).CopyTo(array, arrayIndex);
        }

        public bool Remove(Tuple<UOMProperty, string> item)
        {
            return ((ICollection<Tuple<UOMProperty, string>>)TheList).Remove(item);
        }

        public IEnumerator<Tuple<UOMProperty, string>> GetEnumerator()
        {
            return ((IEnumerable<Tuple<UOMProperty, string>>)TheList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)TheList).GetEnumerator();
        }

        public int Count
        {
            get
            {
                return TheList.Count;
            }
        }

        internal UOMPropertyList Clone()
        {
            UOMPropertyList uom = new UOMPropertyList(this);
            return uom;
        }

        public bool IsReadOnly => ((ICollection<Tuple<UOMProperty, string>>)TheList).IsReadOnly;

        public Tuple<UOMProperty, string> this[int index] { get => ((IList<Tuple<UOMProperty, string>>)TheList)[index]; set => ((IList<Tuple<UOMProperty, string>>)TheList)[index] = value; }
    }
}