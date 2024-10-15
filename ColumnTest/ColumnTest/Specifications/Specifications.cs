using ModelEngine;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngineTest
{
    [TypeConverter(typeof(SpecificationTest)), Description("Expand to see value and units")]
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
    public class SpecificationTest : IEquatable<SpecificationTest>, ISerializable
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
        public COMeSpecType graphicSpecType = COMeSpecType.TrayNetLiqFlow;
        public COMeSpecType engineSpecType = COMeSpecType.TrayNetLiqFlow;

        private Guid enginestageGuid;
        private Guid enginesectionGuid;

        public Guid engineObjectguid;
        public Guid sideStreamGuid;

        public Guid drawObjectGuid;
        public Guid drawStreamGuid;
        public Guid drawSectionGuid;

        public TraySectionTest traysection;
        public TrayTest tray;

        private Guid pADrawTray, pAReturnTray;

        public int stageNo = 999;
        internal double error;
        internal double baseerror;
        public double Gradient = 0;

        public override string ToString()
        {
            return this.name;
        }

        public static ePropID Spec_ePropID(COMeSpecType t)
        {
            switch (t)
            {
                case COMeSpecType.LiquidProductDraw:
                case COMeSpecType.VapProductDraw:
                case COMeSpecType.TrayNetLiqFlow:
                case COMeSpecType.TrayNetVapFlow:
                case COMeSpecType.PAFlow:
                    return ePropID.MOLEF;

                case COMeSpecType.Energy:
                case COMeSpecType.PADuty:
                    return ePropID.EnergyFlow;

                case COMeSpecType.PADeltaT:
                    return ePropID.DeltaT;

                case COMeSpecType.PARetT:
                case COMeSpecType.Temperature:
                case COMeSpecType.DistSpec:
                    return ePropID.T;

                case COMeSpecType.RefluxRatio:
                    return ePropID.NullUnits;

                default:
                    return ePropID.NullUnits;
            }
        }

        public UOMProperty SpecUOMProperty()
        {
            switch (engineSpecType)
            {
                case COMeSpecType.LiquidProductDraw:
                case COMeSpecType.VapProductDraw:
                case COMeSpecType.TrayNetLiqFlow:
                case COMeSpecType.TrayNetVapFlow:
                case COMeSpecType.PAFlow:
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

                case COMeSpecType.Energy:
                case COMeSpecType.PADuty:
                    return new UOMProperty(ePropID.EnergyFlow, specvalue);

                case COMeSpecType.PADeltaT:
                case COMeSpecType.PARetT:
                case COMeSpecType.Temperature:
                    return new UOMProperty(ePropID.T, specvalue);

                case COMeSpecType.RefluxRatio:
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

        public SpecificationTest(SerializationInfo info, StreamingContext context)
        {
            try
            {
                name = info.GetString("name");
                value = info.GetDouble("value1");
                specvalue = info.GetDouble("specvalue1");
                graphicSpecType = (COMeSpecType)info.GetValue("specType", typeof(COMeSpecType));
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
                engineSpecType = (COMeSpecType)info.GetValue("enginspecType", typeof(COMeSpecType));
                FlowType = (COMenumflowtype)info.GetValue("FlowType", typeof(COMenumflowtype));
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

        public SpecificationTest()
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
                case COMenumflowtype.Molar:
                    return specvalue;

                case COMenumflowtype.Mass:
                    return specvalue / MW;

                case COMenumflowtype.StdLiqVol:
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
                case COMeSpecType.RefluxRatio:
                    UOM = UOMUtility.GetUOM(ePropID.NullUnits);
                    break;

                case COMeSpecType.Energy:
                case COMeSpecType.PADuty:
                    UOM = UOMUtility.GetUOM(ePropID.EnergyFlow);
                    break;

                case COMeSpecType.DistSpec:
                case COMeSpecType.Temperature:
                case COMeSpecType.PARetT:
                    UOM = UOMUtility.GetUOM(ePropID.T);
                    //unitConvert.DisplayUnit = "C";
                    break;

                case COMeSpecType.PADeltaT:
                    UOM = UOMUtility.GetUOM(ePropID.DeltaT);
                    //unitConvert.DisplayUnit = "C";
                    break;

                case COMeSpecType.VapProductDraw:
                case COMeSpecType.LiquidProductDraw:
                case COMeSpecType.PAFlow:
                case COMeSpecType.TrayNetLiqFlow:
                case COMeSpecType.TrayNetVapFlow:
                    switch (FlowType)
                    {
                        case COMenumflowtype.Molar:
                            UOM = UOMUtility.GetUOM(ePropID.MOLEF);
                            break;

                        case COMenumflowtype.Mass:
                            UOM = UOMUtility.GetUOM(ePropID.MF);
                            break;

                        case COMenumflowtype.StdLiqVol:
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
                    case COMeSpecType.LiquidProductDraw:
                    case COMeSpecType.TrayNetLiqFlow:
                    case COMeSpecType.PAFlow:
                    case COMeSpecType.RefluxRatio:
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
                    case COMeSpecType.LiquidProductDraw:
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
                    case COMeSpecType.PADuty:
                    case COMeSpecType.PAFlow:
                    case COMeSpecType.PARetT:
                    case COMeSpecType.PADeltaT:
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
        public COMenumflowtype FlowType { get; set; } = COMenumflowtype.Molar;

        public ConnectingStream connectedStream = null;
        public enumDistType distType;
        public enumDistPoints distpoint;
        internal double delta;
        private Guid paguid;

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, SideStream ss, enumDistType distype, enumDistPoints distpoint, bool IsActive = true, bool baseunits = true)
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
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, TraySection ts, bool IsActive = true, bool baseunits = true)
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
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, TrayTest t, bool IsActive = true, bool baseunits = true)
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
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, COMColumn column, bool IsActive = true, bool baseunits = true)
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
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, SideStream ss, bool IsActive = true, bool baseunits = true)
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
                    FlowType = COMenumflowtype.Molar;
                    break;

                case ePropID.MF:
                    FlowType = COMenumflowtype.Mass;
                    break;

                case ePropID.VF:
                    FlowType = COMenumflowtype.StdLiqVol;
                    break;

                default:
                    FlowType = COMenumflowtype.None;
                    break;
            }
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, bool IsActive = true, bool baseunits = true)
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
                    FlowType = COMenumflowtype.Molar;
                    break;

                case ePropID.MF:
                    FlowType = COMenumflowtype.Mass;
                    break;

                case ePropID.VF:
                    FlowType = COMenumflowtype.StdLiqVol;
                    break;

                default:
                    FlowType = COMenumflowtype.None;
                    break;
            }
            // this.engineobject guid = engineobject guid;

            switch (spectype)
            {
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }


        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section,
            TrayTest tray, ConnectingStream cs, bool IsActive = true)
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
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = guid;
                    break;
            }
        }

        public SpecificationTest(string name, double specValue, ePropID property, COMeSpecType spectype, TraySectionTest section, TrayTest tray,
            PumpAroundTest pa, bool IsActive = true)
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
                case COMeSpecType.PAFlow:
                case COMeSpecType.PARetT:
                case COMeSpecType.PADeltaT:
                case COMeSpecType.PADuty:
                    PAguid = pa.Guid;
                    break;
            }
        }

        public SpecificationTest(string name, int value, ePropID nullUnits, COMeSpecType lLE_KSpec, TraySectionTest section, TrayTest tray)
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

        public bool Equals(SpecificationTest obj)
        {
            return Equals(obj as SpecificationTest);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), Serializable]
    public class SpecificationCollectionTest : IEnumerable<SpecificationTest>
    {
        private List<SpecificationTest> list = new();
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

        public SpecificationCollectionTest()
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

        public SpecificationTest GetLiquidDrawSpec(Guid sectionguid, Guid Stage)
        {
            foreach (SpecificationTest s in list)
                if (s.engineSectionGuid == sectionguid
                && s.IsLiquid
                && s.engineStageGuid == Stage
                && s.engineSpecType == COMeSpecType.LiquidProductDraw)
                    return s;
            return null;
        }

        public int IndexOf(SpecificationTest spec)
        {
            return list.IndexOf(spec);
        }

        public SpecificationTest GetVapourDrawSpec(Guid sectionguid, Guid Stage)
        {
            foreach (SpecificationTest s in list)
                if (s.engineSectionGuid == sectionguid
                    && !s.IsLiquid
                    && s.engineStageGuid == Stage
                    && s.engineSpecType == COMeSpecType.TrayNetVapFlow)
                    return s;
            return null;
        }

        public SpecificationTest GetRefluxRatioSpec(Guid section)
        {
            foreach (SpecificationTest s in list)
                if (s.engineSectionGuid == section
                   && s.IsLiquid
                   && s.engineSpecType == COMeSpecType.RefluxRatio) // reflux ratio check
                    return s;
            return null;
        }

        public SpecificationTest GetCondVapourRateSpec(Guid section)
        {
            foreach (SpecificationTest s in list)
                if (s.engineSectionGuid == section
                    && (s.engineSpecType == COMeSpecType.TrayNetVapFlow
                    || s.engineSpecType == COMeSpecType.TrayNetVapFlow))
                    return s;

            return null;
        }

        public SpecificationTest GetTopLiquidDrawSpec(IColumn column, Guid sectionguid)
        {
            foreach (SpecificationTest s in list)
                if (GetStageNoFromGuid(column, s.engineStageGuid, out int stageNo))
                    if (s.engineSectionGuid == sectionguid
                        && s.IsLiquid && stageNo == 0
                        && s.engineSpecType == COMeSpecType.TrayNetLiqFlow)
                        return s;

            return null;
        }

        public static bool GetStageNoFromGuid(IColumn column, Guid trayguid, out Int32 No)
        {
            TrayTest tray;
            foreach (TraySectionTest section in column.TraySections)
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

        public SpecificationTest GetTrayLquidSpec(Guid sectionguid, Guid TrayGuid)
        {
            foreach (SpecificationTest s in list)
                if (s.engineSpecType == COMeSpecType.TrayNetLiqFlow &&
                    s.engineSectionGuid == sectionguid &&
                    s.IsActive &&
                    s.engineStageGuid == TrayGuid)
                    return s;

            return null;
        }

        public SpecificationTest GetPAEnergySpec(Guid PAGuid)
        {
            foreach (SpecificationTest s in list)
            {
                if (s.PAguid == PAGuid)
                {
                    switch (s.engineSpecType)
                    {
                        case COMeSpecType.PADeltaT:
                        case COMeSpecType.PADuty:
                        case COMeSpecType.PARetT:
                            return s;
                    }
                }
            }
            return null;
        }

        IEnumerator<SpecificationTest> IEnumerable<SpecificationTest>.GetEnumerator()
        {
            return ((IEnumerable<SpecificationTest>)list).GetEnumerator();
        }

        public void SetTrays(Guid drawtray, Guid returntray)
        {
            foreach (SpecificationTest s in list)
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
            return ((IEnumerable<SpecificationTest>)list).GetEnumerator();
        }

        public SpecificationTest this[Guid index]
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

        public SpecificationTest this[int index]
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

        public SpecificationTest this[string name]
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

        public List<SpecificationTest> List
        {
            get { return list; }
            set { list = value; }
        }

        public void Add(SpecificationTest t)
        {
            list.Add(t);
        }

        public void RemoveByObject(Guid guid)
        {
            foreach (SpecificationTest spec in list)
            {
                if (spec.drawObjectGuid == guid)
                    List.Remove(spec);
            }
        }

        public void RemoveBySpecGuid(Guid SpecGuid)
        {
            foreach (SpecificationTest spec in list)
            {
                if (spec.Guid == SpecGuid)
                {
                    List.Remove(spec);
                    break;
                }
            }
        }

        internal SpecificationTest GetCondTemperatureSpec(IColumn column, Guid sectionguid)
        {
            foreach (SpecificationTest s in list)
            {
                if (GetStageNoFromGuid(column, s.engineStageGuid, out int stageNo))
                    if (s.engineSectionGuid == sectionguid && stageNo == 0 && s.engineSpecType == COMeSpecType.Temperature)
                        return s;
            }
            return null;
        }

        public SpecificationCollectionTest GetSpecs(COMColumn p)
        {
            SpecificationCollectionTest spc = new();
            foreach (SpecificationTest sp in list)
                if (sp.engineObjectguid == p.Guid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollectionTest GetSpecs(PumpAroundTest p)
        {
            SpecificationCollectionTest spc = new();
            foreach (SpecificationTest sp in list)
                if (sp.PAguid == p.Guid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollectionTest GetSpecs(SideStreamTest sidestream)
        {
            SpecificationCollectionTest spc = new();
            foreach (SpecificationTest sp in list)
                if (sp.sideStreamGuid == sidestream.Guid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollectionTest GetActivePASpecs(Guid PAGuid)
        {
            SpecificationCollectionTest spc = new();
            foreach (SpecificationTest sp in list)
                if (sp.IsActive && sp.PAguid == PAGuid)
                    spc.Add(sp);
            return spc;
        }

        public SpecificationCollectionTest GetActiveSpecs()
        {
            SpecificationCollectionTest spc = new();
            foreach (SpecificationTest sp in list)
                if (sp.IsActive)
                    spc.Add(sp);
            return spc;
        }

        public int GetActiveSpecsCount(bool inludeheatbalancespecs)
        {
            SpecificationCollectionTest spc = new();
            foreach (SpecificationTest sp in list)
                if (sp.IsActive)
                    if (!(!inludeheatbalancespecs && sp.engineSpecType == COMeSpecType.TrayDuty))
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