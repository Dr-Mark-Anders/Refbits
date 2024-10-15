using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum MassEntropyUnit
    {
        J_K,  // Default
        kJ_K
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct MassEntropy : IFormattable, IComparable, IComparable<MassEntropy>, IEquatable<MassEntropy>, IUOM
    {
        private double _J_K;

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out MassEntropyUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public MassEntropy(double Pressure) : this()
        {
            _J_K = Pressure;
        }

        public double Tolerance => 0.0001;

        public MassEntropy(double MassEntropy, MassEntropyUnit unit) : this()
        {
            switch (unit)
            {
                case MassEntropyUnit.J_K:
                    _J_K = MassEntropy;
                    break;

                case MassEntropyUnit.kJ_K:
                    kJ_K = MassEntropy;
                    break;

                default:
                    throw new ArgumentException("The MassEntropy unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Kelvin.
        /// </summary>
        public double J_K
        {
            get { return _J_K; }
            set { _J_K = value; }
        }

        /// <summary>
        /// Gets or sets the Temperature  value in Celsius.
        /// </summary>
        public double kJ_K
        {
            get { return J_K_to_kJ_K(_J_K); }
            set { _J_K = KJ_K_to_J_K(value); }
        }

        public bool IsKnown { get { if (double.IsNaN(_J_K)) return false; else return true; } }

        public double BaseValue { get => _J_K; set => _J_K = value; }

        public string DefaultUnit => throw new NotImplementedException();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(MassEntropyUnit));
            }
        }

        public string Name { get => "Mass Entropy"; }

        public ePropID propid => ePropID.MassEntropy;

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
        public double ValueIn(MassEntropyUnit unit)
        {
            switch (unit)
            {
                case MassEntropyUnit.J_K: return _J_K;
                default:
                    throw new ArgumentException(
              "Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public void EraseValue()
        {
            _J_K = double.NaN;
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
                case "J_K":
                    return _J_K.ToString("F2", provider) + " J_K";

                case "KJ_K":
                    return kJ_K.ToString("F2", provider) + " KJ_K";

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
        public string ToString(MassEntropyUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case MassEntropyUnit.J_K:
                    return ToString("J_K", provider);

                case MassEntropyUnit.kJ_K:
                    return ToString("kJ_K", provider);

                default:
                    throw new FormatException("The MassEntropy unit '" + unit.ToString() + "' is unknown.");
            }
        }

        /// <summary>
        /// return  s a string  representation of the Temperature  value.
        /// </summary>
        /// <param name="unit">
        /// The Temperature  unit as which the Temperature  value should be displayed.
        /// </param>
        /// <return  s>A string  representation of the Temperature .</return  s>
        public string ToString(MassEntropyUnit unit)
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
        public int CompareTo(MassEntropy value)
        {
            return _J_K.CompareTo(value._J_K);
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
            if (!(value is MassEntropy)) throw new ArgumentException();
            return CompareTo((MassEntropy)value);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(MassEntropy value)
        {
            return _J_K == value._J_K;
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
            if (!(value is MassEntropy)) return false;

            return Equals((MassEntropy)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _J_K.GetHashCode();
        }

        public void SetValue(MassEntropy p)
        {
            _J_K = p;
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(MassEntropy t1, MassEntropy t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(MassEntropy t1, MassEntropy t2)
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
        public static bool operator >(MassEntropy t1, MassEntropy t2)
        {
            return t1._J_K > t2._J_K;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(MassEntropy t1, MassEntropy t2)
        {
            return t1._J_K < t2._J_K;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(MassEntropy t1, MassEntropy t2)
        {
            return t1._J_K >= t2._J_K;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(MassEntropy t1, MassEntropy t2)
        {
            return t1._J_K <= t2._J_K;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static MassEntropy operator +(MassEntropy t1, MassEntropy t2)
        {
            return new MassEntropy(t1._J_K + t2._J_K);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static MassEntropy operator -(MassEntropy t1, MassEntropy t2)
        {
            return new MassEntropy(t1._J_K - t2._J_K);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static MassEntropy operator *(MassEntropy t1, MassEntropy t2)
        {
            return new MassEntropy(t1._J_K * t2._J_K);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static MassEntropy operator /(MassEntropy t1, MassEntropy t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MassEntropy(t1._J_K / t2._J_K);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static MassEntropy operator %(MassEntropy t1, MassEntropy t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new MassEntropy(t1._J_K % t2._J_K);
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

        public static implicit operator double(MassEntropy t)
        {
            return t._J_K;
        }

        public static implicit operator MassEntropy(double t)
        {
            return new MassEntropy(t);
        }

        public void ClearpublicSetVariable()
        {
            _J_K = double.NaN;
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
            throw new NotImplementedException();
        }

        public void ValueIn(string unit, double v)
        {
            throw new NotImplementedException();
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