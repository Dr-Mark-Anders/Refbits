namespace Units.UOM
{
    public interface IUOM
    {
        /* Guid guid
         {
             get;
         }*/

        double Tolerance
        {
            get;
        }

        ePropID propid
        {
            get;
        }

        bool IsKnown
        {
            get;
        }

        int CompareTo(object value);

        void DeltaValue(double v);

        void EraseValue();

        string ToString();

        string ToString(string format);

        double BaseValue
        {
            get;
            set;
        }

        string DefaultUnit
        {
            get;
        }

        string[] AllUnits
        {
            get;
        }

        double ValueOut(string unit);

        void ValueIn(string unit, double v);

        string UnitDescriptor(string unit);

        string Name
        {
            get;
            //set;
        }

        double DisplayValue
        {
            get;
            set;
        }
    }
}