using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum EntropyUnit
    {
        J_mole_K, // Default to make calcs easier
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct Entropy : IFormattable, IComparable, IComparable<Entropy>, IEquatable<Entropy>, IUOM
    {
        private double _basevalue;

        public double Tolerance => 0.0001;
        public ePropID propid => ePropID.S;

        public string UnitDescriptor(string unit)
        {
            if (Enum.TryParse(unit, out EntropyUnit res))
                return Enumhelpers.GetEnumDescription(res);
            return null;
        }

        public Entropy(double Entropy) : this()
        {
            _basevalue = Entropy;
        }

        public Entropy(double Entropy, EntropyUnit unit) : this()
        {
            BaseValue = unit switch
            {
                EntropyUnit.J_mole_K => Entropy,
                _ => throw new ArgumentException("The Entropy unit '" + unit.ToString() + "' is unknown."),
            };
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Celsius.
        /// </summary>
        public double KJ_kgmole_K
        {
            get { return _basevalue; }
            set { _basevalue = value; }
        }

        public double J_mole_K
        {
            get { return _basevalue; }
            set { _basevalue = value; }
        }

        public bool IsKnown { get { if (double.IsNaN(_basevalue)) return false; else return true; } }

        public double BaseValue { get => _basevalue; set => _basevalue = value; }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(EntropyUnit), EntropyUnit.J_mole_K);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(EntropyUnit));
            }
        }

        public string Name { get => "Molar Entropy"; }

        double IUOM.DisplayValue
        {
            get
            {
                return BaseValue; // J_Mole
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
        public double Valueout(EntropyUnit unit)
        {
            return unit switch
            {
                EntropyUnit.J_mole_K => _basevalue,
                _ => throw new ArgumentException("Unknown entropy unit '" + unit.ToString() + "'."),
            };
        }

        public void EraseValue()
        {
            _basevalue = double.NaN;
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

            return format.ToUpperInvariant() switch
            {
                "G" or "J_K" => _basevalue.ToString("F2", provider) + " J_K",
                "KJ_K" => KJ_kgmole_K.ToString("F2", provider) + " KJ_K",
                _ => throw new FormatException(string.Format("The {0} format string  is not supported.", format)),
            };
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
        public string ToString(EntropyUnit unit, IFormatProvider provider)
        {
            return unit switch
            {
                EntropyUnit.J_mole_K => ToString("J_K", provider),
                _ => throw new FormatException("The Entropy unit '" + unit.ToString() + "' is unknown."),
            };
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="unit">
        /// The Temperature  unit as which the Temperature  value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
        public string ToString(EntropyUnit unit)
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
        public int CompareTo(Entropy value)
        {
            return _basevalue.CompareTo(value._basevalue);
        }

        /// <summary>
        /// Compares this instance to a specified object  and return  s an indication of
        /// their relative values.
        /// </summary>
        /// <param name="obj">An object  to compare, or null.</param>
        /// <return  s>
        /// A signed number indicating the relative values of this instance and value.
        /// Value Description A negative int eger This instance is less than value. Zero
        /// This instance is equal to value. A positive int eger This instance is greater
        /// than value, or value is null.
        /// </return  s>
        /// <exception cref="ArgumentException">
        /// The value is not a Temperature .
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is not Entropy) throw new();
            return CompareTo((Entropy)obj);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(Entropy value)
        {
            return _basevalue == value._basevalue;
        }

        /// <summary>
        /// Determines whether or not the given object  is considered equal to the Temperature .
        /// </summary>
        /// <param name="obj">The object  to compare to the Temperature .</param>
        /// <return  s>True if the object  is considered equal
        ///     to the Temperature . Otherwise, false.</return  s>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not Entropy)
                return false;

            return Equals((Entropy)obj);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(Entropy t1, Entropy t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(Entropy t1, Entropy t2)
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
        public static bool operator >(Entropy t1, Entropy t2)
        {
            return t1._basevalue > t2._basevalue;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(Entropy t1, Entropy t2)
        {
            return t1._basevalue < t2._basevalue;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(Entropy t1, Entropy t2)
        {
            return t1._basevalue >= t2._basevalue;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(Entropy t1, Entropy t2)
        {
            return t1._basevalue <= t2._basevalue;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static Entropy operator +(Entropy t1, Entropy t2)
        {
            return new Entropy(t1._basevalue + t2._basevalue);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static Entropy operator -(Entropy t1, Entropy t2)
        {
            return new Entropy(t1._basevalue - t2._basevalue);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static Entropy operator *(Entropy t1, Entropy t2)
        {
            return new Entropy(t1._basevalue * t2._basevalue);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static Entropy operator /(Entropy t1, Entropy t2)
        {
            return new Entropy(t1._basevalue / t2._basevalue);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Entropy operator %(Entropy t1, Entropy t2)
        {
            return new Entropy(t1._basevalue % t2._basevalue);
        }

        /// <summary>
        /// Converts a Kelvin Temperature  value to Celsius.
        /// </summary>
        /// <param name="kelvin">The Kelvin value to convert to Celsius.
        /// </param>
        /// <return  s>The Kelvin value in Celsius.</return  s>
        public static double J_K_to_kJ_K(double J_K)
        {
            return J_K / 1000;
        }

        /// <summary>
        /// Converts a Celsius value to Kelvin.
        /// </summary>
        /// <param name="KJ_K">The Celsius value to convert to Kelvin.
        /// </param>
        /// <return  s>The Celsius value in Kelvin.</return  s>
        public static double KJ_K_to_J_K(double KJ_K)
        {
            return KJ_K * 1000;
        }

        public static implicit operator double(Entropy t)
        {
            return t._basevalue;
        }

        public static implicit operator Entropy(double t)
        {
            return new Entropy(t);
        }

        public void ClearpublicSetVariable()
        {
            _basevalue = double.NaN;
        }

        public void DeltaValue(double v)
        {
            _basevalue += v;
        }

        public double ValueOut(string unit)
        {
            if (Enum.TryParse(unit, out EntropyUnit U))
            {
                return Valueout(U);
            }
            else
            {
                return double.NaN;
            }
        }

        public void ValueIn(EntropyUnit unit, double value)
        {
            _basevalue = unit switch
            {
                EntropyUnit.J_mole_K => value,
                _ => throw new ArgumentException("Unknown Temperature  unit '" + unit.ToString() + "'."),
            };
        }

        public void ValueIn(string unit, double v)
        {
            if (Enum.TryParse(unit, out EntropyUnit U))
            {
                ValueIn(U, v);
            }
            else
            {
                throw new ArgumentException("Unknown Molar Entropy unit '" + unit.ToString() + "'.");
            }
        }
    }
}