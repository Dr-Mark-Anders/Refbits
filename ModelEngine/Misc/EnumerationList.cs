using ModelEngine;
using System;
using System.Collections.Generic;

namespace ModelEngine
{
    public class EnumerationList : IList<UOMProperty>
    {
        private List<UOMProperty> Properties = new List<UOMProperty>();

        public UOMProperty this[Guid index]
        {
            get
            {
                foreach (var item in Properties)
                {
                    if (item.guid == index)
                        return item;
                }
                return null;
            }
        }

        public UOMProperty this[int index] { get => ((IList<UOMProperty>)Properties)[index]; set => ((IList<UOMProperty>)Properties)[index] = value; }

        public int Count => Properties.Count;

        public bool IsReadOnly => ((ICollection<UOMProperty>)Properties).IsReadOnly;

        public void Add(UOMProperty item, int col)
        {
            ((ICollection<UOMProperty>)Properties[col]).Add(item);
        }

        public void Add(UOMProperty item)
        {
            ((ICollection<UOMProperty>)Properties).Add(item);
        }

        public void Clear()
        {
            ((ICollection<UOMProperty>)Properties).Clear();
        }

        public bool Contains(UOMProperty item)
        {
            return ((ICollection<UOMProperty>)Properties).Contains(item);
        }

        public void CopyTo(UOMProperty[] array, int arrayIndex)
        {
            ((ICollection<UOMProperty>)Properties).CopyTo(array, arrayIndex);
        }

        public IEnumerator<UOMProperty> GetEnumerator()
        {
            return ((IEnumerable<UOMProperty>)Properties).GetEnumerator();
        }

        public int IndexOf(UOMProperty item)
        {
            return ((IList<UOMProperty>)Properties).IndexOf(item);
        }

        public void Insert(int index, UOMProperty item)
        {
            ((IList<UOMProperty>)Properties).Insert(index, item);
        }

        public bool Remove(UOMProperty item)
        {
            return ((ICollection<UOMProperty>)Properties).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<UOMProperty>)Properties).RemoveAt(index);
        }

        IEnumerator<UOMProperty> IEnumerable<UOMProperty>.GetEnumerator()
        {
            return ((IEnumerable<UOMProperty>)Properties).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}