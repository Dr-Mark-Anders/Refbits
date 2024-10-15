using System;
using System.ComponentModel;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for DeltaTemperature  measurement units.
    /// </summary>
    public enum DeltaTemperatureUnit
    {
        [Description("K")]
        Kelvin,

        [Description("C")]
        Celsius,

        [Description("F")]
        Fahrenheit,

        [Description("R")]
        Rankine
    }

    /// <summary>
    /// A DeltaTemperature  value.
    /// </summary>
    [Serializable]
    public struct DeltaTemperature : IFormattable, IComparable, IComparable<DeltaTemperature>, IEquatable<DeltaTemperature>, IUOM
    {
        private double _Kelvin;

        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_Kelvin, x);
        }

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out DeltaTemperatureUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public ePropID propid => ePropID.T;

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_Kelvin))
                    return false;
                return true;
            }
        }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(DeltaTemperatureUnit), DeltaTemperatureUnit.Kelvin);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(DeltaTemperatureUnit));
            }
        }

        /// <summary>
        /// Creates a new  DeltaTemperature  with the specified value in Kelvin.
        /// </summary>
        /// <param name="kelvin">The value of the DeltaTemperature .</param>
        public DeltaTemperature(double kelvin) : this() { _Kelvin = kelvin; }

        /// <summary>
        /// Creates a new  DeltaTemperature  with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="temp">The value of the DeltaTemperature .</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="temp"/> value is used.</param>
        public DeltaTemperature(double temp, DeltaTemperatureUnit unit) : this()
        {
            switch (unit)
            {
                case DeltaTemperatureUnit.Kelvin:
                    _Kelvin = temp;
                    break;

                case DeltaTemperatureUnit.Celsius:
                    Celsius = temp;
                    break;

                case DeltaTemperatureUnit.Fahrenheit:
                    Fahrenheit = temp;
                    break;

                case DeltaTemperatureUnit.Rankine:
                    Rankine = temp;
                    break;

                default:
                    throw new ArgumentException("The DeltaTemperature  unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the DeltaTemperature  value in Kelvin.
        /// </summary>
        public double Kelvin
        {
            get { return _Kelvin; }
            set { _Kelvin = value; }
        }

        /// <summary>
        /// Gets or sets the DeltaTemperature  value in Celsius.
        /// </summary>
        public double Celsius
        {
            get { return KelvintoCelsius(_Kelvin); }
            set { _Kelvin = CelsiusToKelvin(value); }
        }

        /// <summary>
        /// Gets or sets the DeltaTemperature  value in Fahrenheit.
        /// </summary>
        public double Fahrenheit
        {
            get { return KelvintoFahrenheit(_Kelvin); }
            set { _Kelvin = FahrenheitToKelvin(value); }
        }

        public double Rankine
        {
            get { return KelvintoRankine(_Kelvin); }
            set { _Kelvin = RankineToKelvin(value); }
        }

        public void EraseValue()
        {
            _Kelvin = double.NaN;
        }

        /// <summary>
        /// Gets the DeltaTemperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the DeltaTemperature  should be retrieved.</param>
        /// <return  s>The DeltaTemperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(DeltaTemperatureUnit unit)
        {
            switch (unit)
            {
                case DeltaTemperatureUnit.Kelvin: return _Kelvin;
                case DeltaTemperatureUnit.Celsius: return Celsius;
                case DeltaTemperatureUnit.Fahrenheit: return Fahrenheit;
                case DeltaTemperatureUnit.Rankine: return Rankine;
                default:
                    throw new ArgumentException("Unknown DeltaTemperature  unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// Gets the DeltaTemperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the DeltaTemperature  should be retrieved.</param>
        /// <return  s>The DeltaTemperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public void ValueIn(DeltaTemperatureUnit unit, double value)
        {
            switch (unit)
            {
                case DeltaTemperatureUnit.Kelvin: _Kelvin = value; break;
                case DeltaTemperatureUnit.Celsius: Celsius = value; break;
                case DeltaTemperatureUnit.Fahrenheit: Fahrenheit = value; break;
                case DeltaTemperatureUnit.Rankine: Rankine = value; break;
                default:
                    throw new ArgumentException("Unknown DeltaTemperature  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            DeltaTemperatureUnit U;
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
            DeltaTemperatureUnit U;
            if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, value);
            }
            else
            {
               // throw new ArgumentException("Unknown Delta Temperature  unit '" + unit.ToString() + "'.");
                //return   double.NaN;
            }
        }

        /// <summary>
        /// return  s a string  representation of the DeltaTemperature  value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// DeltaTemperature . The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the DeltaTemperature .</return  s>
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
                    return Celsius.ToString("F2", provider) + " °C";

                case "F":
                    return Fahrenheit.ToString("F2", provider) + " °F";

                case "K":
                    return _Kelvin.ToString("F2", provider) + " K";

                case "R":
                    return _Kelvin.ToString("F2", provider) + " R";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the DeltaTemperature  value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// DeltaTemperature . The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the DeltaTemperature .</return  s>
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
        /// return  s a string  representation of the DeltaTemperature  value.
        /// </summary>
        /// <return  s>A string  representation of the DeltaTemperature .</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the DeltaTemperature  value.
        /// </summary>
        /// <param name="unit">
        /// The DeltaTemperature  unit as which the DeltaTemperature  value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the DeltaTemperature .</return  s>
        public string ToString(DeltaTemperatureUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case DeltaTemperatureUnit.Celsius:
                    return ToString("C", provider);

                case DeltaTemperatureUnit.Fahrenheit:
                    return ToString("F", provider);

                case DeltaTemperatureUnit.Kelvin:
                    return ToString("K", provider);

                case DeltaTemperatureUnit.Rankine:
                    return ToString("R", provider);

                default:
                    throw new FormatException("The DeltaTemperature  unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the DeltaTemperature  value.
        /// </summary>
        /// <param name="unit">
        /// The DeltaTemperature  unit as which the DeltaTemperature  value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the DeltaTemperature .</return  s>
        public string ToString(DeltaTemperatureUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _Kelvin;
            }
            set
            {
                _Kelvin = value;
            }
        }

        public string Name { get => "DeltaTemperature "; }
        public bool HasChanged { get; set; }

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
        /// Compares this instance to a specified DeltaTemperature  object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A DeltaTemperature  object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(DeltaTemperature value)
        {
            return _Kelvin.CompareTo(value._Kelvin);
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
        /// The value is not a DeltaTemperature .
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is DeltaTemperature)) throw new ArgumentException();
            return CompareTo((DeltaTemperature)value);
        }

        /// <summary>
        /// Determines whether or not the given DeltaTemperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The DeltaTemperature  to compare to this instance.</param>
        /// <return  s>True if the DeltaTemperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(DeltaTemperature value)
        {
            return _Kelvin == value._Kelvin;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the DeltaTemperature .
        /// </summary>
        /// <param name="value">The object  to compare to the DeltaTemperature .</param>
        /// <return  s>True if the object  is considered equal
        ///     to the DeltaTemperature . Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is DeltaTemperature)) return false;

            return Equals((DeltaTemperature)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _Kelvin.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first DeltaTemperature  to be compared.</param>
        /// <param name="t2">The second DeltaTemperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(DeltaTemperature t1, DeltaTemperature t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first DeltaTemperature  to be compared.</param>
        /// <param name="t2">The second DeltaTemperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(DeltaTemperature t1, DeltaTemperature t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one DeltaTemperature  is considered greater than another.
        /// </summary>
        /// <param name="t1">The first DeltaTemperature  to be compared.</param>
        /// <param name="t2">The second DeltaTemperature  to be compared.</param>
        /// <return  s>True if the first DeltaTemperature  is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(DeltaTemperature t1, DeltaTemperature t2)
        {
            return t1._Kelvin > t2._Kelvin;
        }

        /// <summary>
        /// Determines whether one DeltaTemperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first DeltaTemperature  to be compared.</param>
        /// <param name="t2">The second DeltaTemperature  to be compared.</param>
        /// <return  s>True if the first DeltaTemperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(DeltaTemperature t1, DeltaTemperature t2)
        {
            return t1._Kelvin < t2._Kelvin;
        }

        /// <summary>
        /// Determines whether one DeltaTemperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first DeltaTemperature  to be compared.</param>
        /// <param name="t2">The second DeltaTemperature  to be compared.</param>
        /// <return  s>
        /// True if the first DeltaTemperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(DeltaTemperature t1, DeltaTemperature t2)
        {
            return t1._Kelvin >= t2._Kelvin;
        }

        /// <summary>
        /// Determines whether one DeltaTemperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first DeltaTemperature  to be compared.</param>
        /// <param name="t2">The second DeltaTemperature  to be compared.</param>
        /// <return  s>
        /// True if the first DeltaTemperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(DeltaTemperature t1, DeltaTemperature t2)
        {
            return t1._Kelvin <= t2._Kelvin;
        }

        /// <summary>
        /// Adds two instances of the DeltaTemperature  object .
        /// </summary>
        /// <param name="t1">The DeltaTemperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The DeltaTemperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static DeltaTemperature operator +(DeltaTemperature t1, DeltaTemperature t2)
        {
            return new DeltaTemperature(t1._Kelvin + t2._Kelvin);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The DeltaTemperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The DeltaTemperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static DeltaTemperature operator -(DeltaTemperature t1, DeltaTemperature t2)
        {
            return new DeltaTemperature(t1._Kelvin - t2._Kelvin);
        }

        /// <summary>
        /// Multiplies two instances of the DeltaTemperature  object .
        /// </summary>
        /// <param name="t1">The DeltaTemperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The DeltaTemperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static DeltaTemperature operator *(DeltaTemperature t1, DeltaTemperature t2)
        {
            return new DeltaTemperature(t1._Kelvin * t2._Kelvin);
        }

        /// <summary>
        /// Divides one instance of a DeltaTemperature  object  by another.
        /// </summary>
        /// <param name="t1">The DeltaTemperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The DeltaTemperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two DeltaTemperature .</return  s>
        public static DeltaTemperature operator /(DeltaTemperature t1, DeltaTemperature t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new DeltaTemperature(t1._Kelvin / t2._Kelvin);
        }

        /// <summary>
        /// Finds the remainder when one instance of a DeltaTemperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The DeltaTemperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The DeltaTemperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static DeltaTemperature operator %(DeltaTemperature t1, DeltaTemperature t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new DeltaTemperature(t1._Kelvin % t2._Kelvin);
        }

        /// <summary>
        /// Converts a Kelvin DeltaTemperature  value to Celsius.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double KelvintoCelsius(double kelvin)
        {
            return kelvin;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="celsius">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double CelsiusToKelvin(double celsius)
        {
            return celsius;
        }

        /// <summary>
        /// Converts a Kelvin value to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Kelvin value in Fahrenheit.</return  s>
        public static double KelvintoFahrenheit(double kelvin)
        {
            return kelvin * 9 / 5;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Kelvin.
        /// </param>
        /// <return  s>The Fahrenheit value in Kelvin.</return  s>
        public static double FahrenheitToKelvin(double fahrenheit)
        {
            return fahrenheit * 5 / 9;
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
            return fahrenheit * 5 / 9;
        }

        /// <summary>
        /// Converts a Celsius value to Fahrenheit.
        /// </summary>
        /// <param name="celsius">The Celsius value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Celsius value in Fahrenheit.</return  s>
        public static double CelsiusToFahrenheit(double celsius)
        {
            return celsius * 9 / 5;
        }

        public static implicit operator double(DeltaTemperature t)
        {
            return t.Kelvin;
        }

        public static implicit operator DeltaTemperature(double t)
        {
            return new DeltaTemperature(t);
        }

        public void ToDefault(DeltaTemperatureUnit from, double val)
        {
            switch (from)
            {
                case DeltaTemperatureUnit.Kelvin:
                    _Kelvin = val;
                    break;

                case DeltaTemperatureUnit.Celsius:
                    _Kelvin = val;
                    break;

                case DeltaTemperatureUnit.Fahrenheit:
                    _Kelvin = val / 1.8;
                    break;

                case DeltaTemperatureUnit.Rankine:
                    _Kelvin = val / 1.8;
                    break;

                default:
                    break;
            }
        }

        public double FromDefault(DeltaTemperatureUnit to)
        {
            switch (to)
            {
                case DeltaTemperatureUnit.Kelvin:
                    return _Kelvin;

                case DeltaTemperatureUnit.Celsius:
                    return _Kelvin;

                case DeltaTemperatureUnit.Fahrenheit:
                    return _Kelvin * 1.8;

                case DeltaTemperatureUnit.Rankine:
                    return _Kelvin * 1.8;

                default:
                    return double.NaN;
            }
        }

        public void DeltaValue(double v)
        {
            _Kelvin += v;
        }
    }
}