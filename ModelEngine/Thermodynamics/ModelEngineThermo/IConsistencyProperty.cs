using System;

namespace ModelEngine
{
    public interface IConsistencyProperty
    {
        Guid Origin { get; set; }
        double Value { get; set; }
    }
}