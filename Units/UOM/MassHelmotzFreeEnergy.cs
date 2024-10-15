using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum MassHelmotzUnit
    {
        kJ_kg,  // Default
        J_g,
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct MassHelmotz : IFormattable, IComparable, IComparable<MassHelmotz>, IEquatable<MassHelmotz>, IUOM
    {
        private double _kJ_kg;

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out MassHelmotzUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public MassHelmotz(double val) : this()
        {
            _kJ_kg = val;
        }

        public MassHelmotz(double MassHelmotz, MassHelmotzUnit unit) : this()
        {
            switch (unit)
            {
                case MassHelmotzUnit.kJ_kg:
                    _kJ_kg = MassHelmotz;
                    break;

                case MassHelmotzUnit.J_g:
                    _kJ_kg = MassHelmotz;
                    break;

                default:
                    throw new ArgumentException("The MassHelmotz unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Kelvin.
        /// </summary>
        public double kJ_kg
        {
            get { return _kJ_kg; }
            set { _kJ_kg = value; }
        }

        /// <summary>
        /// Gets or sets the MassHelmotz value.
        /// </summary>
        public double j_mole
        {
            get { return kJ_kgmol_to_J_mole(_kJ_kg); }
            set { _kJ_kg = J_g_to_kJ_kg(value); }
        }

        public bool IsKnown { get { if (double.IsNaN(_kJ_kg)) return false; else return true; } }

        public double Tolerance => throw new NotImplementedException();

        public ePropID propid => throw new NotImplementedException();

        public double BaseValue { get => _kJ_kg; set => _kJ_kg = value; }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(MassHelmotzUnit), MassHelmotzUnit.kJ_kg);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(MassHelmotzUnit));
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
        public double ValueIn(MassHelmotzUnit unit)
        {
            switch (unit)
            {
                case MassHelmotzUnit.kJ_kg: return _kJ_kg;
                case MassHelmotzUnit.J_g: return _kJ_kg;
                default:
                    throw new ArgumentException(
              "Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public void EraseValue()
        {
            _kJ_kg = double.NaN;
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
                    return _kJ_kg.ToString("F2", provider) + " kJ_kgmole";

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
        public string ToString(MassHelmotzUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case MassHelmotzUnit.kJ_kg:
                    return ToString("kJ_kgmole", provider);

                case MassHelmotzUnit.J_g:
                    return ToString("J_mole", provider);

                default:
                    throw new FormatException("The MassHelmotz unit '" +
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
        public string ToString(MassHelmotzUnit unit)
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
        public int CompareTo(MassHelmotz value)
        {
            return _kJ_kg.CompareTo(value._kJ_kg);
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
            if (!(value is MassHelmotz)) throw new ArgumentException();
            return CompareTo((MassHelmotz)value);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(MassHelmotz value)
        {
            return _kJ_kg == value._kJ_kg;
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
            if (!(value is MassHelmotz)) return false;

            return Equals((MassHelmotz)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _kJ_kg.GetHashCode();
        }

        public void SetValue(MassHelmotz p)
        {
            _kJ_kg = p;
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(MassHelmotz t1, MassHelmotz t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(MassHelmotz t1, MassHelmotz t2)
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
        public static bool operator >(MassHelmotz t1, MassHelmotz t2)
        {
            return t1._kJ_kg > t2._kJ_kg;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(MassHelmotz t1, MassHelmotz t2)
        {
            return t1._kJ_kg < t2._kJ_kg;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(MassHelmotz t1, MassHelmotz t2)
        {
            return t1._kJ_kg >= t2._kJ_kg;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(MassHelmotz t1, MassHelmotz t2)
        {
            return t1._kJ_kg <= t2._kJ_kg;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static MassHelmotz operator +(MassHelmotz t1, MassHelmotz t2)
        {
            return new MassHelmotz(t1._kJ_kg + t2._kJ_kg);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static MassHelmotz operator -(MassHelmotz t1, MassHelmotz t2)
        {
            return new MassHelmotz(t1._kJ_kg - t2._kJ_kg);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static MassHelmotz operator *(MassHelmotz t1, MassHelmotz t2)
        {
            return new MassHelmotz(t1._kJ_kg * t2._kJ_kg);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static MassHelmotz operator /(MassHelmotz t1, MassHelmotz t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MassHelmotz(t1._kJ_kg / t2._kJ_kg);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static MassHelmotz operator %(MassHelmotz t1, MassHelmotz t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MassHelmotz(t1._kJ_kg % t2._kJ_kg);
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
        public static double J_g_to_kJ_kg(double j_mol)
        {
            return j_mol;
        }

        public static implicit operator double(MassHelmotz t)
        {
            return t._kJ_kg;
        }

        public static implicit operator MassHelmotz(double t)
        {
            return new MassHelmotz(t);
        }

        public void ClearpublicSetVariable()
        {
            _kJ_kg = double.NaN;
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