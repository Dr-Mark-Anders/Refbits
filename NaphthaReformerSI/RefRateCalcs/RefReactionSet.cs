using System.Collections.Generic;

namespace RefReactor
{
    public class RefReactionSet
    {
        public List<RefReaction> reactions = new List<RefReaction>();
        public Dictionary<string, double> RateDict = new Dictionary<string, double>();

        public RefReactionSet()
        {
        }

        //public  Reaction this[int  index] { get => ((IList<Reaction>)reactions)[index]; set => ((IList<Reaction>)reactions)[index] = value; }

        //public  int  Count => ((ICollection<Reaction>)reactions).Count;

        public bool IsReadOnly => ((ICollection<RefReaction>)reactions).IsReadOnly;

        public void Add(RefReaction item)
        {
            ((ICollection<RefReaction>)reactions).Add(item);
        }

        public void Clear()
        {
            ((ICollection<RefReaction>)reactions).Clear();
        }

        public bool Contains(RefReaction item)
        {
            return ((ICollection<RefReaction>)reactions).Contains(item);
        }

        public void CopyTo(RefReaction[] array, int arrayIndex)
        {
            ((ICollection<RefReaction>)reactions).CopyTo(array, arrayIndex);
        }

        public IEnumerator<RefReaction> GetEnumerator()
        {
            return ((IEnumerable<RefReaction>)reactions).GetEnumerator();
        }

        public int IndexOf(RefReaction item)
        {
            return ((IList<RefReaction>)reactions).IndexOf(item);
        }

        public void Insert(int index, RefReaction item)
        {
            ((IList<RefReaction>)reactions).Insert(index, item);
        }

        public bool Remove(RefReaction item)
        {
            return ((ICollection<RefReaction>)reactions).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<RefReaction>)reactions).RemoveAt(index);
        }
    }
}