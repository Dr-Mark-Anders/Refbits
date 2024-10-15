using System;
using System.ComponentModel;
using System.Globalization;

namespace Units.UOM
{
    /// <summary>
    /// Options for Temperature  measurement units.
    /// </summary> //{"kg/cm2", "kg/cm2 g", "psia", "psig", "atm a", "atm g", "kPa", "kPa g", "bar a", "bar g", "mmHg", "mmHg g"};;
    public enum PressureUnit
    {
        [Description("Bar a")]
        BarA,

        [Description("MPa a")]
        MPa,

        [Description("kPa a")]
        KPa,

        [Description("mmHg a")]
        MMHga,

        [Description("psi a")]
        PSIA,

        [Description("Bar g")]
        BarG,

        [Description("MPa g")]
        MPaG,

        [Description("kPa g")]
        KPaG,

        [Description("mmHg g")]
        MMHgG,

        [Description("psi g")]
        PSIG,

        [Description("kg/cm2 g")]
        Kg_cm2_g,

        [Description("kg/cm2")]
        Kg_cm2,

        [Description("atmg")]
        atmg,

        [Description("atma")]
        atma
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct Pressure : IFormattable, IComparable, IComparable<Pressure>, IEquatable<Pressure>, IUOM
    {
        public const double One_Atm_in_bar = 1.01325; //1.01325 bar
        public const double Atm_bar = 1.01325; //1.01325 bar
        public const double bar_Atm = 0.986923;
        public const double kgcm2_kpa = 98.0665;

        public const double kgcm2_bar = 0.980665;
        public const double bar_kgcm2 = 1.01972;

        public const double bar_kpa = 100;
        public const double bar_psi = 14.5038;

        public double _BarA;
        public double Tolerance => 0.0001;

        public Pressure(double Pressure) : this()
        {
            _BarA = Pressure;
        }

        public string UnitDescriptor(string unit)
        {
            if (Enum.TryParse(unit, out PressureUnit res))
                return Enumhelpers.GetEnumDescription(res);
            else
                return "";
        }

        public Pressure(double Pressure, PressureUnit unit) : this()
        {
            switch (unit)
            {
                case PressureUnit.BarA:
                    _BarA = Pressure;
                    break;

                case PressureUnit.MPa:
                    _BarA = Pressure * 10;
                    break;

                case PressureUnit.KPa:
                    _BarA = Pressure / 100;
                    break;

                case PressureUnit.PSIA:
                    _BarA = psiatoBarA(Pressure);
                    break;

                case PressureUnit.BarG:
                    _BarA = BarGtoBarA(Pressure);
                    break;

                case PressureUnit.MPaG:
                    _BarA = BarGtoBarA(MPatoBar(Pressure));
                    break;

                case PressureUnit.KPaG:
                    _BarA = BarGtoBarA(kPatoBarA(Pressure));
                    break;

                case PressureUnit.PSIG:
                    _BarA = BarGtoBarA(psiatoBarA(Pressure));
                    break;

                case PressureUnit.Kg_cm2_g:
                    BARG = kgcm2_to_bar(Pressure);
                    break;

                case PressureUnit.Kg_cm2:
                    _BarA = kgcm2_to_bar(Pressure);
                    break;

                case PressureUnit.atmg:
                    BARG = ATMGtoBarG(Pressure);
                    break;

                case PressureUnit.atma:
                    _BarA = ATMGtoBarG(Pressure);
                    break;

                default:
                    throw new ArgumentException("The Pressure  unit '" + unit.ToString() + "' is unknown.");
            }
        }

        public double BarA
        {
            get { return _BarA; }
            set { _BarA = value; }
        }

        public double MPa
        {
            get { return BarToMPA(_BarA); }
            set { _BarA = MPatoBar(value); }
        }

        public double MPaG
        {
            get { return BarAToBarG(BarToMPA(_BarA)); }
            set { _BarA = BarGToBarA(MPatoBar(value)); }
        }

        public double MMHGA
        {
            get { return BarToMMG(_BarA); }
            set { _BarA = MMHGtoBAR(value); }
        }

        public double MMHGG
        {
            get { return BarToMMG(BarAToBarG(_BarA)); }
            set { _BarA = BarAToBarG(MMHGtoBAR(value)); }
        }

        public double ATM
        {
            get { return BarAtoATMA(_BarA); }
            set { _BarA = ATMAtoBarA(value); }
        }

        private static double MMHGtoBAR(double MMHGA)
        {
            return MMHGA * 0.00133322;
        }

        private static double BarToMMG(double barA)
        {
            return barA / 0.00133322;
        }

        public double kPa
        {
            get { return BarAtokPa(_BarA); }
            set { _BarA = kPatoBarA(value); }
        }

        public double PSIA
        {
            get { return BarAtoPSIA(_BarA); }
            set { _BarA = psiatoBarA(value); }
        }

        public double ATMA
        {
            get { return BarAtoATMA(_BarA); }
            set { _BarA = ATMGtoBarG(value); }
        }

        public double ATMG
        {
            get { return BarGtoAtmG(BarAToBarG(_BarA)); }
            set { BARG = ATMGtoBarG(value); }
        }

        public double BARG
        {
            get { return BarAToBarG(_BarA); }
            set { _BarA = BarGtoBarA(value); }
        }

        public double kg_cm2_g
        {
            get { return bar_to_kgcm2(BARG); }
            set { BARG = kgcm2_to_bar(value); }
        }

        public double kg_cm2
        {
            get { return bar_to_kgcm2(_BarA); }
            set { _BarA = kgcm2_to_bar(value); }
        }

        public double PSIG
        {
            get { return BarAtoPSIA(BARG); }
            set { BARG = psiatoBarA(value); }
        }

        private static double BarAToBarG(double barA)
        {
            return barA - Atm_bar;
        }

        private static double BarGToBarA(double barg)
        {
            return barg + Atm_bar;
        }

        private static double kgcm2_to_bar(double kg)
        {
            return kg / bar_kgcm2;
        }

        private static double bar_to_kgcm2(double bar)
        {
            return bar * bar_kgcm2;
        }

        public bool IsKnown { get { if (double.IsNaN(_BarA)) return false; else return true; } }

        public double BaseValue { get => _BarA; set => _BarA = value; }

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

        double IUOM.DisplayValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void EraseValue()
        {
            _BarA = double.NaN;
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "BarA":
                    return _BarA.ToString("F2", provider) + " Pressure ";

                case "MPa":
                    return MPa.ToString("F2", provider) + " MPa";

                case "kPa":
                    return kPa.ToString("F2", provider) + " kPa";

                case "atma":
                    return ATM.ToString("F2", provider) + " atma";

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
                    return ToString("bara", provider);

                case PressureUnit.MPa:
                    return ToString("MPa", provider);

                case PressureUnit.KPa:
                    return ToString("kPa", provider);

                case PressureUnit.MMHga:
                    return ToString("mmHg a", provider);

                case PressureUnit.BarG:
                    return ToString("bar g", provider);

                case PressureUnit.MPaG:
                    return ToString("MPa g", provider);

                case PressureUnit.KPaG:
                    return ToString("kPa g", provider);

                case PressureUnit.MMHgG:
                    return ToString("mmHg g", provider);

                default:
                    throw new FormatException("The Pressure  unit '" +
                          unit.ToString() + "' is unknown.");
            }
        }

        public string ToString(PressureUnit unit)
        {
            return ToString(unit, null);
        }

        public int CompareTo(Pressure value)
        {
            return _BarA.CompareTo(value._BarA);
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (value is not Pressure)
                return 1;

            return CompareTo((Pressure)value);
        }

        public bool Equals(Pressure value)
        {
            return _BarA == value._BarA;
        }

        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (value is not Pressure) return false;

            return Equals((Pressure)value);
        }

        public override int GetHashCode()
        {
            return _BarA.GetHashCode();
        }

        public void SetValue(Pressure p)
        {
            _BarA = p.BaseValue;
        }

        public static bool operator ==(Pressure t1, Pressure t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(Pressure t1, Pressure t2)
        {
            return !t1.Equals(t2);
        }

        public static bool operator >(Pressure t1, Pressure t2)
        {
            return t1._BarA > t2._BarA;
        }

        public static bool operator <(Pressure t1, Pressure t2)
        {
            return t1._BarA < t2._BarA;
        }

        public static bool operator >=(Pressure t1, Pressure t2)
        {
            return t1._BarA >= t2._BarA;
        }

        public static bool operator <=(Pressure t1, Pressure t2)
        {
            return t1._BarA <= t2._BarA;
        }

        public static Pressure operator +(Pressure t1, Pressure t2)
        {
            return new Pressure(t1._BarA + t2._BarA);
        }

        public static Pressure operator -(Pressure t1, Pressure t2)
        {
            return new Pressure(t1._BarA - t2._BarA);
        }

        public static Pressure operator *(Pressure t1, Pressure t2)
        {
            return new Pressure(t1._BarA * t2._BarA);
        }

        public static Pressure operator /(Pressure t1, Pressure t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Pressure(t1._BarA / t2._BarA);
        }

        public static Pressure operator %(Pressure t1, Pressure t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new Pressure(t1._BarA % t2._BarA);
        }

        public static double BarToMPA(double Pressure)
        {
            return Pressure / 10;
        }

        public static double MPatoBar(double MPa)
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

        public static double BarAtoPSIA(double Pressure)
        {
            return Pressure * 14.5038;
        }

        public static double BarGtoBarA(double Pressure)
        {
            return Pressure + Atm_bar;
        }

        public static double BarAtoATMA(double Pressure)
        {
            return Pressure * bar_Atm;
        }

        public static double ATMGtoBarG(double Pressure)
        {
            return Pressure / bar_Atm;
        }

        public static double ATMAtoBarA(double Pressure)
        {
            return Pressure * Atm_bar;
        }

        public static double BarGtoAtmG(double Pressure)
        {
            return (Pressure / Atm_bar) - Atm_bar;
        }

        public static double psiatoBarA(double psia)
        {
            return psia / 14.5038;
        }

        public static implicit operator double(Pressure t)
        {
            return t._BarA;
        }

        public static implicit operator Pressure(double t)
        {
            return new Pressure(t);
        }

        public void ClearpublicSetVariable()
        {
            _BarA = double.NaN;
        }

        public void DeltaValue(double v)
        {
            _BarA += v;
        }

        public double Valueout(PressureUnit unit)
        {
            switch (unit)
            {
                case PressureUnit.BarA: return _BarA;
                case PressureUnit.BarG: return BARG;
                case PressureUnit.KPa: return kPa;
                case PressureUnit.MPa: return MPa;
                case PressureUnit.MMHga: return MMHGA;
                case PressureUnit.MMHgG: return MMHGG;
                case PressureUnit.MPaG: return MPaG;
                case PressureUnit.Kg_cm2: return kg_cm2;
                case PressureUnit.Kg_cm2_g: return kg_cm2_g;
                case PressureUnit.PSIA: return PSIA;
                case PressureUnit.PSIG: return PSIG;
                default:
                    throw new ArgumentException("Unknown Pressure  unit '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(PressureUnit unit, double value)
        {
            switch (unit)
            {
                case PressureUnit.BarA: _BarA = value; break;
                case PressureUnit.BarG: _BarA = BarGtoBarA(value); break;
                case PressureUnit.KPa: _BarA = kPatoBarA(value); break;
                case PressureUnit.KPaG: _BarA = BarGtoBarA(kPatoBarA(value)); break;
                case PressureUnit.MPa: _BarA = MPatoBar(value); break;
                case PressureUnit.MPaG: _BarA = BarGtoBarA(MPatoBar(value)); break;
                case PressureUnit.Kg_cm2: _BarA = kgcm2_to_bar(value); break;
                case PressureUnit.Kg_cm2_g: _BarA = BarGtoBarA(kgcm2_to_bar(value)); break;
                case PressureUnit.MMHga: _BarA = MMHGtoBAR(value); break;
                case PressureUnit.MMHgG: _BarA = BarGtoBarA(MMHGtoBAR(value)); break;
                case PressureUnit.PSIA: _BarA = psiatoBarA(value); break;
                case PressureUnit.PSIG: _BarA = BarGtoBarA(psiatoBarA(value)); break;
                default:
                    throw new ArgumentException("Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            if (Enum.TryParse(unit, out PressureUnit U))
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
            if (Enum.TryParse(unit, out PressureUnit U))
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
            return Math.Pow(_BarA, x);
        }
    }
}