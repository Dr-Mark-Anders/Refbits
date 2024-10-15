using System;
using System.Globalization;
using Units;
using Units.UOM;

/// <summary>
/// Options for LiqGasFlow measurement units.
/// </summary>
public enum LiqGasFlowUnit
{
    MSCFH,
}

/// <summary>
/// A LiqGasFlow value.
/// </summary>
[Serializable]
public struct LiqGasFlow : IFormattable, IComparable, IComparable<LiqGasFlow>, IEquatable<LiqGasFlow>, IUOM
{
    public double _value = double.NaN;
    public double Tolerance => 0.0001;

    public const double kg_per_lb = 0.453592;

    public double Pow(double x)
    {
        return Math.Pow(_value, x);
    }

    public string UnitDescriptor(string unit)
    {
        Enum.TryParse(unit, out LiqGasFlowUnit res);
        return Enumhelpers.GetEnumDescription(res);
    }

    public ePropID propid => ePropID.MOLEF;

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
    /// Creates a new  LiqGasFlow with the specified value in Kelvin.
    /// </summary>
    /// <param name="value">The value of the LiqGasFlow.</param>
    public LiqGasFlow(double value) : this() { _value = value; }

    /// <summary>
    /// Creates a new  LiqGasFlow with the specified value in the
    /// specified unit of measurement.
    /// </summary>
    /// <param name="LiqGasFlow">The value of the LiqGasFlow.</param>
    /// <param name="unit">The unit of measurement that defines how
    /// the <paramref name="LiqGasFlow"/> value is used.</param>
    public LiqGasFlow(double LiqGasFlow, LiqGasFlowUnit unit) : this()
    {
        switch (unit)
        {
            case LiqGasFlowUnit.MSCFH:
                _value = LiqGasFlow;
                break;

            default:
                throw new ArgumentException("The LiqGasFlow unit '" + unit.ToString() + "' is unknown.");
        }
    }

    public LiqGasFlow(double value, VolFlowUnit bpd) : this(value)
    {
    }

    /// <summary>
    /// Gets or sets the LiqGasFlow value in Kelvin.
    /// </summary>
    public double kgMole_hr
    {
        get { return _value; }
        set { _value = value; }
    }

    public double lbMole_hr
    {
        get { return _value / kg_per_lb; }
        set { _value = value * kg_per_lb; }
    }

    /// <summary>
    /// Gets or sets the LiqGasFlow value in Celsius.
    /// </summary>
    public double kgMole_s
    {
        get { return kgMole_hr_to_kgMole_s(_value); }
        set { _value = kgMole_s_to_kgMole_hr(value); }
    }

    public void EraseValue()
    {
        _value = double.NaN;
    }

    /// <summary>
    /// Gets the LiqGasFlow value in the specified unit of measurement.
    /// </summary>
    /// <param name="unit">The unit of measurement
    /// in which the LiqGasFlow should be retrieved.</param>
    /// <return  s>The LiqGasFlow value in the specified
    /// <paramref name="unit"/>.</return  s>
    public double Valueout(LiqGasFlowUnit unit)
    {
        switch (unit)
        {
            case LiqGasFlowUnit.MSCFH: return _value;
            default:
                throw new ArgumentException("Unknown LiqGasFlow unit '" + unit.ToString() + "'.");
        }
    }

    /// <summary>
    /// Gets the LiqGasFlow value in the specified unit of measurement.
    /// </summary>
    /// <param name="unit">The unit of measurement
    /// in which the LiqGasFlow should be retrieved.</param>
    /// <return  s>The LiqGasFlow value in the specified
    /// <paramref name="unit"/>.</return  s>
    public double ValueIn(LiqGasFlowUnit unit, double value)
    {
        switch (unit)
        {
            case LiqGasFlowUnit.MSCFH: _value = value; return value;
            default:
                throw new ArgumentException("Unknown LiqGasFlow unit '" + unit.ToString() + "'.");
        }
    }

    /// <summary>
    /// return  s a string  representation of the LiqGasFlow value.
    /// </summary>
    /// <param name="format">
    /// A single format specifier that indicates how to format the value of this
    /// LiqGasFlow. The format parameter can be "G",
    /// "C", "F", or "K". If format
    /// is null or the empty string  (""), "G" is used.
    /// </param>
    /// <param name="provider">
    /// An IFormatProvider reference that supplies culture-specific formatting
    /// services.
    /// </param>
    /// <return  s>A string  representation of the LiqGasFlow.</return  s>
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
                return kgMole_hr.ToString("F2", provider) + " kgmole/hr";

            case "kgMole_hr":
                return kgMole_hr.ToString("F2", provider) + " kgmole/hr";

            case "kgMole_s":
                return kgMole_s.ToString("F2", provider) + " kgmole/s";

            default:
                throw new FormatException(string.Format("The {0} format string  is not supported.", format));
        }
    }

    /// <summary>
    /// return  s a string  representation of the LiqGasFlow value.
    /// </summary>
    /// <param name="format">
    /// A single format specifier that indicates how to format the value of this
    /// LiqGasFlow. The format parameter can be "G",
    /// "C", "F", or "K". If format
    /// is null or the empty string  (""), "G" is used.
    /// </param>
    /// <return  s>A string  representation of the LiqGasFlow.</return  s>
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
    /// return  s a string  representation of the LiqGasFlow value.
    /// </summary>
    /// <return  s>A string  representation of the LiqGasFlow.</return  s>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    /// return  s a string  representation of the LiqGasFlow value.
    /// </summary>
    /// <param name="unit">
    /// The LiqGasFlow unit as which the LiqGasFlow value should be displayed.
    /// </param>
    /// <param name="provider">
    /// An IFormatProvider reference that supplies culture-specific formatting
    /// services.
    /// </param>
    /// <return  s>A string  representation of the LiqGasFlow.</return  s>
    public string ToString(LiqGasFlowUnit unit, IFormatProvider provider)
    {
        switch (unit)
        {
            case LiqGasFlowUnit.MSCFH:
                return ToString("MSCFH", provider);

            default:
                throw new FormatException("The LiqGasFlow unit '" + unit.ToString() + "' is unknown.");
        }
    }

    /// <summary>
    /// return  s a string  representation of the LiqGasFlow value.
    /// </summary>
    /// <param name="unit">
    /// The LiqGasFlow unit as which the LiqGasFlow value should be displayed.
    /// </param>
    /// <return  s>A string  representation of the LiqGasFlow.</return  s>
    public string ToString(LiqGasFlowUnit unit)
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

    public string DefaultUnit
    {
        get
        {
            return Enum.GetName(typeof(LiqGasFlowUnit), LiqGasFlowUnit.MSCFH);
        }
    }

    public string[] AllUnits
    {
        get
        {
            return Enum.GetNames(typeof(LiqGasFlowUnit));
        }
    }

    public string Name => "Molar Flow";

    double IUOM.DisplayValue
    {
        get
        {
            return Valueout(LiqGasFlowUnit.MSCFH).RoundToSignificantDigits(3);
        }
        set
        {
            BaseValue = value;
        }
    }

    /// <summary>
    /// Compares this instance to a specified LiqGasFlow object  and return  s an indication
    /// of their relative values.
    /// </summary>
    /// <param name="value">A LiqGasFlow object
    ///    to compare to this instance.</param>
    /// <return  s>
    /// A signed number indicating the relative values of this instance and value.
    /// Value Description A negative int eger This instance is less than value. Zero
    /// This instance is equal to value. A positive int eger This instance is greater
    /// than value.
    /// </return  s>
    public int CompareTo(LiqGasFlow value)
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
    /// The value is not a LiqGasFlow.
    /// </exception>
    public int CompareTo(object value)
    {
        if (value == null) return 1;
        if (!(value is LiqGasFlow)) throw new ArgumentException();
        return CompareTo((LiqGasFlow)value);
    }

    /// <summary>
    /// Determines whether or not the given LiqGasFlow is considered equal to this instance.
    /// </summary>
    /// <param name="value">The LiqGasFlow to compare to this instance.</param>
    /// <return  s>True if the LiqGasFlow is considered equal
    ///    to this instance. Otherwise, false.</return  s>
    public bool Equals(LiqGasFlow value)
    {
        return _value == value._value;
    }

    /// <summary>
    /// Determines whether or not the given object  is considered equal to the LiqGasFlow.
    /// </summary>
    /// <param name="value">The object  to compare to the LiqGasFlow.</param>
    /// <return  s>True if the object  is considered equal
    ///     to the LiqGasFlow. Otherwise, false.</return  s>
    public override bool Equals(object value)
    {
        if (value == null) return false;
        if (!(value is LiqGasFlow)) return false;

        return Equals((LiqGasFlow)value);
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
    /// Determines the eQuality  of two LiqGasFlows.
    /// </summary>
    /// <param name="t1">The first LiqGasFlow to be compared.</param>
    /// <param name="t2">The second LiqGasFlow to be compared.</param>
    /// <return  s>True if the LiqGasFlows are equal. Otherwise, false.</return  s>
    public static bool operator ==(LiqGasFlow t1, LiqGasFlow t2)
    {
        return t1.Equals(t2);
    }

    /// <summary>
    /// Determines the ineQuality  of two LiqGasFlows.
    /// </summary>
    /// <param name="t1">The first LiqGasFlow to be compared.</param>
    /// <param name="t2">The second LiqGasFlow to be compared.</param>
    /// <return  s>True if the LiqGasFlows are NOT equal. Otherwise, false.</return  s>
    public static bool operator !=(LiqGasFlow t1, LiqGasFlow t2)
    {
        return !t1.Equals(t2);
    }

    /// <summary>
    /// Determines whether one LiqGasFlow is considered greater than another.
    /// </summary>
    /// <param name="t1">The first LiqGasFlow to be compared.</param>
    /// <param name="t2">The second LiqGasFlow to be compared.</param>
    /// <return  s>True if the first LiqGasFlow is greater than the second.
    /// Otherwise, false.</return  s>
    public static bool operator >(LiqGasFlow t1, LiqGasFlow t2)
    {
        return t1._value > t2._value;
    }

    /// <summary>
    /// Determines whether one LiqGasFlow is considered less than another.
    /// </summary>
    /// <param name="t1">The first LiqGasFlow to be compared.</param>
    /// <param name="t2">The second LiqGasFlow to be compared.</param>
    /// <return  s>True if the first LiqGasFlow is less than the second.
    /// Otherwise, false.</return  s>
    public static bool operator <(LiqGasFlow t1, LiqGasFlow t2)
    {
        return t1._value < t2._value;
    }

    /// <summary>
    /// Determines whether one LiqGasFlow is considered greater to or equal to another.
    /// </summary>
    /// <param name="t1">The first LiqGasFlow to be compared.</param>
    /// <param name="t2">The second LiqGasFlow to be compared.</param>
    /// <return  s>
    /// True if the first LiqGasFlow is greater to or equal to the second. Otherwise, false.
    /// </return  s>
    public static bool operator >=(LiqGasFlow t1, LiqGasFlow t2)
    {
        return t1._value >= t2._value;
    }

    /// <summary>
    /// Determines whether one LiqGasFlow is considered less than or equal to another.
    /// </summary>
    /// <param name="t1">The first LiqGasFlow to be compared.</param>
    /// <param name="t2">The second LiqGasFlow to be compared.</param>
    /// <return  s>
    /// True if the first LiqGasFlow is less than or equal to the second. Otherwise, false.
    /// </return  s>
    public static bool operator <=(LiqGasFlow t1, LiqGasFlow t2)
    {
        return t1._value <= t2._value;
    }

    /// <summary>
    /// Adds two instances of the LiqGasFlow object .
    /// </summary>
    /// <param name="t1">The LiqGasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The LiqGasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The sum of the two LiqGasFlows.</return  s>
    public static LiqGasFlow operator +(LiqGasFlow t1, LiqGasFlow t2)
    {
        return new LiqGasFlow(t1._value + t2._value);
    }

    /// <summary>
    /// Subtracts one instance from another.
    /// </summary>
    /// <param name="t1">The LiqGasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The LiqGasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The difference of the two LiqGasFlows.</return  s>
    public static LiqGasFlow operator -(LiqGasFlow t1, LiqGasFlow t2)
    {
        return new LiqGasFlow(t1._value - t2._value);
    }

    /// <summary>
    /// Multiplies two instances of the LiqGasFlow object .
    /// </summary>
    /// <param name="t1">The LiqGasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The LiqGasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The product of the two LiqGasFlows.</return  s>
    public static LiqGasFlow operator *(LiqGasFlow t1, LiqGasFlow t2)
    {
        return new LiqGasFlow(t1._value * t2._value);
    }

    /// <summary>
    /// Divides one instance of a LiqGasFlow object  by another.
    /// </summary>
    /// <param name="t1">The LiqGasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The LiqGasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The quotient of the two LiqGasFlow.</return  s>
    public static LiqGasFlow operator /(LiqGasFlow t1, LiqGasFlow t2)
    {
        // TODO: throw  a divide-by-zero exception if needed.
        return new LiqGasFlow(t1._value / t2._value);
    }

    /// <summary>
    /// Finds the remainder when one instance of a LiqGasFlow object  is divided by another.
    /// </summary>
    /// <param name="t1">The LiqGasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The LiqGasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The remainder after the quotient is found.</return  s>
    public static LiqGasFlow operator %(LiqGasFlow t1, LiqGasFlow t2)
    {
        // TODO: throw  a divide-by-zero exception if needed.
        return new LiqGasFlow(t1._value % t2._value);
    }

    /// <summary>
    /// Converts a Kelvin LiqGasFlow value to Celsius.
    /// </summary>
    /// <param name="kgMole_hr">The Kelvin value to convert to Celsius.
    /// </param>
    /// <return  s>The Kelvin value in Celsius.</return  s>
    public static double kgMole_hr_to_kgMole_s(double kgMole_hr)
    {
        return kgMole_hr / 3600;
    }

    /// <summary>
    /// Converts a Celsius value to Kelvin.
    /// </summary>
    /// <param name="kgMole_s">The Celsius value to convert to Kelvin.
    /// </param>
    /// <return  s>The Celsius value in Kelvin.</return  s>
    public static double kgMole_s_to_kgMole_hr(double kgMole_s)
    {
        return kgMole_s * 3600;
    }

    public static implicit operator double(LiqGasFlow t)
    {
        return t.kgMole_hr;
    }

    public static implicit operator LiqGasFlow(double t)
    {
        return new LiqGasFlow(t);
    }

    public void DeltaValue(double v)
    {
        _value += v;
    }

    public double ValueOut(string unit)
    {
        LiqGasFlowUnit U;
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
        LiqGasFlowUnit U;
        if (Enum.TryParse(unit, out U))
        {
            ValueIn(U, v);
        }
        else
        {
            ValueIn(U, v); ;
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