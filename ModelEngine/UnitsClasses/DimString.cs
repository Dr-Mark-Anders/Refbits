using System;

namespace Units
{
    /// <summary>
    /// Summary description for class 1.
    /// </summary>
    ///
    [Serializable]
    public struct DimString
    {
        private string BaseVal;
        public string Dimension;
        public string Units;

        public DimString(string Val, string Dimension, string Units)
        {
            this.BaseVal = Val;
            this.Dimension = Dimension;
            this.Units = Units;
        }

        public DimString(string Val)
        {
            this.BaseVal = Val;
            this.Dimension = "";
            this.Units = "";
        }

        public static implicit operator string(DimString a)
        {
            return a.BaseVal;
        }

        public static implicit operator DimString(string a)
        {
            return new DimString(a);
        }
    }
}