using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Units;
using Units.UOM;


namespace ModelEngine
{
    [TypeConverter(typeof(Specification)), Description("Expand to see value and units")]
    [Serializable]
    public class SpecificationExpander : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is UOMProperty property)
            {
                return property.BaseValue;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class Specification : IEquatable<Specification>, ISerializable
    {
        private bool isConfigured = false;
        private string name = "";
        private Guid guid = Guid.NewGuid();
        private bool isactive = false;

        public UOMProperty unitConvert = new();
        public string UnitDescriptor;

        public double specvalue;
        public double value;

        //public  String currentUnits;
        public string currentUnits
        {
            get
            {
                return unitConvert.DisplayUnit;
            }
            set
            {
                unitConvert.DisplayUnit = value;
            }
        }

        public enumMassMolarOrVol FlowBasis = enumMassMolarOrVol.Molar;
        public eSpecType graphicSpecType = eSpecType.TrayNetLiqFlow;
        public eSpecType engineSpecType = eSpecType.TrayNetLiqFlow;

        private Guid enginestageGuid;
        private Guid enginesectionGuid;

        public Guid engineObjectguid;
        public Guid sideStreamGuid;

        public Guid drawObjectGuid;
        public Guid drawStreamGuid;
        public Guid drawSectionGuid;

        public TraySection traysection;
        public Tray tray;

        private Guid pADrawTray, pAReturnTray;

        public int stageNo = 999;
        internal double error;
        internal double baseerror;
        public double Gradient = 0;

        public override string ToString()
        {
            return this.name;
        }

        public static ePropID Spec_ePropID(eSpecType t)
        {
            switch (t)
            {
                case eSpecType.LiquidProductDraw:
                case eSpecType.VapProductDraw:
                case eSpecType.TrayNetLiqFlow:
                case eSpecType.TrayNetVapFlow:
                case eSpecType.PAFlow:
                    return ePropID.MOLEF;

                case eSpecType.Energy:
                case eSpecType.PADuty:
                    return ePropID.EnergyFlow;

                case eSpecType.PADeltaT:
                    return ePropID.DeltaT;

                case eSpecType.PARetT:
                case eSpecType.Temperature:
                case eSpecType.DistSpec:
                    return ePropID.T;

                case eSpecType.RefluxRatio:
                    return ePropID.NullUnits;

                default:
                    return ePropID.NullUnits;
            }
        }

        public UOMProperty SpecUOMProperty()
        {
            switch (engineSpecType)
            {
                case eSpecType.LiquidProductDraw:
                case eSpecType.VapProductDraw:
                case eSpecType.TrayNetLiqFlow:
                case eSpecType.TrayNetVapFlow:
                case eSpecType.PAFlow:
                    switch (FlowBasis)
                    {
                        case enumMassMolarOrVol.Molar:
                            return new UOMProperty(ePropID.MOLEF, specvalue);

                        case enumMassMolarOrVol.Mass:
                            return new UOMProperty(ePropID.Mass, specvalue);

                        case enumMassMolarOrVol.Vol:
                            return new UOMProperty(ePropID.LiquidVolumeFlow, specvalue);
                    }
                    break;

                case eSpecType.Energy:
                case eSpecType.PADuty:
                    return new UOMProperty(ePropID.EnergyFlow, specvalue);

                case eSpecType.PADeltaT:
                case eSpecType.PARetT:
                case eSpecType.Temperature:
                    return new UOMProperty(ePropID.T, specvalue);

                case eSpecType.RefluxRatio:
                    return new UOMProperty(ePropID.NullUnits, specvalue);

                default:
                    return new UOMProperty(ePropID.NullUnits, specvalue);
            }
            return new UOMProperty(ePropID.NullUnits, specvalue);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("value1", value);
            info.AddValue("specvalue1", specvalue);
            info.AddValue("specType", graphicSpecType);
            info.AddValue("currentUnits", currentUnits);
            info.AddValue("guid", guid);
            info.AddValue("isactive", isactive);

            info.AddValue("stage", enginestageGuid);
            info.AddValue("sectionGuid", enginesectionGuid);
            info.AddValue("Configured", isConfigured);
            info.AddValue("drawobjectguid", drawObjectGuid);
            info.AddValue("EngineObjectguid", engineObjectguid);
            info.AddValue("drawStreamGuid", drawStreamGuid);
            info.AddValue("paguid", PAguid);
            info.AddValue("distpoint", distpoint);
            info.AddValue("distType", distType);
            info.AddValue("enginspecType", engineSpecType);
            //info.AddValue("PropID", propID);
            info.AddValue("FlowType", FlowType);
        }

        public Specification(SerializationInfo info, StreamingContext context)
        {
            try
            {
                name = info.GetString("name");
                value = info.GetDouble("value1");
                specvalue = info.GetDouble("specvalue1");
                graphicSpecType = (eSpecType)info.GetValue("specType", typeof(eSpecType));
                currentUnits = info.GetString("currentUnits");
                guid = (Guid)info.GetValue("guid", typeof(Guid));
                IsActive = info.GetBoolean("isactive");

                enginestageGuid = (Guid)info.GetValue("stage", typeof(Guid));
                enginesectionGuid = (Guid)info.GetValue("sectionGuid", typeof(Guid));

                isConfigured = info.GetBoolean("Configured");

                drawObjectGuid = (Guid)info.GetValue("drawobjectguid", typeof(Guid));
                engineObjectguid = (Guid)info.GetValue("EngineObjectguid", typeof(Guid));
                drawStreamGuid = (Guid)info.GetValue("drawStreamGuid", typeof(Guid));
                PAguid = (Guid)info.GetValue("paguid", typeof(Guid));

                distpoint = (enumDistPoints)info.GetValue("distpoint", typeof(enumDistPoints));
                distType = (enumDistType)info.GetValue("distType", typeof(enumDistType));
                engineSpecType = (eSpecType)info.GetValue("enginspecType", typeof(eSpecType));
                FlowType = (enumflowtype)info.GetValue("FlowType", typeof(enumflowtype));
            }
            catch
            {
            }

            unitConvert.DisplayUnit = currentUnits;
            ChangeUnits();
        }

        public bool Equals(Specification other)
        {
            if (this.Guid == other.Guid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Specification()
        {
        }

        [Browsable(false)]
        public ePropID propID
        {
            get
            {
                if (unitConvert.UOM is null)
                    return ePropID.Error;
                else
                    return unitConvert.UOM.propid;
            }
            set
            {
                unitConvert.Property = propID;
            }
        }

        [Browsable(false)]
        public string SpecName
        {
            get { return name; }
            set { name = value; }
        }

        public double SpecValue
        {
            get
            {
                return specvalue;
            }
            set
            {
                specvalue = value;
            }
        }

        public MoleFlow SpecValueMoles(double MW, double SG)
        {
            switch (FlowType)
            {
                case enumflowtype.Molar:
                    return specvalue;

                case enumflowtype.Mass:
                    return specvalue / MW;

                case enumflowtype.StdLiqVol:
                    return specvalue * SG / MW;
            }
            return double.NaN;
        }

        public double SpecDisplayValue
        {
            get
            {
                return unitConvert.DisplayValueOut(specvalue);
            }
            set
            {
                specvalue = unitConvert.DisplayValueIn(value);
            }
        }

        [Browsable(false)]
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public double DisplayValue
        {
            get
            {
                unitConvert.BaseValue = value;
                return unitConvert.DisplayValueOut();
            }
            set
            {
                this.value = unitConvert.DisplayValueIn(value);
            }
        }

        [Browsable(false)]
        public bool IsActive
        {
            get { return isactive; }
            set { isactive = value; }
        }

        [Browsable(false)]
        public string CurrentUnits
        {
            get { return currentUnits; }
        }

        [EditorAttribute(typeof(SelEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(false)]
        public string Units
        {
            get
            {
                string[] Str1 = (string[])unitConvert.AllUnits;
                SelEditor.strList = Str1;
                return unitConvert.DisplayUnit;
            }
            set
            {
                if (value == null)
                    unitConvert.DisplayUnit = this.unitConvert.DefaultUnit;
                else
                    unitConvert.DisplayUnit = value;
            }
        }

        public void ChangeUnits()
        {
            switch (engineSpecType)
            {
                case eSpecType.RefluxRatio:
                    UOM = UOMUtility.GetUOM(ePropID.NullUnits);
                    break;

                case eSpecType.Energy:
                case eSpecType.PADuty:
                    UOM = UOMUtility.GetUOM(ePropID.EnergyFlow);
                    break;

                case eSpecType.DistSpec:
                case eSpecType.Temperature:
                case eSpecType.PARetT:
                    UOM = UOMUtility.GetUOM(ePropID.T);
                    //unitConvert.DisplayUnit = "C";
                    break;

                case eSpecType.PADeltaT:
                    UOM = UOMUtility.GetUOM(ePropID.DeltaT);
                    //unitConvert.DisplayUnit = "C";
                    break;

                case eSpecType.VapProductDraw:
                case eSpecType.LiquidProductDraw:
                case eSpecType.PAFlow:
                case eSpecType.TrayNetLiqFlow:
                case eSpecType.TrayNetVapFlow:
                    switch (FlowType)
                    {
                        case enumflowtype.Molar:
                            UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                            break;

                        case enumflowtype.Mass:
                            UOM = UOMUtility.GetUOM(ePropID.MF);
                            break;

                        case enumflowtype.StdLiqVol:
                            UOM = UOMUtility.GetUOM(ePropID.LiquidVolumeFlow);
                            break;
                    }
                    if (unitConvert.AllUnits.Contains(currentUnits))
                        unitConvert.DisplayUnit = currentUnits;
                    else
                        unitConvert.DisplayUnit = unitConvert.DefaultUnit;
                    break;

                default:
                    break;
            }
        }

        [Browsable(false)]
        public Guid engineSectionGuid
        {
            get
            {
                return enginesectionGuid;
            }
            set
            {
                enginesectionGuid = value;
            }
        }

        [Browsable(false)]
        public Guid engineStageGuid
        {
            get
            {
                return enginestageGuid;
            }
            set
            {
                enginestageGuid = value;
            }
        }

        [Browsable(false)]
        public bool IsLiquid
        {
            get
            {
                switch (graphicSpecType)
                {
                    case eSpecType.LiquidProductDraw:
                    case eSpecType.TrayNetLiqFlow:
                    case eSpecType.PAFlow:
                    case eSpecType.RefluxRatio:
                        return true;
                }
                return false;
            }
        }

        [Browsable(false)]
        public bool IsSideProduct
        {
            get
            {
                switch (graphicSpecType)
                {
                    case eSpecType.LiquidProductDraw:
                        return true;
                }
                return false;
            }
        }

        [Browsable(false)]
        public bool IsPumparoundSpec
        {
            get
            {
                switch (graphicSpecType)
                {
                    case eSpecType.PADuty:
                    case eSpecType.PAFlow:
                    case eSpecType.PARetT:
                    case eSpecType.PADeltaT:
                        return true;
                }
                return false;
            }
        }

        [Browsable(false)]
        public Guid Guid
        {
            get
            {
                return guid;
            }

            set
            {
                guid = value;
            }
        }

        [Browsable(false)]
        public Guid PADrawTray
        {
            get { return pADrawTray; }
            set
            {
                pADrawTray = value;
            }
        }

        [Browsable(false)]
        public Guid PAReturnTray
        {
            get
            {
              return pAReturnTray;
            }
            set
            {
              pAReturnTray = value;
            }
        }

        [Browsable(false)]
        internal int StageNo { get => stageNo; set => stageNo = value; }

        public IUOM UOM { get => unitConvert.UOM; set => unitConvert.UOM = value; }
        public Guid PAguid { get => paguid; set => paguid = value; }
        public bool IsConfigured { get => isConfigured; set => isConfigured = value; }
        public enumflowtype FlowType { get; set; } = enumflowtype.Molar;

        public ConnectingStream connectedStream = null;
        public enumDistType distType;
        public enumDistPoints distpoint;
        internal double delta;
        private Guid paguid;

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, SideStream ss, enumDistType distype, enumDistPoints distpoint, bool IsActive = true, bool baseunits = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.unitConvert = new UOMProperty(property, specValue);
            if (baseunits)
                currentUnits = unitConvert.DefaultUnit;
            else
                currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.unitConvert.DisplayUnit = currentUnits;
            this.isactive = IsActive;
            specvalue = unitConvert.ValueIn(currentUnits, specValue);
            this.sideStreamGuid = ss.Guid;

            this.distType = distype;
            this.distpoint = distpoint;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, TraySection ts, bool IsActive = true, bool baseunits = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.unitConvert = new UOMProperty(property, specValue);
            if (baseunits)
                currentUnits = unitConvert.DefaultUnit;
            else
                currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.unitConvert.DisplayUnit = currentUnits;
            this.isactive = IsActive;
            this.drawObjectGuid = ts.Guid;
            specvalue = unitConvert.ValueIn(currentUnits, specValue);
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, Tray t, bool IsActive = true, bool baseunits = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.unitConvert = new UOMProperty(property, specValue);
            if (baseunits)
                currentUnits = unitConvert.DefaultUnit;
            else
                currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.unitConvert.DisplayUnit = currentUnits;
            this.isactive = IsActive;
            this.drawObjectGuid = t.Guid;
            specvalue = unitConvert.ValueIn(currentUnits, specValue);
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, Column column, bool IsActive = true, bool baseunits = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.unitConvert = new UOMProperty(property, specValue);
            if (baseunits)
                currentUnits = unitConvert.DefaultUnit;
            else
                currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.unitConvert.DisplayUnit = currentUnits;
            this.isactive = IsActive;
            this.drawObjectGuid = column.Guid;
            specvalue = unitConvert.ValueIn(currentUnits, specValue);
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, SideStream ss, bool IsActive = true, bool baseunits = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.unitConvert = new UOMProperty(property, specValue);
            if (baseunits)
                currentUnits = unitConvert.DefaultUnit;
            else
                currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.unitConvert.DisplayUnit = currentUnits;
            this.isactive = IsActive;
            this.sideStreamGuid = ss.Guid;
            specvalue = unitConvert.ValueIn(currentUnits, specValue);

            switch (property)
            {
                case ePropID.MOLEF:
                    FlowType = enumflowtype.Molar;
                    break;

                case ePropID.MF:
                    FlowType = enumflowtype.Mass;
                    break;

                case ePropID.VF:
                    FlowType = enumflowtype.StdLiqVol;
                    break;

                default:
                    FlowType = enumflowtype.None;
                    break;
            }
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, bool IsActive = true, bool baseunits = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.unitConvert = new UOMProperty(property, specValue);
            if (baseunits)
                currentUnits = unitConvert.DefaultUnit;
            else
                currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.unitConvert.DisplayUnit = currentUnits;
            this.isactive = IsActive;
            //this.sideStreamGuid = ss.Guid;
            specvalue = unitConvert.ValueIn(currentUnits, specValue);

            switch (property)
            {
                case ePropID.MOLEF:
                    FlowType = enumflowtype.Molar;
                    break;

                case ePropID.MF:
                    FlowType = enumflowtype.Mass;
                    break;

                case ePropID.VF:
                    FlowType = enumflowtype.StdLiqVol;
                    break;

                default:
                    FlowType = enumflowtype.None;
                    break;
            }
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }


        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section,
            Tray tray, ConnectingStream cs, bool IsActive = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.specvalue = new UOMProperty(property, specValue);
            currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.isactive = IsActive;
            this.connectedStream = cs;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public Specification(string name, double specValue, ePropID property, eSpecType spectype, TraySection section, Tray tray,
            PumpAround pa, bool IsActive = true)
        {
            this.enginesectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.name = name;
            this.graphicSpecType = spectype;
            this.engineSpecType = spectype;
            this.unitConvert.Property = property;
            this.specvalue = new UOMProperty(property, specValue);
            currentUnits = GlobalModel.displayunits.UnitsDict[property];
            this.isactive = IsActive;
            this.connectedStream = null;

            switch (spectype)
            {
                case eSpecType.PAFlow:
                case eSpecType.PARetT:
                case eSpecType.PADeltaT:
                case eSpecType.PADuty:
                    PAguid = pa.Guid;
                    break;
            }
        }

        public Specification(string name, int value, ePropID nullUnits, eSpecType lLE_KSpec, TraySection section, Tray tray)
        {
            this.name = name;
            this.specvalue = value;
            propID = nullUnits;
            engineSpecType = lLE_KSpec;
            this.engineSectionGuid = section.Guid;
            this.enginestageGuid = tray.Guid;
            this.StageNo = section.IndexOf(tray.Guid);
            this.isactive = true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Specification);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), Serializable]
    public class SpecificationCollection : IEnumerable<Specification>
    {
        private List<Specification> list = new();
        private int degreesoffreedom = 0;

        public void Sort()
        {
            // list.Sort((x, y) => x.StageNo.CompareTo(y.StageNo));
            // list.Sort((x, y) => x.SpecType.CompareTo(y.SpecType));

            list.Sort((x, y) =>
            {
                var ret = x.engineSpecType.CompareTo(y.engineSpecType);
                if (ret == 0) ret = ((int)x.StageNo).CompareTo((int)y.StageNo);
                return ret;
            });
        }

        public SpecificationCollection()
        { }

        public List<double> Errors
        {
            get
            {
                List<double> err = new();
                foreach (var item in list)
                {
                    err.Add(item.error);
                }
                return err;
            }
        }

        public List<double> BaseErrors
        {
            get
            {
                List<double> err = new();
                foreach (var item in list)
                {
                    err.Add(item.baseerror);
                }
                return err;
            }
        }

        public List<double> Values
        {
            get
            {
                List<double> err = new();
                foreach (var item in list)
                {
                    err.Add(item.Value);
                }
                return err;
            }
        }

        public Specification GetLiquidDrawSpec(Guid sectionguid, Guid Stage)
        {
            foreach (Specification s in list)
                if (s.engineSectionGuid == sectionguid
                && s.IsLiquid
                && s.engineStageGuid == Stage
                && s.engineSpecType == eSpecType.LiquidProductDraw)
                    return s;
            return null;
        }

        public int IndexOf(Specification spec)
        {
            return list.IndexOf(spec);
        }

        public Specification GetVapourDrawSpec(Guid sectionguid, Guid Stage)
        {
            foreach (Specification s in list)
                if (s.engineSectionGuid == sectionguid
                    && !s.IsLiquid
                    && s.engineStageGuid == Stage
                    && s.engineSpecType == eSpecType.TrayNetVapFlow)
                    return s;
            return null;
        }

        public Specification GetRefluxRatioSpec(Guid section)
        {
            foreach (Specification s in list)
                if (s.engineSectionGuid == section
                   && s.IsLiquid
                   && s.engineSpecType == eSpecType.RefluxRatio) // reflux ratio check
                    return s;
            return null;
        }

        public Specification GetCondVapourRateSpec(Guid section)
        {
            foreach (Specification s in list)
                if (s.engineSectionGuid == section
                    && (s.engineSpecType == eSpecType.TrayNetVapFlow
                    || s.engineSpecType == eSpecType.TrayNetVapFlow))
                    return s;

            return null;
        }

        public Specification GetTopLiquidDrawSpec(IColumn column, Guid sectionguid)
        {
            foreach (Specification s in list)
                if (GetStageNoFromGuid(column, s.engineStageGuid, out int stageNo))
                    if (s.engineSectionGuid == sectionguid
                        && s.IsLiquid && stageNo == 0
                        && s.engineSpecType == eSpecType.TrayNetLiqFlow)
                        return s;

            return null;
        }

        public static bool GetStageNoFromGuid(IColumn column, Guid trayguid, out Int32 No)
        {
            Tray tray;
            foreach (TraySection section in column.TraySections)
            {
                tray = section.GetTray(trayguid);
                if (tray != null)
                {
                    No = section.Trays.IndexOf(tray);
                    return true;
                }
            }
            No = 999;
            return false;
        }

        public Specification GetTrayLquidSpec(Guid sectionguid, Guid TrayGuid)
        {
            foreach (Specification s in list)
                if (s.engineSpecType == eSpecType.TrayNetLiqFlow &&
                    s.engineSectionGuid == sectionguid &&
                    s.IsActive &&
                    s.engineStageGuid == TrayGuid)
                    return s;

            return null;
        }

        public Specification GetPAEnergySpec(Guid PAGuid)
        {
            foreach (Specification s in list)
            {
                if (s.PAguid == PAGuid)
                {
                    switch (s.engineSpecType)
                    {
                        case eSpecType.PADeltaT:
                        case eSpecType.PADuty:
                        case eSpecType.PARetT:
                            return s;
                    }
                }
            }
            return null;
        }

        IEnumerator<Specification> IEnumerable<Specification>.GetEnumerator()
        {
            return ((IEnumerable<Specification>)list).GetEnumerator();
        }

        public void SetTrays(Guid drawtray, Guid returntray)
        {
            foreach (Specification s in list)
            {
                s.PADrawTray = drawtray;
                s.PAReturnTray = returntray;
            }
        }

        public void AddRange(object specs)
        {
            if (specs != null)
                AddRange(specs);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<Specification>)list).GetEnumerator();
        }

        public Specification this[Guid index]
        {
            get
            {
                foreach (var item in list)
                {
                    if (item.Guid == index)
                        return item;
                }
                return null;
            }
        }

        public Specification this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public Specification this[string name]
        {
            get
            {
                foreach (var item in list)
                {
                    if (item.SpecName == name)
                        return item;
                }
                return null;
            }
        }

        public int Count()
        {
            return list.Count;
        }

        public List<Specification> List
        {
            get { return list; }
            set { list = value; }
        }

        public void Add(Specification t)
        {
            list.Add(t);
        }

        public void RemoveByObject(Guid guid)
        {
            foreach (Specification spec in list)
            {
                if (spec.drawObjectGuid == guid)
                    List.Remove(spec);
            }
        }

        public void RemoveBySpecGuid(Guid SpecGuid)
        {
            foreach (Specification spec in list)
            {
                if (spec.Guid == SpecGuid)
                {
                    List.Remove(spec);
                    break;
                }
            }
        }

        internal Specification GetCondTemperatureSpec(IColumn column, Guid sectionguid)
        {
            foreach (Specification s in list)
            {
                if (GetStageNoFromGuid(column, s.engineStageGuid, out int stageNo))
                    if (s.engineSectionGuid == sectionguid && stageNo == 0 && s.engineSpecType == eSpecType.Temperature)
                        return s;
            }
            return null;
        }

        public SpecificationCollection GetSpecs(Column p)
        {
            SpecificationCollection spc = new();
            foreach (Specification sp in list)
                if (sp.engineObjectguid == p.Guid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollection GetSpecs(PumpAround p)
        {
            SpecificationCollection spc = new();
            foreach (Specification sp in list)
                if (sp.PAguid == p.Guid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollection GetSpecs(SideStream sidestream)
        {
            SpecificationCollection spc = new();
            foreach (Specification sp in list)
                if (sp.sideStreamGuid == sidestream.Guid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollection GetActivePASpecs(Guid PAGuid)
        {
            SpecificationCollection spc = new();
            foreach (Specification sp in list)
                if (sp.IsActive && sp.PAguid == PAGuid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollection GetActiveSpecs()
        {
            SpecificationCollection spc = new();
            foreach (Specification sp in list)
                if (sp.IsActive)
                    spc.Add(sp);
            return spc;
        }

        public int GetActiveSpecsCount(bool inludeheatbalancespecs)
        {
            SpecificationCollection spc = new();
            foreach (Specification sp in list)
                if (sp.IsActive)
                    if (!(!inludeheatbalancespecs && sp.engineSpecType == eSpecType.TrayDuty))
                        spc.Add(sp);

            return spc.Count();
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        internal void Clear()
        {
            list.Clear();
        }

        /*   public  SpecificationCollection(SerializationInfo info, StreamingContext context)
           {
               try
               {
                   list = (List<Specification>)info.GetValue("List", typeof(List<Specification>));
               }
               catch
               {
               }
           }*/

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("List", list);
        }

        public int DegreesOfFreedom
        {
            get
            {
                return degreesoffreedom;
            }
            set
            {
                degreesoffreedom = value;
            }
        }
    }
}