using System.Collections.Generic;

namespace Extensions
{
    public class DictionaryPython<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public TValue get(TKey key, TValue def)
        {
            if (this.ContainsKey(key))
                return this[key];
            else
                return def;
        }

        /* public TValue get(TKey key, double def)
         {
             if (this.ContainsKey(key))
                 return this[key];
             else
                 return default(TValue);
         }*/

        public bool canget(TKey key, TValue def)
        {
            if (this.ContainsKey(key))
                return true;
            else
                return false;
        }

        public void update(Dictionary<TKey, TValue> values)
        {
            foreach (KeyValuePair<TKey, TValue> i in values)
            {
                this[i.Key] = i.Value;
            }
        }

        public DictionaryPython<TKey, TValue> Clone()
        {
            DictionaryPython<TKey, TValue> res = new DictionaryPython<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> i in this)
            {
                res[i.Key] = i.Value;
            }
            return res;
        }

        public Dictionary<TKey, TValue> copy()
        {
            return (Dictionary<TKey, TValue>)this.MemberwiseClone();
        }
    }
}