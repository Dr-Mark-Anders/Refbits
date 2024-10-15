using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum HelmotzUnit
    {
        kJ_kgmole,  // Default
        J_mole,
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct Helmotz : IFormattable, IComparable, IComparable<Helmotz>, IEquatable<Helmotz>, IUOM
    {
        private double _kJ_kgmole;

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out HelmotzUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public Helmotz(double val) : this()
        {
            _kJ_kgmole = val;
        }

        public Helmotz(double Helmotz, HelmotzUnit unit) : this()
        {
            switch (unit)
            {
                case HelmotzUnit.kJ_kgmole:
                    _kJ_kgmole = Helmotz;
                    break;

                case HelmotzUnit.J_mole:
                    _kJ_kgmole = Helmotz;
                    break;

                default:
                    throw new ArgumentException("The Helmotz unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Kelvin.
        /// </summary>
        public double kJ_kgmole
        {
            get { return _kJ_kgmole; }
            set { _kJ_kgmole = value; }
        }

        /// <summary>
        /// Gets or sets the Helmotz value.
        /// </summary>
        public double j_mole
        {
            get { return kJ_kgmol_to_J_mole(_kJ_kgmole); }
            set { _kJ_kgmole = J_mol_to_kJ_kgmole(value); }
        }

        public bool IsKnown { get { if (double.IsNaN(_kJ_kgmole)) return false; else return true; } }

        public double Tolerance => throw new NotImplementedException();

        public ePropID propid => throw new NotImplementedException();

        public double BaseValue { get => _kJ_kgmole; set => _kJ_kgmole = value; }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(HelmotzUnit), HelmotzUnit.kJ_kgmole);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(HelmotzUnit));
            }
        }

        public string Name => throw new NotImplementedException();

        public double DisplayValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the Temperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Temperature  should be retrieved.</param>
        /// <return  s>The Temperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(HelmotzUnit unit)
        {
            switch (unit)
            {
                case HelmotzUnit.kJ_kgmole: return _kJ_kgmole;
                case HelmotzUnit.J_mole: return j_mole;
                default:
                    throw new ArgumentException(
              "Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public void EraseValue()
        {
            _kJ_kgmole = double.NaN;
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
            if (string.IsNullOrEmpty(format)) format = "G";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "kJ_kgmole":
                    return _kJ_kgmole.ToString("F2", provider) + " kJ_kgmole";

                case "J_mole":
                    return j_mole.ToString("F2", provider) + " J_mol";

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
        public string ToString(HelmotzUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case HelmotzUnit.kJ_kgmole:
                    return ToString("kJ_kgmole", provider);

                case HelmotzUnit.J_mole:
                    return ToString("J_mole", provider);

                default:
                    throw new FormatException("The Helmotz unit '" +
                          unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="unit">
        /// The Temperature  unit as which the Temperature  value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
        public string ToString(HelmotzUnit unit)
        {
            return ToString(unit, null);
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
        public int CompareTo(Helmotz value)
        {
            return _kJ_kgmole.CompareTo(value._kJ_kgmole);
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
            if (!(value is Helmotz)) throw new ArgumentException();
            return CompareTo((Helmotz)value);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(Helmotz value)
        {
            return _kJ_kgmole == value._kJ_kgmole;
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
            if (!(value is Helmotz)) return false;

            return Equals((Helmotz)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _kJ_kgmole.GetHashCode();
        }

        public void SetValue(Helmotz p)
        {
            _kJ_kgmole = p;
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(Helmotz t1, Helmotz t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(Helmotz t1, Helmotz t2)
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
        public static bool operator >(Helmotz t1, Helmotz t2)
        {
            return t1._kJ_kgmole > t2._kJ_kgmole;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(Helmotz t1, Helmotz t2)
        {
            return t1._kJ_kgmole < t2._kJ_kgmole;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(Helmotz t1, Helmotz t2)
        {
            return t1._kJ_kgmole >= t2._kJ_kgmole;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(Helmotz t1, Helmotz t2)
        {
            return t1._kJ_kgmole <= t2._kJ_kgmole;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static Helmotz operator +(Helmotz t1, Helmotz t2)
        {
            return new Helmotz(t1._kJ_kgmole + t2._kJ_kgmole);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static Helmotz operator -(Helmotz t1, Helmotz t2)
        {
            return new Helmotz(t1._kJ_kgmole - t2._kJ_kgmole);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static Helmotz operator *(Helmotz t1, Helmotz t2)
        {
            return new Helmotz(t1._kJ_kgmole * t2._kJ_kgmole);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static Helmotz operator /(Helmotz t1, Helmotz t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Helmotz(t1._kJ_kgmole / t2._kJ_kgmole);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Helmotz operator %(Helmotz t1, Helmotz t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Helmotz(t1._kJ_kgmole % t2._kJ_kgmole);
        }

        /// <summary>
        /// Converts a Kelvin Temperature  value to Celsius.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double kJ_kgmol_to_J_mole(double kJ_kgmole)
        {
            return kJ_kgmole;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="MPa">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double J_mol_to_kJ_kgmole(double j_mol)
        {
            return j_mol;
        }

        public static implicit operator double(Helmotz t)
        {
            return t._kJ_kgmole;
        }

        public static implicit operator Helmotz(double t)
        {
            return new Helmotz(t);
        }

        public void ClearpublicSetVariable()
        {
            _kJ_kgmole = double.NaN;
        }

        public void DeltaValue(double v)
        {
            throw new NotImplementedException();
        }

        public double ValueOut(string unit)
        {
            throw new NotImplementedException();
        }

        public void ValueIn(string unit, double v)
        {
            throw new NotImplementedException();
        }
    }
}