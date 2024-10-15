using System;
using System.ComponentModel;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for UA measurement units.
    /// </summary>
    public enum UAUnit
    {
        [Description("kW/C")]
        kW_C
    }

    /// <summary>
    /// A UA value.
    /// </summary>
    [Serializable]
    public struct UA : IFormattable, IComparable, IComparable<UA>, IEquatable<UA>, IUOM
    {
        public double _kW_C;

        public double Pow(double x)
        {
            return Math.Pow(_kW_C, x);
        }

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out UAUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public ePropID propid => ePropID.UA;

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_kW_C))
                    return false;
                return true;
            }
        }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(UAUnit), UAUnit.kW_C);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(UAUnit));
            }
        }

        /// <summary>
        /// Creates a new  UA with the specified value in kW_C.
        /// </summary>
        /// <param name="kW_C">The value of the UA.</param>
        public UA(double kW_C) : this() { _kW_C = kW_C; }

        /// <summary>
        /// Creates a new  UA with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="temp">The value of the UA.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="temp"/> value is used.</param>
        public UA(double temp, UAUnit unit) : this()
        {
            switch (unit)
            {
                case UAUnit.kW_C:
                    _kW_C = temp;
                    break;

                default:
                    throw new ArgumentException("The UA unit '" + unit.ToString() + "' is unknown.");
            }
        }

        public void EraseValue()
        {
            _kW_C = double.NaN;
        }

        /// <summary>
        /// Gets the UA value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the UA should be retrieved.</param>
        /// <return  s>The UA value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(UAUnit unit)
        {
            switch (unit)
            {
                case UAUnit.kW_C: return _kW_C;
                default:
                    throw new ArgumentException("Unknown UA unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// Gets the UA value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the UA should be retrieved.</param>
        /// <return  s>The UA value in the specified
        /// <paramref name="unit"/>.</return  s>
        public void ValueIn(UAUnit unit, double value)
        {
            switch (unit)
            {
                case UAUnit.kW_C: _kW_C = value; break;
                default:
                    throw new ArgumentException("Unknown UA unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            UAUnit U;
            if (Enum.TryParse(unit, out U))
            {
                return Valueout(U);
            }
            else
            {
                return double.NaN;
            }
        }

        public void ValueIn(string unit, double value)
        {
            UAUnit U;
            if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, value);
            }
            else
            {
                throw new ArgumentException("Unknown UA unit '" + unit.ToString() + "'.");
                //return   double.NaN;
            }
        }

        /// <summary>
        /// return  s a string  representation of the UA value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// UA. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the UA.</return  s>
        /// <exception cref="FormatException">
        /// The value of format is not null, the empty string  (""), "G", "C", "F", or
        /// "K".
        /// </exception>
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "C";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "C":
                    return _kW_C.ToString("F2", provider) + " kW/C";

                default:
                    return _kW_C.ToString("F2", provider) + " kW/C";
            }
        }

        /// <summary>
        /// return  s a string  representation of the UA value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// UA. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the UA.</return  s>
        /// <exception cref="FormatException">
        /// The value of format is not null,
        /// the empty string  (""), "G", "C", "F", or
        /// "K".
        /// </exception>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// return  s a string  representation of the UA value.
        /// </summary>
        /// <return  s>A string  representation of the UA.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the UA value.
        /// </summary>
        /// <param name="unit">
        /// The UA unit as which the UA value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the UA.</return  s>
        public string ToString(UAUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case UAUnit.kW_C:
                    return ToString("kW/C", provider);

                default:
                    throw new FormatException("The UA unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the UA value.
        /// </summary>
        /// <param name="unit">
        /// The UA unit as which the UA value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the UA.</return  s>
        public string ToString(UAUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _kW_C;
            }
            set
            {
                _kW_C = value;
            }
        }

        public string Name { get => "UA"; }
        public bool HasChanged { get; set; }

        public double Tolerance => 1e-6;

        double IUOM.DisplayValue
        {
            get
            {
                return Math.Round(Valueout(UAUnit.kW_C), 2, MidpointRounding.AwayFromZero);
            }
            set
            {
                ValueIn(UAUnit.kW_C, value);
            }
        }

        /// <summary>
        /// Compares this instance to a specified UA object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A UA object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(UA value)
        {
            return _kW_C.CompareTo(value._kW_C);
        }

        /// <summary>
        /// Compares this instance to a specified object  and return  s an indication of
        /// their relative values.
        /// </summary>
        /// <param name="value">An object  to compare, or null.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value, or value is null.
        /// </return  s>
        /// <exception cref="ArgumentException">
        /// The value is not a UA.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is UA)) throw new ArgumentException();
            return CompareTo((UA)value);
        }

        /// <summary>
        /// Determines whether or not the given UA is considered equal to this instance.
        /// </summary>
        /// <param name="value">The UA to compare to this instance.</param>
        /// <return  s>True if the UA is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(UA value)
        {
            return _kW_C == value._kW_C;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the UA.
        /// </summary>
        /// <param name="value">The object  to compare to the UA.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the UA. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is UA)) return false;

            return Equals((UA)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _kW_C.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first UA to be compared.</param>
        /// <param name="t2">The second UA to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(UA t1, UA t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first UA to be compared.</param>
        /// <param name="t2">The second UA to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(UA t1, UA t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one UA is considered greater than another.
        /// </summary>
        /// <param name="t1">The first UA to be compared.</param>
        /// <param name="t2">The second UA to be compared.</param>
        /// <return  s>True if the first UA is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(UA t1, UA t2)
        {
            return t1._kW_C > t2._kW_C;
        }

        /// <summary>
        /// Determines whether one UA is considered less than another.
        /// </summary>
        /// <param name="t1">The first UA to be compared.</param>
        /// <param name="t2">The second UA to be compared.</param>
        /// <return  s>True if the first UA is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(UA t1, UA t2)
        {
            return t1._kW_C < t2._kW_C;
        }

        /// <summary>
        /// Determines whether one UA is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first UA to be compared.</param>
        /// <param name="t2">The second UA to be compared.</param>
        /// <return  s>
        /// True if the first UA is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(UA t1, UA t2)
        {
            return t1._kW_C >= t2._kW_C;
        }

        /// <summary>
        /// Determines whether one UA is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first UA to be compared.</param>
        /// <param name="t2">The second UA to be compared.</param>
        /// <return  s>
        /// True if the first UA is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(UA t1, UA t2)
        {
            return t1._kW_C <= t2._kW_C;
        }

        /// <summary>
        /// Adds two instances of the UA object .
        /// </summary>
        /// <param name="t1">The UA on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The UA on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static UA operator +(UA t1, UA t2)
        {
            return new UA(t1._kW_C + t2._kW_C);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The UA on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The UA on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static UA operator -(UA t1, UA t2)
        {
            return new UA(t1._kW_C - t2._kW_C);
        }

        /// <summary>
        /// Multiplies two instances of the UA object .
        /// </summary>
        /// <param name="t1">The UA on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The UA on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static UA operator *(UA t1, UA t2)
        {
            return new UA(t1._kW_C * t2._kW_C);
        }

        /// <summary>
        /// Divides one instance of a UA object  by another.
        /// </summary>
        /// <param name="t1">The UA on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The UA on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two UA.</return  s>
        public static UA operator /(UA t1, UA t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new UA(t1._kW_C / t2._kW_C);
        }

        /// <summary>
        /// Finds the remainder when one instance of a UA object  is divided by another.
        /// </summary>
        /// <param name="t1">The UA on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The UA on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static UA operator %(UA t1, UA t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new UA(t1._kW_C % t2._kW_C);
        }

        public static implicit operator double(UA t)
        {
            return t._kW_C;
        }

        public static implicit operator UA(double t)
        {
            return new UA(t);
        }

        public void ToDefault(UAUnit from, double val)
        {
            switch (from)
            {
                case UAUnit.kW_C:
                    _kW_C = val;
                    break;

                default:
                    break;
            }
        }

        public double FromDefault(UAUnit to)
        {
            switch (to)
            {
                case UAUnit.kW_C:
                    return _kW_C;

                default:
                    return double.NaN;
            }
        }

        public void DeltaValue(double v)
        {
            _kW_C += v;
        }
    }
}