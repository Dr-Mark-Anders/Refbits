using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace ModelEngine
{
    public class StackObject<T>
    {
        internal List<T> stack = new();

        public int Count
        {
            get
            {
                return stack.Count;
            }
        }

        public T Pop()
        {
            T res = stack[0];
            stack.RemoveAt(0);
            return res;
        }

        public T Preview()
        {
            T res = stack[0];
            return res;
        }

        public T MoveFirstToLast()
        {
            T res = stack[0];
            stack.RemoveAt(0);
            stack.Insert(stack.Count, res);
            return res;
        }

        public bool Push(T value)
        {
            if (!stack.Contains(value)) // only allow unique items
            {
                stack.Insert(0, value);
                return true;
            }
            return false;
        }

        public List<T> Names
        {
            get
            {
                List<T> res = new();
                foreach (var item in stack)
                {
                    res.Add(item);
                }
                return res;
            }
        }

        public void Clear()
        {
            stack.Clear();
        }

        public bool Contains(T item)
        {
            return stack.Contains(item);
        }

        public T this[int index]
        {
            get { return stack[index]; }
        }
    }

    public class StackIUnitOP : StackObject<IUnitOperation>
    {
        public new List<string> Names
        {
            get
            {
                List<string> res = new();
                foreach (var item in stack)
                {
                    res.Add(item.Name);
                }
                return res;
            }
        }

        internal StackIUnitOP Clone()
        {
            StackIUnitOP newstack = new();

            foreach (UnitOperation item in stack)
            {
                newstack.Push((UnitOperation)item.Clone());
            }
            return newstack;
        }

        internal void Remove(UnitOperation sfs)
        {
            if (stack.Contains(sfs))
                stack.Remove(sfs);
        }

        internal int CountSubFlowsheets()
        {
            int count = 0;
            foreach (var item in stack)
            {
                switch (item)
                {
                    case HXSubFlowSheet:
                    case COlSubFlowSheet:
                    case SubFlowSheet:
                        count++;
                        break;
                    default:
                        break;
                }
            }
            return count;
        }

        internal int CountMainFlowsheetUO()
        {
            int count = 0;
            foreach (UnitOperation item in stack)
            {
                switch (item)
                {
                    case HXSubFlowSheet:
                    case COlSubFlowSheet:
                    case SubFlowSheet:
                        break;
                    default:
                        count++;
                        break;
                }
            }
            return count;
        }
    }
}