using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class CalcPropList : IEnumerable<KeyValuePair<string, CalcProperty>>, ISerializable
    {
        private Dictionary<string, CalcProperty> props = new Dictionary<string, CalcProperty>();

        public Dictionary<string, CalcProperty> Props { get => props; set => props = value; }

        public CalcPropList()
        {
        }

        public CalcPropList(SerializationInfo info, StreamingContext context)
        {
            props = (Dictionary<string, CalcProperty>)info.GetValue("props", typeof(Dictionary<string, CalcProperty>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("props", props);
        }

        [OnDeserialized]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
        }

        public CalcProperty this[string val]
        {
            get
            {
                if (props.ContainsKey(val))
                    return props[val];
                else
                    return null;
            }
            set
            {
                props[val] = value;
            }
        }

        public virtual CalcPropList Clone()
        {
            CalcPropList res = new CalcPropList();

            foreach (var i in props)
                res.props.Add(i.Key, i.Value);

            return res;
        }

        public void Clear()
        {
            props.Clear();
        }

        public bool Contains(string id)
        {
            if (props.ContainsKey(id))
                return true;
            else
                return false;
        }

        public IEnumerator GetEnumerator()
        {
            return props.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, CalcProperty>> IEnumerable<KeyValuePair<string, CalcProperty>>.GetEnumerator()
        {
            return props.GetEnumerator();
        }

        internal void Add(CalcProperty prop)
        {
            props[prop.DisplayName] = prop;
        }
    }
}