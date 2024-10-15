using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for MassFlow measurement units.
    /// </summary>
    public enum MassFlowUnit
    {
        kg_hr,
        kg_s,
        te_d,
        te_hr,
        lbs_hr,
        short_te_hr
    }

    /// <summary>
    /// A MassFlow value.
    /// </summary>
    [Serializable]
    public struct MassFlow : IFormattable, IComparable, IComparable<MassFlow>, IEquatable<MassFlow>, IUOM
    {
        private double _kg_hr = double.NaN;
        public const double kg_per_lb = 0.453592;

        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_kg_hr, x);
        }

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out MassFlowUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public ePropID propid => ePropID.MF;

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_kg_hr))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Creates a new  MassFlow with the specified value in Kelvin.
        /// </summary>
        /// <param name="kelvin">The value of the MassFlow.</param>
        public MassFlow(double kelvin) : this() { _kg_hr = kelvin; }

        /// <summary>
        /// Creates a new  MassFlow with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="MassFlow">The value of the MassFlow.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="MassFlow"/> value is used.</param>
        public MassFlow(double MassFlow, MassFlowUnit unit) : this()
        {
            switch (unit)
            {
                case MassFlowUnit.kg_hr:
                    _kg_hr = MassFlow;
                    break;

                case MassFlowUnit.kg_s:
                    kg_s = MassFlow / 3600;
                    break;

                case MassFlowUnit.te_d:
                    _kg_hr = MassFlow * 1000 / 24;
                    break;

                case MassFlowUnit.te_hr:
                    _kg_hr = MassFlow * 1000;
                    break;

                case MassFlowUnit.lbs_hr:
                    _kg_hr = MassFlow * kg_per_lb;
                    break;

                default:
                    throw new ArgumentException("The MassFlow unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the MassFlow value in Kelvin.
        /// </summary>
        public double kg_hr
        {
            get { return _kg_hr; }
            set { _kg_hr = value; }
        }

        /// <summary>
        /// Gets or sets the MassFlow value in Celsius.
        /// </summary>
        public double kg_s
        {
            get { return kg_hr_to_kg_s(_kg_hr); }
            set { _kg_hr = kg_s_to_kg_hr(value); }
        }

        public double te_d
        {
            get { return _kg_hr * 24 / 1000; }
            set { _kg_hr = value / 24 * 1000; }
        }

        public double te_hr
        {
            get { return _kg_hr / 1000; }
            set { _kg_hr = value * 1000; }
        }

        public double lbs_hr
        {
            get { return _kg_hr / kg_per_lb; }
            set { _kg_hr = value * kg_per_lb; }
        }
        public double st_hr
        {
            get { return _kg_hr / 1.10231/1000; }
            set { _kg_hr = value * 1.10231*1000; }
        }

        public void EraseValue()
        {
            _kg_hr = double.NaN;
        }

        public double ValueIn(MassFlowUnit unit, double val)
        {
            switch (unit)
            {
                case MassFlowUnit.kg_hr: _kg_hr = val; return _kg_hr;
                case MassFlowUnit.kg_s: kg_s = val; return kg_s;
                case MassFlowUnit.te_d: te_d = val; return te_d;
                case MassFlowUnit.te_hr: te_hr = val; return te_hr;
                case MassFlowUnit.lbs_hr: lbs_hr = val; return lbs_hr;
                default:
                    throw new ArgumentException("Unknown MassFlow unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// Gets the MassFlow value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the MassFlow should be retrieved.</param>
        /// <return  s>The MassFlow value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(MassFlowUnit unit)
        {
            switch (unit)
            {
                case MassFlowUnit.kg_hr: return _kg_hr;
                case MassFlowUnit.kg_s: return kg_s;
                case MassFlowUnit.te_d: return te_d;
                case MassFlowUnit.te_hr: return te_hr;
                default:
                    throw new ArgumentException("Unknown MassFlow unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the MassFlow value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// MassFlow. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the MassFlow.</return  s>
        /// <exception cref="FormatException">
        /// The value of format is not null, the empty string  (""), "G", "C", "F", or
        /// "K".
        /// </exception>
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "kg_hr":
                    return kg_hr.ToString("F2", provider) + " kg/hr";

                case "kg_s":
                    return kg_s.ToString("F2", provider) + " kg/s";

                case "te_hr":
                    return kg_s.ToString("F2", provider) + " te/hr";

                case "te_d":
                    return kg_s.ToString("F2", provider) + " te/d";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the MassFlow value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// MassFlow. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the MassFlow.</return  s>
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
        /// return  s a string  representation of the MassFlow value.
        /// </summary>
        /// <return  s>A string  representation of the MassFlow.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the MassFlow value.
        /// </summary>
        /// <param name="unit">
        /// The MassFlow unit as which the MassFlow value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the MassFlow.</return  s>
        public string ToString(MassFlowUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case MassFlowUnit.kg_hr:
                    return ToString("kg/hr", provider);

                case MassFlowUnit.kg_s:
                    return ToString("kg/s", provider);

                default:
                    throw new FormatException("The MassFlow unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the MassFlow value.
        /// </summary>
        /// <param name="unit">
        /// The MassFlow unit as which the MassFlow value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the MassFlow.</return  s>
        public string ToString(MassFlowUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _kg_hr;
            }
            set
            {
                _kg_hr = value;
            }
        }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(MassFlowUnit), MassFlowUnit.kg_hr);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(MassFlowUnit));
            }
        }

        public string Name => "Mass Flow";

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
        /// Compares this instance to a specified MassFlow object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A MassFlow object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(MassFlow value)
        {
            return _kg_hr.CompareTo(value._kg_hr);
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
        /// The value is not a MassFlow.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is MassFlow)) throw new ArgumentException();
            return CompareTo((MassFlow)value);
        }

        /// <summary>
        /// Determines whether or not the given MassFlow is considered equal to this instance.
        /// </summary>
        /// <param name="value">The MassFlow to compare to this instance.</param>
        /// <return  s>True if the MassFlow is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(MassFlow value)
        {
            return _kg_hr == value._kg_hr;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the MassFlow.
        /// </summary>
        /// <param name="value">The object  to compare to the MassFlow.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the MassFlow. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is MassFlow)) return false;

            return Equals((MassFlow)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _kg_hr.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two MassFlows.
        /// </summary>
        /// <param name="t1">The first MassFlow to be compared.</param>
        /// <param name="t2">The second MassFlow to be compared.</param>
        /// <return  s>True if the MassFlows are equal. Otherwise, false.</return  s>
        public static bool operator ==(MassFlow t1, MassFlow t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two MassFlows.
        /// </summary>
        /// <param name="t1">The first MassFlow to be compared.</param>
        /// <param name="t2">The second MassFlow to be compared.</param>
        /// <return  s>True if the MassFlows are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(MassFlow t1, MassFlow t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one MassFlow is considered greater than another.
        /// </summary>
        /// <param name="t1">The first MassFlow to be compared.</param>
        /// <param name="t2">The second MassFlow to be compared.</param>
        /// <return  s>True if the first MassFlow is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(MassFlow t1, MassFlow t2)
        {
            return t1._kg_hr > t2._kg_hr;
        }

        /// <summary>
        /// Determines whether one MassFlow is considered less than another.
        /// </summary>
        /// <param name="t1">The first MassFlow to be compared.</param>
        /// <param name="t2">The second MassFlow to be compared.</param>
        /// <return  s>True if the first MassFlow is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(MassFlow t1, MassFlow t2)
        {
            return t1._kg_hr < t2._kg_hr;
        }

        /// <summary>
        /// Determines whether one MassFlow is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first MassFlow to be compared.</param>
        /// <param name="t2">The second MassFlow to be compared.</param>
        /// <return  s>
        /// True if the first MassFlow is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(MassFlow t1, MassFlow t2)
        {
            return t1._kg_hr >= t2._kg_hr;
        }

        /// <summary>
        /// Determines whether one MassFlow is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first MassFlow to be compared.</param>
        /// <param name="t2">The second MassFlow to be compared.</param>
        /// <return  s>
        /// True if the first MassFlow is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(MassFlow t1, MassFlow t2)
        {
            return t1._kg_hr <= t2._kg_hr;
        }

        /// <summary>
        /// Adds two instances of the MassFlow object .
        /// </summary>
        /// <param name="t1">The MassFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The MassFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two MassFlows.</return  s>
        public static MassFlow operator +(MassFlow t1, MassFlow t2)
        {
            return new MassFlow(t1._kg_hr + t2._kg_hr);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The MassFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The MassFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two MassFlows.</return  s>
        public static MassFlow operator -(MassFlow t1, MassFlow t2)
        {
            return new MassFlow(t1._kg_hr - t2._kg_hr);
        }

        /// <summary>
        /// Multiplies two instances of the MassFlow object .
        /// </summary>
        /// <param name="t1">The MassFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The MassFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two MassFlows.</return  s>
        public static MassFlow operator *(MassFlow t1, MassFlow t2)
        {
            return new MassFlow(t1._kg_hr * t2._kg_hr);
        }

        /// <summary>
        /// Divides one instance of a MassFlow object  by another.
        /// </summary>
        /// <param name="t1">The MassFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The MassFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two MassFlow.</return  s>
        public static MassFlow operator /(MassFlow t1, MassFlow t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MassFlow(t1._kg_hr / t2._kg_hr);
        }

        /// <summary>
        /// Finds the remainder when one instance of a MassFlow object  is divided by another.
        /// </summary>
        /// <param name="t1">The MassFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The MassFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static MassFlow operator %(MassFlow t1, MassFlow t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MassFlow(t1._kg_hr % t2._kg_hr);
        }

        /// <summary>
        /// Converts a Kelvin MassFlow value to Celsius.
        /// </summary>
        /// <param name="kg_hr">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double kg_hr_to_kg_s(double kg_hr)
        {
            return kg_hr / 3600;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="kg_s">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double kg_s_to_kg_hr(double kg_s)
        {
            return kg_s * 3600;
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

        public static implicit operator double(MassFlow t)
        {
            return t.kg_hr;
        }

        public static implicit operator MassFlow(double t)
        {
            return new MassFlow(t);
        }

        public void DeltaValue(double v)
        {
            _kg_hr += v;
        }

        public double ValueOut(string unit)
        {
            MassFlowUnit U;
            if (Enum.TryParse(unit, out U))
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
            if (unit is null || unit is "")
                return;

            MassFlowUnit U;
            if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, v);
            }
            else
            {
                throw new ArgumentException("Unknown unit '" + unit.ToString() + "'.");
                //return   double.NaN;
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