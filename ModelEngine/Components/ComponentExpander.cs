using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace ModelEngine
{
    // The TypeConverter attribute applied to the class.
    [TypeConverter(typeof(Components)), Description("Expand to see stream Components")]
    [Serializable]
    public class ComponentsExpander : ExpandableObjectConverter //, ISerializable
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(List<BaseComp>);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is List<BaseComp> bc)
                return bc.ToArray();
            return null;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(List<BaseComp>);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is List<BaseComp> bc)
                return bc.ToArray();
            return null;
        }
    }
}