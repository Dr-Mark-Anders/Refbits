using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for AMPS measurement units.
    /// </summary>
    public enum AMPSUnit
    {
        amps
    }

    /// <summary>
    /// A AMPS value.
    /// </summary>
    [Serializable]
    public struct AMPS : IFormattable, IComparable, IComparable<AMPS>, IEquatable<AMPS>, IUOM
    {
        private double _amps = double.NaN;
        private string name = "";

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out AMPSUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_amps, x);
        }

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_amps))
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Creates a new  AMPS with the specified value in Kelvin.
        /// </summary>
        /// <param name="AMPS">The value of the AMPS.</param>
        public AMPS(double AMPS) : this() { _amps = AMPS; }

        /// <summary>
        /// Creates a new  AMPS with the specified value in the
        /// specified unit of measurement.
        /// </summary>
        /// <param name="AMPS">The value of the AMPS.</param>
        /// <param name="unit">The unit of measurement that defines how
        /// the <paramref name="AMPS"/> value is used.</param>
        public AMPS(double AMPS, AMPSUnit unit) : this()
        {
            switch (unit)
            {
                case AMPSUnit.amps:
                    _amps = AMPS;
                    break;

                default:
                    throw new ArgumentException("The AMPS unit '" + unit.ToString() + "' is unknown.");
            }
        }

        public void EraseValue()
        {
            _amps = double.NaN;
        }

        /// <summary>
        /// return  s a string  representation of the AMPS value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// AMPS. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <param name="formatProvider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the AMPS.</return  s>
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
                    return _amps.ToString("F2", formatProvider) + " m";

                default:
                    throw new FormatException(string.Format("The {0} format string  is not supported.", format));
            }
        }

        /// <summary>
        /// return  s a string  representation of the AMPS value.
        /// </summary>
        /// <param name="format">
        /// A single format specifier that indicates how to format the value of this
        /// AMPS. The format parameter can be "G",
        /// "C", "F", or "K". If format
        /// is null or the empty string  (""), "G" is used.
        /// </param>
        /// <return  s>A string  representation of the AMPS.</return  s>
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
        /// return  s a string  representation of the AMPS value.
        /// </summary>
        /// <return  s>A string  representation of the AMPS.</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return  s a string  representation of the AMPS value.
        /// </summary>
        /// <param name="unit">
        /// The AMPS unit as which the AMPS value should be displayed.
        /// </param>
        /// <param name="provider">
        /// An IFormatProvider reference that supplies culture-specific formatting
        /// services.
        /// </param>
        /// <return  s>A string  representation of the AMPS.</return  s>
        public string ToString(AMPSUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case AMPSUnit.amps:
                    return ToString("m", provider);

                default:
                    throw new FormatException("The AMPS unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the AMPS value.
        /// </summary>
        /// <param name="unit">
        /// The AMPS unit as which the AMPS value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the AMPS.</return  s>
        public string ToString(AMPSUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _amps;
            }
            set
            {
                _amps = value;
            }
        }

        public string DefaultUnit => AMPSUnit.amps.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(AMPSUnit));
            }
        }

        public string Name => name;

        public ePropID propid => ePropID.ElectricalFlow;

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
        /// Compares this instance to a specified AMPS object  and return  s an indication
        /// of their relative values.
        /// </summary>
        /// <param name="value">A AMPS object
        ///    to compare to this instance.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value.
        /// </return  s>
        public int CompareTo(AMPS value)
        {
            return _amps.CompareTo(value._amps);
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
        /// The value is not a AMPS.
        /// </exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is AMPS)) throw new ArgumentException();
            return CompareTo((AMPS)value);
        }

        /// <summary>
        /// Determines whether or not the given AMPS is considered equal to this instance.
        /// </summary>
        /// <param name="value">The AMPS to compare to this instance.</param>
        /// <return  s>True if the AMPS is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(AMPS value)
        {
            return _amps == value._amps;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the AMPS.
        /// </summary>
        /// <param name="value">The object  to compare to the AMPS.</param>
        /// <return  s>True if the object  is considered equal
        ///     to the AMPS. Otherwise, false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is AMPS)) return false;

            return Equals((AMPS)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _amps.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two AMPSs.
        /// </summary>
        /// <param name="t1">The first AMPS to be compared.</param>
        /// <param name="t2">The second AMPS to be compared.</param>
        /// <return  s>True if the AMPSs are equal. Otherwise, false.</return  s>
        public static bool operator ==(AMPS t1, AMPS t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two AMPSs.
        /// </summary>
        /// <param name="t1">The first AMPS to be compared.</param>
        /// <param name="t2">The second AMPS to be compared.</param>
        /// <return  s>True if the AMPSs are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(AMPS t1, AMPS t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Determines whether one AMPS is considered greater than another.
        /// </summary>
        /// <param name="t1">The first AMPS to be compared.</param>
        /// <param name="t2">The second AMPS to be compared.</param>
        /// <return  s>True if the first AMPS is greater than the second.
        /// Otherwise, false.</return  s>
        public static bool operator >(AMPS t1, AMPS t2)
        {
            return t1._amps > t2._amps;
        }

        /// <summary>
        /// Determines whether one AMPS is considered less than another.
        /// </summary>
        /// <param name="t1">The first AMPS to be compared.</param>
        /// <param name="t2">The second AMPS to be compared.</param>
        /// <return  s>True if the first AMPS is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(AMPS t1, AMPS t2)
        {
            return t1._amps < t2._amps;
        }

        /// <summary>
        /// Determines whether one AMPS is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first AMPS to be compared.</param>
        /// <param name="t2">The second AMPS to be compared.</param>
        /// <return  s>
        /// True if the first AMPS is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(AMPS t1, AMPS t2)
        {
            return t1._amps >= t2._amps;
        }

        /// <summary>
        /// Determines whether one AMPS is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first AMPS to be compared.</param>
        /// <param name="t2">The second AMPS to be compared.</param>
        /// <return  s>
        /// True if the first AMPS is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(AMPS t1, AMPS t2)
        {
            return t1._amps <= t2._amps;
        }

        /// <summary>
        /// Adds two instances of the AMPS object .
        /// </summary>
        /// <param name="t1">The AMPS on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The AMPS on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two AMPSs.</return  s>
        public static AMPS operator +(AMPS t1, AMPS t2)
        {
            return new AMPS(t1._amps + t2._amps);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The AMPS on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The AMPS on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two AMPSs.</return  s>
        public static AMPS operator -(AMPS t1, AMPS t2)
        {
            return new AMPS(t1._amps - t2._amps);
        }

        /// <summary>
        /// Multiplies two instances of the AMPS object .
        /// </summary>
        /// <param name="t1">The AMPS on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The AMPS on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two AMPSs.</return  s>
        public static AMPS operator *(AMPS t1, AMPS t2)
        {
            return new AMPS(t1._amps * t2._amps);
        }

        /// <summary>
        /// Divides one instance of a AMPS object  by another.
        /// </summary>
        /// <param name="t1">The AMPS on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The AMPS on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two AMPS.</return  s>
        public static AMPS operator /(AMPS t1, AMPS t2)
        {
            return new AMPS(t1._amps / t2._amps);
        }

        /// <summary>
        /// Finds the remainder when one instance of a AMPS object  is divided by another.
        /// </summary>
        /// <param name="t1">The AMPS on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The AMPS on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static AMPS operator %(AMPS t1, AMPS t2)
        {
            return new AMPS(t1._amps % t2._amps);
        }

        public static implicit operator AMPS(double t)
        {
            return new AMPS(t);
        }

        public void DeltaValue(double v)
        {
            _amps += v;
        }

        /// <summary>
        /// Gets the AMPS value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the AMPS should be retrieved.</param>
        /// <return  s>The AMPS value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double Valueout(AMPSUnit unit, double value)
        {
            switch (unit)
            {
                case AMPSUnit.amps: _amps = value; return value;
                default:
                    throw new ArgumentException("Unknown AMPS unit '" + unit.ToString() + "'.");
            }
        }

        public double Valueout(AMPSUnit unit)
        {
            switch (unit)
            {
                case AMPSUnit.amps: return _amps;
                default:
                    throw new ArgumentException("Unknown Pressure  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            AMPSUnit U;
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
        /// Gets the AMPS value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the AMPS should be retrieved.</param>
        /// <return  s>The AMPS value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(AMPSUnit unit)
        {
            switch (unit)
            {
                case AMPSUnit.amps: return _amps;
                default:
                    throw new ArgumentException("Unknown AMPS unit '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(AMPSUnit unit, double value)
        {
            switch (unit)
            {
                case AMPSUnit.amps: _amps = value; break;
                default:
                    break;
            }
        }

        public void ValueIn(string unit, double value)
        {
            AMPSUnit U;
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