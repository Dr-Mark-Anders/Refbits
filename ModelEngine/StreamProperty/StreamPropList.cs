using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;

namespace ModelEngine
{
    [Serializable, TypeConverter(typeof(StreamPropListExpander))]
    public class StreamPropList : IEnumerable<KeyValuePair<ePropID, StreamProperty>>, ISerializable, IEquatable<StreamPropList>
    {
        private Dictionary<ePropID, StreamProperty> props = new();

        public Dictionary<ePropID, StreamProperty> Props { get => props; set => props = value; }

        public StreamPropList()
        {
            Props[ePropID.MF] = new StreamProperty(ePropID.MF);
            Props[ePropID.VF] = new StreamProperty(ePropID.VF);
            Props[ePropID.MOLEF] = new StreamProperty(ePropID.MOLEF);
            Props[ePropID.T] = new StreamProperty(ePropID.T);
            Props[ePropID.P] = new StreamProperty(ePropID.P);
            Props[ePropID.Q] = new StreamProperty(ePropID.Q);
            Props[ePropID.H] = new StreamProperty(ePropID.H);
            Props[ePropID.S] = new StreamProperty(ePropID.S);
        }

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        public void SetPort(Port p)
        {
            foreach (StreamProperty item in props.Values)
            { 
                if(item is not null)
                     item.Port = p;
            }
        }

        protected virtual void RaiseChangeEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public int CountValid
        {
            get
            {
                int res = 0;
                foreach (var item in props)
                {
                    if (item.Value is not null && item.Value.IsKnown)
                        res++;
                }
                return res;
            }
        }

        public List<ePropID> OutProps()
        {
            List<ePropID> list = new();
            foreach (var item in props.Values)
            {
                if (item.Source == SourceEnum.Transferred)
                    list.Add(item.Propid);
            }
            return list;
        }

        public List<ePropID> TransferredProps()
        {
            List<ePropID> list = new();
            foreach (var item in props.Values)
            {
                if (item.Source == SourceEnum.Transferred)
                    list.Add(item.Propid);
            }
            return list;
        }

        public List<ePropID> InProps()
        {
            List<ePropID> list = new();
            foreach (var item in props.Values)
            {
                if (item.Source == SourceEnum.Transferred || item.Source == SourceEnum.Empty)
                    list.Add(item.Propid);
            }
            return list;
        }

        public static List<ePropID> IntrinsicProps()
        {
            List<ePropID> list = new();
            list.Add(ePropID.T);
            list.Add(ePropID.P);
            list.Add(ePropID.MOLEF);
            list.Add(ePropID.MF);
            list.Add(ePropID.VF);
            list.Add(ePropID.H);
            list.Add(ePropID.S);
            list.Add(ePropID.Q);
            return list;
        }

        internal void EraseSignalTransfer()
        {
            foreach (var item in props.Values)
            {
                if (item.origin == SourceEnum.SignalTransfer)
                    item.Clear();
            }
        }

        public StreamPropList(SerializationInfo info, StreamingContext context)
        {
            props = (Dictionary<ePropID, StreamProperty>)info.GetValue("props", typeof(Dictionary<ePropID, StreamProperty>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("props", props);
        }

        public StreamProperty this[ePropID val]
        {
            get
            {
                if (props.ContainsKey(val))
                    return props[val];
                else
                {
                    props[val] = new StreamProperty(val, double.NaN); // allways return something, not null
                    return props[val];
                }
            }
            set
            {
                props[val] = value;
            }
        }

        public virtual StreamPropList Clone()
        {
            StreamPropList res = new();

            foreach (var i in props)
                res[i.Key] = (StreamProperty)i.Value.CloneDeep();

            return res;
        }

        public void Clear()
        {
            props.Clear();
        }

        public bool Contains(ePropID id)
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

        IEnumerator<KeyValuePair<ePropID, StreamProperty>> IEnumerable<KeyValuePair<ePropID, StreamProperty>>.GetEnumerator()
        {
            return props.GetEnumerator();
        }

        internal void Add(StreamProperty prop)
        {
            props[prop.Propid] = prop;
        }

        public bool Equals(StreamPropList other)
        {
            if (other is null)
                return false;

            foreach (StreamProperty prop in other.props.Values)
            {
                if (!this.Contains(prop.Propid) || prop.Value != this[prop.Propid].Value)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator ==(StreamPropList point1, StreamPropList point2)
        {
            if (ReferenceEquals(point1, null))
                return false;
            if (ReferenceEquals(point2, null))
                return false;
            return ReferenceEquals(point1, point2);
        }

        public static bool operator !=(StreamPropList point1, StreamPropList point2)
        {
            if (ReferenceEquals(point1, null))
                return true;
            if (ReferenceEquals(point2, null))
                return true;
            return !ReferenceEquals(point1, point2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (ReferenceEquals(obj, null))
            {
                return false;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [TypeConverter(typeof(StreamPropList)), Description("Expand to see value and units")]
    [Serializable]
    public class StreamPropListExpander : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is UOMProperty property)
            {
                return property.BaseValue;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}