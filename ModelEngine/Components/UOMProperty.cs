using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class UOMProperty : BaseObject, ISerializable
    {
        private IUOM uom;
        private string displayunit;
        private string displayName = "";
        public double estimate;
        public string DisplayName { get => displayName; set => displayName = value; }
        public IUOM UOM { get => uom; set => uom = value; }
        public override SourceEnum Source { get => origin; set => origin = value; }

        public UOMProperty()
        {
        }

        public delegate void PropertyChangedEventHandler(object sender, EventArgs e);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChangedEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            PropertyChanged?.Invoke(this, new EventArgs());
        }

        public UOMProperty(double value, string name = "", SourceEnum origin = SourceEnum.Default)
        {
            uom = UOMUtility.GetUOM(ePropID.Value);
            uom.BaseValue = value;
            this.origin = origin;
            displayunit = uom.DefaultUnit;
            this.Name = name;
        }

        public UOMProperty(UOMProperty prop)
        {
            uom = prop.uom;
        }

        public UOMProperty(ePropID id)
        {
            uom = Units.UOMUtility.GetUOM(id);
            origin = SourceEnum.Empty;
            displayunit = uom.DefaultUnit;
        }

        public UOMProperty(IUOM UOM)
        {
            uom = UOM;
            origin = SourceEnum.Empty;
            displayunit = uom.DefaultUnit;
        }

        public UOMProperty(ePropID id, double val, SourceEnum origin = default)
        {
            uom = UOMUtility.GetUOM(id);
            if (uom != null)
            {
                uom.BaseValue = val;
                displayunit = uom.DefaultUnit;
                this.origin = origin;
            }
        }

        public UOMProperty(ePropID id, double val, string Unit)
        {
            uom = UOMUtility.GetUOM(id);
            if (uom != null)
            {
                uom.ValueIn(Unit, val);
                displayunit = Unit;
            }
        }

        public UOMProperty(ePropID id, SourceEnum source, double val)
        {
            uom = UOMUtility.GetUOM(id);
            if (uom != null)
            {
                UOM.BaseValue = val;
                displayunit = uom.DefaultUnit;
            }
            this.origin = source;
        }

        public UOMProperty(ePropID id, SourceEnum source, double val, string name = "")
        {
            uom = UOMUtility.GetUOM(id);
            if (uom != null)
            {
                UOM.BaseValue = val;
                displayunit = uom.DefaultUnit;
            }
            this.origin = source;
            this.Name = name;
            this.displayName = name;
        }

        public UOMProperty(ePropID id, SourceEnum source, string name)
        {
            uom = UOMUtility.GetUOM(id);
            if (uom != null)
            {
                UOM.BaseValue = double.NaN;
                displayunit = uom.DefaultUnit;
            }
            this.origin = source;
            this.displayName = name;
        }

        public UOMProperty(IUOM prop, SourceEnum source, string name)
        {
            uom = prop;
            if (uom != null)
                displayunit = uom.DefaultUnit;

            this.origin = source;
            this.Name = name;
        }

        public override double Value
        {
            get
            {
                if (uom is null)
                    return double.NaN;
                return uom.BaseValue;
            }
            set
            {
                if (uom != null)
                {
                    uom.BaseValue = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public bool IsFlashVariable
        {
            get
            {
                if (!uom.IsKnown)
                    return false;
                switch (origin)
                {
                    case SourceEnum.Input:
                    case SourceEnum.Transferred:
                    case SourceEnum.UnitOpCalcResult:
                    case SourceEnum.SignalTransfer:
                        return true;

                    default:
                        return false;
                }
            }
        }

        protected UOMProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            try
            {
                ePropID temporrarypropid = (ePropID)info.GetValue("temporrarypropid", typeof(ePropID));
                UOM = UOMUtility.GetUOM(temporrarypropid);
                BaseValue = info.GetDouble("BaseValue");
                displayunit = info.GetString("displayunit");
                displayName = info.GetString("displayname");
                OriginPortGuid = (Guid)info.GetValue("OriginGuid", typeof(Guid));
            }
            catch
            {
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("displayunit", displayunit);
            info.AddValue("BaseValue", BaseValue);
            info.AddValue("temporrarypropid", Propid);
            info.AddValue("displayname", displayName);
            info.AddValue("OriginGuid", OriginPortGuid);
        }

        public ePropID Propid
        {
            get
            {
                if (uom != null)
                    return uom.propid;
                else
                    return ePropID.NullUnits;
            }
        }

        public override Guid Guid { get => guid; set => guid = value; }
        public override Guid OriginPortGuid { get => _originPortGuid; set => _originPortGuid = value; }

        public override string Name { get => name; }

        public static implicit operator double(UOMProperty a)
        {
            if (a is null || a.uom is null)
                return double.NaN;
            return a.UOM.BaseValue;
        }

        public void Clear()
        {
            if (uom is null)
                return;
            uom.EraseValue();
            origin = SourceEnum.Empty;
            OriginPortGuid = Guid.Empty;
            RaisePropertyChangedEvent();
        }

        public ePropID Property
        {
            get
            {
                return uom.propid;
            }
            set
            {
                uom = UOMUtility.GetUOM(value);
            }
        }

        public double BaseValue
        {
            get
            {
                if (uom is null)
                    return double.NaN;
                else
                    return uom.BaseValue;
            }
            set
            {
                if (uom != null)
                {
                    uom.BaseValue = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public string DefaultUnit
        {
            get
            {
                if (uom != null)
                    return uom.DefaultUnit;
                else
                    return null;
            }
            set
            {
                // iUOM.DefaultUnit = value;
            }
        }

        public string[] AllUnits
        {
            get
            {
                return uom.AllUnits;
            }
        }

        public bool IsDirty { get; set; }

        public bool IsKnown
        {
            get
            {
                return uom.IsKnown;
            }
        }

        public bool IsFixed
        {
            get
            {
                return origin == SourceEnum.Input;
            }
        }

        public string DisplayUnit
        {
            get
            {
                if (displayunit is null)
                {
                    displayunit = DefaultUnit;
                    return DefaultUnit;
                }
                return displayunit;
            }
            set
            {
                displayunit = value;
            }
        }

        public string UnitDescriptor
        {
            get
            {
                if (uom != null)
                    return uom.UnitDescriptor(displayunit);
                else
                    return null;
            }
        }

        public bool IsInput => origin == SourceEnum.Input;

        public void ToUnits(string v)
        {
            if (uom != null)
                uom.ValueOut(v);
        }

        public override string ToString()
        {
            return BaseValue.ToString();
        }

        public double DisplayValueIn(double val)
        {
            if (uom != null)
            {
                uom.ValueIn(DisplayUnit, val);
                return uom.BaseValue;
            }
            else
                return double.NaN;
        }

        public double DisplayValueOut()
        {
            if (displayunit is null)
                return BaseValue;

            if (uom is null)
                return double.NaN;

            return uom.ValueOut(displayunit);
        }

        public double DisplayValueOut(double basevalue)
        {
            BaseValue = basevalue;

            if (displayunit is null)
                return BaseValue;

            if (uom is null)
                return double.NaN;
            return uom.ValueOut(displayunit);
        }

        public double ValueIn(string unit, double v)
        {
            if (unit is null)
            {
                uom.BaseValue = v;
                return BaseValue;
            }
            else
            {
                uom.ValueIn(unit, v);
                return BaseValue;
            }
        }

        public double ValueIn(double v)
        {
            if (displayunit is null)
            {
                uom.BaseValue = v;
                return BaseValue;
            }
            else
            {
                uom.ValueIn(displayunit, v);
                return BaseValue;
            }
        }

        public virtual UOMProperty Clone()
        {
            UOMProperty bp = new();
            bp.uom = this.uom;
            bp.OriginPortGuid = this.OriginPortGuid;
            bp.guid = this.guid;
            bp.origin = this.origin;
            return bp;
        }

        /* public static implicit operator UOMProperty(Temperature t)
         {
             UOMProperty uom = new UOMProperty(ePropID.T, t.BaseValue);
             return uom;
         }

         public static implicit operator UOMProperty(Density d)
         {
             UOMProperty uom = new UOMProperty(ePropID.Density, d.BaseValue);
             return uom;
         }*/
    }
}