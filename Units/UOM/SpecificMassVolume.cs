using System;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary>
    public enum SpecificMassVolumeUnit
    {
        m3_kg,
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct SpecificMassVolume : IFormattable, IComparable, IComparable<SpecificMolarVolume>, IEquatable<SpecificMolarVolume>, IUOM
    {
        public double _m3_kg;
        public double Tolerance => 0.0001;
        public ePropID propid => ePropID.Q;

        private bool _isKnown;
        public bool IsPrimary;

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out DensityUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public SpecificMassVolume(double Quality) : this()
        {
            _m3_kg = Quality;
        }

        public const double WaterDensity = 999.98;

        public SpecificMassVolume(double Quality, DensityUnit unit) : this()
        {
            switch (unit)
            {
                case DensityUnit.kg_m3:
                    _m3_kg = Quality;
                    break;

                default:
                    throw new ArgumentException("The specfic volume unit '" + unit.ToString() + "' is unknown.");
            }
        }

        public bool IsKnown { get { if (double.IsNaN(_m3_kg)) return false; else return true; } set => _isKnown = value; }

        public double BaseValue { get => _m3_kg; set => _m3_kg = value; }

        public string DefaultUnit
        {
            get
            {
                return Enum.GetName(typeof(SpecificMassVolumeUnit), SpecificMassVolumeUnit.m3_kg);
            }
        }

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(SpecificMassVolumeUnit));
            }
        }

        public string Name => "Vapour Fraction";

        double IUOM.DisplayValue { get => BaseValue.RoundToSignificantDigits(4); set => BaseValue = value; }

        /// <summary>
        /// Gets the Temperature  value in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement
        /// in which the Temperature  should be retrieved.</param>
        /// <return  s>The Temperature  value in the specified
        /// <paramref name="unit"/>.</return  s>
        public double ValueIn(SpecificMassVolumeUnit unit)
        {
            switch (unit)
            {
                case SpecificMassVolumeUnit.m3_kg: return _m3_kg;
                default:
                    throw new ArgumentException(
              "Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public void EraseValue()
        {
            _m3_kg = double.NaN;
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
                case "Pressure":
                    return _m3_kg.ToString("F2", provider) + " Quality ";

                default:
                    throw new FormatException(
                          string.Format("The {0} format string  is not supported.", format));
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
        public string ToString(DensityUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case DensityUnit.kg_m3:
                    return ToString("Quality Molar", provider);

                default:
                    throw new FormatException("The Quality  unit '" +
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
        public string ToString(DensityUnit unit)
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
        public int CompareTo(SpecificMolarVolume value)
        {
            return _m3_kg.CompareTo(value._m3_mole);
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
            if (!(value is Quality)) throw new ArgumentException();
            return CompareTo((Quality)value);
        }

        /// <summary>
        /// Determines whether or not the given Temperature  is considered equal to this instance.
        /// </summary>
        /// <param name="value">The Temperature  to compare to this instance.</param>
        /// <return  s>True if the Temperature  is considered equal
        ///    to this instance. Otherwise, false.</return  s>
        public bool Equals(SpecificMolarVolume value)
        {
            return _m3_kg == value._m3_mole;
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
            if (!(value is Quality)) return false;

            return Equals((Quality)value);
        }

        /// <summary>
        /// return  s the hash code for this instance.
        /// </summary>
        /// <return  s>A 32-bit signed int eger hash code.</return  s>
        public override int GetHashCode()
        {
            return _m3_kg.GetHashCode();
        }

        public void SetValue(Quality p)
        {
            _m3_kg = p;
        }

        /// <summary>
        /// Determines the eQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are equal. Otherwise, false.</return  s>
        public static bool operator ==(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Determines the ineQuality  of two Temperature s.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the Temperature s are NOT equal. Otherwise, false.</return  s>
        public static bool operator !=(SpecificMassVolume t1, SpecificMassVolume t2)
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
        public static bool operator >(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return t1._m3_kg > t2._m3_kg;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>True if the first Temperature  is less than the second.
        /// Otherwise, false.</return  s>
        public static bool operator <(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return t1._m3_kg < t2._m3_kg;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered greater to or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is greater to or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator >=(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return t1._m3_kg >= t2._m3_kg;
        }

        /// <summary>
        /// Determines whether one Temperature  is considered less than or equal to another.
        /// </summary>
        /// <param name="t1">The first Temperature  to be compared.</param>
        /// <param name="t2">The second Temperature  to be compared.</param>
        /// <return  s>
        /// True if the first Temperature  is less than or equal to the second. Otherwise, false.
        /// </return  s>
        public static bool operator <=(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return t1._m3_kg <= t2._m3_kg;
        }

        /// <summary>
        /// Adds two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The sum of the two Temperature s.</return  s>
        public static Quality operator +(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return new Quality(t1._m3_kg + t2._m3_kg);
        }

        /// <summary>
        /// Subtracts one instance from another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The difference of the two Temperature s.</return  s>
        public static Quality operator -(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return new Quality(t1._m3_kg - t2._m3_kg);
        }

        /// <summary>
        /// Multiplies two instances of the Temperature  object .
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The product of the two Temperature s.</return  s>
        public static Quality operator *(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            return new Quality(t1._m3_kg * t2._m3_kg);
        }

        /// <summary>
        /// Divides one instance of a Temperature  object  by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The quotient of the two Temperature .</return  s>
        public static Quality operator /(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Quality(t1._m3_kg / t2._m3_kg);
        }

        /// <summary>
        /// Finds the remainder when one instance of a Temperature  object  is divided by another.
        /// </summary>
        /// <param name="t1">The Temperature  on the left-hand side of the  operator  .
        /// </param>
        /// <param name="t2">The Temperature  on the right-hand side of the  operator  .
        /// </param>
        /// <return  s>The remainder after the quotient is found.</return  s>
        public static Quality operator %(SpecificMassVolume t1, SpecificMassVolume t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Quality(t1._m3_kg % t2._m3_kg);
        }

        public static implicit operator double(SpecificMassVolume t)
        {
            return t._m3_kg;
        }

        public static implicit operator SpecificMassVolume(double t)
        {
            return new SpecificMassVolume(t);
        }

        public void ClearpublicSetVariable()
        {
            _m3_kg = double.NaN;
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
            return _m3_kg;
        }

        public void ValueIn(string unit, double v)
        {
            _m3_kg   = v;
        }
    }
}