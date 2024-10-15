using System;
using System.Globalization;

namespace Units.UOM
{
    ///<summary>
    ///OptionsforArea measurementunits.
    ///</summary>
    public enum AreaUnit
    {
        m2,
        cm2,
        mm2,
    }

    ///<summary>
    ///AArea value.
    ///</summary>
    [Serializable]
    public struct Area : IFormattable, IComparable, IComparable<Area>, IEquatable<Area>, IUOM
    {
        private double _value = double.NaN;
        private string name = "";

        public string UnitDescriptor(string unit)
        {
            Enum.TryParse(unit, out AreaUnit res);
            return Enumhelpers.GetEnumDescription(res);
        }

        public double Tolerance => 0.0001;

        public double Pow(double x)
        {
            return Math.Pow(_value, x);
        }

        public bool IsKnown
        {
            get
            {
                if (double.IsNaN(_value))
                    return false;
                return true;
            }
        }

        ///<summary>
        ///Createsanew Area withthespecifiedvalueinKelvin.
        ///</summary>
        ///<paramname="Area ">ThevalueoftheArea .</param>
        public Area(double Area) : this() { _value = Area; }

        ///<summary>
        ///Createsanew Area withthespecifiedvalueint he
        ///specifiedunitofmeasurement.
        ///</summary>
        ///<paramname="Area ">ThevalueoftheArea .</param>
        ///<paramname="unit">Theunitofmeasurementthatdefineshow
        ///the<paramrefname="Area "/>value is used.</param>
        public Area(double Area, AreaUnit unit) : this()
        {
            switch (unit)
            {
                case AreaUnit.m2:
                    _value = Area;
                    break;

                case AreaUnit.cm2:
                    cm = Area;
                    break;

                case AreaUnit.mm2:
                    mm = Area;
                    break;

                default:
                    throw new ArgumentException("The Area unit '" + unit.ToString() + "'isunknown.");
            }
        }

        ///<summary>
        ///GetsorsetstheArea valueinKelvin.
        ///</summary>
        public double m
        {
            get { return _value; }
            set { _value = value; }
        }

        ///<summary>
        ///GetsorsetstheArea valueinCelsius.
        ///</summary>
        public double cm
        {
            get { return m_to_cm(_value); }
            set { _value = cm_to_m(value); }
        }

        public double mm
        {
            get { return m_to_mm(_value); }
            set { _value = mm_to_m(value); }
        }

        public void EraseValue()
        {
            _value = double.NaN;
        }

        ///<summary>
        ///return  sastring representationoftheArea value.
        ///</summary>
        ///<paramname="format">
        ///Asingleformatspecifierthatindicateshowtoformatthevalueofthis
        ///Area .Theformatparametercanbe"G",
        ///"C","F",or"K".Ifformat
        ///isnullortheemptystring (""),"G"isused.
        ///</param>
        ///<paramname="formatProvider">
        ///AnIFormatProviderreferencethatsuppliesculture-specificformatting
        ///services.
        ///</param>
        ///<return  s>Astring representationoftheArea .</return  s>
        ///<exceptioncref="FormatException">
        ///Thevalueofformatisnotnull,theemptystring (""),"G","C","F",or
        ///"K".
        ///</exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "K";
            if (formatProvider == null) formatProvider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G":
                case "m":
                    return m.ToString("F2", formatProvider) + "m";

                case "cm":
                    return cm.ToString("F2", formatProvider) + "cm";

                case "mm":
                    return cm.ToString("F2", formatProvider) + "mm";

                default:
                    throw new FormatException(string.Format("The{0}formatstring isnotsupported.", format));
            }
        }

        ///<summary>
        ///return  sastring representationoftheArea value.
        ///</summary>
        ///<paramname="format">
        ///Asingleformatspecifierthatindicateshowtoformatthevalueofthis
        ///Area .Theformatparametercanbe"G",
        ///"C","F",or"K".Ifformat
        ///isnullortheemptystring (""),"G"isused.
        ///</param>
        ///<return  s>Astring representationoftheArea .</return  s>
        ///<exceptioncref="FormatException">
        ///Thevalueofformatisnotnull,
        ///theemptystring (""),"G","C","F",or
        ///"K".
        ///</exception>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(ePropID prop, IFormatProvider provider)
        {
            return "";
        }

        ///<summary>
        ///return  sastring representationoftheArea value.
        ///</summary>
        ///<return  s>Astring representationoftheArea .</return  s>
        public override string ToString()
        {
            return ToString(null, null);
        }

        ///<summary>
        ///return  sastring representationoftheArea value.
        ///</summary>
        ///<paramname="unit">
        ///TheAreaUnit  aswhichtheArea valueshouldbedisplayed.
        ///</param>
        ///<paramname="provider">
        ///AnIFormatProviderreferencethatsuppliesculture-specificformatting
        ///services.
        ///</param>
        ///<return  s>Astring representationoftheArea .</return  s>
        public string ToString(AreaUnit unit, IFormatProvider provider)
        {
            switch (unit)
            {
                case AreaUnit.m2:
                    return ToString("m", provider);

                case AreaUnit.cm2:
                    return ToString("cm", provider);

                case AreaUnit.mm2:
                    return ToString("mm", provider);

                default:
                    throw new FormatException("TheAreaUnit  '" + unit.ToString() + "'isunknown.");
            }
        }

        ///<summary>
        ///return  sastring representationoftheArea value.
        ///</summary>
        ///<paramname="unit">
        ///TheAreaUnit  aswhichtheArea valueshouldbedisplayed.
        ///</param>
        ///<return  s>Astring representationoftheArea .</return  s>
        public string ToString(AreaUnit unit)
        {
            return ToString(unit, null);
        }

        public double BaseValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string DefaultUnit => AreaUnit.m2.ToString();

        public string[] AllUnits
        {
            get
            {
                return Enum.GetNames(typeof(AreaUnit));
            }
        }

        public string Name => name;

        public ePropID propid => ePropID.Area;

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

        ///<summary>
        ///ComparesthisinstancetoaspecifiedArea object andreturn  sanindication
        ///oftheirrelativevalues.
        ///</summary>
        ///<paramname="value">AArea object
        ///tocomparetothisinstance.</param>
        ///<return  s>
        ///Asignednumberindicatingtherelativevaluesofthisinstanceandvalue.
        ///ValueDescriptionAnegativeint egerThisinstanceislessthanvalue.Zero
        ///Thisinstanceisequaltovalue.Apositiveint egerThisinstanceisgreater
        ///thanvalue.
        ///</return  s>
        public int CompareTo(Area value)
        {
            return _value.CompareTo(value._value);
        }

        ///<summary>
        ///Comparesthisinstancetoaspecifiedobject andreturn  sanindicationof
        ///theirrelativevalues.
        ///</summary>
        ///<paramname="value">Anobject tocompare,ornull.</param>
        ///<return  s>
        ///Asignednumberindicatingtherelativevaluesofthisinstanceandvalue.
        ///ValueDescriptionAnegativeint egerThisinstanceislessthanvalue.Zero
        ///Thisinstanceisequaltovalue.Apositiveint egerThisinstanceisgreater
        ///thanvalue,orvalue is null.
        ///</return  s>
        ///<exceptioncref="ArgumentException">
        ///Thevalue is notaArea .
        ///</exception>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is Area)) throw new ArgumentException();
            return CompareTo((Area)value);
        }

        ///<summary>
        ///DetermineswhetherornotthegivenArea isconsideredequaltothisinstance.
        ///</summary>
        ///<paramname="value">TheArea tocomparetothisinstance.</param>
        ///<return  s>TrueiftheArea isconsideredequal
        ///tothisinstance.Otherwise,false.</return  s>
        public bool Equals(Area value)
        {
            return _value == value._value;
        }

        ///<summary>
        ///Determineswhetherornotthegivenobject isconsideredequaltotheArea .
        ///</summary>
        ///<paramname="value">Theobject tocomparetotheArea .</param>
        ///<return  s>Trueiftheobject isconsideredequal
        ///totheArea .Otherwise,false.</return  s>
        public override bool Equals(object value)
        {
            if (value == null) return false;
            if (!(value is Area)) return false;

            return Equals((Area)value);
        }

        ///<summary>
        ///return  sthehashcodeforthisinstance.
        ///</summary>
        ///<return  s>A32-bitsignedint egerhashcode.</return  s>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        ///<summary>
        ///DeterminestheeQuality oftwoArea s.
        ///</summary>
        ///<paramname="t1">ThefirstArea tobecompared.</param>
        ///<paramname="t2">ThesecondArea tobecompared.</param>
        ///<return  s>TrueiftheArea sareequal.Otherwise,false.</return  s>
        public static bool operator ==(Area t1, Area t2)
        {
            return t1.Equals(t2);
        }

        ///<summary>
        ///DeterminestheineQuality oftwoArea s.
        ///</summary>
        ///<paramname="t1">ThefirstArea tobecompared.</param>
        ///<paramname="t2">ThesecondArea tobecompared.</param>
        ///<return  s>TrueiftheArea sareNOTequal.Otherwise,false.</return  s>
        public static bool operator !=(Area t1, Area t2)
        {
            return !t1.Equals(t2);
        }

        ///<summary>
        ///DetermineswhetheroneArea isconsideredgreaterthananother.
        ///</summary>
        ///<paramname="t1">ThefirstArea tobecompared.</param>
        ///<paramname="t2">ThesecondArea tobecompared.</param>
        ///<return  s>TrueifthefirstArea isgreaterthanthesecond.
        ///Otherwise,false.</return  s>
        public static bool operator >(Area t1, Area t2)
        {
            return t1._value > t2._value;
        }

        ///<summary>
        ///DetermineswhetheroneArea isconsideredlessthananother.
        ///</summary>
        ///<paramname="t1">ThefirstArea tobecompared.</param>
        ///<paramname="t2">ThesecondArea tobecompared.</param>
        ///<return  s>TrueifthefirstArea islessthanthesecond.
        ///Otherwise,false.</return  s>
        public static bool operator <(Area t1, Area t2)
        {
            return t1._value < t2._value;
        }

        ///<summary>
        ///DetermineswhetheroneArea isconsideredgreatertoorequaltoanother.
        ///</summary>
        ///<paramname="t1">ThefirstArea tobecompared.</param>
        ///<paramname="t2">ThesecondArea tobecompared.</param>
        ///<return  s>
        ///TrueifthefirstArea isgreatertoorequaltothesecond.Otherwise,false.
        ///</return  s>
        public static bool operator >=(Area t1, Area t2)
        {
            return t1._value >= t2._value;
        }

        ///<summary>
        ///DetermineswhetheroneArea isconsideredlessthanorequaltoanother.
        ///</summary>
        ///<paramname="t1">ThefirstArea tobecompared.</param>
        ///<paramname="t2">ThesecondArea tobecompared.</param>
        ///<return  s>
        ///TrueifthefirstArea islessthanorequaltothesecond.Otherwise,false.
        ///</return  s>
        public static bool operator <=(Area t1, Area t2)
        {
            return t1._value <= t2._value;
        }

        ///<summary>
        ///AddstwoinstancesoftheArea object .
        ///</summary>
        ///<paramname="t1">TheArea ontheleft-handsideofthe operator  .
        ///</param>
        ///<paramname="t2">TheArea ontheright-handsideofthe operator  .
        ///</param>
        ///<return  s>ThesumofthetwoArea s.</return  s>
        public static Area operator +(Area t1, Area t2)
        {
            return new Area(t1._value + t2._value);
        }

        ///<summary>
        ///Subtractsoneinstancefromanother.
        ///</summary>
        ///<paramname="t1">TheArea ontheleft-handsideofthe operator  .
        ///</param>
        ///<paramname="t2">TheArea ontheright-handsideofthe operator  .
        ///</param>
        ///<return  s>ThedifferenceofthetwoArea s.</return  s>
        public static Area operator -(Area t1, Area t2)
        {
            return new Area(t1._value - t2._value);
        }

        ///<summary>
        ///MultipliestwoinstancesoftheArea object .
        ///</summary>
        ///<paramname="t1">TheArea ontheleft-handsideofthe operator  .
        ///</param>
        ///<paramname="t2">TheArea ontheright-handsideofthe operator  .
        ///</param>
        ///<return  s>TheproductofthetwoArea s.</return  s>
        public static Area operator *(Area t1, Area t2)
        {
            return new Area(t1._value * t2._value);
        }

        ///<summary>
        ///DividesoneinstanceofaArea object byanother.
        ///</summary>
        ///<paramname="t1">TheArea ontheleft-handsideofthe operator  .
        ///</param>
        ///<paramname="t2">TheArea ontheright-handsideofthe operator  .
        ///</param>
        ///<return  s>ThequotientofthetwoArea .</return  s>
        public static Area operator /(Area t1, Area t2)
        {
            return new Area(t1._value / t2._value);
        }

        ///<summary>
        ///FindstheremainderwhenoneinstanceofaArea object isdividedbyanother.
        ///</summary>
        ///<paramname="t1">TheArea ontheleft-handsideofthe operator  .
        ///</param>
        ///<paramname="t2">TheArea ontheright-handsideofthe operator  .
        ///</param>
        ///<return  s>Theremainderafterthequotientisfound.</return  s>
        public static Area operator %(Area t1, Area t2)
        {
            return new Area(t1._value % t2._value);
        }

        ///<summary>
        ///ConvertsaKelvinArea valuetoCelsius.
        ///</summary>
        ///<paramname="m">TheKelvinvaluetoconverttoCelsius.
        ///</param>
        ///<return  s>TheKelvinvalueinCelsius.</return  s>
        public static double m_to_cm(double m)
        {
            return m * 100;
        }

        ///<summary>
        ///ConvertsaCelsiusvaluetoKelvin.
        ///</summary>
        ///<paramname="cm">TheCelsiusvaluetoconverttoKelvin.
        ///</param>
        ///<return  s>TheCelsiusvalueinKelvin.</return  s>
        public static double cm_to_m(double cm)
        {
            return cm / 100;
        }

        public static double mm_to_m(double mm)
        {
            return mm / 1000;
        }

        public static double m_to_mm(double m)
        {
            return m * 1000;
        }

        public static implicit operator double(Area t)
        {
            return t.m;
        }

        public static implicit operator Area(double t)
        {
            return new Area(t);
        }

        public void DeltaValue(double v)
        {
            _value += v;
        }

        ///<summary>
        ///GetstheArea valueint hespecifiedunitofmeasurement.
        ///</summary>
        ///<paramname="unit">Theunitofmeasurement
        ///inwhichtheArea shouldberetrieved.</param>
        ///<return  s>TheArea valueint hespecified
        ///<paramrefname="unit"/>.</return  s>
        public double Valueout(AreaUnit unit, double value)
        {
            switch (unit)
            {
                case AreaUnit.m2: _value = value; return value;
                case AreaUnit.cm2: cm = value; return value;
                case AreaUnit.mm2: mm = value; return value;
                default:
                    throw new ArgumentException("UnknownAreaUnit  '" + unit.ToString() + "'.");
            }
        }

        public double Valueout(AreaUnit unit)
        {
            switch (unit)
            {
                case AreaUnit.m2: return m;
                case AreaUnit.cm2: return cm;
                case AreaUnit.mm2: return mm;
                default:
                    throw new ArgumentException("UnknownPressure unit'" + unit.ToString() + "'.");
            }
        }

        public double ValueOut(string unit)
        {
            AreaUnit U;
            if (Enum.TryParse(unit, out U))
            {
                return Valueout(U);
            }
            else
            {
                return double.NaN;
            }
        }

        ///<summary>
        ///GetstheArea valueint hespecifiedunitofmeasurement.
        ///</summary>
        ///<paramname="unit">Theunitofmeasurement
        ///inwhichtheArea shouldberetrieved.</param>
        ///<return  s>TheArea valueint hespecified
        ///<paramrefname="unit"/>.</return  s>
        public double ValueIn(AreaUnit unit)
        {
            switch (unit)
            {
                case AreaUnit.m2: return _value;
                case AreaUnit.cm2: return cm;
                case AreaUnit.mm2: return mm;
                default:
                    throw new ArgumentException("UnknownAreaUnit  '" + unit.ToString() + "'.");
            }
        }

        public void ValueIn(AreaUnit unit, double value)
        {
            switch (unit)
            {
                case AreaUnit.m2: m = value; break;
                case AreaUnit.cm2: cm = value; break;
                case AreaUnit.mm2: mm = value; break;
                default:
                    break;
            }
        }

        public void ValueIn(string unit, double value)
        {
            AreaUnit U;
            if (Enum.TryParse(unit, out U))
            {
                ValueIn(U, value);
            }
        }

        public string Unitstring(ePropID prop)
        {
            return "";
        }
    }
}