using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ModelEngine
{
    [TypeConverter(typeof(ConnectedPorts)), Description("Expand to see Port Details")]
    public class ConnectedPorts : ExpandableObjectConverter, IList<Port>
    {
        private List<Port> Ports = new();

        public ConnectedPorts()
        {
        }

        public void Add(Port item)
        {
            if (Ports.Count > 1)
                return;
            if (!Ports.Contains(item))
                Ports.Add(item);
        }

        internal void Add(ConnectedPorts connectedPorts)
        {
            foreach (var item in connectedPorts)
            {
                Add(item);
            }
        }

        public bool IsConnected
        {
            get
            {
                if (Ports.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public bool HasMultipleConnections
        {
            get
            {
                if (Ports.Count > 1)
                    return true;
                else
                    return false;
            }
        }

        public Port this[int index]
        {
            get
            {
                if (index >= Ports.Count)
                    return null;
                else
                    return ((IList<Port>)Ports)[index];
            }
            set
            {
                if (index > Ports.Count)
                    Ports.Add(value);
                else
                    ((IList<Port>)Ports)[index] = value;
            }
        }

        bool ICollection<Port>.IsReadOnly => false;

        public int Count => ((ICollection<Port>)Ports).Count;

        public List<Port> Ports1 { get => Ports; set => Ports = value; }

        public void Clear()
        {
            Ports.Clear();
        }

        public bool Contains(Port item)
        {
            return ((ICollection<Port>)Ports).Contains(item);
        }

        public void CopyTo(Port[] array, int arrayIndex)
        {
            ((ICollection<Port>)Ports).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Port> GetEnumerator()
        {
            return ((IEnumerable<Port>)Ports).GetEnumerator();
        }

        public int IndexOf(Port item)
        {
            return ((IList<Port>)Ports).IndexOf(item);
        }

        public void Insert(int index, Port item)
        {
            ((IList<Port>)Ports).Insert(index, item);
        }

        public bool Remove(Port item)
        {
            return ((ICollection<Port>)Ports).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Port>)Ports).RemoveAt(index);
        }

        void ICollection<Port>.Add(Port item)
        {
            Ports.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Ports).GetEnumerator();
        }
    }
}