using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngine
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SideStream : ISerializable
    {
        public Guid Guid { get; set; }

        public SideStream(string name, TraySection traysection, Tray tray)
        {
            EngineDrawSection = traysection;
            EngineDrawTray = tray;
            Name = name;
            Guid = Guid.NewGuid();
        }

        public SideStream()
        {
            Guid = Guid.NewGuid();
        }

        public SideStream(SerializationInfo info, StreamingContext context)
        {
            try
            {
                Name = info.GetString("Name");
                Guid = (Guid)info.GetValue("Guid", typeof(Guid));
            }
            catch
            {
            }
            if (Guid == Guid.Empty)
                Guid = Guid.NewGuid();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Guid", Guid);
        }

        internal SideStream CloneDeep(Column col)
        {
            SideStream ss = new();
            ss.EngineDrawSection = EngineDrawSection.CloneDeep(col);
            ss.EngineDrawTray = EngineDrawTray.CloneDeep(col);
            ss.Name = Name;
            ss.Guid = this.Guid;

            return ss;
        }

        internal SideStream Clone()
        {
            SideStream ss = new();
            ss.EngineDrawSection = EngineDrawSection.Clone();
            ss.EngineDrawTray = EngineDrawTray.Clone();
            ss.Name = Name;
            return ss;
        }

        public Tray EngineDrawTray;

        public string Name { get; set; }

        public TraySection EngineDrawSection;
        public double DrawFactor { get; set; }
        public StreamProperty Flow;

        public double MW
        {
            get
            {
                if (EngineDrawTray != null)
                    return EngineDrawTray.liquidDrawRight.cc.MW();
                else
                    return double.NaN;
            }
        }

        public double SG
        {
            get
            {
                if (EngineDrawTray != null)
                    return EngineDrawTray.liquidDrawRight.cc.SG();
                else
                    return double.NaN;
            }
        }

        public MoleFlow MoleFlow
        {
            get
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            return Flow.BaseValue;

                        case ePropID.MF:
                            return Flow.BaseValue / MW;

                        case ePropID.VF:
                            return Flow.BaseValue * SG / MW;
                    }
                return double.NaN;
            }
            set
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            Flow.BaseValue = value;
                            break;

                        case ePropID.MF:
                            Flow.BaseValue = value * MW;
                            break;

                        case ePropID.VF:
                            Flow.BaseValue = value * MW / SG;
                            break;
                    }
            }
        }

        public MassFlow MassFlow
        {
            get
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            return Flow.BaseValue / MW;

                        case ePropID.MF:
                            return Flow.BaseValue;

                        case ePropID.VF:
                            return Flow.BaseValue * SG;
                    }
                return double.NaN;
            }
            set
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            Flow.BaseValue = value / MW;
                            break;

                        case ePropID.MF:
                            Flow.BaseValue = value;
                            break;

                        case ePropID.VF:
                            Flow.BaseValue = value * SG;
                            break;
                    }
            }
        }

        public VolumeFlow VolFlow
        {
            get
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            return Flow.BaseValue * MW / SG;

                        case ePropID.MF:
                            return Flow.BaseValue / SG;

                        case ePropID.VF:
                            return Flow.BaseValue;
                    }
                return double.NaN;
            }
            set
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            Flow.BaseValue = value * SG / MW;
                            break;

                        case ePropID.MF:
                            Flow.BaseValue = value * SG;
                            break;

                        case ePropID.VF:
                            Flow.BaseValue = value;
                            break;
                    }
            }
        }

        public bool IsActive { get; set; }
    }
}