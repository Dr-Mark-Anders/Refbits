using System;
using System.Globalization;
using Units;
using Units.UOM;

/// <summary>
/// Options for MoleFlow measurement units.
/// </summary>
public enum MoleFlowUnit
{
    kgmol_hr,
    kgmol_s,
    lb_moles_hr,
}

/// <summary>
/// A MoleFlow value.
/// </summary>
[Serializable]
public struct MoleFlow : IFormattable, IComparable, IComparable<MoleFlow>, IEquatable<MoleFlow>, IUOM
{
    public double _value=double.NaN;
    public double Tolerance => 0.0001;

    public const double kg_per_lb = 0.453592;

    public double Pow(double x)
    {
        return Math.Pow(_value, x);
    }

    public string UnitDescriptor(string unit)
    {
        Enum.TryParse(unit, out MoleFlowUnit res);
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
    /// Creates a new  MoleFlow with the specified value in Kelvin.
    /// </summary>
    /// <param name="value">The value of the MoleFlow.</param>
    public MoleFlow(double value) : this() { _value = value; }

    /// <summary>
    /// Creates a new  MoleFlow with the specified value in the
    /// specified unit of measurement.
    /// </summary>
    /// <param name="MoleFlow">The value of the MoleFlow.</param>
    /// <param name="unit">The unit of measurement that defines how
    /// the <paramref name="MoleFlow"/> value is used.</param>
    public MoleFlow(double MoleFlow, MoleFlowUnit unit) : this()
    {
        switch (unit)
        {
            case MoleFlowUnit.kgmol_hr:
                _value = MoleFlow;
                break;

            case MoleFlowUnit.kgmol_s:
                kgMole_s = MoleFlow;
                break;

            case MoleFlowUnit.lb_moles_hr:
                _value = MoleFlow;
                break;

            default:
                throw new ArgumentException("The MoleFlow unit '" + unit.ToString() + "' is unknown.");
        }
    }

    /// <summary>
    /// Gets or sets the MoleFlow value in Kelvin.
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
    /// Gets or sets the MoleFlow value in Celsius.
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
    /// Gets the MoleFlow value in the specified unit of measurement.
    /// </summary>
    /// <param name="unit">The unit of measurement
    /// in which the MoleFlow should be retrieved.</param>
    /// <return  s>The MoleFlow value in the specified
    /// <paramref name="unit"/>.</return  s>
    public double Valueout(MoleFlowUnit unit)
    {
        switch (unit)
        {
            case MoleFlowUnit.kgmol_hr: return _value;
            case MoleFlowUnit.kgmol_s: return kgMole_s;
            default:
                throw new ArgumentException("Unknown MoleFlow unit '" + unit.ToString() + "'.");
        }
    }

    /// <summary>
    /// Gets the MoleFlow value in the specified unit of measurement.
    /// </summary>
    /// <param name="unit">The unit of measurement
    /// in which the MoleFlow should be retrieved.</param>
    /// <return  s>The MoleFlow value in the specified
    /// <paramref name="unit"/>.</return  s>
    public double ValueIn(MoleFlowUnit unit, double value)
    {
        switch (unit)
        {
            case MoleFlowUnit.kgmol_hr: _value = value; return value;
            case MoleFlowUnit.kgmol_s: kgMole_s = value; return value;
            default:
                throw new ArgumentException("Unknown MoleFlow unit '" + unit.ToString() + "'.");
        }
    }

    /// <summary>
    /// return  s a string  representation of the MoleFlow value.
    /// </summary>
    /// <param name="format">
    /// A single format specifier that indicates how to format the value of this
    /// MoleFlow. The format parameter can be "G",
    /// "C", "F", or "K". If format
    /// is null or the empty string  (""), "G" is used.
    /// </param>
    /// <param name="provider">
    /// An IFormatProvider reference that supplies culture-specific formatting
    /// services.
    /// </param>
    /// <return  s>A string  representation of the MoleFlow.</return  s>
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
    /// return  s a string  representation of the MoleFlow value.
    /// </summary>
    /// <param name="format">
    /// A single format specifier that indicates how to format the value of this
    /// MoleFlow. The format parameter can be "G",
    /// "C", "F", or "K". If format
    /// is null or the empty string  (""), "G" is used.
    /// </param>
    /// <return  s>A string  representation of the MoleFlow.</return  s>
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
    /// return  s a string  representation of the MoleFlow value.
    /// </summary>
    /// <return  s>A string  representation of the MoleFlow.</return  s>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    /// return  s a string  representation of the MoleFlow value.
    /// </summary>
    /// <param name="unit">
    /// The MoleFlow unit as which the MoleFlow value should be displayed.
    /// </param>
    /// <param name="provider">
    /// An IFormatProvider reference that supplies culture-specific formatting
    /// services.
    /// </param>
    /// <return  s>A string  representation of the MoleFlow.</return  s>
    public string ToString(MoleFlowUnit unit, IFormatProvider provider)
    {
        switch (unit)
        {
            case MoleFlowUnit.kgmol_hr:
                return ToString("kgmole/hr", provider);

            case MoleFlowUnit.kgmol_s:
                return ToString("kgmole/s", provider);

            default:
                throw new FormatException("The MoleFlow unit '" + unit.ToString() + "' is unknown.");
        }
    }

    /// <summary>
    /// return  s a string  representation of the MoleFlow value.
    /// </summary>
    /// <param name="unit">
    /// The MoleFlow unit as which the MoleFlow value should be displayed.
    /// </param>
    /// <return  s>A string  representation of the MoleFlow.</return  s>
    public string ToString(MoleFlowUnit unit)
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
            return Enum.GetName(typeof(MoleFlowUnit), MoleFlowUnit.kgmol_hr);
        }
    }

    public string[] AllUnits
    {
        get
        {
            return Enum.GetNames(typeof(MoleFlowUnit));
        }
    }

    public string Name => "Molar Flow";

    double IUOM.DisplayValue
    {
        get
        {
            return Valueout(MoleFlowUnit.kgmol_hr).RoundToSignificantDigits(3);
        }
        set
        {
            BaseValue = value;
        }
    }

    /// <summary>
    /// Compares this instance to a specified MoleFlow object  and return  s an indication
    /// of their relative values.
    /// </summary>
    /// <param name="value">A MoleFlow object
    ///    to compare to this instance.</param>
    /// <return  s>
    /// A signed number indicating the relative values of this instance and value.
    /// Value Description A negative int eger This instance is less than value. Zero
    /// This instance is equal to value. A positive int eger This instance is greater
    /// than value.
    /// </return  s>
    public int CompareTo(MoleFlow value)
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
    /// The value is not a MoleFlow.
    /// </exception>
    public int CompareTo(object value)
    {
        if (value == null) return 1;
        if (!(value is MoleFlow)) throw new ArgumentException();
        return CompareTo((MoleFlow)value);
    }

    /// <summary>
    /// Determines whether or not the given MoleFlow is considered equal to this instance.
    /// </summary>
    /// <param name="value">The MoleFlow to compare to this instance.</param>
    /// <return  s>True if the MoleFlow is considered equal
    ///    to this instance. Otherwise, false.</return  s>
    public bool Equals(MoleFlow value)
    {
        return _value == value._value;
    }

    /// <summary>
    /// Determines whether or not the given object  is considered equal to the MoleFlow.
    /// </summary>
    /// <param name="value">The object  to compare to the MoleFlow.</param>
    /// <return  s>True if the object  is considered equal
    ///     to the MoleFlow. Otherwise, false.</return  s>
    public override bool Equals(object value)
    {
        if (value == null) return false;
        if (!(value is MoleFlow)) return false;

        return Equals((MoleFlow)value);
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
    /// Determines the eQuality  of two MoleFlows.
    /// </summary>
    /// <param name="t1">The first MoleFlow to be compared.</param>
    /// <param name="t2">The second MoleFlow to be compared.</param>
    /// <return  s>True if the MoleFlows are equal. Otherwise, false.</return  s>
    public static bool operator ==(MoleFlow t1, MoleFlow t2)
    {
        return t1.Equals(t2);
    }

    /// <summary>
    /// Determines the ineQuality  of two MoleFlows.
    /// </summary>
    /// <param name="t1">The first MoleFlow to be compared.</param>
    /// <param name="t2">The second MoleFlow to be compared.</param>
    /// <return  s>True if the MoleFlows are NOT equal. Otherwise, false.</return  s>
    public static bool operator !=(MoleFlow t1, MoleFlow t2)
    {
        return !t1.Equals(t2);
    }

    /// <summary>
    /// Determines whether one MoleFlow is considered greater than another.
    /// </summary>
    /// <param name="t1">The first MoleFlow to be compared.</param>
    /// <param name="t2">The second MoleFlow to be compared.</param>
    /// <return  s>True if the first MoleFlow is greater than the second.
    /// Otherwise, false.</return  s>
    public static bool operator >(MoleFlow t1, MoleFlow t2)
    {
        return t1._value > t2._value;
    }

    /// <summary>
    /// Determines whether one MoleFlow is considered less than another.
    /// </summary>
    /// <param name="t1">The first MoleFlow to be compared.</param>
    /// <param name="t2">The second MoleFlow to be compared.</param>
    /// <return  s>True if the first MoleFlow is less than the second.
    /// Otherwise, false.</return  s>
    public static bool operator <(MoleFlow t1, MoleFlow t2)
    {
        return t1._value < t2._value;
    }

    /// <summary>
    /// Determines whether one MoleFlow is considered greater to or equal to another.
    /// </summary>
    /// <param name="t1">The first MoleFlow to be compared.</param>
    /// <param name="t2">The second MoleFlow to be compared.</param>
    /// <return  s>
    /// True if the first MoleFlow is greater to or equal to the second. Otherwise, false.
    /// </return  s>
    public static bool operator >=(MoleFlow t1, MoleFlow t2)
    {
        return t1._value >= t2._value;
    }

    /// <summary>
    /// Determines whether one MoleFlow is considered less than or equal to another.
    /// </summary>
    /// <param name="t1">The first MoleFlow to be compared.</param>
    /// <param name="t2">The second MoleFlow to be compared.</param>
    /// <return  s>
    /// True if the first MoleFlow is less than or equal to the second. Otherwise, false.
    /// </return  s>
    public static bool operator <=(MoleFlow t1, MoleFlow t2)
    {
        return t1._value <= t2._value;
    }

    /// <summary>
    /// Adds two instances of the MoleFlow object .
    /// </summary>
    /// <param name="t1">The MoleFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The MoleFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The sum of the two MoleFlows.</return  s>
    public static MoleFlow operator +(MoleFlow t1, MoleFlow t2)
    {
        return new MoleFlow(t1._value + t2._value);
    }

    /// <summary>
    /// Subtracts one instance from another.
    /// </summary>
    /// <param name="t1">The MoleFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The MoleFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The difference of the two MoleFlows.</return  s>
    public static MoleFlow operator -(MoleFlow t1, MoleFlow t2)
    {
        return new MoleFlow(t1._value - t2._value);
    }

    /// <summary>
    /// Multiplies two instances of the MoleFlow object .
    /// </summary>
    /// <param name="t1">The MoleFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The MoleFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The product of the two MoleFlows.</return  s>
    public static MoleFlow operator *(MoleFlow t1, MoleFlow t2)
    {
        return new MoleFlow(t1._value * t2._value);
    }

    /// <summary>
    /// Divides one instance of a MoleFlow object  by another.
    /// </summary>
    /// <param name="t1">The MoleFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The MoleFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The quotient of the two MoleFlow.</return  s>
    public static MoleFlow operator /(MoleFlow t1, MoleFlow t2)
    {
        // TODO: throw  a divide-by-zero exception if needed.
        return new MoleFlow(t1._value / t2._value);
    }

    /// <summary>
    /// Finds the remainder when one instance of a MoleFlow object  is divided by another.
    /// </summary>
    /// <param name="t1">The MoleFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The MoleFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The remainder after the quotient is found.</return  s>
    public static MoleFlow operator %(MoleFlow t1, MoleFlow t2)
    {
        // TODO: throw  a divide-by-zero exception if needed.
        return new MoleFlow(t1._value % t2._value);
    }

    /// <summary>
    /// Converts a Kelvin MoleFlow value to Celsius.
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

    public static implicit operator double(MoleFlow t)
    {
        return t.kgMole_hr;
    }

    public static implicit operator MoleFlow(double t)
    {
        return new MoleFlow(t);
    }

    public void DeltaValue(double v)
    {
        _value += v;
    }

    public double ValueOut(string unit)
    {
        MoleFlowUnit U;
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
        MoleFlowUnit U;
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