using System;
using System.Globalization;
using Units;
using Units.UOM;

/// <summary>
/// Options for GasFlow measurement units.
/// </summary>
public enum GasFlowUnit
{
    NM3_hr,
    SM3_hr,
    MMSCFH,
    MSCFH,
    SCFD
}

/// <summary>
/// A GasFlow value.
/// </summary>
[Serializable]
public struct GasFlow : IFormattable, IComparable, IComparable<GasFlow>, IEquatable<GasFlow>, IUOM
{
    public double Nm3_hr = double.NaN;
    public double Tolerance => 0.0001;

    public const double kg_per_lb = 0.453592;

    public double Pow(double x)
    {
        return Math.Pow(Nm3_hr, x);
    }

    public string UnitDescriptor(string unit)
    {
        Enum.TryParse(unit, out GasFlowUnit res);
        return Enumhelpers.GetEnumDescription(res);
    }

    public ePropID propid => ePropID.MOLEF;

    public bool IsKnown
    {
        get
        {
            if (double.IsNaN(Nm3_hr))
                return false;
            return true;
        }
    }

    /// <summary>
    /// Creates a new  GasFlow with the specified value in Kelvin.
    /// </summary>
    /// <param name="value">The value of the GasFlow.</param>
    public GasFlow(double value) : this() { Nm3_hr = value; }

    /// <summary>
    /// Creates a new  GasFlow with the specified value in the
    /// specified unit of measurement.
    /// </summary>
    /// <param name="GasFlow">The value of the GasFlow.</param>
    /// <param name="unit">The unit of measurement that defines how
    /// the <paramref name="GasFlow"/> value is used.</param>
    public GasFlow(double GasFlow, GasFlowUnit unit) : this()
    {
        switch (unit)
        {
            case GasFlowUnit.MSCFH:
                Nm3_hr = GasFlow * 0.0269;
                break;
            case GasFlowUnit.MMSCFH:
                Nm3_hr = GasFlow * 0.0269 * 1000;
                break;
            case GasFlowUnit.NM3_hr:
                Nm3_hr = GasFlow;
                break;
            case GasFlowUnit.SM3_hr:
                Nm3_hr = GasFlow * 0.944665399;
                break;
            case GasFlowUnit.SCFD:
                Nm3_hr = GasFlow * 0.0269 /24/1000;
                break;
            default:
                throw new ArgumentException("The GasFlow unit '" + unit.ToString() + "' is unknown.");
        }
    }

    public MoleFlow MoleFlow(double MW)
    {
        return Nm3_hr / 22.414; // moles per Nm3
    }

    /// <summary>
    /// Gets or sets the GasFlow value in Kelvin.
    /// </summary>
    public double kgMole_hr
    {
        get { return Nm3_hr; }
        set { Nm3_hr = value; }
    }

    public double lbMole_hr
    {
        get { return Nm3_hr / kg_per_lb; }
        set { Nm3_hr = value * kg_per_lb; }
    }

    /// <summary>
    /// Gets or sets the GasFlow value in Celsius.
    /// </summary>
    public double kgMole_s
    {
        get { return kgMole_hr_to_kgMole_s(Nm3_hr); }
        set { Nm3_hr = kgMole_s_to_kgMole_hr(value); }
    }

    public void EraseValue()
    {
        Nm3_hr = double.NaN;
    }

    /// <summary>
    /// Gets the GasFlow value in the specified unit of measurement.
    /// </summary>
    /// <param name="unit">The unit of measurement
    /// in which the GasFlow should be retrieved.</param>
    /// <return  s>The GasFlow value in the specified
    /// <paramref name="unit"/>.</return  s>
    public double Valueout(GasFlowUnit unit)
    {
        switch (unit)
        {
            case GasFlowUnit.MMSCFH: return Nm3_hr;
            default:
                throw new ArgumentException("Unknown GasFlow unit '" + unit.ToString() + "'.");
        }
    }

    /// <summary>
    /// Gets the GasFlow value in the specified unit of measurement.
    /// </summary>
    /// <param name="unit">The unit of measurement
    /// in which the GasFlow should be retrieved.</param>
    /// <return  s>The GasFlow value in the specified
    /// <paramref name="unit"/>.</return  s>
    public double ValueIn(GasFlowUnit unit, double value)
    {
        switch (unit)
        {
            case GasFlowUnit.MMSCFH: Nm3_hr = value; return value;
            default:
                throw new ArgumentException("Unknown GasFlow unit '" + unit.ToString() + "'.");
        }
    }

    /// <summary>
    /// return  s a string  representation of the GasFlow value.
    /// </summary>
    /// <param name="format">
    /// A single format specifier that indicates how to format the value of this
    /// GasFlow. The format parameter can be "G",
    /// "C", "F", or "K". If format
    /// is null or the empty string  (""), "G" is used.
    /// </param>
    /// <param name="provider">
    /// An IFormatProvider reference that supplies culture-specific formatting
    /// services.
    /// </param>
    /// <return  s>A string  representation of the GasFlow.</return  s>
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
    /// return  s a string  representation of the GasFlow value.
    /// </summary>
    /// <param name="format">
    /// A single format specifier that indicates how to format the value of this
    /// GasFlow. The format parameter can be "G",
    /// "C", "F", or "K". If format
    /// is null or the empty string  (""), "G" is used.
    /// </param>
    /// <return  s>A string  representation of the GasFlow.</return  s>
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
    /// return  s a string  representation of the GasFlow value.
    /// </summary>
    /// <return  s>A string  representation of the GasFlow.</return  s>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    /// return  s a string  representation of the GasFlow value.
    /// </summary>
    /// <param name="unit">
    /// The GasFlow unit as which the GasFlow value should be displayed.
    /// </param>
    /// <param name="provider">
    /// An IFormatProvider reference that supplies culture-specific formatting
    /// services.
    /// </param>
    /// <return  s>A string  representation of the GasFlow.</return  s>
    public string ToString(GasFlowUnit unit, IFormatProvider provider)
    {
        switch (unit)
        {
            case GasFlowUnit.MMSCFH:
                return ToString("MSCFH", provider);

            default:
                throw new FormatException("The GasFlow unit '" + unit.ToString() + "' is unknown.");
        }
    }

    /// <summary>
    /// return  s a string  representation of the GasFlow value.
    /// </summary>
    /// <param name="unit">
    /// The GasFlow unit as which the GasFlow value should be displayed.
    /// </param>
    /// <return  s>A string  representation of the GasFlow.</return  s>
    public string ToString(GasFlowUnit unit)
    {
        return ToString(unit, null);
    }

    public double BaseValue
    {
        get
        {
            return Nm3_hr;
        }
        set
        {
            Nm3_hr = value;
        }
    }

    public string DefaultUnit
    {
        get
        {
            return Enum.GetName(typeof(GasFlowUnit), GasFlowUnit.MMSCFH);
        }
    }

    public string[] AllUnits
    {
        get
        {
            return Enum.GetNames(typeof(GasFlowUnit));
        }
    }

    public string Name => "Molar Flow";

    double IUOM.DisplayValue
    {
        get
        {
            return Valueout(GasFlowUnit.MMSCFH).RoundToSignificantDigits(3);
        }
        set
        {
            BaseValue = value;
        }
    }

    /// <summary>
    /// Compares this instance to a specified GasFlow object  and return  s an indication
    /// of their relative values.
    /// </summary>
    /// <param name="value">A GasFlow object
    ///    to compare to this instance.</param>
    /// <return  s>
    /// A signed number indicating the relative values of this instance and value.
    /// Value Description A negative int eger This instance is less than value. Zero
    /// This instance is equal to value. A positive int eger This instance is greater
    /// than value.
    /// </return  s>
    public int CompareTo(GasFlow value)
    {
        return Nm3_hr.CompareTo(value.Nm3_hr);
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
    /// The value is not a GasFlow.
    /// </exception>
    public int CompareTo(object value)
    {
        if (value == null) return 1;
        if (!(value is GasFlow)) throw new ArgumentException();
        return CompareTo((GasFlow)value);
    }

    /// <summary>
    /// Determines whether or not the given GasFlow is considered equal to this instance.
    /// </summary>
    /// <param name="value">The GasFlow to compare to this instance.</param>
    /// <return  s>True if the GasFlow is considered equal
    ///    to this instance. Otherwise, false.</return  s>
    public bool Equals(GasFlow value)
    {
        return Nm3_hr == value.Nm3_hr;
    }

    /// <summary>
    /// Determines whether or not the given object  is considered equal to the GasFlow.
    /// </summary>
    /// <param name="value">The object  to compare to the GasFlow.</param>
    /// <return  s>True if the object  is considered equal
    ///     to the GasFlow. Otherwise, false.</return  s>
    public override bool Equals(object value)
    {
        if (value == null) return false;
        if (!(value is GasFlow)) return false;

        return Equals((GasFlow)value);
    }

    /// <summary>
    /// return  s the hash code for this instance.
    /// </summary>
    /// <return  s>A 32-bit signed int eger hash code.</return  s>
    public override int GetHashCode()
    {
        return Nm3_hr.GetHashCode();
    }

    /// <summary>
    /// Determines the eQuality  of two GasFlows.
    /// </summary>
    /// <param name="t1">The first GasFlow to be compared.</param>
    /// <param name="t2">The second GasFlow to be compared.</param>
    /// <return  s>True if the GasFlows are equal. Otherwise, false.</return  s>
    public static bool operator ==(GasFlow t1, GasFlow t2)
    {
        return t1.Equals(t2);
    }

    /// <summary>
    /// Determines the ineQuality  of two GasFlows.
    /// </summary>
    /// <param name="t1">The first GasFlow to be compared.</param>
    /// <param name="t2">The second GasFlow to be compared.</param>
    /// <return  s>True if the GasFlows are NOT equal. Otherwise, false.</return  s>
    public static bool operator !=(GasFlow t1, GasFlow t2)
    {
        return !t1.Equals(t2);
    }

    /// <summary>
    /// Determines whether one GasFlow is considered greater than another.
    /// </summary>
    /// <param name="t1">The first GasFlow to be compared.</param>
    /// <param name="t2">The second GasFlow to be compared.</param>
    /// <return  s>True if the first GasFlow is greater than the second.
    /// Otherwise, false.</return  s>
    public static bool operator >(GasFlow t1, GasFlow t2)
    {
        return t1.Nm3_hr > t2.Nm3_hr;
    }

    /// <summary>
    /// Determines whether one GasFlow is considered less than another.
    /// </summary>
    /// <param name="t1">The first GasFlow to be compared.</param>
    /// <param name="t2">The second GasFlow to be compared.</param>
    /// <return  s>True if the first GasFlow is less than the second.
    /// Otherwise, false.</return  s>
    public static bool operator <(GasFlow t1, GasFlow t2)
    {
        return t1.Nm3_hr < t2.Nm3_hr;
    }

    /// <summary>
    /// Determines whether one GasFlow is considered greater to or equal to another.
    /// </summary>
    /// <param name="t1">The first GasFlow to be compared.</param>
    /// <param name="t2">The second GasFlow to be compared.</param>
    /// <return  s>
    /// True if the first GasFlow is greater to or equal to the second. Otherwise, false.
    /// </return  s>
    public static bool operator >=(GasFlow t1, GasFlow t2)
    {
        return t1.Nm3_hr >= t2.Nm3_hr;
    }

    /// <summary>
    /// Determines whether one GasFlow is considered less than or equal to another.
    /// </summary>
    /// <param name="t1">The first GasFlow to be compared.</param>
    /// <param name="t2">The second GasFlow to be compared.</param>
    /// <return  s>
    /// True if the first GasFlow is less than or equal to the second. Otherwise, false.
    /// </return  s>
    public static bool operator <=(GasFlow t1, GasFlow t2)
    {
        return t1.Nm3_hr <= t2.Nm3_hr;
    }

    /// <summary>
    /// Adds two instances of the GasFlow object .
    /// </summary>
    /// <param name="t1">The GasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The GasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The sum of the two GasFlows.</return  s>
    public static GasFlow operator +(GasFlow t1, GasFlow t2)
    {
        return new GasFlow(t1.Nm3_hr + t2.Nm3_hr);
    }

    /// <summary>
    /// Subtracts one instance from another.
    /// </summary>
    /// <param name="t1">The GasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The GasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The difference of the two GasFlows.</return  s>
    public static GasFlow operator -(GasFlow t1, GasFlow t2)
    {
        return new GasFlow(t1.Nm3_hr - t2.Nm3_hr);
    }

    /// <summary>
    /// Multiplies two instances of the GasFlow object .
    /// </summary>
    /// <param name="t1">The GasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The GasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The product of the two GasFlows.</return  s>
    public static GasFlow operator *(GasFlow t1, GasFlow t2)
    {
        return new GasFlow(t1.Nm3_hr * t2.Nm3_hr);
    }

    /// <summary>
    /// Divides one instance of a GasFlow object  by another.
    /// </summary>
    /// <param name="t1">The GasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The GasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The quotient of the two GasFlow.</return  s>
    public static GasFlow operator /(GasFlow t1, GasFlow t2)
    {
        // TODO: throw  a divide-by-zero exception if needed.
        return new GasFlow(t1.Nm3_hr / t2.Nm3_hr);
    }

    /// <summary>
    /// Finds the remainder when one instance of a GasFlow object  is divided by another.
    /// </summary>
    /// <param name="t1">The GasFlow on the left-hand side of the  operator  .
    /// </param>
    /// <param name="t2">The GasFlow on the right-hand side of the  operator  .
    /// </param>
    /// <return  s>The remainder after the quotient is found.</return  s>
    public static GasFlow operator %(GasFlow t1, GasFlow t2)
    {
        // TODO: throw  a divide-by-zero exception if needed.
        return new GasFlow(t1.Nm3_hr % t2.Nm3_hr);
    }

    /// <summary>
    /// Converts a Kelvin GasFlow value to Celsius.
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

    public static implicit operator double(GasFlow t)
    {
        return t.kgMole_hr;
    }

    public static implicit operator GasFlow(double t)
    {
        return new GasFlow(t);
    }

    public void DeltaValue(double v)
    {
        Nm3_hr += v;
    }

    public double ValueOut(string unit)
    {
        GasFlowUnit U;
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
        GasFlowUnit U;
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