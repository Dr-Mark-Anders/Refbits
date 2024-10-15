using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Length measurement units.
    /// </summary>
    public enum VelocityUnit
    {
        m_s,
        cm_s,
        mm_s,
    }

    /// <summary>
    /// A Length value.
    /// </summary>
    [Serializable]
    public struct Velocity : IFormattable, IComparable, IComparable<Velocity>, IEquatable<Velocity>, IUOM
    {
        private double _value = double.NaN;
        private string name = "";

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out LengthUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_value, x);
        }

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_value))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Creates a new  Length with the specified value in Kelvin.
        /// </summary>
        /// <param name="length">The value of the Length.</param>
        public Velocity(double length) : this() { _value = length; }

        /// <summary>
        /// Creates a new  Length with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="Length">The value of the Length.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="Length"/> value is used.</param>
        public Velocity(double Length, LengthUnit unit) : this()
        {
            switch (unit)
            {
                case LengthUnit.m:
                    _value = Length;
                    break;

                case LengthUnit.cm:
                    cm = Length;
                    break;

                case LengthUnit.mm:
                    mm = Length;
                    break;

                default:
                    throw new ArgumentException("The Length unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Length value in Kelvin.
        /// </summary>
        public double m
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the Length value in Celsius.
        /// </summary>
        public double cm
        {
            get { return m_to_cm(_value); }
            set { _value = cm_to_m(value); }
        }

        public double mm
        {
            get { return m_to_mm(_value); }
            set { _value = mm_to_m(value); }
        }

        public void EraseValue()
        {
            _value = double.NaN;
        }

        /// <summary>
        /// return  s a string  representation of the Length value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Length. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="formatProvider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Length.</return  s>
        /// <exception cref="FormatException">
        /// The value of format is not null, the empty string  (""), "G", "C", "F", or
        /// "K".
        /// </exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "K";
            if (formatProvider == null) formatProvider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "m":
                    return m.ToString("F2", formatProvider) + " m";

                case "cm":
                    return cm.ToString("F2", formatProvider) + " cm";

                case "mm":
                    return cm.ToString("F2", formatProvider) + " mm";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the Length value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Length. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the Length.</return  s>
        /// <exception cref="FormatException">
        /// The value of format is not null,
        /// the empty string  (""), "G", "C", "F", or
        /// "K".
        /// </exception>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(ePropID prop, IFormatProvider provider)
        {
            return "";
        }

        /// <summary>
        /// return  s a string  representation of the Length value.
        /// </summary>
        /// <return  s>A string  representation of the Length.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the Length value.
        /// </summary>
        /// <param name="unit">
        /// The Length unit as which the Length value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Length.</return  s>
        public string ToString(LengthUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case LengthUnit.m:
                    return ToString("m", provider);

                case LengthUnit.cm:
                    return ToString("cm", provider);

                case LengthUnit.mm:
                    return ToString("mm", provider);

                default:
                    throw new FormatException("The Length unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Length value.
        /// </summary>
        /// <param name="unit">
        /// The Length unit as which the Length value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Length.</return  s>
        public string ToString(LengthUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string DefaultUnit => LengthUnit.m.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(LengthUnit));
            }
        }

        public string Name => name;

        public ePropID propid => ePropID.Length;

        double IUOM.DisplayValue
        {
            get
            {
                return BaseValue;
            }
            set
            {
                BaseValue = value;
            }
        }

        /// <summary>
        /// Compares this instance to a specified Length object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A Length object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(Velocity value)
        {
            return _value.CompareTo(value._value);
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
        /// The value is not a Length.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is Length)) throw new ArgumentException();
            return CompareTo((Length)value);
        }

        /// <summary>
        /// Determines whether or not the given Length is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Length to compare to this instance.</param>
        /// <return  s>True if the Length is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(Velocity value)
        {
            return _value == value._value;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the Length.
        /// </summary>
        /// <param name="value">The object  to compare to the Length.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the Length. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is Length)) return false;

            return Equals((Length)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two Lengths.
        /// </summary>
        /// <param name="t1">The first Length to be compared.</param>
        /// <param name="t2">The second Length to be compared.</param>
        /// <return  s>True if the Lengths are equal. Otherwise, false.</return  s>
        public static bool operator ==(Velocity t1, Velocity t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Lengths.
        /// </summary>
        /// <param name="t1">The first Length to be compared.</param>
        /// <param name="t2">The second Length to be compared.</param>
        /// <return  s>True if the Lengths are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(Velocity t1, Velocity t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one Length is considered greater than another.
        /// </summary>
        /// <param name="t1">The first Length to be compared.</param>
        /// <param name="t2">The second Length to be compared.</param>
        /// <return  s>True if the first Length is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(Velocity t1, Velocity t2)
        {
            return t1._value > t2._value;
        }

        /// <summary>
        /// Determines whether one Length is considered less than another.
        /// </summary>
        /// <param name="t1">The first Length to be compared.</param>
        /// <param name="t2">The second Length to be compared.</param>
        /// <return  s>True if the first Length is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(Velocity t1, Velocity t2)
        {
            return t1._value < t2._value;
        }

        /// <summary>
        /// Determines whether one Length is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Length to be compared.</param>
        /// <param name="t2">The second Length to be compared.</param>
        /// <return  s>
        /// True if the first Length is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(Velocity t1, Velocity t2)
        {
            return t1._value >= t2._value;
        }

        /// <summary>
        /// Determines whether one Length is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Length to be compared.</param>
        /// <param name="t2">The second Length to be compared.</param>
        /// <return  s>
        /// True if the first Length is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(Velocity t1, Velocity t2)
        {
            return t1._value <= t2._value;
        }

        /// <summary>
        /// Adds two instances of the Length object .
        /// </summary>
        /// <param name="t1">The Length on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Length on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Lengths.</return  s>
        public static Velocity operator +(Velocity t1, Velocity t2)
        {
            return new Velocity(t1._value + t2._value);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Length on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Length on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Lengths.</return  s>
        public static Velocity operator -(Velocity t1, Velocity t2)
        {
            return new Velocity(t1._value - t2._value);
        }

        /// <summary>
        /// Multiplies two instances of the Length object .
        /// </summary>
        /// <param name="t1">The Length on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Length on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Lengths.</return  s>
        public static Velocity operator *(Velocity t1, Velocity t2)
        {
            return new Velocity(t1._value * t2._value);
        }

        /// <summary>
        /// Divides one instance of a Length object  by another.
        /// </summary>
        /// <param name="t1">The Length on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Length on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Length.</return  s>
        public static Velocity operator /(Velocity t1, Velocity t2)
        {
            return new Velocity(t1._value / t2._value);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Length object  is divided by another.
        /// </summary>
        /// <param name="t1">The Length on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Length on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Velocity operator %(Velocity t1, Velocity t2)
        {
            return new Velocity(t1._value % t2._value);
        }

        /// <summary>
        /// Converts a Kelvin Length value to Celsius.
        /// </summary>
        /// <param name="m">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double m_to_cm(double m)
        {
            return m * 100;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="cm">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double cm_to_m(double cm)
        {
            return cm / 100;
        }

        public static double mm_to_m(double mm)
        {
            return mm / 1000;
        }

        public static double m_to_mm(double m)
        {
            return m * 1000;
        }

        public static implicit operator double(Velocity t)
        {
            return t.m;
        }

        public static implicit operator Velocity(double t)
        {
            return new Velocity(t);
        }

        public void DeltaValue(double v)
        {
            _value += v;
        }

        /// <summary>
        /// Gets the Length value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Length should be retrieved.</param>
        /// <return  s>The Length value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(LengthUnit unit, double value)
        {
            switch (unit)
            {
                case LengthUnit.m: _value = value; return value;
                case LengthUnit.cm: cm = value; return value;
                case LengthUnit.mm: mm = value; return value;
                default:
                    throw new ArgumentException("Unknown Length unit '" + unit.ToString() + "'.");
            }
        }

        public double Valueout(LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.m: return m;
                case LengthUnit.cm: return cm;
                case LengthUnit.mm: return mm;
                default:
                    throw new ArgumentException("Unknown Pressure  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            LengthUnit U;
            if (Enum.TryParse(unit, out U))
            {
                return Valueout(U);
            }
            else
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Gets the Length value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Length should be retrieved.</param>
        /// <return  s>The Length value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.m: return _value;
                case LengthUnit.cm: return cm;
                case LengthUnit.mm: return mm;
                default:
                    throw new ArgumentException("Unknown Length unit '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(LengthUnit unit, double value)
        {
            switch (unit)
            {
                case LengthUnit.m: m = value; break;
                case LengthUnit.cm: cm = value; break;
                case LengthUnit.mm: mm = value; break;
                default:
                    break;
            }
        }

        public void ValueIn(string unit, double value)
        {
            LengthUnit U;
            if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, value);
            }
        }

        public string Unitstring(ePropID prop)
        {
            return "";
        }
    }
}