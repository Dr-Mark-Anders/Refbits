using System;
using System.ComponentModel;
using System.Globalization;

namespace Units.UOM
{
    // Units are not finished......

    /// <summary>
    /// Options for energy measurement units.
    /// </summary>
    public enum EnergyFlowUnit
    {
        [Description("kW")]
        kW,

        [Description("MW")]
        MW,

        [Description("kJ/s")]
        kJ_s,

        [Description("kJ/hr")]
        kJ_hr,

        [Description("BTU/hr")]
        BTU_hr
    }

    /// <summary>
    /// A Temperature  value.
    /// </summary>
    [Serializable]
    public struct EnergyFlow : IFormattable, IComparable, IComparable<EnergyFlow>, IEquatable<EnergyFlow>, IUOM
    {
        private double _kW;  // Base unit
        public double Tolerance => 0.0001;

        public EnergyFlow(double energy = double.NaN) : this()
        {
            _kW = energy;
        }

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out EnergyFlowUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public EnergyFlow(double energy = double.NaN, EnergyFlowUnit unit = EnergyFlowUnit.kW) : this()
        {
            switch (unit)
            {
                case EnergyFlowUnit.kW:
                    _kW = energy;
                    break;

                case EnergyFlowUnit.MW:
                    _kW = MWtokw(energy);
                    break;

                case EnergyFlowUnit.kJ_s:
                    _kW = kJ_stokW(energy);
                    break;

                case EnergyFlowUnit.kJ_hr:
                    _kW = kJ_hrtokW(energy);
                    break;

                case EnergyFlowUnit.BTU_hr:
                    _kW = BTU_hr_to_kW(energy);
                    break;

                default:
                    throw new ArgumentException("The Energy unit '" + unit.ToString() + "' is unknown.");
            }
        }

        public double kW
        {
            get { return _kW; }
            set { _kW = value; }
        }

        public double MW
        {
            get { return kwtoMW(_kW); }
            set { _kW = MWtokw(value); }
        }

        public double kJ_s
        {
            get { return kwtokJ_s(_kW); }
            set { _kW = kJ_stokW(value); }
        }

        public double kJ_hr
        {
            get { return kwtokJ_hr(_kW); }
            set { _kW = kJ_hrtokW(value); }
        }

        public double BTU_hr
        {
            get { return kwtoBTU_hr(_kW); }
            set { _kW = BTU_hr_to_kW(value); }
        }

        private double BTU_hr_to_kW(double BTUHr)
        {
            return BTUHr / 3412.142;
        }

        private double kwtoBTU_hr(double kw)
        {
            return kw * 3412.142;
        }

        public bool IsKnown { get { if (double.IsNaN(_kW)) return false; else return true; } }

        public double BaseValue { get => _kW; set => _kW = value; }

        public string DefaultUnit => EnergyFlowUnit.kW.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(EnergyFlowUnit));
            }
        }

        public string Name { get => "Power"; }

        public ePropID propid => ePropID.EnergyFlow;

        double IUOM.DisplayValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void EraseValue()
        {
            _kW = double.NaN;
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "kW":
                    return _kW.ToString("F2", provider) + " kW";

                case "MW":
                    return MW.ToString("F2", provider) + " MW";

                case "kJ_s":
                    return kJ_hr.ToString("F2", provider) + " kJ/hr";

                case "BTU_hr":
                    return BTU_hr.ToString("F2", provider) + " BTU_hr";

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

        public string ToString(EnergyFlowUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case EnergyFlowUnit.kW:
                    return ToString("kW", provider);

                case EnergyFlowUnit.MW:
                    return ToString("MW", provider);

                case EnergyFlowUnit.kJ_s:
                    return ToString("kJ/s", provider);

                case EnergyFlowUnit.kJ_hr:
                    return ToString("kJ/hr", provider);

                case EnergyFlowUnit.BTU_hr:
                    return ToString("BTU/hr", provider);

                default:
                    throw new FormatException("The Energy unit '" +
                          unit.ToString() + "' is unknown.");
            }
        }

        public string ToString(EnergyFlowUnit unit)
        {
            return ToString(unit, null);
        }

        public int CompareTo(EnergyFlow value)
        {
            return _kW.CompareTo(value._kW);
        }

        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is EnergyFlow)) throw new ArgumentException();
            return CompareTo((EnergyFlow)value);
        }

        public bool Equals(EnergyFlow value)
        {
            return _kW == value._kW;
        }

        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is EnergyFlow)) return false;

            return Equals((EnergyFlow)value);
        }

        public override int GetHashCode()
        {
            return _kW.GetHashCode();
        }

        public void SetValue(EnergyFlow p)
        {
            _kW = p;
        }

        public static bool operator ==(EnergyFlow t1, EnergyFlow t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(EnergyFlow t1, EnergyFlow t2)
        {
            return !t1.Equals(t2);
        }

        public static bool operator >(EnergyFlow t1, EnergyFlow t2)
        {
            return t1._kW > t2._kW;
        }

        public static bool operator <(EnergyFlow t1, EnergyFlow t2)
        {
            return t1._kW < t2._kW;
        }

        public static bool operator >=(EnergyFlow t1, EnergyFlow t2)
        {
            return t1._kW >= t2._kW;
        }

        public static bool operator <=(EnergyFlow t1, EnergyFlow t2)
        {
            return t1._kW <= t2._kW;
        }

        public static EnergyFlow operator +(EnergyFlow t1, EnergyFlow t2)
        {
            return new EnergyFlow(t1._kW + t2._kW);
        }

        public static EnergyFlow operator -(EnergyFlow t1, EnergyFlow t2)
        {
            return new EnergyFlow(t1._kW - t2._kW);
        }

        public static EnergyFlow operator *(EnergyFlow t1, EnergyFlow t2)
        {
            return new EnergyFlow(t1._kW * t2._kW);
        }

        public static EnergyFlow operator /(EnergyFlow t1, EnergyFlow t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new EnergyFlow(t1._kW / t2._kW);
        }

        public static EnergyFlow operator %(EnergyFlow t1, EnergyFlow t2)
        {
            // TODO: throw  a divide-by-zero exception if needed.
            return new EnergyFlow(t1._kW % t2._kW);
        }

        public static double kwtoMW(double Power)
        {
            return Power / 1000;
        }

        public static double MWtokw(double MW)
        {
            return MW * 1000;
        }

        public static double kwtokJ_hr(double kW)
        {
            return kW * 3600;
        }

        public static double kJ_hrtokW(double kj_hr)
        {
            return kj_hr / 3600;
        }

        public static double kwtokJ_s(double kW)
        {
            return kW;
        }

        public static double kJ_stokW(double kj_s)
        {
            return kj_s;
        }

        public static implicit operator double(EnergyFlow t)
        {
            return t._kW;
        }

        public static implicit operator EnergyFlow(double t)
        {
            return new EnergyFlow(t);
        }

        public void ClearpublicSetVariable()
        {
            _kW = double.NaN;
        }

        public void DeltaValue(double v)
        {
            _kW += v;
        }

        public double Valueout(EnergyFlowUnit unit)
        {
            switch (unit)
            {
                case EnergyFlowUnit.kW: return _kW;
                case EnergyFlowUnit.MW: return MW;
                case EnergyFlowUnit.kJ_s: return kJ_s;
                case EnergyFlowUnit.BTU_hr: return BTU_hr;
                default:
                    throw new ArgumentException("Unknown Power unit '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(EnergyFlowUnit unit, double value)
        {
            switch (unit)
            {
                case EnergyFlowUnit.kW: _kW = value; break;
                case EnergyFlowUnit.MW: MW = value; break;
                case EnergyFlowUnit.kJ_s: kJ_s = value; break;
                case EnergyFlowUnit.kJ_hr: kJ_hr = value; break;
                case EnergyFlowUnit.BTU_hr: BTU_hr = value; break;
                default:
                    throw new ArgumentException("Unknown Temperature  unit '" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            EnergyFlowUnit U;
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
            EnergyFlowUnit U;
            if (double.IsNaN(value))
            {
                BaseValue = double.NaN;
            }
            else if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, value);
            }
            else
            {
                throw new ArgumentException("Unknown Power unit '" + unit.ToString() + "'.");
            }
        }

        public double Pow(double x)
        {
            return double.NaN;
        }
    }
}