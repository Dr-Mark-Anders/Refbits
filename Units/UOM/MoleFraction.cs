using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum MoleFractionUnit
    {
        _MassFrac,
        _MassPCT,  // Default
        _MassPPM,
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct MoleFraction : IFormattable, IComparable, IComparable<MoleFraction>, IEquatable<MoleFraction>, IUOM
    {
        private double _MassFrac;

        public MoleFraction(double val) : this()
        {
            _MassFrac = val;
        }

        public double Tolerance => 0.0001;

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out MassEnthalpyUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public MoleFraction(double Mass, MoleFractionUnit unit) : this()
        {
            switch (unit)
            {
                case MoleFractionUnit._MassFrac:
                    _MassFrac = Mass;
                    break;

                case MoleFractionUnit._MassPCT:
                    _MassFrac = Mass/100;
                    break;

                case MoleFractionUnit._MassPPM:
                    _MassFrac = Mass / 1e6;
                    break;

                default:
                    throw new ArgumentException("The MassEnthalpy unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Kelvin.
        /// </summary>
        public double MassFrac
        {
            get { return _MassFrac; }
            set { _MassFrac = value; }
        }


        public bool IsKnown { get { if (double.IsNaN(_MassFrac)) return false; else return true; } }

        public double BaseValue { get => _MassFrac; set => _MassFrac = value; }

        public string DefaultUnit => MassUnits.kg.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(MassUnits));
            }
        }

        public string Name => throw new NotImplementedException();

        public ePropID propid => throw new NotImplementedException();

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
        /// Gets the Temperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Temperature  should be retrieved.</param>
        /// <return  s>The Temperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(MoleFractionUnit unit)
        {
            switch (unit)
            {
                case MoleFractionUnit._MassFrac: return _MassFrac;
                case MoleFractionUnit._MassPPM: return _MassFrac*1e6;
                case MoleFractionUnit._MassPCT: return _MassFrac * 100;
                default:
                    throw new ArgumentException(
              "Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public void EraseValue()
        {
            _MassFrac = double.NaN;
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
                case "kJ_kg":
                    return _MassFrac.ToString("F2", provider) + " kJ_kg";

                case "J_g":
                    return _MassFrac.ToString("F2", provider) + " J_mol";

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
        public string ToString(MoleFractionUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case MoleFractionUnit._MassFrac:
                    return ToString("Fraction", provider);

                case MoleFractionUnit._MassPCT:
                    return ToString("%", provider);

                default:
                    throw new FormatException("The MoleFraction unit '" +
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
        public string ToString(MoleFractionUnit unit)
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
        public int CompareTo(MoleFraction value)
        {
            return _MassFrac.CompareTo(value.BaseValue);
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
            if (!(value is MassEnthalpy)) throw new ArgumentException();
            return CompareTo((MassEnthalpy)value);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(MoleFraction value)
        {
            return _MassFrac == value._MassFrac;
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
            if (!(value is MoleFraction)) return false;

            return Equals((MoleFraction)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _MassFrac.GetHashCode();
        }

        public void SetValue(MoleFraction p)
        {
            _MassFrac = p;
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(MoleFraction t1, MoleFraction t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(MoleFraction t1, MoleFraction t2)
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
        public static bool operator >(MoleFraction t1, MoleFraction t2)
        {
            return t1._MassFrac > t2._MassFrac;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(MoleFraction t1, MoleFraction t2)
        {
            return t1._MassFrac < t2._MassFrac;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(MoleFraction t1, MoleFraction t2)
        {
            return t1._MassFrac >= t2._MassFrac;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(MoleFraction t1, MoleFraction t2)
        {
            return t1._MassFrac <= t2._MassFrac;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static Mass operator +(MoleFraction t1, MoleFraction t2)
        {
            return new Mass(t1._MassFrac + t2._MassFrac);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static Mass operator -(MoleFraction t1, MoleFraction t2)
        {
            return new Mass(t1._MassFrac - t2._MassFrac);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static MoleFraction operator *(MoleFraction t1, MoleFraction t2)
        {
            return new MoleFraction(t1._MassFrac * t2._MassFrac);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static MoleFraction operator /(MoleFraction t1, MoleFraction t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MoleFraction(t1._MassFrac / t2._MassFrac);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static MoleFraction operator %(MoleFraction t1, MoleFraction t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MoleFraction(t1._MassFrac % t2._MassFrac);
        }

        /// <summary>
        /// Converts a Kelvin Temperature  value to Celsius.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double kg_to_g(double kJ_kg)
        {
            return kJ_kg;
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

        public static implicit operator double(MoleFraction t)
        {
            return t._MassFrac;
        }

        public static implicit operator MoleFraction(double t)
        {
            return new MoleFraction(t);
        }

        public void ClearpublicSetVariable()
        {
            _MassFrac = double.NaN;
        }

        public void DeltaValue(double v)
        {
            throw new NotImplementedException();
        }

        public double Pow(double x)
        {
            throw new NotImplementedException();
        }

        public double ValueOut(string unit)
        {
            return _MassFrac;
        }

        public void ValueIn(string unit, double v)
        {
            _MassFrac = v;
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