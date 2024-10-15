using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Vol measurement units.
    /// </summary>
    public enum VolUnit
    {
        m3,
        L,
        BBL
    }

    /// <summary>
    /// A Vol value.
    /// </summary>
    [Serializable]
    public struct Volume : IFormattable, IComparable, IComparable<Volume>, IEquatable<Volume>, IUOM
    {
        private double _m3;
        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_m3, x);
        }

        public string UnitDescriptor(string unit)
        {
            if (Enum.TryParse(unit, out VolUnit res))
                return Enumhelpers.GetEnumDescription(res);
            else
                return null;
        }

        public ePropID propid => ePropID.VF;

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_m3))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Creates a new  Vol with the specified value in Kelvin.
        /// </summary>
        /// <param name="value">The value of the Vol.</param>
        public Volume(double value) : this() { _m3 = value; }

        /// <summary>
        /// Creates a new  Vol with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="Vol">The value of the Vol.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="Vol"/> value is used.</param>
        public Volume(double Vol, VolUnit unit) : this()
        {
            switch (unit)
            {
                case VolUnit.m3:
                    _m3 = Vol;
                    break;

                case VolUnit.L:
                    l = Vol;
                    break;

                case VolUnit.BBL:
                    l = Vol;
                    break;

                default:
                    throw new ArgumentException("The Vol unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Vol value in Kelvin.
        /// </summary>
        public double m3_hr
        {
            get { return _m3; }
            set { _m3 = value; }
        }

        /// <summary>
        /// Gets or sets the Vol value in Celsius.
        /// </summary>
        public double l
        {
            get { return _m3 * 1000; }
            set { _m3 = value / 1000; }
        }

        /// <summary>
        /// Gets or sets the Vol value in Fahrenheit.
        /// </summary>
        public double BBL
        {
            get { return _m3 * 6.289814; }
            set { _m3 = value / 6.289814; }
        }

        public void EraseValue()
        {
            _m3 = double.NaN;
        }

        /// <summary>
        /// Gets the Vol value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Vol should be retrieved.</param>
        /// <return  s>The Vol value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(VolUnit unit)
        {
            switch (unit)
            {
                case VolUnit.m3: return _m3;
                case VolUnit.L: return l;
                case VolUnit.BBL: return BBL;
                default:
                    throw new ArgumentException("Unknown Vol unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// Gets the Vol value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Vol should be retrieved.</param>
        /// <return  s>The Vol value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(VolUnit unit, double value)
        {
            switch (unit)
            {
                case VolUnit.m3: _m3 = value; return value;
                case VolUnit.L: l = value; return value;
                case VolUnit.BBL: l = value; return value;
                default:
                    throw new ArgumentException("Unknown Vol unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Vol value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Vol. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Vol.</return  s>
        /// <exception cref="FormatException">
        /// The value of format is not null, the empty string  (""), "G", "C", "F", or
        /// "K".
        /// </exception>
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "K";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "m3":
                    return m3_hr.ToString("F2", provider) + " m3";

                case "l":
                    return l.ToString("F2", provider) + " l";

                case "bbl":
                    return BBL.ToString("F2", provider) + " bbl";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the Vol value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Vol. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the Vol.</return  s>
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
        /// return  s a string  representation of the Vol value.
        /// </summary>
        /// <return  s>A string  representation of the Vol.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the Vol value.
        /// </summary>
        /// <param name="unit">
        /// The Vol unit as which the Vol value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Vol.</return  s>
        public string ToString(VolUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case VolUnit.m3:
                    return ToString("m3", provider);

                case VolUnit.L:
                    return ToString("L", provider);

                case VolUnit.BBL:
                    return ToString("BBL", provider);

                default:
                    throw new FormatException("The Vol unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Vol value.
        /// </summary>
        /// <param name="unit">
        /// The Vol unit as which the Vol value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Vol.</return  s>
        public string ToString(VolUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _m3;
            }
            set
            {
                _m3 = value;
            }
        }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(VolUnit), VolUnit.m3);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(VolUnit));
            }
        }

        public string Name => "Liquid Volume Flow";

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
        /// Compares this instance to a specified Vol object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A Vol object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(Volume value)
        {
            return _m3.CompareTo(value._m3);
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
        /// The value is not a Vol.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (value is not Volume)
                return 1;
            return CompareTo((Volume)value);
        }

        /// <summary>
        /// Determines whether or not the given Vol is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Vol to compare to this instance.</param>
        /// <return  s>True if the Vol is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(Volume value)
        {
            return _m3 == value._m3;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the Vol.
        /// </summary>
        /// <param name="value">The object  to compare to the Vol.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the Vol. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (value is not Volume) return false;

            return Equals((Volume)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _m3.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two Vols.
        /// </summary>
        /// <param name="t1">The first Vol to be compared.</param>
        /// <param name="t2">The second Vol to be compared.</param>
        /// <return  s>True if the Vols are equal. Otherwise, false.</return  s>
        public static bool operator ==(Volume t1, Volume t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Vols.
        /// </summary>
        /// <param name="t1">The first Vol to be compared.</param>
        /// <param name="t2">The second Vol to be compared.</param>
        /// <return  s>True if the Vols are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(Volume t1, Volume t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one Vol is considered greater than another.
        /// </summary>
        /// <param name="t1">The first Vol to be compared.</param>
        /// <param name="t2">The second Vol to be compared.</param>
        /// <return  s>True if the first Vol is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(Volume t1, Volume t2)
        {
            return t1._m3 > t2._m3;
        }

        /// <summary>
        /// Determines whether one Vol is considered less than another.
        /// </summary>
        /// <param name="t1">The first Vol to be compared.</param>
        /// <param name="t2">The second Vol to be compared.</param>
        /// <return  s>True if the first Vol is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(Volume t1, Volume t2)
        {
            return t1._m3 < t2._m3;
        }

        /// <summary>
        /// Determines whether one Vol is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Vol to be compared.</param>
        /// <param name="t2">The second Vol to be compared.</param>
        /// <return  s>
        /// True if the first Vol is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(Volume t1, Volume t2)
        {
            return t1._m3 >= t2._m3;
        }

        /// <summary>
        /// Determines whether one Vol is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Vol to be compared.</param>
        /// <param name="t2">The second Vol to be compared.</param>
        /// <return  s>
        /// True if the first Vol is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(Volume t1, Volume t2)
        {
            return t1._m3 <= t2._m3;
        }

        /// <summary>
        /// Adds two instances of the Vol object .
        /// </summary>
        /// <param name="t1">The Vol on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Vol on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Vols.</return  s>
        public static Volume operator +(Volume t1, Volume t2)
        {
            return new Volume(t1._m3 + t2._m3);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Vol on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Vol on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Vols.</return  s>
        public static Volume operator -(Volume t1, Volume t2)
        {
            return new Volume(t1._m3 - t2._m3);
        }

        /// <summary>
        /// Multiplies two instances of the Vol object .
        /// </summary>
        /// <param name="t1">The Vol on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Vol on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Vols.</return  s>
        public static Volume operator *(Volume t1, Volume t2)
        {
            return new Volume(t1._m3 * t2._m3);
        }

        /// <summary>
        /// Divides one instance of a Vol object  by another.
        /// </summary>
        /// <param name="t1">The Vol on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Vol on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Vol.</return  s>
        public static Volume operator /(Volume t1, Volume t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Volume(t1._m3 / t2._m3);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Vol object  is divided by another.
        /// </summary>
        /// <param name="t1">The Vol on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Vol on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Volume operator %(Volume t1, Volume t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Volume(t1._m3 % t2._m3);
        }

        /// <summary>
        /// Converts a Kelvin Vol value to Celsius.
        /// </summary>
        /// <param name="m3e_hr">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double m3e_hr_to_m3e_s(double m3e_hr)
        {
            return m3e_hr / 3600;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="m3e_s">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double m3e_s_to_m3e_hr(double m3e_s)
        {
            return m3e_s * 3600;
        }

        /// <summary>
        /// Converts a Kelvin value to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Kelvin value in Fahrenheit.</return  s>
        public static double KelvintoFahrenheit(double kelvin)
        {
            return kelvin * 9 / 5 - 459.67;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Kelvin.
        /// </param>
        /// <return  s>The Fahrenheit value in Kelvin.</return  s>
        public static double FahrenheitToKelvin(double fahrenheit)
        {
            return (fahrenheit + 459.67) * 5 / 9;
        }

        /// <summary>
        /// Converts a Kelvin value to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Kelvin value in Fahrenheit.</return  s>
        public static double KelvintoRankine(double kelvin)
        {
            return kelvin * 9 / 5;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Kelvin.
        /// </param>
        /// <return  s>The Fahrenheit value in Kelvin.</return  s>
        public static double RankineToKelvin(double fahrenheit)
        {
            return fahrenheit * 5 / 9;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Celsius.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Celsius.
        /// </param>
        /// <return  s>The Fahrenheit value in Celsius.</return  s>
        public static double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        /// <summary>
        /// Converts a Celsius value to Fahrenheit.
        /// </summary>
        /// <param name="celsius">The Celsius value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Celsius value in Fahrenheit.</return  s>
        public static double CelsiusToFahrenheit(double celsius)
        {
            return celsius * 9 / 5 + 32;
        }

        public static implicit operator double(Volume t)
        {
            return t.m3_hr;
        }

        public static implicit operator Volume(double t)
        {
            return new Volume(t);
        }

        public void DeltaValue(double v)
        {
            _m3 += v;
        }

        public double ValueOut(string unit)
        {
            if (Enum.TryParse(unit, out VolUnit U))
            {
                return Valueout(U);
            }
            else
            {
                return double.NaN;
            }
        }

        public void ValueIn(string unit, double v)
        {
            if (Enum.TryParse(unit, out VolUnit U))
            {
                ValueIn(U, v);
            }
            else
            {
                if (unit is null)
                    BaseValue = double.NaN;
            }
        }

        public string Unitstring(ePropID prop)
        {
            throw new NotImplementedException();
        }

        public string ToString(ePropID prop, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}