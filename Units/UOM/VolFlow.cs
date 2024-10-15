using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for VolFlow measurement units.
    /// </summary>
    public enum VolFlowUnit
    {
        m3_hr,
        m3_s,
        kbpd,
        bpd,
        bphr
    }

    /// <summary>
    /// A VolFlow value.
    /// </summary>
    [Serializable]
    public struct VolumeFlow : IFormattable, IComparable, IComparable<VolumeFlow>, IEquatable<VolumeFlow>, IUOM
    {
        private const double BBL_Per_m3 = 6.2898;

        private double _m3_hr = double.NaN;
        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_m3_hr, x);
        }

        public string UnitDescriptor(string unit)
        {
            if (Enum.TryParse(unit, out VolFlowUnit res))
                return Enumhelpers.GetEnumDescription(res);
            else
                return null;
        }

        public ePropID propid => ePropID.VF;

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_m3_hr))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Creates a new  VolFlow with the specified value in Kelvin.
        /// </summary>
        /// <param name="value">The value of the VolFlow.</param>
        public VolumeFlow(double value) : this() { _m3_hr = value; }

        /// <summary>
        /// Creates a new  VolFlow with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="VolFlow">The value of the VolFlow.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="VolFlow"/> value is used.</param>
        public VolumeFlow(double VolFlow, VolFlowUnit unit) : this()
        {
            switch (unit)
            {
                case VolFlowUnit.m3_hr:
                    _m3_hr = VolFlow;
                    break;

                case VolFlowUnit.m3_s:
                    m3_hr = VolFlow * 3600;
                    break;

                case VolFlowUnit.kbpd:
                    m3_hr = VolFlow /BBL_Per_m3/1000/24;
                    break;

                case VolFlowUnit.bpd:
                    m3_hr = VolFlow / BBL_Per_m3 / 24;
                    break;

                case VolFlowUnit.bphr:
                    m3_hr = VolFlow / BBL_Per_m3;
                    break;

                default:
                    throw new ArgumentException("The VolFlow unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the VolFlow value in Kelvin.
        /// </summary>
        public double m3_hr
        {
            get { return _m3_hr; }
            set { _m3_hr = value; }
        }

        /// <summary>
        /// Gets or sets the VolFlow value in Celsius.
        /// </summary>
        public double m3_s
        {
            get { return m3e_hr_to_m3e_s(_m3_hr); }
            set { _m3_hr = m3e_s_to_m3e_hr(value); }
        }

        /// <summary>
        /// Gets or sets the VolFlow value in Fahrenheit.
        /// </summary>
        public double BPD
        {
            get { return _m3_hr * 24 * BBL_Per_m3; }
            set { _m3_hr = value / BBL_Per_m3 / 24; }
        }

        public double BPHr
        {
            get { return _m3_hr * BBL_Per_m3; }
            set { _m3_hr = value / BBL_Per_m3; }
        }

        public double KBPD
        {
            get { return BPD / 1000; }
            set { BPD = value * 1000; }
        }

        public void EraseValue()
        {
            _m3_hr = double.NaN;
        }

        /// <summary>
        /// Gets the VolFlow value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the VolFlow should be retrieved.</param>
        /// <return  s>The VolFlow value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(VolFlowUnit unit)
        {
            switch (unit)
            {
                case VolFlowUnit.m3_hr: return _m3_hr;
                case VolFlowUnit.m3_s: return m3_s;
                case VolFlowUnit.kbpd: return KBPD;
                case VolFlowUnit.bpd: return BPD;
                case VolFlowUnit.bphr: return BPHr;
                default:
                    throw new ArgumentException("Unknown VolFlow unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// Gets the VolFlow value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the VolFlow should be retrieved.</param>
        /// <return  s>The VolFlow value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(VolFlowUnit unit, double value)
        {
            switch (unit)
            {
                case VolFlowUnit.m3_hr: _m3_hr = value; return value;
                case VolFlowUnit.m3_s: m3_s = value; return value;
                default:
                    throw new ArgumentException("Unknown VolFlow unit '" + unit.ToString() + "'.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the VolFlow value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// VolFlow. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the VolFlow.</return  s>
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
                case "m3e_hr":
                    return m3_hr.ToString("F2", provider) + " m3e/hr";

                case "m3e_s":
                    return m3_s.ToString("F2", provider) + " m3e/s";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the VolFlow value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// VolFlow. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the VolFlow.</return  s>
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
        /// return  s a string  representation of the VolFlow value.
        /// </summary>
        /// <return  s>A string  representation of the VolFlow.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the VolFlow value.
        /// </summary>
        /// <param name="unit">
        /// The VolFlow unit as which the VolFlow value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the VolFlow.</return  s>
        public string ToString(VolFlowUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case VolFlowUnit.m3_hr:
                    return ToString("m3e/hr", provider);

                case VolFlowUnit.m3_s:
                    return ToString("m3e/s", provider);

                default:
                    throw new FormatException("The VolFlow unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the VolFlow value.
        /// </summary>
        /// <param name="unit">
        /// The VolFlow unit as which the VolFlow value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the VolFlow.</return  s>
        public string ToString(VolFlowUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _m3_hr;
            }
            set
            {
                _m3_hr = value;
            }
        }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(VolFlowUnit), VolFlowUnit.m3_hr);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(VolFlowUnit));
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
        /// Compares this instance to a specified VolFlow object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A VolFlow object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(VolumeFlow value)
        {
            return _m3_hr.CompareTo(value._m3_hr);
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
        /// The value is not a VolFlow.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (value is not VolumeFlow)
                return 1;
            return CompareTo((VolumeFlow)value);
        }

        /// <summary>
        /// Determines whether or not the given VolFlow is considered equal to this instance.
        /// </summary>
        /// <param name="value">The VolFlow to compare to this instance.</param>
        /// <return  s>True if the VolFlow is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(VolumeFlow value)
        {
            return _m3_hr == value._m3_hr;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the VolFlow.
        /// </summary>
        /// <param name="value">The object  to compare to the VolFlow.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the VolFlow. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (value is not VolumeFlow) return false;

            return Equals((VolumeFlow)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _m3_hr.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two VolFlows.
        /// </summary>
        /// <param name="t1">The first VolFlow to be compared.</param>
        /// <param name="t2">The second VolFlow to be compared.</param>
        /// <return  s>True if the VolFlows are equal. Otherwise, false.</return  s>
        public static bool operator ==(VolumeFlow t1, VolumeFlow t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two VolFlows.
        /// </summary>
        /// <param name="t1">The first VolFlow to be compared.</param>
        /// <param name="t2">The second VolFlow to be compared.</param>
        /// <return  s>True if the VolFlows are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(VolumeFlow t1, VolumeFlow t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one VolFlow is considered greater than another.
        /// </summary>
        /// <param name="t1">The first VolFlow to be compared.</param>
        /// <param name="t2">The second VolFlow to be compared.</param>
        /// <return  s>True if the first VolFlow is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(VolumeFlow t1, VolumeFlow t2)
        {
            return t1._m3_hr > t2._m3_hr;
        }

        /// <summary>
        /// Determines whether one VolFlow is considered less than another.
        /// </summary>
        /// <param name="t1">The first VolFlow to be compared.</param>
        /// <param name="t2">The second VolFlow to be compared.</param>
        /// <return  s>True if the first VolFlow is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(VolumeFlow t1, VolumeFlow t2)
        {
            return t1._m3_hr < t2._m3_hr;
        }

        /// <summary>
        /// Determines whether one VolFlow is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first VolFlow to be compared.</param>
        /// <param name="t2">The second VolFlow to be compared.</param>
        /// <return  s>
        /// True if the first VolFlow is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(VolumeFlow t1, VolumeFlow t2)
        {
            return t1._m3_hr >= t2._m3_hr;
        }

        /// <summary>
        /// Determines whether one VolFlow is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first VolFlow to be compared.</param>
        /// <param name="t2">The second VolFlow to be compared.</param>
        /// <return  s>
        /// True if the first VolFlow is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(VolumeFlow t1, VolumeFlow t2)
        {
            return t1._m3_hr <= t2._m3_hr;
        }

        /// <summary>
        /// Adds two instances of the VolFlow object .
        /// </summary>
        /// <param name="t1">The VolFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The VolFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two VolFlows.</return  s>
        public static VolumeFlow operator +(VolumeFlow t1, VolumeFlow t2)
        {
            return new VolumeFlow(t1._m3_hr + t2._m3_hr);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The VolFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The VolFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two VolFlows.</return  s>
        public static VolumeFlow operator -(VolumeFlow t1, VolumeFlow t2)
        {
            return new VolumeFlow(t1._m3_hr - t2._m3_hr);
        }

        /// <summary>
        /// Multiplies two instances of the VolFlow object .
        /// </summary>
        /// <param name="t1">The VolFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The VolFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two VolFlows.</return  s>
        public static VolumeFlow operator *(VolumeFlow t1, VolumeFlow t2)
        {
            return new VolumeFlow(t1._m3_hr * t2._m3_hr);
        }

        /// <summary>
        /// Divides one instance of a VolFlow object  by another.
        /// </summary>
        /// <param name="t1">The VolFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The VolFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two VolFlow.</return  s>
        public static VolumeFlow operator /(VolumeFlow t1, VolumeFlow t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new VolumeFlow(t1._m3_hr / t2._m3_hr);
        }

        /// <summary>
        /// Finds the remainder when one instance of a VolFlow object  is divided by another.
        /// </summary>
        /// <param name="t1">The VolFlow on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The VolFlow on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static VolumeFlow operator %(VolumeFlow t1, VolumeFlow t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new VolumeFlow(t1._m3_hr % t2._m3_hr);
        }

        /// <summary>
        /// Converts a Kelvin VolFlow value to Celsius.
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

        public static implicit operator double(VolumeFlow t)
        {
            return t.m3_hr;
        }

        public static implicit operator VolumeFlow(double t)
        {
            return new VolumeFlow(t);
        }

        public void DeltaValue(double v)
        {
            _m3_hr += v;
        }

        public double ValueOut(string unit)
        {
            if (Enum.TryParse(unit, out VolFlowUnit U))
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
            if (Enum.TryParse(unit, out VolFlowUnit U))
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