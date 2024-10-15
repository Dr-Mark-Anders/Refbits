using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Time measurement units.
    /// </summary>
    public enum TimeUnit
    {
        Hr,
        Min,
        s,
    }

    /// <summary>
    /// A Time value.
    /// </summary>
    [Serializable]
    public struct Time : IFormattable, IComparable, IComparable<Time>, IEquatable<Time>, IUOM
    {
        private double _value = double.NaN;
        private string name = "";

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out TimeUnit res);
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
        /// Creates a new  Time with the specified value in Kelvin.
        /// </summary>
        /// <param name="Time">The value of the Time.</param>
        public Time(double Time) : this() { _value = Time; }

        /// <summary>
        /// Creates a new  Time with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="Time">The value of the Time.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="Time"/> value is used.</param>
        public Time(double Time, TimeUnit unit) : this()
        {
            switch (unit)
            {
                case TimeUnit.Hr:
                    _value = Time;
                    break;

                case TimeUnit.Min:
                    _value = Time/60;
                    break;

                case TimeUnit.s:
                    _value = Time/3600;
                    break;

                default:
                    throw new ArgumentException("The Time unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Time value in Kelvin.
        /// </summary>
        public double Hr
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the Time value in Celsius.
        /// </summary>
        public double Min
        { 
            get { return _value*60; }
            set { _value = value/60; }
        }

        public double S
        {
            get { return _value * 3600; }
            set { _value = value/3600; }
        }

        public void EraseValue()
        {
            _value = double.NaN;
        }

        /// <summary>
        /// return  s a string  representation of the Time value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Time. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="formatProvider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Time.</return  s>
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
                case "Hr":
                    return Hr.ToString("F2", formatProvider) + " Hr";

                case "Min":
                    return Min.ToString("F2", formatProvider) + " Min";

                case "S":
                    return S.ToString("F2", formatProvider) + " S";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the Time value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Time. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the Time.</return  s>
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
        /// return  s a string  representation of the Time value.
        /// </summary>
        /// <return  s>A string  representation of the Time.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the Time value.
        /// </summary>
        /// <param name="unit">
        /// The Time unit as which the Time value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Time.</return  s>
        public string ToString(TimeUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case TimeUnit.Hr:
                    return ToString("m", provider);

                case TimeUnit.Min:
                    return ToString("cm", provider);

                case TimeUnit.s:
                    return ToString("mm", provider);

                default:
                    throw new FormatException("The Time unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Time value.
        /// </summary>
        /// <param name="unit">
        /// The Time unit as which the Time value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Time.</return  s>
        public string ToString(TimeUnit unit)
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

        public string DefaultUnit => TimeUnit.Hr.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(TimeUnit));
            }
        }

        public string Name => name;

        public ePropID propid => ePropID.Time;

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
        /// Compares this instance to a specified Time object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A Time object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(Time value)
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
        /// The value is not a Time.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is Time)) throw new ArgumentException();
            return CompareTo((Time)value);
        }

        /// <summary>
        /// Determines whether or not the given Time is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Time to compare to this instance.</param>
        /// <return  s>True if the Time is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(Time value)
        {
            return _value == value._value;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the Time.
        /// </summary>
        /// <param name="value">The object  to compare to the Time.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the Time. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is Time)) return false;

            return Equals((Time)value);
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
        /// Determines the eQuality  of two Times.
        /// </summary>
        /// <param name="t1">The first Time to be compared.</param>
        /// <param name="t2">The second Time to be compared.</param>
        /// <return  s>True if the Times are equal. Otherwise, false.</return  s>
        public static bool operator ==(Time t1, Time t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Times.
        /// </summary>
        /// <param name="t1">The first Time to be compared.</param>
        /// <param name="t2">The second Time to be compared.</param>
        /// <return  s>True if the Times are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(Time t1, Time t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one Time is considered greater than another.
        /// </summary>
        /// <param name="t1">The first Time to be compared.</param>
        /// <param name="t2">The second Time to be compared.</param>
        /// <return  s>True if the first Time is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(Time t1, Time t2)
        {
            return t1._value > t2._value;
        }

        /// <summary>
        /// Determines whether one Time is considered less than another.
        /// </summary>
        /// <param name="t1">The first Time to be compared.</param>
        /// <param name="t2">The second Time to be compared.</param>
        /// <return  s>True if the first Time is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(Time t1, Time t2)
        {
            return t1._value < t2._value;
        }

        /// <summary>
        /// Determines whether one Time is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Time to be compared.</param>
        /// <param name="t2">The second Time to be compared.</param>
        /// <return  s>
        /// True if the first Time is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(Time t1, Time t2)
        {
            return t1._value >= t2._value;
        }

        /// <summary>
        /// Determines whether one Time is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Time to be compared.</param>
        /// <param name="t2">The second Time to be compared.</param>
        /// <return  s>
        /// True if the first Time is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(Time t1, Time t2)
        {
            return t1._value <= t2._value;
        }

        /// <summary>
        /// Adds two instances of the Time object .
        /// </summary>
        /// <param name="t1">The Time on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Time on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Times.</return  s>
        public static Time operator +(Time t1, Time t2)
        {
            return new Time(t1._value + t2._value);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Time on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Time on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Times.</return  s>
        public static Time operator -(Time t1, Time t2)
        {
            return new Time(t1._value - t2._value);
        }

        /// <summary>
        /// Multiplies two instances of the Time object .
        /// </summary>
        /// <param name="t1">The Time on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Time on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Times.</return  s>
        public static Time operator *(Time t1, Time t2)
        {
            return new Time(t1._value * t2._value);
        }

        /// <summary>
        /// Divides one instance of a Time object  by another.
        /// </summary>
        /// <param name="t1">The Time on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Time on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Time.</return  s>
        public static Time operator /(Time t1, Time t2)
        {
            return new Time(t1._value / t2._value);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Time object  is divided by another.
        /// </summary>
        /// <param name="t1">The Time on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Time on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Time operator %(Time t1, Time t2)
        {
            return new Time(t1._value % t2._value);
        }

        /// <summary>
        /// Converts a Kelvin Time value to Celsius.
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

        public static implicit operator double(Time t)
        {
            return t.Min;
        }

        public static implicit operator Time(double t)
        {
            return new Time(t);
        }

        public void DeltaValue(double v)
        {
            _value += v;
        }

        /// <summary>
        /// Gets the Time value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Time should be retrieved.</param>
        /// <return  s>The Time value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(TimeUnit unit, double value)
        {
            switch (unit)
            {
                case TimeUnit.Hr: _value = value; return value;
                case TimeUnit.Min: Min = value; return value;
                case TimeUnit.s: S = value; return value;
                default:
                    throw new ArgumentException("Unknown Time unit '" + unit.ToString() + "'.");
            }
        }

        public double Valueout(TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Hr: return Hr;
                case TimeUnit.Min: return Min;
                case TimeUnit.s: return S;
                default:
                    throw new ArgumentException("Unknown Pressure  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            TimeUnit U;
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
        /// Gets the Time value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Time should be retrieved.</param>
        /// <return  s>The Time value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Hr: return _value;
                case TimeUnit.Min: return Min;
                case TimeUnit.s: return S;
                default:
                    throw new ArgumentException("Unknown Time unit '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(TimeUnit unit, double value)
        {
            switch (unit)
            {
                case TimeUnit.Hr: Hr = value; break;
                case TimeUnit.Min: Min = value; break;
                case TimeUnit.s: S = value; break;
                default:
                    break;
            }
        }

        public void ValueIn(string unit, double value)
        {
            TimeUnit U;
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