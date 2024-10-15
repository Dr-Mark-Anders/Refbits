using ModelEngine;
using System;
using System.ComponentModel;
using Units;

namespace ModelEngine
{
    [TypeConverter(typeof(DataStoreExpander)), Serializable]
    public class DataStore : IEquatable<DataStore>
    {
        private Components cc = new();
        private StreamPropList properties = new();

        public DataStore()
        {
        }

        public DataStore(Port_Material pm)
        {
            this.cc = pm.cc;
            this.properties = pm.Properties;
        }

        [Browsable(true)]
        public StreamPropList Properties { get => properties; set => properties = value; }

        [Browsable(true)]
        public Components Cc { get => cc; set => cc = value; }

        public bool IsInput { get; internal set; }

        public bool Equals(DataStore other)
        {
            if (other.cc.Equals(this.cc) && other.properties.Equals(this.properties))
                return true;
            return false;
        }

        internal DataStore Clone()
        {
            DataStore res = new DataStore();
            res.properties = this.properties.Clone();
            res.cc = this.cc.Clone();
            return res;
        }

        internal void Clear()
        {
            cc.Clear();
            properties.Clear();
        }

        internal void SetProp(ePropID propid, StreamProperty streamProperty)
        {
            Properties[propid] = streamProperty;
        }
    }

    [TypeConverter(typeof(DataStore)), Description("Expand to see value and units")]
    [Serializable]
    public class DataStoreExpander : ExpandableObjectConverter
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