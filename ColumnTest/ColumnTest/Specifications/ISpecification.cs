using ModelEngine;

namespace UnitsTest
{
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.TypeConverter))]
    public interface IFlowSpecification
    {
        bool IsActive { get; set; }
        string Name { get; set; }
        enumflowtype FlowType { get; set; }
        string Units { get; }
        double Value { get; set; }
    }
}