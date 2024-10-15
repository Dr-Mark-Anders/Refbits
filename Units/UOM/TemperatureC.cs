using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum TemperatureCUnit
    {
        [Description("°K")]
        Kelvin,

        [Description("°C")]
        Celsius,

        [Description("°F")]
        Fahrenheit,

        [Description("°R")]
        Rankine
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable, ComVisible(true)]
    public struct TemperatureC : IFormattable, IComparable, IComparable<Temperature>, IEquatable<Temperature>, IUOM
    {
        public double _Kelvin;

        public double Pow(double x)
        {
            return Math.Pow(_Kelvin, x);
        }

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out TemperatureUnit res);
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
                return Enum.GetName(typeof(TemperatureUnit), TemperatureUnit.Celsius);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(TemperatureUnit));
            }
        }

        /// <summary>
        /// Creates a new  Temperature  with the specified value in Kelvin.
        /// </summary>
        /// <param name="kelvin">The value of the Temperature .</param>
        public TemperatureC(double Centrigrade) : this() { _Kelvin = Centrigrade + 273.15; }

        /// <summary>
        /// Creates a new  Temperature  with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="temp">The value of the Temperature .</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="temp"/> value is used.</param>
        public TemperatureC(double temp, TemperatureCUnit unit) : this()
        {
            switch (unit)
            {
                case TemperatureCUnit.Kelvin:
                    _Kelvin = temp;
                    break;

                case TemperatureCUnit.Celsius:
                    Celsius = temp;
                    break;

                case TemperatureCUnit.Fahrenheit:
                    Fahrenheit = temp;
                    break;

                case TemperatureCUnit.Rankine:
                    Rankine = temp;
                    break;

                default:
                    throw new ArgumentException("The Temperature  unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Kelvin.
        /// </summary>
        public double Kelvin
        {
            get { return _Kelvin; }
            set { _Kelvin = value; }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Celsius.
        /// </summary>
        public double Celsius
        {
            get { return KelvintoCelsius(_Kelvin); }
            set { _Kelvin = CelsiusToKelvin(value); }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Fahrenheit.
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
        /// Gets the Temperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Temperature  should be retrieved.</param>
        /// <return  s>The Temperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueOut(TemperatureUnit unit)
        {
            switch (unit)
            {
                case TemperatureUnit.Kelvin: return _Kelvin;
                case TemperatureUnit.Celsius: return Celsius;
                case TemperatureUnit.Fahrenheit: return Fahrenheit;
                case TemperatureUnit.Rankine: return Rankine;
                default:
                    throw new ArgumentException("Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// Gets the Temperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Temperature  should be retrieved.</param>
        /// <return  s>The Temperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public void ValueIn(TemperatureUnit unit, double value)
        {
            switch (unit)
            {
                case TemperatureUnit.Kelvin: _Kelvin = value; break;
                case TemperatureUnit.Celsius: Celsius = value; break;
                case TemperatureUnit.Fahrenheit: Fahrenheit = value; break;
                case TemperatureUnit.Rankine: Rankine = value; break;
                default:
                    throw new ArgumentException("Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            if (Enum.TryParse(unit, out TemperatureUnit U))
                return ValueOut(U);
            else
                return double.NaN;
        }

        public void ValueIn(string unit, double value)
        {
            if (Enum.TryParse(unit, out TemperatureUnit U))
                ValueIn(U, value);
            else
                ValueIn(TemperatureUnit.Celsius, value);
            //return   double.NaN;
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Temperature . The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
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
                    return Rankine.ToString("F2", provider) + " R";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// Temperature . The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
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
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <return  s>A string  representation of the Temperature .</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="unit">
        /// The Temperature  unit as which the Temperature  value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
        public string ToString(TemperatureUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return ToString("C", provider);

                case TemperatureUnit.Fahrenheit:
                    return ToString("F", provider);

                case TemperatureUnit.Kelvin:
                    return ToString("K", provider);

                case TemperatureUnit.Rankine:
                    return ToString("R", provider);

                default:
                    throw new FormatException("The Temperature  unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="unit">
        /// The Temperature  unit as which the Temperature  value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
        public string ToString(TemperatureUnit unit)
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

        public string Name { get => "Temperature"; }
        public bool HasChanged { get; set; }

        public double Tolerance => 1e-6;

        double IUOM.DisplayValue
        {
            get
            {
                return Math.Round(ValueOut(TemperatureUnit.Celsius), 2, MidpointRounding.AwayFromZero);
            }
            set
            {
                ValueIn(TemperatureUnit.Celsius, value);
            }
        }

        /// <summary>
        /// Compares this instance to a specified Temperature  object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A Temperature  object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(Temperature value)
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
        /// The value is not a Temperature .
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is Temperature)) throw new ArgumentException();
            return CompareTo((Temperature)value);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(Temperature value)
        {
            return _Kelvin == value._Kelvin;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the Temperature .
        /// </summary>
        /// <param name="value">The object  to compare to the Temperature .</param>
        /// <return  s>True if the object  is considered equal
        ///     to the Temperature . Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is Temperature)) return false;

            return Equals((Temperature)value);
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
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(TemperatureC t1, TemperatureC t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(TemperatureC t1, TemperatureC t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(TemperatureC t1, TemperatureC t2)
        {
            return t1._Kelvin > t2._Kelvin;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(TemperatureC t1, TemperatureC t2)
        {
            return t1._Kelvin < t2._Kelvin;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(TemperatureC t1, TemperatureC t2)
        {
            return t1._Kelvin >= t2._Kelvin;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(TemperatureC t1, TemperatureC t2)
        {
            return t1._Kelvin <= t2._Kelvin;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static Temperature operator +(TemperatureC t1, TemperatureC t2)
        {
            return new Temperature(t1._Kelvin + t2._Kelvin);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static Temperature operator -(TemperatureC t1, TemperatureC t2)
        {
            return new Temperature(t1._Kelvin - t2._Kelvin);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static Temperature operator *(TemperatureC t1, TemperatureC t2)
        {
            return new Temperature(t1._Kelvin * t2._Kelvin);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static Temperature operator /(TemperatureC t1, TemperatureC t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Temperature(t1._Kelvin / t2._Kelvin);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Temperature operator %(TemperatureC t1, TemperatureC t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new TemperatureC(t1._Kelvin % t2._Kelvin);
        }

        /// <summary>
        /// Converts a Kelvin Temperature  value to Celsius.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double KelvintoCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="celsius">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double CelsiusToKelvin(double celsius)
        {
            return celsius + 273.15;
        }

        /// <summary>
        /// Converts a Kelvin value to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Kelvin value in Fahrenheit.</return  s>
        public static double KelvintoFahrenheit(double kelvin)
        {
            return kelvin * 1.8 - 459.67;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Kelvin.
        /// </param>
        /// <return  s>The Fahrenheit value in Kelvin.</return  s>
        public static double FahrenheitToKelvin(double fahrenheit)
        {
            return (fahrenheit + 459.67) / 1.8;
        }

        /// <summary>
        /// Converts a Kelvin value to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Kelvin value in Fahrenheit.</return  s>
        public static double KelvintoRankine(double kelvin)
        {
            return kelvin * 1.8;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Kelvin.
        /// </param>
        /// <return  s>The Fahrenheit value in Kelvin.</return  s>
        public static double RankineToKelvin(double fahrenheit)
        {
            return fahrenheit / 1.8;
        }

        /// <summary>
        /// Converts a Fahrenheit value to Celsius.
        /// </summary>
        /// <param name="fahrenheit">The Fahrenheit value to convert to Celsius.
        /// </param>
        /// <return  s>The Fahrenheit value in Celsius.</return  s>
        public static double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) / 1.8;
        }

        /// <summary>
        /// Converts a Celsius value to Fahrenheit.
        /// </summary>
        /// <param name="celsius">The Celsius value to convert to Fahrenheit.
        /// </param>
        /// <return  s>The Celsius value in Fahrenheit.</return  s>
        public static double CelsiusToFahrenheit(double celsius)
        {
            return celsius * 1.8 + 32;
        }

        public static implicit operator double(TemperatureC t)
        {
            return t._Kelvin;
        }

        public static implicit operator TemperatureC(double t)
        {
            return new TemperatureC(t);
        }

        public static implicit operator TemperatureC(Temperature t)
        {
            return new TemperatureC(t.Celsius);
        }

        public static implicit operator Temperature(TemperatureC t)
        {
            return new Temperature(t.Kelvin);
        }

        public void ToDefault(TemperatureUnit from, double val)
        {
            switch (from)
            {
                case TemperatureUnit.Kelvin:
                    _Kelvin = val;
                    break;

                case TemperatureUnit.Celsius:
                    _Kelvin = val - 273.15;
                    break;

                case TemperatureUnit.Fahrenheit:
                    _Kelvin = (val - 32) / 1.8;
                    break;

                case TemperatureUnit.Rankine:
                    _Kelvin = val / 1.8;
                    break;

                default:
                    break;
            }
        }

        public double FromDefault(TemperatureUnit to)
        {
            switch (to)
            {
                case TemperatureUnit.Kelvin:
                    return _Kelvin;

                case TemperatureUnit.Celsius:
                    return _Kelvin + 273.15;

                case TemperatureUnit.Fahrenheit:
                    return (_Kelvin - 273.15) * 1.8 + 32;

                case TemperatureUnit.Rankine:
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