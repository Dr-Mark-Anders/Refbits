using System.ComponentModel;

namespace ModelEngine
{
    [TypeConverter(typeof(FlowSheet)), Description("Expand to see flowsheet Unitops")]
    public class FlowSheetExpander : ExpandableObjectConverter
    {
    }
}