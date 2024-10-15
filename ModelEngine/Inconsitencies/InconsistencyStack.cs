using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    public class InconsistencyStack : List<InconsistencyObject>
    {
        public new void Add(InconsistencyObject io)
        {
            if (io != null)
                base.Add(io);
        }
    }

    [Serializable]
    public class ModelStack : IList<UnitOperation>, ISerializable
    {
        private List<UnitOperation> modelList = new List<UnitOperation>();

        public void Add(IUnitOperation uo)
        {
            modelList.Add((UnitOperation)uo);
        }

        public ModelStack Clone()
        {
            ModelStack ms = new();

            for (int i = 0; i < modelList.Count; i++)
            {
                ms.Add((UnitOperation)modelList[i].Clone());
            }

            return ms;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("List", modelList);
        }

        public ModelStack(SerializationInfo info, StreamingContext context)
        {
            modelList = (List<UnitOperation>)info.GetValue("List", typeof(List<UnitOperation>));
        }

        public ModelStack()
        {
        }

        public int IndexOf(UnitOperation item)
        {
            return modelList.IndexOf(item);
        }

        public void Insert(int index, IUnitOperation item)
        {
            ((IList<IUnitOperation>)modelList).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<IUnitOperation>)modelList).RemoveAt(index);
        }

        public void Clear()
        {
            ((ICollection<UnitOperation>)modelList).Clear();
        }

        public bool Contains(IUnitOperation item)
        {
            return ((ICollection<IUnitOperation>)modelList).Contains(item);
        }

        public void CopyTo(IUnitOperation[] array, int arrayIndex)
        {
            ((ICollection<IUnitOperation>)modelList).CopyTo(array, arrayIndex);
        }

        public bool Remove(IUnitOperation item)
        {
            return ((ICollection<IUnitOperation>)modelList).Remove(item);
        }

        public IEnumerator<IUnitOperation> GetEnumerator()
        {
            return ((IEnumerable<IUnitOperation>)modelList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)modelList).GetEnumerator();
        }

        public void Insert(int index, UnitOperation item)
        {
            ((IList<UnitOperation>)modelList).Insert(index, item);
        }

        public void Add(UnitOperation item)
        {
            ((ICollection<UnitOperation>)modelList).Add(item);
        }

        public bool Contains(UnitOperation item)
        {
            return ((ICollection<UnitOperation>)modelList).Contains(item);
        }

        public void CopyTo(UnitOperation[] array, int arrayIndex)
        {
            ((ICollection<UnitOperation>)modelList).CopyTo(array, arrayIndex);
        }

        public bool Remove(UnitOperation item)
        {
            return ((ICollection<UnitOperation>)modelList).Remove(item);
        }

        IEnumerator<UnitOperation> IEnumerable<UnitOperation>.GetEnumerator()
        {
            return ((IEnumerable<UnitOperation>)modelList).GetEnumerator();
        }

        public List<string> Names
        {
            get
            {
                List<string> res = new List<string>();
                foreach (var item in this)
                {
                    res.Add(item.Name);
                }

                return res;
            }
        }

        public int Count => ((ICollection<UnitOperation>)modelList).Count;

        public bool IsReadOnly => ((ICollection<UnitOperation>)modelList).IsReadOnly;

        UnitOperation IList<UnitOperation>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IUnitOperation this[int index]
        {
            get
            {
                return modelList[index];
            }

            set
            {
                modelList[index] = (UnitOperation)value;
            }
        }

        public IUnitOperation this[Guid guid]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.Guid == guid)
                        return item;
                }
                return null;
            }
        }

        public IUnitOperation this[string name]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.Name == name)
                        return item;
                }
                return null;
            }
        }

        public void SetDirty()
        {
            foreach (var model in modelList)
            {
                model.IsDirty = true;
            };
        }
    }
}