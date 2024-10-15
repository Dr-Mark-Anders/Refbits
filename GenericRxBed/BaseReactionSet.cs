using System.Collections.Generic;

namespace GenericRxBed
{
    public class BaseReactionSet
    {
        public List<BaseReaction> reactions = new List<BaseReaction>();
        public Dictionary<string, double> RateDict = new Dictionary<string, double>();

        public BaseReactionSet()
        {
        }

        //public Reactionthis[int index]{get=>((IList<Reaction>)reactions)[index];set=>((IList<Reaction>)reactions)[index]=value;}

        //public int Count=>((ICollection<Reaction>)reactions).Count;

        public bool Isreadonly => ((ICollection<BaseReaction>)reactions).IsReadOnly;

        public void Add(BaseReaction item)
        {
            ((ICollection<BaseReaction>)reactions).Add(item);
        }

        public void Clear()
        {
            ((ICollection<BaseReaction>)reactions).Clear();
        }

        public bool Contains(BaseReaction item)
        {
            return ((ICollection<BaseReaction>)reactions).Contains(item);
        }

        public void CopyTo(BaseReaction[] array, int arrayIndex)
        {
            ((ICollection<BaseReaction>)reactions).CopyTo(array, arrayIndex);
        }

        public IEnumerator<BaseReaction> GetEnumerator()
        {
            return ((IEnumerable<BaseReaction>)reactions).GetEnumerator();
        }

        public int IndexOf(BaseReaction item)
        {
            return ((IList<BaseReaction>)reactions).IndexOf(item);
        }

        public void Insert(int index, BaseReaction item)
        {
            ((IList<BaseReaction>)reactions).Insert(index, item);
        }

        public bool Remove(BaseReaction item)
        {
            return ((ICollection<BaseReaction>)reactions).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<BaseReaction>)reactions).RemoveAt(index);
        }
    }
}