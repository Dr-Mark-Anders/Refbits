using System;
using System.ComponentModel;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary> //{"kg/cm2", "kg/cm2 g", "psia", "psig", "atm a", "atm g", "kPa", "kPa g", "bar a", "bar g", "mmHg", "mmHg g"};;
    public enum DeltaPressureUnit
    {
        [Description("Bar")]
        BarA,

        [Description("MPa")]
        MPa,

        [Description("kPa")]
        KPa,

        [Description("mmHg")]
        MMHga,

        [Description("psi")]
        PSI
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct DeltaPressure : IFormattable, IComparable, IComparable<DeltaPressure>, IEquatable<DeltaPressure>, IUOM
    {
        private double _Bar;

        public DeltaPressure(double Pressure) : this()
        {
            _Bar = Pressure;
        }

        public DeltaPressure(double Pressure, DeltaPressureUnit unit) : this()
        {
            switch (unit)
            {
                case DeltaPressureUnit.BarA:
                    _Bar = Pressure;
                    break;

                case DeltaPressureUnit.MPa:
                    MPa = Pressure;
                    break;

                case DeltaPressureUnit.KPa:
                    kPa = Pressure;
                    break;

                case DeltaPressureUnit.PSI:

                default:
                    throw new ArgumentException("The Pressure  unit '" + unit.ToString() + "' is unknown.");
            }
        }

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out DeltaPressureUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public double BarA
        {
            get { return _Bar; }
            set { _Bar = value; }
        }

        public double MPa
        {
            get { return BarAToMPA(_Bar); }
            set { _Bar = MPatoBarA(value); }
        }

        public double MMHGA
        {
            get { return BarAToMMG(_Bar); }
            set { _Bar = MMHGAtoBARA(value); }
        }

        private double MMHGAtoBARA(double MMHGA)
        {
            return MMHGA * 0.00133322;
        }

        private double BarAToMMG(double barA)
        {
            return barA / 0.00133322;
        }

        public double kg_cm2
        {
            get { return _Bar / 0.980665; }

            set { _Bar = value * 0.980665; }
        }

        public double PSI
        {
            get { return BarAtoPSIA(_Bar); }
            set { _Bar = psiatoBarA(value); }
        }

        public static double psiatoBarA(double psia)
        {
            return psia / 14.5038;
        }

        public static double BarAtoPSIA(double psia)
        {
            return psia * 14.5038;
        }

        public double kPa
        {
            get { return BarAtokPa(_Bar); }
            set { _Bar = kPatoBarA(value); }
        }

        public bool IsKnown { get { if (double.IsNaN(_Bar)) return false; else return true; } }

        public double BaseValue { get => _Bar; set => _Bar = value; }

        public string DefaultUnit => PressureUnit.BarA.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(PressureUnit));
            }
        }

        public string Name { get => "Pressure"; }
        public ePropID propid => ePropID.P;

        public double Tolerance => 0.0001;

        public double DisplayValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void EraseValue()
        {
            _Bar = double.NaN;
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "BarA":
                    return _Bar.ToString("F2", provider) + " Pressure ";

                case "MPa":
                    return MPa.ToString("F2", provider) + " MPa";

                case "kPa":
                    return kPa.ToString("F2", provider) + " kPa";

                default:
                    throw new FormatException(
                          string.Format("The {0} format string  is not supported.", format));
            }
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(PressureUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case PressureUnit.BarA:
                    return ToString("bar A", provider);

                case PressureUnit.MPa:
                    return ToString("MPa", provider);

                case PressureUnit.KPa:
                    return ToString("kPa", provider);

                case PressureUnit.MMHga:
                    return ToString("mmHga", provider);

                default:
                    throw new FormatException("The Pressure  unit '" +
                          unit.ToString() + "' is unknown.");
            }
        }

        public string ToString(PressureUnit unit)
        {
            return ToString(unit, null);
        }

        public int CompareTo(DeltaPressure value)
        {
            return _Bar.CompareTo(value._Bar);
        }

        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is Pressure)) throw new ArgumentException();
            return CompareTo((Pressure)value);
        }

        public bool Equals(DeltaPressure value)
        {
            return _Bar == value._Bar;
        }

        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is DeltaPressure)) return false;

            return Equals((DeltaPressure)value);
        }

        public override int GetHashCode()
        {
            return _Bar.GetHashCode();
        }

        public void SetValue(Pressure p)
        {
            _Bar = p.BaseValue;
        }

        public static bool operator ==(DeltaPressure t1, DeltaPressure t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(DeltaPressure t1, DeltaPressure t2)
        {
            return !t1.Equals(t2);
        }

        public static bool operator >(DeltaPressure t1, DeltaPressure t2)
        {
            return t1._Bar > t2._Bar;
        }

        public static bool operator <(DeltaPressure t1, DeltaPressure t2)
        {
            return t1._Bar < t2._Bar;
        }

        public static bool operator >=(DeltaPressure t1, DeltaPressure t2)
        {
            return t1._Bar >= t2._Bar;
        }

        public static bool operator <=(DeltaPressure t1, DeltaPressure t2)
        {
            return t1._Bar <= t2._Bar;
        }

        public static Pressure operator +(DeltaPressure t1, DeltaPressure t2)
        {
            return new Pressure(t1._Bar + t2._Bar);
        }

        public static Pressure operator -(DeltaPressure t1, DeltaPressure t2)
        {
            return new Pressure(t1._Bar - t2._Bar);
        }

        public static Pressure operator *(DeltaPressure t1, DeltaPressure t2)
        {
            return new Pressure(t1._Bar * t2._Bar);
        }

        public static Pressure operator /(DeltaPressure t1, DeltaPressure t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Pressure(t1._Bar / t2._Bar);
        }

        public static Pressure operator %(DeltaPressure t1, DeltaPressure t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Pressure(t1._Bar % t2._Bar);
        }

        public static double BarAToMPA(double Pressure)
        {
            return Pressure / 10;
        }

        public static double MPatoBarA(double MPa)
        {
            return MPa * 10;
        }

        public static double BarAtokPa(double Pressure)
        {
            return Pressure * 100;
        }

        public static double kPatoBarA(double kpa)
        {
            return kpa / 100;
        }

        public static implicit operator double(DeltaPressure t)
        {
            return t._Bar;
        }

        public static implicit operator DeltaPressure(double t)
        {
            return new DeltaPressure(t);
        }

        public void ClearpublicSetVariable()
        {
            _Bar = double.NaN;
        }

        public void DeltaValue(double v)
        {
            _Bar += v;
        }

        public double Valueout(DeltaPressureUnit unit)
        {
            switch (unit)
            {
                case DeltaPressureUnit.BarA: return _Bar;
                case DeltaPressureUnit.KPa: return kPa;
                case DeltaPressureUnit.MPa: return MPa;
                case DeltaPressureUnit.MMHga: return MMHGA;
                default:
                    throw new ArgumentException("Unknown Pressure  unit '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(DeltaPressureUnit unit, double value)
        {
            switch (unit)
            {
                case DeltaPressureUnit.BarA: _Bar = value; break;
                case DeltaPressureUnit.KPa: kPa = value; break;
                case DeltaPressureUnit.MPa: MPa = value; break;
                case DeltaPressureUnit.MMHga: MMHGA = value; break;
                default:
                    throw new ArgumentException("Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            DeltaPressureUnit U;
            if (Enum.TryParse(unit, out U))
            {
                return Valueout(U);
            }
            else
            {
                return double.NaN;
            }
        }

        public void ValueIn(string unit, double value)
        {
            DeltaPressureUnit U;
            if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, value);
            }
            else
            {
                BaseValue = value;
            }
        }

        public double Pow(double x)
        {
            return double.NaN;
        }
    }
}